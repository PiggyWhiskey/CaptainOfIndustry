// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Population.Edicts.FoodConsumptionEdictProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Prototypes;
using System;

#nullable disable
namespace Mafi.Core.Population.Edicts
{
  /// <summary>
  /// Increases consumption of food in exchange for more unity.
  /// </summary>
  public class FoodConsumptionEdictProto : EdictProto
  {
    public readonly Percent ConsumptionDiff;

    public FoodConsumptionEdictProto(
      Proto.ID id,
      Proto.Str strings,
      EdictCategoryProto category,
      Upoints monthlyUpointsCost,
      Type edictImplementation,
      Percent consumptionDiff,
      Option<EdictProto> previousTier,
      EdictProto.Gfx graphics,
      bool? isGeneratingUnity = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, category, monthlyUpointsCost, edictImplementation, graphics, isGeneratingUnity, new Option<EdictProto>?(previousTier));
      this.ConsumptionDiff = consumptionDiff;
      Assert.That<bool>(consumptionDiff.IsPositive && this.MonthlyUpointsCost.IsNegative || consumptionDiff.IsNegative && this.MonthlyUpointsCost.IsPositive);
    }
  }
}
