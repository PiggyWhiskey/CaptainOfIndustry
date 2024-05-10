// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Storages.StorageAlertSetThresholdCmd
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
  public class StorageAlertSetThresholdCmd : InputCommand
  {
    public readonly EntityId StorageId;
    public readonly Percent Value;
    public readonly bool IsAbove;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public StorageAlertSetThresholdCmd(IEntity storage, Percent value, bool isAbove)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(storage.Id, value, isAbove);
    }

    public StorageAlertSetThresholdCmd(EntityId storageId, Percent value, bool isAbove)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.StorageId = storageId;
      this.Value = value;
      this.IsAbove = isAbove;
    }

    public static void Serialize(StorageAlertSetThresholdCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<StorageAlertSetThresholdCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, StorageAlertSetThresholdCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteBool(this.IsAbove);
      EntityId.Serialize(this.StorageId, writer);
      Percent.Serialize(this.Value, writer);
    }

    public static StorageAlertSetThresholdCmd Deserialize(BlobReader reader)
    {
      StorageAlertSetThresholdCmd alertSetThresholdCmd;
      if (reader.TryStartClassDeserialization<StorageAlertSetThresholdCmd>(out alertSetThresholdCmd))
        reader.EnqueueDataDeserialization((object) alertSetThresholdCmd, StorageAlertSetThresholdCmd.s_deserializeDataDelayedAction);
      return alertSetThresholdCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<StorageAlertSetThresholdCmd>(this, "IsAbove", (object) reader.ReadBool());
      reader.SetField<StorageAlertSetThresholdCmd>(this, "StorageId", (object) EntityId.Deserialize(reader));
      reader.SetField<StorageAlertSetThresholdCmd>(this, "Value", (object) Percent.Deserialize(reader));
    }

    static StorageAlertSetThresholdCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      StorageAlertSetThresholdCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      StorageAlertSetThresholdCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
