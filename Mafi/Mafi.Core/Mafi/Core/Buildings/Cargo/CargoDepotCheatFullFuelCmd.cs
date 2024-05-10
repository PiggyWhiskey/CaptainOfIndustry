// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Cargo.CargoDepotCheatFullFuelCmd
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
  public class CargoDepotCheatFullFuelCmd : InputCommand
  {
    public readonly EntityId CargoDepotId;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public CargoDepotCheatFullFuelCmd(EntityId cargoDepotId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.CargoDepotId = cargoDepotId;
    }

    public static void Serialize(CargoDepotCheatFullFuelCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<CargoDepotCheatFullFuelCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, CargoDepotCheatFullFuelCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.CargoDepotId, writer);
    }

    public static CargoDepotCheatFullFuelCmd Deserialize(BlobReader reader)
    {
      CargoDepotCheatFullFuelCmd cheatFullFuelCmd;
      if (reader.TryStartClassDeserialization<CargoDepotCheatFullFuelCmd>(out cheatFullFuelCmd))
        reader.EnqueueDataDeserialization((object) cheatFullFuelCmd, CargoDepotCheatFullFuelCmd.s_deserializeDataDelayedAction);
      return cheatFullFuelCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<CargoDepotCheatFullFuelCmd>(this, "CargoDepotId", (object) EntityId.Deserialize(reader));
    }

    static CargoDepotCheatFullFuelCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      CargoDepotCheatFullFuelCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      CargoDepotCheatFullFuelCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
