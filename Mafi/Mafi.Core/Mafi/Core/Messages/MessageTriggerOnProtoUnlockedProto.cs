// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.MessageTriggerOnProtoUnlockedProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Prototypes;
using System;

#nullable disable
namespace Mafi.Core.Messages
{
  public class MessageTriggerOnProtoUnlockedProto : MessageTriggerProto
  {
    public readonly Proto UnlockedProto;

    public override Type Implementation => typeof (MessageTriggerOnProtoUnlocked);

    public MessageTriggerOnProtoUnlockedProto(MessageProto messageProto, Proto unlockedProto)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(messageProto);
      this.UnlockedProto = unlockedProto;
    }
  }
}
