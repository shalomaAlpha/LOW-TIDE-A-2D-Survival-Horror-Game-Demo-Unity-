using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OfficeOpenXml;
using System.IO;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase Instance { get; private set; } // Ensure only one instance

    private string filePath;
    private Dictionary<int, ItemData> itemDictionary = new Dictionary<int, ItemData>(); // Use Dictionary to store item

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        filePath = Application.dataPath + "/Resources/ItemStaticData.xlsx"; 
        LoadItemsFromExcel();
    }
    private void LoadItemsFromExcel()
    {
        FileInfo fileInfo = new FileInfo(filePath);
        if (!fileInfo.Exists)
        {
            Debug.LogError("Excel file not found: " + filePath);
            return;
        }

        using (ExcelPackage excel = new ExcelPackage(fileInfo))
        {
            ExcelWorkbook workbook = excel.Workbook;
            if (workbook.Worksheets.Count == 0)
            {
                Debug.LogError("No worksheets found in Excel file.");
                return;
            }

            ExcelWorksheet sheet = workbook.Worksheets[1]; // Read the first sheet
            int rowCount = sheet.Dimension.Rows; // Get the row numbers

            for (int row = 2; row <= rowCount; row++) // Start from the second row
            {
                int id = int.Parse(sheet.Cells[row, 1].Text);
                string name = sheet.Cells[row, 2].Text;
                string description = sheet.Cells[row, 3].Text;
                int maxStack = int.Parse(sheet.Cells[row, 4].Text);
                ItemType type = (ItemType)System.Enum.Parse(typeof(ItemType), sheet.Cells[row, 5].Text);

                ItemData item = new ItemData(id, name, description, maxStack, type);
                itemDictionary[id] = item;
            }

            ExcelWorksheet weaponSheet = workbook.Worksheets[2]; // Read Second sheet 
            int weaponRowCount = weaponSheet.Dimension.Rows;

            for (int row = 2; row <= weaponRowCount; row++)
            {
                int id = int.Parse(weaponSheet.Cells[row, 1].Text);
                if (!itemDictionary.ContainsKey(id)) continue; 

                int damage = int.Parse(weaponSheet.Cells[row, 3].Text);
                float windUp = float.Parse(weaponSheet.Cells[row, 4].Text);
                float recovery = float.Parse(weaponSheet.Cells[row, 5].Text);
                float range = float.Parse(weaponSheet.Cells[row, 6].Text);

                WeaponAttackType attackType=(WeaponAttackType)System.Enum.Parse(typeof(WeaponAttackType), weaponSheet.Cells[row, 7].Text);

                ItemData baseItem = itemDictionary[id];
                WeaponData weapon = new WeaponData(baseItem.id, baseItem.name, baseItem.description, baseItem.maxStack, damage, windUp,recovery, range,attackType);
                itemDictionary[id] = weapon;
            }

        }
        Debug.Log("Loaded " + itemDictionary.Count + " items from Excel.");
    }

    public ItemData GetItemById(int id)
    {
        if (itemDictionary.TryGetValue(id, out ItemData item))
        {
            return item;
        }
        Debug.LogWarning("Item ID " + id + " not found!");
        return null;
    }

    public ItemData GetItemByName(string name)
    {
        foreach (var item in itemDictionary.Values)
        {
            if (item.name == name)
            {
                return item;
            }
        }
        Debug.LogWarning("Item Name " + name + " not found!");
        return null;
    }

    public List<ItemData> GetAllItems()
    {
        return new List<ItemData>(itemDictionary.Values);
    }
}
