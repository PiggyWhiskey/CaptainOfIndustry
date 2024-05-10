// Decompiled with JetBrains decompiler
// Type: RTG.MaterialEx
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;

#nullable disable
namespace RTG
{
  public static class MaterialEx
  {
    public static void SetZWriteEnabled(this Material material, bool enabled)
    {
      material.SetInt("_ZWrite", enabled ? 1 : 0);
    }

    public static void SetZTestEnabled(this Material material, bool enabled)
    {
      material.SetInt("_ZTest", enabled ? 4 : 8);
    }

    public static void SetZTestAlways(this Material material) => material.SetInt("_ZTest", 8);

    public static void SetZTestLess(this Material material) => material.SetInt("_ZTest", 2);

    public static void SetCullModeBack(this Material material) => material.SetInt("_CullMode", 2);

    public static void SetCullModeFront(this Material material) => material.SetInt("_CullMode", 1);

    public static void SetCullModeOff(this Material material) => material.SetInt("_CullMode", 0);

    public static void SetColor(this Material material, Color color)
    {
      material.SetColor("_Color", color);
    }

    public static void SetStencilCmpAlways(this Material material)
    {
      material.SetInt("_StencilComp", 8);
    }

    public static void SetStencilCmpNotEqual(this Material material)
    {
      material.SetInt("_StencilComp", 6);
    }
  }
}
