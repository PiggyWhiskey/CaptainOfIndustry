// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Transports.DeconstructTransportSegmentCmd
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
  public class DeconstructTransportSegmentCmd : InputCommand
  {
    public readonly EntityId TransportId;
    public readonly Tile3i StartPosition;
    public readonly Tile3i EndPosition;
    [NewInSaveVersion(121, null, null, null, null)]
    public readonly bool QuickRemove;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public DeconstructTransportSegmentCmd(
      Transport transport,
      Tile3i startPosition,
      Tile3i endPosition,
      bool quickRemove = false)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.StartPosition = startPosition;
      this.EndPosition = endPosition;
      this.TransportId = transport.Id;
      this.QuickRemove = quickRemove;
    }

    public DeconstructTransportSegmentCmd(
      EntityId transportId,
      Tile3i startPosition,
      Tile3i endPosition,
      bool quickRemove = false)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.TransportId = transportId;
      this.StartPosition = startPosition;
      this.EndPosition = endPosition;
      this.QuickRemove = quickRemove;
    }

    public static void Serialize(DeconstructTransportSegmentCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<DeconstructTransportSegmentCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, DeconstructTransportSegmentCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      Tile3i.Serialize(this.EndPosition, writer);
      writer.WriteBool(this.QuickRemove);
      Tile3i.Serialize(this.StartPosition, writer);
      EntityId.Serialize(this.TransportId, writer);
    }

    public static DeconstructTransportSegmentCmd Deserialize(BlobReader reader)
    {
      DeconstructTransportSegmentCmd transportSegmentCmd;
      if (reader.TryStartClassDeserialization<DeconstructTransportSegmentCmd>(out transportSegmentCmd))
        reader.EnqueueDataDeserialization((object) transportSegmentCmd, DeconstructTransportSegmentCmd.s_deserializeDataDelayedAction);
      return transportSegmentCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<DeconstructTransportSegmentCmd>(this, "EndPosition", (object) Tile3i.Deserialize(reader));
      reader.SetField<DeconstructTransportSegmentCmd>(this, "QuickRemove", (object) (bool) (reader.LoadedSaveVersion >= 121 ? (reader.ReadBool() ? 1 : 0) : 0));
      reader.SetField<DeconstructTransportSegmentCmd>(this, "StartPosition", (object) Tile3i.Deserialize(reader));
      reader.SetField<DeconstructTransportSegmentCmd>(this, "TransportId", (object) EntityId.Deserialize(reader));
    }

    static DeconstructTransportSegmentCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      DeconstructTransportSegmentCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      DeconstructTransportSegmentCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
