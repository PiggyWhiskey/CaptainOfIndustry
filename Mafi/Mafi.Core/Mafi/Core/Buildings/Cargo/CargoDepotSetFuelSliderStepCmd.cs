// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Cargo.CargoDepotSetFuelSliderStepCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Cargo
{
  [GenerateSerializer(false, null, 0)]
  public class CargoDepotSetFuelSliderStepCmd : InputCommand
  {
    public readonly EntityId CargoDepotId;
    /// <summary>Negative if undefined.</summary>
    public readonly int ImportStep;
    /// <summary>Negative if undefined.</summary>
    public readonly int ExportStep;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public CargoDepotSetFuelSliderStepCmd(EntityId cargoDepotId, int? importStep, int? exportStep)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.CargoDepotId = cargoDepotId;
      this.ImportStep = importStep.GetValueOrDefault(-1).CheckWithinIncl(-1, 10);
      this.ExportStep = exportStep.GetValueOrDefault(-1).CheckWithinIncl(-1, 10);
    }

    public static void Serialize(CargoDepotSetFuelSliderStepCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<CargoDepotSetFuelSliderStepCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, CargoDepotSetFuelSliderStepCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.CargoDepotId, writer);
      writer.WriteInt(this.ExportStep);
      writer.WriteInt(this.ImportStep);
    }

    public static CargoDepotSetFuelSliderStepCmd Deserialize(BlobReader reader)
    {
      CargoDepotSetFuelSliderStepCmd fuelSliderStepCmd;
      if (reader.TryStartClassDeserialization<CargoDepotSetFuelSliderStepCmd>(out fuelSliderStepCmd))
        reader.EnqueueDataDeserialization((object) fuelSliderStepCmd, CargoDepotSetFuelSliderStepCmd.s_deserializeDataDelayedAction);
      return fuelSliderStepCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<CargoDepotSetFuelSliderStepCmd>(this, "CargoDepotId", (object) EntityId.Deserialize(reader));
      reader.SetField<CargoDepotSetFuelSliderStepCmd>(this, "ExportStep", (object) reader.ReadInt());
      reader.SetField<CargoDepotSetFuelSliderStepCmd>(this, "ImportStep", (object) reader.ReadInt());
    }

    static CargoDepotSetFuelSliderStepCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      CargoDepotSetFuelSliderStepCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      CargoDepotSetFuelSliderStepCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
