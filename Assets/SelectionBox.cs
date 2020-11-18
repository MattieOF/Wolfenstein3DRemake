using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionBox : MonoBehaviour
{
    [Header("Scene References")]
    public Camera editorCamera;
    public EditorManager editorManager;

    void Update()
    {
        Ray ray = editorCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;
        Vector3 roundedLoc;
        if (Physics.Raycast(ray, out rayHit))
        {
            GetComponent<MeshRenderer>().enabled = true;
            roundedLoc = new Vector3(Mathf.FloorToInt(rayHit.point.x + 0.5f), 1, Mathf.FloorToInt(rayHit.point.z + 0.5f));
            transform.position = roundedLoc;

            if (Input.GetMouseButton(0))
            {
                editorManager.PlaceTile(roundedLoc);
            } else if (Input.GetMouseButton(1))
            {
                editorManager.RemoveTile(roundedLoc);
            }
        } else
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
