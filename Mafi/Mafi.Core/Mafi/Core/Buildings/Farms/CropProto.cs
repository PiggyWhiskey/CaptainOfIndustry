// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Farms.CropProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Buildings.Farms
{
  public class CropProto : Proto, IProtoWithIcon, IProto
  {
    /// <summary>Can be none if nothing is produced.</summary>
    public readonly ProductQuantity ProductProduced;
    /// <summary>Consumed water to grow the crop (should be positive).</summary>
    public readonly PartialQuantity ConsumedWaterPerDay;
    /// <summary>
    /// Consumed fertility per month (may be negative for crops that increase farm fertility).
    /// </summary>
    public readonly Percent ConsumedFertilityPerDay;
    /// <summary>Minimum fertility level to start growth.</summary>
    public readonly Percent MinFertilityToStartGrowth;
    /// <summary>How many days this crop will grow.</summary>
    public readonly int DaysToGrow;
    /// <summary>
    /// Consecutive days the crop can survive without water. If null, crop never dies by lack of water.
    /// </summary>
    public readonly int? DaysToSurviveWithNoWater;
    public readonly CropProto.Gfx Graphics;
    public readonly bool IsEmptyCrop;
    public readonly bool RequiresGreenhouse;
    /// <summary>Used when crop rotation is not available yet.</summary>
    public readonly bool PlantByDefault;

    public string IconPath => this.Graphics.IconPath;

    public int MonthsToGrow => this.DaysToGrow / 30;

    public CropProto(
      Proto.ID id,
      Proto.Str strings,
      ProductQuantity productProduced,
      PartialQuantity consumedWaterPerDay,
      Percent consumedFertilityPerDay,
      Percent minFertilityToStartGrowth,
      Duration growthDuration,
      Duration? surviveWithNoWaterDuration,
      CropProto.Gfx graphics,
      bool? requiresGreenhouse = false,
      bool? plantByDefault = false)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings);
      this.ProductProduced = productProduced;
      this.ConsumedWaterPerDay = consumedWaterPerDay;
      this.ConsumedFertilityPerDay = consumedFertilityPerDay;
      this.MinFertilityToStartGrowth = minFertilityToStartGrowth;
      this.DaysToGrow = growthDuration.Days.ToIntCeiled().CheckPositive();
      this.DaysToSurviveWithNoWater = surviveWithNoWaterDuration.HasValue ? new int?(surviveWithNoWaterDuration.Value.Days.ToIntCeiled().CheckPositive()) : new int?();
      this.Graphics = graphics;
      this.RequiresGreenhouse = requiresGreenhouse.GetValueOrDefault();
      this.PlantByDefault = plantByDefault.GetValueOrDefault();
      this.IsEmptyCrop = productProduced.IsEmpty && consumedWaterPerDay.IsZero && consumedFertilityPerDay.IsZero;
    }

    public ProductQuantity GetProductProduced(FarmProto farm)
    {
      return this.ProductProduced.ScaledBy(farm.YieldMultiplier);
    }

    public Percent GetConsumedFertilityPerDay(FarmProto farm)
    {
      return this.ConsumedFertilityPerDay.IsNegative ? farm.YieldMultiplier.Apply(this.ConsumedFertilityPerDay) : farm.DemandsMultiplier.Apply(this.ConsumedFertilityPerDay);
    }

    public PartialQuantity GetConsumedWaterPerDay(FarmProto farm)
    {
      return this.ConsumedWaterPerDay.ScaledBy(farm.DemandsMultiplier);
    }

    public new class Gfx : Proto.Gfx
    {
      public static readonly CropProto.Gfx Empty;
      /// <summary>Icon asset path to be used in UI.</summary>
      public readonly string IconPath;
      public readonly string PrefabPath;
      public readonly float ScaleVariation;
      public readonly float WindTimeScale;
      public readonly float WindWaviness;
      public readonly float WindAmplitude;

      public Gfx(
        string iconPath,
        string prefabPath,
        float scaleVariation,
        float windTimeScale,
        float windWaviness,
        float windAmplitude)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.IconPath = iconPath;
        this.PrefabPath = prefabPath;
        this.WindTimeScale = windTimeScale;
        this.WindWaviness = windWaviness;
        this.WindAmplitude = windAmplitude;
        this.ScaleVariation = scaleVariation;
      }

      static Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        CropProto.Gfx.Empty = new CropProto.Gfx("EMPTY", "EMPTY", 0.0f, 1f, 1f, 1f);
      }
    }
  }
}
