// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Ai.Scripted.Actions.WaitForEnoughUnityAction
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Population;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Ai.Scripted.Actions
{
  [GenerateSerializer(false, null, 0)]
  public class WaitForEnoughUnityAction : IScriptedAiPlayerAction
  {
    private readonly Duration m_maxWait;
    private readonly Upoints m_amountNeeded;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public Type ActionCoreType => typeof (WaitForEnoughUnityAction.Core);

    public string Description
    {
      get
      {
        return "Waiting for enough unity to build a house (max wait: " + this.m_maxWait.Seconds.ToStringRounded() + ")";
      }
    }

    public WaitForEnoughUnityAction(Upoints amountNeeded)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(amountNeeded, 5.Minutes());
    }

    public WaitForEnoughUnityAction(Upoints amountNeeded, Duration maxWait)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_maxWait = maxWait;
      this.m_amountNeeded = amountNeeded;
    }

    public static void Serialize(WaitForEnoughUnityAction value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<WaitForEnoughUnityAction>(value))
        return;
      writer.EnqueueDataSerialization((object) value, WaitForEnoughUnityAction.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Upoints.Serialize(this.m_amountNeeded, writer);
      Duration.Serialize(this.m_maxWait, writer);
    }

    public static WaitForEnoughUnityAction Deserialize(BlobReader reader)
    {
      WaitForEnoughUnityAction enoughUnityAction;
      if (reader.TryStartClassDeserialization<WaitForEnoughUnityAction>(out enoughUnityAction))
        reader.EnqueueDataDeserialization((object) enoughUnityAction, WaitForEnoughUnityAction.s_deserializeDataDelayedAction);
      return enoughUnityAction;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<WaitForEnoughUnityAction>(this, "m_amountNeeded", (object) Upoints.Deserialize(reader));
      reader.SetField<WaitForEnoughUnityAction>(this, "m_maxWait", (object) Duration.Deserialize(reader));
    }

    static WaitForEnoughUnityAction()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      WaitForEnoughUnityAction.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((WaitForEnoughUnityAction) obj).SerializeData(writer));
      WaitForEnoughUnityAction.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((WaitForEnoughUnityAction) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    private class Core : IScriptedAiPlayerActionCore
    {
      private readonly WaitForEnoughUnityAction m_action;
      private readonly UpointsManager m_upointsManager;
      private SimStep? m_initialSimStep;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public Core(WaitForEnoughUnityAction action, UpointsManager upointsManager)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_action = action;
        this.m_upointsManager = upointsManager;
      }

      public bool Perform(ScriptedAiPlayer player)
      {
        if (!this.m_initialSimStep.HasValue)
          this.m_initialSimStep = new SimStep?(player.SimLoopEvents.CurrentStep);
        if (this.m_upointsManager.CanConsume(this.m_action.m_amountNeeded))
          return true;
        Duration duration = (player.SimLoopEvents.CurrentStep.Value - this.m_initialSimStep.Value.Value).Ticks();
        if (!(duration > this.m_action.m_maxWait))
          return false;
        Log.Error("Failed to wait for enough unity in " + duration.Minutes.ToStringRounded() + " mins: " + string.Format("We have '{0}' unity ", (object) this.m_upointsManager.Quantity.Upoints()) + string.Format("but we need '{0}'. ", (object) this.m_action.m_amountNeeded) + string.Format("Action: {0}/{1}, stage: {2}.", (object) player.CurrentActionIndex, (object) player.ActionsCount, (object) player.Stage));
        return true;
      }

      public static void Serialize(WaitForEnoughUnityAction.Core value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<WaitForEnoughUnityAction.Core>(value))
          return;
        writer.EnqueueDataSerialization((object) value, WaitForEnoughUnityAction.Core.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        WaitForEnoughUnityAction.Serialize(this.m_action, writer);
        writer.WriteNullableStruct<SimStep>(this.m_initialSimStep);
        UpointsManager.Serialize(this.m_upointsManager, writer);
      }

      public static WaitForEnoughUnityAction.Core Deserialize(BlobReader reader)
      {
        WaitForEnoughUnityAction.Core core;
        if (reader.TryStartClassDeserialization<WaitForEnoughUnityAction.Core>(out core))
          reader.EnqueueDataDeserialization((object) core, WaitForEnoughUnityAction.Core.s_deserializeDataDelayedAction);
        return core;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        reader.SetField<WaitForEnoughUnityAction.Core>(this, "m_action", (object) WaitForEnoughUnityAction.Deserialize(reader));
        this.m_initialSimStep = reader.ReadNullableStruct<SimStep>();
        reader.SetField<WaitForEnoughUnityAction.Core>(this, "m_upointsManager", (object) UpointsManager.Deserialize(reader));
      }

      static Core()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        WaitForEnoughUnityAction.Core.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((WaitForEnoughUnityAction.Core) obj).SerializeData(writer));
        WaitForEnoughUnityAction.Core.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((WaitForEnoughUnityAction.Core) obj).DeserializeData(reader));
      }
    }
  }
}
