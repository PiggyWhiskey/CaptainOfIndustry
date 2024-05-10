// Decompiled with JetBrains decompiler
// Type: Mafi.Random.Noise.PolygonSignedDistanceNoise
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
  public class PolygonSignedDistanceNoise : INoise2D
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly Polygon2f m_polygon;
    [DoNotSave(0, null)]
    private readonly Polygon2fFast m_polygonFastCache;

    public static void Serialize(PolygonSignedDistanceNoise value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<PolygonSignedDistanceNoise>(value))
        return;
      writer.EnqueueDataSerialization((object) value, PolygonSignedDistanceNoise.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Polygon2f.Serialize(this.m_polygon, writer);
      Fix32.Serialize(this.Period, writer);
    }

    public static PolygonSignedDistanceNoise Deserialize(BlobReader reader)
    {
      PolygonSignedDistanceNoise signedDistanceNoise;
      if (reader.TryStartClassDeserialization<PolygonSignedDistanceNoise>(out signedDistanceNoise))
        reader.EnqueueDataDeserialization((object) signedDistanceNoise, PolygonSignedDistanceNoise.s_deserializeDataDelayedAction);
      return signedDistanceNoise;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<PolygonSignedDistanceNoise>(this, "m_polygon", (object) Polygon2f.Deserialize(reader));
      this.Period = Fix32.Deserialize(reader);
      reader.RegisterInitAfterLoad<PolygonSignedDistanceNoise>(this, "initAfterLoad", InitPriority.Normal);
    }

    public Fix32 MeanValue => Fix32.Zero;

    public Fix32 Amplitude => Fix32.One;

    public Fix32 Period { get; private set; }

    public PolygonSignedDistanceNoise(Polygon2f polygon)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_polygon = polygon;
      this.m_polygonFastCache = polygon.MakeFast2f();
      this.Period = this.m_polygonFastCache.BoundingBoxMax.DistanceTo(this.m_polygonFastCache.BoundingBoxMin);
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void initAfterLoad()
    {
      ReflectionUtils.SetField<PolygonSignedDistanceNoise>(this, "m_polygonFastCache", (object) this.m_polygon.MakeFast2f());
    }

    public Fix64 GetValue(Vector2f point)
    {
      Fix64 fix64 = this.m_polygonFastCache.DistanceSqrTo(point).Sqrt();
      return !this.m_polygonFastCache.Contains(point) ? fix64 : -fix64;
    }

    public INoise2D ReseedClone(IRandom random) => (INoise2D) this;

    static PolygonSignedDistanceNoise()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      PolygonSignedDistanceNoise.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((PolygonSignedDistanceNoise) obj).SerializeData(writer));
      PolygonSignedDistanceNoise.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((PolygonSignedDistanceNoise) obj).DeserializeData(reader));
    }
  }
}
