// Decompiled with JetBrains decompiler
// Type: Mafi.Fix32Extensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi
{
  public static class Fix32Extensions
  {
    [Obsolete("Use NextFix32Between01Fast instead.")]
    [OnlyForSaveCompatibility(null)]
    public static Fix32 NextFix32Between01(this IRandom random)
    {
      return Fix32.FromRaw(random.NextInt(0, 1024));
    }

    /// <summary>
    /// Returns a random number in range [0, 1). This has <see cref="F:Mafi.Fix32.FRACTIONAL_BITS" /> of entropy.
    /// </summary>
    public static Fix32 NextFix32Between01Fast(this IRandom random)
    {
      return Fix32.FromRaw((int) random.NextUlong() & 1023);
    }

    /// <summary>
    /// Returns a random number in range [0, 1). This has <see cref="F:Mafi.Fix32.FRACTIONAL_BITS" /> of entropy.
    /// </summary>
    public static Fix64 NextFix64Between01Fast(this IRandom random)
    {
      return Fix64.FromRaw((long) random.NextUlong() & 1048575L);
    }

    /// <summary>
    /// Returns a random number in range [-8, 8).
    /// This has 4 more bits of entropy than <see cref="M:Mafi.Fix32Extensions.NextFix32Between01Fast(Mafi.IRandom)" />.
    /// </summary>
    public static Fix32 NextFix32BetweenPlusMinus8Fast(this IRandom random)
    {
      return Fix32.FromRaw((int) random.NextUlong() & 16383) - Fix32.Eight;
    }

    /// <summary>
    /// Returns a random number in range [-8, 8).
    /// This has 4 more bits of entropy than <see cref="M:Mafi.Fix32Extensions.NextFix64Between01Fast(Mafi.IRandom)" />.
    /// </summary>
    public static Fix64 NextFix64BetweenPlusMinus8Fast(this IRandom random)
    {
      return Fix64.FromRaw((long) ((int) random.NextUlong() & 16777215)) - Fix64.Eight;
    }

    public static Fix32 NextFix32(this IRandom random, Fix32 minValueIncl, Fix32 maxValueExcl)
    {
      return Fix32.FromRaw(random.NextInt(minValueIncl.RawValue, maxValueExcl.RawValue));
    }

    /// <summary>
    /// It is ok to divide an int that does not fit to Fix32 as long as the result will fit.
    /// </summary>
    public static Fix32 DivToFix32(this int lhs, Fix32 rhs) => lhs / rhs;

    public static Fix64 DivToFix64(this int lhs, Fix32 rhs) => lhs.ToFix32().DivToFix64(rhs);

    /// <summary>
    /// It is ok to divide ints that do not fit to Fix32 as long as the result will fit.
    /// </summary>
    public static Fix32 DivToFix32(this int lhs, int rhs)
    {
      return Fix32.FromFraction((long) lhs, (long) rhs);
    }

    [Pure]
    public static Fix32 BilinearInterpolate(
      this Fix32[] data,
      int stride,
      int baseI,
      Percent tx,
      Percent ty)
    {
      return data[baseI].Lerp(data[baseI + 1], tx).Lerp(data[baseI + stride].Lerp(data[baseI + stride + 1], tx), ty);
    }
  }
}
