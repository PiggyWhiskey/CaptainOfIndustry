// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.MessageTriggerProto
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
  public abstract class MessageTriggerProto : Proto
  {
    public readonly MessageProto MessageProto;

    public abstract Type Implementation { get; }

    public override bool IsAvailable => base.IsAvailable && this.MessageProto.IsAvailable;

    public MessageTriggerProto(MessageProto messageProto, string version = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(new Proto.ID("TriggerFor_" + messageProto.Id.Value + version), Proto.Str.Empty);
      this.MessageProto = messageProto;
    }
  }
}
