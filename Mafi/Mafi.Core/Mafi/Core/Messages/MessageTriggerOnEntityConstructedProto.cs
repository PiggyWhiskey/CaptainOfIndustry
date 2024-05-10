// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.MessageTriggerOnEntityConstructedProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Static;
using System;

#nullable disable
namespace Mafi.Core.Messages
{
  public class MessageTriggerOnEntityConstructedProto : MessageTriggerProto
  {
    public readonly StaticEntityProto.ID ConstructedProtoId;

    public override Type Implementation => typeof (MessageTriggerOnEntityConstructed);

    public MessageTriggerOnEntityConstructedProto(
      MessageProto messageProto,
      StaticEntityProto.ID protoId,
      string version = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(messageProto, version);
      this.ConstructedProtoId = protoId;
    }
  }
}
