// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Shipyard.ShipayardSetFuelSliderStepCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Shipyard
{
  [GenerateSerializer(false, null, 0)]
  public class ShipayardSetFuelSliderStepCmd : InputCommand
  {
    public readonly EntityId ShipyardId;
    /// <summary>Negative if undefined.</summary>
    public readonly int ImportStep;
    /// <summary>Negative if undefined.</summary>
    public readonly int ExportStep;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public ShipayardSetFuelSliderStepCmd(EntityId shipyardId, int? importStep, int? exportStep)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ShipyardId = shipyardId;
      this.ImportStep = importStep.GetValueOrDefault(-1).CheckWithinIncl(-1, 10);
      this.ExportStep = exportStep.GetValueOrDefault(-1).CheckWithinIncl(-1, 10);
    }

    public static void Serialize(ShipayardSetFuelSliderStepCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ShipayardSetFuelSliderStepCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ShipayardSetFuelSliderStepCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteInt(this.ExportStep);
      writer.WriteInt(this.ImportStep);
      EntityId.Serialize(this.ShipyardId, writer);
    }

    public static ShipayardSetFuelSliderStepCmd Deserialize(BlobReader reader)
    {
      ShipayardSetFuelSliderStepCmd fuelSliderStepCmd;
      if (reader.TryStartClassDeserialization<ShipayardSetFuelSliderStepCmd>(out fuelSliderStepCmd))
        reader.EnqueueDataDeserialization((object) fuelSliderStepCmd, ShipayardSetFuelSliderStepCmd.s_deserializeDataDelayedAction);
      return fuelSliderStepCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<ShipayardSetFuelSliderStepCmd>(this, "ExportStep", (object) reader.ReadInt());
      reader.SetField<ShipayardSetFuelSliderStepCmd>(this, "ImportStep", (object) reader.ReadInt());
      reader.SetField<ShipayardSetFuelSliderStepCmd>(this, "ShipyardId", (object) EntityId.Deserialize(reader));
    }

    static ShipayardSetFuelSliderStepCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ShipayardSetFuelSliderStepCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      ShipayardSetFuelSliderStepCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
