using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneKeybind : MonoBehaviour
{
    [Header("Properties")]
    public string sceneName;
    public KeyCode key;

    void Update()
    {
        if (Input.GetKeyDown(key))
            SceneManager.LoadSceneAsync(sceneName);
    }
}
