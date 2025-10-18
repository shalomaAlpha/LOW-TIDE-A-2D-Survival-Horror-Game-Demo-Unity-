using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using TMPro;

public class PhoneUIManager : MonoBehaviour
{
    public static PhoneUIManager Instance;
    public GameObject PhoneUI;
    public GameObject EmptyHint;

    public bool isOpen;
    public TMP_Text pageText;

    [Header("Spawn Select icon")]
    public Transform content;
    public GameObject prefab;

    int currentPageIndex=0;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        PhoneUI.SetActive(false);
        EmptyHint.SetActive(false);
        isOpen = false;
    }

    void Update()
    {
        bool isView = DocumentManager.Instance.isViewing;
        if (Input.GetKeyDown(KeyCode.P)&& !isView)
        {
            if (!isOpen)
            {
                if(PlayerInventoryUI.Instance.isOpen)
                {
                    PlayerInventoryUI.Instance.isOpen = false;
                    ItemToolTip.Instance.SetVisibility(false);
                    GameManager.Instance.FreezePlayerMovementAndInteract(false);
                }
                OpenPhoneUI();
                RefreshUI();
            }
            else
            {
                ClearUI();
                ClosePhoneUI();
            }
                
        }
        bool isViewing = DocumentManager.Instance.isViewing;
        if (isOpen && (Input.GetKeyDown(KeyCode.Escape))&&isViewing)
        {
            ClosePhoneUI();
        }
    }

    public void OpenPhoneUI()
    {
        //GameManager.Instance.PauseGame();
        PhoneUI.SetActive(true);
        isOpen = true;
        
    }

    public void ClosePhoneUI()
    {
        //GameManager.Instance.ContinueGame();
        PhoneUI.SetActive(false);
        EmptyHint.SetActive(false);
        isOpen = false;
    }

    void ClearUI()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
    }

    public void RefreshUI()
    {
        ClearUI();
        List<Document> currentList=new List<Document>();
        switch (currentPageIndex)
        {
            case 0:
                currentList = PlayerBehaviourTracking.Instance.fishmenDocuments;
                break;
            case 1:
                currentList = PlayerBehaviourTracking.Instance.sergeantDocuments;
                break;
            case 2:
                currentList = PlayerBehaviourTracking.Instance.researcherDocuments;
                break;
            default:
                currentList = PlayerBehaviourTracking.Instance.otherDocuments;
                break;

        }
        if (currentList.Count == 0)
        {
            EmptyHint.SetActive(true);
        }
        else
        {
            EmptyHint.SetActive(false);
            foreach (Document document in currentList)
            {
                GameObject Selection = Instantiate(prefab, content);
                DocumentSelect documentSelect = Selection.GetComponent<DocumentSelect>();
                documentSelect.owner = document.owner;
                documentSelect.index = document.index;

                string textContent = document.owner + "-" + document.form + " " + document.index;
                documentSelect.setTextContent(textContent);
            }
        }
        pageText.text = "PAGE:" + (currentPageIndex+1) + "/4";
    }

    public void NextPage()
    {
        if(currentPageIndex<3)
        {
            currentPageIndex++;
        }
        else
        {
            currentPageIndex = 0;
        }
        RefreshUI();
        SFXManager.Instance.PlaySFX("SFX_ButtonPressed");
        Debug.Log("Current index" + currentPageIndex);

    }

    public void PrevPage()
    {
        if (currentPageIndex > 0)
        {
            currentPageIndex--;
        }
        else
        {
            currentPageIndex = 3;
        }
        RefreshUI();
        SFXManager.Instance.PlaySFX("SFX_ButtonPressed");
        Debug.Log("Current index" + currentPageIndex);
    }

}
