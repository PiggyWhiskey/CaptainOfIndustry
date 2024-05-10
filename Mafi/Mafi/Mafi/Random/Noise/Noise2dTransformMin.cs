// Decompiled with JetBrains decompiler
// Type: Mafi.Random.Noise.Noise2dTransformMin
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
  public class Noise2dTransformMin : INoise2D
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly INoise2D m_baseNoise;
    public readonly Fix64 MaxValue;

    public static void Serialize(Noise2dTransformMin value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<Noise2dTransformMin>(value))
        return;
      writer.EnqueueDataSerialization((object) value, Noise2dTransformMin.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteGeneric<INoise2D>(this.m_baseNoise);
      Fix64.Serialize(this.MaxValue, writer);
    }

    public static Noise2dTransformMin Deserialize(BlobReader reader)
    {
      Noise2dTransformMin noise2dTransformMin;
      if (reader.TryStartClassDeserialization<Noise2dTransformMin>(out noise2dTransformMin))
        reader.EnqueueDataDeserialization((object) noise2dTransformMin, Noise2dTransformMin.s_deserializeDataDelayedAction);
      return noise2dTransformMin;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<Noise2dTransformMin>(this, "m_baseNoise", (object) reader.ReadGenericAs<INoise2D>());
      reader.SetField<Noise2dTransformMin>(this, "MaxValue", (object) Fix64.Deserialize(reader));
    }

    public Noise2dTransformMin(INoise2D baseNoise, Fix64 maxValue)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.MaxValue = maxValue;
      this.m_baseNoise = baseNoise.CheckNotNull<INoise2D>();
    }

    public Fix32 MeanValue => this.m_baseNoise.MeanValue;

    public Fix32 Amplitude => this.m_baseNoise.Amplitude;

    public Fix32 Period => this.m_baseNoise.Period;

    public Fix64 GetValue(Vector2f point) => this.m_baseNoise.GetValue(point).Min(this.MaxValue);

    public INoise2D ReseedClone(IRandom random) => (INoise2D) this;

    static Noise2dTransformMin()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      Noise2dTransformMin.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Noise2dTransformMin) obj).SerializeData(writer));
      Noise2dTransformMin.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Noise2dTransformMin) obj).DeserializeData(reader));
    }
  }
}
