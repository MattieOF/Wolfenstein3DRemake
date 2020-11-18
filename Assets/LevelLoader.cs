using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using TMPro;

public class LevelLoader : MonoBehaviour
{
    [HideInInspector]
    public static string levelToLoad = "";

    [Header("Scene References")]
    public GameObject levelObjects;
    public GameObject playerObject, tilePrefab;
    public MapInfoPanel mapInfoPanel;

    private LevelData level;
    private int tilesCount = 0, enemyCount = 0;

    void Start()
    {
        LoadLevel(levelToLoad);
    }

    public void LoadLevel(string levelName)
    {
        if (!EditorManager.LevelExists(levelName)) return;

        Debug.Log("Opening level " + Application.persistentDataPath + "/Wolf3DLevels/" + levelName + ".xml");
        StreamReader file = new StreamReader(Application.persistentDataPath + "/Wolf3DLevels/" + levelName + ".xml");

        ClearLevel();
        XmlSerializer serializer = new XmlSerializer(typeof(LevelData));
        level = (LevelData)serializer.Deserialize(file.BaseStream);
        // mapInfoPanel.SetValues(level.name)
        file.Close();
        LoadTiles();
    }

    public void LoadTiles()
    {
        tilesCount = 0;
        enemyCount = 0;
        for (int x = 0; x < level.levelSize.x; x++)
        {
            for (int y = 0; y < level.levelSize.y; y++)
            {
                if (level.tiles[x][y] != null)
                {
                    tilesCount++;
                    GameObject go = Instantiate(tilePrefab, levelObjects.transform);
                    go.transform.position = new Vector3(x, 1, y);
                    level.tiles[x][y].texture = Resources.Load("Textures/" + level.tiles[x][y].textureName, typeof(Texture)) as Texture;
                    go.GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", level.tiles[x][y].texture);

                    Vector2[] uvs = go.GetComponent<MeshFilter>().sharedMesh.uv;

                    uvs[6] = new Vector2(0, 0);
                    uvs[7] = new Vector2(1, 0);
                    uvs[10] = new Vector2(0, 1);
                    uvs[11] = new Vector2(1, 1);

                    go.GetComponent<MeshFilter>().sharedMesh.uv = uvs;
                }
            }
        }

        if (level.playerPosition != Vector3.one * -1)
        {
            playerObject.transform.position = level.playerPosition;
        } else
        {
            playerObject.transform.position = new Vector3(level.tiles.Length / 2, 1, level.tiles[0].Length / 2);
        }

        mapInfoPanel.SetValues(level.name, tilesCount, enemyCount);
    }

    public void ClearLevel()
    {
        foreach (Transform t in levelObjects.transform)
            Destroy(t.gameObject);
    }
}
