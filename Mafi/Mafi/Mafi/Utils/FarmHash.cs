// Decompiled with JetBrains decompiler
// Type: Mafi.Utils.FarmHash
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System.Runtime.CompilerServices;

#nullable disable
namespace Mafi.Utils
{
  /// <summary>
  /// Farmhash.Sharp is a .NET port of Google's Farmhash algorithm for calculating 32bit and 64bit non-cryptographic
  /// hashes. Farmhash.Sharp has great performance characteristics when calculating 64bit hashes, especially on short
  /// strings or a subsequence of byte arrays.
  /// </summary>
  public static class FarmHash
  {
    private static uint Rotate32(uint val, int shift)
    {
      return shift != 0 ? val >> shift | val << 32 - shift : val;
    }

    private static uint Rotate(uint val, int shift) => FarmHash.Rotate32(val, shift);

    private static unsafe uint Fetch32(byte* p) => *(uint*) p;

    private static unsafe uint Fetch(byte* p) => FarmHash.Fetch32(p);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint fmix(uint h)
    {
      h ^= h >> 16;
      h *= 2246822507U;
      h ^= h >> 13;
      h *= 3266489909U;
      h ^= h >> 16;
      return h;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe uint Hash32Len0to4(byte* s, uint len, uint seed = 0)
    {
      uint a = seed;
      uint h = 9;
      for (int index = 0; (long) index < (long) len; ++index)
      {
        a = a * 3432918353U + (uint) s[index];
        h ^= a;
      }
      return FarmHash.fmix(FarmHash.Mur(a, FarmHash.Mur(len, h)));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint Mur(uint a, uint h)
    {
      a *= 3432918353U;
      a = FarmHash.Rotate32(a, 17);
      a *= 461845907U;
      h ^= a;
      h = FarmHash.Rotate32(h, 19);
      return (uint) ((int) h * 5 - 430675100);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe uint Hash32Len13to24(byte* s, uint len, uint seed = 0)
    {
      uint val1 = FarmHash.Fetch(s - 4 + (len >> 1));
      uint num1 = FarmHash.Fetch(s + 4);
      uint a1 = FarmHash.Fetch(s + len - 8);
      uint num2 = FarmHash.Fetch(s + (len >> 1));
      uint a2 = FarmHash.Fetch(s);
      uint num3 = FarmHash.Fetch(s + len - 4);
      uint h1 = num2 * 3432918353U + len + seed;
      uint val2 = FarmHash.Rotate(val1, 12) + num3;
      uint h2 = FarmHash.Mur(a1, h1) + val2;
      uint num4 = FarmHash.Rotate(val2, 3) + a1;
      uint h3 = FarmHash.Mur(a2, h2) + num4;
      uint num5 = FarmHash.Rotate(num4 + num3, 12) + num2;
      return FarmHash.fmix(FarmHash.Mur(num1 ^ seed, h3) + num5);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe uint Hash32Len5to12(byte* s, uint len, uint seed = 0)
    {
      uint num1 = len;
      uint num2 = len * 5U;
      uint num3 = 9;
      uint h = num2 + seed;
      uint a1 = num1 + FarmHash.Fetch(s);
      uint a2 = num2 + FarmHash.Fetch(s + len - 4);
      uint a3 = num3 + FarmHash.Fetch(s + (len >> 1 & 4U));
      return FarmHash.fmix(seed ^ FarmHash.Mur(a3, FarmHash.Mur(a2, FarmHash.Mur(a1, h))));
    }

    /// <summary>
    /// Calculates a 32bit hash from a given byte array upto a certain length
    /// </summary>
    /// <param name="s">pointer to bytes that contain at least <paramref name="len" /> bytes</param>
    /// <param name="length">number of bytes to consume to calculate hash</param>
    /// <returns>A 32bit hash</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe uint Hash32(byte* s, int length)
    {
      uint len = (uint) length;
      if (len <= 24U)
      {
        if (len > 12U)
          return FarmHash.Hash32Len13to24(s, len);
        return len > 4U ? FarmHash.Hash32Len5to12(s, len) : FarmHash.Hash32Len0to4(s, len);
      }
      uint num1 = len;
      uint num2 = 3432918353U * len;
      uint num3 = num2;
      uint num4 = FarmHash.Rotate(FarmHash.Fetch(s + len - 4) * 3432918353U, 17) * 461845907U;
      uint num5 = FarmHash.Rotate(FarmHash.Fetch(s + len - 8) * 3432918353U, 17) * 461845907U;
      uint num6 = FarmHash.Rotate(FarmHash.Fetch(s + len - 16) * 3432918353U, 17) * 461845907U;
      uint num7 = FarmHash.Rotate(FarmHash.Fetch(s + len - 12) * 3432918353U, 17) * 461845907U;
      uint num8 = FarmHash.Rotate(FarmHash.Fetch(s + len - 20) * 3432918353U, 17) * 461845907U;
      uint num9 = (uint) ((int) FarmHash.Rotate((uint) ((int) FarmHash.Rotate(num1 ^ num4, 19) * 5 - 430675100) ^ num6, 19) * 5 - 430675100);
      uint val1 = (uint) ((int) FarmHash.Rotate((uint) ((int) FarmHash.Rotate(num2 ^ num5, 19) * 5 - 430675100) ^ num7, 19) * 5 - 430675100);
      uint val2 = FarmHash.Rotate(num3 + num8, 19) + 113U;
      uint num10 = (len - 1U) / 20U;
      do
      {
        uint num11 = FarmHash.Fetch(s);
        uint num12 = FarmHash.Fetch(s + 4);
        uint a1 = FarmHash.Fetch(s + 8);
        uint a2 = FarmHash.Fetch(s + 12);
        uint num13 = FarmHash.Fetch(s + 16);
        uint h1 = num9 + num11;
        uint h2 = val1 + num12;
        uint h3 = val2 + a1;
        num9 = FarmHash.Mur(a2, h1) + num13;
        uint num14 = FarmHash.Mur(a1, h2) + num11;
        val2 = FarmHash.Mur(num12 + num13 * 3432918353U, h3) + a2 + num14;
        val1 = num14 + val2;
        s += 20;
      }
      while (--num10 != 0U);
      uint num15 = FarmHash.Rotate(FarmHash.Rotate(val1, 11) * 3432918353U, 17) * 3432918353U;
      uint num16 = FarmHash.Rotate(FarmHash.Rotate(val2, 11) * 3432918353U, 17) * 3432918353U;
      return FarmHash.Rotate((uint) ((int) FarmHash.Rotate(FarmHash.Rotate((uint) ((int) FarmHash.Rotate(num9 + num15, 19) * 5 - 430675100), 17) * 3432918353U + num16, 19) * 5 - 430675100), 17) * 3432918353U;
    }

    /// <summary>
    /// Calculates a 32bit hash from a given byte array upto a certain length
    /// </summary>
    /// <param name="s">Byte array to calculate the hash on</param>
    /// <param name="length">Number of bytes from the buffer to calculate the hash with</param>
    /// <returns>A 32bit hash</returns>
    public static unsafe uint Hash32(byte[] s, int length)
    {
      fixed (byte* s1 = s)
        return FarmHash.Hash32(s1, length);
    }

    /// <summary>
    /// Calculates a 32bit hash from a given string.
    /// <para>
    /// See the
    /// <see href="/articles/guides/strings.html">article on strings</see>
    /// for longform explanation
    /// </para>
    /// </summary>
    /// <param name="s">String to hash</param>
    /// <returns>A 32bit hash</returns>
    public static unsafe uint Hash32(string s)
    {
      fixed (char* s1 = s)
        return FarmHash.Hash32((byte*) s1, s.Length * 2);
    }
  }
}
