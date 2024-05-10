// Decompiled with JetBrains decompiler
// Type: Mafi.Core.ProtoAssertionExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Prototypes;
using System.Diagnostics;

#nullable disable
namespace Mafi.Core
{
  public static class ProtoAssertionExtensions
  {
    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotNullOrPhantom<T>(this Assertion<T> proto, string message = "") where T : Proto
    {
      if ((Proto) proto.Value == (Proto) null)
      {
        Mafi.Assert.FailAssertion(string.Format("Prototype '{0}' is null.", (object) typeof (T)), message);
      }
      else
      {
        if (!proto.Value.IsPhantom)
          return;
        Mafi.Assert.FailAssertion(string.Format("Prototype '{0}' is Phantom.", (object) typeof (T)), message);
      }
    }
  }
}
