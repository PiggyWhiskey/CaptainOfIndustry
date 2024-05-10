// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Farms.FarmSetFertilityTargetCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Farms
{
  [GenerateSerializer(false, null, 0)]
  public class FarmSetFertilityTargetCmd : InputCommand
  {
    public readonly EntityId FarmId;
    public readonly Percent Target;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public FarmSetFertilityTargetCmd(Farm farm, Percent target)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(farm.Id, target);
    }

    public FarmSetFertilityTargetCmd(EntityId farmId, Percent target)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.FarmId = farmId;
      this.Target = target;
    }

    public static void Serialize(FarmSetFertilityTargetCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<FarmSetFertilityTargetCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, FarmSetFertilityTargetCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.FarmId, writer);
      Percent.Serialize(this.Target, writer);
    }

    public static FarmSetFertilityTargetCmd Deserialize(BlobReader reader)
    {
      FarmSetFertilityTargetCmd fertilityTargetCmd;
      if (reader.TryStartClassDeserialization<FarmSetFertilityTargetCmd>(out fertilityTargetCmd))
        reader.EnqueueDataDeserialization((object) fertilityTargetCmd, FarmSetFertilityTargetCmd.s_deserializeDataDelayedAction);
      return fertilityTargetCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<FarmSetFertilityTargetCmd>(this, "FarmId", (object) EntityId.Deserialize(reader));
      reader.SetField<FarmSetFertilityTargetCmd>(this, "Target", (object) Percent.Deserialize(reader));
    }

    static FarmSetFertilityTargetCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      FarmSetFertilityTargetCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      FarmSetFertilityTargetCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
