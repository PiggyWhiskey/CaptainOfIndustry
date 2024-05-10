// Decompiled with JetBrains decompiler
// Type: Mafi.Random.Noise.SteppedNoise2D
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
  public class SteppedNoise2D : INoise2D
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly SteppedNoiseParams Parameters;
    private readonly INoise2D m_noise;
    private readonly Fix64 m_precomputedDenominator;

    public static void Serialize(SteppedNoise2D value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SteppedNoise2D>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SteppedNoise2D.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteGeneric<INoise2D>(this.m_noise);
      Fix64.Serialize(this.m_precomputedDenominator, writer);
      SteppedNoiseParams.Serialize(this.Parameters, writer);
    }

    public static SteppedNoise2D Deserialize(BlobReader reader)
    {
      SteppedNoise2D steppedNoise2D;
      if (reader.TryStartClassDeserialization<SteppedNoise2D>(out steppedNoise2D))
        reader.EnqueueDataDeserialization((object) steppedNoise2D, SteppedNoise2D.s_deserializeDataDelayedAction);
      return steppedNoise2D;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<SteppedNoise2D>(this, "m_noise", (object) reader.ReadGenericAs<INoise2D>());
      reader.SetField<SteppedNoise2D>(this, "m_precomputedDenominator", (object) Fix64.Deserialize(reader));
      reader.SetField<SteppedNoise2D>(this, "Parameters", (object) SteppedNoiseParams.Deserialize(reader));
    }

    public Fix32 MeanValue => this.m_noise.MeanValue;

    public Fix32 Amplitude => this.m_noise.Amplitude;

    public Fix32 Period => this.m_noise.Period;

    public SteppedNoise2D(INoise2D noise, SteppedNoiseParams parameters)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Assert.That<Fix32>(parameters.StepSize).IsPositive();
      Assert.That<Fix32>(parameters.StepSteepness).IsGreater((Fix32) 1);
      this.Parameters = parameters;
      this.m_noise = noise.CheckNotNull<INoise2D>();
      this.m_precomputedDenominator = 2 * (this.Parameters.StepSteepness.ToFix64() / 2).Tanh();
    }

    public Fix64 GetValue(Vector2f point)
    {
      Fix64 fix64_1 = this.m_noise.GetValue(point) / this.Parameters.StepSize;
      Fix64 fix64_2 = fix64_1.Floored() + Fix64.Half;
      return this.Parameters.StepSize * ((this.Parameters.StepSteepness * (fix64_1 - fix64_2)).Tanh() / this.m_precomputedDenominator + fix64_2);
    }

    public INoise2D ReseedClone(IRandom random)
    {
      return (INoise2D) new SteppedNoise2D(this.m_noise.ReseedClone(random), this.Parameters);
    }

    static SteppedNoise2D()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      SteppedNoise2D.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((SteppedNoise2D) obj).SerializeData(writer));
      SteppedNoise2D.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((SteppedNoise2D) obj).DeserializeData(reader));
    }
  }
}
