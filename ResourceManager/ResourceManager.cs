using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Require: Placed in the Scene
/// Functionalities:
/// 1.Calculate the cloest resource near player
/// 2.Handle player loot resource
/// 3.Record resource Data in this scene
/// 4.Pacake and return resources data 
/// 5.Spawn resources according to record
/// </summary>
public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;
    public GameObject ResourcePrefab;
    public float detectionRadius=3f;

    List<Resource> resources = new List<Resource>();
    List<Vector3> resourceLocation = new List<Vector3>();

    GameObject player;
    Resource nearestResource;
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        player=PlayerController.Instance.gameObject;
        StartCoroutine(CheckForNearbyResource());
    }

    #region Player loot resoure
    void Update()
    {
        if (UIManager.Instance.AllowToDisplayOtherHint == false) return;
        if (nearestResource != null && Input.GetKeyDown(KeyCode.E)) 
        {
            nearestResource.lootReousrce(player);
            UIManager.Instance.ModifyGuideText("");
        }
        else if(nearestResource == null)
        {
            UIManager.Instance.ModifyGuideText("");
        }
    }
    #endregion

    #region Calculate the cloest resource near player
    private IEnumerator CheckForNearbyResource()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f); // check each 0.5s

            float minDistance = detectionRadius;
            Resource closest = null;

            foreach (var resource in resources)
            {
                float distance = Vector3.Distance(player.transform.position, resource.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = resource;
                }
            }
            //Debug.Log(resources.Count + "in total");
            if (nearestResource != closest)
            {
                if (nearestResource != null)
                {
                    nearestResource.SetPlayerNearby(false);
                }
                nearestResource = closest;
                

                if (nearestResource != null)
                {
                    nearestResource.SetPlayerNearby(true);
                }
                    
            }
        }
    }
    #endregion

    #region record resource data
    public void regisiterResourceInfo(Resource resource, Vector3 location)
    {
        //Debug.Log("Regisiter");
        resources.Add(resource);
        resourceLocation.Add(location);
    }

    public void removeResourceInfo(Resource resource, Vector3 location)
    {
        resources.Remove(resource);
        resourceLocation.Remove(location);
    }
    #endregion //store in the manager

    #region Pacake and return resources data
    public BeachSceneData StoreBeachSceneData()
    {
        BeachSceneData sceneData = new BeachSceneData();
        foreach (var resource in resources)
        {
            ResourceData resourceData=new ResourceData(resource.itemID,resource.itemAmount);
            sceneData.resourceData.Add(resourceData);
            Vector3 location=resource.gameObject.transform.position;
            sceneData.resourceLocation.Add(location);
        }
        return sceneData;
    }

    public void ClearList()
    {
        resources.Clear();
        resourceLocation.Clear();
    }
    #endregion

    #region spawn resource according to record
    public void SpawnRecordedResource(BeachSceneData data)
    {
        List<ResourceData> resourcesData = data.resourceData;
        List<Vector3>locations = data.resourceLocation;
        int amount=resources.Count;
        for(int i = 0; i < amount; i++)
        {
            GameObject newResource = Instantiate(ResourcePrefab, locations[i], Quaternion.identity);
            Resource resourceComponent= newResource.GetComponent<Resource>();

            resourceComponent.InitilizeResourceInManager(resourcesData[i]);
        }
    }
    #endregion
}



public class ResourceData
{
    public int itemID;
    public int amount;

    public ResourceData(int itemID, int amount)
    {
        this.itemID = itemID;
        this.amount = amount;
    }

    public int ReturnItemID()
    {
        return itemID;
    }
    public int ReturnAmount()
    {
        return amount;
    }
}
