// Decompiled with JetBrains decompiler
// Type: RTG.ObjectBounds
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public static class ObjectBounds
  {
    private static ObjectBounds.QueryConfig _defaultQConfig;

    static ObjectBounds()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      ObjectBounds._defaultQConfig = new ObjectBounds.QueryConfig();
      ObjectBounds._defaultQConfig.ObjectTypes = GameObjectTypeHelper.AllCombined;
      ObjectBounds._defaultQConfig.NoVolumeSize = Vector3.zero;
    }

    public static ObjectBounds.QueryConfig DefaultQConfig => ObjectBounds._defaultQConfig;

    public static Rect CalcScreenRect(
      GameObject gameObject,
      Camera camera,
      ObjectBounds.QueryConfig queryConfig)
    {
      AABB aabb = ObjectBounds.CalcWorldAABB(gameObject, queryConfig);
      return !aabb.IsValid ? new Rect(0.0f, 0.0f, 0.0f, 0.0f) : aabb.GetScreenRectangle(camera);
    }

    public static OBB CalcSpriteWorldOBB(GameObject gameObject)
    {
      AABB modelSpaceAABB = ObjectBounds.CalcSpriteModelAABB(gameObject);
      return !modelSpaceAABB.IsValid ? OBB.GetInvalid() : new OBB(modelSpaceAABB, gameObject.transform);
    }

    public static AABB CalcSpriteWorldAABB(GameObject gameObject)
    {
      AABB aabb = ObjectBounds.CalcSpriteModelAABB(gameObject);
      if (!aabb.IsValid)
        return aabb;
      aabb.Transform(gameObject.transform.localToWorldMatrix);
      return aabb;
    }

    public static AABB CalcSpriteModelAABB(GameObject spriteObject)
    {
      SpriteRenderer component = spriteObject.GetComponent<SpriteRenderer>();
      return (Object) component == (Object) null ? AABB.GetInvalid() : component.GetModelSpaceAABB();
    }

    public static OBB GetMeshWorldOBB(GameObject gameObject)
    {
      AABB modelSpaceAABB = ObjectBounds.CalcMeshModelAABB(gameObject);
      return !modelSpaceAABB.IsValid ? OBB.GetInvalid() : new OBB(modelSpaceAABB, gameObject.transform);
    }

    public static AABB GetMeshWorldAABB(GameObject gameObject)
    {
      AABB meshWorldAabb = ObjectBounds.CalcMeshModelAABB(gameObject);
      if (!meshWorldAabb.IsValid)
        return meshWorldAabb;
      meshWorldAabb.Transform(gameObject.transform.localToWorldMatrix);
      return meshWorldAabb;
    }

    public static AABB CalcObjectCollectionWorldAABB(
      IEnumerable<GameObject> gameObjectCollection,
      ObjectBounds.QueryConfig queryConfig)
    {
      AABB aabb1 = AABB.GetInvalid();
      foreach (GameObject gameObject in gameObjectCollection)
      {
        AABB aabb2 = ObjectBounds.CalcWorldAABB(gameObject, queryConfig);
        if (aabb2.IsValid)
        {
          if (aabb1.IsValid)
            aabb1.Encapsulate(aabb2);
          else
            aabb1 = aabb2;
        }
      }
      return aabb1;
    }

    public static AABB CalcHierarchyCollectionWorldAABB(
      IEnumerable<GameObject> roots,
      ObjectBounds.QueryConfig queryConfig)
    {
      AABB aabb1 = AABB.GetInvalid();
      foreach (GameObject root in roots)
      {
        AABB aabb2 = ObjectBounds.CalcHierarchyWorldAABB(root, queryConfig);
        if (aabb2.IsValid)
        {
          if (aabb1.IsValid)
            aabb1.Encapsulate(aabb2);
          else
            aabb1 = aabb2;
        }
      }
      return aabb1;
    }

    public static OBB CalcHierarchyWorldOBB(GameObject root, ObjectBounds.QueryConfig queryConfig)
    {
      AABB modelSpaceAABB = ObjectBounds.CalcHierarchyModelAABB(root, queryConfig);
      return !modelSpaceAABB.IsValid ? OBB.GetInvalid() : new OBB(modelSpaceAABB, root.transform);
    }

    public static AABB CalcHierarchyWorldAABB(GameObject root, ObjectBounds.QueryConfig queryConfig)
    {
      AABB aabb = ObjectBounds.CalcHierarchyModelAABB(root, queryConfig);
      if (!aabb.IsValid)
        return AABB.GetInvalid();
      aabb.Transform(root.transform.localToWorldMatrix);
      return aabb;
    }

    public static OBB CalcWorldOBB(GameObject gameObject, ObjectBounds.QueryConfig queryConfig)
    {
      AABB modelSpaceAABB = ObjectBounds.CalcModelAABB(gameObject, queryConfig, gameObject.GetGameObjectType());
      return !modelSpaceAABB.IsValid ? OBB.GetInvalid() : new OBB(modelSpaceAABB, gameObject.transform);
    }

    public static AABB CalcWorldAABB(GameObject gameObject, ObjectBounds.QueryConfig queryConfig)
    {
      AABB aabb = ObjectBounds.CalcModelAABB(gameObject, queryConfig, gameObject.GetGameObjectType());
      if (!aabb.IsValid)
        return aabb;
      aabb.Transform(gameObject.transform.localToWorldMatrix);
      return aabb;
    }

    public static AABB CalcMeshWorldAABB(GameObject gameObject)
    {
      AABB aabb = ObjectBounds.CalcMeshModelAABB(gameObject);
      if (!aabb.IsValid)
        return aabb;
      aabb.Transform(gameObject.transform.localToWorldMatrix);
      return aabb;
    }

    public static AABB CalcHierarchyModelAABB(GameObject root, ObjectBounds.QueryConfig queryConfig)
    {
      Matrix4x4 localToWorldMatrix = root.transform.localToWorldMatrix;
      AABB aabb1 = ObjectBounds.CalcModelAABB(root, queryConfig, root.GetGameObjectType());
      foreach (GameObject allChild in root.GetAllChildren())
      {
        AABB aabb2 = ObjectBounds.CalcModelAABB(allChild, queryConfig, allChild.GetGameObjectType());
        if (aabb2.IsValid)
        {
          Matrix4x4 relativeTransform = allChild.transform.localToWorldMatrix.GetRelativeTransform(localToWorldMatrix);
          aabb2.Transform(relativeTransform);
          if (aabb1.IsValid)
            aabb1.Encapsulate(aabb2);
          else
            aabb1 = aabb2;
        }
      }
      return aabb1;
    }

    public static AABB CalcMeshModelAABB(GameObject gameObject)
    {
      Mesh mesh = gameObject.GetMesh();
      return (Object) mesh == (Object) null ? AABB.GetInvalid() : new AABB(mesh.bounds);
    }

    public static AABB CalcModelAABB(
      GameObject gameObject,
      ObjectBounds.QueryConfig queryConfig,
      GameObjectType objectType)
    {
      if ((objectType & queryConfig.ObjectTypes) == (GameObjectType) 0)
        return AABB.GetInvalid();
      switch (objectType)
      {
        case GameObjectType.Mesh:
          Mesh mesh = gameObject.GetMesh();
          return (Object) mesh == (Object) null ? AABB.GetInvalid() : new AABB(mesh.bounds);
        case GameObjectType.Terrain:
          TerrainData terrainData = gameObject.GetComponent<Terrain>().terrainData;
          if ((Object) terrainData == (Object) null)
            return AABB.GetInvalid();
          Vector3 size = terrainData.bounds.size;
          return new AABB(terrainData.bounds.center, size);
        case GameObjectType.Sprite:
          return ObjectBounds.CalcSpriteModelAABB(gameObject);
        default:
          return new AABB(Vector3.zero, queryConfig.NoVolumeSize);
      }
    }

    public struct QueryConfig
    {
      public GameObjectType ObjectTypes;
      public Vector3 NoVolumeSize;
    }
  }
}
