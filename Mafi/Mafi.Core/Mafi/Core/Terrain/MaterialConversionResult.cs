// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.MaterialConversionResult
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.Terrain
{
  public readonly struct MaterialConversionResult
  {
    public readonly Tile2iAndIndex TileAndIndex;
    public readonly TerrainMaterialThicknessSlim SourceMaterialThickness;

    public static MaterialConversionResult Empty => new MaterialConversionResult();

    public bool IsEmpty => this.SourceMaterialThickness == new TerrainMaterialThicknessSlim();

    public bool IsNotEmpty => this.SourceMaterialThickness != new TerrainMaterialThicknessSlim();

    public MaterialConversionResult(
      Tile2iAndIndex tileAndIndex,
      TerrainMaterialThicknessSlim sourceMaterialThickness)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.TileAndIndex = tileAndIndex;
      this.SourceMaterialThickness = sourceMaterialThickness;
    }
  }
}
