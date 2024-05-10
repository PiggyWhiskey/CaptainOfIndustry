// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.MessageTriggerOnShipRepair
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Simulation;
using Mafi.Core.World;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Messages
{
  [GenerateSerializer(false, null, 0)]
  public class MessageTriggerOnShipRepair : MessageTrigger
  {
    private readonly ISimLoopEvents m_simLoopEvents;
    private readonly TravelingFleetManager m_travelingFleetManager;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public MessageTriggerOnShipRepair(
      MessageTriggerOnShipRepairProto triggerProto,
      ISimLoopEvents simLoopEvents,
      MessagesManager messageManager,
      TravelingFleetManager travelingFleetManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector((MessageTriggerProto) triggerProto, simLoopEvents, messageManager);
      this.m_simLoopEvents = simLoopEvents;
      this.m_travelingFleetManager = travelingFleetManager;
      simLoopEvents.Update.Add<MessageTriggerOnShipRepair>(this, new Action(this.checkShipRepaired));
    }

    private void checkShipRepaired()
    {
      if (!this.m_travelingFleetManager.HasFleet || !this.m_travelingFleetManager.TravelingFleet.HasEnoughHpToOperate)
        return;
      this.DeliverMessage();
    }

    protected override void OnDestroy()
    {
      this.m_simLoopEvents.Update.Remove<MessageTriggerOnShipRepair>(this, new Action(this.checkShipRepaired));
    }

    public static void Serialize(MessageTriggerOnShipRepair value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<MessageTriggerOnShipRepair>(value))
        return;
      writer.EnqueueDataSerialization((object) value, MessageTriggerOnShipRepair.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteGeneric<ISimLoopEvents>(this.m_simLoopEvents);
      TravelingFleetManager.Serialize(this.m_travelingFleetManager, writer);
    }

    public static MessageTriggerOnShipRepair Deserialize(BlobReader reader)
    {
      MessageTriggerOnShipRepair triggerOnShipRepair;
      if (reader.TryStartClassDeserialization<MessageTriggerOnShipRepair>(out triggerOnShipRepair))
        reader.EnqueueDataDeserialization((object) triggerOnShipRepair, MessageTriggerOnShipRepair.s_deserializeDataDelayedAction);
      return triggerOnShipRepair;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<MessageTriggerOnShipRepair>(this, "m_simLoopEvents", (object) reader.ReadGenericAs<ISimLoopEvents>());
      reader.SetField<MessageTriggerOnShipRepair>(this, "m_travelingFleetManager", (object) TravelingFleetManager.Deserialize(reader));
    }

    static MessageTriggerOnShipRepair()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      MessageTriggerOnShipRepair.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((MessageTrigger) obj).SerializeData(writer));
      MessageTriggerOnShipRepair.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((MessageTrigger) obj).DeserializeData(reader));
    }
  }
}
