// Decompiled with JetBrains decompiler
// Type: Mafi.Core.PathFinding.VehicleRoadPathSegment
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Roads;
using Mafi.Core.Terrain;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.PathFinding
{
  [GenerateSerializer(false, null, 0)]
  public class VehicleRoadPathSegment : IVehiclePathSegment
  {
    public ImmutableArray<RoadPathSegment> Path;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public Option<IVehiclePathSegment> NextSegment { get; set; }

    public RectangleTerrainArea2i ComputeBoundingBox()
    {
      if (this.Path.IsEmpty)
        return new RectangleTerrainArea2i();
      Tile2i p1 = this.Path.First.Entity.CenterTile.Xy;
      Tile2i p2 = p1;
      foreach (RoadPathSegment roadPathSegment in this.Path)
      {
        Tile2i xy = roadPathSegment.Entity.CenterTile.Xy;
        p1 = p1.Min(xy);
        p2 = p2.Max(xy);
      }
      return RectangleTerrainArea2i.FromTwoPositions(p1, p2);
    }

    public static void Serialize(VehicleRoadPathSegment value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<VehicleRoadPathSegment>(value))
        return;
      writer.EnqueueDataSerialization((object) value, VehicleRoadPathSegment.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Option<IVehiclePathSegment>.Serialize(this.NextSegment, writer);
      ImmutableArray<RoadPathSegment>.Serialize(this.Path, writer);
    }

    public static VehicleRoadPathSegment Deserialize(BlobReader reader)
    {
      VehicleRoadPathSegment vehicleRoadPathSegment;
      if (reader.TryStartClassDeserialization<VehicleRoadPathSegment>(out vehicleRoadPathSegment))
        reader.EnqueueDataDeserialization((object) vehicleRoadPathSegment, VehicleRoadPathSegment.s_deserializeDataDelayedAction);
      return vehicleRoadPathSegment;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.NextSegment = Option<IVehiclePathSegment>.Deserialize(reader);
      this.Path = ImmutableArray<RoadPathSegment>.Deserialize(reader);
    }

    public VehicleRoadPathSegment()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static VehicleRoadPathSegment()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      VehicleRoadPathSegment.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VehicleRoadPathSegment) obj).SerializeData(writer));
      VehicleRoadPathSegment.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VehicleRoadPathSegment) obj).DeserializeData(reader));
    }
  }
}
