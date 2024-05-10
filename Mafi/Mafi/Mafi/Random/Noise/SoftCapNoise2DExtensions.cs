// Decompiled with JetBrains decompiler
// Type: Mafi.Random.Noise.SoftCapNoise2DExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

#nullable disable
namespace Mafi.Random.Noise
{
  public static class SoftCapNoise2DExtensions
  {
    public static INoise2D SoftCap(this INoise2D noise, SoftCapNoiseParams capParams)
    {
      return capParams.CapStart.IsNear(capParams.CapEnd) ? noise : (INoise2D) new SoftCapNoise2D(noise, capParams);
    }
  }
}
