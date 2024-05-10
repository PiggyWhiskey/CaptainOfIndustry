// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Cargo.Modules.CargoDepotModuleSetProductCmd
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
  public class CargoDepotModuleSetProductCmd : InputCommand
  {
    public readonly EntityId ModuleId;
    public readonly ProductProto.ID ProductId;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public CargoDepotModuleSetProductCmd(CargoDepotModule module, ProductProto product)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(module.Id, product.Id);
    }

    private CargoDepotModuleSetProductCmd(EntityId moduleId, ProductProto.ID productId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ModuleId = moduleId;
      this.ProductId = productId;
    }

    public static void Serialize(CargoDepotModuleSetProductCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<CargoDepotModuleSetProductCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, CargoDepotModuleSetProductCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.ModuleId, writer);
      ProductProto.ID.Serialize(this.ProductId, writer);
    }

    public static CargoDepotModuleSetProductCmd Deserialize(BlobReader reader)
    {
      CargoDepotModuleSetProductCmd moduleSetProductCmd;
      if (reader.TryStartClassDeserialization<CargoDepotModuleSetProductCmd>(out moduleSetProductCmd))
        reader.EnqueueDataDeserialization((object) moduleSetProductCmd, CargoDepotModuleSetProductCmd.s_deserializeDataDelayedAction);
      return moduleSetProductCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<CargoDepotModuleSetProductCmd>(this, "ModuleId", (object) EntityId.Deserialize(reader));
      reader.SetField<CargoDepotModuleSetProductCmd>(this, "ProductId", (object) ProductProto.ID.Deserialize(reader));
    }

    static CargoDepotModuleSetProductCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      CargoDepotModuleSetProductCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      CargoDepotModuleSetProductCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
