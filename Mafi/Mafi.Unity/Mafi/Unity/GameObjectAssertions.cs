// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.GameObjectAssertions
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System.Diagnostics;
using UnityEngine;

#nullable disable
namespace Mafi.Unity
{
  public static class GameObjectAssertions
  {
    /// <summary>
    /// Asserts that given <see cref="T:UnityEngine.GameObject" /> is not null or destroyed.
    /// </summary>
    [Conditional("MAFI_ASSERTIONS")]
    public static void IsValidUnityObject<T>(this Assertion<T> actual, string message = "") where T : Object
    {
      if ((bool) (Object) actual.Value)
        return;
      Mafi.Assert.FailAssertion(string.Format("Value of Unity Object {0} is destroyed or null.", (object) typeof (Object)), message);
    }

    /// <summary>
    /// Asserts that given <see cref="T:Mafi.Option`1" /> valid unity object or None.
    /// </summary>
    [Conditional("MAFI_ASSERTIONS")]
    public static void IsValidUnityObjectOrNone<T>(this Assertion<Option<T>> actual, string message = "") where T : Object
    {
      if (actual.Value.IsNone || (bool) (Object) actual.Value.Value)
        return;
      Mafi.Assert.FailAssertion(string.Format("Value of {0} is destroyed or null.", (object) typeof (GameObject)), message);
    }
  }
}
