// Decompiled with JetBrains decompiler
// Type: Mafi.Core.World.Entities.WorldMapVillageProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.World.Contracts;
using Mafi.Core.World.QuickTrade;
using Mafi.Localization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.World.Entities
{
  public class WorldMapVillageProto : WorldMapEntityProto
  {
    public readonly Upoints UpointsPerPopToAdopt;
    /// <summary>Donation cost per reputation level.</summary>
    public readonly Func<int, AssetValue> CostPerLevel;
    public ImmutableArray<QuickTradePairProto> QuickTrades;
    public ImmutableArray<ContractProto> Contracts;
    /// <summary>Products that this village can lend to the player.</summary>
    public ImmutableArray<WorldMapVillageProto.ProductToLend> ProductsToLend;
    /// <summary>Max upgrade level this village can reach.</summary>
    public readonly int MaxReputation;
    public readonly int StartingReputation;
    /// <summary>-1 if adoption disabled entirely</summary>
    public readonly int MinReputationNeededToAdopt;
    /// <summary>
    /// Returns duration it takes to get additional pop for adoption per each reputation level.
    /// </summary>
    public readonly Func<int, Duration> DurationPerNewPopPerReputationLevel;

    public override Type EntityType => typeof (WorldMapVillage);

    public WorldMapVillageProto(
      EntityProto.ID id,
      Proto.Str strings,
      int minReputationNeededToAdopt,
      int startingReputation,
      Upoints upointsPerPopToAdopt,
      Func<int, AssetValue> costPerLevel,
      ImmutableArray<QuickTradePairProto> quickTrades,
      ImmutableArray<ContractProto> contracts,
      ImmutableArray<WorldMapVillageProto.ProductToLend> productsToLend,
      WorldMapEntityProto.Gfx graphics,
      IEnumerable<Tag> tags = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.DurationPerNewPopPerReputationLevel = (Func<int, Duration>) (level => 4.Minutes() - (level * 30.Seconds()).Min(2.Minutes()));
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, EntityCosts.None, graphics, tags);
      this.MinReputationNeededToAdopt = minReputationNeededToAdopt;
      this.CostPerLevel = costPerLevel;
      this.StartingReputation = startingReputation;
      this.UpointsPerPopToAdopt = upointsPerPopToAdopt;
      this.QuickTrades = quickTrades.CheckNotDefaultStruct<ImmutableArray<QuickTradePairProto>>();
      this.Contracts = contracts.CheckNotDefaultStruct<ImmutableArray<ContractProto>>();
      this.ProductsToLend = productsToLend;
      int num = minReputationNeededToAdopt < 0 ? 0 : 4;
      if (quickTrades.IsNotEmpty)
        num = quickTrades.Max<int>((Func<QuickTradePairProto, int>) (x => x.MinReputationRequired)).Max(num);
      if (contracts.IsNotEmpty)
        num = contracts.Max<int>((Func<ContractProto, int>) (x => x.MinReputationRequired)).Max(num);
      this.MaxReputation = num;
    }

    /// <summary>
    /// Returns capacity of pops available to adopt per each reputation level.
    /// </summary>
    public int PopsToAdoptCapPerReputationLevel(int level)
    {
      return level < this.MinReputationNeededToAdopt ? 0 : 10 * level;
    }

    public LocStrFormatted GetTitleFor(int reputation)
    {
      return this.MaxReputation > 0 ? TrCore.SettlementTitleWithReputation.Format(this.Strings.Name.TranslatedString, reputation.ToString()) : (LocStrFormatted) this.Strings.Name;
    }

    public LocStrFormatted GetUpgradeTitle(int reputation)
    {
      return TrCore.ReputationIncreaseTitle.Format(reputation.ToString());
    }

    public readonly struct ProductToLend
    {
      public readonly ProductProto Product;
      public readonly bool BorrowFromStart;

      public ProductToLend(ProductProto product, bool borrowFromStart)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Product = product;
        this.BorrowFromStart = borrowFromStart;
      }
    }
  }
}
