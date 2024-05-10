// Decompiled with JetBrains decompiler
// Type: RTG.GameObjectEx
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
  public static class GameObjectEx
  {
    private static List<Transform> _transformsChildren;

    public static void SetStatic(this GameObject gameObject, bool isStatic, bool affectChildren)
    {
      if (!affectChildren)
      {
        gameObject.isStatic = isStatic;
      }
      else
      {
        foreach (GameObject gameObject1 in gameObject.GetAllChildrenAndSelf())
          gameObject1.isStatic = isStatic;
      }
    }

    public static bool IsRTGAppObject(this GameObject gameObject)
    {
      return gameObject.transform.root.gameObject.GetComponent<IRLDApplication>() != null;
    }

    /// <summary>Returns the game object's type.</summary>
    /// <remarks>
    /// Because a game object might have multiple components of different types attached to
    /// it, the function uses the following priority for object components:
    ///     1. Mesh;
    ///     2. Terrain;
    ///     3. Sprite;
    ///     4. Camera;
    ///     5. Light;
    ///     6. Particle System;
    ///     7. Empty;
    /// </remarks>
    public static GameObjectType GetGameObjectType(this GameObject gameObject)
    {
      if ((Object) gameObject.GetMesh() != (Object) null)
        return GameObjectType.Mesh;
      if ((Object) gameObject.GetComponent<Terrain>() != (Object) null)
        return GameObjectType.Terrain;
      if ((Object) gameObject.GetSprite() != (Object) null)
        return GameObjectType.Sprite;
      if ((Object) gameObject.GetComponent<Camera>() != (Object) null)
        return GameObjectType.Camera;
      if ((Object) gameObject.GetComponent<Light>() != (Object) null)
        return GameObjectType.Light;
      return (Object) gameObject.GetComponent<ParticleSystem>() != (Object) null ? GameObjectType.ParticleSystem : GameObjectType.Empty;
    }

    public static bool HierarchyHasMesh(this GameObject root)
    {
      foreach (GameObject gameObject in root.GetAllChildrenAndSelf())
      {
        if ((Object) gameObject.GetMesh() != (Object) null)
          return true;
      }
      return false;
    }

    public static bool HierarchyHasSprite(this GameObject root)
    {
      foreach (GameObject gameObject in root.GetAllChildrenAndSelf())
      {
        if ((Object) gameObject.GetSprite() != (Object) null)
          return true;
      }
      return false;
    }

    public static bool HierarchyHasObjectsOfType(this GameObject root, GameObjectType typeFlags)
    {
      foreach (GameObject gameObject in root.GetAllChildrenAndSelf())
      {
        GameObjectType gameObjectType = gameObject.GetGameObjectType();
        if ((typeFlags & gameObjectType) != (GameObjectType) 0)
          return true;
      }
      return false;
    }

    public static List<GameObject> GetMeshObjectsInHierarchy(this GameObject root)
    {
      List<GameObject> allChildrenAndSelf = root.GetAllChildrenAndSelf();
      List<GameObject> objectsInHierarchy = new List<GameObject>(allChildrenAndSelf.Count);
      foreach (GameObject gameObject in allChildrenAndSelf)
      {
        if ((Object) gameObject.GetMesh() != (Object) null)
          objectsInHierarchy.Add(gameObject);
      }
      return objectsInHierarchy;
    }

    public static List<GameObject> GetSpriteObjectsInHierarchy(this GameObject root)
    {
      List<GameObject> allChildrenAndSelf = root.GetAllChildrenAndSelf();
      List<GameObject> objectsInHierarchy = new List<GameObject>(allChildrenAndSelf.Count);
      foreach (GameObject gameObject in allChildrenAndSelf)
      {
        if ((Object) gameObject.GetSprite() != (Object) null)
          objectsInHierarchy.Add(gameObject);
      }
      return objectsInHierarchy;
    }

    public static void SetHierarchyWorldScaleByPivot(
      this GameObject root,
      Vector3 worldScale,
      Vector3 pivotPoint)
    {
      if ((double) worldScale.x == 0.0)
        worldScale.x = 0.0001f;
      if ((double) worldScale.y == 0.0)
        worldScale.y = 0.0001f;
      if ((double) worldScale.z == 0.0)
        worldScale.z = 0.0001f;
      Transform transform = root.transform;
      Vector3 b = transform.position - pivotPoint;
      Vector3 lossyScale = transform.lossyScale;
      transform.SetWorldScale(worldScale);
      Vector3 inverse = lossyScale.GetInverse();
      Vector3 vector3 = Vector3.Scale(Vector3.Scale(worldScale, inverse), b);
      transform.position = pivotPoint + vector3;
    }

    public static List<GameObject> GetAllChildren(this GameObject gameObject)
    {
      Transform[] componentsInChildren = gameObject.GetComponentsInChildren<Transform>();
      List<GameObject> allChildren = new List<GameObject>(componentsInChildren.Length);
      foreach (Transform transform in componentsInChildren)
      {
        if ((Object) transform.gameObject != (Object) gameObject)
          allChildren.Add(transform.gameObject);
      }
      return allChildren;
    }

    public static List<GameObject> GetAllChildrenAndSelf(this GameObject gameObject)
    {
      Transform[] componentsInChildren = gameObject.GetComponentsInChildren<Transform>();
      List<GameObject> allChildrenAndSelf = new List<GameObject>(componentsInChildren.Length);
      foreach (Transform transform in componentsInChildren)
        allChildrenAndSelf.Add(transform.gameObject);
      return allChildrenAndSelf;
    }

    public static void GetAllChildrenAndSelf(
      this GameObject gameObject,
      List<GameObject> childrenAndSelf)
    {
      childrenAndSelf.Clear();
      GameObjectEx._transformsChildren.Clear();
      gameObject.GetComponentsInChildren<Transform>(GameObjectEx._transformsChildren);
      foreach (Transform transformsChild in GameObjectEx._transformsChildren)
        childrenAndSelf.Add(transformsChild.gameObject);
    }

    public static Mesh GetMesh(this GameObject gameObject)
    {
      MeshFilter component1 = gameObject.GetComponent<MeshFilter>();
      if ((Object) component1 != (Object) null && (Object) component1.sharedMesh != (Object) null)
        return component1.sharedMesh;
      SkinnedMeshRenderer component2 = gameObject.GetComponent<SkinnedMeshRenderer>();
      return (Object) component2 != (Object) null && (Object) component2.sharedMesh != (Object) null ? component2.sharedMesh : (Mesh) null;
    }

    public static Renderer GetMeshRenderer(this GameObject gameObject)
    {
      MeshRenderer component = gameObject.GetComponent<MeshRenderer>();
      return (Object) component != (Object) null ? (Renderer) component : (Renderer) gameObject.GetComponent<SkinnedMeshRenderer>();
    }

    public static Sprite GetSprite(this GameObject gameObject)
    {
      SpriteRenderer component = gameObject.GetComponent<SpriteRenderer>();
      return (Object) component == (Object) null ? (Sprite) null : component.sprite;
    }

    public static List<GameObject> GetRoots(IEnumerable<GameObject> gameObjects)
    {
      if (gameObjects == null)
        return new List<GameObject>();
      List<GameObject> roots = new List<GameObject>();
      HashSet<GameObject> gameObjectSet = new HashSet<GameObject>();
      foreach (GameObject gameObject1 in gameObjects)
      {
        GameObject gameObject2 = gameObject1.transform.root.gameObject;
        if (!gameObjectSet.Contains(gameObject2))
        {
          gameObjectSet.Add(gameObject2);
          roots.Add(gameObject2);
        }
      }
      gameObjectSet.Clear();
      return roots;
    }

    public static void FilterParentsOnly(
      IEnumerable<GameObject> gameObjects,
      List<GameObject> parents)
    {
      if (gameObjects == null)
        return;
      parents.Clear();
      foreach (GameObject gameObject1 in gameObjects)
      {
        bool flag = false;
        Transform transform = gameObject1.transform;
        foreach (GameObject gameObject2 in gameObjects)
        {
          if ((Object) gameObject2 != (Object) gameObject1 && transform.IsChildOf(gameObject2.transform))
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          parents.Add(gameObject1);
      }
    }

    public static List<GameObject> FilterParentsOnly(IEnumerable<GameObject> gameObjects)
    {
      if (gameObjects == null)
        return new List<GameObject>();
      List<GameObject> gameObjectList = new List<GameObject>(10);
      foreach (GameObject gameObject1 in gameObjects)
      {
        bool flag = false;
        Transform transform = gameObject1.transform;
        foreach (GameObject gameObject2 in gameObjects)
        {
          if ((Object) gameObject2 != (Object) gameObject1 && transform.IsChildOf(gameObject2.transform))
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          gameObjectList.Add(gameObject1);
      }
      return gameObjectList;
    }

    static GameObjectEx()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      GameObjectEx._transformsChildren = new List<Transform>();
    }
  }
}
