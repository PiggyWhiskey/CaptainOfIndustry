// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.World.LocationExtensions
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Economy;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Research;
using Mafi.Core.World;

#nullable disable
namespace Mafi.Base.Prototypes.World
{
  public static class LocationExtensions
  {
    public static WorldMapLocation AddTechnology(
      this WorldMapLocation loc,
      ProtosDb db,
      Proto.ID unlockedProtoId)
    {
      TechnologyProto orThrow = db.GetOrThrow<TechnologyProto>(unlockedProtoId);
      if (loc.Loot.HasValue)
      {
        Assert.That<ImmutableArray<TechnologyProto>>(loc.Loot.Value.ProtosToUnlock).IsEmpty<TechnologyProto>();
        loc.Loot.Value.ProtosToUnlock = ImmutableArray.Create<TechnologyProto>(orThrow);
      }
      else
        loc.Loot = (Option<WorldMapLoot>) new WorldMapLoot()
        {
          ProtosToUnlock = ImmutableArray.Create<TechnologyProto>(orThrow)
        };
      return loc;
    }

    public static WorldMapLocation AddProduct(
      this WorldMapLocation loc,
      ProtosDb db,
      ProductProto.ID productToGive,
      int quantity)
    {
      AssetValue assetValue = new AssetValue(db.GetOrThrow<ProductProto>((Proto.ID) productToGive), quantity.Quantity());
      if (loc.Loot.HasValue)
        loc.Loot.Value.Products += assetValue;
      else
        loc.Loot = (Option<WorldMapLoot>) new WorldMapLoot()
        {
          Products = assetValue
        };
      return loc;
    }
  }
}
