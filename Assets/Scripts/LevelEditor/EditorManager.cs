﻿using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Uween;

public class EditorManager : MonoBehaviour
{
    [Header("Scene References")]
    public GameObject saveFirstObject;
    public TilePalette tilePalette;
    public LevelNameControl levelNameControl;
    public TMP_InputField levelLoadInput;
    public GameObject levelLoadInputObject;
    public EditorCameraControl cameraControl;
    public Image levelNameBG;
    public Color levelNameBGDefault;
    public GameObject moveableTileUI;

    [Header("Asset References")]
    public GameObject tilePrefab;

    [HideInInspector]
    public static bool loadLevelOnStart = false;
    [HideInInspector]
    public static string levelToLoadOnStart = "";

    private List<GameObject> objects = new List<GameObject>();

    private bool editingMoveTileStart = false, editingMoveTileEnd = false;

    // [Header("Level Properties")]
    public string LevelName
    {
        get { return level.name; }
        set { level.name = value; }
    }

    [Header("Properties")]
    public string menuSceneName = "Menu";
    public string gameSceneName = "Game";

    private LevelData level = new LevelData(100, 100);

    void Start()
    {
        saveFirstObject.SetActive(false);
        levelLoadInputObject.SetActive(false);
        if (loadLevelOnStart)
        {
            LoadLevel(levelToLoadOnStart);
        }
    }

    public static bool LevelExists(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return false;
        return File.Exists(Application.persistentDataPath + "/Wolf3DLevels/" + name + ".xml");
    }

    public void SetName(string name)
    {
        level.name = name;
    }

    public void Save()
    {
        Debug.Log("Saving level.");
        LevelSerialiser.Save(level);
        // SAVE LEVEL HERE
    }

    public void ShowLoadLevelInput()
    {
        cameraControl.acceptInput = false;
        cameraControl.ResetInput();
        levelLoadInputObject.SetActive(true);
    }

    public void HideLoadLevelInput()
    {
        cameraControl.acceptInput = true;
        levelLoadInputObject.SetActive(false);
    }

    public void LoadFromInput()
    {
        LoadLevel(levelLoadInput.text);
        HideLoadLevelInput();
    }

    public void LoadLevel(string name)
    {
        if (!File.Exists(Application.persistentDataPath + "/Wolf3DLevels/" + name + ".xml"))
        {
            Debug.Log("Level " + Application.persistentDataPath + "/Wolf3DLevels/" + name + ".xml not found.");
            return;
        }

        Debug.Log("Opening level " + Application.persistentDataPath + "/Wolf3DLevels/" + name + ".xml");
        StreamReader file = new StreamReader(Application.persistentDataPath + "/Wolf3DLevels/" + name + ".xml");

        ClearEditor();
        XmlSerializer serializer = new XmlSerializer(typeof(LevelData));
        level = (LevelData)serializer.Deserialize(file.BaseStream);
        levelNameControl.SetName(level.name);
        file.Close();
        LoadTiles();
        LoadEntities();
    }

    public void LoadTiles()
    {
        for (int x = 0; x < level.levelSize.x; x++)
        {
            for (int y = 0; y < level.levelSize.y; y++)
            {
                if (level.tiles[x][y] != null)
                {
                    GameObject go = Instantiate(tilePrefab);
                    go.transform.position = new Vector3(x, 1, y);
                    level.tiles[x][y].texture = Resources.Load("Textures/" + level.tiles[x][y].textureName, typeof(Texture)) as Texture;
                    go.GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", level.tiles[x][y].texture);
                    objects.Add(go);
                }
            }
        }
    }

    public void LoadEntities()
    {
        for (int x = 0; x < level.levelSize.x; x++)
        {
            for (int y = 0; y < level.levelSize.y; y++)
            {
                if (level.entities[x][y] != null)
                {
                    GameObject go = Instantiate(tilePrefab);
                    go.transform.position = new Vector3(x, 1, y);
                    level.entities[x][y].texture = Resources.Load("Textures/" + level.entities[x][y].editorIconName, typeof(Texture)) as Texture;
                    go.GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", level.entities[x][y].texture);
                    objects.Add(go);
                }
            }
        }
    }

    public void ClearEditor()
    {
        foreach (GameObject go in objects) Destroy(go);
        objects.Clear();
    }

    public void OpenSaveFirstMessage()
    {
        saveFirstObject.SetActive(true);
    }

    public void CloseSaveFirstMessage()
    {
        saveFirstObject.SetActive(false);
    }

    public void CloseEditor()
    {
        // CLEANUP HERE
        SceneManager.LoadScene(menuSceneName);
    }

    public void Place(Vector3 location)
    {
        if (tilePalette.selectedItemType == ItemType.None) return;
        if (level.ItemExistsAt((int)location.x, (int)location.z)) return;
        if (tilePalette.selectedItemType == ItemType.Tile)
        {
            if (tilePalette.selectedTile == null) return;
            else PlaceTile(location);
        } else
        {
            if (tilePalette.selectedEntity == null) return;
            else PlaceEntity(location);
        }
    }

    public void PlaceTile(Vector3 location)
    {
        level.SetTileAt((int)location.x, (int)location.z, tilePalette.selectedTile);

        GameObject go = Instantiate(tilePrefab);
        go.transform.position = location;
        go.GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", tilePalette.selectedTile.texture);
        objects.Add(go);
    }

    public void PlaceEntity(Vector3 location)
    {
        level.SetEntityAt((int)location.x, (int)location.z, tilePalette.selectedEntity);
        GameObject go = Instantiate(tilePrefab);
        go.transform.position = location;
        go.GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", tilePalette.selectedEntity.texture);
        go.tag = "Entity";

        objects.Add(go);
    }

    public void RemoveItem(Vector3 location)
    {
        if (!level.ItemExistsAt((int)location.x, (int)location.z)) return;
        level.RemoveItemAt((int)location.x, (int)location.z);

        Collider[] colliders = Physics.OverlapSphere(location, 0.2f);
        if (colliders.Length != 0)
        {
            foreach (Collider collider in colliders)
            {
                if (collider.tag == "Tile" || collider.tag == "Entity")
                {
                    objects.Remove(collider.gameObject);
                    Destroy(collider.gameObject);
                }
            }
        }
        else return;
    }

    public void PlayLevel()
    {
        if (LevelName == "Untitled Level")
        {
            levelNameBG.color = Color.red;
            TweenC.Add(levelNameBG.gameObject, 1.5f, levelNameBGDefault);
            TweenA.Add(levelNameBG.gameObject, 1f, levelNameBGDefault.a);
            return;
        }

        Save();
        LevelLoader.loadedFromEditor = true;
        LevelLoader.levelToLoad = LevelName;
        SceneManager.LoadScene(gameSceneName);
    }

    public void ToggleMoveableTileUI()
    {
        if (editingMoveTileEnd || editingMoveTileStart) return;
        moveableTileUI.SetActive(!moveableTileUI.activeSelf);
    }

    public void EditMoveableTileStart()
    {
        if (editingMoveTileEnd) return;
        editingMoveTileStart = true;
        // Show UI
    }

    public void EditMoveableTileEnd()
    {
        if (editingMoveTileStart) return;
        editingMoveTileEnd = true;
        // Show UI
    }
}
