// Decompiled with JetBrains decompiler
// Type: Mafi.Random.Noise.INoise2D
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

#nullable disable
namespace Mafi.Random.Noise
{
  public interface INoise2D
  {
    /// <summary>
    /// Approximate mean value of this noise. For example, Perlin noise has mean value of 0.
    /// </summary>
    Fix32 MeanValue { get; }

    /// <summary>
    /// Approximate peak amplitude of this noise. For example, Perlin noise has amplitude of 1 (+-1 around mean
    /// value 0).
    /// </summary>
    Fix32 Amplitude { get; }

    /// <summary>
    /// Approximate period of this noise. Noise of period `p` should have one peak/valley per `p` units.
    /// </summary>
    Fix32 Period { get; }

    /// <summary>Returns noise value at requested point.</summary>
    Fix64 GetValue(Vector2f point);

    /// <summary>
    /// Returns a new noise that is re-seeded with given random generator. If a noise implementation is immutable
    /// and does not contain any source or randomness it can simply return itself.
    /// </summary>
    INoise2D ReseedClone(IRandom random);
  }
}
