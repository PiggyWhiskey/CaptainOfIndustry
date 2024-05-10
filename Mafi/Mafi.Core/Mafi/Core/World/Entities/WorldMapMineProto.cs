// Decompiled with JetBrains decompiler
// Type: Mafi.Core.World.Entities.WorldMapMineProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Population;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.World.Entities
{
  public class WorldMapMineProto : WorldMapEntityProto
  {
    public readonly ProductQuantity ProducedProductPerStep;
    public readonly Duration ProductionDuration;
    public readonly UpointsCategoryProto UpointsCategory;
    /// <summary>Upoints cost of this mine per production level</summary>
    public readonly Upoints MonthlyUpointsPerLevel;
    /// <summary>
    /// Upgrade cost, workers cost, maintenance cost of this mine per production level
    /// </summary>
    public readonly Func<int, EntityCosts> CostPerLevel;
    /// <summary>Starting level this mine will have after repair.</summary>
    public readonly int Level;
    /// <summary>Max upgrade level this mine can reach.</summary>
    public readonly int MaxLevel;
    /// <summary>By how many level we jump per each upgrade.</summary>
    public readonly int LevelsPerUpgrade;
    /// <summary>Null if unlimited.</summary>
    public readonly Quantity? QuantityAvailable;

    public override Type EntityType => typeof (WorldMapMine);

    public WorldMapMineProto(
      EntityProto.ID id,
      Proto.Str strings,
      ProductQuantity producedProductPerStep,
      Duration productionDuration,
      Upoints monthlyUpointsPerLevel,
      UpointsCategoryProto upointsCategory,
      EntityCosts costs,
      Func<int, EntityCosts> costPerLevel,
      int maxLevel,
      Quantity? quantityAvailable,
      WorldMapEntityProto.Gfx graphics,
      int levelsPerUpgrade = 2,
      int? startingLevel = null,
      IEnumerable<Tag> tags = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, costs, graphics, tags);
      this.ProducedProductPerStep = producedProductPerStep;
      this.ProductionDuration = productionDuration;
      this.MonthlyUpointsPerLevel = monthlyUpointsPerLevel;
      this.CostPerLevel = costPerLevel;
      this.MaxLevel = maxLevel;
      this.LevelsPerUpgrade = levelsPerUpgrade.CheckPositive();
      this.Level = startingLevel.HasValue ? startingLevel.GetValueOrDefault().CheckGreaterOrEqual(1) : this.LevelsPerUpgrade;
      this.UpointsCategory = upointsCategory;
      this.QuantityAvailable = quantityAvailable;
      if (this.MaxLevel % this.LevelsPerUpgrade != 0)
        throw new ProtoBuilderException(string.Format("{0}: MaxLevel '{1}' cannot be divided by {2}", (object) id, (object) this.MaxLevel, (object) this.LevelsPerUpgrade));
      if (this.Level % this.LevelsPerUpgrade != 0)
        throw new ProtoBuilderException(string.Format("{0}: Starting level '{1}' cannot be divided by {2}", (object) id, (object) this.Level, (object) this.LevelsPerUpgrade));
    }

    public LocStrFormatted GetTitleFor(int level)
    {
      return TrCore.WorldMineTitleWithLevel.Format(this.Strings.Name.TranslatedString, level.ToString());
    }
  }
}
