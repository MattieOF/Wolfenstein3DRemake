using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Diagnostics;
using System;

public class MapInfoPanel : MonoBehaviour
{
    [Header("Scene References")]
    public TextMeshProUGUI mapNameText;
    public TextMeshProUGUI tileCountText, killCountText, timeText;

    [Header("Properties")]
    public bool visible = false;

    private readonly Stopwatch timer = new Stopwatch();
    private int killCount;
    
    void Start()
    {
        SetVisible(visible);
        StartTimer();
    }

    public void SetValues(string mapName, int tileCount, int enemyCount)
    {
        mapNameText.text = mapName;
        tileCountText.text = "Tiles: " + tileCount;
        killCountText.text = "Kills: 0/" + enemyCount;
    }

    public void StartTimer()
    {
        timer.Start();
    }

    public void StopTimer()
    {
        timer.Stop();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SetVisible(!visible);
        }

        if (visible)
        {
            TimeSpan ts = timer.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}",
                ts.TotalMinutes, ts.Seconds, ts.Milliseconds / 10);
            timeText.text = "Time: " + elapsedTime;
        }
    }

    void SetVisible(bool newVisible)
    {
        foreach (Transform go in gameObject.transform)
            go.gameObject.SetActive(newVisible);

        GetComponent<Image>().enabled = newVisible;

        visible = newVisible;
    }
}
