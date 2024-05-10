// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.TileSurfacesEdgesSpec
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.Terrain
{
  public readonly struct TileSurfacesEdgesSpec
  {
    public static readonly TileSurfacesEdgesSpec Empty;
    public readonly string AlbedoTextureEdgeFullPath;
    public readonly string NormalsTextureEdgeFullPath;
    public readonly string SmoothMetalTextureEdgeFullPath;
    public readonly string AlbedoTextureEdgeHorizontalPath;
    public readonly string NormalsTextureEdgeHorizontalPath;
    public readonly string SmoothMetalTextureEdgeHorizontalPath;
    public readonly string AlbedoTextureEdgeVerticalPath;
    public readonly string NormalsTextureEdgeVerticalPath;
    public readonly string SmoothMetalTextureEdgeVerticalPath;
    public readonly string AlbedoTextureEdgeCornersPath;
    public readonly string NormalsTextureEdgeCornersPath;
    public readonly string SmoothMetalTextureEdgeCornersPath;

    public TileSurfacesEdgesSpec(
      string albedoTextureEdgeFullPath,
      string normalsTextureEdgeFullPath,
      string smoothMetalTextureEdgeFullPath,
      string albedoTextureEdgeHorizontalPath,
      string normalsTextureEdgeHorizontalPath,
      string smoothMetalTextureEdgeHorizontalPath,
      string albedoTextureEdgeVerticalPath,
      string normalsTextureEdgeVerticalPath,
      string smoothMetalTextureEdgeVerticalPath,
      string albedoTextureEdgeCornersPath,
      string normalsTextureEdgeCornersPath,
      string smoothMetalTextureEdgeCornersPath)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.AlbedoTextureEdgeFullPath = albedoTextureEdgeFullPath;
      this.NormalsTextureEdgeFullPath = normalsTextureEdgeFullPath;
      this.SmoothMetalTextureEdgeFullPath = smoothMetalTextureEdgeFullPath;
      this.AlbedoTextureEdgeHorizontalPath = albedoTextureEdgeHorizontalPath;
      this.NormalsTextureEdgeHorizontalPath = normalsTextureEdgeHorizontalPath;
      this.SmoothMetalTextureEdgeHorizontalPath = smoothMetalTextureEdgeHorizontalPath;
      this.AlbedoTextureEdgeVerticalPath = albedoTextureEdgeVerticalPath;
      this.NormalsTextureEdgeVerticalPath = normalsTextureEdgeVerticalPath;
      this.SmoothMetalTextureEdgeVerticalPath = smoothMetalTextureEdgeVerticalPath;
      this.AlbedoTextureEdgeCornersPath = albedoTextureEdgeCornersPath;
      this.NormalsTextureEdgeCornersPath = normalsTextureEdgeCornersPath;
      this.SmoothMetalTextureEdgeCornersPath = smoothMetalTextureEdgeCornersPath;
    }

    static TileSurfacesEdgesSpec()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TileSurfacesEdgesSpec.Empty = new TileSurfacesEdgesSpec("EMPTY", "EMPTY", "EMPTY", "EMPTY", "EMPTY", "EMPTY", "EMPTY", "EMPTY", "EMPTY", "EMPTY", "EMPTY", "EMPTY");
    }
  }
}
