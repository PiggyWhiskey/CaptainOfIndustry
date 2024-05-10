// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Cargo.Modules.CargoDepotModuleProtoBuilder
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Buildings.Cargo.Modules
{
  public sealed class CargoDepotModuleProtoBuilder : IProtoBuilder
  {
    public ProtosDb ProtosDb => this.Registrator.PrototypesDb;

    public ProtoRegistrator Registrator { get; }

    public CargoDepotModuleProtoBuilder(ProtoRegistrator registrator)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Registrator = registrator;
    }

    public CargoDepotModuleProtoBuilder.State Start(string name, CargoDepotModuleProto.ID protoId)
    {
      return new CargoDepotModuleProtoBuilder.State(this, protoId, name);
    }

    public class State : LayoutEntityBuilderState<CargoDepotModuleProtoBuilder.State>
    {
      private readonly CargoDepotModuleProto.ID m_protoId;
      private Quantity? m_capacity;
      private Quantity? m_quantityPerExchange;
      private Duration? m_durationPerExchange;
      private ProductType? m_productType;
      private Option<CargoDepotModuleProto> m_nextTier;
      private Electricity m_consumedPowerForCranePerTick;
      private Option<string> m_cranePrefabPath;
      private Percent m_percentOfAnimationToDropCargoToShip;
      private bool m_hasCraneAnimation;

      public State(
        CargoDepotModuleProtoBuilder builder,
        CargoDepotModuleProto.ID protoId,
        string name)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector((IProtoBuilder) builder, (StaticEntityProto.ID) protoId, name);
        this.m_protoId = protoId;
      }

      [MustUseReturnValue]
      public CargoDepotModuleProtoBuilder.State SetCapacity(int capacity)
      {
        this.m_capacity = new Quantity?(new Quantity(capacity).CheckPositive());
        return this;
      }

      [MustUseReturnValue]
      public CargoDepotModuleProtoBuilder.State SetExchangeParams(
        int quantityPerExchange,
        Duration duration)
      {
        this.m_quantityPerExchange = new Quantity?(new Quantity(quantityPerExchange).CheckPositive());
        this.m_durationPerExchange = new Duration?(duration);
        return this;
      }

      [MustUseReturnValue]
      public CargoDepotModuleProtoBuilder.State SetNextTier(CargoDepotModuleProto.ID moduleId)
      {
        this.m_nextTier = (Option<CargoDepotModuleProto>) this.Builder.ProtosDb.GetOrThrow<CargoDepotModuleProto>((Proto.ID) moduleId);
        return this;
      }

      [MustUseReturnValue]
      public CargoDepotModuleProtoBuilder.State SetProductType(ProductType productType)
      {
        this.m_productType = new ProductType?(productType);
        return this;
      }

      [MustUseReturnValue]
      public CargoDepotModuleProtoBuilder.State SetPowerConsumedByCrane(
        Electricity consumedPowerForCranePerTick)
      {
        this.m_consumedPowerForCranePerTick = consumedPowerForCranePerTick;
        return this;
      }

      [MustUseReturnValue]
      public CargoDepotModuleProtoBuilder.State SetCraneGraphics(
        string cranePrefabPath,
        Percent percentOfAnimationToDropCargoToShip)
      {
        this.m_cranePrefabPath = (Option<string>) cranePrefabPath;
        this.m_hasCraneAnimation = true;
        this.m_percentOfAnimationToDropCargoToShip = percentOfAnimationToDropCargoToShip;
        return this;
      }

      [MustUseReturnValue]
      public CargoDepotModuleProtoBuilder.State SetPumpCraneGraphics(string cranePrefabPath)
      {
        this.m_cranePrefabPath = (Option<string>) cranePrefabPath;
        this.m_percentOfAnimationToDropCargoToShip = 50.Percent();
        return this;
      }

      private CargoDepotModuleProto.Gfx createGraphics()
      {
        return !string.IsNullOrEmpty(this.PrefabPath) && !this.m_cranePrefabPath.IsNone ? new CargoDepotModuleProto.Gfx(this.PrefabPath, this.PrefabOrigin, this.m_cranePrefabPath.Value, this.CustomIconPath, ColorRgba.Empty, this.GetCategoriesOrThrow()) : CargoDepotModuleProto.Gfx.Empty;
      }

      public CargoDepotModuleProto BuildAndAdd()
      {
        CargoDepotModuleProto.ID protoId = this.m_protoId;
        Proto.Str strings = this.Strings;
        EntityLayout layoutOrThrow = this.LayoutOrThrow;
        ProductType productType = this.ValueOrThrow<ProductType>(this.m_productType, "Product type");
        Option<CargoDepotModuleProto> nextTier = this.m_nextTier;
        Quantity capacity = this.ValueOrThrow<Quantity>(this.m_capacity, "Capacity");
        Quantity quantityPerExchange = this.ValueOrThrow<Quantity>(this.m_quantityPerExchange, "Quantity per exchange");
        Duration durationPerExchange = this.ValueOrThrow<Duration>(this.m_durationPerExchange, "Duration per exchange");
        Electricity powerForCranePerTick = this.m_consumedPowerForCranePerTick;
        Percent toDropCargoToShip = this.m_percentOfAnimationToDropCargoToShip;
        int num = this.m_hasCraneAnimation ? 1 : 0;
        Percent percentOfAnimationToDropCargoToShip = toDropCargoToShip;
        EntityCosts costs = this.Costs;
        CargoDepotModuleProto.Gfx graphics = this.createGraphics();
        return this.AddToDb<CargoDepotModuleProto>(new CargoDepotModuleProto(protoId, strings, layoutOrThrow, productType, nextTier, capacity, quantityPerExchange, durationPerExchange, powerForCranePerTick, num != 0, percentOfAnimationToDropCargoToShip, costs, graphics));
      }
    }
  }
}
