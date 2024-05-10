// Decompiled with JetBrains decompiler
// Type: Mafi.Vector4fAssertions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System.Diagnostics;

#nullable disable
namespace Mafi
{
  public static class Vector4fAssertions
  {
    [Conditional("MAFI_ASSERTIONS")]
    public static void IsZero(this Assertion<Vector4f> actual, string message = "")
    {
      if (actual.Value.IsZero)
        return;
      Assert.FailAssertion(string.Format("Vector4f{0} was expected to be zero.", (object) actual.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotZero(this Assertion<Vector4f> actual, string message = "")
    {
      if (actual.Value.IsNotZero)
        return;
      Assert.FailAssertion(string.Format("Vector4f{0} was not expected to be zero.", (object) actual.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear(
      this Assertion<Vector4f> actual,
      Vector4f expected,
      Fix32 tolerance,
      string message = "")
    {
      if (actual.Value.IsNear(expected, tolerance))
        return;
      Assert.FailAssertion(string.Format("Vector4f{0} was expected to be near {1} +- {2}.", (object) actual.Value, (object) expected, (object) tolerance), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNear(
      this Assertion<Vector4f> actual,
      Vector4f expected,
      Fix32 tolerance,
      string message = "")
    {
      if (!actual.Value.IsNear(expected, tolerance))
        return;
      Assert.FailAssertion(string.Format("Vector4f{0} was NOT expected to be near {1} +- {2}.", (object) actual.Value, (object) expected, (object) tolerance), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNormalized(this Assertion<Vector4f> actual, string message = "")
    {
      if (actual.Value.IsNormalized)
        return;
      Assert.FailAssertion(string.Format("Vector4f{0} is not normalized, lenght: {1}, tolerance:{2}.", (object) actual.Value, (object) actual.Value.Length, (object) Fix64.EpsilonFix32NearOneSqr), message);
    }
  }
}
