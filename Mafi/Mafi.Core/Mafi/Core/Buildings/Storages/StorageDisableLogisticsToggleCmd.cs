// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Storages.StorageDisableLogisticsToggleCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Storages
{
  [GenerateSerializer(false, null, 0)]
  public class StorageDisableLogisticsToggleCmd : InputCommand
  {
    public readonly EntityId StorageId;
    public readonly bool IsInput;
    public readonly bool IsDisabled;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public StorageDisableLogisticsToggleCmd(
      IEntityWithSimpleLogisticsControl storage,
      bool isInput,
      bool isDisabled)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(storage.Id, isInput, isDisabled);
    }

    public StorageDisableLogisticsToggleCmd(EntityId storageId, bool isInput, bool isDisabled)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.StorageId = storageId;
      this.IsInput = isInput;
      this.IsDisabled = isDisabled;
    }

    public static void Serialize(StorageDisableLogisticsToggleCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<StorageDisableLogisticsToggleCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, StorageDisableLogisticsToggleCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteBool(this.IsDisabled);
      writer.WriteBool(this.IsInput);
      EntityId.Serialize(this.StorageId, writer);
    }

    public static StorageDisableLogisticsToggleCmd Deserialize(BlobReader reader)
    {
      StorageDisableLogisticsToggleCmd logisticsToggleCmd;
      if (reader.TryStartClassDeserialization<StorageDisableLogisticsToggleCmd>(out logisticsToggleCmd))
        reader.EnqueueDataDeserialization((object) logisticsToggleCmd, StorageDisableLogisticsToggleCmd.s_deserializeDataDelayedAction);
      return logisticsToggleCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<StorageDisableLogisticsToggleCmd>(this, "IsDisabled", (object) reader.ReadBool());
      reader.SetField<StorageDisableLogisticsToggleCmd>(this, "IsInput", (object) reader.ReadBool());
      reader.SetField<StorageDisableLogisticsToggleCmd>(this, "StorageId", (object) EntityId.Deserialize(reader));
    }

    static StorageDisableLogisticsToggleCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      StorageDisableLogisticsToggleCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      StorageDisableLogisticsToggleCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
