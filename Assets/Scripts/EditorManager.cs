using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EditorManager : MonoBehaviour
{
    [Header("Scene References")]
    public GameObject saveFirstObject;

    [Header("Level Properties")]
    public string levelName = "Untitled level";

    [Header("Properties")]
    public string menuSceneName = "Game";

    void Start()
    {
        saveFirstObject.SetActive(false);
    }

    public void Save()
    {
        Debug.Log("Saving level.");
        // SAVE LEVEL HERE
    }

    public void OpenSaveFirstMessage()
    {
        saveFirstObject.SetActive(true);
    }

    public void CloseSaveFirstMessage()
    {
        saveFirstObject.SetActive(false);
    }

    public void CloseEditor()
    {
        // CLEANUP HERE
        SceneManager.LoadScene(menuSceneName);
    }
}
