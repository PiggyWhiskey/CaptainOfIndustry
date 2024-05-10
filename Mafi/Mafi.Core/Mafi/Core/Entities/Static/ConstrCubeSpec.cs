// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.ConstrCubeSpec
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Utils;

#nullable disable
namespace Mafi.Core.Entities.Static
{
  [ExpectedStructSize(12)]
  public readonly struct ConstrCubeSpec
  {
    public readonly Tile2iSlim Position;
    public readonly HeightTilesISlim Height;
    public readonly ushort Volume;
    public readonly byte ScaleX;
    public readonly byte ScaleY;
    public readonly byte ScaleZ;
    public readonly byte TransitionHeightTiles;

    public ConstrCubeSpec(
      Tile2iSlim position,
      HeightTilesISlim height,
      byte scaleX,
      byte scaleY,
      byte scaleZ,
      byte transitionHeightTiles)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Position = position;
      this.Height = height;
      this.ScaleX = scaleX;
      this.ScaleY = scaleY;
      this.ScaleZ = scaleZ;
      this.TransitionHeightTiles = transitionHeightTiles;
      this.Volume = (ushort) ((uint) scaleX * (uint) scaleY * (uint) scaleZ);
    }

    public override string ToString()
    {
      return string.Format("({0}, {1}, {2}) s({3}, {4}, {5}) h {6}", (object) this.Position.X, (object) this.Position.Y, (object) this.Height, (object) this.ScaleX, (object) this.ScaleY, (object) this.ScaleZ, (object) this.TransitionHeightTiles);
    }
  }
}
