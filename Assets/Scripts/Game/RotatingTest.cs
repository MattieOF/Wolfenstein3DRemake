using UnityEngine;

public class RotatingTest : MonoBehaviour
{
    public float speed = 20f;

    void Update()
    {
        transform.Rotate(0, speed * Time.deltaTime, 0);
    }
}
