using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Manager the show and hide of the document element
/// </summary>
public class DocumentManager : MonoBehaviour
{
    public static DocumentManager Instance;
    public GameObject DocumentViewUI;
    public GameObject currentShowDocument;

    [Header("Document Gameobject")]
    public List<GameObject> fishmanDocuments=new List<GameObject>();
    public List<GameObject> sergeantDocuments = new List<GameObject>();
    public List<GameObject> researcherDocuments = new List<GameObject>();
    public List<GameObject> OtherDocuments = new List<GameObject>();

    public bool isViewing;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        DocumentViewUI.SetActive(false);
        HideAllCanvas();
    }

    private void Update()
    {
        if(isViewing&& (Input.GetKeyDown(KeyCode.P)|| Input.GetKeyDown(KeyCode.Escape))) 
        {
           OnBackPressed();
        }
    }
    public void HideAllCanvas()
    {
        isViewing = false;
        foreach(GameObject doc in fishmanDocuments)
        {
            doc.SetActive(false);
        }

        foreach (GameObject doc in sergeantDocuments)
        {
            doc.SetActive(false);
        }

        foreach (GameObject doc in researcherDocuments)
        {
            doc.SetActive(false);
        }

        foreach (GameObject doc in OtherDocuments)
        {
            doc.SetActive(false);
        }
    }

    public void ShowDocument(string owner, int index)
    {
        GameObject documentToShow;
        DocumentViewUI.SetActive(true);
        isViewing = true;
        switch (owner)
        {
            case "Fisherman":
                documentToShow = fishmanDocuments[index];
                break;
            case "Sergeant":
                documentToShow = sergeantDocuments[index];
                break;
            case "Researcher":
                documentToShow = researcherDocuments[index];
                break;
            default:
                documentToShow = OtherDocuments[index];
                break;
        }
        documentToShow.SetActive(true);
        currentShowDocument = documentToShow;
    }

    public void OnBackPressed()
    {
        isViewing = false;
        currentShowDocument.SetActive(false);
        DocumentViewUI.SetActive(false);
        PhoneUIManager.Instance.OpenPhoneUI();
        SFXManager.Instance.PlaySFX("PhoneClose");
    }

}
