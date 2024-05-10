// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.MessageTriggerGlobalProductLow
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Messages
{
  [GenerateSerializer(false, null, 0)]
  public class MessageTriggerGlobalProductLow : MessageTrigger
  {
    private readonly MessageTriggerGlobalProductLowProto m_triggerProto;
    private readonly ICalendar m_calendar;
    private readonly ProductStats m_stats;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public MessageTriggerGlobalProductLow(
      MessageTriggerGlobalProductLowProto triggerProto,
      ISimLoopEvents simLoopEvents,
      ICalendar calendar,
      MessagesManager messageManager,
      IProductsManager productsManager,
      ProtosDb protosDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector((MessageTriggerProto) triggerProto, simLoopEvents, messageManager);
      this.m_triggerProto = triggerProto;
      this.m_calendar = calendar;
      ProductProto proto;
      if (!protosDb.TryGetProto<ProductProto>((Proto.ID) triggerProto.ProductId, out proto))
      {
        Log.Error(string.Format("Unknown product proto for trigger {0}", (object) triggerProto.Id));
      }
      else
      {
        this.m_stats = productsManager.GetStatsFor(proto);
        this.m_calendar.NewDay.Add<MessageTriggerGlobalProductLow>(this, new Action(this.newDay));
      }
    }

    private void newDay()
    {
      if (!(this.m_stats.StoredAvailableQuantity < this.m_triggerProto.MinQuantity))
        return;
      this.DeliverMessage();
    }

    protected override void OnDestroy()
    {
      this.m_calendar.NewDay.Remove<MessageTriggerGlobalProductLow>(this, new Action(this.newDay));
    }

    public static void Serialize(MessageTriggerGlobalProductLow value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<MessageTriggerGlobalProductLow>(value))
        return;
      writer.EnqueueDataSerialization((object) value, MessageTriggerGlobalProductLow.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteGeneric<ICalendar>(this.m_calendar);
      ProductStats.Serialize(this.m_stats, writer);
      writer.WriteGeneric<MessageTriggerGlobalProductLowProto>(this.m_triggerProto);
    }

    public static MessageTriggerGlobalProductLow Deserialize(BlobReader reader)
    {
      MessageTriggerGlobalProductLow globalProductLow;
      if (reader.TryStartClassDeserialization<MessageTriggerGlobalProductLow>(out globalProductLow))
        reader.EnqueueDataDeserialization((object) globalProductLow, MessageTriggerGlobalProductLow.s_deserializeDataDelayedAction);
      return globalProductLow;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<MessageTriggerGlobalProductLow>(this, "m_calendar", (object) reader.ReadGenericAs<ICalendar>());
      reader.SetField<MessageTriggerGlobalProductLow>(this, "m_stats", (object) ProductStats.Deserialize(reader));
      reader.SetField<MessageTriggerGlobalProductLow>(this, "m_triggerProto", (object) reader.ReadGenericAs<MessageTriggerGlobalProductLowProto>());
    }

    static MessageTriggerGlobalProductLow()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      MessageTriggerGlobalProductLow.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((MessageTrigger) obj).SerializeData(writer));
      MessageTriggerGlobalProductLow.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((MessageTrigger) obj).DeserializeData(reader));
    }
  }
}
