// Decompiled with JetBrains decompiler
// Type: Mafi.RelTile1fExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi
{
  public static class RelTile1fExtensions
  {
    public static RelTile1f Tiles(this Fix32 value) => new RelTile1f(value);

    public static RelTile1f Tiles(this float value) => new RelTile1f(value.ToFix32());

    public static RelTile1f Tiles(this double value) => new RelTile1f(value.ToFix32());

    public static RelTile1f Meters(this double value) => new RelTile1f((value / 2.0).ToFix32());
  }
}
