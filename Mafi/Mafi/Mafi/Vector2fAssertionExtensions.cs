// Decompiled with JetBrains decompiler
// Type: Mafi.Vector2fAssertionExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System.Diagnostics;

#nullable disable
namespace Mafi
{
  public static class Vector2fAssertionExtensions
  {
    [Conditional("MAFI_ASSERTIONS")]
    public static void IsWithin01InclExcl(this Assertion<Vector2f> actual, string message = "")
    {
      if (actual.Value.X >= Fix32.Zero && actual.Value.X < Fix32.One && actual.Value.Y >= Fix32.Zero && actual.Value.Y < Fix32.One)
        return;
      Assert.FailAssertion(string.Format("Value {0} is not >= 0.0 and < 1.0.", (object) actual.Value), message);
    }
  }
}
