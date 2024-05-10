// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Farms.FarmHarvestNowCmd
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
  public class FarmHarvestNowCmd : InputCommand
  {
    public readonly EntityId FarmId;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public FarmHarvestNowCmd(Farm farm)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(farm.Id);
    }

    public FarmHarvestNowCmd(EntityId farmId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.FarmId = farmId;
    }

    public static void Serialize(FarmHarvestNowCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<FarmHarvestNowCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, FarmHarvestNowCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.FarmId, writer);
    }

    public static FarmHarvestNowCmd Deserialize(BlobReader reader)
    {
      FarmHarvestNowCmd farmHarvestNowCmd;
      if (reader.TryStartClassDeserialization<FarmHarvestNowCmd>(out farmHarvestNowCmd))
        reader.EnqueueDataDeserialization((object) farmHarvestNowCmd, FarmHarvestNowCmd.s_deserializeDataDelayedAction);
      return farmHarvestNowCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<FarmHarvestNowCmd>(this, "FarmId", (object) EntityId.Deserialize(reader));
    }

    static FarmHarvestNowCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      FarmHarvestNowCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      FarmHarvestNowCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
