// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Designation.ITerrainDesignationsManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Products;
using Mafi.Core.Terrain.Props;
using Mafi.Core.Terrain.Trees;
using Mafi.Localization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Terrain.Designation
{
  public interface ITerrainDesignationsManager
  {
    bool AllowDesignationsOverEntities { get; }

    void SetAllowDesignationsOverEntities(bool value);

    TerrainManager TerrainManager { get; }

    TreesManager TreesManager { get; }

    TerrainPropsManager TerrainPropsManager { get; }

    TerrainOccupancyManager OccupancyManager { get; }

    IEntitiesManager EntitiesManager { get; }

    IEvent<TerrainDesignation> DesignationAdded { get; }

    IEvent<TerrainDesignation> DesignationRemoved { get; }

    IEvent<TerrainDesignation> DesignationManagedTowersChanged { get; }

    IEvent<TerrainDesignation> DesignationFulfilledChanged { get; }

    IReadOnlyCollection<TerrainDesignation> Designations { get; }

    IEnumerable<TerrainDesignation> SelectDesignationsInArea(Tile2i fromCoord, Tile2i toCoord);

    bool RemoveDesignation(Tile2i originCoord);

    Option<TerrainDesignation> GetDesignationAt(Tile2i coord);

    bool TryGetAnyDesignationAtVertex(Tile2i coord, out TerrainDesignation designation);

    bool TryFindBestReadyToFulfill(
      IEnumerable<TerrainDesignation> designations,
      Tile2i position,
      Vehicle servicingVehicle,
      out TerrainDesignation bestDesignation,
      Option<LooseProductProto> productToPrefer = default (Option<LooseProductProto>),
      bool tryIgnoreReservations = false,
      Predicate<TerrainDesignation> predicate = null,
      Lyst<TerrainDesignation> additionalNearbyDesignations = null,
      bool preferDesignationNearAssignedEntity = false,
      bool isMining = false,
      bool isDumping = false);

    HeightTilesI? GetDesignatedHeightAtOrigin(Tile2i origin);

    bool IsDesignationAllowed(DesignationData designationData, out LocStrFormatted error);
  }
}
