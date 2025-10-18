using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Store all player behavior records
/// Can be used for event detection
/// </summary>
public class PlayerBehaviourTracking : MonoBehaviour
{
    public static PlayerBehaviourTracking Instance;

    [Header("SceneRelatived")]
    public bool FirsTimeEnterShelter;

    [Header("DocumantCollected")]
    public List<Document> fishmenDocuments = new List<Document>();
    public List<Document> sergeantDocuments = new List<Document>();
    public List<Document> researcherDocuments=new List<Document>();
    public List<Document> otherDocuments = new List<Document>();

    [Header("DialogueTrigged")]
    public bool InfrontShelter=false;
    public bool EnterShelter = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        /*
        Document document = new Document("Fisherman", 0, "Diaray");
        fishmenDocuments.Add(document);

        Document document1 = new Document("Fisherman", 1,"Diaray");
        fishmenDocuments.Add(document1);

        Document document2 = new Document("Sergeant", 0, "Deployment Log");
        fishmenDocuments.Add(document2);
        */
    }

}
