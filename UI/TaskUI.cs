using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// handle the task and document found text hint
/// </summary>
public class TaskUI : MonoBehaviour
{
    public static TaskUI Instance;
    public TMP_Text TaskText;
    public TMP_Text FoundDocumentText;
    public TMP_Text PhoneToViewHintText;

    Coroutine updateTaskContentCoroutine;
    Coroutine foundDocumentCoroutine;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        FoundDocumentText.text = "";
        PhoneToViewHintText.text = "";
    }
    #region Task
    public void UpdateTaskContent(string content, float interval)
    {
        updateTaskContentCoroutine = StartCoroutine(UpdateTaskText(content, interval));
    }

    IEnumerator UpdateTaskText(string content, float interval)
    {
        TaskText.text = "";
        string hint = "Task Update";
        foreach (char c in hint)
        {
            TaskText.text += c;
            yield return new WaitForSeconds(interval);
        }
        yield return new WaitForSeconds(2f);

        TaskText.text = "";
        foreach (char c in content)
        {
            TaskText.text += c;
            yield return new WaitForSeconds(interval);
        }
    }
    #endregion

    #region Document Found
    public void DisplayNewDocumentFound()
    {
        foundDocumentCoroutine = StartCoroutine(LeftHintAnimation());
    }
    IEnumerator LeftHintAnimation()
    {
        FoundDocumentText.text = "";
        string hint1 = "NEW DOCUMENT FOUND";
        foreach (char c in hint1)
        {
            FoundDocumentText.text += c;
            yield return new WaitForSeconds(0.02f);
        }
        yield return new WaitForSeconds(1.5f);

        PhoneToViewHintText.text = "";
        string hint2 = "OPEN THE PHONE TO VIEW";
        foreach(char c in hint2)
        {
            PhoneToViewHintText.text += c;
            yield return new WaitForSeconds(0.02f);
        }

        yield return new WaitForSeconds(2f);
        FoundDocumentText.text = "";
        PhoneToViewHintText.text = "";
    }
    #endregion
}
