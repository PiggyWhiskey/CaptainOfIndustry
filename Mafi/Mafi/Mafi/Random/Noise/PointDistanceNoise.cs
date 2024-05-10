// Decompiled with JetBrains decompiler
// Type: Mafi.Random.Noise.PointDistanceNoise
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Random.Noise
{
  [GenerateSerializer(false, null, 0)]
  public class PointDistanceNoise : INoise2D
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly Vector2f m_point;

    public static void Serialize(PointDistanceNoise value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<PointDistanceNoise>(value))
        return;
      writer.EnqueueDataSerialization((object) value, PointDistanceNoise.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Vector2f.Serialize(this.m_point, writer);
    }

    public static PointDistanceNoise Deserialize(BlobReader reader)
    {
      PointDistanceNoise pointDistanceNoise;
      if (reader.TryStartClassDeserialization<PointDistanceNoise>(out pointDistanceNoise))
        reader.EnqueueDataDeserialization((object) pointDistanceNoise, PointDistanceNoise.s_deserializeDataDelayedAction);
      return pointDistanceNoise;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<PointDistanceNoise>(this, "m_point", (object) Vector2f.Deserialize(reader));
    }

    public Fix32 MeanValue => Fix32.Zero;

    public Fix32 Amplitude => Fix32.One;

    public Fix32 Period => Fix32.One;

    public PointDistanceNoise(Vector2f point)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_point = point;
    }

    public Fix64 GetValue(Vector2f point) => this.m_point.DistanceSqrTo(point).Sqrt();

    public INoise2D ReseedClone(IRandom random) => (INoise2D) this;

    static PointDistanceNoise()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      PointDistanceNoise.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((PointDistanceNoise) obj).SerializeData(writer));
      PointDistanceNoise.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((PointDistanceNoise) obj).DeserializeData(reader));
    }
  }
}
