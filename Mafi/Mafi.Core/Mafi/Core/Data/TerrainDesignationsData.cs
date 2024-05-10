// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Data.TerrainDesignationsData
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Static;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain.Designation;
using Mafi.Core.Terrain.Trees;
using System;

#nullable disable
namespace Mafi.Core.Data
{
  internal sealed class TerrainDesignationsData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      registrator.PrototypesDb.Add<TerrainDesignationProto>(new TerrainDesignationProto(IdsCore.TerrainDesignators.MiningDesignator, Proto.CreateStr(IdsCore.TerrainDesignators.MiningDesignator, "Mining designation"), (Func<ITerrainDesignationsManager, Tile2iAndIndex, HeightTilesF, bool, bool>) ((manager, tileAndIndex, designationHeight, upperEdge) =>
      {
        if (!(manager.TerrainManager.GetHeight(tileAndIndex.Index) <= designationHeight))
          return false;
        if (upperEdge)
          return true;
        return !manager.TerrainPropsManager.TerrainTileToProp.ContainsKey(tileAndIndex.TileCoordSlim) && !manager.TreesManager.Stumps.ContainsKey(new TreeId(tileAndIndex.TileCoordSlim));
      }), Option<Func<ITerrainDesignationsManager, Tile2iAndIndex, HeightTilesF, bool, bool>>.Create((Func<ITerrainDesignationsManager, Tile2iAndIndex, HeightTilesF, bool, bool>) ((manager, tileAndIndex, designationHeight, upperEdge) =>
      {
        if (!(manager.TerrainManager.GetHeight(tileAndIndex.Index) <= designationHeight))
          return false;
        if (upperEdge)
          return true;
        return !manager.TerrainPropsManager.TerrainTileToProp.ContainsKey(tileAndIndex.TileCoordSlim) && !manager.TreesManager.Stumps.ContainsKey(new TreeId(tileAndIndex.TileCoordSlim));
      })), (Option<Func<ITerrainDesignationsManager, Tile2iAndIndex, HeightTilesF, bool, bool>>) Option.None, false, 1, true, TrCore.DesignationWarning__NoTower, true, true, new TerrainDesignationProto.Gfx(new ColorRgba(16764992, 176), new ColorRgba(16756800, 176), new ColorRgba(16764992, 64), new ColorRgba(16728128, 176))));
      registrator.PrototypesDb.Add<TerrainDesignationProto>(new TerrainDesignationProto(IdsCore.TerrainDesignators.DumpingDesignator, Proto.CreateStr(IdsCore.TerrainDesignators.DumpingDesignator, "Dumping designation"), (Func<ITerrainDesignationsManager, Tile2iAndIndex, HeightTilesF, bool, bool>) ((manager, tileAndIndex, designationHeight, upperEdge) =>
      {
        HeightTilesF height = manager.TerrainManager.GetHeight(tileAndIndex.Index);
        if (height >= designationHeight)
          return true;
        EntityId entityId;
        ILayoutEntity entity;
        return manager.OccupancyManager.TryGetAnyOccupyingEntityAt(tileAndIndex.TileCoord.ExtendHeight(height.HeightI), out entityId) && manager.EntitiesManager.TryGetEntity<ILayoutEntity>(entityId, out entity) && entity.Position3f.Height >= designationHeight;
      }), (Option<Func<ITerrainDesignationsManager, Tile2iAndIndex, HeightTilesF, bool, bool>>) Option.None, Option<Func<ITerrainDesignationsManager, Tile2iAndIndex, HeightTilesF, bool, bool>>.Create((Func<ITerrainDesignationsManager, Tile2iAndIndex, HeightTilesF, bool, bool>) ((manager, tileAndIndex, designationHeight, upperEdge) =>
      {
        HeightTilesF height = manager.TerrainManager.GetHeight(tileAndIndex.Index);
        if (height >= designationHeight)
          return true;
        EntityId entityId;
        ILayoutEntity entity;
        return manager.OccupancyManager.TryGetAnyOccupyingEntityAt(tileAndIndex.TileCoord.ExtendHeight(height.HeightI), out entityId) && manager.EntitiesManager.TryGetEntity<ILayoutEntity>(entityId, out entity) && entity.Position3f.Height >= designationHeight;
      })), true, 2, false, TrCore.DesignationWarning__NoTower, true, false, new TerrainDesignationProto.Gfx(new ColorRgba(6340672, 176), new ColorRgba(10534976, 176), new ColorRgba(6340672, 64), new ColorRgba(16728128, 176))));
      registrator.PrototypesDb.Add<TerrainDesignationProto>(new TerrainDesignationProto(IdsCore.TerrainDesignators.LevelDesignator, Proto.CreateStr(IdsCore.TerrainDesignators.LevelDesignator, "Leveling designation"), (Func<ITerrainDesignationsManager, Tile2iAndIndex, HeightTilesF, bool, bool>) ((manager, tileAndIndex, designationHeight, upperEdge) =>
      {
        HeightTilesF height = manager.TerrainManager.GetHeight(tileAndIndex.Index);
        if (height == designationHeight)
        {
          if (upperEdge)
            return true;
          return !manager.TerrainPropsManager.TerrainTileToProp.ContainsKey(tileAndIndex.TileCoordSlim) && !manager.TreesManager.Stumps.ContainsKey(new TreeId(tileAndIndex.TileCoordSlim));
        }
        EntityId entityId;
        ILayoutEntity entity;
        return !(height > designationHeight) && manager.OccupancyManager.TryGetAnyOccupyingEntityAt(tileAndIndex.TileCoord.ExtendHeight(height.HeightI), out entityId) && manager.EntitiesManager.TryGetEntity<ILayoutEntity>(entityId, out entity) && entity.Position3f.Height >= designationHeight;
      }), Option<Func<ITerrainDesignationsManager, Tile2iAndIndex, HeightTilesF, bool, bool>>.Create((Func<ITerrainDesignationsManager, Tile2iAndIndex, HeightTilesF, bool, bool>) ((manager, tileAndIndex, designationHeight, upperEdge) =>
      {
        if (!(manager.TerrainManager.GetHeight(tileAndIndex.Index) <= designationHeight))
          return false;
        if (upperEdge)
          return true;
        return !manager.TerrainPropsManager.TerrainTileToProp.ContainsKey(tileAndIndex.TileCoordSlim) && !manager.TreesManager.Stumps.ContainsKey(new TreeId(tileAndIndex.TileCoordSlim));
      })), Option<Func<ITerrainDesignationsManager, Tile2iAndIndex, HeightTilesF, bool, bool>>.Create((Func<ITerrainDesignationsManager, Tile2iAndIndex, HeightTilesF, bool, bool>) ((manager, tileAndIndex, designationHeight, upperEdge) =>
      {
        HeightTilesF height = manager.TerrainManager.GetHeight(tileAndIndex.Index);
        if (height >= designationHeight)
          return true;
        EntityId entityId;
        ILayoutEntity entity;
        return manager.OccupancyManager.TryGetAnyOccupyingEntityAt(tileAndIndex.TileCoord.ExtendHeight(height.HeightI), out entityId) && manager.EntitiesManager.TryGetEntity<ILayoutEntity>(entityId, out entity) && entity.Position3f.Height >= designationHeight;
      })), true, 1, true, TrCore.DesignationWarning__NoTower, true, true, new TerrainDesignationProto.Gfx(ColorRgba.Empty, ColorRgba.Empty, ColorRgba.Empty, new ColorRgba(16728128, 176))));
      registrator.PrototypesDb.Add<TerrainDesignationProto>(new TerrainDesignationProto(IdsCore.TerrainDesignators.ForestryDesignator, Proto.CreateStr(IdsCore.TerrainDesignators.ForestryDesignator, "Tree planting designation"), (Func<ITerrainDesignationsManager, Tile2iAndIndex, HeightTilesF, bool, bool>) ((manager, tileAndIndex, designationHeight, upperEdge) =>
      {
        if (upperEdge)
          return true;
        return manager.TreesManager.IsGroundFertileAtPosition(tileAndIndex.TileCoord) && !manager.OccupancyManager.IsOccupied(tileAndIndex.Index);
      }), (Option<Func<ITerrainDesignationsManager, Tile2iAndIndex, HeightTilesF, bool, bool>>) Option.None, (Option<Func<ITerrainDesignationsManager, Tile2iAndIndex, HeightTilesF, bool, bool>>) Option.None, false, 1, true, TrCore.DesignationWarning__NoForestryTower, false, false, new TerrainDesignationProto.Gfx(new ColorRgba(9490496, 176), new ColorRgba(16756800, 176), new ColorRgba(8446016, 176), new ColorRgba(16728128, 176))));
    }

    public TerrainDesignationsData()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
