// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.AssetBundleLoader
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity
{
  public class AssetBundleLoader : IDisposable
  {
    public const string MANIFEST_FILE_NAME = "mafi_bundles.manifest";
    private static readonly char[] IGNORED_BUNDLE_NAME_CHARS;
    /// <summary>File header of Unity asset bundles.</summary>
    private static readonly byte[] s_expectedHeader;
    private readonly byte[] m_headerReadBuffer;
    private Option<string> m_assetsOverrideRootPath;
    private Option<IAlternativeAssetLoader> m_alternativeAssetLoader;
    private readonly Dict<string, AssetBundle> m_loadedBundles;
    private readonly Dict<string, AssetBundle> m_allAssetsBundles;
    private readonly Dict<string, AssetBundle> m_notLoadedAssets;
    private readonly Dict<string, UnityEngine.Object> m_loadedAssets;

    /// <summary>
    /// Note that this may contain duplicate object for normal and lowercase key paths.
    /// </summary>
    public IReadOnlyDictionary<string, UnityEngine.Object> LoadedAssets
    {
      get => (IReadOnlyDictionary<string, UnityEngine.Object>) this.m_loadedAssets;
    }

    public void Dispose()
    {
      this.m_loadedAssets.Clear();
      this.m_notLoadedAssets.Clear();
      this.m_allAssetsBundles.Clear();
      foreach (AssetBundle assetBundle in this.m_loadedBundles.Values)
        assetBundle.Unload(true);
      this.m_loadedBundles.Clear();
    }

    public void SetAssetOverrideRootPath(string path)
    {
      this.m_assetsOverrideRootPath = (Option<string>) path;
    }

    public bool ContainsAsset(string assetPath)
    {
      if (assetPath == null)
      {
        Log.Error("Null asset path.");
        return false;
      }
      if (this.m_alternativeAssetLoader.HasValue)
        return this.m_alternativeAssetLoader.Value.ContainsAsset(assetPath);
      if (this.m_loadedAssets.ContainsKey(assetPath))
        return true;
      string lowerInvariant = assetPath.ToLowerInvariant();
      UnityEngine.Object @object;
      if (!this.m_loadedAssets.TryGetValue(lowerInvariant, out @object))
        return this.m_notLoadedAssets.ContainsKey(lowerInvariant);
      this.m_loadedAssets.Add(assetPath, @object);
      return true;
    }

    public bool TryGetAsset<T>(string assetPath, bool ignoreCache, out T asset) where T : UnityEngine.Object
    {
      if (assetPath == null)
      {
        asset = default (T);
        return false;
      }
      if (ignoreCache)
      {
        string lowerInvariant = assetPath.ToLowerInvariant();
        AssetBundle bundle;
        if (this.m_allAssetsBundles.TryGetValue(lowerInvariant, out bundle))
        {
          asset = this.loadNewAsset<T>(lowerInvariant, bundle);
          return true;
        }
        asset = default (T);
        return false;
      }
      UnityEngine.Object object1;
      if (this.m_loadedAssets.TryGetValue(assetPath, out object1))
      {
        if (object1 is T obj)
        {
          asset = obj;
          return true;
        }
        Log.Warning("Cached asset '" + assetPath + "' was expected to be of type '" + typeof (T).Name + "' but it is '" + object1.GetType().Name + "'.");
        asset = default (T);
        return false;
      }
      string lowerInvariant1 = assetPath.ToLowerInvariant();
      UnityEngine.Object object2;
      if (this.m_loadedAssets.TryGetValue(lowerInvariant1, out object2))
      {
        if (object2 is T obj)
        {
          asset = obj;
          this.m_loadedAssets.Add(assetPath, (UnityEngine.Object) asset);
          return true;
        }
        Log.Warning("Cached asset '" + assetPath + "' was expected to be of type '" + typeof (T).Name + "' but it is '" + object1.GetType().Name + "'.");
        asset = default (T);
        return false;
      }
      AssetBundle bundle1;
      if (!this.m_notLoadedAssets.TryRemove(lowerInvariant1, out bundle1))
      {
        asset = default (T);
        return false;
      }
      asset = this.loadNewAsset<T>(lowerInvariant1, bundle1);
      this.m_loadedAssets.Add(lowerInvariant1, (UnityEngine.Object) asset);
      return true;
    }

    public void LoadAllRegisteredAssets()
    {
      foreach (KeyValuePair<string, AssetBundle> notLoadedAsset in this.m_notLoadedAssets)
      {
        if (!this.m_loadedAssets.ContainsKey(notLoadedAsset.Key))
        {
          UnityEngine.Object @object = this.loadNewAsset<UnityEngine.Object>(notLoadedAsset.Key, notLoadedAsset.Value);
          this.m_loadedAssets.Add(notLoadedAsset.Key, @object);
        }
      }
      this.m_notLoadedAssets.Clear();
    }

    private T loadNewAsset<T>(string path, AssetBundle bundle) where T : UnityEngine.Object
    {
      T asset;
      if (!this.m_alternativeAssetLoader.HasValue || !this.m_alternativeAssetLoader.Value.TryLoadAsset<T>(path, out asset))
        asset = bundle.LoadAsset<T>(path);
      if ((bool) (UnityEngine.Object) asset)
      {
        if (this.m_assetsOverrideRootPath.HasValue)
        {
          string str = path;
          if (path.StartsWith("Assets", StringComparison.OrdinalIgnoreCase))
            str = str.SubstringSafe("Assets".Length + 1);
          string path1 = Path.Combine(this.m_assetsOverrideRootPath.Value, str);
          if (File.Exists(path1))
          {
            T newAsset;
            string error;
            if (AssetBundleLoader.tryOverrideAssetFromFile<T>(path1, asset, out newAsset, out error))
            {
              asset = newAsset;
              Log.Info("Loaded override asset from '" + path1 + "' replacing '" + path + "'");
            }
            else
              Log.Warning("Failed to override asset " + path1 + ": " + error);
          }
        }
      }
      else
        Log.Error("Loaded asset is invalid: " + path);
      return asset;
    }

    public void RegisterAssetBundlesIn(string bundlesRootDirPath, string coreBundlesDirPath)
    {
      string path = Path.Combine(bundlesRootDirPath, "mafi_bundles.manifest");
      if (File.Exists(path))
      {
        foreach (string path2 in File.ReadAllLines(path))
        {
          if (!string.IsNullOrEmpty(path2))
          {
            bool flag = false;
            if (path2[0] == '+')
            {
              flag = true;
              path2 = path2.TrimStart(AssetBundleLoader.IGNORED_BUNDLE_NAME_CHARS);
            }
            string fullPath = Path.GetFullPath(path2[0] == '!' ? Path.Combine(coreBundlesDirPath, path2.Substring(1)) : Path.Combine(bundlesRootDirPath, path2));
            AssetBundle bundle;
            if (this.m_loadedBundles.TryGetValue(fullPath, out bundle))
            {
              if (flag)
              {
                foreach (string allAssetName in bundle.GetAllAssetNames())
                {
                  if (!this.m_loadedAssets.ContainsKey(allAssetName))
                    this.loadNewAsset<UnityEngine.Object>(allAssetName, bundle);
                }
              }
            }
            else if (!File.Exists(fullPath))
              Log.Error("Failed to load asset bundle '" + path2 + "', file listed in manifest does not exist in '" + bundlesRootDirPath + "'");
            else
              this.loadNewBundle(fullPath);
          }
        }
      }
      else
      {
        Log.Warning("Bundles manifest file is missing, bundles may be read in incorrect order causing missing assets. Expected path: " + path);
        foreach (string enumerateFile in Directory.EnumerateFiles(bundlesRootDirPath))
        {
          using (FileStream fileStream = File.OpenRead(enumerateFile))
          {
            if (fileStream.Read(this.m_headerReadBuffer, 0, this.m_headerReadBuffer.Length) == this.m_headerReadBuffer.Length)
            {
              bool flag = true;
              for (int index = 0; index < this.m_headerReadBuffer.Length; ++index)
              {
                if ((int) this.m_headerReadBuffer[index] != (int) AssetBundleLoader.s_expectedHeader[index])
                {
                  flag = false;
                  break;
                }
              }
              if (!flag)
                continue;
            }
            else
              continue;
          }
          if (!this.m_loadedBundles.ContainsKey(enumerateFile))
            this.loadNewBundle(enumerateFile);
        }
      }
    }

    private void loadNewBundle(string bundlePath)
    {
      try
      {
        AssetBundle assetBundle = AssetBundle.LoadFromFile(bundlePath);
        if ((UnityEngine.Object) assetBundle == (UnityEngine.Object) null)
        {
          Log.Error("No asset bundle at '" + bundlePath + "', LoadFromFile returned null.");
        }
        else
        {
          this.m_loadedBundles.AddAndAssertNew(bundlePath, assetBundle);
          foreach (string allAssetName in assetBundle.GetAllAssetNames())
          {
            string lowerInvariant = allAssetName.ToLowerInvariant();
            this.m_notLoadedAssets.AddAndAssertNew(lowerInvariant, assetBundle);
            this.m_allAssetsBundles.AddAndAssertNew(lowerInvariant, assetBundle);
          }
        }
      }
      catch (Exception ex)
      {
        Log.Exception(ex, "Failed to load asset bundle from '" + bundlePath + "'.");
      }
    }

    public void CheckAllAssets()
    {
      foreach (KeyValuePair<string, UnityEngine.Object> loadedAsset in this.m_loadedAssets)
      {
        if (!(bool) loadedAsset.Value)
          Log.Error("Invalid asset found in DB: " + loadedAsset.Key);
      }
    }

    public void ClearCachedAssets()
    {
      this.m_loadedAssets.Clear();
      this.m_notLoadedAssets.Clear();
      foreach (AssetBundle assetBundle in this.m_loadedBundles.Values)
      {
        foreach (string allAssetName in assetBundle.GetAllAssetNames())
          this.m_notLoadedAssets.AddAndAssertNew(allAssetName.ToLowerInvariant(), assetBundle);
      }
    }

    private static bool tryOverrideAssetFromFile<T>(
      string path,
      T originalAsset,
      out T newAsset,
      out string error)
      where T : UnityEngine.Object
    {
      newAsset = default (T);
      if (originalAsset is Texture2D texture2D)
      {
        Texture2D tex = new Texture2D(texture2D.width, texture2D.height, texture2D.graphicsFormat, texture2D.mipmapCount, TextureCreationFlags.None);
        try
        {
          if (tex.LoadImage(File.ReadAllBytes(path)))
          {
            tex.Apply(true, !texture2D.isReadable);
            newAsset = (T) tex;
            error = "";
            return true;
          }
          error = "Failed load file as a texture: " + path;
          return false;
        }
        catch (Exception ex)
        {
          error = ex.Message;
          return false;
        }
      }
      else
      {
        error = "Unsupported asset type '" + originalAsset.GetType().Name + "'.";
        return false;
      }
    }

    public void SetAlternativeAssetLoader(IAlternativeAssetLoader loader)
    {
      this.m_alternativeAssetLoader = loader.CreateOption<IAlternativeAssetLoader>();
    }

    public AssetBundleLoader()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_headerReadBuffer = new byte[AssetBundleLoader.s_expectedHeader.Length];
      this.m_loadedBundles = new Dict<string, AssetBundle>(64);
      this.m_allAssetsBundles = new Dict<string, AssetBundle>(256);
      this.m_notLoadedAssets = new Dict<string, AssetBundle>(256);
      this.m_loadedAssets = new Dict<string, UnityEngine.Object>(256);
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static AssetBundleLoader()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      AssetBundleLoader.IGNORED_BUNDLE_NAME_CHARS = new char[1]
      {
        '+'
      };
      AssetBundleLoader.s_expectedHeader = new byte[5]
      {
        (byte) 85,
        (byte) 110,
        (byte) 105,
        (byte) 116,
        (byte) 121
      };
    }
  }
}
