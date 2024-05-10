// Decompiled with JetBrains decompiler
// Type: Mafi.Random.Noise.INoise3D
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

#nullable disable
namespace Mafi.Random.Noise
{
  public interface INoise3D
  {
    /// <summary>
    /// Approximate mean value of this noise. For example, Perlin noise has mean value of 0.
    /// </summary>
    float MeanValue { get; }

    /// <summary>
    /// Approximate peak amplitude of this noise. For example, Perlin noise has amplitude of 1 (+-1 around mean value
    /// 0).
    /// </summary>
    float Amplitude { get; }

    /// <summary>
    /// Approximate period of this noise. Noise of period `p` should have one peak/valley per `p` units.
    /// </summary>
    float Period { get; }

    /// <summary>Returns noise value at requested 3D point.</summary>
    float GetValue(Vector3f point);
  }
}
