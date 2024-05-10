// Decompiled with JetBrains decompiler
// Type: Mafi.TypeAssertionExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System;
using System.Diagnostics;

#nullable disable
namespace Mafi
{
  public static class TypeAssertionExtensions
  {
    [Conditional("MAFI_ASSERTIONS")]
    public static void IsAssignableTo(this Assertion<Type> actual, Type other, string message = "")
    {
      if (actual.Value.IsAssignableTo(other))
        return;
      Assert.FailAssertion(string.Format("Type '{0}' is not assignable to '{1}'.", (object) actual.Value, (object) other), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsAssignableTo<T>(this Assertion<Type> actual, string message = "")
    {
      if (actual.Value.IsAssignableTo<T>())
        return;
      Assert.FailAssertion(string.Format("Type '{0}' is not assignable to '{1}'.", (object) actual.Value, (object) typeof (T)), message);
    }
  }
}
