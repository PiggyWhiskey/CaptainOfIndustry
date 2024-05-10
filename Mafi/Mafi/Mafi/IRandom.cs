// Decompiled with JetBrains decompiler
// Type: Mafi.IRandom
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

#nullable disable
namespace Mafi
{
  /// <summary>Pseudo-random generator.</summary>
  /// <remarks>
  /// It is assumed that this generator may not be able to return value 0. Helper methods for getting random values
  /// count with it.
  /// </remarks>
  public interface IRandom
  {
    /// <summary>
    /// Seeds the random generator by given seed data. The data may not be very random so further randomization with
    /// something like hashing is recommended. The implementation should use ALL bytes to create a seed. Change of a
    /// single bit should result in very different sequence.
    /// </summary>
    void Seed(byte[] seed);

    void SeedFast(ulong seed);

    void SeedFast(ulong seed1, ulong seed2);

    /// <summary>
    /// Returns state of this generator. If this state is passed to the <see cref="M:Mafi.IRandom.Seed(System.Byte[])" /> method it should result
    /// in the same generator at the one that returned the seed. Returned array is from <see cref="T:Mafi.ArrayPool`1" />,
    /// please return it there if possible.
    /// </summary>
    byte[] GetState();

    /// <summary>
    /// Jumps random generator to very many states forward. Typically on the order of 2^64 steps or more. This can
    /// me used to shuffle the generator to generate very different sequence.
    /// </summary>
    void Jump();

    /// <summary>
    /// Returns pseudo-random uniformly distributed 64-bit value.
    /// </summary>
    ulong NextUlong();

    /// <summary>
    /// Clones this instance. Both this and returned instances have identical state.
    /// </summary>
    IRandom Clone();
  }
}
