using UnityEngine;

public class PaletteItem : MonoBehaviour
{
    [Header("Scene References")]
    public GameObject highlight;
    public GameObject selectHighlight;
    public TilePalette tilePalette;

    void Start()
    {
        highlight.SetActive(false);
        selectHighlight.SetActive(false);
    }

    public void PointerEnter()
    {
        highlight.SetActive(true);
    }

    public void PointerExit()
    {
        highlight.SetActive(false);
    }

    public virtual void Select()
    {
        if (selectHighlight.activeSelf) return;
        selectHighlight.SetActive(true);
    }

    public void Deselect()
    {
        selectHighlight.SetActive(false);
    }
}
