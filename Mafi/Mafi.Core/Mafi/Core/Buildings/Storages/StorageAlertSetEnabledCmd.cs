// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Storages.StorageAlertSetEnabledCmd
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
  public class StorageAlertSetEnabledCmd : InputCommand
  {
    public readonly EntityId StorageId;
    public readonly bool IsEnabled;
    public readonly bool IsAbove;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public StorageAlertSetEnabledCmd(IEntity storage, bool isEnabled, bool isAbove)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(storage.Id, isEnabled, isAbove);
    }

    public StorageAlertSetEnabledCmd(EntityId storageId, bool isEnabled, bool isAbove)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.StorageId = storageId;
      this.IsEnabled = isEnabled;
      this.IsAbove = isAbove;
    }

    public static void Serialize(StorageAlertSetEnabledCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<StorageAlertSetEnabledCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, StorageAlertSetEnabledCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteBool(this.IsAbove);
      writer.WriteBool(this.IsEnabled);
      EntityId.Serialize(this.StorageId, writer);
    }

    public static StorageAlertSetEnabledCmd Deserialize(BlobReader reader)
    {
      StorageAlertSetEnabledCmd alertSetEnabledCmd;
      if (reader.TryStartClassDeserialization<StorageAlertSetEnabledCmd>(out alertSetEnabledCmd))
        reader.EnqueueDataDeserialization((object) alertSetEnabledCmd, StorageAlertSetEnabledCmd.s_deserializeDataDelayedAction);
      return alertSetEnabledCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<StorageAlertSetEnabledCmd>(this, "IsAbove", (object) reader.ReadBool());
      reader.SetField<StorageAlertSetEnabledCmd>(this, "IsEnabled", (object) reader.ReadBool());
      reader.SetField<StorageAlertSetEnabledCmd>(this, "StorageId", (object) EntityId.Deserialize(reader));
    }

    static StorageAlertSetEnabledCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      StorageAlertSetEnabledCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      StorageAlertSetEnabledCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
