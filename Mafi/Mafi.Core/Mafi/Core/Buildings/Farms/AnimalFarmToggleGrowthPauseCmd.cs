// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Farms.AnimalFarmToggleGrowthPauseCmd
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
  public class AnimalFarmToggleGrowthPauseCmd : InputCommand
  {
    public readonly EntityId AnimalFarmId;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public AnimalFarmToggleGrowthPauseCmd(AnimalFarm farm)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(farm.Id);
    }

    public AnimalFarmToggleGrowthPauseCmd(EntityId farmId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.AnimalFarmId = farmId;
    }

    public static void Serialize(AnimalFarmToggleGrowthPauseCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<AnimalFarmToggleGrowthPauseCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, AnimalFarmToggleGrowthPauseCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.AnimalFarmId, writer);
    }

    public static AnimalFarmToggleGrowthPauseCmd Deserialize(BlobReader reader)
    {
      AnimalFarmToggleGrowthPauseCmd toggleGrowthPauseCmd;
      if (reader.TryStartClassDeserialization<AnimalFarmToggleGrowthPauseCmd>(out toggleGrowthPauseCmd))
        reader.EnqueueDataDeserialization((object) toggleGrowthPauseCmd, AnimalFarmToggleGrowthPauseCmd.s_deserializeDataDelayedAction);
      return toggleGrowthPauseCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<AnimalFarmToggleGrowthPauseCmd>(this, "AnimalFarmId", (object) EntityId.Deserialize(reader));
    }

    static AnimalFarmToggleGrowthPauseCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      AnimalFarmToggleGrowthPauseCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      AnimalFarmToggleGrowthPauseCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
