// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Map.CellEdge
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Numerics;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Map
{
  [GenerateSerializer(false, null, 0)]
  public struct CellEdge : IEquatable<CellEdge>
  {
    public readonly MapCell C1;
    public readonly MapCell C2;

    public Tile2i CenterTile => this.C1.CenterTile.Average(this.C2.CenterTile);

    public static CellEdge Create(MapCell c1, MapCell c2)
    {
      Assert.That<int>(c1.Id.Value).IsLess(c2.Id.Value, "Edge is not sorted correctly.");
      return new CellEdge(c1, c2);
    }

    [LoadCtor]
    public CellEdge(MapCell c1, MapCell c2)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.C1 = c1.CheckNotNull<MapCell>();
      this.C2 = c2.CheckNotNull<MapCell>();
    }

    public Line2i GetEdgeLine() => this.C1.GetEdgeToNeighbor(this.C2);

    public bool CheckOrderInvariant(Func<MapCell, MapCell, bool> checkFn)
    {
      return checkFn(this.C1, this.C2) || checkFn(this.C2, this.C1);
    }

    public bool CheckOrderInvariant(
      Func<MapCell, MapCell, bool> checkFn,
      out MapCell c1,
      out MapCell c2)
    {
      if (checkFn(this.C1, this.C2))
      {
        c1 = this.C1;
        c2 = this.C2;
        return true;
      }
      c1 = this.C2;
      c2 = this.C1;
      return checkFn(this.C2, this.C1);
    }

    public bool Equals(CellEdge other)
    {
      return object.Equals((object) this.C1, (object) other.C1) && object.Equals((object) this.C2, (object) other.C2);
    }

    public override bool Equals(object obj)
    {
      return obj != null && obj is CellEdge other && this.Equals(other);
    }

    public override int GetHashCode() => Hash.Combine<MapCell, MapCell>(this.C1, this.C2);

    public static void Serialize(CellEdge value, BlobWriter writer)
    {
      MapCell.Serialize(value.C1, writer);
      MapCell.Serialize(value.C2, writer);
    }

    public static CellEdge Deserialize(BlobReader reader)
    {
      return new CellEdge(MapCell.Deserialize(reader), MapCell.Deserialize(reader));
    }
  }
}
