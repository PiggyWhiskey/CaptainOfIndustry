// Decompiled with JetBrains decompiler
// Type: Mafi.IntExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using Mafi.Utils;
using System.Diagnostics;

#nullable disable
namespace Mafi
{
  [DebuggerStepThrough]
  public static class IntExtensions
  {
    [Pure]
    public static Fix32 ToFix32(this int value) => Fix32.FromInt(value);

    [Pure]
    public static Fix64 ToFix64(this int value) => Fix64.FromInt(value);

    [Pure]
    public static Fix32 Over(this int numerator, int denominator)
    {
      return Fix32.FromFraction((long) numerator, (long) denominator);
    }

    [Pure]
    public static Fix32 ScaledByToFix32(this int value, Percent p) => p.ApplyToFix32(value);

    [Pure]
    public static int ScaledByRounded(this int value, Percent p) => p.Apply(value);

    [Pure]
    public static string ToStringCached(this int value) => IntToStringCache.GetString(value);

    [Pure]
    public static string ToStringCached(this uint value) => IntToStringCache.GetString(value);

    [Pure]
    public static string ToStringCached(this byte value) => IntToStringCache.GetString(value);
  }
}
