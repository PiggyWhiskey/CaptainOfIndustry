// Decompiled with JetBrains decompiler
// Type: Mafi.Random.Noise.Noise2dTransformMult
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
  public class Noise2dTransformMult : INoise2D
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly INoise2D m_baseNoise;
    public readonly Fix64 Multiplier;

    public static void Serialize(Noise2dTransformMult value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<Noise2dTransformMult>(value))
        return;
      writer.EnqueueDataSerialization((object) value, Noise2dTransformMult.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteGeneric<INoise2D>(this.m_baseNoise);
      Fix64.Serialize(this.Multiplier, writer);
    }

    public static Noise2dTransformMult Deserialize(BlobReader reader)
    {
      Noise2dTransformMult noise2dTransformMult;
      if (reader.TryStartClassDeserialization<Noise2dTransformMult>(out noise2dTransformMult))
        reader.EnqueueDataDeserialization((object) noise2dTransformMult, Noise2dTransformMult.s_deserializeDataDelayedAction);
      return noise2dTransformMult;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<Noise2dTransformMult>(this, "m_baseNoise", (object) reader.ReadGenericAs<INoise2D>());
      reader.SetField<Noise2dTransformMult>(this, "Multiplier", (object) Fix64.Deserialize(reader));
    }

    public Noise2dTransformMult(INoise2D baseNoise, Fix64 multiplier)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Multiplier = multiplier;
      this.m_baseNoise = baseNoise.CheckNotNull<INoise2D>();
    }

    public Fix32 MeanValue => this.m_baseNoise.MeanValue;

    public Fix32 Amplitude => (this.m_baseNoise.Amplitude * this.Multiplier).ToFix32();

    public Fix32 Period => this.m_baseNoise.Period;

    public Fix64 GetValue(Vector2f point) => this.Multiplier * this.m_baseNoise.GetValue(point);

    public INoise2D ReseedClone(IRandom random) => (INoise2D) this;

    static Noise2dTransformMult()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      Noise2dTransformMult.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Noise2dTransformMult) obj).SerializeData(writer));
      Noise2dTransformMult.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Noise2dTransformMult) obj).DeserializeData(reader));
    }
  }
}
