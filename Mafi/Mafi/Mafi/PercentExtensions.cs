// Decompiled with JetBrains decompiler
// Type: Mafi.PercentExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Mafi
{
  public static class PercentExtensions
  {
    /// <summary>
    /// Converts given integer to percent. Value 1 becomes 1%.
    /// </summary>
    public static Mafi.Percent Percent(this int value) => Mafi.Percent.FromPercentVal(value);

    /// <summary>
    /// Converts given float to percent. Value 0.01 becomes 1%.
    /// </summary>
    public static Mafi.Percent Percent(this float value) => Mafi.Percent.FromFloat(value / 100f);

    /// <summary>Converts given double to percent. Value 1 becomes 1%.</summary>
    public static Mafi.Percent Percent(this double value) => Mafi.Percent.FromDouble(value) / 100;

    /// <summary>
    /// Returns random percentage in range 0-100% (exclusive).
    /// </summary>
    public static Mafi.Percent NextPercent(this IRandom random)
    {
      return Mafi.Percent.FromRaw(random.NextInt(0, 100000));
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithin0To100PercIncl(this Assertion<Mafi.Percent> actual, string message = "")
    {
      if (actual.Value.IsWithin0To100PercIncl)
        return;
      Assert.FailAssertion(string.Format("Value of Percent is {0}, expected within 0 and 100% (inclusive).", (object) actual.Value), message);
    }

    [Pure]
    public static Mafi.Percent CheckWithin01Excl(this Mafi.Percent value)
    {
      if (value < Mafi.Percent.Zero)
      {
        Log.Error(string.Format("CHECK FAIL: Value {0} is less than 0%.", (object) value));
        return Mafi.Percent.Zero;
      }
      if (!(value >= Mafi.Percent.Hundred))
        return value;
      Log.Error(string.Format("CHECK FAIL: Value {0} is greater or equal than 100%.", (object) value));
      return 99.Percent();
    }

    [Pure]
    public static Mafi.Percent Sum(this IEnumerable<Mafi.Percent> values)
    {
      Mafi.Percent zero = Mafi.Percent.Zero;
      foreach (Mafi.Percent percent in values)
        zero += percent;
      return zero;
    }
  }
}
