// Decompiled with JetBrains decompiler
// Type: Mafi.AssertionExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

#nullable disable
namespace Mafi
{
  public static class AssertionExtensions
  {
    [MustUseReturnValue("Must use the return value to actually perform the assertion otherwise it is no-op.")]
    [Pure]
    public static Assertion<bool> Or(this Assertion<bool> self, bool value)
    {
      return new Assertion<bool>(self.Value | value);
    }

    [Pure]
    [MustUseReturnValue("Must use the return value to actually perform the assertion otherwise it is no-op.")]
    public static Assertion<bool> And(this Assertion<bool> self, bool value)
    {
      return new Assertion<bool>(self.Value & value);
    }
  }
}
