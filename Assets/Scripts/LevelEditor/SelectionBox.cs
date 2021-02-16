using UnityEngine;

public class SelectionBox : MonoBehaviour
{
    [Header("Scene References")]
    public Camera editorCamera;
    public EditorManager editorManager;
    public TilePosText tilePosText;

    [Header("Properties")]
    public bool allowPlace = true;

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
            tilePosText.SetPos((int)roundedLoc.x, (int)roundedLoc.z);

            if (Input.GetMouseButton(0) && allowPlace)
            {
                editorManager.Place(roundedLoc, Input.GetMouseButtonDown(0));
            } else if (Input.GetMouseButton(1) && allowPlace)
            {
                editorManager.RemoveItem(roundedLoc);
            }
        } else
        {
            tilePosText.SetTilePosActive(false);
            GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public void EnablePlacement() { allowPlace = true; }

    public void DisablePlacement() { allowPlace = false; }
}
