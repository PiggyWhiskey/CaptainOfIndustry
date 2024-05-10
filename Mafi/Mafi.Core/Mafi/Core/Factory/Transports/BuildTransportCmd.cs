// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Transports.BuildTransportCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Static;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Factory.Transports
{
  /// <summary>
  /// Creates a new transport from given pivots positions and start/end directions. First and last pivot position may
  /// be occupied with already existing pivot that has the same proto and is not fully connected.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public class BuildTransportCmd : InputCommand<EntityId>
  {
    public readonly StaticEntityProto.ID ProtoId;
    public readonly ImmutableArray<Tile3i> PivotPositions;
    public readonly ImmutableArray<Tile2i> PillarHints;
    public readonly Direction903d? StartDirection;
    public readonly Direction903d? EndDirection;
    [NewInSaveVersion(124, null, null, null, null)]
    public readonly bool DisablePortSnapping;
    public readonly bool IsFree;
    public readonly bool AllowDirectConnection;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    /// <summary>
    /// Build pivot at all given positions and connects them all with segments.
    /// </summary>
    public BuildTransportCmd(
      StaticEntityProto.ID protoId,
      ImmutableArray<Tile3i> pivotPositions,
      ImmutableArray<Tile2i> pillarHints,
      Direction903d? startDirection,
      Direction903d? endDirection,
      bool disablePortSnapping,
      bool isFree,
      bool allowDirectConnection = false)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ProtoId = protoId;
      this.PivotPositions = pivotPositions.CheckNotEmpty<Tile3i>();
      this.StartDirection = startDirection;
      this.EndDirection = endDirection;
      this.DisablePortSnapping = disablePortSnapping;
      this.IsFree = isFree;
      this.PillarHints = pillarHints;
      this.AllowDirectConnection = allowDirectConnection;
    }

    public static void Serialize(BuildTransportCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<BuildTransportCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, BuildTransportCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteBool(this.AllowDirectConnection);
      writer.WriteBool(this.DisablePortSnapping);
      writer.WriteNullableStruct<Direction903d>(this.EndDirection);
      writer.WriteBool(this.IsFree);
      ImmutableArray<Tile2i>.Serialize(this.PillarHints, writer);
      ImmutableArray<Tile3i>.Serialize(this.PivotPositions, writer);
      StaticEntityProto.ID.Serialize(this.ProtoId, writer);
      writer.WriteNullableStruct<Direction903d>(this.StartDirection);
    }

    public static BuildTransportCmd Deserialize(BlobReader reader)
    {
      BuildTransportCmd buildTransportCmd;
      if (reader.TryStartClassDeserialization<BuildTransportCmd>(out buildTransportCmd))
        reader.EnqueueDataDeserialization((object) buildTransportCmd, BuildTransportCmd.s_deserializeDataDelayedAction);
      return buildTransportCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<BuildTransportCmd>(this, "AllowDirectConnection", (object) reader.ReadBool());
      reader.SetField<BuildTransportCmd>(this, "DisablePortSnapping", (object) (bool) (reader.LoadedSaveVersion >= 124 ? (reader.ReadBool() ? 1 : 0) : 0));
      reader.SetField<BuildTransportCmd>(this, "EndDirection", (object) reader.ReadNullableStruct<Direction903d>());
      reader.SetField<BuildTransportCmd>(this, "IsFree", (object) reader.ReadBool());
      reader.SetField<BuildTransportCmd>(this, "PillarHints", (object) ImmutableArray<Tile2i>.Deserialize(reader));
      reader.SetField<BuildTransportCmd>(this, "PivotPositions", (object) ImmutableArray<Tile3i>.Deserialize(reader));
      reader.SetField<BuildTransportCmd>(this, "ProtoId", (object) StaticEntityProto.ID.Deserialize(reader));
      reader.SetField<BuildTransportCmd>(this, "StartDirection", (object) reader.ReadNullableStruct<Direction903d>());
    }

    static BuildTransportCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      BuildTransportCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<EntityId>) obj).SerializeData(writer));
      BuildTransportCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<EntityId>) obj).DeserializeData(reader));
    }
  }
}
