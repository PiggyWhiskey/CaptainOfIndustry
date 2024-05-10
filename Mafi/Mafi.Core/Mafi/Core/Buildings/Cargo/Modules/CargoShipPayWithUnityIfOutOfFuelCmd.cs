// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Cargo.Modules.CargoShipPayWithUnityIfOutOfFuelCmd
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
  public class CargoShipPayWithUnityIfOutOfFuelCmd : InputCommand
  {
    public readonly EntityId CargoShipId;
    public readonly bool PayWithUnity;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public CargoShipPayWithUnityIfOutOfFuelCmd(CargoShip ship, bool payWithUnity)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(ship.Id, payWithUnity);
    }

    private CargoShipPayWithUnityIfOutOfFuelCmd(EntityId shipId, bool payWithUnity)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.CargoShipId = shipId;
      this.PayWithUnity = payWithUnity;
    }

    public static void Serialize(CargoShipPayWithUnityIfOutOfFuelCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<CargoShipPayWithUnityIfOutOfFuelCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, CargoShipPayWithUnityIfOutOfFuelCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.CargoShipId, writer);
      writer.WriteBool(this.PayWithUnity);
    }

    public static CargoShipPayWithUnityIfOutOfFuelCmd Deserialize(BlobReader reader)
    {
      CargoShipPayWithUnityIfOutOfFuelCmd unityIfOutOfFuelCmd;
      if (reader.TryStartClassDeserialization<CargoShipPayWithUnityIfOutOfFuelCmd>(out unityIfOutOfFuelCmd))
        reader.EnqueueDataDeserialization((object) unityIfOutOfFuelCmd, CargoShipPayWithUnityIfOutOfFuelCmd.s_deserializeDataDelayedAction);
      return unityIfOutOfFuelCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<CargoShipPayWithUnityIfOutOfFuelCmd>(this, "CargoShipId", (object) EntityId.Deserialize(reader));
      reader.SetField<CargoShipPayWithUnityIfOutOfFuelCmd>(this, "PayWithUnity", (object) reader.ReadBool());
    }

    static CargoShipPayWithUnityIfOutOfFuelCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      CargoShipPayWithUnityIfOutOfFuelCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      CargoShipPayWithUnityIfOutOfFuelCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
