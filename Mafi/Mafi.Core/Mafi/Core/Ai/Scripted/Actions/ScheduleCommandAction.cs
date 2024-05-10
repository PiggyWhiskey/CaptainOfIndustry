// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Ai.Scripted.Actions.ScheduleCommandAction
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Ai.Scripted.Actions
{
  [GenerateSerializer(false, null, 0)]
  public class ScheduleCommandAction : IScriptedAiPlayerAction
  {
    private readonly IInputCommand m_cmd;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public Type ActionCoreType => typeof (ScheduleCommandAction.Core);

    public string Description => "Schedule command: " + this.m_cmd.GetType().Name;

    public ScheduleCommandAction(IInputCommand cmd)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_cmd = cmd;
    }

    public static void Serialize(ScheduleCommandAction value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ScheduleCommandAction>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ScheduleCommandAction.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteGeneric<IInputCommand>(this.m_cmd);
    }

    public static ScheduleCommandAction Deserialize(BlobReader reader)
    {
      ScheduleCommandAction scheduleCommandAction;
      if (reader.TryStartClassDeserialization<ScheduleCommandAction>(out scheduleCommandAction))
        reader.EnqueueDataDeserialization((object) scheduleCommandAction, ScheduleCommandAction.s_deserializeDataDelayedAction);
      return scheduleCommandAction;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<ScheduleCommandAction>(this, "m_cmd", (object) reader.ReadGenericAs<IInputCommand>());
    }

    static ScheduleCommandAction()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ScheduleCommandAction.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ScheduleCommandAction) obj).SerializeData(writer));
      ScheduleCommandAction.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ScheduleCommandAction) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    private class Core : IScriptedAiPlayerActionCore
    {
      private readonly ScheduleCommandAction m_action;
      private readonly InputScheduler m_inputScheduler;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public Core(ScheduleCommandAction action, InputScheduler inputScheduler)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_action = action;
        this.m_inputScheduler = inputScheduler;
      }

      public bool Perform(ScriptedAiPlayer player)
      {
        this.m_inputScheduler.ScheduleInputCmd<IInputCommand>(this.m_action.m_cmd.ShallowCloneWithoutResult());
        return true;
      }

      public static void Serialize(ScheduleCommandAction.Core value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<ScheduleCommandAction.Core>(value))
          return;
        writer.EnqueueDataSerialization((object) value, ScheduleCommandAction.Core.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        ScheduleCommandAction.Serialize(this.m_action, writer);
        InputScheduler.Serialize(this.m_inputScheduler, writer);
      }

      public static ScheduleCommandAction.Core Deserialize(BlobReader reader)
      {
        ScheduleCommandAction.Core core;
        if (reader.TryStartClassDeserialization<ScheduleCommandAction.Core>(out core))
          reader.EnqueueDataDeserialization((object) core, ScheduleCommandAction.Core.s_deserializeDataDelayedAction);
        return core;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        reader.SetField<ScheduleCommandAction.Core>(this, "m_action", (object) ScheduleCommandAction.Deserialize(reader));
        reader.SetField<ScheduleCommandAction.Core>(this, "m_inputScheduler", (object) InputScheduler.Deserialize(reader));
      }

      static Core()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        ScheduleCommandAction.Core.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ScheduleCommandAction.Core) obj).SerializeData(writer));
        ScheduleCommandAction.Core.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ScheduleCommandAction.Core) obj).DeserializeData(reader));
      }
    }
  }
}
