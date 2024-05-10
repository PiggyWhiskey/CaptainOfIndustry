// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Mine.AddProductToNotifyIfCannotDumpCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Core.Products;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Mine
{
  [GenerateSerializer(false, null, 0)]
  public class AddProductToNotifyIfCannotDumpCmd : InputCommand
  {
    public readonly EntityId MineTowerId;
    public readonly ProductProto.ID ProductId;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public AddProductToNotifyIfCannotDumpCmd(MineTower mineTower, ProductProto productProto)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(mineTower.Id, new ProductProto.ID(productProto.Id.Value));
    }

    private AddProductToNotifyIfCannotDumpCmd(EntityId mineTowerId, ProductProto.ID productId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.MineTowerId = mineTowerId;
      this.ProductId = productId;
    }

    public static void Serialize(AddProductToNotifyIfCannotDumpCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<AddProductToNotifyIfCannotDumpCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, AddProductToNotifyIfCannotDumpCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.MineTowerId, writer);
      ProductProto.ID.Serialize(this.ProductId, writer);
    }

    public static AddProductToNotifyIfCannotDumpCmd Deserialize(BlobReader reader)
    {
      AddProductToNotifyIfCannotDumpCmd notifyIfCannotDumpCmd;
      if (reader.TryStartClassDeserialization<AddProductToNotifyIfCannotDumpCmd>(out notifyIfCannotDumpCmd))
        reader.EnqueueDataDeserialization((object) notifyIfCannotDumpCmd, AddProductToNotifyIfCannotDumpCmd.s_deserializeDataDelayedAction);
      return notifyIfCannotDumpCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<AddProductToNotifyIfCannotDumpCmd>(this, "MineTowerId", (object) EntityId.Deserialize(reader));
      reader.SetField<AddProductToNotifyIfCannotDumpCmd>(this, "ProductId", (object) ProductProto.ID.Deserialize(reader));
    }

    static AddProductToNotifyIfCannotDumpCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      AddProductToNotifyIfCannotDumpCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      AddProductToNotifyIfCannotDumpCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
