// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Cargo.Modules.CargoDepotAssignContractCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Core.Prototypes;
using Mafi.Core.World.Contracts;
using Mafi.Serialization;
using System;

#nullable enable
namespace Mafi.Core.Buildings.Cargo.Modules
{
  [GenerateSerializer(false, null, 0)]
  public class CargoDepotAssignContractCmd : InputCommand
  {
    public readonly EntityId CargoDepotId;
    public readonly Proto.ID? ContractId;
    private static readonly 
    #nullable disable
    Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public CargoDepotAssignContractCmd(CargoDepot depot, 
    #nullable enable
    ContractProto? contract)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(depot.Id, contract?.Id);
    }

    private CargoDepotAssignContractCmd(EntityId depotId, Proto.ID? contractId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.CargoDepotId = depotId;
      this.ContractId = contractId;
    }

    public static void Serialize(
    #nullable disable
    CargoDepotAssignContractCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<CargoDepotAssignContractCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, CargoDepotAssignContractCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.CargoDepotId, writer);
      writer.WriteNullableStruct<Proto.ID>(this.ContractId);
    }

    public static CargoDepotAssignContractCmd Deserialize(BlobReader reader)
    {
      CargoDepotAssignContractCmd assignContractCmd;
      if (reader.TryStartClassDeserialization<CargoDepotAssignContractCmd>(out assignContractCmd))
        reader.EnqueueDataDeserialization((object) assignContractCmd, CargoDepotAssignContractCmd.s_deserializeDataDelayedAction);
      return assignContractCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<CargoDepotAssignContractCmd>(this, "CargoDepotId", (object) EntityId.Deserialize(reader));
      reader.SetField<CargoDepotAssignContractCmd>(this, "ContractId", (object) reader.ReadNullableStruct<Proto.ID>());
    }

    static CargoDepotAssignContractCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      CargoDepotAssignContractCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      CargoDepotAssignContractCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
