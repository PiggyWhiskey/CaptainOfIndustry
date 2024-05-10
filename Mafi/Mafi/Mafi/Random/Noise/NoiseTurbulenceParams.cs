// Decompiled with JetBrains decompiler
// Type: Mafi.Random.Noise.NoiseTurbulenceParams
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Random.Noise
{
  [GenerateSerializer(false, null, 0)]
  public readonly struct NoiseTurbulenceParams
  {
    [EditorLabel(null, "Number of noise copies (octaves) to add together. Each successive octave gets its frequency multiplied by lacunarity and its magnitude by persistence (cumulatively). A good value is in rage of 3-6, depending on source noise frequency and lacunarity/persistence values. With persistence of 50%, 6th octave would have only 0.5^5 = 3% contribution. Octaves count lower than 2 disables the turbulence.", false, false)]
    [EditorRange(0.0, 8.0)]
    public readonly int OctavesCount;
    /// <summary>Frequency multiplier between successive octaves.</summary>
    [EditorRange(0.0, 4.0)]
    [EditorLabel(null, "Frequency multiplier between successive octaves. For the best results, use a value between 150% and 350%. A good value is number just under 200% (e.g. 192%) to prevent regular overlap in noise lattices.", false, false)]
    public readonly Percent Lacunarity;
    /// <summary>Scale multiplier between successive octaves.</summary>
    [EditorRange(0.0, 2.0)]
    [EditorLabel(null, "Scale multiplier between successive octaves. A value less than 100% is recommended, e.g as 50%.", false, false)]
    public readonly Percent Persistence;

    public static void Serialize(NoiseTurbulenceParams value, BlobWriter writer)
    {
      writer.WriteInt(value.OctavesCount);
      Percent.Serialize(value.Lacunarity, writer);
      Percent.Serialize(value.Persistence, writer);
    }

    public static NoiseTurbulenceParams Deserialize(BlobReader reader)
    {
      return new NoiseTurbulenceParams(reader.ReadInt(), Percent.Deserialize(reader), Percent.Deserialize(reader));
    }

    /// <param name="octavesCount">Number of created noise octaves.</param>
    /// <param name="lacunarity">Frequency multiplier between successive octaves. For the best results, set
    /// to a number between 1.5 and 3.5. A good value is number just under 2.0 (e.g. 1.92) to prevent regular
    /// overlap in noise lattices.</param>
    /// <param name="persistence">Scale multiplier between successive octaves. Value less than one is recommended,
    /// such as 0.5.</param>
    public NoiseTurbulenceParams(int octavesCount, Percent lacunarity, Percent persistence)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.OctavesCount = octavesCount.CheckNotNegative();
      this.Lacunarity = lacunarity.CheckNotNegative();
      this.Persistence = persistence.CheckNotNegative();
    }

    /// <summary>Returns total persistence over octaves count.</summary>
    [Pure]
    public Percent GetSumPersistence()
    {
      if (this.OctavesCount <= 1)
        return Percent.Hundred;
      if (this.Persistence.IsNear(Percent.Hundred, Percent.One))
        return this.OctavesCount * this.Persistence;
      return this.Persistence.IsNotPositive ? Percent.Zero : (this.Persistence.Pow(this.OctavesCount) - Percent.Hundred) / (this.Persistence - Percent.Hundred);
    }
  }
}
