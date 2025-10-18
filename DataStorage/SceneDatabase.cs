using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This database handle the read and write of the scene data
/// </summary>
public class SceneDatabase : MonoBehaviour
{
    public static SceneDatabase Instance;

    [Header("SceneData")]
    BeachSceneData beachSceneData;
    ShelterSceneData shelterSceneData;
    private void Awake()
    {
        Instance = this;
    }

    #region BeachScene
    public void StoreBeachSceneData()
    {
        beachSceneData = ResourceManager.Instance.StoreBeachSceneData();
        Debug.Log("Store SceneData success");
    }

    public BeachSceneData GetBeachSceneData()
    {
        return beachSceneData;
    }
    #endregion

    #region ShelterScene
    public void StoreShelterSceneData()
    {
        shelterSceneData = ShelterSceneManager.Instance.StoreInventoryData();
    }

    public ShelterSceneData GetShelterData()
    {
        return shelterSceneData;
    }
    #endregion
}
