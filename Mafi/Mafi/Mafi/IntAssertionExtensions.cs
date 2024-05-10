// Decompiled with JetBrains decompiler
// Type: Mafi.IntAssertionExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using Mafi.Collections.ReadonlyCollections;
using System.Diagnostics;

#nullable disable
namespace Mafi
{
  public static class IntAssertionExtensions
  {
    [Conditional("MAFI_ASSERTIONS")]
    public static void IsValidIndexFor<T>(
      this Assertion<int> actual,
      IIndexable<T> collection,
      string message = "")
    {
      if ((uint) actual.Value < (uint) collection.Count)
        return;
      Assert.FailAssertion(string.Format("Value {0} is not valid index for '{1}' of length {2}.", (object) actual.Value, (object) collection.GetType(), (object) collection.Count), message);
    }
  }
}
