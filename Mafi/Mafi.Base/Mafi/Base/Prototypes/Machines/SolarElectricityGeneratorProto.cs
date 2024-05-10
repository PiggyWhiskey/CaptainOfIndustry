// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.SolarElectricityGeneratorProto
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Prototypes;
using System;

#nullable disable
namespace Mafi.Base.Prototypes.Machines
{
  public class SolarElectricityGeneratorProto : 
    LayoutEntityProto,
    IProtoWithUpgrade<SolarElectricityGeneratorProto>,
    IProtoWithUpgrade,
    IProto
  {
    public readonly Electricity OutputElectricity;

    public override Type EntityType => typeof (SolarElectricityGenerator);

    public UpgradeData<SolarElectricityGeneratorProto> Upgrade { get; }

    public IUpgradeData UpgradeNonGeneric => (IUpgradeData) this.Upgrade;

    public SolarElectricityGeneratorProto(
      StaticEntityProto.ID id,
      Proto.Str strings,
      EntityLayout layout,
      EntityCosts costs,
      Option<SolarElectricityGeneratorProto> nextTier,
      Electricity outputElectricity,
      LayoutEntityProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, layout, costs, graphics);
      this.Upgrade = new UpgradeData<SolarElectricityGeneratorProto>(this, nextTier);
      this.OutputElectricity = outputElectricity;
    }
  }
}
