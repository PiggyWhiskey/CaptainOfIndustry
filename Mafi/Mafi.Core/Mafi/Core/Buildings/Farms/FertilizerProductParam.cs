// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Farms.FertilizerProductParam
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Farms
{
  /// <summary>
  /// Adds fertility parameter to a product proto. This makes marked product acceptable by farm as a fertilizer.
  /// </summary>
  public sealed class FertilizerProductParam : IProtoParam
  {
    /// <summary>Amount of fertility provided by one quantity.</summary>
    public readonly Percent FertilityPerQuantity;
    /// <summary>Max fertility provided by this fertilizer.</summary>
    public readonly Percent MaxFertility;

    public Type AllowedProtoType => typeof (FluidProductProto);

    /// <summary>
    /// Water pollution per used fertilizer (quantity of polluted water).
    /// </summary>
    public FertilizerProductParam(Percent fertilityPerQuantity, Percent maxFertility)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.FertilityPerQuantity = fertilityPerQuantity;
      this.MaxFertility = maxFertility;
    }
  }
}
