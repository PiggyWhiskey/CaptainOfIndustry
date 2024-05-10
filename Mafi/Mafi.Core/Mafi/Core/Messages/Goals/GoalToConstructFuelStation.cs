// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.Goals.GoalToConstructFuelStation
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.FuelStations;
using Mafi.Core.Entities;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Messages.Goals
{
  [GenerateSerializer(false, null, 0)]
  public class GoalToConstructFuelStation : Goal
  {
    private readonly GoalToConstructFuelStation.Proto m_goalProto;
    private readonly EntitiesManager m_entitiesManager;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public GoalToConstructFuelStation(
      GoalToConstructFuelStation.Proto goalProto,
      EntitiesManager entitiesManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector((GoalProto) goalProto);
      this.m_goalProto = goalProto;
      this.m_entitiesManager = entitiesManager;
      this.Title = goalProto.Title.Value;
    }

    protected override bool UpdateInternal()
    {
      return this.m_entitiesManager.GetFirstEntityOfType<FuelStation>((Predicate<FuelStation>) (x =>
      {
        if (!x.IsConstructed || !x.StoredFuel.IsPositive)
          return false;
        return !this.m_goalProto.IsAssignedTruckRequired || x.VehiclesTotal() > 0;
      })).HasValue;
    }

    protected override void UpdateTitleOnLoad() => this.Title = this.m_goalProto.Title.Value;

    public static void Serialize(GoalToConstructFuelStation value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<GoalToConstructFuelStation>(value))
        return;
      writer.EnqueueDataSerialization((object) value, GoalToConstructFuelStation.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntitiesManager.Serialize(this.m_entitiesManager, writer);
      writer.WriteGeneric<GoalToConstructFuelStation.Proto>(this.m_goalProto);
    }

    public static GoalToConstructFuelStation Deserialize(BlobReader reader)
    {
      GoalToConstructFuelStation constructFuelStation;
      if (reader.TryStartClassDeserialization<GoalToConstructFuelStation>(out constructFuelStation))
        reader.EnqueueDataDeserialization((object) constructFuelStation, GoalToConstructFuelStation.s_deserializeDataDelayedAction);
      return constructFuelStation;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<GoalToConstructFuelStation>(this, "m_entitiesManager", (object) EntitiesManager.Deserialize(reader));
      reader.SetField<GoalToConstructFuelStation>(this, "m_goalProto", (object) reader.ReadGenericAs<GoalToConstructFuelStation.Proto>());
    }

    static GoalToConstructFuelStation()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      GoalToConstructFuelStation.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Goal) obj).SerializeData(writer));
      GoalToConstructFuelStation.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Goal) obj).DeserializeData(reader));
    }

    public class Proto : GoalProto
    {
      public readonly LocStrFormatted Title;
      public readonly bool IsAssignedTruckRequired;

      public override Type Implementation => typeof (GoalToConstructFuelStation);

      public Proto(
        string id,
        LocStrFormatted title,
        bool isAssignedTruckRequired,
        int lockedByIndex = -1)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(id, new Mafi.Core.Prototypes.Proto.ID?(), lockedByIndex);
        this.IsAssignedTruckRequired = isAssignedTruckRequired;
        this.Title = title;
      }
    }
  }
}
