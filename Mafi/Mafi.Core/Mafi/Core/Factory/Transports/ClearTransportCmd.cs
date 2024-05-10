// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Transports.ClearTransportCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Factory.Transports
{
  [GenerateSerializer(false, null, 0)]
  public class ClearTransportCmd : InputCommand<EntityId>
  {
    public readonly EntityId TransportId;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public ClearTransportCmd(Transport transport)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.TransportId = transport.Id;
    }

    public ClearTransportCmd(EntityId transportId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.TransportId = transportId;
    }

    public static void Serialize(ClearTransportCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ClearTransportCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ClearTransportCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.TransportId, writer);
    }

    public static ClearTransportCmd Deserialize(BlobReader reader)
    {
      ClearTransportCmd clearTransportCmd;
      if (reader.TryStartClassDeserialization<ClearTransportCmd>(out clearTransportCmd))
        reader.EnqueueDataDeserialization((object) clearTransportCmd, ClearTransportCmd.s_deserializeDataDelayedAction);
      return clearTransportCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<ClearTransportCmd>(this, "TransportId", (object) EntityId.Deserialize(reader));
    }

    static ClearTransportCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ClearTransportCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<EntityId>) obj).SerializeData(writer));
      ClearTransportCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<EntityId>) obj).DeserializeData(reader));
    }
  }
}
