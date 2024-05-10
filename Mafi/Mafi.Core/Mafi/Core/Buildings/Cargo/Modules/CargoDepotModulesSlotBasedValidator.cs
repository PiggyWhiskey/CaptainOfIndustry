// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Cargo.Modules.CargoDepotModulesSlotBasedValidator
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Cargo.Modules
{
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class CargoDepotModulesSlotBasedValidator : 
    LayoutEntitySlotBasedValidatorBase<CargoDepotModuleProto>
  {
    private readonly Lyst<CargoDepot> m_depots;
    [DoNotSave(0, null)]
    private ImmutableArray<LayoutEntitySlot> m_availableSlots;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public CargoDepotModulesSlotBasedValidator(IEntitiesManager entitiesManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_depots = new Lyst<CargoDepot>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      entitiesManager.StaticEntityAdded.Add<CargoDepotModulesSlotBasedValidator>(this, new Action<IStaticEntity>(this.staticEntityAdded));
      entitiesManager.StaticEntityRemoved.Add<CargoDepotModulesSlotBasedValidator>(this, new Action<IStaticEntity>(this.staticEntityRemoved));
    }

    private void staticEntityAdded(IStaticEntity entity)
    {
      if (!(entity is CargoDepot cargoDepot))
        return;
      this.m_depots.AddAssertNew(cargoDepot);
    }

    private void staticEntityRemoved(IStaticEntity entity)
    {
      if (!(entity is CargoDepot cargoDepot))
        return;
      this.m_depots.RemoveAndAssert(cargoDepot);
    }

    public override ImmutableArray<LayoutEntitySlot> GetAvailableSlots(CargoDepotModuleProto proto)
    {
      Lyst<LayoutEntitySlot> lyst = new Lyst<LayoutEntitySlot>(this.m_depots.Count * 6);
      foreach (CargoDepot depot in this.m_depots)
      {
        foreach (CargoDepotProto.ModuleSlotPosition moduleSlot in depot.Prototype.ModuleSlots)
        {
          Tile3i modulePosition = moduleSlot.GetModulePosition(depot, proto);
          lyst.Add(new LayoutEntitySlot(new TileTransform(modulePosition, depot.Transform.Rotation, depot.Transform.IsReflected)));
        }
      }
      this.m_availableSlots = lyst.ToImmutableArray();
      return this.m_availableSlots;
    }

    public static void Serialize(CargoDepotModulesSlotBasedValidator value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<CargoDepotModulesSlotBasedValidator>(value))
        return;
      writer.EnqueueDataSerialization((object) value, CargoDepotModulesSlotBasedValidator.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      Lyst<CargoDepot>.Serialize(this.m_depots, writer);
    }

    public static CargoDepotModulesSlotBasedValidator Deserialize(BlobReader reader)
    {
      CargoDepotModulesSlotBasedValidator slotBasedValidator;
      if (reader.TryStartClassDeserialization<CargoDepotModulesSlotBasedValidator>(out slotBasedValidator))
        reader.EnqueueDataDeserialization((object) slotBasedValidator, CargoDepotModulesSlotBasedValidator.s_deserializeDataDelayedAction);
      return slotBasedValidator;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<CargoDepotModulesSlotBasedValidator>(this, "m_depots", (object) Lyst<CargoDepot>.Deserialize(reader));
    }

    static CargoDepotModulesSlotBasedValidator()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      CargoDepotModulesSlotBasedValidator.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((LayoutEntitySlotBasedValidatorBase<CargoDepotModuleProto>) obj).SerializeData(writer));
      CargoDepotModulesSlotBasedValidator.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((LayoutEntitySlotBasedValidatorBase<CargoDepotModuleProto>) obj).DeserializeData(reader));
    }
  }
}
