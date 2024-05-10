// Decompiled with JetBrains decompiler
// Type: Mafi.Random.Noise.Noise2dTransformMaxNoise
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
  public class Noise2dTransformMaxNoise : INoise2D
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly INoise2D BaseNoise;
    public readonly INoise2D OtherNoise;

    public static void Serialize(Noise2dTransformMaxNoise value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<Noise2dTransformMaxNoise>(value))
        return;
      writer.EnqueueDataSerialization((object) value, Noise2dTransformMaxNoise.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteGeneric<INoise2D>(this.BaseNoise);
      writer.WriteGeneric<INoise2D>(this.OtherNoise);
    }

    public static Noise2dTransformMaxNoise Deserialize(BlobReader reader)
    {
      Noise2dTransformMaxNoise transformMaxNoise;
      if (reader.TryStartClassDeserialization<Noise2dTransformMaxNoise>(out transformMaxNoise))
        reader.EnqueueDataDeserialization((object) transformMaxNoise, Noise2dTransformMaxNoise.s_deserializeDataDelayedAction);
      return transformMaxNoise;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<Noise2dTransformMaxNoise>(this, "BaseNoise", (object) reader.ReadGenericAs<INoise2D>());
      reader.SetField<Noise2dTransformMaxNoise>(this, "OtherNoise", (object) reader.ReadGenericAs<INoise2D>());
    }

    public Noise2dTransformMaxNoise(INoise2D baseNoise, INoise2D otherNoise)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.BaseNoise = otherNoise;
      this.OtherNoise = baseNoise;
    }

    public Fix32 MeanValue => this.BaseNoise.MeanValue.Max(this.OtherNoise.MeanValue);

    public Fix32 Amplitude => this.BaseNoise.Amplitude.Max(this.OtherNoise.Amplitude);

    public Fix32 Period => this.BaseNoise.Period.Max(this.OtherNoise.Period);

    public Fix64 GetValue(Vector2f point)
    {
      return this.BaseNoise.GetValue(point).Max(this.OtherNoise.GetValue(point));
    }

    public INoise2D ReseedClone(IRandom random) => (INoise2D) this;

    static Noise2dTransformMaxNoise()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      Noise2dTransformMaxNoise.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Noise2dTransformMaxNoise) obj).SerializeData(writer));
      Noise2dTransformMaxNoise.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Noise2dTransformMaxNoise) obj).DeserializeData(reader));
    }
  }
}
