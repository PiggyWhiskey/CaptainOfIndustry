// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Cargo.Ships.CargoShipProtoBuilder
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Buildings.Cargo.Ships.Modules;
using Mafi.Core.Entities;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Buildings.Cargo.Ships
{
  public class CargoShipProtoBuilder : IProtoBuilder
  {
    public ProtosDb ProtosDb => this.Registrator.PrototypesDb;

    public ProtoRegistrator Registrator { get; }

    public CargoShipProtoBuilder(ProtoRegistrator registrator)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Registrator = registrator;
    }

    public CargoShipProtoBuilder.State Start(string name, EntityProto.ID cargoShipId)
    {
      return new CargoShipProtoBuilder.State(this, cargoShipId, name);
    }

    public class State : ProtoBuilderState<CargoShipProtoBuilder.State>
    {
      private readonly EntityProto.ID m_id;
      private EntityCosts m_costs;
      private int m_maximumModulesCount;
      private RelTile2f m_dockOffset;
      private ImmutableArray<CargoShipModuleProto> m_availableModules;
      private ImmutableArray<CargoShipProto.FuelData> m_availableFuels;
      private CargoShipProto.Gfx m_graphics;

      public State(
        CargoShipProtoBuilder builder,
        EntityProto.ID id,
        string name,
        string translationComment = "ship name")
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.m_costs = EntityCosts.None;
        this.m_graphics = CargoShipProto.Gfx.EMPTY;
        // ISSUE: explicit constructor call
        base.\u002Ector((IProtoBuilder) builder, (Proto.ID) id, name, translationComment);
        this.m_id = id;
      }

      public CargoShipProtoBuilder.State SetMaximumModulesCount(int modulesCount)
      {
        this.m_maximumModulesCount = modulesCount;
        return this;
      }

      [MustUseReturnValue]
      public CargoShipProtoBuilder.State SetCost(EntityCostsTpl value, bool costsDisabled = false)
      {
        if (costsDisabled)
          return this;
        if (this.m_costs.Price.IsNotEmpty)
          throw new ProtoBuilderException(string.Format("Cost of {0} was already set.", (object) this.Id));
        this.m_costs = value.MapToEntityCosts(this.Builder.Registrator);
        return this;
      }

      public CargoShipProtoBuilder.State SetGraphics(CargoShipProto.Gfx gfx)
      {
        this.m_graphics = gfx;
        return this;
      }

      public CargoShipProtoBuilder.State SetDockOffset(RelTile2f offset)
      {
        this.m_dockOffset = offset;
        return this;
      }

      public CargoShipProtoBuilder.State SetAvailableModules(
        ImmutableArray<CargoShipModuleProto> availableModules)
      {
        this.m_availableModules = availableModules;
        return this;
      }

      public CargoShipProtoBuilder.State SetAvailableFuels(
        ImmutableArray<CargoShipProto.FuelData> availableFuels)
      {
        this.m_availableFuels = availableFuels;
        return this;
      }

      public CargoShipProto BuildAndAdd()
      {
        return this.AddToDb<CargoShipProto>(new CargoShipProto(this.m_id, this.Strings, this.m_costs, this.m_maximumModulesCount, 30.Seconds(), this.m_dockOffset, this.m_availableModules, this.m_availableFuels, this.m_graphics));
      }
    }
  }
}
