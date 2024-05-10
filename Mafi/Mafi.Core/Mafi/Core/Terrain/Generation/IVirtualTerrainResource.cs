// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.IVirtualTerrainResource
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Products;

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  /// <summary>
  /// Terrain resource that is not physically present in the terrain tiles but is still mineable through special API.
  /// </summary>
  public interface IVirtualTerrainResource : ITerrainResource
  {
    VirtualResourceProductProto Product { get; }

    /// <summary>
    /// Capacity as configured by the map. Actual capacity may differ based on game difficulty settings.
    /// </summary>
    Quantity ConfiguredCapacity { get; }

    Quantity Capacity { get; }

    Quantity Quantity { get; }

    /// <summary>
    /// Special quantity that is generated when resource is depleted.
    /// Used by easy difficulty for ground water.
    /// </summary>
    PartialQuantity EmergencyQuantity { get; set; }

    /// <summary>Whether this resource is present at given tile.</summary>
    /// <remarks>Use this to filter all usable resources for particular tile.</remarks>
    bool IsAt(Tile2i position);

    void AddAsMuchAs(Quantity quantity);

    /// <summary>Returns approximate thickness at given tile.</summary>
    /// <remarks>
    /// This is for the resources exploration API. Measuring virtual resources in thickness might not be sensible so
    /// thi method returns approximate thickness so that API for terrain resources and virtual resources matches.
    /// </remarks>
    ThicknessTilesF GetApproxThicknessAt(Tile2i position);

    /// <summary>Mines this resource at given tile.</summary>
    /// <remarks>
    /// The <paramref name="maxQuantity" /> should represent reasonable max mined amount for the particular mining
    /// approach. The resource implementation may choose to return less than than but not more.
    /// </remarks>
    ProductQuantity MineResourceAt(Tile2i position, Quantity maxQuantity);
  }
}
