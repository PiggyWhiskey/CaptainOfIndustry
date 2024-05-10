// Decompiled with JetBrains decompiler
// Type: Mafi.MechPowerExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi
{
  public static class MechPowerExtensions
  {
    public static MechPower KwMech(this int kw) => MechPower.FromKw(kw);

    public static MechPower MwMech(this int mw) => MechPower.FromMw(mw);
  }
}
