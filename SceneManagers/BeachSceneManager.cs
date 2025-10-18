using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 1.Handle the player spawn point in Scene Beach
/// 2.teleport player to specific position accoring to spawnPointID
/// 3.Initilize containers in the scene after transform 
/// </summary>
public class BeachSceneManager : MonoBehaviour
{
    [Header("Transfer Player")]
    public static BeachSceneManager Instance;
    public Transform SpawnPoint1;
    public Transform SpawnPoint2;
    public int SpawnPointID;
    private GameObject player;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        GameManager.Instance.ContinueGame();
        TransformPlayer();
    }

    public void TransformPlayer()
    {
        player = PlayerController.Instance.gameObject;
        switch (SpawnPointID)
        {
            case 1:
                //Debug.Log("Transfer player to point 1");
                player.transform.position = SpawnPoint1.position;
                break;
            case 2:
                //Debug.Log("Transfer player to point 2");
                player.transform.position = SpawnPoint2.position;
                break;
            default:
                break;
        }
    }

    //It will only be called when scenes other than the first enter the beach scene
    //be called in the call back function in SceneTransormer 
    /*public void SetSceneData()  
    {
        foreach (var container in FindObjectsOfType<Container>())
        {
            Destroy(container.gameObject);
        }

        if(SceneDatabase.Instance!=null)
        {
            GameObject containerPrefab = ContainerManager.Instance.ContainerPrefabs;
            BeachSceneData sceneData=SceneDatabase.Instance.GetBeachSceneData();

            foreach (var containerData in sceneData.containersData)
            {
                GameObject newContainer = Instantiate(containerPrefab); 
                newContainer.transform.position = containerData.position;

                Container container = newContainer.GetComponent<Container>();
                container.uniqueID = containerData.uniqueID;
                container.containerType = 
                    (ContainerType)System.Enum.Parse(typeof(ContainerType), containerData.ContainerType);

                // Add item into the container
                for (int i = 0; i < containerData.ItemID.Length; i++)
                {
                    var item = ItemDatabase.Instance.GetItemById(containerData.ItemID[i]);
                    container.AddItemToContainer(item, containerData.amount[i]);
                }
            }
        }

        
    }*/



}
