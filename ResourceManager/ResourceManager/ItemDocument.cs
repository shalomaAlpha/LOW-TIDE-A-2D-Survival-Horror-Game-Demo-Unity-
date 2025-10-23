using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDocument : MonoBehaviour
{
    Document document;
    [Header("Details")]
    public string owner;
    public int index;
    public string form;

    public GameObject Hint;
    bool allowToPick;

    void Start()
    {
        Hint.SetActive(false);
        if (DocumentPickedOrNot())
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            document = new Document(owner, index, form);
        }
    }
    
    void Update()
    {
        if(allowToPick&&Input.GetKeyUp(KeyCode.E))
        {
            SFXManager.Instance.PlaySFX("SFX_Pickup");
            AddDocumentToRecord();
            TaskUI.Instance.DisplayNewDocumentFound();
            Destroy(gameObject);
        }
    }

    #region trigger enter and exit
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            allowToPick = true;
            Hint.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            allowToPick=false;
            Hint.SetActive(false);
        }
    }
    #endregion

    private bool DocumentPickedOrNot()
    {
        bool result=false;
        switch (owner) 
        {
            case "Fisherman":
                result = index <= PlayerBehaviourTracking.Instance.fishmenDocuments.Count;
                break;
            case "Sergeant":
                result = index <= PlayerBehaviourTracking.Instance.sergeantDocuments.Count;
                break;
            case "Researcher":
                result = index <= PlayerBehaviourTracking.Instance.researcherDocuments.Count;
                break;
            case "Other":
                result = index <= PlayerBehaviourTracking.Instance.otherDocuments.Count;
                break;
        }
        return result;
    }

    private void AddDocumentToRecord()
    {
        switch (owner)
        {
            case "Fisherman":
                PlayerBehaviourTracking.Instance.fishmenDocuments.Add(document);
                break;
            case "Sergeant":
                PlayerBehaviourTracking.Instance.sergeantDocuments.Add(document);
                break;
            case "Researcher":
                PlayerBehaviourTracking.Instance.researcherDocuments.Add(document);
                break;
            case "Other":
                PlayerBehaviourTracking.Instance.otherDocuments.Add(document);
                break;
        }
    }
}
