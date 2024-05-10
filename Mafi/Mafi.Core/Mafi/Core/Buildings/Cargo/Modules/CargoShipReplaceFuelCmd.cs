// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Cargo.Modules.CargoShipReplaceFuelCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Core.Products;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Cargo.Modules
{
  [GenerateSerializer(false, null, 0)]
  public class CargoShipReplaceFuelCmd : InputCommand
  {
    public readonly EntityId CargoShipId;
    public readonly ProductProto.ID FuelId;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public CargoShipReplaceFuelCmd(EntityId cargoShipId, ProductProto.ID fuelId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.CargoShipId = cargoShipId;
      this.FuelId = fuelId;
    }

    public static void Serialize(CargoShipReplaceFuelCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<CargoShipReplaceFuelCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, CargoShipReplaceFuelCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.CargoShipId, writer);
      ProductProto.ID.Serialize(this.FuelId, writer);
    }

    public static CargoShipReplaceFuelCmd Deserialize(BlobReader reader)
    {
      CargoShipReplaceFuelCmd shipReplaceFuelCmd;
      if (reader.TryStartClassDeserialization<CargoShipReplaceFuelCmd>(out shipReplaceFuelCmd))
        reader.EnqueueDataDeserialization((object) shipReplaceFuelCmd, CargoShipReplaceFuelCmd.s_deserializeDataDelayedAction);
      return shipReplaceFuelCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<CargoShipReplaceFuelCmd>(this, "CargoShipId", (object) EntityId.Deserialize(reader));
      reader.SetField<CargoShipReplaceFuelCmd>(this, "FuelId", (object) ProductProto.ID.Deserialize(reader));
    }

    static CargoShipReplaceFuelCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      CargoShipReplaceFuelCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      CargoShipReplaceFuelCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
