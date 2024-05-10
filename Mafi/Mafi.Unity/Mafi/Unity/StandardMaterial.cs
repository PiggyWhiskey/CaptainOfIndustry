// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.StandardMaterial
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity
{
  public static class StandardMaterial
  {
    public const string MAIN_TEX_NAME = "_MainTex";
    public static readonly int MAIN_TEX_SHADER_ID;
    public const string NORMAL_MAP_NAME = "_BumpMap";
    public static readonly int NORMAL_MAP_SHADER_ID;
    private static readonly int METALLIC_GLOSS_MAP_SHADER_ID;
    private static readonly int METALLIC_GLOSS_MAP_ALT_SHADER_ID;
    private static readonly int EMISSION_MAP_SHADER_ID;

    public static Texture GetMainTexture(Material mat, bool ignoreMissingTextures = false)
    {
      if (mat.HasTexture(StandardMaterial.MAIN_TEX_SHADER_ID))
        return mat.GetTexture(StandardMaterial.MAIN_TEX_SHADER_ID);
      if (!ignoreMissingTextures)
        Log.Error(string.Format("Failed to find texture with property '{0}' on material {1}.", (object) "_MainTex", (object) mat));
      return (Texture) null;
    }

    public static Texture GetNormalTexture(Material mat, bool ignoreMissingTextures = false)
    {
      if (mat.HasTexture(StandardMaterial.NORMAL_MAP_SHADER_ID))
        return mat.GetTexture(StandardMaterial.NORMAL_MAP_SHADER_ID);
      if (!ignoreMissingTextures)
        Log.Error(string.Format("Failed to find texture with property '{0}' on material {1}.", (object) "_BumpMap", (object) mat));
      return (Texture) null;
    }

    public static Texture GetMetallicGlossTexture(Material mat, bool ignoreMissingTextures = false)
    {
      if (mat.HasTexture(StandardMaterial.METALLIC_GLOSS_MAP_SHADER_ID))
        return mat.GetTexture(StandardMaterial.METALLIC_GLOSS_MAP_SHADER_ID);
      if (mat.HasTexture(StandardMaterial.METALLIC_GLOSS_MAP_ALT_SHADER_ID))
        return mat.GetTexture(StandardMaterial.METALLIC_GLOSS_MAP_ALT_SHADER_ID);
      if (!ignoreMissingTextures)
        Log.Error("Failed to find texture with property '_MetallicGlossMap' " + string.Format("or '{0}' on material {1}.", (object) StandardMaterial.METALLIC_GLOSS_MAP_ALT_SHADER_ID, (object) mat));
      return (Texture) null;
    }

    public static Texture GetEmissionTexture(Material mat, bool ignoreMissingTextures = false)
    {
      if (mat.HasTexture(StandardMaterial.EMISSION_MAP_SHADER_ID))
        return mat.GetTexture(StandardMaterial.EMISSION_MAP_SHADER_ID);
      if (!ignoreMissingTextures)
        Log.Error(string.Format("Failed to find texture with property '{0}' on material {1}.", (object) StandardMaterial.EMISSION_MAP_SHADER_ID, (object) mat));
      return (Texture) null;
    }

    static StandardMaterial()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      StandardMaterial.MAIN_TEX_SHADER_ID = Shader.PropertyToID("_MainTex");
      StandardMaterial.NORMAL_MAP_SHADER_ID = Shader.PropertyToID("_BumpMap");
      StandardMaterial.METALLIC_GLOSS_MAP_SHADER_ID = Shader.PropertyToID("_MetallicGlossMap");
      StandardMaterial.METALLIC_GLOSS_MAP_ALT_SHADER_ID = Shader.PropertyToID("_MetalEmissSmoothMap");
      StandardMaterial.EMISSION_MAP_SHADER_ID = Shader.PropertyToID("_EmissionMap");
    }
  }
}
