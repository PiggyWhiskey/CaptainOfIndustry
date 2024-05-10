// Decompiled with JetBrains decompiler
// Type: Mafi.Random.Noise.SimplexNoise2dSeedExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

#nullable disable
namespace Mafi.Random.Noise
{
  public static class SimplexNoise2dSeedExtensions
  {
    public static SimplexNoise2dSeed NoiseSeed2dLegacy(this IRandom random)
    {
      return new SimplexNoise2dSeed(random.NextFix32Between01(), Fix32.One - random.NextFix32Between01());
    }

    public static SimplexNoise2dSeed NoiseSeed2d(this IRandom random)
    {
      return new SimplexNoise2dSeed(random.NextFix32BetweenPlusMinus8Fast(), random.NextFix32BetweenPlusMinus8Fast());
    }
  }
}
