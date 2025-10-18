using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IconDatabase", menuName = "Inventory/ImageDatabase")]
public class IconDatabase : ScriptableObject
{
    [System.Serializable]
    public class ImageEntry
    {
        public int itemID;     
        public Sprite icon;      
        public string Name;
    }

    public List<ImageEntry> images;

    public Sprite GetIcon(int id)
    {
        var entry = images.Find(e => e.itemID == id);
        if (entry != null) return entry.icon;
        else
        {
            entry = images.Find(e => e.itemID == 0);
            return entry.icon;
        }
    }
}
