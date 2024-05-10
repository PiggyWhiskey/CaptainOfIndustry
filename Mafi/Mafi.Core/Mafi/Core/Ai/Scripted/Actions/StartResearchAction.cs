// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Ai.Scripted.Actions.StartResearchAction
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Core.Research;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Ai.Scripted.Actions
{
  [GenerateSerializer(false, null, 0)]
  public class StartResearchAction : IScriptedAiPlayerAction
  {
    private readonly ResearchNodeProto.ID m_researchId;
    private readonly Duration m_maxWaitForPrevious;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public Type ActionCoreType => typeof (StartResearchAction.Core);

    public string Description
    {
      get
      {
        return string.Format("Start research `{0}` ", (object) this.m_researchId) + "(max wait: " + this.m_maxWaitForPrevious.Seconds.ToStringRounded() + ")";
      }
    }

    public StartResearchAction(ResearchNodeProto.ID researchId, Duration maxWaitForPrevious)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_researchId = researchId;
      this.m_maxWaitForPrevious = maxWaitForPrevious;
    }

    public static void Serialize(StartResearchAction value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<StartResearchAction>(value))
        return;
      writer.EnqueueDataSerialization((object) value, StartResearchAction.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Duration.Serialize(this.m_maxWaitForPrevious, writer);
      ResearchNodeProto.ID.Serialize(this.m_researchId, writer);
    }

    public static StartResearchAction Deserialize(BlobReader reader)
    {
      StartResearchAction startResearchAction;
      if (reader.TryStartClassDeserialization<StartResearchAction>(out startResearchAction))
        reader.EnqueueDataDeserialization((object) startResearchAction, StartResearchAction.s_deserializeDataDelayedAction);
      return startResearchAction;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<StartResearchAction>(this, "m_maxWaitForPrevious", (object) Duration.Deserialize(reader));
      reader.SetField<StartResearchAction>(this, "m_researchId", (object) ResearchNodeProto.ID.Deserialize(reader));
    }

    static StartResearchAction()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      StartResearchAction.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((StartResearchAction) obj).SerializeData(writer));
      StartResearchAction.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((StartResearchAction) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    private class Core : IScriptedAiPlayerActionCore
    {
      private readonly StartResearchAction m_action;
      private readonly InputScheduler m_inputScheduler;
      private readonly ResearchManager m_researchManager;
      private Option<ResearchStartCmd> m_cmd;
      private SimStep? m_stepWhenStarted;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public Core(
        StartResearchAction action,
        InputScheduler inputScheduler,
        ResearchManager researchManager)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_action = action;
        this.m_inputScheduler = inputScheduler;
        this.m_researchManager = researchManager;
      }

      public bool Perform(ScriptedAiPlayer player)
      {
        if (!this.m_stepWhenStarted.HasValue)
          this.m_stepWhenStarted = new SimStep?(player.SimLoopEvents.CurrentStep);
        if (!this.m_cmd.IsNone)
          return this.m_cmd.Value.IsProcessed;
        if (this.m_researchManager.CurrentResearch.HasValue)
        {
          ResearchNode researchNode = this.m_researchManager.CurrentResearch.Value;
          if (!(player.SimLoopEvents.CurrentStep - this.m_stepWhenStarted.Value > this.m_action.m_maxWaitForPrevious))
            return false;
          Log.Error(string.Format("Failed start research `{0}` in ", (object) this.m_action.m_researchId) + string.Format("{0} steps. Current research `{1}` ", (object) this.m_action.m_maxWaitForPrevious, (object) researchNode.Proto.Id) + string.Format("is at {0}.", (object) this.m_researchManager.CurrentResearch.Value.ProgressInPerc));
          return true;
        }
        this.m_cmd = (Option<ResearchStartCmd>) this.m_inputScheduler.ScheduleInputCmd<ResearchStartCmd>(new ResearchStartCmd(this.m_action.m_researchId));
        return false;
      }

      public static void Serialize(StartResearchAction.Core value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<StartResearchAction.Core>(value))
          return;
        writer.EnqueueDataSerialization((object) value, StartResearchAction.Core.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        StartResearchAction.Serialize(this.m_action, writer);
        Option<ResearchStartCmd>.Serialize(this.m_cmd, writer);
        InputScheduler.Serialize(this.m_inputScheduler, writer);
        ResearchManager.Serialize(this.m_researchManager, writer);
        writer.WriteNullableStruct<SimStep>(this.m_stepWhenStarted);
      }

      public static StartResearchAction.Core Deserialize(BlobReader reader)
      {
        StartResearchAction.Core core;
        if (reader.TryStartClassDeserialization<StartResearchAction.Core>(out core))
          reader.EnqueueDataDeserialization((object) core, StartResearchAction.Core.s_deserializeDataDelayedAction);
        return core;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        reader.SetField<StartResearchAction.Core>(this, "m_action", (object) StartResearchAction.Deserialize(reader));
        this.m_cmd = Option<ResearchStartCmd>.Deserialize(reader);
        reader.SetField<StartResearchAction.Core>(this, "m_inputScheduler", (object) InputScheduler.Deserialize(reader));
        reader.SetField<StartResearchAction.Core>(this, "m_researchManager", (object) ResearchManager.Deserialize(reader));
        this.m_stepWhenStarted = reader.ReadNullableStruct<SimStep>();
      }

      static Core()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        StartResearchAction.Core.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((StartResearchAction.Core) obj).SerializeData(writer));
        StartResearchAction.Core.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((StartResearchAction.Core) obj).DeserializeData(reader));
      }
    }
  }
}
