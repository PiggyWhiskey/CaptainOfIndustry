// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.PropertiesData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Mods;
using Mafi.Core.PropertiesDb;

#nullable disable
namespace Mafi.Base.Prototypes
{
  internal class PropertiesData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      registrator.PrototypesDb.Add<PercentPropertyProto>(new PercentPropertyProto(Ids.Properties.HouseholdAppliancesConsumptionMultiplier, Percent.Hundred));
      registrator.PrototypesDb.Add<PercentPropertyProto>(new PercentPropertyProto(Ids.Properties.HouseholdGoodsConsumptionMultiplier, Percent.Hundred));
      registrator.PrototypesDb.Add<PercentPropertyProto>(new PercentPropertyProto(Ids.Properties.ConsumerElectronicsConsumptionMultiplier, Percent.Hundred));
      registrator.PrototypesDb.Add<PercentPropertyProto>(new PercentPropertyProto(Ids.Properties.SettlementWaterConsumptionMultiplier, Percent.Hundred));
      registrator.PrototypesDb.Add<PercentPropertyProto>(new PercentPropertyProto(Ids.Properties.HouseholdAppliancesUnityMultiplier, Percent.Hundred));
      registrator.PrototypesDb.Add<PercentPropertyProto>(new PercentPropertyProto(Ids.Properties.HouseholdGoodsUnityMultiplier, Percent.Hundred));
      registrator.PrototypesDb.Add<PercentPropertyProto>(new PercentPropertyProto(Ids.Properties.ConsumerElectronicsUnityMultiplier, Percent.Hundred));
    }

    public PropertiesData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
