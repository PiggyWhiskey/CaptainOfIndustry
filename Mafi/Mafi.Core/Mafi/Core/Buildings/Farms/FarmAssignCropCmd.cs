// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Farms.FarmAssignCropCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Farms
{
  [GenerateSerializer(false, null, 0)]
  public class FarmAssignCropCmd : InputCommand
  {
    public readonly EntityId FarmId;
    public readonly Proto.ID? CropId;
    public readonly int ScheduleSlot;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public FarmAssignCropCmd(Farm farm, Option<CropProto> crop, int scheduleSlot)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(farm.Id, crop.ValueOrNull?.Id, scheduleSlot);
    }

    public FarmAssignCropCmd(EntityId farmId, Proto.ID? cropId, int scheduleSlot)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.FarmId = farmId;
      this.CropId = cropId;
      this.ScheduleSlot = scheduleSlot.CheckNotNegative();
    }

    public static void Serialize(FarmAssignCropCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<FarmAssignCropCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, FarmAssignCropCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteNullableStruct<Proto.ID>(this.CropId);
      EntityId.Serialize(this.FarmId, writer);
      writer.WriteInt(this.ScheduleSlot);
    }

    public static FarmAssignCropCmd Deserialize(BlobReader reader)
    {
      FarmAssignCropCmd farmAssignCropCmd;
      if (reader.TryStartClassDeserialization<FarmAssignCropCmd>(out farmAssignCropCmd))
        reader.EnqueueDataDeserialization((object) farmAssignCropCmd, FarmAssignCropCmd.s_deserializeDataDelayedAction);
      return farmAssignCropCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<FarmAssignCropCmd>(this, "CropId", (object) reader.ReadNullableStruct<Proto.ID>());
      reader.SetField<FarmAssignCropCmd>(this, "FarmId", (object) EntityId.Deserialize(reader));
      reader.SetField<FarmAssignCropCmd>(this, "ScheduleSlot", (object) reader.ReadInt());
    }

    static FarmAssignCropCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      FarmAssignCropCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      FarmAssignCropCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
