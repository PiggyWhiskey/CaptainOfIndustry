// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Designation.SurfaceDesignationDataFactory
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections;
using System;

#nullable disable
namespace Mafi.Core.Terrain.Designation
{
  /// <summary>Helper class that helps with designation creation.</summary>
  public static class SurfaceDesignationDataFactory
  {
    public static Tile2i GetOrigin(Vector2i v) => new Tile2i(v) * 4;

    public static SurfaceDesignationData Create(
      Tile2i position,
      uint unassignedTilesBitmap,
      TerrainTileSurfaceProto surfaceProto)
    {
      return new SurfaceDesignationData(SurfaceDesignation.GetOrigin(position), unassignedTilesBitmap, surfaceProto.SlimId);
    }

    public static Lyst<SurfaceDesignationData> CreateArea(
      Vector2i from,
      Vector2i toIncl,
      TerrainTileSurfaceProto surfaceProto)
    {
      return SurfaceDesignationDataFactory.CreateArea(SurfaceDesignationDataFactory.GetOrigin(from), SurfaceDesignationDataFactory.GetOrigin(toIncl), surfaceProto);
    }

    public static Lyst<SurfaceDesignationData> CreateArea(
      Tile2i from,
      Tile2i toIncl,
      TerrainTileSurfaceProto surfaceProto)
    {
      Tile2i origin1 = SurfaceDesignation.GetOrigin(from.Min(toIncl));
      Tile2i origin2 = SurfaceDesignation.GetOrigin(from.Max(toIncl));
      RelTile2i relTile2i = (origin2 - origin1) / 4;
      relTile2i = relTile2i.AddXy(1);
      int productInt = relTile2i.ProductInt;
      Lyst<SurfaceDesignationData> area = new Lyst<SurfaceDesignationData>(productInt);
      for (int y = origin1.Y; y <= origin2.Y; y += 4)
      {
        for (int x = origin1.X; x <= origin2.X; x += 4)
          area.Add(new SurfaceDesignationData(new Tile2i(x, y), 0U, surfaceProto.SlimId));
      }
      Assert.That<Lyst<SurfaceDesignationData>>(area).HasLength<SurfaceDesignationData>(productInt);
      return area;
    }

    public static SurfaceDesignationData CreateNoSnapping(
      Tile2i position,
      uint unassignedTilesBitmap,
      TerrainTileSurfaceProto surfaceProto)
    {
      return SurfaceDesignationDataFactory.Create(SurfaceDesignation.GetOrigin(position), unassignedTilesBitmap, surfaceProto);
    }

    /// <summary>For placing the initial designation.</summary>
    public static bool TryCreateSnapToNeighbors(
      Tile2i position,
      ISurfaceDesignationsManager designationManager,
      TerrainTileSurfaceProto surfaceProto,
      out SurfaceDesignationData data)
    {
      Tile2i origin = SurfaceDesignation.GetOrigin(position);
      RelTile2i relTile2i = position - origin;
      if (!designationManager.CanPlaceSurfaceTile(position))
      {
        data = SurfaceDesignationDataFactory.CreateNoSnapping(position, (uint) ushort.MaxValue, surfaceProto);
        return false;
      }
      uint unassignedTilesBitmap = (uint) ~(1 << relTile2i.Y * 4 + relTile2i.X);
      data = SurfaceDesignationDataFactory.CreateNoSnapping(position, unassignedTilesBitmap, surfaceProto);
      return true;
    }

