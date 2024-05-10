// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InstancedRendering.InstancingUtils
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InstancedRendering
{
  public static class InstancingUtils
  {
    private static readonly int MAIN_TEX_SHADER_ID;
    private static readonly int BUMP_MAP_SHADER_ID;
    private static readonly int METALLIC_GLOSS_MAP_SHADER_ID;
    private static readonly int EMISSION_MAP_SHADER_ID;
    private static readonly int EMISSION_COLOR_SHADER_ID;
    private static readonly int EMISSION_INTENSITY_SHADER_ID;
    [ThreadStatic]
    private static List<MeshRenderer> s_renderersTmp;
    [ThreadStatic]
    private static List<MeshFilter> s_meshFiltersTmp;
    [ThreadStatic]
    private static List<Collider> s_collidersTmp;

    public static Material InstantiateMaterialAndCopyTextures(
      Material materialToInstantiate,
      Material sourceOfTextures,
      bool ignoreMissingTextures = false)
    {
      Material material = UnityEngine.Object.Instantiate<Material>(materialToInstantiate);
      if (material.HasTexture(InstancingUtils.MAIN_TEX_SHADER_ID))
        material.SetTexture(InstancingUtils.MAIN_TEX_SHADER_ID, StandardMaterial.GetMainTexture(sourceOfTextures, ignoreMissingTextures));
      if (material.HasTexture(InstancingUtils.BUMP_MAP_SHADER_ID))
        material.SetTexture(InstancingUtils.BUMP_MAP_SHADER_ID, StandardMaterial.GetNormalTexture(sourceOfTextures, ignoreMissingTextures));
      if (material.HasTexture(InstancingUtils.METALLIC_GLOSS_MAP_SHADER_ID))
        material.SetTexture(InstancingUtils.METALLIC_GLOSS_MAP_SHADER_ID, StandardMaterial.GetMetallicGlossTexture(sourceOfTextures, ignoreMissingTextures));
      if (sourceOfTextures.HasTexture(InstancingUtils.EMISSION_MAP_SHADER_ID) && material.HasTexture(InstancingUtils.EMISSION_MAP_SHADER_ID))
      {
        material.SetTexture(InstancingUtils.EMISSION_MAP_SHADER_ID, StandardMaterial.GetEmissionTexture(sourceOfTextures, ignoreMissingTextures));
        material.EnableKeyword("ENABLE_EMISSION");
        Color color = sourceOfTextures.GetColor(InstancingUtils.EMISSION_COLOR_SHADER_ID);
        material.SetColor(InstancingUtils.EMISSION_COLOR_SHADER_ID, color);
        material.SetFloat(InstancingUtils.EMISSION_INTENSITY_SHADER_ID, color.a);
      }
      material.name = sourceOfTextures.name;
      return material;
    }

    public static bool TryGetMeshLodsAndMaterialFromPrefab(
      AssetsDb assetsDb,
      string prefabPath,
      out Mesh[] meshes,
      out Material sharedMaterial,
      out string error)
    {
      GameObject collidersGo = (GameObject) null;
      return InstancingUtils.TryGetMeshLodsAndMaterialFromPrefab(assetsDb, prefabPath, out meshes, out sharedMaterial, ref collidersGo, out error);
    }

    /// <summary>
    /// Extracts mesh LODs and material from given prefab paths. Returned mesh array has always size of
    /// <see cref="F:Mafi.Unity.Utils.LodUtils.LODS_COUNT" />. If there are not enough prefabs provided, the last valid mesh will be copied
    /// to the remaining LODs. Resulting array will not have any nulls in it.
    /// </summary>
    public static bool TryGetMeshLodsAndMaterialFromPrefab(
      AssetsDb assetsDb,
      string prefabPath,
      out Mesh[] meshes,
      out Material sharedMaterial,
      ref GameObject collidersGo,
      out string error)
    {
      sharedMaterial = (Material) null;
      if (string.IsNullOrEmpty(prefabPath))
      {
        meshes = (Mesh[]) null;
        error = "No prefabs given";
        return false;
      }
      GameObject prefab;
      if (!assetsDb.TryGetSharedPrefab(prefabPath, out prefab))
      {
        meshes = (Mesh[]) null;
        error = "Prefab '" + prefabPath + "' was not found.";
        return false;
      }
      meshes = new Mesh[7];
      if ((bool) (UnityEngine.Object) collidersGo)
      {
        List<Collider> results = InstancingUtils.s_collidersTmp ?? (InstancingUtils.s_collidersTmp = new List<Collider>());
        prefab.GetComponentsInChildren<Collider>(results);
        foreach (Collider collider in results)
        {
          switch (collider)
          {
            case BoxCollider boxCollider2:
              BoxCollider boxCollider1 = collidersGo.AddComponent<BoxCollider>();
              boxCollider1.center = boxCollider2.center;
              boxCollider1.size = boxCollider2.size;
              continue;
            case CapsuleCollider capsuleCollider2:
              CapsuleCollider capsuleCollider1 = collidersGo.AddComponent<CapsuleCollider>();
              capsuleCollider1.center = capsuleCollider2.center;
              capsuleCollider1.direction = capsuleCollider2.direction;
              capsuleCollider1.height = capsuleCollider2.height;
              capsuleCollider1.radius = capsuleCollider2.radius;
              continue;
            case SphereCollider sphereCollider2:
              SphereCollider sphereCollider1 = collidersGo.AddComponent<SphereCollider>();
              sphereCollider1.center = sphereCollider2.center;
              sphereCollider1.radius = sphereCollider2.radius;
              continue;
            default:
              Log.Error("Failed to copy prefab collider, unhandled type: " + collider.GetType().Name);
              continue;
          }
        }
      }
      int index;
      for (index = 0; index < 7; ++index)
      {
        GameObject resultGo;
        if (!prefab.TryFindChild("LOD" + index.ToString(), out resultGo))
        {
          if (index != 0)
            meshes[index] = meshes[index - 1];
          else
            break;
        }
        else
        {
          Mesh mesh;
          Material material;
          if (!resultGo.TryGetSharedMatMesh(true, out mesh, out material, out error))
          {
            if (index == 0)
            {
              meshes = (Mesh[]) null;
              error = "Error while loading prefab '" + prefabPath + "': " + error;
              return false;
            }
            Log.Warning(string.Format("Failed to load mesh/mat from LOD{0} of '{1}', skipping.", (object) index, (object) prefabPath));
            break;
          }
          if (index == 0)
            sharedMaterial = material;
          meshes[index] = mesh;
        }
      }
      if (index == 0)
      {
        Mesh mesh;
        if (!prefab.TryGetSharedMatMesh(true, out mesh, out sharedMaterial, out error))
        {
          meshes = (Mesh[]) null;
          error = "Error while loading prefab '" + prefabPath + "': " + error;
          return false;
        }
        meshes[0] = mesh;
        index = 1;
      }
      for (; index < meshes.Length; ++index)
        meshes[index] = meshes[index - 1];
      error = "";
      return true;
    }

    public static bool TryGetSharedMatMesh(
      this GameObject go,
      bool includeInactive,
      out Mesh mesh,
      out Material material,
      out string error)
    {
      mesh = (Mesh) null;
      material = (Material) null;
      List<MeshFilter> results1 = InstancingUtils.s_meshFiltersTmp ?? (InstancingUtils.s_meshFiltersTmp = new List<MeshFilter>());
      results1.Clear();
      go.GetComponentsInChildren<MeshFilter>(includeInactive, results1);
      if (results1.Count == 0)
      {
        error = "MeshFilter component was not found on '" + go.name + "'.";
        results1.Clear();
        return false;
      }
      List<MeshRenderer> results2 = InstancingUtils.s_renderersTmp ?? (InstancingUtils.s_renderersTmp = new List<MeshRenderer>());
      results2.Clear();
      go.GetComponentsInChildren<MeshRenderer>(includeInactive, results2);
      if (results2.Count == 0)
      {
        error = "MeshRenderer component was not found on '" + go.name + "'.";
        results1.Clear();
        return false;
      }
      bool flag = false;
      if (results2.Count > 1)
      {
        foreach (MeshRenderer meshRenderer in results2)
        {
          if ((UnityEngine.Object) meshRenderer.sharedMaterial != (UnityEngine.Object) results2[0].sharedMaterial || meshRenderer.sharedMaterials.Length > 1)
          {
            flag = true;
            break;
          }
        }
      }
      if (flag && results2.Count > 1)
      {
        error = "Too many materials on prefab '{0}'";
        results2.Clear();
        results1.Clear();
        return false;
      }
      if (results1.Count > 1)
      {
        foreach (MeshFilter meshFilter in results1)
        {
          if (!meshFilter.sharedMesh.isReadable)
          {
            error = "Unreadable mesh meshes on prefab '{0}'";
            results2.Clear();
            results1.Clear();
            return false;
          }
        }
      }
      if (results1.Count > 1 && results1.Count == results2.Count)
      {
        CombineInstance[] combine = new CombineInstance[results1.Count];
        for (int index = 0; index < results1.Count; ++index)
        {
          combine[index].mesh = results1[index].sharedMesh;
          combine[index].transform = results1[index].gameObject.transform.localToWorldMatrix;
        }
        mesh = new Mesh();
        mesh.name = go.name + "_" + results2[0].sharedMaterial.name;
        mesh.CombineMeshes(combine, true, true);
      }
      else
        mesh = results1[0].sharedMesh;
      material = results2[0].sharedMaterial;
      results1.Clear();
      results2.Clear();
      error = "";
      return true;
    }

    /// <summary>
    /// Extracts mesh LODs and materials from given prefab paths. Capable of dealing with prefabs which have multiple materials.
    /// Each returned mesh array has size of <see cref="F:Mafi.Unity.Utils.LodUtils.LODS_COUNT" />. If there are not enough LODs provided,
    /// the last valid mesh will be copied to the remaining LODs. Resulting array will not have any nulls in it.
    /// </summary>
    public static bool TryGetMeshLodsAndMaterialsFromPrefab(
      AssetsDb assetsDb,
      string prefabPath,
      ImmutableArray<ImmutableArray<KeyValuePair<string, IReadOnlySet<string>>>> animatedMeshes,
      ImmutableArray<string> excludedGameObjects,
      bool isStatic,
      out IReadOnlyDictionary<Material, Mesh[]> meshMatLods,
      ref GameObject collidersGo,
      out string error)
    {
      if (string.IsNullOrEmpty(prefabPath))
      {
        meshMatLods = (IReadOnlyDictionary<Material, Mesh[]>) null;
        error = "No prefabs given";
        return false;
      }
      GameObject prefab;
      if (!assetsDb.TryGetSharedPrefab(prefabPath, out prefab))
      {
        meshMatLods = (IReadOnlyDictionary<Material, Mesh[]>) null;
        error = "Prefab '" + prefabPath + "' was not found.";
        return false;
      }
      if ((bool) (UnityEngine.Object) collidersGo)
      {
        List<Collider> results = InstancingUtils.s_collidersTmp ?? (InstancingUtils.s_collidersTmp = new List<Collider>());
        prefab.GetComponentsInChildren<Collider>(results);
        foreach (Collider collider in results)
        {
          switch (collider)
          {
            case BoxCollider boxCollider2:
              BoxCollider boxCollider1 = collidersGo.AddComponent<BoxCollider>();
              boxCollider1.center = boxCollider2.center;
              boxCollider1.size = boxCollider2.size;
              continue;
            case CapsuleCollider capsuleCollider2:
              CapsuleCollider capsuleCollider1 = collidersGo.AddComponent<CapsuleCollider>();
              capsuleCollider1.center = capsuleCollider2.center;
              capsuleCollider1.direction = capsuleCollider2.direction;
              capsuleCollider1.height = capsuleCollider2.height;
              capsuleCollider1.radius = capsuleCollider2.radius;
              continue;
            case SphereCollider sphereCollider2:
              SphereCollider sphereCollider1 = collidersGo.AddComponent<SphereCollider>();
              sphereCollider1.center = sphereCollider2.center;
              sphereCollider1.radius = sphereCollider2.radius;
              continue;
            default:
              Log.Error("Failed to copy prefab collider, unhandled type: " + collider.GetType().Name);
              continue;
          }
        }
      }
      Dict<Material, Mesh[]> dict = new Dict<Material, Mesh[]>();
      LODGroup component = prefab.GetComponent<LODGroup>();
      int lodIndex = 0;
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        for (LOD[] lods = component.GetLODs(); lodIndex < lods.Length.Min(7); lodIndex++)
        {
          ImmutableArray<KeyValuePair<string, IReadOnlySet<string>>> animatedMeshes1 = lodIndex >= animatedMeshes.Length ? ImmutableArray<KeyValuePair<string, IReadOnlySet<string>>>.Empty : animatedMeshes[lodIndex];
          ImmutableArray<Pair<Mesh, Material>> meshMats;
          if (!prefab.TryGetSharedMatsMeshes(true, animatedMeshes1, excludedGameObjects, isStatic, out meshMats, out error, (Predicate<GameObject>) (go => ((IEnumerable<Renderer>) lods[lodIndex].renderers).Any<Renderer>((Func<Renderer, bool>) (r => (UnityEngine.Object) r.gameObject == (UnityEngine.Object) go)))))
          {
            if (lodIndex == 0)
            {
              meshMatLods = (IReadOnlyDictionary<Material, Mesh[]>) null;
              error = "Error while loading prefab '" + prefabPath + "': " + error;
              return false;
            }
            Log.Warning(string.Format("Failed to load mesh/mat from LOD{0} of '{1}', skipping.", (object) lodIndex, (object) prefabPath));
            break;
          }
          foreach (Pair<Mesh, Material> pair in meshMats)
          {
            Mesh[] meshArray;
            if (!dict.TryGetValue(pair.Second, out meshArray))
            {
              meshArray = new Mesh[7];
              for (int index = 0; index < lodIndex; ++index)
              {
                meshArray[index] = new Mesh();
                meshArray[index].name = string.Format("{0}_{1}_LOD{2}_empty", (object) prefab.name, (object) pair.Second.name, (object) index);
              }
              dict[pair.Second] = meshArray;
            }
            meshArray[lodIndex] = pair.First;
          }
          foreach (Mesh[] meshArray in dict.Values)
          {
            if ((UnityEngine.Object) meshArray[lodIndex] == (UnityEngine.Object) null)
            {
              meshArray[lodIndex] = new Mesh();
              meshArray[lodIndex].name = string.Format("{0}_LOD{1}_empty", (object) prefab.name, (object) lodIndex);
            }
          }
        }
      }
      else
      {
        GameObject resultGo;
        for (; lodIndex < 7 && prefab.TryFindChild("LOD" + lodIndex.ToString(), out resultGo); ++lodIndex)
        {
          if (!isStatic)
          {
            meshMatLods = (IReadOnlyDictionary<Material, Mesh[]>) null;
            error = "Error while loading prefab '" + prefabPath + "': animated LODed prefab with no LODGroup.";
            return false;
          }
          ImmutableArray<Pair<Mesh, Material>> meshMats;
          if (!resultGo.TryGetSharedMatsMeshes(true, animatedMeshes[lodIndex], excludedGameObjects, isStatic, out meshMats, out error))
          {
            if (lodIndex == 0)
            {
              meshMatLods = (IReadOnlyDictionary<Material, Mesh[]>) null;
              error = "Error while loading prefab '" + prefabPath + "': " + error;
              return false;
            }
            Log.Warning(string.Format("Failed to load mesh/mat from LOD{0} of '{1}', skipping.", (object) lodIndex, (object) prefabPath));
            break;
          }
          foreach (Pair<Mesh, Material> pair in meshMats)
          {
            Mesh[] meshArray;
            if (!dict.TryGetValue(pair.Second, out meshArray))
            {
              if (lodIndex != 0)
              {
                meshMatLods = (IReadOnlyDictionary<Material, Mesh[]>) null;
                error = "Error while loading prefab '" + prefabPath + "': new material on non-first LOD";
                return false;
              }
              meshArray = new Mesh[7];
              dict[pair.Second] = meshArray;
            }
            meshArray[lodIndex] = pair.First;
          }
        }
        if (lodIndex == 0)
        {
          ImmutableArray<Pair<Mesh, Material>> meshMats;
          if (!prefab.TryGetSharedMatsMeshes(true, animatedMeshes[0], excludedGameObjects, isStatic, out meshMats, out error))
          {
            meshMatLods = (IReadOnlyDictionary<Material, Mesh[]>) null;
            error = "Error while loading prefab '" + prefabPath + "': " + error;
            return false;
          }
          foreach (Pair<Mesh, Material> pair in meshMats)
          {
            Mesh[] meshArray;
            if (!dict.TryGetValue(pair.Second, out meshArray))
            {
              meshArray = new Mesh[7];
              dict[pair.Second] = meshArray;
            }
            meshArray[lodIndex] = pair.First;
          }
          lodIndex = 1;
        }
      }
      for (; lodIndex < 7; ++lodIndex)
      {
        foreach (Material key in dict.Keys)
          dict[key][lodIndex] = dict[key][lodIndex - 1];
      }
      meshMatLods = (IReadOnlyDictionary<Material, Mesh[]>) dict;
      error = "";
      return true;
    }

    public static bool TryGetSharedMatsMeshes(
      this GameObject go,
      bool includeInactive,
      ImmutableArray<KeyValuePair<string, IReadOnlySet<string>>> animatedMeshes,
      ImmutableArray<string> excludedGameObjects,
      bool isStatic,
      out ImmutableArray<Pair<Mesh, Material>> meshMats,
      out string error,
      Predicate<GameObject> childPredicate = null)
    {
      meshMats = (ImmutableArray<Pair<Mesh, Material>>) (ImmutableArray.EmptyArray) null;
      Lyst<GameObject> outGos = new Lyst<GameObject>();
      if (childIsValid(go))
        outGos.Add(go);
      go.GetAllChildren(new Predicate<GameObject>(childIsValid), outGos);
      if (outGos.Count == 0)
      {
        error = "'" + go.name + "' appears not to be rendering anything.";
        return false;
      }
      Lyst<Material> lyst1 = new Lyst<Material>();
      Dict<Material, Lyst<Pair<Transform, Mesh>>> dict = new Dict<Material, Lyst<Pair<Transform, Mesh>>>();
      Set<string> set = new Set<string>();
      foreach (KeyValuePair<string, IReadOnlySet<string>> animatedMesh in animatedMeshes)
        set.AddRange((IEnumerable<string>) animatedMesh.Value);
      bool flag = true;
      foreach (GameObject go1 in outGos)
      {
        if (set.Contains(go1.name) != isStatic)
        {
          if (go1.HasTag(UnityTag.NoHi) || go1.HasTag(UnityTag.NoOnlyHi))
          {
            error = string.Format("'{0}' has an unsupported highlighting tag.", (object) go1);
            return false;
          }
          SkinnedMeshRenderer component1 = go1.GetComponent<SkinnedMeshRenderer>();
          if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
          {
            Mesh sharedMesh = component1.sharedMesh;
            if (!sharedMesh.isReadable)
            {
              if (outGos.Count > 1)
              {
                error = "'" + go.name + "' has unreadable meshes and multiple meshes on a material.";
                return false;
              }
              flag = false;
            }
            Assert.That<int>(sharedMesh.subMeshCount).IsEqualTo(1);
            Assert.That<int>(sharedMesh.vertices.Length).IsEqualTo(sharedMesh.normals.Length);
            Mesh mesh = new Mesh();
            component1.BakeMesh(mesh, true);
            foreach (Material sharedMaterial in component1.sharedMaterials)
            {
              if (dict.ContainsKey(sharedMaterial))
              {
                dict[sharedMaterial].Add(new Pair<Transform, Mesh>(go1.transform, mesh));
              }
              else
              {
                dict[sharedMaterial] = new Lyst<Pair<Transform, Mesh>>()
                {
                  new Pair<Transform, Mesh>(go1.transform, mesh)
                };
                lyst1.AddAssertNew(sharedMaterial);
              }
            }
          }
          else
          {
            MeshRenderer component2 = go1.GetComponent<MeshRenderer>();
            MeshFilter component3 = component2.GetComponent<MeshFilter>();
            if ((UnityEngine.Object) component3.sharedMesh == (UnityEngine.Object) null)
            {
              Log.Warning("Mesh filter with no mesh assigned on '" + go.name + "'.");
            }
            else
            {
              if (!component3.sharedMesh.isReadable)
              {
                if (outGos.Count > 1)
                {
                  error = "'" + go.name + "' has unreadable meshes and multiple meshes on a material.";
                  return false;
                }
                flag = false;
              }
              foreach (Material sharedMaterial in component2.sharedMaterials)
              {
                if (dict.ContainsKey(sharedMaterial))
                {
                  dict[sharedMaterial].Add(new Pair<Transform, Mesh>(go1.transform, component3.sharedMesh));
                }
                else
                {
                  dict[sharedMaterial] = new Lyst<Pair<Transform, Mesh>>()
                  {
                    new Pair<Transform, Mesh>(go1.transform, component3.sharedMesh)
                  };
                  lyst1.AddAssertNew(sharedMaterial);
                }
              }
            }
          }
        }
      }
      Lyst<Pair<Mesh, Material>> lyst2 = new Lyst<Pair<Mesh, Material>>();
      if (outGos.Count > 1 | flag)
      {
        foreach (Material material in lyst1)
        {
          CombineInstance[] combine = new CombineInstance[dict[material].Count];
          for (int index = 0; index < dict[material].Count; ++index)
          {
            combine[index].mesh = dict[material][index].Second;
            combine[index].transform = dict[material][index].First.localToWorldMatrix;
          }
          Mesh first = new Mesh();
          first.name = go.name + "_" + material.name;
          first.CombineMeshes(combine, true, true);
          lyst2.Add(new Pair<Mesh, Material>(first, material));
        }
      }
      else
      {
        if ((UnityEngine.Object) go.transform.parent != (UnityEngine.Object) null)
          Log.Warning("Instancing non-readable meshes for '" + go.name + "' parent '" + go.transform.parent.name + "'");
        else
          Log.Warning("Instancing non-readable meshes for '" + go.name + "'");
        foreach (GameObject gameObject in outGos)
        {
          if (gameObject.transform.localRotation != Quaternion.identity)
          {
            error = "'" + go.name + "' is not readable and has rotation.";
            return false;
          }
          if (gameObject.transform.localScale != Vector3.one)
          {
            error = "'" + go.name + "' is not readable and has scale.";
            return false;
          }
          if (gameObject.transform.localPosition != Vector3.zero)
          {
            error = "'" + go.name + "' is not readable and has translation.";
            return false;
          }
        }
        foreach (Material material in lyst1)
        {
          foreach (Pair<Transform, Mesh> pair in dict[material])
            lyst2.Add(new Pair<Mesh, Material>(pair.Second, material));
        }
      }
      meshMats = lyst2.ToImmutableArrayAndClear();
      error = "";
      return true;

      bool childIsValid(GameObject child)
      {
        if (child.IsNullOrDestroyed() || childPredicate != null && !childPredicate(child) || excludedGameObjects.Contains(child.name))
          return false;
        return (UnityEngine.Object) child.GetComponent<MeshRenderer>() != (UnityEngine.Object) null || (UnityEngine.Object) child.GetComponent<SkinnedMeshRenderer>() != (UnityEngine.Object) null;
      }
    }

    static InstancingUtils()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      InstancingUtils.MAIN_TEX_SHADER_ID = Shader.PropertyToID("_MainTex");
      InstancingUtils.BUMP_MAP_SHADER_ID = Shader.PropertyToID("_BumpMap");
      InstancingUtils.METALLIC_GLOSS_MAP_SHADER_ID = Shader.PropertyToID("_MetallicGlossMap");
      InstancingUtils.EMISSION_MAP_SHADER_ID = Shader.PropertyToID("_EmissionMap");
      InstancingUtils.EMISSION_COLOR_SHADER_ID = Shader.PropertyToID("_EmissionColor");
      InstancingUtils.EMISSION_INTENSITY_SHADER_ID = Shader.PropertyToID("_EmissionIntensity");
    }
  }
}
