// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Storages.StorageSetProductCmd
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
namespace Mafi.Core.Buildings.Storages
{
  [GenerateSerializer(false, null, 0)]
  public class StorageSetProductCmd : InputCommand
  {
    public readonly EntityId StorageId;
    public readonly ProductProto.ID ProductId;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public StorageSetProductCmd(Storage storage, ProductProto product)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(storage.Id, product.Id);
    }

    public StorageSetProductCmd(EntityId storageId, ProductProto.ID productId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.StorageId = storageId;
      this.ProductId = productId;
    }

    public static void Serialize(StorageSetProductCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<StorageSetProductCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, StorageSetProductCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      ProductProto.ID.Serialize(this.ProductId, writer);
      EntityId.Serialize(this.StorageId, writer);
    }

    public static StorageSetProductCmd Deserialize(BlobReader reader)
    {
      StorageSetProductCmd storageSetProductCmd;
      if (reader.TryStartClassDeserialization<StorageSetProductCmd>(out storageSetProductCmd))
        reader.EnqueueDataDeserialization((object) storageSetProductCmd, StorageSetProductCmd.s_deserializeDataDelayedAction);
      return storageSetProductCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<StorageSetProductCmd>(this, "ProductId", (object) ProductProto.ID.Deserialize(reader));
      reader.SetField<StorageSetProductCmd>(this, "StorageId", (object) EntityId.Deserialize(reader));
    }

    static StorageSetProductCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      StorageSetProductCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      StorageSetProductCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
