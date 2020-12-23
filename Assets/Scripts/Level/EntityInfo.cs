using System.Xml.Serialization;
using UnityEngine;

[CreateAssetMenu(fileName = "New Entity", menuName = "Game/Entity")]
public class EntityInfo : ScriptableObject
{
    [Header("Editor Values")]
    public string editorIconName;

    [Header("Entity Properties")]
    public string entityPrefabName;
    public string entityName;
    public bool isEnemy = false;

    [XmlIgnore]
    [HideInInspector]
    public Texture texture;
}