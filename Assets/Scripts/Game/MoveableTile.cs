using UnityEngine;
using Uween;

public enum MoveAxis
{
    X,
    Y
}

public class MoveableTile : MonoBehaviour
{
    [Header("Properties")]
    public Vector2 endPosition;
    [Tooltip("Time in seconds for the tile to move one unit")]
    public float speed = .75f;

    private bool moved = false;
    private MoveAxis axis;

    public void Move()
    {
        if (moved) return;

        WorkOutDirAndSpeed();
        switch (axis)
        {
            case MoveAxis.X:
                TweenX.Add(gameObject, speed, endPosition.x);
                break;
            case MoveAxis.Y:
                TweenY.Add(gameObject, speed, endPosition.y);
                break;
        }

        moved = true;
    }

    private void WorkOutDirAndSpeed()
    {
        bool moveX, moveY;
        moveX = !(transform.position.x == endPosition.x);
        moveY = !(transform.position.z == endPosition.y);
        if (moveX) axis = MoveAxis.X;
        else if (moveY) axis = MoveAxis.Y;

        float distance = 0;
        switch (axis)
        {
            case MoveAxis.X:
                distance = Mathf.Abs(transform.position.x - endPosition.x);
                break;
            case MoveAxis.Y:
                distance = Mathf.Abs(transform.position.z - endPosition.y);
                break;
        }

        speed *= distance;
    }
}
