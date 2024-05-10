// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Ai.Scripted.Actions.ToggleLogisticsAction
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Commands;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Ai.Scripted.Actions
{
  [GenerateSerializer(false, null, 0)]
  public class ToggleLogisticsAction : IScriptedAiPlayerAction
  {
    private readonly string m_entityName;
    private readonly bool m_isInput;
    private readonly EntityLogisticsMode m_mode;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public Type ActionCoreType => typeof (ToggleLogisticsAction.Core);

    public string Description => "Toggle logistics of entity `" + this.m_entityName + "`";

    public ToggleLogisticsAction(string entityName, bool isInput, EntityLogisticsMode mode)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entityName = entityName;
      this.m_isInput = isInput;
      this.m_mode = mode;
    }

    public static void Serialize(ToggleLogisticsAction value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ToggleLogisticsAction>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ToggleLogisticsAction.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteString(this.m_entityName);
      writer.WriteBool(this.m_isInput);
      writer.WriteInt((int) this.m_mode);
    }

    public static ToggleLogisticsAction Deserialize(BlobReader reader)
    {
      ToggleLogisticsAction toggleLogisticsAction;
      if (reader.TryStartClassDeserialization<ToggleLogisticsAction>(out toggleLogisticsAction))
        reader.EnqueueDataDeserialization((object) toggleLogisticsAction, ToggleLogisticsAction.s_deserializeDataDelayedAction);
      return toggleLogisticsAction;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<ToggleLogisticsAction>(this, "m_entityName", (object) reader.ReadString());
      reader.SetField<ToggleLogisticsAction>(this, "m_isInput", (object) reader.ReadBool());
      reader.SetField<ToggleLogisticsAction>(this, "m_mode", (object) (EntityLogisticsMode) reader.ReadInt());
    }

    static ToggleLogisticsAction()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ToggleLogisticsAction.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ToggleLogisticsAction) obj).SerializeData(writer));
      ToggleLogisticsAction.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ToggleLogisticsAction) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    private class Core : IScriptedAiPlayerActionCore
    {
      private readonly ToggleLogisticsAction m_action;
      private readonly IInputScheduler m_inputScheduler;
      private Option<EntityDisableLogisticsToggleCmd> m_cmd;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public Core(ToggleLogisticsAction action, IInputScheduler inputScheduler)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_action = action;
        this.m_inputScheduler = inputScheduler;
      }

      public bool Perform(ScriptedAiPlayer player)
      {
        Machine entity;
        if (!player.TryGetNamedEntity<Machine>(this.m_action.m_entityName, out entity))
        {
          Log.Error("Failed to find machine `" + this.m_action.m_entityName + "`.");
          return true;
        }
        if (!this.m_cmd.IsNone)
          return this.m_cmd.Value.IsProcessed;
        this.m_cmd = (Option<EntityDisableLogisticsToggleCmd>) this.m_inputScheduler.ScheduleInputCmd<EntityDisableLogisticsToggleCmd>(new EntityDisableLogisticsToggleCmd(entity.Id, this.m_action.m_isInput, this.m_action.m_mode));
        return false;
      }

      public static void Serialize(ToggleLogisticsAction.Core value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<ToggleLogisticsAction.Core>(value))
          return;
        writer.EnqueueDataSerialization((object) value, ToggleLogisticsAction.Core.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        ToggleLogisticsAction.Serialize(this.m_action, writer);
        Option<EntityDisableLogisticsToggleCmd>.Serialize(this.m_cmd, writer);
        writer.WriteGeneric<IInputScheduler>(this.m_inputScheduler);
      }

      public static ToggleLogisticsAction.Core Deserialize(BlobReader reader)
      {
        ToggleLogisticsAction.Core core;
        if (reader.TryStartClassDeserialization<ToggleLogisticsAction.Core>(out core))
          reader.EnqueueDataDeserialization((object) core, ToggleLogisticsAction.Core.s_deserializeDataDelayedAction);
        return core;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        reader.SetField<ToggleLogisticsAction.Core>(this, "m_action", (object) ToggleLogisticsAction.Deserialize(reader));
        this.m_cmd = Option<EntityDisableLogisticsToggleCmd>.Deserialize(reader);
        reader.SetField<ToggleLogisticsAction.Core>(this, "m_inputScheduler", (object) reader.ReadGenericAs<IInputScheduler>());
      }

      static Core()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        ToggleLogisticsAction.Core.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ToggleLogisticsAction.Core) obj).SerializeData(writer));
        ToggleLogisticsAction.Core.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ToggleLogisticsAction.Core) obj).DeserializeData(reader));
      }
    }
  }
}
