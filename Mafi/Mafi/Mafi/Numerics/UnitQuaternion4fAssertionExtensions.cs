// Decompiled with JetBrains decompiler
// Type: Mafi.Numerics.UnitQuaternion4fAssertionExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System.Diagnostics;

#nullable disable
namespace Mafi.Numerics
{
  public static class UnitQuaternion4fAssertionExtensions
  {
    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNormalized(this Assertion<UnitQuaternion4f> actual, string message = "")
    {
      if (actual.Value.IsNormalized)
        return;
      Mafi.Assert.FailAssertion(string.Format("Quaternion is not normalized: {0}", (object) actual.Value), message);
    }
  }
}
