// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.MessageTriggerOnQuantityProduced
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Products;
using Mafi.Core.Simulation;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Messages
{
  [GenerateSerializer(false, null, 0)]
  public class MessageTriggerOnQuantityProduced : MessageTrigger
  {
    private readonly MessageTriggerOnQuantityProducedProto m_proto;
    private readonly ProductStats m_productStats;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public MessageTriggerOnQuantityProduced(
      MessageTriggerOnQuantityProducedProto triggerProto,
      IProductsManager productsManager,
      ISimLoopEvents simLoopEvents,
      MessagesManager messageManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector((MessageTriggerProto) triggerProto, simLoopEvents, messageManager);
      this.m_proto = triggerProto;
      this.m_productStats = productsManager.GetStatsFor(this.m_proto.Product);
      simLoopEvents.Update.Add<MessageTriggerOnQuantityProduced>(this, new Action(this.simUpdate));
    }

    private void simUpdate()
    {
      if (this.m_proto.IsObsolete)
      {
        this.OnDestroy();
      }
      else
      {
        if (!(this.m_productStats.CreatedByProduction.Lifetime > (QuantityLarge) this.m_proto.Quantity))
          return;
        this.DeliverMessage();
      }
    }

    protected override void OnDestroy()
    {
      this.SimLoopEvents.Update.Remove<MessageTriggerOnQuantityProduced>(this, new Action(this.simUpdate));
    }

    public static void Serialize(MessageTriggerOnQuantityProduced value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<MessageTriggerOnQuantityProduced>(value))
        return;
      writer.EnqueueDataSerialization((object) value, MessageTriggerOnQuantityProduced.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      ProductStats.Serialize(this.m_productStats, writer);
      writer.WriteGeneric<MessageTriggerOnQuantityProducedProto>(this.m_proto);
    }

    public static MessageTriggerOnQuantityProduced Deserialize(BlobReader reader)
    {
      MessageTriggerOnQuantityProduced quantityProduced;
      if (reader.TryStartClassDeserialization<MessageTriggerOnQuantityProduced>(out quantityProduced))
        reader.EnqueueDataDeserialization((object) quantityProduced, MessageTriggerOnQuantityProduced.s_deserializeDataDelayedAction);
      return quantityProduced;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<MessageTriggerOnQuantityProduced>(this, "m_productStats", (object) ProductStats.Deserialize(reader));
      reader.SetField<MessageTriggerOnQuantityProduced>(this, "m_proto", (object) reader.ReadGenericAs<MessageTriggerOnQuantityProducedProto>());
    }

    static MessageTriggerOnQuantityProduced()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      MessageTriggerOnQuantityProduced.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((MessageTrigger) obj).SerializeData(writer));
      MessageTriggerOnQuantityProduced.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((MessageTrigger) obj).DeserializeData(reader));
    }
  }
}
