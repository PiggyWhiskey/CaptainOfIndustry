// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Vector3Assertions
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System.Diagnostics;
using UnityEngine;

#nullable disable
namespace Mafi.Unity
{
  public static class Vector3Assertions
  {
    [Conditional("MAFI_ASSERTIONS")]
    public static void IsFinite(this Assertion<Vector3> actual, string message = "")
    {
      if (actual.Value.IsFinite())
        return;
      Mafi.Assert.FailAssertion(string.Format("Vector is not finite: {0}", (object) actual.Value), message);
    }
  }
}
