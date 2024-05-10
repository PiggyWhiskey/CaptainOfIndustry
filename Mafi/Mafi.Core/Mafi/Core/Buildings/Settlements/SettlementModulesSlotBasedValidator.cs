// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Settlements.SettlementModulesSlotBasedValidator
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
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Buildings.Settlements
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  [GenerateSerializer(false, null, 0)]
  public class SettlementModulesSlotBasedValidator : 
    LayoutEntitySlotBasedValidatorBase<ISettlementModuleProto>
  {
    private readonly Dict<Tile2i, ISettlementSquareModule> m_squareModules;
    [DoNotSave(0, null)]
    private ImmutableArray<LayoutEntitySlot> m_availableSlots;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public SettlementModulesSlotBasedValidator(IEntitiesManager entitiesManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_squareModules = new Dict<Tile2i, ISettlementSquareModule>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      entitiesManager.StaticEntityAdded.Add<SettlementModulesSlotBasedValidator>(this, new Action<IStaticEntity>(this.staticEntityAdded));
      entitiesManager.StaticEntityRemoved.Add<SettlementModulesSlotBasedValidator>(this, new Action<IStaticEntity>(this.staticEntityRemoved));
    }

    private void staticEntityAdded(IStaticEntity entity)
    {
      if (!(entity is ISettlementSquareModule settlementSquareModule))
        return;
      this.m_squareModules.AddAndAssertNew(settlementSquareModule.Transform.Position.Xy, settlementSquareModule);
      this.m_availableSlots = new ImmutableArray<LayoutEntitySlot>();
    }

    private void staticEntityRemoved(IStaticEntity entity)
    {
      if (!(entity is ISettlementSquareModule settlementSquareModule))
        return;
      this.m_squareModules.RemoveAndAssert(settlementSquareModule.Transform.Position.Xy);
      this.m_availableSlots = new ImmutableArray<LayoutEntitySlot>();
    }

    public override ImmutableArray<LayoutEntitySlot> GetAvailableSlots(ISettlementModuleProto proto)
    {
      if (this.m_availableSlots.IsValid)
        return this.m_availableSlots;
      Lyst<LayoutEntitySlot> slots = new Lyst<LayoutEntitySlot>(this.m_squareModules.Count * 10);
      foreach (KeyValuePair<Tile2i, ISettlementSquareModule> squareModule in this.m_squareModules)
      {
        ISettlementSquareModule settlementSquareModule = squareModule.Value;
        RelTile2i coreSize = settlementSquareModule.Prototype.Layout.CoreSize;
        Assert.That<int>(coreSize.X).IsEqualTo(coreSize.Y, "Square module is not square.");
        Tile3i position = settlementSquareModule.Transform.Position;
        foreach (Direction90 allFourDirection in Direction90.AllFourDirections)
          tryExtend(position, allFourDirection, coreSize.X);
      }
      this.m_availableSlots = slots.ToImmutableArray();
      return this.m_availableSlots;

      void tryExtend(Tile3i from, Direction90 direction, int parentSize)
      {
        if (this.m_squareModules.ContainsKey(from.Xy + parentSize * direction.ToTileDirection()))
          return;
        RelTile2i coreSize = proto.Layout.CoreSize;
        Assert.That<int>(parentSize % coreSize.Y).IsZero("Settlement module Y-size does not divide settlement house module without remainder.");
        Rotation90 rotation = direction.ToRotation();
        int num = parentSize / coreSize.Y;
        RelTile2i relTile2i1 = new RelTile2i(parentSize + coreSize.X, -parentSize + coreSize.Y);
        for (int index = 0; index < num; ++index)
        {
          Tile3i tile3i = from;
          RelTile2i relTile2i2 = relTile2i1.AddY(index * coreSize.Y * 2);
          relTile2i2 = relTile2i2.Rotate(rotation);
          relTile2i2 = relTile2i2.FloorDiv(2);
          RelTile3i relTile3i = relTile2i2.ExtendZ(0);
          Tile3i position = tile3i + relTile3i;
          slots.Add(new LayoutEntitySlot(new TileTransform(position, rotation)));
        }
      }
    }

    public static void Serialize(SettlementModulesSlotBasedValidator value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SettlementModulesSlotBasedValidator>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SettlementModulesSlotBasedValidator.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      Dict<Tile2i, ISettlementSquareModule>.Serialize(this.m_squareModules, writer);
    }

    public static SettlementModulesSlotBasedValidator Deserialize(BlobReader reader)
    {
      SettlementModulesSlotBasedValidator slotBasedValidator;
      if (reader.TryStartClassDeserialization<SettlementModulesSlotBasedValidator>(out slotBasedValidator))
        reader.EnqueueDataDeserialization((object) slotBasedValidator, SettlementModulesSlotBasedValidator.s_deserializeDataDelayedAction);
      return slotBasedValidator;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<SettlementModulesSlotBasedValidator>(this, "m_squareModules", (object) Dict<Tile2i, ISettlementSquareModule>.Deserialize(reader));
    }

    static SettlementModulesSlotBasedValidator()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      SettlementModulesSlotBasedValidator.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((LayoutEntitySlotBasedValidatorBase<ISettlementModuleProto>) obj).SerializeData(writer));
      SettlementModulesSlotBasedValidator.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((LayoutEntitySlotBasedValidatorBase<ISettlementModuleProto>) obj).DeserializeData(reader));
    }
  }
}
