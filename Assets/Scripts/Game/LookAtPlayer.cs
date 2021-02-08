using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    public Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (!player) return;
        transform.rotation = player.rotation;
    }
}
