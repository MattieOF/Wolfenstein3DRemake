using UnityEngine;

public class EntityPaletteItem : PaletteItem
{
    [Header("Properties")]
    public EntityInfo entityInfo;

    public override void Select()
    {
        base.Select();
        tilePalette.SelectEntity(this);
    }

    public override void PointerEnter()
    {
        base.PointerEnter();
        tilePalette.SetTileInfoBox(entityInfo);
    }
}
