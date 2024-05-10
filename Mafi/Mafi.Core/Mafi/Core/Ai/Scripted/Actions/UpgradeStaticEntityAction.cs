// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Ai.Scripted.Actions.UpgradeStaticEntityAction
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Ai.Scripted.Actions
{
  [GenerateSerializer(false, null, 0)]
  public class UpgradeStaticEntityAction : IScriptedAiPlayerAction
  {
    private readonly string m_entityName;
    private readonly Duration m_maxWait;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public Type ActionCoreType => typeof (UpgradeStaticEntityAction.Core);

    public string Description => "Upgrade of `" + this.m_entityName + "`";

    public UpgradeStaticEntityAction(string name, Duration maxWait)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entityName = name;
      this.m_maxWait = maxWait.CheckPositive();
    }

    public static void Serialize(UpgradeStaticEntityAction value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<UpgradeStaticEntityAction>(value))
        return;
      writer.EnqueueDataSerialization((object) value, UpgradeStaticEntityAction.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteString(this.m_entityName);
      Duration.Serialize(this.m_maxWait, writer);
    }

    public static UpgradeStaticEntityAction Deserialize(BlobReader reader)
    {
      UpgradeStaticEntityAction staticEntityAction;
      if (reader.TryStartClassDeserialization<UpgradeStaticEntityAction>(out staticEntityAction))
        reader.EnqueueDataDeserialization((object) staticEntityAction, UpgradeStaticEntityAction.s_deserializeDataDelayedAction);
      return staticEntityAction;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<UpgradeStaticEntityAction>(this, "m_entityName", (object) reader.ReadString());
      reader.SetField<UpgradeStaticEntityAction>(this, "m_maxWait", (object) Duration.Deserialize(reader));
    }

    static UpgradeStaticEntityAction()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      UpgradeStaticEntityAction.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((UpgradeStaticEntityAction) obj).SerializeData(writer));
      UpgradeStaticEntityAction.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((UpgradeStaticEntityAction) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    private class Core : IScriptedAiPlayerActionCore, IEntityObserverForUpgrade, IEntityObserver
    {
      private readonly UpgradeStaticEntityAction m_action;
      private readonly InputScheduler m_inputScheduler;
      private Option<UpgradeEntityCmd> m_cmd;
      private Option<IEntity> m_upgradedEntity;
      private Duration m_waitRemaining;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public Core(UpgradeStaticEntityAction action, InputScheduler inputScheduler)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_action = action;
        this.m_inputScheduler = inputScheduler;
        this.m_waitRemaining = action.m_maxWait;
      }

      public bool Perform(ScriptedAiPlayer player)
      {
        if (this.m_cmd.IsNone)
        {
          IEntity entity;
          if (!player.TryGetNamedEntity<IEntity>(this.m_action.m_entityName, out entity))
          {
            Log.Error("Failed to find entity `" + this.m_action.m_entityName + "`.");
            return true;
          }
          entity.AddObserver((IEntityObserver) this);
          this.m_cmd = (Option<UpgradeEntityCmd>) this.m_inputScheduler.ScheduleInputCmd<UpgradeEntityCmd>(new UpgradeEntityCmd(entity.Id));
          return false;
        }
        if (!this.m_cmd.Value.IsProcessed)
          return false;
        if (!this.m_cmd.Value.Result)
        {
          Log.Error("Failed to upgrade entity `" + this.m_action.m_entityName + "`.");
          return true;
        }
        if (this.m_upgradedEntity.IsNone)
        {
          this.m_waitRemaining -= Duration.OneTick;
          if (!this.m_waitRemaining.IsNegative)
            return false;
          Log.Error(string.Format("Failed to upgrade in `{0}` ticks, progress: ", (object) this.m_action.m_maxWait) + getConstrProgress());
          return true;
        }
        player.RegisterNamedEntity(this.m_upgradedEntity.Value.Id, this.m_action.m_entityName, true);
        return true;

        string getConstrProgress()
        {
          IUpgradableEntity entity;
          return player.TryGetNamedEntity<IUpgradableEntity>(this.m_action.m_entityName, out entity) && entity.ConstructionProgress.HasValue ? entity.ConstructionProgress.Value.Buffers.Select<string>((Func<IProductBufferReadOnly, string>) (x => string.Format("{0}: {1}/{2}", (object) x.Product.Id, (object) x.Quantity, (object) x.Capacity))).JoinStrings(", ") : "n/a";
        }
      }

      void IEntityObserverForUpgrade.OnEntityUpgraded(IEntity entity, IEntityProto previousProto)
      {
        entity.RemoveObserver((IEntityObserver) this);
        this.m_upgradedEntity = entity.SomeOption<IEntity>();
      }

      void IEntityObserver.OnEntityDestroy(IEntity entity)
      {
        entity.RemoveObserver((IEntityObserver) this);
      }

      public static void Serialize(UpgradeStaticEntityAction.Core value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<UpgradeStaticEntityAction.Core>(value))
          return;
        writer.EnqueueDataSerialization((object) value, UpgradeStaticEntityAction.Core.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        UpgradeStaticEntityAction.Serialize(this.m_action, writer);
        Option<UpgradeEntityCmd>.Serialize(this.m_cmd, writer);
        InputScheduler.Serialize(this.m_inputScheduler, writer);
        Option<IEntity>.Serialize(this.m_upgradedEntity, writer);
        Duration.Serialize(this.m_waitRemaining, writer);
      }

      public static UpgradeStaticEntityAction.Core Deserialize(BlobReader reader)
      {
        UpgradeStaticEntityAction.Core core;
        if (reader.TryStartClassDeserialization<UpgradeStaticEntityAction.Core>(out core))
          reader.EnqueueDataDeserialization((object) core, UpgradeStaticEntityAction.Core.s_deserializeDataDelayedAction);
        return core;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        reader.SetField<UpgradeStaticEntityAction.Core>(this, "m_action", (object) UpgradeStaticEntityAction.Deserialize(reader));
        this.m_cmd = Option<UpgradeEntityCmd>.Deserialize(reader);
        reader.SetField<UpgradeStaticEntityAction.Core>(this, "m_inputScheduler", (object) InputScheduler.Deserialize(reader));
        this.m_upgradedEntity = Option<IEntity>.Deserialize(reader);
        this.m_waitRemaining = Duration.Deserialize(reader);
      }

      static Core()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        UpgradeStaticEntityAction.Core.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((UpgradeStaticEntityAction.Core) obj).SerializeData(writer));
        UpgradeStaticEntityAction.Core.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((UpgradeStaticEntityAction.Core) obj).DeserializeData(reader));
      }
    }
  }
}
