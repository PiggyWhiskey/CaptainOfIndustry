// Decompiled with JetBrains decompiler
// Type: Mafi.ListAssertionExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Mafi
{
  public static class ListAssertionExtensions
  {
    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEmpty<T>(this Assertion<List<T>> actual, string message = "")
    {
      if (actual.Value.Count == 0)
        return;
      Assert.FailAssertion(string.Format("List<{0}> expected to be empty but it contains '{1}' values.", (object) typeof (T), (object) actual.Value.Count), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEmpty<T>(this Assertion<List<T>> actual, string message = "")
    {
      if (actual.Value.Count != 0)
        return;
      Assert.FailAssertion(string.Format("List<{0}> expected to be NOT empty but it is empty.", (object) typeof (T)), message);
    }
  }
}
