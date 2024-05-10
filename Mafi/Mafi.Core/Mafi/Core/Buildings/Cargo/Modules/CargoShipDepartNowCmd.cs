// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Cargo.Modules.CargoShipDepartNowCmd
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
  public class CargoShipDepartNowCmd : InputCommand
  {
    public readonly EntityId CargoShipId;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public CargoShipDepartNowCmd(CargoShip ship)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(ship.Id);
    }

    private CargoShipDepartNowCmd(EntityId shipId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.CargoShipId = shipId;
    }

    public static void Serialize(CargoShipDepartNowCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<CargoShipDepartNowCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, CargoShipDepartNowCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.CargoShipId, writer);
    }

    public static CargoShipDepartNowCmd Deserialize(BlobReader reader)
    {
      CargoShipDepartNowCmd shipDepartNowCmd;
      if (reader.TryStartClassDeserialization<CargoShipDepartNowCmd>(out shipDepartNowCmd))
        reader.EnqueueDataDeserialization((object) shipDepartNowCmd, CargoShipDepartNowCmd.s_deserializeDataDelayedAction);
      return shipDepartNowCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<CargoShipDepartNowCmd>(this, "CargoShipId", (object) EntityId.Deserialize(reader));
    }

    static CargoShipDepartNowCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      CargoShipDepartNowCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      CargoShipDepartNowCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
