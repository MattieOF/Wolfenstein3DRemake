using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[Serializable]
public class Entity
{
    [Header("Properties")]
    public string entityType;

    public Entity()
    { }

    [JsonIgnore]
    public static Dictionary<string, EntityInfo> entityTypes = new Dictionary<string, EntityInfo>();

    public static void InitEntityTypes()
    {
        entityTypes.Clear();
        EntityInfo[] entities = Resources.LoadAll<EntityInfo>("Entities");

        foreach (EntityInfo entity in entities)
        {
            entity.texture = Resources.Load("Textures/" + entity.editorIconName, typeof(Texture)) as Texture;
            entityTypes.Add(entity.name, entity);
        }
    }
}
