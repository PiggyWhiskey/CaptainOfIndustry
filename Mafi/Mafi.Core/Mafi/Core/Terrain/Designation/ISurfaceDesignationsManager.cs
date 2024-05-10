// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Designation.ISurfaceDesignationsManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Products;
using Mafi.Core.Vehicles.Trucks;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Terrain.Designation
{
  public interface ISurfaceDesignationsManager
  {
    /// <summary>
    /// Finds the closest designation that is ready to be concreted.
    /// </summary>
    bool TryFindClosestReadyToPlace(
      ProductProto product,
      Tile2i position,
      Truck servicingVehicle,
      Quantity maxQuantity,
      Lyst<SurfaceDesignation> resultDesignations,
      out Quantity toExchange);

    bool TryFindClosestReadyToClear(
      ProductProto product,
      Tile2i position,
      Truck servicingVehicle,
      Quantity maxQuantity,
      Lyst<SurfaceDesignation> resultDesignations,
      out Quantity toExchange);

    bool TryGetDataFor(ProductProto product, out DesignationsPerProductCache data);

    IReadOnlyCollection<SurfaceDesignation> PlacingDesignations { get; }

    IReadOnlyCollection<SurfaceDesignation> ClearingDesignations { get; }

    Option<SurfaceDesignation> GetDesignationAt(Tile2i coord);

    bool TryGetEntityOccupyingAt(
      Tile2iAndIndex tileAndIndex,
      out OccupiedTileRelative occupiedTile,
      out IStaticEntity entity,
      out IStaticEntityProto entityProto);

    bool CanPlaceSurfaceTile(Tile2i tile);

    bool RemoveDesignation(Tile2i originCoord);
  }
}
