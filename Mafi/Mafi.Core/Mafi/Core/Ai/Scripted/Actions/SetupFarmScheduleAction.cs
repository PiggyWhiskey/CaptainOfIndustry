// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Ai.Scripted.Actions.SetupFarmScheduleAction
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Buildings.Farms;
using Mafi.Core.Input;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;
using System.Linq;

#nullable disable
namespace Mafi.Core.Ai.Scripted.Actions
{
  [GenerateSerializer(false, null, 0)]
  public class SetupFarmScheduleAction : IScriptedAiPlayerAction
  {
    private readonly string m_entityName;
    private readonly Proto.ID[] m_cropIds;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public Type ActionCoreType => typeof (SetupFarmScheduleAction.Core);

    public string Description => "Set schedule of farm `" + this.m_entityName + "`";

    public SetupFarmScheduleAction(string entityName, Proto.ID[] cropIds)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entityName = entityName;
      this.m_cropIds = cropIds;
    }

    public static void Serialize(SetupFarmScheduleAction value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SetupFarmScheduleAction>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SetupFarmScheduleAction.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteArray<Proto.ID>(this.m_cropIds);
      writer.WriteString(this.m_entityName);
    }

    public static SetupFarmScheduleAction Deserialize(BlobReader reader)
    {
      SetupFarmScheduleAction farmScheduleAction;
      if (reader.TryStartClassDeserialization<SetupFarmScheduleAction>(out farmScheduleAction))
        reader.EnqueueDataDeserialization((object) farmScheduleAction, SetupFarmScheduleAction.s_deserializeDataDelayedAction);
      return farmScheduleAction;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<SetupFarmScheduleAction>(this, "m_cropIds", (object) reader.ReadArray<Proto.ID>());
      reader.SetField<SetupFarmScheduleAction>(this, "m_entityName", (object) reader.ReadString());
    }

    static SetupFarmScheduleAction()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      SetupFarmScheduleAction.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((SetupFarmScheduleAction) obj).SerializeData(writer));
      SetupFarmScheduleAction.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((SetupFarmScheduleAction) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    private class Core : IScriptedAiPlayerActionCore
    {
      private readonly SetupFarmScheduleAction m_action;
      private readonly IInputScheduler m_inputScheduler;
      private readonly Lyst<FarmAssignCropCmd> m_cmds;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public Core(SetupFarmScheduleAction action, IInputScheduler inputScheduler)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.m_cmds = new Lyst<FarmAssignCropCmd>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_action = action;
        this.m_inputScheduler = inputScheduler;
      }

      public bool Perform(ScriptedAiPlayer player)
      {
        Farm entity;
        if (!player.TryGetNamedEntity<Farm>(this.m_action.m_entityName, out entity))
        {
          Log.Error("Failed to find farm `" + this.m_action.m_entityName + "`.");
          return true;
        }
        if (!this.m_cmds.IsEmpty)
          return this.m_cmds.All<FarmAssignCropCmd>((Func<FarmAssignCropCmd, bool>) (x => x.IsProcessed));
        int scheduleSlot = 0;
        foreach (Proto.ID cropId in this.m_action.m_cropIds)
        {
          this.m_cmds.Add(this.m_inputScheduler.ScheduleInputCmd<FarmAssignCropCmd>(new FarmAssignCropCmd(entity.Id, new Proto.ID?(cropId), scheduleSlot)));
          ++scheduleSlot;
        }
        return false;
      }

      public static void Serialize(SetupFarmScheduleAction.Core value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<SetupFarmScheduleAction.Core>(value))
          return;
        writer.EnqueueDataSerialization((object) value, SetupFarmScheduleAction.Core.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        SetupFarmScheduleAction.Serialize(this.m_action, writer);
        Lyst<FarmAssignCropCmd>.Serialize(this.m_cmds, writer);
        writer.WriteGeneric<IInputScheduler>(this.m_inputScheduler);
      }

      public static SetupFarmScheduleAction.Core Deserialize(BlobReader reader)
      {
        SetupFarmScheduleAction.Core core;
        if (reader.TryStartClassDeserialization<SetupFarmScheduleAction.Core>(out core))
          reader.EnqueueDataDeserialization((object) core, SetupFarmScheduleAction.Core.s_deserializeDataDelayedAction);
        return core;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        reader.SetField<SetupFarmScheduleAction.Core>(this, "m_action", (object) SetupFarmScheduleAction.Deserialize(reader));
        reader.SetField<SetupFarmScheduleAction.Core>(this, "m_cmds", (object) Lyst<FarmAssignCropCmd>.Deserialize(reader));
        reader.SetField<SetupFarmScheduleAction.Core>(this, "m_inputScheduler", (object) reader.ReadGenericAs<IInputScheduler>());
      }

      static Core()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        SetupFarmScheduleAction.Core.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((SetupFarmScheduleAction.Core) obj).SerializeData(writer));
        SetupFarmScheduleAction.Core.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((SetupFarmScheduleAction.Core) obj).DeserializeData(reader));
      }
    }
  }
}
