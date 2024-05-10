// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Designation.IDesignation
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Prototypes;
using Mafi.Localization;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Terrain.Designation
{
  public interface IDesignation : IIsSafeAsHashKey
  {
    LocStrFormatted Name { get; }

    int SizeTiles { get; }

    Proto.ID ProtoId { get; }

    bool IsFulfilled { get; }

    bool IsNotFulfilled { get; }

    bool IsFulfilledAt(RelTile2i coord);

    bool IsReadyToBeFulfilled { get; }

    bool IsDestroyed { get; }

    Tile2i OriginTileCoord { get; }

    Tile2i CenterTileCoord { get; }

    Tile3f GetTargetCoordAt(RelTile2i position);

    HeightTilesF GetTargetHeightAt(Tile2i position);

    HeightTilesF GetTargetHeightAt(RelTile2i position);

    bool ContainsPosition(Tile2i position);

    ushort UnreachableVehiclesCount { get; set; }
  }
}
