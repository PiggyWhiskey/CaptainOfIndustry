// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Beacons.BeaconProtoBuilder
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Static;
using Mafi.Core.Mods;
using Mafi.Core.Population;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Buildings.Beacons
{
  public class BeaconProtoBuilder : IProtoBuilder
  {
    public ProtosDb ProtosDb => this.Registrator.PrototypesDb;

    public ProtoRegistrator Registrator { get; }

    public BeaconProtoBuilder(ProtoRegistrator registrator)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Registrator = registrator;
    }

    public BeaconProtoBuilder.State Start(string name, StaticEntityProto.ID labId)
    {
      return new BeaconProtoBuilder.State(this, labId, name);
    }

    public class State : LayoutEntityBuilderState<BeaconProtoBuilder.State>
    {
      private readonly StaticEntityProto.ID m_id;
      private int? m_tier;
      private Upoints m_unityMonthlyCost;
      private Option<BeaconProto> m_nextTier;
      private Option<UpointsCategoryProto> m_upointsCategory;
      private Electricity m_electricityConsumed;

      public State(BeaconProtoBuilder builder, StaticEntityProto.ID id, string name)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.m_unityMonthlyCost = Upoints.Zero;
        this.m_electricityConsumed = Electricity.Zero;
        // ISSUE: explicit constructor call
        base.\u002Ector((IProtoBuilder) builder, id, name);
        this.m_id = id;
      }

      [MustUseReturnValue]
      public BeaconProtoBuilder.State SetElectricityConsumed(Electricity electricityConsumed)
      {
        this.m_electricityConsumed = electricityConsumed;
        return this;
      }

      [MustUseReturnValue]
      public BeaconProtoBuilder.State SetTier(int tier)
      {
        this.m_tier = new int?(tier);
        return this;
      }

      [MustUseReturnValue]
      public BeaconProtoBuilder.State SetUnityMonthlyCost(
        Upoints unityMonthlyCost,
        UpointsCategoryProto upointsCategory)
      {
        this.m_unityMonthlyCost = unityMonthlyCost;
        this.m_upointsCategory = (Option<UpointsCategoryProto>) upointsCategory;
        return this;
      }

      public BeaconProto BuildAndAdd()
      {
        return this.AddToDb<BeaconProto>(new BeaconProto(this.m_id, this.Strings, this.LayoutOrThrow, this.m_electricityConsumed, this.Costs, this.m_unityMonthlyCost, this.ValueOrThrow<UpointsCategoryProto>(this.m_upointsCategory, "upointsCategory not set"), this.ValueOrThrow<int>(this.m_tier, "Tier not set!"), this.Graphics));
      }
    }
  }
}
