using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// component on the phone screen
/// </summary>
public class DocumentSelect : MonoBehaviour
{
    public int index;
    public string owner;
    public TMP_Text text;

    public void setTextContent(string content)
    {
        text.text = content;
    }

    public void OnPressDocumentBar()
    {
        SFXManager.Instance.PlaySFX("PhoneTap");
        DocumentManager.Instance.ShowDocument(owner, index);
        PhoneUIManager.Instance.PhoneUI.SetActive(false);
    }
    
}
