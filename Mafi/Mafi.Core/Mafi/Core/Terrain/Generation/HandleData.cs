// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.HandleData
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  public readonly struct HandleData
  {
    public readonly Tile2f Position;
    public readonly ColorRgba Color;
    public readonly ColorRgba IconColor;
    public readonly Option<string> IconAssetPath;
    public readonly HeightTilesF? Height;

    public HandleData(
      Tile2f position,
      ColorRgba color,
      Option<string> iconAssetPath,
      ColorRgba iconColor = default (ColorRgba),
      HeightTilesF? height = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Position = position;
      this.Height = height;
      this.Color = color;
      this.IconAssetPath = iconAssetPath;
      this.IconColor = iconColor == new ColorRgba() ? ColorRgba.White : iconColor;
    }
  }
}
