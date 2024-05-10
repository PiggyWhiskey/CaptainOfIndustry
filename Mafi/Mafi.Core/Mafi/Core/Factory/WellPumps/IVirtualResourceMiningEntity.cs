// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.WellPumps.IVirtualResourceMiningEntity
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Products;

#nullable disable
namespace Mafi.Core.Factory.WellPumps
{
  /// <summary>Entity that mines virtual terrain resources.</summary>
  public interface IVirtualResourceMiningEntity
  {
    /// <summary>Describes to the player what is the resource state.</summary>
    string Description { get; }

    ProductProto ProductToMine { get; }

    Quantity CapacityOfMine { get; }

    Quantity QuantityLeftToMine { get; }

    bool NotifyOnLowReserve { get; }
  }
}
