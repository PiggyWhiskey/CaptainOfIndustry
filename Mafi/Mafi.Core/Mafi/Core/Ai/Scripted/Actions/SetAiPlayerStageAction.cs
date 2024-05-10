// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Ai.Scripted.Actions.SetAiPlayerStageAction
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Ai.Scripted.Actions
{
  [GenerateSerializer(false, null, 0)]
  public class SetAiPlayerStageAction : IScriptedAiPlayerAction
  {
    private readonly int m_stage;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public Type ActionCoreType => typeof (SetAiPlayerStageAction.Core);

    public string Description
    {
      get => string.Format("Set AI player stage to `{0}`.", (object) this.m_stage);
    }

    public SetAiPlayerStageAction(int stage)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_stage = stage;
    }

    public static void Serialize(SetAiPlayerStageAction value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SetAiPlayerStageAction>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SetAiPlayerStageAction.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer) => writer.WriteInt(this.m_stage);

    public static SetAiPlayerStageAction Deserialize(BlobReader reader)
    {
      SetAiPlayerStageAction playerStageAction;
      if (reader.TryStartClassDeserialization<SetAiPlayerStageAction>(out playerStageAction))
        reader.EnqueueDataDeserialization((object) playerStageAction, SetAiPlayerStageAction.s_deserializeDataDelayedAction);
      return playerStageAction;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<SetAiPlayerStageAction>(this, "m_stage", (object) reader.ReadInt());
    }

    static SetAiPlayerStageAction()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      SetAiPlayerStageAction.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((SetAiPlayerStageAction) obj).SerializeData(writer));
      SetAiPlayerStageAction.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((SetAiPlayerStageAction) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    private class Core : IScriptedAiPlayerActionCore
    {
      private readonly SetAiPlayerStageAction m_action;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public Core(SetAiPlayerStageAction action)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_action = action;
      }

      public bool Perform(ScriptedAiPlayer player)
      {
        player.SetStage(this.m_action.m_stage);
        return true;
      }

      public static void Serialize(SetAiPlayerStageAction.Core value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<SetAiPlayerStageAction.Core>(value))
          return;
        writer.EnqueueDataSerialization((object) value, SetAiPlayerStageAction.Core.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        SetAiPlayerStageAction.Serialize(this.m_action, writer);
      }

      public static SetAiPlayerStageAction.Core Deserialize(BlobReader reader)
      {
        SetAiPlayerStageAction.Core core;
        if (reader.TryStartClassDeserialization<SetAiPlayerStageAction.Core>(out core))
          reader.EnqueueDataDeserialization((object) core, SetAiPlayerStageAction.Core.s_deserializeDataDelayedAction);
        return core;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        reader.SetField<SetAiPlayerStageAction.Core>(this, "m_action", (object) SetAiPlayerStageAction.Deserialize(reader));
      }

      static Core()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        SetAiPlayerStageAction.Core.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((SetAiPlayerStageAction.Core) obj).SerializeData(writer));
        SetAiPlayerStageAction.Core.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((SetAiPlayerStageAction.Core) obj).DeserializeData(reader));
      }
    }
  }
}
