// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Map.MapCellEdge
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.Map
{
  public struct MapCellEdge
  {
    public readonly Tile2i From;
    public readonly Tile2i To;
    public readonly int EdgeIndex;

    public Tile2f CenterPoint => this.From.CornerTile2f.Average(this.To.CornerTile2f);

    public MapCellEdge(Tile2i from, Tile2i to, int edgeIndex)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.From = from;
      this.To = to;
      this.EdgeIndex = edgeIndex;
    }
  }
}
