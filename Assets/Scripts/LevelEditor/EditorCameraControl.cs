using System;
using UnityEngine;

[Serializable]
public enum EditorCameraProjection
{
    Perspective,
    Orthrographic
}

public class EditorCameraControl : MonoBehaviour
{
    [Header("Camera Properties")]
    public float zoomAmount = 3f;
    public float moveSpeed = 2f;
    public bool acceptInput = true;
    public EditorCameraProjection projection = EditorCameraProjection.Perspective;

    [Header("Keybinds")]
    public KeyCode toggleProjection = KeyCode.P;

    private float mouseWheel;
    private float horizontal;
    private float vertical;

    void Start()
    {
        if (projection == EditorCameraProjection.Orthrographic) GetComponent<Camera>().orthographic = true;
        else GetComponent<Camera>().orthographic = false;
    }

    // Input polling 
    void Update()
    {
        if (!acceptInput) return;

        mouseWheel = Input.mouseScrollDelta.y;
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(toggleProjection))
        {
            if (projection == EditorCameraProjection.Orthrographic)
            {
                projection = EditorCameraProjection.Perspective;
                GetComponent<Camera>().orthographic = false;
            } else
            {
                projection = EditorCameraProjection.Orthrographic;
                GetComponent<Camera>().orthographic = true;
            }
        }
    }

    // Perform camera movement here
    void FixedUpdate()
    {
        if (mouseWheel != 0)
        {
            if (projection == EditorCameraProjection.Orthrographic)
            {
                GetComponent<Camera>().orthographicSize += zoomAmount * -mouseWheel * 0.5f;
            }
            else
            {
                Vector3 pos = transform.position;
                pos.y += zoomAmount * -mouseWheel;
                transform.position = pos;
            }
        }

        if (horizontal != 0 || vertical != 0)
        {
            Vector3 pos = transform.position;
            pos.x += horizontal * moveSpeed;
            pos.z += vertical * moveSpeed;
            transform.position = pos;
        }
    }

    public void ResetInput()
    {
        vertical = 0;
        horizontal = 0;
        mouseWheel = 0;
    }
}
