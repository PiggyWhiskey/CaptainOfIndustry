// Decompiled with JetBrains decompiler
// Type: Mafi.Random.Noise.SoftCapNoiseParams
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
  public readonly struct SoftCapNoiseParams
  {
    [EditorEnforceOrder(61)]
    [EditorLabel(null, "Start of the soft-cap region. The function will start converging to the 'cap end' from here.", false, false)]
    public readonly Fix32 CapStart;
    [EditorEnforceOrder(66)]
    [EditorLabel(null, "End of the soft-cap region. If start is lower than end, this will be the maximum value, otherwise it will be the minimum.", false, false)]
    public readonly Fix32 CapEnd;

    public static void Serialize(SoftCapNoiseParams value, BlobWriter writer)
    {
      Fix32.Serialize(value.CapStart, writer);
      Fix32.Serialize(value.CapEnd, writer);
    }

    public static SoftCapNoiseParams Deserialize(BlobReader reader)
    {
      return new SoftCapNoiseParams(Fix32.Deserialize(reader), Fix32.Deserialize(reader));
    }

    public SoftCapNoiseParams(Fix32 capStart, Fix32 capEnd)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.CapStart = capStart;
      this.CapEnd = capEnd;
    }
  }
}
