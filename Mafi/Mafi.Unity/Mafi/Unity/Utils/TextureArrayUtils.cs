// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Utils.TextureArrayUtils
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using UnityEngine;

#nullable disable
namespace Mafi.Unity.Utils
{
  public static class TextureArrayUtils
  {
    public static Texture2DArray Create2DArrayOrThrow(
      string name,
      string[] textureAssetPaths,
      AssetsDb assetsDb,
      bool isLinear,
      TextureFormat? enforceFormat = null,
      TextureWrapMode? enforceWarpMode = null,
      FilterMode? enforceFilterMode = null,
      int? enforceAnisoLevel = null,
      float mipMapBias = 0.0f)
    {
      Texture2D[] textures = new Texture2D[textureAssetPaths.Length];
      for (int index = 0; index < textureAssetPaths.Length; ++index)
        textures[index] = assetsDb.GetSharedAssetOrThrow<Texture2D>(textureAssetPaths[index]);
      return TextureArrayUtils.Create2DArrayOrThrow(name, textures, isLinear, enforceFormat, enforceWarpMode, enforceFilterMode, enforceAnisoLevel, mipMapBias);
    }

    /// <summary>
    /// Creates texture array from the given textures. Any invalid or null textures are skipped.
    /// </summary>
    public static Texture2DArray Create2DArrayOrThrow(
      string name,
      Texture2D[] textures,
      bool isLinear,
      TextureFormat? enforceFormat = null,
      TextureWrapMode? enforceWarpMode = null,
      FilterMode? enforceFilterMode = null,
      int? enforceAnisoLevel = null,
      float mipMapBias = 0.0f)
    {
      if (textures == null || textures.Length == 0)
        throw new Exception("Failed to create texture array '" + name + "': No textures given.");
      Texture2D texture2D = (Texture2D) null;
      foreach (Texture2D texture in textures)
      {
        if ((bool) (UnityEngine.Object) texture)
        {
          texture2D = texture;
          break;
        }
      }
      int width = !((UnityEngine.Object) texture2D == (UnityEngine.Object) null) ? texture2D.width : throw new Exception("Failed to create texture array '" + name + "': No valid textures given.");
      int height = texture2D.height;
      int mipmapCount = texture2D.mipmapCount;
      TextureFormat textureFormat = (TextureFormat) ((int) enforceFormat ?? (int) texture2D.format);
      TextureWrapMode textureWrapMode = (TextureWrapMode) ((int) enforceWarpMode ?? (int) texture2D.wrapMode);
      FilterMode filterMode = (FilterMode) ((int) enforceFilterMode ?? (int) texture2D.filterMode);
      int num = enforceAnisoLevel ?? texture2D.anisoLevel;
      for (int index = 0; index < textures.Length; ++index)
      {
        Texture2D texture = textures[index];
        if ((bool) (UnityEngine.Object) texture)
        {
          if (texture.width != width || texture.height != height)
            throw new Exception("Failed to create texture array '" + name + "': " + string.Format("size of texture {0} '{1}' is {2}x{3} ", (object) index, (object) texture.name, (object) texture.width, (object) texture.height) + string.Format("but {0}x{1} was expected.", (object) width, (object) height));
          if (texture.format != textureFormat)
            throw new Exception("Failed to create texture array '" + name + "': " + string.Format("format of texture {0} '{1}' is {2} but {3} was expected.", (object) index, (object) texture.name, (object) texture.format, (object) textureFormat));
          if (texture.mipmapCount != mipmapCount)
            throw new Exception("Failed to create texture array '" + name + "': " + string.Format("mipmap count of texture {0} '{1}' is {2} but {3} was expected.", (object) index, (object) texture.name, (object) texture.mipmapCount, (object) mipmapCount));
          if (!enforceWarpMode.HasValue && texture.wrapMode != textureWrapMode)
            throw new Exception("Failed to create texture array '" + name + "': " + string.Format("wrap mode of texture {0} '{1}' is {2} but {3} was expected.", (object) index, (object) texture.name, (object) texture.wrapMode, (object) textureWrapMode));
          if (!enforceFilterMode.HasValue && texture.filterMode != filterMode)
            throw new Exception("Failed to create texture array '" + name + "': " + string.Format("filter mode of texture {0} '{1}' is {2} but {3} was expected.", (object) index, (object) texture.name, (object) texture.filterMode, (object) filterMode));
          if (!enforceAnisoLevel.HasValue && texture.anisoLevel != num)
            throw new Exception("Failed to create texture array '" + name + "': " + string.Format("aniso level of texture {0} '{1}' is {2} but {3} was expected.", (object) index, (object) texture.name, (object) texture.anisoLevel, (object) num));
        }
      }
      Texture2DArray texture2Darray = new Texture2DArray(width, height, textures.Length, textureFormat, mipmapCount, isLinear);
      texture2Darray.name = name;
      texture2Darray.wrapMode = textureWrapMode;
      texture2Darray.filterMode = filterMode;
      texture2Darray.anisoLevel = num;
      texture2Darray.mipMapBias = mipMapBias;
      Texture2DArray dst = texture2Darray;
      for (int dstElement = 0; dstElement < textures.Length; ++dstElement)
      {
        if ((bool) (UnityEngine.Object) textures[dstElement])
          Graphics.CopyTexture((Texture) textures[dstElement], 0, (Texture) dst, dstElement);
      }
      return dst;
    }
  }
}
