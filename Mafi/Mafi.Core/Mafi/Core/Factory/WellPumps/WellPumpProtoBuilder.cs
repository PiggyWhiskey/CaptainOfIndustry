// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.WellPumps.WellPumpProtoBuilder
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Factory.WellPumps
{
  public sealed class WellPumpProtoBuilder : MachineProtoBuilder
  {
    public WellPumpProtoBuilder(ProtoRegistrator registrator)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(registrator);
    }

    public WellPumpProtoBuilder.State Start(
      string name,
      string resourceDescription,
      MachineProto.ID machineId)
    {
      return new WellPumpProtoBuilder.State(this, machineId, name, resourceDescription);
    }

    public class State : MachineProtoBuilder.StateBase<WellPumpProtoBuilder.State>
    {
      private Option<VirtualResourceProductProto> m_minedProduct;
      private Percent m_notifyWhenBelow;
      private readonly string m_resourceDescription;

      public State(
        WellPumpProtoBuilder builder,
        MachineProto.ID protoId,
        string name,
        string resourceDescription)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.m_notifyWhenBelow = 20.Percent();
        // ISSUE: explicit constructor call
        base.\u002Ector((MachineProtoBuilder) builder, protoId, name, "pump name");
        this.m_resourceDescription = resourceDescription;
      }

      [MustUseReturnValue]
      public WellPumpProtoBuilder.State SetMinedProduct(Proto.ID productId)
      {
        this.m_minedProduct = (Option<VirtualResourceProductProto>) this.Builder.ProtosDb.GetOrThrow<VirtualResourceProductProto>(productId);
        return this.ShowVirtualResourcesOnCreation(this.m_minedProduct.Value);
      }

      [MustUseReturnValue]
      public WellPumpProtoBuilder.State NotifyWhenBelow(Percent notifyWhenBelow)
      {
        this.m_notifyWhenBelow = notifyWhenBelow;
        return this;
      }

      public WellPumpProto BuildAndAdd()
      {
        return this.AddToDb<WellPumpProto>(new WellPumpProto(this.ProtoId, this.Strings, this.m_resourceDescription, this.LayoutOrThrow, this.Costs, this.Electricity, this.ValueOrThrow<VirtualResourceProductProto>(this.m_minedProduct, "Mined product has to be set."), this.m_notifyWhenBelow, this.BuffersMultiplier, this.AnimationParams.ToImmutableArray(), this.CreateMachineGfx()));
      }
    }
  }
}
