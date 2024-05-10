// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Population.MedicalSuppliesParam
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System;

#nullable disable
namespace Mafi.Core.Population
{
  public class MedicalSuppliesParam : IProtoParam
  {
    private readonly Upoints UpointsWhenProvided;
    public readonly Percent HealthPointsWhenProvided;
    /// <summary>Value will be subtracted from current mortality.</summary>
    public readonly Percent MortalityDeductionWhenProvided;

    public Type AllowedProtoType => typeof (CountableProductProto);

    public MedicalSuppliesParam(
      Upoints upointsWhenProvided,
      Percent healthPointsWhenProvided,
      Percent mortalityDeductionWhenProvided)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.UpointsWhenProvided = upointsWhenProvided.CheckNotNegative();
      this.HealthPointsWhenProvided = healthPointsWhenProvided.CheckNotNegative();
      this.MortalityDeductionWhenProvided = mortalityDeductionWhenProvided.CheckNotNegative();
    }

    public Upoints GetUnityWhenProvided(Percent multiplier)
    {
      return this.UpointsWhenProvided.ScaledBy(multiplier);
    }
  }
}
