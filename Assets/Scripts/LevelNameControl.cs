using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelNameControl : MonoBehaviour
{
    [Header("Scene References")]
    public GameObject nameTextObject;
    public GameObject nameInputObject;
    public TextMeshProUGUI nameText;
    public TMP_InputField nameInputField;
    public EditorCameraControl cameraControl;
    public Image background;

    [Header("Properties")]
    public Color defaultBackgroundColour;
    public Color hoveredBackgroundColour;
    public string levelName = "Untitled level";

    private bool input = false;

    void Start()
    {
        nameInputField.text = levelName;
    }

    public void StartInput()
    {
        input = true;
        cameraControl.acceptInput = false;
        nameInputObject.SetActive(true);
        nameTextObject.SetActive(false);
    }

    public void EndInput()
    {
        input = false;
        cameraControl.acceptInput = true;
        nameText.text = nameInputField.text;
        // TODO Update level name
        nameInputObject.SetActive(false);
        nameTextObject.SetActive(true);
    }

    public void MouseEnter()
    {
        background.color = hoveredBackgroundColour;
    }

    public void MouseExit()
    {
        background.color = defaultBackgroundColour;
    }

}
