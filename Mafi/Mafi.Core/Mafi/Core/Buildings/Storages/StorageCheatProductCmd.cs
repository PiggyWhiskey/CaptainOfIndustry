// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Storages.StorageCheatProductCmd
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
  public class StorageCheatProductCmd : InputCommand
  {
    public readonly EntityId StorageId;
    public readonly ProductProto.ID ProductId;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public StorageCheatProductCmd(Storage storage, ProductProto product)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(storage.Id, new ProductProto.ID(product.Id.Value));
    }

    public StorageCheatProductCmd(EntityId storageId, ProductProto.ID productId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.StorageId = storageId;
      this.ProductId = productId;
    }

    public static void Serialize(StorageCheatProductCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<StorageCheatProductCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, StorageCheatProductCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      ProductProto.ID.Serialize(this.ProductId, writer);
      EntityId.Serialize(this.StorageId, writer);
    }

    public static StorageCheatProductCmd Deserialize(BlobReader reader)
    {
      StorageCheatProductCmd storageCheatProductCmd;
      if (reader.TryStartClassDeserialization<StorageCheatProductCmd>(out storageCheatProductCmd))
        reader.EnqueueDataDeserialization((object) storageCheatProductCmd, StorageCheatProductCmd.s_deserializeDataDelayedAction);
      return storageCheatProductCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<StorageCheatProductCmd>(this, "ProductId", (object) ProductProto.ID.Deserialize(reader));
      reader.SetField<StorageCheatProductCmd>(this, "StorageId", (object) EntityId.Deserialize(reader));
    }

    static StorageCheatProductCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      StorageCheatProductCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      StorageCheatProductCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
