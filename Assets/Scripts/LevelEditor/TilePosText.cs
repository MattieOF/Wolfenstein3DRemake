using UnityEngine;
using TMPro;

public class TilePosText : MonoBehaviour
{
    [Header("Scene References")]
    public TextMeshProUGUI xPosText;
    public TextMeshProUGUI yPosText;

    private bool currentlyActive = true;

    public void SetTilePosActive(bool active)
    {
        if (active == currentlyActive) return;
        currentlyActive = active;

        foreach (Transform t in transform)
            t.gameObject.SetActive(active);
    }

    public void SetPos(int x, int y)
    {
        SetTilePosActive(true);
        xPosText.text = x.ToString();
        yPosText.text = y.ToString();
    }
}
