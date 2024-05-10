// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.MessageTrigger
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Simulation;
using Mafi.Core.Utils;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Messages
{
  public abstract class MessageTrigger
  {
    private readonly MessagesManager m_messageManager;
    private readonly ISimLoopEvents m_simLoopEvents;
    private readonly MessageProto m_message;
    private readonly TickTimer m_tickTimer;

    protected ISimLoopEvents SimLoopEvents => this.m_simLoopEvents;

    protected MessageTrigger(
      MessageTriggerProto triggerProto,
      ISimLoopEvents simLoopEvents,
      MessagesManager messageManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_tickTimer = new TickTimer();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_message = triggerProto.MessageProto;
      this.m_messageManager = messageManager;
      this.m_simLoopEvents = simLoopEvents;
    }

    protected void DeliverMessage(Duration delay = default (Duration))
    {
      this.OnDestroy();
      if (this.m_message.IsObsolete)
        return;
      if (delay.IsZero)
      {
        this.m_messageManager.AddMessage(this.m_message);
      }
      else
      {
        this.m_simLoopEvents.Update.Add<MessageTrigger>(this, new Action(this.onTimerSimUpdate));
        this.m_tickTimer.Start(delay);
      }
    }

    private void onTimerSimUpdate()
    {
      if (this.m_tickTimer.Decrement())
        return;
      this.m_messageManager.AddMessage(this.m_message);
      this.m_simLoopEvents.Update.Remove<MessageTrigger>(this, new Action(this.onTimerSimUpdate));
    }

    protected abstract void OnDestroy();

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteGeneric<MessageProto>(this.m_message);
      MessagesManager.Serialize(this.m_messageManager, writer);
      writer.WriteGeneric<ISimLoopEvents>(this.m_simLoopEvents);
      TickTimer.Serialize(this.m_tickTimer, writer);
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<MessageTrigger>(this, "m_message", (object) reader.ReadGenericAs<MessageProto>());
      reader.SetField<MessageTrigger>(this, "m_messageManager", (object) MessagesManager.Deserialize(reader));
      reader.SetField<MessageTrigger>(this, "m_simLoopEvents", (object) reader.ReadGenericAs<ISimLoopEvents>());
      reader.SetField<MessageTrigger>(this, "m_tickTimer", (object) TickTimer.Deserialize(reader));
    }
  }
}
