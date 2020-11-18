using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Uween;
using TMPro;

public class MenuButtons : MonoBehaviour
{
    [Header("Properties")]
    public bool hasRisen = false;
    public float tweenTime = 0.2f;

    [Header("Scene References")]
    public TMP_InputField levelToLoadInput;
    public GameObject campaignButton, editorButton, customLevelButton, fadeOut, editorMenu, campaignMenu, customLevelMenu, levelNotFound;

    void Start()
    {
        levelNotFound.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Quit();
    }

    public void Rise()
    {
        TweenY.Add(gameObject, tweenTime, 250).EaseInOutSine();
        hasRisen = true;
    }

    public void EditorButton()
    {
        editorMenu.SetActive(true);
        campaignMenu.SetActive(false);
        customLevelMenu.SetActive(false);
        if (!hasRisen) Rise();
        TweenX.Add(editorButton, tweenTime, 0).EaseInOutSine();
        TweenX.Add(campaignButton, tweenTime, 140).EaseInOutSine();
        TweenX.Add(customLevelButton, tweenTime, 280).EaseInOutSine();
    }

    public void CampaignButton()
    {
        editorMenu.SetActive(false);
        campaignMenu.SetActive(true);
        customLevelMenu.SetActive(false);
        if (!hasRisen) Rise();
        TweenX.Add(editorButton, tweenTime, -140).EaseInOutSine();
        TweenX.Add(campaignButton, tweenTime, 0).EaseInOutSine();
        TweenX.Add(customLevelButton, tweenTime, 140).EaseInOutSine();
    }

    public void CustomLevelButton()
    {
        editorMenu.SetActive(false);
        campaignMenu.SetActive(false);
        customLevelMenu.SetActive(true);
        if (!hasRisen) Rise();
        TweenX.Add(editorButton, tweenTime, -280).EaseInOutSine();
        TweenX.Add(campaignButton, tweenTime, -140).EaseInOutSine();
        TweenX.Add(customLevelButton, tweenTime, 0).EaseInOutSine();
    }

    public void Quit()
    {
        Fadeout();
        StartCoroutine(QuitGameAfterDelay());
    }

    public void OpenEditor()
    {
        Fadeout();
        StartCoroutine(LoadSceneAfterDelay(0.45f, "Editor"));
    }

    public void LoadLevelInEditor()
    {
        if (!EditorManager.LevelExists(levelToLoadInput.text))
        {
            levelNotFound.SetActive(true);
            return;
        }

        EditorManager.loadLevelOnStart = true;
        EditorManager.levelToLoadOnStart = levelToLoadInput.text;
        Fadeout();
        StartCoroutine(LoadSceneAfterDelay(0.45f, "Editor"));
    }

    public void TestLevel()
    {
        Fadeout();
        StartCoroutine(LoadSceneAfterDelay(0.45f, "Game"));
    }

    public void Fadeout()
    {
        fadeOut.SetActive(true);
        TweenA.Add(fadeOut, 0.4f, 1);
        TweenY.Add(gameObject, 0.25f, -50).EaseInOutSine();
    }

    IEnumerator QuitGameAfterDelay(float delay = 0.45f)
    {
        yield return new WaitForSecondsRealtime(delay);
        Application.Quit();
    }

    IEnumerator LoadSceneAfterDelay(float delay, string scene)
    {
        yield return new WaitForSecondsRealtime(delay);
        SceneManager.LoadScene(scene);
    }
}
