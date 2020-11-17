using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New Tile", menuName = "Game/Tile")]
public class TileInfo : ScriptableObject
{
    public Texture texture;

    [HideInInspector]
    public bool moveableTile = false;
    [HideInInspector]
    public Vector2 tileMoveOffset = new Vector2();

    // TODO Possible support for directional textures?
}
