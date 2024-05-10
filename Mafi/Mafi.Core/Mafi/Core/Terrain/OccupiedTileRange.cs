// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.OccupiedTileRange
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Utils;

#nullable disable
namespace Mafi.Core.Terrain
{
  [ExpectedStructSize(8)]
  public readonly struct OccupiedTileRange
  {
    public readonly Tile2iSlim Position;
    public readonly short FromRaw;
    public readonly ushort VerticalSizeRaw;

    public HeightTilesI From => new HeightTilesI((int) this.FromRaw);

    public ThicknessTilesI VerticalSize => new ThicknessTilesI((int) this.VerticalSizeRaw);

    public HeightTilesI ToExcl => new HeightTilesI((int) this.FromRaw + (int) this.VerticalSizeRaw);

    public OccupiedTileRange(Tile2iSlim position, short fromRaw, ushort verticalSizeRaw)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Position = position;
      this.FromRaw = fromRaw;
      this.VerticalSizeRaw = verticalSizeRaw;
    }

    public OccupiedTileRange(Tile2i position, HeightTilesI from, ThicknessTilesI verticalSize)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this = new OccupiedTileRange(position.AsSlim, (short) from.Value, (ushort) verticalSize.Value);
    }

    public override string ToString()
    {
      return string.Format("{0} {1}+{2}", (object) this.Position, (object) this.FromRaw, (object) this.VerticalSizeRaw);
    }
  }
}
