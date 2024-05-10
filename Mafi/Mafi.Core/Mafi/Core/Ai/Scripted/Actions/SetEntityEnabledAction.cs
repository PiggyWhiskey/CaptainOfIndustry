// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Ai.Scripted.Actions.SetEntityEnabledAction
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Ai.Scripted.Actions
{
  [GenerateSerializer(false, null, 0)]
  public class SetEntityEnabledAction : IScriptedAiPlayerAction
  {
    private readonly string m_entityName;
    private readonly bool m_isEnabled;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public Type ActionCoreType => typeof (SetEntityEnabledAction.Core);

    public string Description
    {
      get => "Set `" + this.m_entityName + "` " + (this.m_isEnabled ? "enabled" : "disabled") + ".";
    }

    public SetEntityEnabledAction(string entityName, bool isEnabled)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entityName = entityName;
      this.m_isEnabled = isEnabled;
    }

    public static void Serialize(SetEntityEnabledAction value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SetEntityEnabledAction>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SetEntityEnabledAction.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteString(this.m_entityName);
      writer.WriteBool(this.m_isEnabled);
    }

    public static SetEntityEnabledAction Deserialize(BlobReader reader)
    {
      SetEntityEnabledAction entityEnabledAction;
      if (reader.TryStartClassDeserialization<SetEntityEnabledAction>(out entityEnabledAction))
        reader.EnqueueDataDeserialization((object) entityEnabledAction, SetEntityEnabledAction.s_deserializeDataDelayedAction);
      return entityEnabledAction;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<SetEntityEnabledAction>(this, "m_entityName", (object) reader.ReadString());
      reader.SetField<SetEntityEnabledAction>(this, "m_isEnabled", (object) reader.ReadBool());
    }

    static SetEntityEnabledAction()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      SetEntityEnabledAction.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((SetEntityEnabledAction) obj).SerializeData(writer));
      SetEntityEnabledAction.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((SetEntityEnabledAction) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    private class Core : IScriptedAiPlayerActionCore
    {
      private readonly SetEntityEnabledAction m_action;
      private readonly IInputScheduler m_inputScheduler;
      private Option<SetEntityEnabledCmd> m_cmd;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public Core(SetEntityEnabledAction action, IInputScheduler inputScheduler)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_action = action;
        this.m_inputScheduler = inputScheduler;
      }

      public bool Perform(ScriptedAiPlayer player)
      {
        IEntity entity;
        if (!player.TryGetNamedEntity<IEntity>(this.m_action.m_entityName, out entity))
        {
          Log.Error("Failed to find entity `" + this.m_action.m_entityName + "`.");
          return true;
        }
        if (!this.m_cmd.IsNone)
          return this.m_cmd.Value.IsProcessed;
        this.m_cmd = (Option<SetEntityEnabledCmd>) this.m_inputScheduler.ScheduleInputCmd<SetEntityEnabledCmd>(new SetEntityEnabledCmd(entity.Id, this.m_action.m_isEnabled));
        return false;
      }

      public static void Serialize(SetEntityEnabledAction.Core value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<SetEntityEnabledAction.Core>(value))
          return;
        writer.EnqueueDataSerialization((object) value, SetEntityEnabledAction.Core.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        SetEntityEnabledAction.Serialize(this.m_action, writer);
        Option<SetEntityEnabledCmd>.Serialize(this.m_cmd, writer);
        writer.WriteGeneric<IInputScheduler>(this.m_inputScheduler);
      }

      public static SetEntityEnabledAction.Core Deserialize(BlobReader reader)
      {
        SetEntityEnabledAction.Core core;
        if (reader.TryStartClassDeserialization<SetEntityEnabledAction.Core>(out core))
          reader.EnqueueDataDeserialization((object) core, SetEntityEnabledAction.Core.s_deserializeDataDelayedAction);
        return core;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        reader.SetField<SetEntityEnabledAction.Core>(this, "m_action", (object) SetEntityEnabledAction.Deserialize(reader));
        this.m_cmd = Option<SetEntityEnabledCmd>.Deserialize(reader);
        reader.SetField<SetEntityEnabledAction.Core>(this, "m_inputScheduler", (object) reader.ReadGenericAs<IInputScheduler>());
      }

      static Core()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        SetEntityEnabledAction.Core.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((SetEntityEnabledAction.Core) obj).SerializeData(writer));
        SetEntityEnabledAction.Core.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((SetEntityEnabledAction.Core) obj).DeserializeData(reader));
      }
    }
  }
}
