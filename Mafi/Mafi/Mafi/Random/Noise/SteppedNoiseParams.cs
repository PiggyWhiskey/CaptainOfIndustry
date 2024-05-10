// Decompiled with JetBrains decompiler
// Type: Mafi.Random.Noise.SteppedNoiseParams
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
  public readonly struct SteppedNoiseParams
  {
    /// <summary>
    /// Step size, for example 0.2 will make 10 steps on noise that spans [-1, 1] range.
    /// </summary>
    [EditorLabel(null, "A value of 10 will make 5 steps on input values in range [-25, 25]. Non-positive values will turn off the steps generation.", false, false)]
    public readonly Fix32 StepSize;
    /// <summary>
    /// Step steepness 1 is no steps, values 10-20 are good for step sizes around 0.2-0.4.
    /// </summary>
    [EditorLabel(null, "Controls how steep each step is and must be greater than 1. Steepness values of 5-10 are good for step sizes around 10.", false, false)]
    public readonly Fix32 StepSteepness;

    public static void Serialize(SteppedNoiseParams value, BlobWriter writer)
    {
      Fix32.Serialize(value.StepSize, writer);
      Fix32.Serialize(value.StepSteepness, writer);
    }

    public static SteppedNoiseParams Deserialize(BlobReader reader)
    {
      return new SteppedNoiseParams(Fix32.Deserialize(reader), Fix32.Deserialize(reader));
    }

    public SteppedNoiseParams(Fix32 stepSize, Fix32 stepSteepness)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.StepSize = stepSize;
      this.StepSteepness = stepSteepness;
    }
  }
}
