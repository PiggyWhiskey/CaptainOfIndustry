// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.MessageTriggerOnEntityConstructed
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Simulation;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Messages
{
  [GenerateSerializer(false, null, 0)]
  public class MessageTriggerOnEntityConstructed : MessageTrigger
  {
    private readonly MessageTriggerOnEntityConstructedProto m_proto;
    private readonly IConstructionManager m_constructionManager;
    private readonly EntitiesManager m_entitiesManager;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public MessageTriggerOnEntityConstructed(
      MessageTriggerOnEntityConstructedProto triggerProto,
      ISimLoopEvents simLoopEvents,
      MessagesManager messageManager,
      IConstructionManager constructionManager,
      EntitiesManager entitiesManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector((MessageTriggerProto) triggerProto, simLoopEvents, messageManager);
      this.m_proto = triggerProto;
      this.m_constructionManager = constructionManager;
      this.m_entitiesManager = entitiesManager;
      constructionManager.EntityConstructed.Add<MessageTriggerOnEntityConstructed>(this, new Action<IStaticEntity>(this.onEntityConstructedStatic));
      entitiesManager.OnUpgradeJustPerformed.Add<MessageTriggerOnEntityConstructed>(this, new Action<IUpgradableEntity, IEntityProto>(this.onEntityUpgraded));
    }

    private void onEntityUpgraded(IEntity entity, IEntityProto previousProto)
    {
      this.onEntityConstructed(entity);
    }

    private void onEntityConstructed(IEntity entity)
    {
      if (this.m_proto.IsObsolete)
      {
        this.OnDestroy();
      }
      else
      {
        if (!(entity.Prototype.Id == this.m_proto.ConstructedProtoId))
          return;
        this.DeliverMessage();
      }
    }

    private void onEntityConstructedStatic(IStaticEntity entity)
    {
      if (this.m_proto.IsObsolete)
        this.OnDestroy();
      else
        this.onEntityConstructed((IEntity) entity);
    }

    protected override void OnDestroy()
    {
      this.m_constructionManager.EntityConstructed.Remove<MessageTriggerOnEntityConstructed>(this, new Action<IStaticEntity>(this.onEntityConstructedStatic));
      this.m_entitiesManager.OnUpgradeJustPerformed.Remove<MessageTriggerOnEntityConstructed>(this, new Action<IUpgradableEntity, IEntityProto>(this.onEntityUpgraded));
    }

    public static void Serialize(MessageTriggerOnEntityConstructed value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<MessageTriggerOnEntityConstructed>(value))
        return;
      writer.EnqueueDataSerialization((object) value, MessageTriggerOnEntityConstructed.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteGeneric<IConstructionManager>(this.m_constructionManager);
      EntitiesManager.Serialize(this.m_entitiesManager, writer);
      writer.WriteGeneric<MessageTriggerOnEntityConstructedProto>(this.m_proto);
    }

    public static MessageTriggerOnEntityConstructed Deserialize(BlobReader reader)
    {
      MessageTriggerOnEntityConstructed entityConstructed;
      if (reader.TryStartClassDeserialization<MessageTriggerOnEntityConstructed>(out entityConstructed))
        reader.EnqueueDataDeserialization((object) entityConstructed, MessageTriggerOnEntityConstructed.s_deserializeDataDelayedAction);
      return entityConstructed;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<MessageTriggerOnEntityConstructed>(this, "m_constructionManager", (object) reader.ReadGenericAs<IConstructionManager>());
      reader.SetField<MessageTriggerOnEntityConstructed>(this, "m_entitiesManager", (object) EntitiesManager.Deserialize(reader));
      reader.SetField<MessageTriggerOnEntityConstructed>(this, "m_proto", (object) reader.ReadGenericAs<MessageTriggerOnEntityConstructedProto>());
    }

    static MessageTriggerOnEntityConstructed()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      MessageTriggerOnEntityConstructed.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((MessageTrigger) obj).SerializeData(writer));
      MessageTriggerOnEntityConstructed.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((MessageTrigger) obj).DeserializeData(reader));
    }
  }
}
