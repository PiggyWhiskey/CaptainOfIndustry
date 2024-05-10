// Decompiled with JetBrains decompiler
// Type: Mafi.Random.Noise.Noise2dTransformMax
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
  public class Noise2dTransformMax : INoise2D
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly INoise2D m_baseNoise;
    public readonly Fix64 MinValue;

    public static void Serialize(Noise2dTransformMax value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<Noise2dTransformMax>(value))
        return;
      writer.EnqueueDataSerialization((object) value, Noise2dTransformMax.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteGeneric<INoise2D>(this.m_baseNoise);
      Fix64.Serialize(this.MinValue, writer);
    }

    public static Noise2dTransformMax Deserialize(BlobReader reader)
    {
      Noise2dTransformMax noise2dTransformMax;
      if (reader.TryStartClassDeserialization<Noise2dTransformMax>(out noise2dTransformMax))
        reader.EnqueueDataDeserialization((object) noise2dTransformMax, Noise2dTransformMax.s_deserializeDataDelayedAction);
      return noise2dTransformMax;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<Noise2dTransformMax>(this, "m_baseNoise", (object) reader.ReadGenericAs<INoise2D>());
      reader.SetField<Noise2dTransformMax>(this, "MinValue", (object) Fix64.Deserialize(reader));
    }

    public Noise2dTransformMax(INoise2D baseNoise, Fix64 minValue)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.MinValue = minValue;
      this.m_baseNoise = baseNoise.CheckNotNull<INoise2D>();
    }

    public Fix32 MeanValue => this.m_baseNoise.MeanValue;

    public Fix32 Amplitude => this.m_baseNoise.Amplitude;

    public Fix32 Period => this.m_baseNoise.Period;

    public Fix64 GetValue(Vector2f point) => this.m_baseNoise.GetValue(point).Max(this.MinValue);

    public INoise2D ReseedClone(IRandom random) => (INoise2D) this;

    static Noise2dTransformMax()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      Noise2dTransformMax.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Noise2dTransformMax) obj).SerializeData(writer));
      Noise2dTransformMax.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Noise2dTransformMax) obj).DeserializeData(reader));
    }
  }
}
