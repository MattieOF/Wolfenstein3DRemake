using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    [Header("Scene References")]
    public Transform cameraTransform;
    [Header("Properties")]
    public float interactionDistance = .75f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RaycastHit hit;
            if (Physics.Raycast(cameraTransform.position, transform.TransformDirection(Vector3.forward), out hit, interactionDistance))
            {
                if (hit.transform.tag == "TileMoveable")
                {
                    hit.transform.gameObject.GetComponent<MoveableTile>().Move();
                }
            }
        }
    }
}
