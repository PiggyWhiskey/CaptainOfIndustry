// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.GameObjectExtensions
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity
{
  public static class GameObjectExtensions
  {
    public const string DESTROYED_NAME = "DESTROYED";
    private static readonly List<Collider> s_collidersTemp;
    [ThreadStatic]
    private static List<Renderer> m_renderersTmp;

    /// <summary>
    /// Destroys all colliders from this Game Object. This method is not recursive. Not thread safe!
    /// </summary>
    public static GameObject DestroyAllCollidersImmediate(this GameObject go)
    {
      go.GetComponents<Collider>(GameObjectExtensions.s_collidersTemp);
      Assert.That<int>(GameObjectExtensions.s_collidersTemp.Count).IsPositive("No colliders to destroy.");
      foreach (UnityEngine.Object @object in GameObjectExtensions.s_collidersTemp)
        UnityEngine.Object.DestroyImmediate(@object);
      return go;
    }

    /// <summary>
    /// Destroys this Game Object. Keep in mind that actual destruction is delayed, so for example cloning the GO
    /// after destroying a component will still have the deleted component.
    /// </summary>
    public static void Destroy(this GameObject go)
    {
      if (go.IsNullOrDestroyed())
        return;
      go.name = "DESTROYED";
      UnityEngine.Object.Destroy((UnityEngine.Object) go);
    }

    /// <summary>Destroys this Game Object immediately.</summary>
    public static void DestroyImmediate(this GameObject go) => UnityEngine.Object.DestroyImmediate((UnityEngine.Object) go);

    /// <summary>
    /// Checks if a GameObject has been destroyed or marked as destroyed.
    /// </summary>
    public static bool IsNullOrDestroyed(this GameObject go)
    {
      return !(bool) (UnityEngine.Object) go || go.name == "DESTROYED";
    }

    public static T GetComponentOrCreateNewAndLogError<T>(this GameObject go) where T : Component
    {
      T component;
      if (go.TryGetComponent<T>(out component))
        return component;
      Log.Error("Failed to find component '" + typeof (T).Name + "' on game object '" + go.name + "'.");
      return go.AddComponent<T>();
    }

    /// <summary>
    /// Recursively gets all children that satisfies given predicate.
    /// </summary>
    public static void GetAllChildren(
      this GameObject go,
      Predicate<GameObject> predicate,
      Lyst<GameObject> outGos)
    {
      Transform transform = go.transform;
      for (int index = 0; index < transform.childCount; ++index)
      {
        GameObject gameObject = transform.GetChild(index).gameObject;
        if (predicate(gameObject))
          outGos.Add(gameObject);
        gameObject.GetAllChildren(predicate, outGos);
      }
    }

    /// <summary>
    /// Recursively gets the first child that satisfies given predicate.
    /// </summary>
    public static GameObject GetFirstChildOrDefault(
      this GameObject go,
      Predicate<GameObject> predicate)
    {
      Transform transform = go.transform;
      for (int index = 0; index < transform.childCount; ++index)
      {
        GameObject gameObject = transform.GetChild(index).gameObject;
        if (predicate(gameObject))
          return gameObject;
      }
      return (GameObject) null;
    }

    /// <summary>
    /// Recursively gets all components in this object and all children.
    /// </summary>
    public static void GetAllComponentsInHierarchy<T>(this GameObject go, Lyst<T> outComponents)
    {
      T component;
      if (go.TryGetComponent<T>(out component))
        outComponents.Add(component);
      Transform transform = go.transform;
      for (int index = 0; index < transform.childCount; ++index)
        transform.GetChild(index).gameObject.GetAllComponentsInHierarchy<T>(outComponents);
    }

    public static void SetActiveAllChildren(this GameObject go, bool isActive)
    {
      if (go.IsNullOrDestroyed())
      {
        Assert.Fail("The object of type 'GameObject' has been destroyed but you are still trying to access it.");
      }
      else
      {
        Transform transform = go.transform;
        for (int index = 0; index < transform.childCount; ++index)
          transform.GetChild(index).gameObject.SetActive(isActive);
      }
    }

    /// <summary>
    /// Sets layer of this <see cref="T:UnityEngine.GameObject" /> and all its children.
    /// </summary>
    public static void SetLayerRecursively(this GameObject go, int layer)
    {
      go.layer = layer;
      foreach (Component component in go.transform)
        component.gameObject.SetLayerRecursively(layer);
    }

    public static void SetSharedMaterialRecursively(this GameObject go, Material material)
    {
      if (GameObjectExtensions.m_renderersTmp == null)
        GameObjectExtensions.m_renderersTmp = new List<Renderer>();
      GameObjectExtensions.m_renderersTmp.Clear();
      go.GetComponentsInChildren<Renderer>(GameObjectExtensions.m_renderersTmp);
      foreach (Renderer renderer in GameObjectExtensions.m_renderersTmp)
        renderer.sharedMaterial = material;
      GameObjectExtensions.m_renderersTmp.Clear();
    }

    /// <summary>Tries to find a child by name recursively.</summary>
    public static bool TryFindNameInHierarchy(this GameObject go, string id, out GameObject result)
    {
      if (!(bool) (UnityEngine.Object) go)
      {
        result = (GameObject) null;
        return false;
      }
      if (go.name == id)
      {
        result = go;
        return true;
      }
      Transform transform = go.transform;
      for (int index = 0; index < transform.childCount; ++index)
      {
        if (transform.GetChild(index).gameObject.TryFindNameInHierarchy(id, out result))
          return true;
      }
      result = (GameObject) null;
      return false;
    }

    /// <summary>
    /// Tries to find a child by name. Does not perform a recursive descend, but if the given name contains
    /// a '/' character it will access the Transform in the hierarchy like a path name.
    /// </summary>
    public static bool TryFindChild(this GameObject go, string path, out GameObject resultGo)
    {
      Transform transform = go.transform.Find(path);
      if ((bool) (UnityEngine.Object) transform)
      {
        resultGo = transform.gameObject;
        return true;
      }
      resultGo = (GameObject) null;
      return false;
    }

    /// <summary>
    /// Tries to find a child by name. Does not perform a recursive descend, but if the given name contains
    /// a '/' character it will access the Transform in the hierarchy like a path name.
    /// </summary>
    public static bool TryFindChild(
      this Transform baseTransform,
      string path,
      out Transform resultTransform)
    {
      resultTransform = baseTransform.Find(path);
      return (bool) (UnityEngine.Object) resultTransform;
    }

    /// <summary>
    /// Makes a copy of each unique material on this object. If one material is used multiple times, there will be
    /// only once copy which will be also used multiple times.
    /// </summary>
    public static Lyst<MeshRenderer> InstantiateMaterials(
      this GameObject go,
      Predicate<MeshRenderer> predicate = null,
      Lyst<Material> outInstantiatedMaterials = null)
    {
      Lyst<MeshRenderer> lyst = new Lyst<MeshRenderer>();
      go.GetAllComponentsInHierarchy<MeshRenderer>(lyst);
      lyst.RemoveAll((Predicate<MeshRenderer>) (x =>
      {
        if ((UnityEngine.Object) x.sharedMaterial == (UnityEngine.Object) null)
          return true;
        Predicate<MeshRenderer> predicate1 = predicate;
        return (predicate1 != null ? (predicate1(x) ? 1 : 0) : 1) == 0;
      }));
      if (lyst.Count == 0)
        return lyst;
      Material sharedMaterial = lyst[0].sharedMaterial;
      bool flag = true;
      for (int index = 1; index < lyst.Count; ++index)
      {
        if ((UnityEngine.Object) lyst[index].sharedMaterial != (UnityEngine.Object) sharedMaterial)
        {
          flag = false;
          break;
        }
      }
      if (flag)
      {
        Material material = UnityEngine.Object.Instantiate<Material>(sharedMaterial);
        outInstantiatedMaterials?.Add(material);
        foreach (Renderer renderer in lyst)
          renderer.sharedMaterial = material;
        return lyst;
      }
      foreach (IGrouping<Material, MeshRenderer> grouping in lyst.GroupBy<MeshRenderer, Material>((Func<MeshRenderer, Material>) (x => x.sharedMaterial)))
      {
        Material material = UnityEngine.Object.Instantiate<Material>(grouping.Key);
        outInstantiatedMaterials?.Add(material);
        foreach (Renderer renderer in (IEnumerable<MeshRenderer>) grouping)
          renderer.sharedMaterial = material;
      }
      return lyst;
    }

    public static bool TestAnimatorValid(this Animator animator)
    {
      try
      {
        float num = animator.speed++;
        animator.speed = num;
      }
      catch
      {
        Log.Error("Found invalid animator on game object '" + animator.gameObject.name + "'.");
        return false;
      }
      return true;
    }

    /// <summary>
    /// Traverses all children of this GO and combines all meshes based on their material.
    /// All children will be then deleted.
    /// </summary>
    public static void CombineMeshesWithSameMaterialRecursively(
      this GameObject gameObject,
      out int originalMeshesCount,
      out int originalGameObjects,
      out int mewMeshesCount)
    {
      Dict<Material, Lyst<CombineInstance>> dict = new Dict<Material, Lyst<CombineInstance>>();
      Stak<Transform> stak = new Stak<Transform>();
      Transform transform1 = gameObject.transform;
      stak.Push(transform1);
      originalGameObjects = 0;
      while (stak.IsNotEmpty)
      {
        Transform transform2 = stak.Pop();
        GameObject gameObject1 = transform2.gameObject;
        ++originalGameObjects;
        MeshFilter component1;
        MeshRenderer component2;
        if (gameObject1.TryGetComponent<MeshFilter>(out component1) && gameObject1.TryGetComponent<MeshRenderer>(out component2))
        {
          Lyst<CombineInstance> lyst;
          if (!dict.TryGetValue(component2.sharedMaterial, out lyst))
          {
            lyst = new Lyst<CombineInstance>();
            dict.Add(component2.sharedMaterial, lyst);
          }
          CombineInstance combineInstance = new CombineInstance()
          {
            mesh = component1.sharedMesh,
            transform = transform2.localToWorldMatrix
          };
          lyst.Add(combineInstance);
        }
        int childCount = transform2.childCount;
        for (int index = 0; index < childCount; ++index)
          stak.Push(transform2.GetChild(index));
      }
      GameObject[] gameObjectArray = new GameObject[dict.Count];
      int num = 0;
      originalMeshesCount = 0;
      mewMeshesCount = dict.Count;
      foreach (KeyValuePair<Material, Lyst<CombineInstance>> keyValuePair in dict)
      {
        originalMeshesCount += keyValuePair.Value.Count;
        GameObject gameObject2 = new GameObject(keyValuePair.Key.name);
        Mesh mesh = new Mesh();
        if (keyValuePair.Value.Sum<CombineInstance>((Func<CombineInstance, int>) (x => (int) x.mesh.GetIndexCount(0))) > (int) ushort.MaxValue)
          mesh.indexFormat = IndexFormat.UInt32;
        mesh.CombineMeshes(keyValuePair.Value.ToArray());
        gameObject2.AddComponent<MeshFilter>().sharedMesh = mesh;
        gameObject2.AddComponent<MeshRenderer>().sharedMaterial = keyValuePair.Key;
        gameObjectArray[num++] = gameObject2;
      }
      for (int index = transform1.childCount - 1; index >= 0; --index)
        UnityEngine.Object.Destroy((UnityEngine.Object) transform1.GetChild(index).gameObject);
      foreach (GameObject gameObject3 in gameObjectArray)
        gameObject3.transform.SetParent(transform1, false);
    }

    public static Bounds ComputeMaxBounds(this GameObject go)
    {
      Renderer[] componentsInChildren = go.GetComponentsInChildren<Renderer>();
      if (componentsInChildren.Length == 0)
        return new Bounds(go.transform.position, Vector3.zero);
      Bounds bounds = componentsInChildren[0].bounds;
      for (int index = 1; index < componentsInChildren.Length; ++index)
      {
        Renderer renderer = componentsInChildren[index];
        bounds.Encapsulate(renderer.bounds);
      }
      return bounds;
    }

    public static Bounds ExtendMaxBounds(this GameObject go, Bounds bounds)
    {
      foreach (Renderer componentsInChild in go.GetComponentsInChildren<Renderer>())
        bounds.Encapsulate(componentsInChild.bounds);
      return bounds;
    }

    public static Bounds ComputeMaxBounds(this IEnumerable<GameObject> gos)
    {
      bool flag = true;
      Bounds maxBounds = new Bounds();
      foreach (GameObject go in gos)
      {
        if (flag)
        {
          maxBounds = go.ComputeMaxBounds();
          flag = false;
        }
        else
          maxBounds.Encapsulate(go.ComputeMaxBounds());
      }
      return maxBounds;
    }

    public static int ComputeTriangleCount(this GameObject go)
    {
      MeshFilter[] componentsInChildren = go.GetComponentsInChildren<MeshFilter>();
      if (componentsInChildren.Length == 0)
        return 0;
      uint num = 0;
      foreach (MeshFilter meshFilter in componentsInChildren)
      {
        Mesh sharedMesh = meshFilter.sharedMesh;
        for (int submesh = 0; submesh < sharedMesh.subMeshCount; ++submesh)
          num += meshFilter.sharedMesh.GetIndexCount(submesh);
      }
      return (int) (num / 3U);
    }

    static GameObjectExtensions()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      GameObjectExtensions.s_collidersTemp = new List<Collider>();
    }
  }
}
