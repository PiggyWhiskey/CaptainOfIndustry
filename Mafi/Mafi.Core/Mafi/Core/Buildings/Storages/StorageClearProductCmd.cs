// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Storages.StorageClearProductCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Storages
{
  [GenerateSerializer(false, null, 0)]
  public class StorageClearProductCmd : InputCommand
  {
    public readonly EntityId StorageId;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public StorageClearProductCmd(Storage storage)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(storage.Id);
    }

    private StorageClearProductCmd(EntityId storageId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.StorageId = storageId;
    }

    public static void Serialize(StorageClearProductCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<StorageClearProductCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, StorageClearProductCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.StorageId, writer);
    }

    public static StorageClearProductCmd Deserialize(BlobReader reader)
    {
      StorageClearProductCmd storageClearProductCmd;
      if (reader.TryStartClassDeserialization<StorageClearProductCmd>(out storageClearProductCmd))
        reader.EnqueueDataDeserialization((object) storageClearProductCmd, StorageClearProductCmd.s_deserializeDataDelayedAction);
      return storageClearProductCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<StorageClearProductCmd>(this, "StorageId", (object) EntityId.Deserialize(reader));
    }

    static StorageClearProductCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      StorageClearProductCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      StorageClearProductCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
