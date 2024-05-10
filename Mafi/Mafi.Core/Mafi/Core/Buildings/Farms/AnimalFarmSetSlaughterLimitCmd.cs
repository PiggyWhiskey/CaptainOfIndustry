// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Farms.AnimalFarmSetSlaughterLimitCmd
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
  public class AnimalFarmSetSlaughterLimitCmd : InputCommand
  {
    public readonly EntityId AnimalFarmId;
    public readonly int? SlaughterSliderStep;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public AnimalFarmSetSlaughterLimitCmd(AnimalFarm farm, int? slaughterSliderStep)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(farm.Id, slaughterSliderStep);
    }

    public AnimalFarmSetSlaughterLimitCmd(EntityId farmId, int? slaughterSliderStep)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.AnimalFarmId = farmId;
      this.SlaughterSliderStep = slaughterSliderStep;
    }

    public static void Serialize(AnimalFarmSetSlaughterLimitCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<AnimalFarmSetSlaughterLimitCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, AnimalFarmSetSlaughterLimitCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.AnimalFarmId, writer);
      writer.WriteNullableStruct<int>(this.SlaughterSliderStep);
    }

    public static AnimalFarmSetSlaughterLimitCmd Deserialize(BlobReader reader)
    {
      AnimalFarmSetSlaughterLimitCmd slaughterLimitCmd;
      if (reader.TryStartClassDeserialization<AnimalFarmSetSlaughterLimitCmd>(out slaughterLimitCmd))
        reader.EnqueueDataDeserialization((object) slaughterLimitCmd, AnimalFarmSetSlaughterLimitCmd.s_deserializeDataDelayedAction);
      return slaughterLimitCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<AnimalFarmSetSlaughterLimitCmd>(this, "AnimalFarmId", (object) EntityId.Deserialize(reader));
      reader.SetField<AnimalFarmSetSlaughterLimitCmd>(this, "SlaughterSliderStep", (object) reader.ReadNullableStruct<int>());
    }

    static AnimalFarmSetSlaughterLimitCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      AnimalFarmSetSlaughterLimitCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      AnimalFarmSetSlaughterLimitCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
