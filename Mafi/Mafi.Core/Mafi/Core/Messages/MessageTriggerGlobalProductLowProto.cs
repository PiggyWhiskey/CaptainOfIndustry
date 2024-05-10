﻿// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.MessageTriggerGlobalProductLowProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Products;
using System;

#nullable disable
namespace Mafi.Core.Messages
{
  public class MessageTriggerGlobalProductLowProto : MessageTriggerProto
  {
    public readonly ProductProto.ID ProductId;
    public readonly QuantityLarge MinQuantity;

    public override Type Implementation => typeof (MessageTriggerGlobalProductLow);

    public MessageTriggerGlobalProductLowProto(
      MessageProto messageProto,
      ProductProto.ID productId,
      QuantityLarge minQuantity)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(messageProto);
      this.ProductId = productId;
      this.MinQuantity = minQuantity;
    }
  }
}
