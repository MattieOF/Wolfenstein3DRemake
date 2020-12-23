using UnityEngine;

public class TilePaletteItem : PaletteItem
{
    [Header("Properties")]
    public TileInfo tileInfo;

    public override void Select()
    {
        base.Select();
        tilePalette.SelectTile(this);
    }

    public override void PointerEnter()
    {
        base.PointerEnter();
        tilePalette.SetTileInfoBox(tileInfo);
    }
}
