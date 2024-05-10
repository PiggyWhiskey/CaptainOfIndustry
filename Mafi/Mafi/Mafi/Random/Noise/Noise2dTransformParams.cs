// Decompiled with JetBrains decompiler
// Type: Mafi.Random.Noise.Noise2dTransformParams
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
  public readonly struct Noise2dTransformParams
  {
    [EditorLabel(null, "Multiplies the value of the input noise.", false, false)]
    public readonly Fix64 Multiplier;
    [EditorLabel(null, "Adds to the value of the input noise.", false, false)]
    public readonly Fix64 Addend;
    [EditorLabel(null, "Multiplies the frequency of the input noise.", false, false)]
    public readonly Fix32 FrequencyMult;

    public static void Serialize(Noise2dTransformParams value, BlobWriter writer)
    {
      Fix64.Serialize(value.Multiplier, writer);
      Fix64.Serialize(value.Addend, writer);
      Fix32.Serialize(value.FrequencyMult, writer);
    }

    public static Noise2dTransformParams Deserialize(BlobReader reader)
    {
      return new Noise2dTransformParams(Fix64.Deserialize(reader), Fix64.Deserialize(reader), Fix32.Deserialize(reader));
    }

    public Noise2dTransformParams(Fix64 multiplier, Fix64 addend, Fix32 frequencyMult = default (Fix32))
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.Multiplier = multiplier;
      this.Addend = addend;
      this.FrequencyMult = frequencyMult == 0 ? Fix32.One : frequencyMult;
    }
  }
}
