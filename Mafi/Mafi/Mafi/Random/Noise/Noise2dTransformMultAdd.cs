// Decompiled with JetBrains decompiler
// Type: Mafi.Random.Noise.Noise2dTransformMultAdd
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
  public class Noise2dTransformMultAdd : INoise2D
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly INoise2D m_baseNoise;
    public readonly Fix64 Multiplier;
    public readonly Fix64 Addend;

    public static void Serialize(Noise2dTransformMultAdd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<Noise2dTransformMultAdd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, Noise2dTransformMultAdd.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Fix64.Serialize(this.Addend, writer);
      writer.WriteGeneric<INoise2D>(this.m_baseNoise);
      Fix64.Serialize(this.Multiplier, writer);
    }

    public static Noise2dTransformMultAdd Deserialize(BlobReader reader)
    {
      Noise2dTransformMultAdd transformMultAdd;
      if (reader.TryStartClassDeserialization<Noise2dTransformMultAdd>(out transformMultAdd))
        reader.EnqueueDataDeserialization((object) transformMultAdd, Noise2dTransformMultAdd.s_deserializeDataDelayedAction);
      return transformMultAdd;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<Noise2dTransformMultAdd>(this, "Addend", (object) Fix64.Deserialize(reader));
      reader.SetField<Noise2dTransformMultAdd>(this, "m_baseNoise", (object) reader.ReadGenericAs<INoise2D>());
      reader.SetField<Noise2dTransformMultAdd>(this, "Multiplier", (object) Fix64.Deserialize(reader));
    }

    public Noise2dTransformMultAdd(INoise2D baseNoise, Fix64 multiplier, Fix64 addend)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Multiplier = multiplier;
      this.Addend = addend;
      this.m_baseNoise = baseNoise.CheckNotNull<INoise2D>();
    }

    public Fix32 MeanValue => (this.m_baseNoise.MeanValue + this.Addend).ToFix32();

    public Fix32 Amplitude => (this.m_baseNoise.Amplitude * this.Multiplier).ToFix32();

    public Fix32 Period => this.m_baseNoise.Period;

    public Fix64 GetValue(Vector2f point)
    {
      return this.Addend + this.Multiplier * this.m_baseNoise.GetValue(point);
    }

    public INoise2D ReseedClone(IRandom random) => (INoise2D) this;

    static Noise2dTransformMultAdd()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      Noise2dTransformMultAdd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Noise2dTransformMultAdd) obj).SerializeData(writer));
      Noise2dTransformMultAdd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Noise2dTransformMultAdd) obj).DeserializeData(reader));
    }
  }
}
