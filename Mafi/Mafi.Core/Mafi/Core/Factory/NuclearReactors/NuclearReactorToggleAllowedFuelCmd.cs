// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.NuclearReactors.NuclearReactorToggleAllowedFuelCmd
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
namespace Mafi.Core.Factory.NuclearReactors
{
  [GenerateSerializer(false, null, 0)]
  public class NuclearReactorToggleAllowedFuelCmd : InputCommand
  {
    public readonly EntityId ReactorId;
    public readonly ProductProto.ID FuelProtoId;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public NuclearReactorToggleAllowedFuelCmd(NuclearReactor reactor, ProductProto.ID fuelProtoId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(reactor.Id, fuelProtoId);
    }

    public NuclearReactorToggleAllowedFuelCmd(EntityId reactorId, ProductProto.ID fuelProtoId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ReactorId = reactorId;
      this.FuelProtoId = fuelProtoId;
    }

    public static void Serialize(NuclearReactorToggleAllowedFuelCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<NuclearReactorToggleAllowedFuelCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, NuclearReactorToggleAllowedFuelCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      ProductProto.ID.Serialize(this.FuelProtoId, writer);
      EntityId.Serialize(this.ReactorId, writer);
    }

    public static NuclearReactorToggleAllowedFuelCmd Deserialize(BlobReader reader)
    {
      NuclearReactorToggleAllowedFuelCmd toggleAllowedFuelCmd;
      if (reader.TryStartClassDeserialization<NuclearReactorToggleAllowedFuelCmd>(out toggleAllowedFuelCmd))
        reader.EnqueueDataDeserialization((object) toggleAllowedFuelCmd, NuclearReactorToggleAllowedFuelCmd.s_deserializeDataDelayedAction);
      return toggleAllowedFuelCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<NuclearReactorToggleAllowedFuelCmd>(this, "FuelProtoId", (object) ProductProto.ID.Deserialize(reader));
      reader.SetField<NuclearReactorToggleAllowedFuelCmd>(this, "ReactorId", (object) EntityId.Deserialize(reader));
    }

    static NuclearReactorToggleAllowedFuelCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      NuclearReactorToggleAllowedFuelCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      NuclearReactorToggleAllowedFuelCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
