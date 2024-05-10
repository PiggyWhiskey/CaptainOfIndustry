// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Population.PopNeedProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Population
{
  public class PopNeedProto : Proto
  {
    /// <summary>
    /// Unity generated per fully satisfied population.
    /// Note: Don't use it without applying multipliers first!
    /// </summary>
    public readonly Upoints Unity;
    /// <summary>Health difference when this need is fully provided.</summary>
    public readonly PopNeedProto.HealthData? HealthGiven;
    public readonly PopNeedProto.Gfx Graphics;
    public readonly PropertyId<Percent>? ConsumptionMultiplierProperty;
    public readonly PropertyId<Percent>? UnityMultiplierProperty;
    public readonly UpointsCategoryProto UpointsCategory;

    public bool IsFoodNeed => this.Id.Value == "FoodNeed";

    public bool IsHealthcareNeed => this.Id.Value == "HealthCareNeed";

    public PopNeedProto(
      Proto.ID id,
      Proto.Str strings,
      Upoints unity,
      UpointsCategoryProto upointsCategory,
      PopNeedProto.HealthData? healthGiven,
      PropertyId<Percent>? consumptionMultiplierProperty,
      PropertyId<Percent>? unityMultiplierProperty,
      PopNeedProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings);
      this.Graphics = graphics;
      this.Unity = unity.CheckNotNegative();
      this.UpointsCategory = upointsCategory;
      this.HealthGiven = healthGiven;
      this.ConsumptionMultiplierProperty = consumptionMultiplierProperty;
      this.UnityMultiplierProperty = unityMultiplierProperty;
    }

    public readonly struct HealthData
    {
      public readonly Percent Diff;
      public readonly HealthPointsCategoryProto HealthPointsCategory;

      public HealthData(Percent diff, HealthPointsCategoryProto category)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Diff = diff;
        this.HealthPointsCategory = category;
      }
    }

    public new class Gfx : Proto.Gfx
    {
      public static readonly PopNeedProto.Gfx Empty;

      /// <summary>Icon asset path to be used in UI.</summary>
      public string IconPath { get; private set; }

      public Gfx(string customIconPath)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.IconPath = customIconPath.CheckNotNull<string>();
      }

      static Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        PopNeedProto.Gfx.Empty = new PopNeedProto.Gfx("EMPTY");
      }
    }
  }
}
