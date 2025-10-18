using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemToolTip : MonoBehaviour
{
    public static ItemToolTip Instance;
    public GameObject ToolHintPanel;
    public TMP_Text itemNameText;
    public TMP_Text itemDescriptionText;
    public bool visiblity;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        SetVisibility(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!visiblity) return;
        else
        {
            Vector3 offset = new Vector3(20f, -30f, 0f);
            ToolHintPanel.transform.position = Input.mousePosition + offset;
        }
    }
    public void SetVisibility(bool showOrHide)
    {
        visiblity = showOrHide;
        SetToolHintText("", "");
    }

    public void SetToolHintText(string itemName, string itemDescription)
    {
        itemNameText.text = itemName;
        itemDescriptionText.text = itemDescription;
    }

    
}
