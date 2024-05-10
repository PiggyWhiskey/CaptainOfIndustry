// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.MessageTriggerOnEntityConstructedOrProductRunningOut
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Static;
using Mafi.Core.Products;
using Mafi.Core.Simulation;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Messages
{
  [GenerateSerializer(false, null, 0)]
  public class MessageTriggerOnEntityConstructedOrProductRunningOut : MessageTrigger
  {
    private readonly MessageTriggerOnEntityConstructedOrProductRunningOutProto m_proto;
    private readonly ISimLoopEvents m_simLoopEvents;
    private readonly IConstructionManager m_constructionManager;
    private readonly ICalendar m_calendar;
    private readonly ProductStats m_productStats;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public MessageTriggerOnEntityConstructedOrProductRunningOut(
      MessageTriggerOnEntityConstructedOrProductRunningOutProto triggerProto,
      ISimLoopEvents simLoopEvents,
      MessagesManager messageManager,
      IConstructionManager constructionManager,
      IProductsManager productsManager,
      ICalendar calendar)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector((MessageTriggerProto) triggerProto, simLoopEvents, messageManager);
      this.m_proto = triggerProto;
      this.m_simLoopEvents = simLoopEvents;
      this.m_constructionManager = constructionManager;
      this.m_calendar = calendar;
      this.m_productStats = productsManager.GetStatsFor(this.m_proto.ProductQuantityToTrigger.Product);
      constructionManager.EntityConstructed.Add<MessageTriggerOnEntityConstructedOrProductRunningOut>(this, new Action<IStaticEntity>(this.onEntityConstructed));
      simLoopEvents.Update.Add<MessageTriggerOnEntityConstructedOrProductRunningOut>(this, new Action(this.checkProductStatus));
    }

    private void checkProductStatus()
    {
      if (this.m_proto.IsObsolete)
      {
        this.OnDestroy();
      }
      else
      {
        if (this.m_calendar.CurrentDate.RelGameDate.TotalMonthsFloored <= 1 || !(this.m_productStats.StoredAvailableQuantity < (QuantityLarge) this.m_proto.ProductQuantityToTrigger.Quantity))
          return;
        this.DeliverMessage();
      }
    }

    private void onEntityConstructed(IStaticEntity entity)
    {
      if (!(entity.Prototype.Id == this.m_proto.ConstructedProtoId))
        return;
      this.DeliverMessage();
    }

    protected override void OnDestroy()
    {
      this.m_constructionManager.EntityConstructed.Remove<MessageTriggerOnEntityConstructedOrProductRunningOut>(this, new Action<IStaticEntity>(this.onEntityConstructed));
      this.m_simLoopEvents.Update.Remove<MessageTriggerOnEntityConstructedOrProductRunningOut>(this, new Action(this.checkProductStatus));
    }

    public static void Serialize(
      MessageTriggerOnEntityConstructedOrProductRunningOut value,
      BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<MessageTriggerOnEntityConstructedOrProductRunningOut>(value))
        return;
      writer.EnqueueDataSerialization((object) value, MessageTriggerOnEntityConstructedOrProductRunningOut.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteGeneric<ICalendar>(this.m_calendar);
      writer.WriteGeneric<IConstructionManager>(this.m_constructionManager);
      ProductStats.Serialize(this.m_productStats, writer);
      writer.WriteGeneric<MessageTriggerOnEntityConstructedOrProductRunningOutProto>(this.m_proto);
      writer.WriteGeneric<ISimLoopEvents>(this.m_simLoopEvents);
    }

    public static MessageTriggerOnEntityConstructedOrProductRunningOut Deserialize(BlobReader reader)
    {
      MessageTriggerOnEntityConstructedOrProductRunningOut productRunningOut;
      if (reader.TryStartClassDeserialization<MessageTriggerOnEntityConstructedOrProductRunningOut>(out productRunningOut))
        reader.EnqueueDataDeserialization((object) productRunningOut, MessageTriggerOnEntityConstructedOrProductRunningOut.s_deserializeDataDelayedAction);
      return productRunningOut;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<MessageTriggerOnEntityConstructedOrProductRunningOut>(this, "m_calendar", (object) reader.ReadGenericAs<ICalendar>());
      reader.SetField<MessageTriggerOnEntityConstructedOrProductRunningOut>(this, "m_constructionManager", (object) reader.ReadGenericAs<IConstructionManager>());
      reader.SetField<MessageTriggerOnEntityConstructedOrProductRunningOut>(this, "m_productStats", (object) ProductStats.Deserialize(reader));
      reader.SetField<MessageTriggerOnEntityConstructedOrProductRunningOut>(this, "m_proto", (object) reader.ReadGenericAs<MessageTriggerOnEntityConstructedOrProductRunningOutProto>());
      reader.SetField<MessageTriggerOnEntityConstructedOrProductRunningOut>(this, "m_simLoopEvents", (object) reader.ReadGenericAs<ISimLoopEvents>());
    }

    static MessageTriggerOnEntityConstructedOrProductRunningOut()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      MessageTriggerOnEntityConstructedOrProductRunningOut.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((MessageTrigger) obj).SerializeData(writer));
      MessageTriggerOnEntityConstructedOrProductRunningOut.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((MessageTrigger) obj).DeserializeData(reader));
    }
  }
}
