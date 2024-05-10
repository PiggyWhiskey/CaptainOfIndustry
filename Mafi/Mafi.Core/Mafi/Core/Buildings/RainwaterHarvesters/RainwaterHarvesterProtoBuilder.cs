// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.RainwaterHarvesters.RainwaterHarvesterProtoBuilder
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Static;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Buildings.RainwaterHarvesters
{
  public class RainwaterHarvesterProtoBuilder : IProtoBuilder
  {
    public ProtosDb ProtosDb => this.Registrator.PrototypesDb;

    public ProtoRegistrator Registrator { get; }

    public RainwaterHarvesterProtoBuilder(ProtoRegistrator registrator)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Registrator = registrator;
    }

    public RainwaterHarvesterProtoBuilder.State Start(string name, StaticEntityProto.ID labId)
    {
      return new RainwaterHarvesterProtoBuilder.State(this, labId, name);
    }

    public class State : LayoutEntityBuilderState<RainwaterHarvesterProtoBuilder.State>
    {
      private readonly StaticEntityProto.ID m_id;
      private Option<FluidProductProto> m_waterProto;
      private Quantity? m_capacity;
      private PartialQuantity? m_collectedPerDay;

      public State(RainwaterHarvesterProtoBuilder builder, StaticEntityProto.ID id, string name)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector((IProtoBuilder) builder, id, name);
        this.m_id = id;
      }

      [MustUseReturnValue]
      public RainwaterHarvesterProtoBuilder.State SetWaterProto(Proto.ID waterProtoId)
      {
        return this.SetWaterProto(this.Builder.ProtosDb.GetOrThrow<FluidProductProto>(waterProtoId));
      }

      [MustUseReturnValue]
      public RainwaterHarvesterProtoBuilder.State SetWaterProto(FluidProductProto waterProto)
      {
        this.m_waterProto = (Option<FluidProductProto>) waterProto.CheckNotNull<FluidProductProto>();
        return this;
      }

      [MustUseReturnValue]
      public RainwaterHarvesterProtoBuilder.State SetCapacity(int quantity)
      {
        this.m_capacity = new Quantity?(new Quantity(quantity).CheckPositive());
        return this;
      }

      [MustUseReturnValue]
      public RainwaterHarvesterProtoBuilder.State SetWaterCollectedPerDay(double quantity)
      {
        this.m_collectedPerDay = new PartialQuantity?(quantity.Quantity());
        return this;
      }

      public RainwaterHarvesterProto BuildAndAdd()
      {
        return this.AddToDb<RainwaterHarvesterProto>(new RainwaterHarvesterProto(this.m_id, this.ValueOrThrow<FluidProductProto>(this.m_waterProto, "No water proto set for water collector"), this.ValueOrThrow<Quantity>(this.m_capacity, "No capacity set for water collector"), this.ValueOrThrow<PartialQuantity>(this.m_collectedPerDay, "No water collected per day set!"), this.Strings, this.LayoutOrThrow, this.Costs, this.Graphics));
      }
    }
  }
}
