// Decompiled with JetBrains decompiler
// Type: Mafi.Random.Noise.Noise2dTransformMinNoise
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
  public class Noise2dTransformMinNoise : INoise2D
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly INoise2D BaseNoise;
    public readonly INoise2D OtherNoise;

    public static void Serialize(Noise2dTransformMinNoise value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<Noise2dTransformMinNoise>(value))
        return;
      writer.EnqueueDataSerialization((object) value, Noise2dTransformMinNoise.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteGeneric<INoise2D>(this.BaseNoise);
      writer.WriteGeneric<INoise2D>(this.OtherNoise);
    }

    public static Noise2dTransformMinNoise Deserialize(BlobReader reader)
    {
      Noise2dTransformMinNoise transformMinNoise;
      if (reader.TryStartClassDeserialization<Noise2dTransformMinNoise>(out transformMinNoise))
        reader.EnqueueDataDeserialization((object) transformMinNoise, Noise2dTransformMinNoise.s_deserializeDataDelayedAction);
      return transformMinNoise;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<Noise2dTransformMinNoise>(this, "BaseNoise", (object) reader.ReadGenericAs<INoise2D>());
      reader.SetField<Noise2dTransformMinNoise>(this, "OtherNoise", (object) reader.ReadGenericAs<INoise2D>());
    }

    public Noise2dTransformMinNoise(INoise2D baseNoise, INoise2D otherNoise)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.BaseNoise = otherNoise;
      this.OtherNoise = baseNoise;
    }

    public Fix32 MeanValue => this.BaseNoise.MeanValue.Min(this.OtherNoise.MeanValue);

    public Fix32 Amplitude => this.BaseNoise.Amplitude.Min(this.OtherNoise.Amplitude);

    public Fix32 Period => this.BaseNoise.Period.Min(this.OtherNoise.Period);

    public Fix64 GetValue(Vector2f point)
    {
      return this.BaseNoise.GetValue(point).Min(this.OtherNoise.GetValue(point));
    }

    public INoise2D ReseedClone(IRandom random) => (INoise2D) this;

    static Noise2dTransformMinNoise()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      Noise2dTransformMinNoise.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Noise2dTransformMinNoise) obj).SerializeData(writer));
      Noise2dTransformMinNoise.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Noise2dTransformMinNoise) obj).DeserializeData(reader));
    }
  }
}
