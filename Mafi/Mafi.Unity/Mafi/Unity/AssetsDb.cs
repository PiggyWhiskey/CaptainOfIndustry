// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.AssetsDb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Unity.UiFramework.Styles;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.TextCore.Text;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity
{
  public sealed class AssetsDb : IDisposable
  {
    public const string TODO_STR = "TODO";
    public const string EMPTY_STR = "EMPTY";
    public const string ASSETS_ROOT_DIR_NAME = "Assets";
    /// <summary>
    /// Returns "Standard" material. This should be used only for debugging purposes. All materials should be loaded
    /// from assets.
    /// </summary>
    public readonly Material DefaultMaterial;
    public readonly Texture2D DefaultTexture;
    public readonly Mesh DefaultMesh;
    public readonly GameObject DefaultPrefab;
    public readonly GameObject EmptyGo;
    private readonly AssetBundleLoader m_bundleLoader;
    private readonly Set<string> m_registeredAssetsDirs;
    private Option<IAlternativeAssetLoader> m_dynamicAssetLoader;
    private readonly Dict<string, Sprite> m_spriteCache;

    /// <summary>
    /// Note that this may contain duplicate object for normal and lowercase key paths.
    /// </summary>
    public IReadOnlyDictionary<string, UnityEngine.Object> LoadedAssets
    {
      get => this.m_bundleLoader.LoadedAssets;
    }

    public AssetsDb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_bundleLoader = new AssetBundleLoader();
      this.m_registeredAssetsDirs = new Set<string>();
      this.m_spriteCache = new Dict<string, Sprite>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.EmptyGo = new GameObject("Empty GO");
      Texture2D texture2D = new Texture2D(8, 8, TextureFormat.RGBA32, false);
      texture2D.name = "FallbackTexture";
      this.DefaultTexture = texture2D;
      this.DefaultMesh = new Mesh();
      Shader shader = Shader.Find("Standard");
      this.DefaultMaterial = !((UnityEngine.Object) shader == (UnityEngine.Object) null) ? new Material(shader)
      {
        mainTexture = (Texture) this.DefaultTexture
      } : throw new ApplicationException("Standard shader was not found!");
      this.DefaultPrefab = new GameObject("MissingPrefab");
      MeshBuilder instance = MeshBuilder.Instance;
      instance.AddAaBox(Vector3.zero, Vector3.one, new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
      instance.UpdateGoAndClear(this.DefaultPrefab);
      this.DefaultPrefab.GetComponent<MeshRenderer>().sharedMaterial = this.DefaultMaterial;
      this.DefaultPrefab.SetActive(false);
    }

    /// <summary>
    /// Registers all asset bundles in the given directory. Does nothing if the given dir path is already
    /// registered.
    /// </summary>
    public void RegisterAssetBundlesIn(string bundlesRootDirPath, string coreBundlesDirPath)
    {
      if (!Directory.Exists(bundlesRootDirPath))
      {
        Log.Error("Assets directory does not exist: " + bundlesRootDirPath);
      }
      else
      {
        if (!this.m_registeredAssetsDirs.Add(bundlesRootDirPath))
          return;
        this.m_bundleLoader.RegisterAssetBundlesIn(bundlesRootDirPath, coreBundlesDirPath);
      }
    }

    public void SetAssetOverrideRootPath(string path)
    {
      this.m_bundleLoader.SetAssetOverrideRootPath(path);
    }

    public void SetAlternativeAssetLoader(IAlternativeAssetLoader loader)
    {
      this.m_bundleLoader.SetAlternativeAssetLoader(loader);
    }

    /// <summary>Forces loading of all registered bundles.</summary>
    internal void LoadAllRegisteredAssets() => this.m_bundleLoader.LoadAllRegisteredAssets();

    public bool ContainsAsset(string assetPath) => this.m_bundleLoader.ContainsAsset(assetPath);

    /// <summary>
    /// Retrieves asset at given <paramref name="assetPath" /> as <typeparamref name="T" />. Returns true is asset was
    /// found. Returned instance is a reference to global asset database. Make yourself a copy if you want to change
    /// it.
    /// </summary>
    public bool TryGetSharedAsset<T>(
      string assetPath,
      out T result,
      bool suppressErrors = false,
      bool doNotCache = false)
      where T : UnityEngine.Object
    {
      if (assetPath == null)
      {
        Log.Error("Null asset path for asset of type '" + typeof (T).Name + "'.");
        result = default (T);
        return false;
      }
      T asset;
      if (!this.m_bundleLoader.TryGetAsset<T>(assetPath, doNotCache, out asset))
      {
        if (!suppressErrors)
          Log.Error("Asset '" + Path.GetFileName(assetPath) + "' (" + typeof (T).Name + ") was not found in any bundle, path: " + assetPath);
        result = default (T);
        return false;
      }
      if (!(bool) (UnityEngine.Object) asset)
      {
        if (!suppressErrors)
          Log.Error("Loaded asset is invalid game object! Someone destroyed shared asset? Requested: " + typeof (T).FullName + ", path: " + assetPath);
        result = default (T);
        return false;
      }
      result = asset;
      if (!((UnityEngine.Object) result == (UnityEngine.Object) null))
        return true;
      if (!suppressErrors)
        Log.Error("Loaded asset has incorrect type. Requested: " + typeof (T).FullName + ", actual: " + asset.GetType().FullName + ", path: " + assetPath);
      return false;
    }

    /// <summary>
    /// Returns asset at given path but throws <see cref="T:Mafi.Unity.AssetsDbException" /> if asset is not found. Use this only
    /// during game initialization when exception is actually wanted, otherwise use
    /// <see cref="M:Mafi.Unity.AssetsDb.TryGetSharedAsset``1(System.String,``0@,System.Boolean,System.Boolean)" />.
    /// </summary>
    public T GetSharedAssetOrThrow<T>(string assetPath) where T : UnityEngine.Object
    {
      T result;
      if (this.TryGetSharedAsset<T>(assetPath, out result))
        return result;
      throw new AssetsDbException("Failed to find asset of type " + typeof (T).Name + " at " + assetPath);
    }

    /// <summary>
    /// Returns a shared <see cref="T:UnityEngine.Material" /> from DB. Returned material is not cloned and will be shared with all
    /// objects. If you wish to change properties of this material please use <see cref="M:Mafi.Unity.AssetsDb.GetClonedMaterial(System.String)" />.
    /// 
    /// If requested material does not exist, <see cref="F:Mafi.Unity.AssetsDb.DefaultMaterial" /> is returned.
    /// 
    /// Note that assigning <see cref="P:UnityEngine.Renderer.material" /> will make a copy but
    /// <see cref="P:UnityEngine.Renderer.sharedMaterial" /> will not.
    /// </summary>
    public Material GetSharedMaterial(string materialAssetPath)
    {
      Material result;
      return !this.TryGetSharedAsset<Material>(materialAssetPath, out result) ? this.DefaultMaterial : result;
    }

    /// <summary>
    /// Returns a clone of <see cref="T:UnityEngine.Material" /> from DB. Use this if you wish to change properties of this
    /// material. Otherwise, if you do not want to change this material consider using <see cref="M:Mafi.Unity.AssetsDb.GetSharedMaterial(System.String)" /> that will not make a copy.
    /// 
    /// If requested material does not exist, <see cref="F:Mafi.Unity.AssetsDb.DefaultMaterial" /> is returned.
    /// </summary>
    public Material GetClonedMaterial(string materialAssetPath)
    {
      Material result;
      return UnityEngine.Object.Instantiate<Material>(this.TryGetSharedAsset<Material>(materialAssetPath, out result) ? result : this.DefaultMaterial);
    }

    /// <summary>
    /// Returns a shared <see cref="T:UnityEngine.Texture2D" />
    /// </summary>
    public Texture2D GetSharedTexture(string textureAssetPath)
    {
      Texture2D result;
      return !this.TryGetSharedAsset<Texture2D>(textureAssetPath, out result) ? this.DefaultTexture : result;
    }

    /// <summary>Returns shared sprite resource from the given path.</summary>
    /// <param name="spriteAssetPath">Path to the sprite.</param>
    /// <param name="border">Border if the sprite is sliced otherwise null.</param>
    public Sprite GetSharedSprite(string spriteAssetPath, Vector4? border = null)
    {
      Sprite sharedSprite1;
      if (this.m_spriteCache.TryGetValue(spriteAssetPath, out sharedSprite1))
        return sharedSprite1;
      Texture2D result;
      if (!this.TryGetSharedAsset<Texture2D>(spriteAssetPath, out result, true) && !this.TryGetSharedAsset<Texture2D>("Assets/Unity/UserInterface/General/ComingSoon.svg", out result))
        result = this.DefaultTexture;
      Rect rect = new Rect(0.0f, 0.0f, (float) result.width, (float) result.height);
      Vector2 pivot = new Vector2((float) result.width / 2f, (float) result.height / 2f);
      Sprite sharedSprite2 = !border.HasValue ? Sprite.Create(result, rect, pivot) : Sprite.Create(result, rect, pivot, 100f, 0U, SpriteMeshType.FullRect, border.Value);
      this.m_spriteCache.Add(spriteAssetPath, sharedSprite2);
      return sharedSprite2;
    }

    public Sprite GetSharedSprite(SlicedSpriteStyle slicedSprite)
    {
      return this.GetSharedSprite(slicedSprite.IconAssetPath, new Vector4?(slicedSprite.SliceBorder));
    }

    public Font GetSharedFont(string fontAssetPath)
    {
      Font result;
      if (this.TryGetSharedAsset<Font>(fontAssetPath, out result))
        return result;
      Log.Warning("Font on path '" + fontAssetPath + "' was not found");
      return UnityEngine.Resources.GetBuiltinResource<Font>("Arial.ttf");
    }

    public Option<FontAsset> GetSharedFontAsset(string fontAssetPath)
    {
      FontAsset result;
      if (this.TryGetSharedAsset<FontAsset>(fontAssetPath, out result))
        return (Option<FontAsset>) result;
      Log.Warning("Font on path '" + fontAssetPath + "' was not found");
      return (Option<FontAsset>) Option.None;
    }

    /// <summary>
    /// Returns stored prefab in the DB. Any modifications will mutate the stored prefab. Use with caution!
    /// </summary>
    public bool TryGetSharedPrefab(string prefabPath, out GameObject prefab, bool suppressErrors = false)
    {
      return this.TryGetSharedAsset<GameObject>(prefabPath, out prefab, suppressErrors);
    }

    public Option<GameObject> GetSharedPrefab(Option<string> prefabPath)
    {
      GameObject prefab;
      return prefabPath.IsNone || !this.TryGetSharedPrefab(prefabPath.Value, out prefab) ? Option<GameObject>.None : (Option<GameObject>) prefab;
    }

    public GameObject GetSharedPrefabOrEmptyGo(string prefabPath)
    {
      GameObject prefab;
      return this.TryGetSharedPrefab(prefabPath, out prefab) ? prefab : this.EmptyGo;
    }

    public GameObject GetSharedPrefabOrThrow(string prefabPath)
    {
      GameObject prefab;
      if (this.TryGetSharedPrefab(prefabPath, out prefab))
        return prefab;
      throw new AssetsDbException("Prefab was not found: " + prefabPath);
    }

    public bool TryGetClonedPrefab(string prefabPath, out GameObject prefab)
    {
      GameObject prefab1;
      if (this.TryGetSharedPrefab(prefabPath, out prefab1))
      {
        prefab = UnityEngine.Object.Instantiate<GameObject>(prefab1);
        return true;
      }
      prefab = (GameObject) null;
      return false;
    }

    public GameObject GetClonedPrefabOrEmptyGo(string prefabPath)
    {
      GameObject prefab;
      if (!this.TryGetSharedPrefab(prefabPath, out prefab))
        return new GameObject(prefabPath);
      GameObject clonedPrefabOrEmptyGo = UnityEngine.Object.Instantiate<GameObject>(prefab);
      clonedPrefabOrEmptyGo.SetActive(true);
      return clonedPrefabOrEmptyGo;
    }

    public void ClearCachedAssets()
    {
      this.m_spriteCache.Clear();
      this.m_bundleLoader.ClearCachedAssets();
    }

    public void Dispose() => this.m_bundleLoader.Dispose();

    public Mesh GetMeshFromAsset(string assetPath)
    {
      GameObject result;
      if (!this.TryGetSharedAsset<GameObject>(assetPath, out result))
        result = this.DefaultPrefab;
      Mesh meshFromAsset = (Mesh) null;
      MeshFilter component;
      if (result.TryGetComponent<MeshFilter>(out component))
        meshFromAsset = component.sharedMesh;
      if (!(bool) (UnityEngine.Object) meshFromAsset)
      {
        Log.Error(string.Format("Missing a mesh on prefab '{0}'.", (object) result));
        meshFromAsset = this.DefaultPrefab.GetComponent<MeshFilter>().sharedMesh;
      }
      return meshFromAsset;
    }

    public KeyValuePair<Mesh, Material> GetMeshAndMaterialFromAsset(string assetPath)
    {
      GameObject result;
      if (!this.TryGetSharedAsset<GameObject>(assetPath, out result))
        result = this.DefaultPrefab;
      Mesh key = (Mesh) null;
      MeshFilter component1;
      if (result.TryGetComponent<MeshFilter>(out component1))
        key = component1.sharedMesh;
      if (!(bool) (UnityEngine.Object) key)
      {
        Log.Error(string.Format("Missing a mesh on prefab '{0}'.", (object) result));
        key = this.DefaultPrefab.GetComponent<MeshFilter>().sharedMesh;
      }
      Material material = (Material) null;
      MeshRenderer component2;
      if (result.TryGetComponent<MeshRenderer>(out component2))
        material = component2.sharedMaterial;
      if (!(bool) (UnityEngine.Object) material)
      {
        Log.Error(string.Format("Missing a material on prefab '{0}'.", (object) result));
        material = this.DefaultMaterial;
      }
      return Make.Kvp<Mesh, Material>(key, material);
    }
  }
}
