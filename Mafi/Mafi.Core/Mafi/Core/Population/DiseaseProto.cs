// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Population.DiseaseProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using System;

#nullable disable
namespace Mafi.Core.Population
{
  public class DiseaseProto : Proto
  {
    public readonly Percent HealthPenalty;
    /// <summary>Morality rate (based on the total population).</summary>
    public readonly Percent MonthlyMortalityRate;
    public readonly int DurationInMonths;
    /// <summary>
    /// Minimum distance traveled on the map so this disease can be eligible for selection.
    /// </summary>
    public readonly int MinDistanceTraveled;
    public readonly LocStr Reason;

    public virtual Option<Type> CustomTrigger => Option<Type>.None;

    public DiseaseProto(
      Proto.ID id,
      Proto.Str strings,
      LocStr reason,
      Percent healthPenalty,
      Percent monthlyMortalityRate,
      int durationInMonths,
      int minDistanceTraveled)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings);
      this.Reason = reason;
      this.HealthPenalty = healthPenalty.CheckPositive();
      this.MonthlyMortalityRate = monthlyMortalityRate.CheckNotNegative();
      this.DurationInMonths = durationInMonths.CheckPositive();
      this.MinDistanceTraveled = minDistanceTraveled;
    }
  }
}
