// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.MessageTriggerOnEventProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi.Core.Messages
{
  public class MessageTriggerOnEventProto : MessageTriggerProto
  {
    public readonly int DelayDays;
    public readonly Func<IResolver, IEvent> GetEvent;

    public override Type Implementation => typeof (MessageTriggerOnEvent);

    public MessageTriggerOnEventProto(
      MessageProto messageProto,
      int delayDays,
      Func<IResolver, IEvent> getEvent)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(messageProto);
      this.DelayDays = delayDays;
      this.GetEvent = getEvent;
    }
  }
}
