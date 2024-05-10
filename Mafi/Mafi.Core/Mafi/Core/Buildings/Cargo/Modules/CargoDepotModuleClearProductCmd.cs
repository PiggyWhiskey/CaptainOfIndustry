// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Cargo.Modules.CargoDepotModuleClearProductCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Cargo.Modules
{
  [GenerateSerializer(false, null, 0)]
  public class CargoDepotModuleClearProductCmd : InputCommand
  {
    public readonly EntityId ModuleId;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public CargoDepotModuleClearProductCmd(CargoDepotModule module)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(module.Id);
    }

    private CargoDepotModuleClearProductCmd(EntityId moduleId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ModuleId = moduleId;
    }

    public static void Serialize(CargoDepotModuleClearProductCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<CargoDepotModuleClearProductCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, CargoDepotModuleClearProductCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.ModuleId, writer);
    }

    public static CargoDepotModuleClearProductCmd Deserialize(BlobReader reader)
    {
      CargoDepotModuleClearProductCmd moduleClearProductCmd;
      if (reader.TryStartClassDeserialization<CargoDepotModuleClearProductCmd>(out moduleClearProductCmd))
        reader.EnqueueDataDeserialization((object) moduleClearProductCmd, CargoDepotModuleClearProductCmd.s_deserializeDataDelayedAction);
      return moduleClearProductCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<CargoDepotModuleClearProductCmd>(this, "ModuleId", (object) EntityId.Deserialize(reader));
    }

    static CargoDepotModuleClearProductCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      CargoDepotModuleClearProductCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      CargoDepotModuleClearProductCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
