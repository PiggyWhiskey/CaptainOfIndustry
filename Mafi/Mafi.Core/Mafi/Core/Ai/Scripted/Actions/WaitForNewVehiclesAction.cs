// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Ai.Scripted.Actions.WaitForNewVehiclesAction
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Buildings.VehicleDepots;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Ai.Scripted.Actions
{
  [GenerateSerializer(false, null, 0)]
  public class WaitForNewVehiclesAction : IScriptedAiPlayerAction
  {
    private readonly DynamicEntityProto.ID m_protoId;
    private readonly int m_delta;
    private readonly Duration m_maxWait;
    private readonly string m_waitReason;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public Type ActionCoreType => typeof (WaitForNewVehiclesAction.Core);

    public string Description
    {
      get
      {
        return string.Format("Wait for {0} new vehicles of type {1}.", (object) this.m_delta, (object) this.m_protoId);
      }
    }

    public WaitForNewVehiclesAction(
      DynamicEntityProto.ID protoId,
      int delta,
      Duration maxWait,
      string waitReason)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_protoId = protoId;
      this.m_delta = delta.CheckPositive();
      this.m_maxWait = maxWait;
      this.m_waitReason = waitReason;
    }

    public static void Serialize(WaitForNewVehiclesAction value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<WaitForNewVehiclesAction>(value))
        return;
      writer.EnqueueDataSerialization((object) value, WaitForNewVehiclesAction.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteInt(this.m_delta);
      Duration.Serialize(this.m_maxWait, writer);
      DynamicEntityProto.ID.Serialize(this.m_protoId, writer);
      writer.WriteString(this.m_waitReason);
    }

    public static WaitForNewVehiclesAction Deserialize(BlobReader reader)
    {
      WaitForNewVehiclesAction newVehiclesAction;
      if (reader.TryStartClassDeserialization<WaitForNewVehiclesAction>(out newVehiclesAction))
        reader.EnqueueDataDeserialization((object) newVehiclesAction, WaitForNewVehiclesAction.s_deserializeDataDelayedAction);
      return newVehiclesAction;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<WaitForNewVehiclesAction>(this, "m_delta", (object) reader.ReadInt());
      reader.SetField<WaitForNewVehiclesAction>(this, "m_maxWait", (object) Duration.Deserialize(reader));
      reader.SetField<WaitForNewVehiclesAction>(this, "m_protoId", (object) DynamicEntityProto.ID.Deserialize(reader));
      reader.SetField<WaitForNewVehiclesAction>(this, "m_waitReason", (object) reader.ReadString());
    }

    static WaitForNewVehiclesAction()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      WaitForNewVehiclesAction.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((WaitForNewVehiclesAction) obj).SerializeData(writer));
      WaitForNewVehiclesAction.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((WaitForNewVehiclesAction) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    private class Core : IScriptedAiPlayerActionCore
    {
      private readonly WaitForNewVehiclesAction m_action;
      private readonly IEntitiesManager m_entitiesManager;
      private int? m_initialCount;
      private SimStep m_initialSimStep;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public Core(WaitForNewVehiclesAction action, IEntitiesManager entitiesManager)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_action = action;
        this.m_entitiesManager = entitiesManager;
      }

      public bool Perform(ScriptedAiPlayer player)
      {
        if (player.IsInstaBuildEnabled)
          return true;
        if (!this.m_initialCount.HasValue)
        {
          this.m_initialSimStep = player.SimLoopEvents.CurrentStep;
          this.m_initialCount = new int?(this.m_entitiesManager.GetAllEntitiesOfType<DynamicGroundEntity>().Count<DynamicGroundEntity>((Func<DynamicGroundEntity, bool>) (x => x.Prototype.Id == this.m_action.m_protoId)));
          Lyst<VehicleDepot> lyst = this.m_entitiesManager.GetAllEntitiesOfType<VehicleDepot>().ToLyst<VehicleDepot>();
          if (lyst.Any<VehicleDepot>((Predicate<VehicleDepot>) (x => x.CurrentlyBuildVehicle.HasValue && x.CurrentlyBuildVehicle.Value.Id == this.m_action.m_protoId)) | lyst.Where<VehicleDepot>((Func<VehicleDepot, bool>) (x => x.BuildQueue.Count > 1)).SelectMany<VehicleDepot, DrivingEntityProto>((Func<VehicleDepot, IEnumerable<DrivingEntityProto>>) (x => x.BuildQueue.AsEnumerable().Skip<DrivingEntityProto>(1))).Any<DrivingEntityProto>((Func<DrivingEntityProto, bool>) (x => x.Id == this.m_action.m_protoId)))
            return false;
          this.raiseError(player, "Failed to start waiting on a new vehicles of type " + string.Format("{0}. Currently no such vehicles are being produced or queued.", (object) this.m_action.m_protoId));
          return true;
        }
        int num = this.m_entitiesManager.GetAllEntitiesOfType<DynamicGroundEntity>().Count<DynamicGroundEntity>((Func<DynamicGroundEntity, bool>) (x => x.Prototype.Id == this.m_action.m_protoId)) - this.m_initialCount.Value;
        if (num >= this.m_action.m_delta)
          return true;
        Duration duration = (player.SimLoopEvents.CurrentStep.Value - this.m_initialSimStep.Value).Ticks();
        if (!(duration > this.m_action.m_maxWait))
          return false;
        this.raiseError(player, string.Format("Failed to create {0} new vehicles of type ", (object) this.m_action.m_delta) + string.Format("{0}. Current vehicles delta is {1} after ", (object) this.m_action.m_protoId, (object) num) + duration.Minutes.ToStringRounded() + " mins.");
        return true;
      }

      private void raiseError(ScriptedAiPlayer player, string message)
      {
        Lyst<VehicleDepot> lyst = this.m_entitiesManager.GetAllEntitiesOfType<VehicleDepot>().ToLyst<VehicleDepot>();
        string str1 = lyst.Where<VehicleDepot>((Func<VehicleDepot, bool>) (x => x.CurrentlyBuildVehicle.HasValue)).Select<VehicleDepot, string>((Func<VehicleDepot, string>) (x => string.Format("{0} ({1})", (object) x.CurrentlyBuildVehicle.Value.Id, (object) x.VehicleConstructionProgress.Value.Progress))).JoinStrings(", ");
        if (str1 == "")
          str1 = "none";
        string str2 = lyst.Where<VehicleDepot>((Func<VehicleDepot, bool>) (x => x.BuildQueue.Count > 1)).SelectMany<VehicleDepot, DrivingEntityProto>((Func<VehicleDepot, IEnumerable<DrivingEntityProto>>) (x => x.BuildQueue.AsEnumerable().Skip<DrivingEntityProto>(1))).Select<DrivingEntityProto, string>((Func<DrivingEntityProto, string>) (x => x.Id.Value)).JoinStrings(", ");
        if (str2 == "")
          str2 = "none";
        Log.Error(this.m_action.m_waitReason + ": " + message + " Vehicles being built: " + str1 + ". Queued vehicles: " + str2 + ". " + string.Format("Action: {0}/{1}, stage: {2}.", (object) player.CurrentActionIndex, (object) player.ActionsCount, (object) player.Stage));
      }

      public static void Serialize(WaitForNewVehiclesAction.Core value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<WaitForNewVehiclesAction.Core>(value))
          return;
        writer.EnqueueDataSerialization((object) value, WaitForNewVehiclesAction.Core.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        WaitForNewVehiclesAction.Serialize(this.m_action, writer);
        writer.WriteGeneric<IEntitiesManager>(this.m_entitiesManager);
        writer.WriteNullableStruct<int>(this.m_initialCount);
        SimStep.Serialize(this.m_initialSimStep, writer);
      }

      public static WaitForNewVehiclesAction.Core Deserialize(BlobReader reader)
      {
        WaitForNewVehiclesAction.Core core;
        if (reader.TryStartClassDeserialization<WaitForNewVehiclesAction.Core>(out core))
          reader.EnqueueDataDeserialization((object) core, WaitForNewVehiclesAction.Core.s_deserializeDataDelayedAction);
        return core;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        reader.SetField<WaitForNewVehiclesAction.Core>(this, "m_action", (object) WaitForNewVehiclesAction.Deserialize(reader));
        reader.SetField<WaitForNewVehiclesAction.Core>(this, "m_entitiesManager", (object) reader.ReadGenericAs<IEntitiesManager>());
        this.m_initialCount = reader.ReadNullableStruct<int>();
        this.m_initialSimStep = SimStep.Deserialize(reader);
      }

      static Core()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        WaitForNewVehiclesAction.Core.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((WaitForNewVehiclesAction.Core) obj).SerializeData(writer));
        WaitForNewVehiclesAction.Core.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((WaitForNewVehiclesAction.Core) obj).DeserializeData(reader));
      }
    }
  }
}
