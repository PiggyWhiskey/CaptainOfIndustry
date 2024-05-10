// Decompiled with JetBrains decompiler
// Type: Mafi.Random.Noise.NoiseTurbulence
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
  /// <summary>
  /// Turbulence function combines base noises as a weighted sum of different frequencies. For every octave the
  /// frequency increases and contribution decreases exponentially.
  /// 
  /// The lacunarity specifies the frequency multiplier between successive octaves. For the best results, set the
  /// lacunarity to a number between 1.5 and 3.5. A good value is number just under 2.0 (e.g. 1.92) to prevent regular
  /// overlap in noise lattices.
  /// 
  /// The persistence value determines how quickly the amplitudes diminish for successive octaves. The amplitude of the
  /// first octave is 1.0. The amplitude of each subsequent octave is equal to the product of the previous octave's
  /// amplitude and the persistence value. The persistence is controls roughness.
  /// 
  /// To cover all possible feature scales, the number of octaves is typically a bit less than log(terrain_width) /
  /// log(lacunarity). So, for a 1024 x 1024 height field, about 10 octaves are needed.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public class NoiseTurbulence : INoise2D
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private static readonly Percent MIN_CONTRIBUTION;
    public readonly INoise2D BaseNoise;
    public readonly SimplexNoise2dSeed Seed;
    public readonly NoiseTurbulenceParams TurbulenceParams;
    private readonly Vector2f[] m_offsets;
    private readonly Fix64 m_sumPersistenceInv;

    public static void Serialize(NoiseTurbulence value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<NoiseTurbulence>(value))
        return;
      writer.EnqueueDataSerialization((object) value, NoiseTurbulence.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteGeneric<INoise2D>(this.BaseNoise);
      writer.WriteArray<Vector2f>(this.m_offsets);
      Fix64.Serialize(this.m_sumPersistenceInv, writer);
      Fix32.Serialize(this.Period, writer);
      SimplexNoise2dSeed.Serialize(this.Seed, writer);
      NoiseTurbulenceParams.Serialize(this.TurbulenceParams, writer);
    }

    public static NoiseTurbulence Deserialize(BlobReader reader)
    {
      NoiseTurbulence noiseTurbulence;
      if (reader.TryStartClassDeserialization<NoiseTurbulence>(out noiseTurbulence))
        reader.EnqueueDataDeserialization((object) noiseTurbulence, NoiseTurbulence.s_deserializeDataDelayedAction);
      return noiseTurbulence;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<NoiseTurbulence>(this, "BaseNoise", (object) reader.ReadGenericAs<INoise2D>());
      reader.SetField<NoiseTurbulence>(this, "m_offsets", (object) reader.ReadArray<Vector2f>());
      reader.SetField<NoiseTurbulence>(this, "m_sumPersistenceInv", (object) Fix64.Deserialize(reader));
      this.Period = Fix32.Deserialize(reader);
      reader.SetField<NoiseTurbulence>(this, "Seed", (object) SimplexNoise2dSeed.Deserialize(reader));
      reader.SetField<NoiseTurbulence>(this, "TurbulenceParams", (object) NoiseTurbulenceParams.Deserialize(reader));
    }

    public Fix32 MeanValue => this.BaseNoise.MeanValue;

    public Fix32 Amplitude => this.BaseNoise.Amplitude;

    public Fix32 Period { get; private set; }

    public NoiseTurbulence(
      INoise2D baseNoise,
      SimplexNoise2dSeed seed,
      NoiseTurbulenceParams turbulenceParams)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.BaseNoise = baseNoise.CheckNotNull<INoise2D>();
      this.Seed = seed;
      this.TurbulenceParams = turbulenceParams;
      this.m_sumPersistenceInv = Fix64.One / turbulenceParams.GetSumPersistence().ToFix64();
      Percent percent = turbulenceParams.OctavesCount <= 1 ? turbulenceParams.Lacunarity : turbulenceParams.Lacunarity.Pow(turbulenceParams.OctavesCount);
      this.Period = percent.IsPositive ? percent.ApplyInverse(baseNoise.Period) : this.BaseNoise.Period;
      if (turbulenceParams.OctavesCount <= 1 || turbulenceParams.Persistence.IsNotPositive || turbulenceParams.Lacunarity.IsNotPositive)
      {
        this.m_offsets = Array.Empty<Vector2f>();
      }
      else
      {
        this.m_offsets = new Vector2f[turbulenceParams.OctavesCount - 1];
        Vector2f vector2f = 10 * seed.Vector2f;
        for (int index = 1; index < turbulenceParams.OctavesCount; ++index)
        {
          vector2f = (vector2f * Fix32.Sqrt2 + vector2f.Yx * index).Modulo((Fix32) 10);
          this.m_offsets[index - 1] = vector2f;
        }
      }
    }

    public Fix64 GetValue(Vector2f point)
    {
      Fix64 fix64 = this.BaseNoise.GetValue(point);
      Percent hundred = Percent.Hundred;
      foreach (Vector2f offset in this.m_offsets)
      {
        point += offset;
        point *= this.TurbulenceParams.Lacunarity;
        hundred *= this.TurbulenceParams.Persistence;
        if (!(hundred < NoiseTurbulence.MIN_CONTRIBUTION))
          fix64 += hundred.Apply(this.BaseNoise.GetValue(point));
        else
          break;
      }
      return fix64 * this.m_sumPersistenceInv;
    }

    public INoise2D ReseedClone(IRandom random) => throw new NotImplementedException();

    static NoiseTurbulence()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      NoiseTurbulence.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((NoiseTurbulence) obj).SerializeData(writer));
      NoiseTurbulence.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((NoiseTurbulence) obj).DeserializeData(reader));
      NoiseTurbulence.MIN_CONTRIBUTION = 3.Percent();
    }
  }
}
