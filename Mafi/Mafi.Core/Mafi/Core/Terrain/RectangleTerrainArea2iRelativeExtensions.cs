// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.RectangleTerrainArea2iRelativeExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Entities.Static.Layout;

#nullable disable
namespace Mafi.Core.Terrain
{
  public static class RectangleTerrainArea2iRelativeExtensions
  {
    public static RectangleTerrainArea2i Transform(
      this EntityLayout entityLayout,
      RectangleTerrainArea2iRelative area,
      TileTransform transform)
    {
      Tile2i tile2iRounded = entityLayout.TransformF_PointRelToCore(area.Origin.RelTile2f, transform).Tile2iRounded;
      RelTile2i relTile2i = transform.TransformMatrix.Transform(area.Size);
      return new RectangleTerrainArea2i(tile2iRounded.Min(tile2iRounded + relTile2i), relTile2i.AbsValue);
    }
  }
}
