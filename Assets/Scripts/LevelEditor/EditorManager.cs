using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EditorManager : MonoBehaviour
{
    [Header("Scene References")]
    public GameObject saveFirstObject;

    // [Header("Level Properties")]
    public string LevelName
    {
        get { return level.name; }
        set { level.name = value; }
    }

    [Header("Properties")]
    public string menuSceneName = "Game";

    private LevelData level = new LevelData(100, 100);

    void Start()
    {
        saveFirstObject.SetActive(false);
    }

    public void SetName(string name)
    {
        level.name = name;
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
