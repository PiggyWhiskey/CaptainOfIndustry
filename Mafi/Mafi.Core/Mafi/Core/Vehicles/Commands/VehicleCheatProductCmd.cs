// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Commands.VehicleCheatProductCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Input;
using Mafi.Core.Products;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Vehicles.Commands
{
  [GenerateSerializer(false, null, 0)]
  public class VehicleCheatProductCmd : InputCommand
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly EntityId VehicleId;
    public readonly ProductProto.ID ProductId;

    public static void Serialize(VehicleCheatProductCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<VehicleCheatProductCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, VehicleCheatProductCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      ProductProto.ID.Serialize(this.ProductId, writer);
      EntityId.Serialize(this.VehicleId, writer);
    }

    public static VehicleCheatProductCmd Deserialize(BlobReader reader)
    {
      VehicleCheatProductCmd vehicleCheatProductCmd;
      if (reader.TryStartClassDeserialization<VehicleCheatProductCmd>(out vehicleCheatProductCmd))
        reader.EnqueueDataDeserialization((object) vehicleCheatProductCmd, VehicleCheatProductCmd.s_deserializeDataDelayedAction);
      return vehicleCheatProductCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<VehicleCheatProductCmd>(this, "ProductId", (object) ProductProto.ID.Deserialize(reader));
      reader.SetField<VehicleCheatProductCmd>(this, "VehicleId", (object) EntityId.Deserialize(reader));
    }

    public VehicleCheatProductCmd(Vehicle vehicle, ProductProto product)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(vehicle.Id, new ProductProto.ID(product.Id.Value));
    }

    public VehicleCheatProductCmd(EntityId storageId, ProductProto.ID productId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.VehicleId = storageId;
      this.ProductId = productId;
    }

    static VehicleCheatProductCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      VehicleCheatProductCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      VehicleCheatProductCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
