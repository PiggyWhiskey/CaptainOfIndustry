// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Ai.Scripted.Actions.WaitForStaticEntitiesBuiltAction
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Static;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Ai.Scripted.Actions
{
  [GenerateSerializer(false, null, 0)]
  public class WaitForStaticEntitiesBuiltAction : IScriptedAiPlayerAction
  {
    private readonly Duration m_maxWait;
    private readonly string[] m_names;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public Type ActionCoreType => typeof (WaitForStaticEntitiesBuiltAction.Core);

    public string Description
    {
      get
      {
        return "Waiting for buildings to be built: " + ((IEnumerable<string>) this.m_names).JoinStrings(", ") + " (max wait: " + this.m_maxWait.Seconds.ToStringRounded() + ")";
      }
    }

    public WaitForStaticEntitiesBuiltAction(params string[] names)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector((1 + 3 * names.Length).Minutes(), names);
    }

    public WaitForStaticEntitiesBuiltAction(Duration maxWait, params string[] names)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_maxWait = maxWait;
      this.m_names = names;
    }

    public static void Serialize(WaitForStaticEntitiesBuiltAction value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<WaitForStaticEntitiesBuiltAction>(value))
        return;
      writer.EnqueueDataSerialization((object) value, WaitForStaticEntitiesBuiltAction.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Duration.Serialize(this.m_maxWait, writer);
      writer.WriteArray<string>(this.m_names);
    }

    public static WaitForStaticEntitiesBuiltAction Deserialize(BlobReader reader)
    {
      WaitForStaticEntitiesBuiltAction entitiesBuiltAction;
      if (reader.TryStartClassDeserialization<WaitForStaticEntitiesBuiltAction>(out entitiesBuiltAction))
        reader.EnqueueDataDeserialization((object) entitiesBuiltAction, WaitForStaticEntitiesBuiltAction.s_deserializeDataDelayedAction);
      return entitiesBuiltAction;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<WaitForStaticEntitiesBuiltAction>(this, "m_maxWait", (object) Duration.Deserialize(reader));
      reader.SetField<WaitForStaticEntitiesBuiltAction>(this, "m_names", (object) reader.ReadArray<string>());
    }

    static WaitForStaticEntitiesBuiltAction()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      WaitForStaticEntitiesBuiltAction.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((WaitForStaticEntitiesBuiltAction) obj).SerializeData(writer));
      WaitForStaticEntitiesBuiltAction.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((WaitForStaticEntitiesBuiltAction) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    private class Core : IScriptedAiPlayerActionCore
    {
      private readonly WaitForStaticEntitiesBuiltAction m_action;
      private readonly IConstructionManager m_constManager;
      private SimStep? m_initialSimStep;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public Core(WaitForStaticEntitiesBuiltAction action, IConstructionManager constructionManager)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_action = action;
        this.m_constManager = constructionManager;
      }

      public bool Perform(ScriptedAiPlayer player)
      {
        if (!this.m_initialSimStep.HasValue)
          this.m_initialSimStep = new SimStep?(player.SimLoopEvents.CurrentStep);
        foreach (string name in this.m_action.m_names)
        {
          IStaticEntity entity;
          if (!player.TryGetNamedEntity<IStaticEntity>(name, out entity))
          {
            Log.Error("Unknown entity `" + name + "`.");
            return true;
          }
          if (entity.ConstructionState != ConstructionState.Constructed)
          {
            Duration duration = (player.SimLoopEvents.CurrentStep.Value - this.m_initialSimStep.Value.Value).Ticks();
            if (!(duration > this.m_action.m_maxWait))
              return false;
            Log.Error("Failed to build entities in " + duration.Minutes.ToStringRounded() + " mins: " + ((IEnumerable<string>) this.m_action.m_names).Select<string, string>(new Func<string, string>(getConstrProgress)).JoinStrings(", ") + ". " + string.Format("Action: {0}/{1}, stage: {2}.", (object) player.CurrentActionIndex, (object) player.ActionsCount, (object) player.Stage));
            return true;
          }
        }
        return true;

        string getConstrProgress(string entityName)
        {
          string str = "n/a";
          IStaticEntity entity;
          if (player.TryGetNamedEntity<IStaticEntity>(entityName, out entity))
          {
            Option<IEntityConstructionProgress> constructionProgress = this.m_constManager.GetConstructionProgress(entity);
            if (constructionProgress.HasValue)
              str = constructionProgress.Value.Progress.ToStringRounded();
          }
          return entityName + " (" + str + ")";
        }
      }

      public static void Serialize(WaitForStaticEntitiesBuiltAction.Core value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<WaitForStaticEntitiesBuiltAction.Core>(value))
          return;
        writer.EnqueueDataSerialization((object) value, WaitForStaticEntitiesBuiltAction.Core.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        WaitForStaticEntitiesBuiltAction.Serialize(this.m_action, writer);
        writer.WriteGeneric<IConstructionManager>(this.m_constManager);
        writer.WriteNullableStruct<SimStep>(this.m_initialSimStep);
      }

      public static WaitForStaticEntitiesBuiltAction.Core Deserialize(BlobReader reader)
      {
        WaitForStaticEntitiesBuiltAction.Core core;
        if (reader.TryStartClassDeserialization<WaitForStaticEntitiesBuiltAction.Core>(out core))
          reader.EnqueueDataDeserialization((object) core, WaitForStaticEntitiesBuiltAction.Core.s_deserializeDataDelayedAction);
        return core;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        reader.SetField<WaitForStaticEntitiesBuiltAction.Core>(this, "m_action", (object) WaitForStaticEntitiesBuiltAction.Deserialize(reader));
        reader.SetField<WaitForStaticEntitiesBuiltAction.Core>(this, "m_constManager", (object) reader.ReadGenericAs<IConstructionManager>());
        this.m_initialSimStep = reader.ReadNullableStruct<SimStep>();
      }

      static Core()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        WaitForStaticEntitiesBuiltAction.Core.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((WaitForStaticEntitiesBuiltAction.Core) obj).SerializeData(writer));
        WaitForStaticEntitiesBuiltAction.Core.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((WaitForStaticEntitiesBuiltAction.Core) obj).DeserializeData(reader));
      }
    }
  }
}
