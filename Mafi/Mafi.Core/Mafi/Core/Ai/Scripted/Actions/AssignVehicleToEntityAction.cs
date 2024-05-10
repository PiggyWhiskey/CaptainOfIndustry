// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Ai.Scripted.Actions.AssignVehicleToEntityAction
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.Input;
using Mafi.Core.Vehicles.Commands;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Ai.Scripted.Actions
{
  [GenerateSerializer(false, null, 0)]
  public class AssignVehicleToEntityAction : IScriptedAiPlayerAction
  {
    private readonly DynamicEntityProto.ID m_protoId;
    private readonly int m_count;
    private readonly string m_staticEntityName;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public Type ActionCoreType => typeof (AssignVehicleToEntityAction.Core);

    public string Description
    {
      get
      {
        return string.Format("Assign {0} `{1}` to `{2}`.", (object) this.m_count, (object) this.m_protoId, (object) this.m_staticEntityName);
      }
    }

    public AssignVehicleToEntityAction(
      DynamicEntityProto.ID protoId,
      int count,
      string staticEntityName)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_protoId = protoId;
      this.m_count = count.CheckPositive();
      this.m_staticEntityName = staticEntityName;
    }

    public static void Serialize(AssignVehicleToEntityAction value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<AssignVehicleToEntityAction>(value))
        return;
      writer.EnqueueDataSerialization((object) value, AssignVehicleToEntityAction.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteInt(this.m_count);
      DynamicEntityProto.ID.Serialize(this.m_protoId, writer);
      writer.WriteString(this.m_staticEntityName);
    }

    public static AssignVehicleToEntityAction Deserialize(BlobReader reader)
    {
      AssignVehicleToEntityAction vehicleToEntityAction;
      if (reader.TryStartClassDeserialization<AssignVehicleToEntityAction>(out vehicleToEntityAction))
        reader.EnqueueDataDeserialization((object) vehicleToEntityAction, AssignVehicleToEntityAction.s_deserializeDataDelayedAction);
      return vehicleToEntityAction;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<AssignVehicleToEntityAction>(this, "m_count", (object) reader.ReadInt());
      reader.SetField<AssignVehicleToEntityAction>(this, "m_protoId", (object) DynamicEntityProto.ID.Deserialize(reader));
      reader.SetField<AssignVehicleToEntityAction>(this, "m_staticEntityName", (object) reader.ReadString());
    }

    static AssignVehicleToEntityAction()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      AssignVehicleToEntityAction.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((AssignVehicleToEntityAction) obj).SerializeData(writer));
      AssignVehicleToEntityAction.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((AssignVehicleToEntityAction) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    private class Core : IScriptedAiPlayerActionCore
    {
      private readonly AssignVehicleToEntityAction m_action;
      private readonly InputScheduler m_inputScheduler;
      private Option<AssignVehicleTypeToEntityCmd> m_cmd;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public Core(AssignVehicleToEntityAction action, InputScheduler inputScheduler)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_action = action;
        this.m_inputScheduler = inputScheduler;
      }

      public bool Perform(ScriptedAiPlayer player)
      {
        if (!this.m_cmd.IsNone)
          return this.m_cmd.Value.IsProcessed;
        IStaticEntity entity;
        if (!player.TryGetNamedEntity<IStaticEntity>(this.m_action.m_staticEntityName, out entity))
        {
          Log.Error("Failed to find static entity named `" + this.m_action.m_staticEntityName + "`.");
          return true;
        }
        this.m_cmd = (Option<AssignVehicleTypeToEntityCmd>) this.m_inputScheduler.ScheduleInputCmd<AssignVehicleTypeToEntityCmd>(new AssignVehicleTypeToEntityCmd(this.m_action.m_protoId, entity.Id, this.m_action.m_count));
        return false;
      }

      public static void Serialize(AssignVehicleToEntityAction.Core value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<AssignVehicleToEntityAction.Core>(value))
          return;
        writer.EnqueueDataSerialization((object) value, AssignVehicleToEntityAction.Core.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        AssignVehicleToEntityAction.Serialize(this.m_action, writer);
        Option<AssignVehicleTypeToEntityCmd>.Serialize(this.m_cmd, writer);
        InputScheduler.Serialize(this.m_inputScheduler, writer);
      }

      public static AssignVehicleToEntityAction.Core Deserialize(BlobReader reader)
      {
        AssignVehicleToEntityAction.Core core;
        if (reader.TryStartClassDeserialization<AssignVehicleToEntityAction.Core>(out core))
          reader.EnqueueDataDeserialization((object) core, AssignVehicleToEntityAction.Core.s_deserializeDataDelayedAction);
        return core;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        reader.SetField<AssignVehicleToEntityAction.Core>(this, "m_action", (object) AssignVehicleToEntityAction.Deserialize(reader));
        this.m_cmd = Option<AssignVehicleTypeToEntityCmd>.Deserialize(reader);
        reader.SetField<AssignVehicleToEntityAction.Core>(this, "m_inputScheduler", (object) InputScheduler.Deserialize(reader));
      }

      static Core()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        AssignVehicleToEntityAction.Core.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((AssignVehicleToEntityAction.Core) obj).SerializeData(writer));
        AssignVehicleToEntityAction.Core.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((AssignVehicleToEntityAction.Core) obj).DeserializeData(reader));
      }
    }
  }
}
