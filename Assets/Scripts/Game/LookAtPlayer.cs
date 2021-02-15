using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    public Transform player;

    void Update()
    {
        if (!player) player = GameObject.FindGameObjectWithTag("Player").transform;
        if (!player) return;
        transform.rotation = player.rotation;
    }
}
