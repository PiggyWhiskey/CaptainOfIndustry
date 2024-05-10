// Decompiled with JetBrains decompiler
// Type: Mafi.Random.Noise.PolygonDistanceNoise
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Numerics;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Random.Noise
{
  [GenerateSerializer(false, null, 0)]
  public class PolygonDistanceNoise : INoise2D
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly Polygon2f m_polygon;
    [DoNotSave(0, null)]
    private readonly Polygon2fFast m_polygonFastCache;

    public static void Serialize(PolygonDistanceNoise value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<PolygonDistanceNoise>(value))
        return;
      writer.EnqueueDataSerialization((object) value, PolygonDistanceNoise.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Polygon2f.Serialize(this.m_polygon, writer);
      Fix32.Serialize(this.Period, writer);
    }

    public static PolygonDistanceNoise Deserialize(BlobReader reader)
    {
      PolygonDistanceNoise polygonDistanceNoise;
      if (reader.TryStartClassDeserialization<PolygonDistanceNoise>(out polygonDistanceNoise))
        reader.EnqueueDataDeserialization((object) polygonDistanceNoise, PolygonDistanceNoise.s_deserializeDataDelayedAction);
      return polygonDistanceNoise;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<PolygonDistanceNoise>(this, "m_polygon", (object) Polygon2f.Deserialize(reader));
      this.Period = Fix32.Deserialize(reader);
    }

    public Fix32 MeanValue => Fix32.Zero;

    public Fix32 Amplitude => Fix32.One;

    public Fix32 Period { get; private set; }

    public PolygonDistanceNoise(Polygon2f polygon)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_polygon = polygon;
      this.m_polygonFastCache = polygon.MakeFast2f();
      this.Period = this.m_polygonFastCache.BoundingBoxMax.DistanceTo(this.m_polygonFastCache.BoundingBoxMin);
    }

    private void initAfterLoad()
    {
      ReflectionUtils.SetField<PolygonDistanceNoise>(this, "m_polygonFastCache", (object) this.m_polygon.MakeFast2f());
    }

    public Fix64 GetValue(Vector2f point) => this.m_polygonFastCache.DistanceSqrTo(point).Sqrt();

    public INoise2D ReseedClone(IRandom random) => (INoise2D) this;

    static PolygonDistanceNoise()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      PolygonDistanceNoise.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((PolygonDistanceNoise) obj).SerializeData(writer));
      PolygonDistanceNoise.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((PolygonDistanceNoise) obj).DeserializeData(reader));
    }
  }
}
