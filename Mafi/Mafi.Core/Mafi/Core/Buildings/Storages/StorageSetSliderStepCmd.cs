// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Storages.StorageSetSliderStepCmd
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
  public class StorageSetSliderStepCmd : InputCommand
  {
    public readonly EntityId StorageId;
    /// <summary>Negative if undefined.</summary>
    public readonly int ImportStep;
    /// <summary>Negative if undefined.</summary>
    public readonly int ExportStep;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public StorageSetSliderStepCmd(Storage storage, int? importStep, int? exportStep)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(storage.Id, importStep, exportStep);
    }

    public StorageSetSliderStepCmd(EntityId storageId, int? importStep, int? exportStep)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.StorageId = storageId;
      this.ImportStep = importStep.GetValueOrDefault(-1).CheckWithinIncl(-1, 10);
      this.ExportStep = exportStep.GetValueOrDefault(-1).CheckWithinIncl(-1, 10);
    }

    public StorageSetSliderStepCmd(EntityId storageId, Percent? importUntil = null, Percent? exportFrom = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.StorageId = storageId;
      this.ImportStep = (importUntil.HasValue ? importUntil.GetValueOrDefault().Apply(10) : -1).CheckWithinIncl(-1, 10);
      this.ExportStep = (exportFrom.HasValue ? exportFrom.GetValueOrDefault().Apply(10) : -1).CheckWithinIncl(-1, 10);
    }

    public static void Serialize(StorageSetSliderStepCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<StorageSetSliderStepCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, StorageSetSliderStepCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteInt(this.ExportStep);
      writer.WriteInt(this.ImportStep);
      EntityId.Serialize(this.StorageId, writer);
    }

    public static StorageSetSliderStepCmd Deserialize(BlobReader reader)
    {
      StorageSetSliderStepCmd setSliderStepCmd;
      if (reader.TryStartClassDeserialization<StorageSetSliderStepCmd>(out setSliderStepCmd))
        reader.EnqueueDataDeserialization((object) setSliderStepCmd, StorageSetSliderStepCmd.s_deserializeDataDelayedAction);
      return setSliderStepCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<StorageSetSliderStepCmd>(this, "ExportStep", (object) reader.ReadInt());
      reader.SetField<StorageSetSliderStepCmd>(this, "ImportStep", (object) reader.ReadInt());
      reader.SetField<StorageSetSliderStepCmd>(this, "StorageId", (object) EntityId.Deserialize(reader));
    }

    static StorageSetSliderStepCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      StorageSetSliderStepCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      StorageSetSliderStepCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
