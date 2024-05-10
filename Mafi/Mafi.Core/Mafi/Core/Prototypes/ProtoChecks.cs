// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Prototypes.ProtoChecks
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi.Core.Prototypes
{
  public static class ProtoChecks
  {
    [Pure]
    public static T CheckNotNullOrPhantom<T>(this T proto) where T : Proto
    {
      if ((Proto) proto == (Proto) null)
        throw new CheckException(string.Format("Value of '{0}' is null! Fix it!", (object) typeof (T)));
      if (proto.IsPhantom)
        Assert.Fail(string.Format("Value of '{0}' is phantom! Fix it!", (object) typeof (T)));
      return proto;
    }
  }
}
