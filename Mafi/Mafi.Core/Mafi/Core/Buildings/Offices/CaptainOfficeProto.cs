// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Offices.CaptainOfficeProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Prototypes;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Offices
{
  public class CaptainOfficeProto : 
    LayoutEntityProto,
    IProtoWithUpgrade<CaptainOfficeProto>,
    IProtoWithUpgrade,
    IProto,
    IProtoWithPowerConsumption
  {
    public readonly int? EmissionIntensity;
    /// <summary>Whether it can process advanced edicts.</summary>
    public readonly bool SupportsAdvancedEdicts;
    /// <summary>Allows to increase volume for quick trades.</summary>
    public readonly Percent TradeVolumeDiff;

    public override Type EntityType => typeof (CaptainOffice);

    public UpgradeData<CaptainOfficeProto> Upgrade { get; }

    public IUpgradeData UpgradeNonGeneric => (IUpgradeData) this.Upgrade;

    public Electricity ElectricityConsumed { get; }

    public CaptainOfficeProto(
      StaticEntityProto.ID id,
      Proto.Str strings,
      EntityLayout layout,
      EntityCosts costs,
      Electricity electricityConsumed,
      Percent tradeVolumeDiff,
      Option<CaptainOfficeProto> nextTier,
      bool supportsAdvancedEdicts,
      int? emissionIntensity,
      LayoutEntityProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, layout, costs, graphics, isUnique: true);
      this.ElectricityConsumed = electricityConsumed;
      this.Upgrade = new UpgradeData<CaptainOfficeProto>(this, nextTier);
      this.SupportsAdvancedEdicts = supportsAdvancedEdicts;
      this.EmissionIntensity = emissionIntensity;
      this.TradeVolumeDiff = tradeVolumeDiff.CheckNotNegative();
    }
  }
}