    /// <summary>
    /// Extends given designation to area specified by <paramref name="extensionEnd" />.
    /// </summary>
    public static void UpdateDesignationExtension(
      ISurfaceDesignationsManager manager,
      SurfaceDesignationProto proto,
      SurfaceDesignationData sourceData,
      Tile2i selectionStart,
      Tile2i extensionEnd,
      TerrainTileSurfaceProto surfaceProto,
      Action<SurfaceDesignationProto, SurfaceDesignationData> added,
      Action<SurfaceDesignationProto, SurfaceDesignationData> removed,
      Action<SurfaceDesignationProto, SurfaceDesignationData> updated,
      Dict<Tile2i, SurfaceDesignationData> designations)
    {
      Assert.That<Dict<Tile2i, SurfaceDesignationData>>(designations).ContainsKey<Tile2i, SurfaceDesignationData>((Tile2i) sourceData.OriginTile);
      RectangleTerrainArea2i area = RectangleTerrainArea2i.FromTwoPositions(selectionStart.Min(extensionEnd), selectionStart.Max(extensionEnd));
      Tile2i originTile = (Tile2i) sourceData.OriginTile;
      Tile2i origin = SurfaceDesignation.GetOrigin(extensionEnd);
      Tile2i tile2i1 = originTile.Min(origin);
      Tile2i tile2i2 = originTile.Max(origin);
      Lyst<Tile2i> lyst1 = (Lyst<Tile2i>) null;
      foreach (SurfaceDesignationData surfaceDesignationData in designations.Values)
      {
        if (!(surfaceDesignationData.OriginTile >= tile2i1) || !(surfaceDesignationData.OriginTile <= tile2i2))
        {
          if (lyst1 == null)
            lyst1 = new Lyst<Tile2i>();
          lyst1.Add((Tile2i) surfaceDesignationData.OriginTile);
        }
      }
      if (lyst1 != null)
      {
        foreach (Tile2i key in lyst1)
        {
          SurfaceDesignationData surfaceDesignationData;
          designations.TryRemove(key, out surfaceDesignationData).AssertTrue();
          removed(proto, surfaceDesignationData);
        }
      }
      Lyst<Tile2i> lyst2 = (Lyst<Tile2i>) null;
      if (lyst1 != null)
      {
        lyst2 = lyst1;
        lyst2.Clear();
      }
      foreach (SurfaceDesignationData surfaceDesignationData in designations.Values)
      {
        if (lyst2 == null)
          lyst2 = new Lyst<Tile2i>();
        lyst2.Add((Tile2i) surfaceDesignationData.OriginTile);
      }
      if (lyst2 != null && lyst2.Count > 0)
      {
        foreach (Tile2i key in lyst2)
        {
          SurfaceDesignationData surfaceDesignationData;
          if (!designations.TryGetValue(key, out surfaceDesignationData))
          {
            Log.Error("Unable to find designation to update.");
          }
          else
          {
            uint unassignedTilesBitmap = getUnassignedTilesBitmap((Tile2i) surfaceDesignationData.OriginTile);
            if ((int) unassignedTilesBitmap != (int) surfaceDesignationData.UnassignedTilesBitmap)
            {
              surfaceDesignationData = surfaceDesignationData.WithNewUnassignedTiles(unassignedTilesBitmap);
              designations[key] = surfaceDesignationData;
              updated(proto, surfaceDesignationData);
            }
          }
        }
      }
      for (int y = tile2i1.Y; y <= tile2i2.Y; y += 4)
      {
        for (int x = tile2i1.X; x <= tile2i2.X; x += 4)
        {
          Tile2i tile2i3 = new Tile2i(x, y);
          if (!designations.ContainsKey(tile2i3))
          {
            uint unassignedTilesBitmap = getUnassignedTilesBitmap(tile2i3);
            SurfaceDesignationData surfaceDesignationData = SurfaceDesignationDataFactory.Create(tile2i3, unassignedTilesBitmap, surfaceProto);
            designations.Add((Tile2i) surfaceDesignationData.OriginTile, surfaceDesignationData);
            added(proto, surfaceDesignationData);
          }
        }
      }

      uint getUnassignedTilesBitmap(Tile2i origin)
      {
        uint unassignedTilesBitmap = 0;
        for (int y = 0; y < 4; ++y)
        {
          for (int x = 0; x < 4; ++x)
          {
            RelTile2i relTile2i = new RelTile2i(x, y);
            if (!area.ContainsTile(origin + relTile2i) || !manager.CanPlaceSurfaceTile(origin + relTile2i))
              unassignedTilesBitmap |= (uint) (1 << relTile2i.X + relTile2i.Y * 4);
          }
        }
        return unassignedTilesBitmap;
      }
    }
  }
}
