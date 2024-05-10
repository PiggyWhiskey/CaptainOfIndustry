// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Ai.Scripted.Actions.ScheduleVehicleConstructionAction
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.VehicleDepots;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Input;
using Mafi.Core.Vehicles.Commands;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Ai.Scripted.Actions
{
  [GenerateSerializer(false, null, 0)]
  public class ScheduleVehicleConstructionAction : IScriptedAiPlayerAction
  {
    private readonly DynamicEntityProto.ID m_protoId;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public Type ActionCoreType => typeof (ScheduleVehicleConstructionAction.Core);

    public string Description => string.Format("Vehicle build `{0}`", (object) this.m_protoId);

    public ScheduleVehicleConstructionAction(DynamicEntityProto.ID protoId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_protoId = protoId;
    }

    public static void Serialize(ScheduleVehicleConstructionAction value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ScheduleVehicleConstructionAction>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ScheduleVehicleConstructionAction.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      DynamicEntityProto.ID.Serialize(this.m_protoId, writer);
    }

    public static ScheduleVehicleConstructionAction Deserialize(BlobReader reader)
    {
      ScheduleVehicleConstructionAction constructionAction;
      if (reader.TryStartClassDeserialization<ScheduleVehicleConstructionAction>(out constructionAction))
        reader.EnqueueDataDeserialization((object) constructionAction, ScheduleVehicleConstructionAction.s_deserializeDataDelayedAction);
      return constructionAction;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<ScheduleVehicleConstructionAction>(this, "m_protoId", (object) DynamicEntityProto.ID.Deserialize(reader));
    }

    static ScheduleVehicleConstructionAction()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ScheduleVehicleConstructionAction.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ScheduleVehicleConstructionAction) obj).SerializeData(writer));
      ScheduleVehicleConstructionAction.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ScheduleVehicleConstructionAction) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    private class Core : IScriptedAiPlayerActionCore
    {
      private readonly ScheduleVehicleConstructionAction m_action;
      private readonly InputScheduler m_inputScheduler;
      private readonly EntitiesManager m_entitiesManager;
      private Option<AddVehicleToBuildQueueCmd> m_cmd;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public Core(
        ScheduleVehicleConstructionAction action,
        InputScheduler inputScheduler,
        EntitiesManager entitiesManager)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_action = action;
        this.m_inputScheduler = inputScheduler;
        this.m_entitiesManager = entitiesManager;
      }

      public bool Perform(ScriptedAiPlayer player)
      {
        if (!this.m_cmd.IsNone)
          return this.m_cmd.Value.IsProcessed;
        Option<VehicleDepot> firstEntityOfType = this.m_entitiesManager.GetFirstEntityOfType<VehicleDepot>();
        if (firstEntityOfType.HasValue)
        {
          this.m_cmd = (Option<AddVehicleToBuildQueueCmd>) this.m_inputScheduler.ScheduleInputCmd<AddVehicleToBuildQueueCmd>(new AddVehicleToBuildQueueCmd(this.m_action.m_protoId, firstEntityOfType.Value.Id));
          return false;
        }
        Log.Error("No vehicle depot found in the game.");
        return true;
      }

      public static void Serialize(ScheduleVehicleConstructionAction.Core value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<ScheduleVehicleConstructionAction.Core>(value))
          return;
        writer.EnqueueDataSerialization((object) value, ScheduleVehicleConstructionAction.Core.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        ScheduleVehicleConstructionAction.Serialize(this.m_action, writer);
        Option<AddVehicleToBuildQueueCmd>.Serialize(this.m_cmd, writer);
        EntitiesManager.Serialize(this.m_entitiesManager, writer);
        InputScheduler.Serialize(this.m_inputScheduler, writer);
      }

      public static ScheduleVehicleConstructionAction.Core Deserialize(BlobReader reader)
      {
        ScheduleVehicleConstructionAction.Core core;
        if (reader.TryStartClassDeserialization<ScheduleVehicleConstructionAction.Core>(out core))
          reader.EnqueueDataDeserialization((object) core, ScheduleVehicleConstructionAction.Core.s_deserializeDataDelayedAction);
        return core;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        reader.SetField<ScheduleVehicleConstructionAction.Core>(this, "m_action", (object) ScheduleVehicleConstructionAction.Deserialize(reader));
        this.m_cmd = Option<AddVehicleToBuildQueueCmd>.Deserialize(reader);
        reader.SetField<ScheduleVehicleConstructionAction.Core>(this, "m_entitiesManager", (object) EntitiesManager.Deserialize(reader));
        reader.SetField<ScheduleVehicleConstructionAction.Core>(this, "m_inputScheduler", (object) InputScheduler.Deserialize(reader));
      }

      static Core()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        ScheduleVehicleConstructionAction.Core.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ScheduleVehicleConstructionAction.Core) obj).SerializeData(writer));
        ScheduleVehicleConstructionAction.Core.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ScheduleVehicleConstructionAction.Core) obj).DeserializeData(reader));
      }
    }
  }
}
