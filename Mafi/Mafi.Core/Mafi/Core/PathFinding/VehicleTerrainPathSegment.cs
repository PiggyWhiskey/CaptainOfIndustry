// Decompiled with JetBrains decompiler
// Type: Mafi.Core.PathFinding.VehicleTerrainPathSegment
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Terrain;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.PathFinding
{
  [GenerateSerializer(false, null, 0)]
  public class VehicleTerrainPathSegment : IVehiclePathSegment
  {
    /// <summary>
    /// Path stored in reverse, first tile of the path is the last.
    /// This makes for efficient reconstruction and popping from end during driving.
    /// </summary>
    public LystStruct<Tile2i> PathRawReversed;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public Option<IVehiclePathSegment> NextSegment { get; set; }

    public RectangleTerrainArea2i ComputeBoundingBox()
    {
      if (this.PathRawReversed.IsEmpty)
        return new RectangleTerrainArea2i();
      Tile2i p1 = this.PathRawReversed.First;
      Tile2i p2 = p1;
      foreach (Tile2i rhs in this.PathRawReversed)
      {
        p1 = p1.Min(rhs);
        p2 = p2.Max(rhs);
      }
      return RectangleTerrainArea2i.FromTwoPositions(p1, p2);
    }

    public static void Serialize(VehicleTerrainPathSegment value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<VehicleTerrainPathSegment>(value))
        return;
      writer.EnqueueDataSerialization((object) value, VehicleTerrainPathSegment.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Option<IVehiclePathSegment>.Serialize(this.NextSegment, writer);
      LystStruct<Tile2i>.Serialize(this.PathRawReversed, writer);
    }

    public static VehicleTerrainPathSegment Deserialize(BlobReader reader)
    {
      VehicleTerrainPathSegment terrainPathSegment;
      if (reader.TryStartClassDeserialization<VehicleTerrainPathSegment>(out terrainPathSegment))
        reader.EnqueueDataDeserialization((object) terrainPathSegment, VehicleTerrainPathSegment.s_deserializeDataDelayedAction);
      return terrainPathSegment;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.NextSegment = Option<IVehiclePathSegment>.Deserialize(reader);
      this.PathRawReversed = LystStruct<Tile2i>.Deserialize(reader);
    }

    public VehicleTerrainPathSegment()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static VehicleTerrainPathSegment()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      VehicleTerrainPathSegment.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VehicleTerrainPathSegment) obj).SerializeData(writer));
      VehicleTerrainPathSegment.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VehicleTerrainPathSegment) obj).DeserializeData(reader));
    }
  }
}
