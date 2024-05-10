// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Buildings.ThermalStorages.ThermalStorageSetProductCmd
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core;
using Mafi.Core.Input;
using Mafi.Core.Products;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Base.Prototypes.Buildings.ThermalStorages
{
  [GenerateSerializer(false, null, 0)]
  public class ThermalStorageSetProductCmd : InputCommand
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly EntityId StorageId;
    public readonly ProductProto.ID ProductId;

    public static void Serialize(ThermalStorageSetProductCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ThermalStorageSetProductCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ThermalStorageSetProductCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      ProductProto.ID.Serialize(this.ProductId, writer);
      EntityId.Serialize(this.StorageId, writer);
    }

    public static ThermalStorageSetProductCmd Deserialize(BlobReader reader)
    {
      ThermalStorageSetProductCmd storageSetProductCmd;
      if (reader.TryStartClassDeserialization<ThermalStorageSetProductCmd>(out storageSetProductCmd))
        reader.EnqueueDataDeserialization((object) storageSetProductCmd, ThermalStorageSetProductCmd.s_deserializeDataDelayedAction);
      return storageSetProductCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<ThermalStorageSetProductCmd>(this, "ProductId", (object) ProductProto.ID.Deserialize(reader));
      reader.SetField<ThermalStorageSetProductCmd>(this, "StorageId", (object) EntityId.Deserialize(reader));
    }

    public ThermalStorageSetProductCmd(ThermalStorage storage, ProductProto product)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      this.\u002Ector(storage.Id, product.Id);
    }

    private ThermalStorageSetProductCmd(EntityId storageId, ProductProto.ID productId)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.StorageId = storageId;
      this.ProductId = productId;
    }

    static ThermalStorageSetProductCmd()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      ThermalStorageSetProductCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      ThermalStorageSetProductCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
