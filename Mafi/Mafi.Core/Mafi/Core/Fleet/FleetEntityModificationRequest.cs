// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Fleet.FleetEntityModificationRequest
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Economy;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Fleet
{
  [GenerateSerializer(false, null, 0)]
  public struct FleetEntityModificationRequest
  {
    public ImmutableArray<SlotModification> SlotsData;
    public FleetEntityHullProto.ID? HullId;

    public FleetEntityModificationRequest(
      ImmutableArray<SlotModification> slotsData,
      FleetEntityHullProto.ID? hullId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.SlotsData = slotsData;
      this.HullId = hullId;
    }

    public bool GetPriceForModifications(
      FleetEntity entity,
      ProtosDb protosDb,
      out AssetValue valueToPay)
    {
      return this.GetPriceForModifications(entity, protosDb, out valueToPay, out FleetEntityStats _, out FleetEntityStats _);
    }

    public bool GetPriceForModifications(
      FleetEntity entity,
      ProtosDb protosDb,
      out AssetValue valueToPay,
      out FleetEntityStats oldStats,
      out FleetEntityStats newStats)
    {
      bool forModifications = false;
      valueToPay = AssetValue.Empty;
      oldStats = new FleetEntityStats();
      newStats = new FleetEntityStats();
      entity.Hull.Proto.ApplyToStats(oldStats);
      if (this.HullId.HasValue)
      {
        FleetEntityHullProto.ID? hullId = this.HullId;
        FleetEntityHullProto.ID id = entity.Hull.Proto.Id;
        if ((hullId.HasValue ? (hullId.GetValueOrDefault() != id ? 1 : 0) : 1) != 0)
        {
          forModifications = true;
          FleetEntityHullProto orThrow = protosDb.GetOrThrow<FleetEntityHullProto>((Proto.ID) this.HullId.Value);
          AssetValue assetValue = orThrow.Value - entity.Hull.Proto.Value;
          valueToPay += assetValue.TakePositiveValuesOnly();
          orThrow.ApplyToStats(newStats);
          goto label_4;
        }
      }
      entity.Hull.Proto.ApplyToStats(newStats);
label_4:
      foreach (SlotModification slotModification1 in this.SlotsData)
      {
        SlotModification slotModification = slotModification1;
        FleetEntitySlot fleetEntitySlot = entity.Slots.FirstOrDefault<FleetEntitySlot>((Predicate<FleetEntitySlot>) (x => x.Proto.Id == slotModification.SlotId));
        if (fleetEntitySlot == null)
        {
          Log.Error(string.Format("Slot group with id '{0}' not found!", (object) slotModification.SlotId));
          return false;
        }
        FleetEntityPartProto.ID? newPartId = slotModification.Part;
        Option<FleetEntityPartProto> existingPart = fleetEntitySlot.ExistingPart;
        if (existingPart.HasValue)
          existingPart.Value.ApplyToStats(oldStats);
        if (!existingPart.IsNone || newPartId.HasValue)
        {
          if (existingPart.HasValue)
          {
            FleetEntityPartProto.ID id = existingPart.Value.Id;
            FleetEntityPartProto.ID? nullable = newPartId;
            if ((nullable.HasValue ? (id == nullable.GetValueOrDefault() ? 1 : 0) : 0) != 0)
              goto label_12;
          }
          forModifications = true;
          FleetEntityPartProto fleetEntityPartProto = fleetEntitySlot.Proto.EligibleItems.FirstOrDefault((Func<FleetEntityPartProto, bool>) (x =>
          {
            FleetEntityPartProto.ID id = x.Id;
            FleetEntityPartProto.ID? nullable = newPartId;
            return nullable.HasValue && id == nullable.GetValueOrDefault();
          }));
          if ((Proto) fleetEntityPartProto == (Proto) null)
          {
            Log.Error(string.Format("Received part '{0}' that is not eligible for slot '{1}'!", (object) newPartId, (object) slotModification.SlotId));
            return false;
          }
          if (existingPart.HasValue)
          {
            AssetValue assetValue = fleetEntityPartProto.Value - existingPart.Value.Value;
            valueToPay += assetValue.TakePositiveValuesOnly();
          }
          else
            valueToPay += fleetEntityPartProto.Value;
          if (newPartId.HasValue)
          {
            fleetEntityPartProto.ApplyToStats(newStats);
            continue;
          }
          continue;
        }
label_12:
        if (existingPart.HasValue)
          existingPart.Value.ApplyToStats(newStats);
      }
      return forModifications;
    }

    public static void Serialize(FleetEntityModificationRequest value, BlobWriter writer)
    {
      ImmutableArray<SlotModification>.Serialize(value.SlotsData, writer);
      writer.WriteNullableStruct<FleetEntityHullProto.ID>(value.HullId);
    }

    public static FleetEntityModificationRequest Deserialize(BlobReader reader)
    {
      return new FleetEntityModificationRequest(ImmutableArray<SlotModification>.Deserialize(reader), reader.ReadNullableStruct<FleetEntityHullProto.ID>());
    }
  }
}
