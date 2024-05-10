// Decompiled with JetBrains decompiler
// Type: Mafi.Random.Noise.Noise2dTransform
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
  public class Noise2dTransform : INoise2D
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly INoise2D m_baseNoise;
    public readonly Fix64 Multiplier;
    public readonly Fix64 Addend;
    public readonly Fix32 FrequencyMult;

    public static void Serialize(Noise2dTransform value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<Noise2dTransform>(value))
        return;
      writer.EnqueueDataSerialization((object) value, Noise2dTransform.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Fix64.Serialize(this.Addend, writer);
      Fix32.Serialize(this.FrequencyMult, writer);
      writer.WriteGeneric<INoise2D>(this.m_baseNoise);
      Fix64.Serialize(this.Multiplier, writer);
    }

    public static Noise2dTransform Deserialize(BlobReader reader)
    {
      Noise2dTransform noise2dTransform;
      if (reader.TryStartClassDeserialization<Noise2dTransform>(out noise2dTransform))
        reader.EnqueueDataDeserialization((object) noise2dTransform, Noise2dTransform.s_deserializeDataDelayedAction);
      return noise2dTransform;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<Noise2dTransform>(this, "Addend", (object) Fix64.Deserialize(reader));
      reader.SetField<Noise2dTransform>(this, "FrequencyMult", (object) Fix32.Deserialize(reader));
      reader.SetField<Noise2dTransform>(this, "m_baseNoise", (object) reader.ReadGenericAs<INoise2D>());
      reader.SetField<Noise2dTransform>(this, "Multiplier", (object) Fix64.Deserialize(reader));
    }

    public Noise2dTransform(
      INoise2D baseNoise,
      Fix64 multiplier,
      Fix64 addend,
      Fix32 frequencyMult)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Multiplier = multiplier;
      this.Addend = addend;
      this.FrequencyMult = frequencyMult;
      this.m_baseNoise = baseNoise.CheckNotNull<INoise2D>();
    }

    public Fix32 MeanValue => (this.m_baseNoise.MeanValue + this.Addend).ToFix32();

    public Fix32 Amplitude => (this.m_baseNoise.Amplitude * this.Multiplier).ToFix32();

    public Fix32 Period => this.m_baseNoise.Period / this.FrequencyMult;

    public Fix64 GetValue(Vector2f point)
    {
      return this.Addend + this.Multiplier * this.m_baseNoise.GetValue(point * this.FrequencyMult);
    }

    public INoise2D ReseedClone(IRandom random) => (INoise2D) this;

    static Noise2dTransform()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      Noise2dTransform.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Noise2dTransform) obj).SerializeData(writer));
      Noise2dTransform.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Noise2dTransform) obj).DeserializeData(reader));
    }
  }
}
