// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.TerrainMaterialThicknessSlimExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Products;

#nullable disable
namespace Mafi.Core.Terrain
{
  public static class TerrainMaterialThicknessSlimExtensions
  {
    public static TerrainMaterialThicknessSlim Of(this int thickness, TerrainMaterialSlimId id)
    {
      return new TerrainMaterialThicknessSlim(id, new ThicknessTilesF(thickness));
    }

    public static TerrainMaterialThicknessSlim Of(this Fix32 thickness, TerrainMaterialSlimId id)
    {
      return new TerrainMaterialThicknessSlim(id, new ThicknessTilesF(thickness));
    }

    public static TerrainMaterialThicknessSlim Of(
      this ThicknessTilesF thickness,
      TerrainMaterialSlimId id)
    {
      return new TerrainMaterialThicknessSlim(id, thickness);
    }
  }
}
