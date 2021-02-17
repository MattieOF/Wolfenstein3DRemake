using UnityEngine;

public class InitTextures : MonoBehaviour
{
    void Start()
    {
        Tile.InitTileTypes();
        Entity.InitEntityTypes();
    }
}
