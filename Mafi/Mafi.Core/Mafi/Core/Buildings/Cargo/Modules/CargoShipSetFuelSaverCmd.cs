// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Cargo.Modules.CargoShipSetFuelSaverCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.Cargo.Ships;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Cargo.Modules
{
  [GenerateSerializer(false, null, 0)]
  public class CargoShipSetFuelSaverCmd : InputCommand
  {
    public readonly EntityId CargoShipId;
    public readonly bool IsFuelSaverEnabled;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public CargoShipSetFuelSaverCmd(CargoShip ship, bool isFuelSaverEnabled)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(ship.Id, isFuelSaverEnabled);
    }

    private CargoShipSetFuelSaverCmd(EntityId shipId, bool isFuelSaverEnabled)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.CargoShipId = shipId;
      this.IsFuelSaverEnabled = isFuelSaverEnabled;
    }

    public static void Serialize(CargoShipSetFuelSaverCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<CargoShipSetFuelSaverCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, CargoShipSetFuelSaverCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.CargoShipId, writer);
      writer.WriteBool(this.IsFuelSaverEnabled);
    }

    public static CargoShipSetFuelSaverCmd Deserialize(BlobReader reader)
    {
      CargoShipSetFuelSaverCmd shipSetFuelSaverCmd;
      if (reader.TryStartClassDeserialization<CargoShipSetFuelSaverCmd>(out shipSetFuelSaverCmd))
        reader.EnqueueDataDeserialization((object) shipSetFuelSaverCmd, CargoShipSetFuelSaverCmd.s_deserializeDataDelayedAction);
      return shipSetFuelSaverCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<CargoShipSetFuelSaverCmd>(this, "CargoShipId", (object) EntityId.Deserialize(reader));
      reader.SetField<CargoShipSetFuelSaverCmd>(this, "IsFuelSaverEnabled", (object) reader.ReadBool());
    }

    static CargoShipSetFuelSaverCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      CargoShipSetFuelSaverCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      CargoShipSetFuelSaverCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
