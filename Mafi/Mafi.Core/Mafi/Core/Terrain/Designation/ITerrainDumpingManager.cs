// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Designation.ITerrainDumpingManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Products;
using Mafi.Core.Vehicles;
using Mafi.Core.Vehicles.Trucks;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Terrain.Designation
{
  public interface ITerrainDumpingManager
  {
    ImmutableArray<LooseProductProto> AllDumpableProducts { get; }

    IReadOnlySet<ProductProto> ProductsAllowedToDump { get; }

    IEnumerable<TerrainDesignation> DumpingDesignations { get; }

    int Count { get; }

    Option<TerrainDesignation> GetDumpingDesignationAt(Tile2i coord);

    /// <summary>
    /// Finds the closest designation that is ready to be dumped at.
    /// If the <paramref name="additionalNearbyDesignations" /> is set and some designation is found, additional
    /// designations around in radius of <paramref name="alsoSearchNearbyInRadius" /> designations will be added.
    /// </summary>
    bool TryFindClosestReadyToDump(
      Tile2i position,
      Option<LooseProductProto> product,
      Truck servicingVehicle,
      out TerrainDesignation bestDesignation,
      bool tryIgnoreReservations = false,
      Predicate<TerrainDesignation> predicate = null,
      Lyst<TerrainDesignation> additionalNearbyDesignations = null);

    bool TryFindClosestReadyToDump(
      Tile2i position,
      RegisteredOutputBuffer outputBuffer,
      Option<LooseProductProto> product,
      Truck servicingVehicle,
      out TerrainDesignation bestDesignation,
      Lyst<TerrainDesignation> additionalNearbyDesignations = null);

    /// <summary>Makes the given product to be allowed to be dumped.</summary>
    void AddProductToDump(LooseProductProto product);

    /// <summary>
    /// Removes the given product from the allowed dumpable products.
    /// </summary>
    void RemoveProductToDump(LooseProductProto product);

    bool HasEligibleDumpingDesignationsFor(LooseProductProto product);
  }
}
