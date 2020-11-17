using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionBox : MonoBehaviour
{
    [Header("Scene References")]
    public Camera editorCamera;

    void Update()
    {
        Ray ray = editorCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;
        if (Physics.Raycast(ray, out rayHit))
        {
            GetComponent<MeshRenderer>().enabled = true;
            Vector3 roundedLoc = new Vector3(Mathf.FloorToInt(rayHit.point.x + 0.5f), 1, Mathf.FloorToInt(rayHit.point.z + 0.5f));
            transform.position = roundedLoc;
        } else
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
