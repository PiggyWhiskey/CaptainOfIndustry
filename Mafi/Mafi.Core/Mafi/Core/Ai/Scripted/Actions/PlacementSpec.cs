// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Ai.Scripted.Actions.PlacementSpec
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Ai.Scripted.Actions
{
  [GenerateSerializer(false, null, 0)]
  public class PlacementSpec
  {
    public readonly bool SecondRow;
    public readonly int? CustomSpacing;
    public readonly bool AltLane;
    public readonly Rotation90 Rotation;
    public readonly Option<string> RelativeTo;
    public readonly Direction90 RelativeToDirection;
    public readonly RelTile2i RelativeOffset;
    public readonly Tile2i? CustomPosition;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public PlacementSpec(
      bool secondRow = false,
      int? customSpacing = null,
      bool altLane = false,
      Rotation90 rotation = default (Rotation90),
      string relativeTo = null,
      Direction90 relativeToDirection = default (Direction90),
      RelTile2i relativeOffset = default (RelTile2i),
      Tile2i? customPosition = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.SecondRow = secondRow;
      this.CustomSpacing = customSpacing;
      this.AltLane = altLane;
      this.Rotation = rotation;
      this.RelativeTo = (Option<string>) relativeTo;
      this.RelativeToDirection = relativeToDirection;
      this.RelativeOffset = relativeOffset;
      this.CustomPosition = customPosition;
    }

    public static void Serialize(PlacementSpec value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<PlacementSpec>(value))
        return;
      writer.EnqueueDataSerialization((object) value, PlacementSpec.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteBool(this.AltLane);
      writer.WriteNullableStruct<Tile2i>(this.CustomPosition);
      writer.WriteNullableStruct<int>(this.CustomSpacing);
      RelTile2i.Serialize(this.RelativeOffset, writer);
      Option<string>.Serialize(this.RelativeTo, writer);
      Direction90.Serialize(this.RelativeToDirection, writer);
      Rotation90.Serialize(this.Rotation, writer);
      writer.WriteBool(this.SecondRow);
    }

    public static PlacementSpec Deserialize(BlobReader reader)
    {
      PlacementSpec placementSpec;
      if (reader.TryStartClassDeserialization<PlacementSpec>(out placementSpec))
        reader.EnqueueDataDeserialization((object) placementSpec, PlacementSpec.s_deserializeDataDelayedAction);
      return placementSpec;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<PlacementSpec>(this, "AltLane", (object) reader.ReadBool());
      reader.SetField<PlacementSpec>(this, "CustomPosition", (object) reader.ReadNullableStruct<Tile2i>());
      reader.SetField<PlacementSpec>(this, "CustomSpacing", (object) reader.ReadNullableStruct<int>());
      reader.SetField<PlacementSpec>(this, "RelativeOffset", (object) RelTile2i.Deserialize(reader));
      reader.SetField<PlacementSpec>(this, "RelativeTo", (object) Option<string>.Deserialize(reader));
      reader.SetField<PlacementSpec>(this, "RelativeToDirection", (object) Direction90.Deserialize(reader));
      reader.SetField<PlacementSpec>(this, "Rotation", (object) Rotation90.Deserialize(reader));
      reader.SetField<PlacementSpec>(this, "SecondRow", (object) reader.ReadBool());
    }

    static PlacementSpec()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      PlacementSpec.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((PlacementSpec) obj).SerializeData(writer));
      PlacementSpec.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((PlacementSpec) obj).DeserializeData(reader));
    }
  }
}
