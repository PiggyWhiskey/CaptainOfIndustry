// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.RuinedBuildings.RuinsProtoBuilder
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Economy;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Buildings.RuinedBuildings
{
  public class RuinsProtoBuilder : IProtoBuilder
  {
    public ProtosDb ProtosDb => this.Registrator.PrototypesDb;

    public ProtoRegistrator Registrator { get; }

    public RuinsProtoBuilder(ProtoRegistrator registrator)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Registrator = registrator;
    }

    public RuinsProtoBuilder.State Start(string name, StaticEntityProto.ID labId)
    {
      return new RuinsProtoBuilder.State(this, labId, name);
    }

    public class State : LayoutEntityBuilderState<RuinsProtoBuilder.State>
    {
      private readonly StaticEntityProto.ID m_id;
      private Duration? m_durationPerProduct;
      private AssetValue? m_productsGiven;

      public State(RuinsProtoBuilder builder, StaticEntityProto.ID id, string name)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector((IProtoBuilder) builder, id, name);
        this.m_id = id;
      }

      [MustUseReturnValue]
      public RuinsProtoBuilder.State SetDeconstructionParams(
        AssetValue productGiven,
        Duration durationPerProduct)
      {
        Assert.That<bool>(productGiven.IsNotEmpty).IsTrue();
        Assert.That<Duration>(durationPerProduct).IsPositive();
        this.m_durationPerProduct = new Duration?(durationPerProduct);
        this.m_productsGiven = new AssetValue?(productGiven);
        return this;
      }

      public RuinsProto BuildAndAdd()
      {
        StaticEntityProto.ID id = this.m_id;
        Proto.Str strings = this.Strings;
        EntityLayout layoutOrThrow = this.LayoutOrThrow;
        Duration durationPerProduct = this.ValueOrThrow<Duration>(this.m_durationPerProduct, "DurationPerProduct not set!");
        AssetValue productsGiven = this.ValueOrThrow<AssetValue>(this.m_productsGiven, "ProductGiven not set!");
        EntityLayout layout = layoutOrThrow;
        EntityCosts none = EntityCosts.None;
        LayoutEntityProto.Gfx graphics = this.Graphics;
        return this.AddToDb<RuinsProto>(new RuinsProto(id, strings, durationPerProduct, productsGiven, layout, none, graphics));
      }
    }
  }
}
