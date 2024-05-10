﻿// Decompiled with JetBrains decompiler
// Type: Mafi.Tile3iAssertions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using System.Diagnostics;

#nullable disable
namespace Mafi
{
  public static class Tile3iAssertions
  {
    [Conditional("MAFI_ASSERTIONS")]
    public static void IsZero(this Assertion<Tile3i> actual, string message = "")
    {
      if (actual.Value.IsZero)
        return;
      Assert.FailAssertion(string.Format("Tile3i{0} was expected to be zero.", (object) actual.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotZero(this Assertion<Tile3i> actual, string message = "")
    {
      if (actual.Value.IsNotZero)
        return;
      Assert.FailAssertion(string.Format("Tile3i{0} was not expected to be zero.", (object) actual.Value), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNear(
      this Assertion<Tile3i> actual,
      Tile3i expected,
      int tolerance,
      string message = "")
    {
      if (actual.Value.IsNear(expected, tolerance))
        return;
      Assert.FailAssertion(string.Format("Tile3i{0} was expected to be near {1} +- {2}.", (object) actual.Value, (object) expected, (object) tolerance), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNear(
      this Assertion<Tile3i> actual,
      Tile3i expected,
      int tolerance,
      string message = "")
    {
      if (!actual.Value.IsNear(expected, tolerance))
        return;
      Assert.FailAssertion(string.Format("Tile3i{0} was NOT expected to be near {1} +- {2}.", (object) actual.Value, (object) expected, (object) tolerance), message);
    }
  }
}
