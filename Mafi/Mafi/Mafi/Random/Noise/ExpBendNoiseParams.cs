// Decompiled with JetBrains decompiler
// Type: Mafi.Random.Noise.ExpBendNoiseParams
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
  public readonly struct ExpBendNoiseParams
  {
    [EditorLabel(null, "The effect amount, how much to bend from linear to exponential.", false, false)]
    [EditorRange(0.0, 1.25)]
    public readonly Percent Amount;
    [EditorLabel(null, "Specifies the origin point of the transformation.", false, false)]
    public readonly Fix32 Bias;

    public static void Serialize(ExpBendNoiseParams value, BlobWriter writer)
    {
      Percent.Serialize(value.Amount, writer);
      Fix32.Serialize(value.Bias, writer);
    }

    public static ExpBendNoiseParams Deserialize(BlobReader reader)
    {
      return new ExpBendNoiseParams(Percent.Deserialize(reader), Fix32.Deserialize(reader));
    }

    public ExpBendNoiseParams(Percent amount, Fix32 bias)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.Amount = amount;
      this.Bias = bias;
    }
  }
}
