// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.Chunk64Area
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  public readonly struct Chunk64Area
  {
    public readonly Chunk2i Origin;
    public readonly Vector2i Size;

    public int TotalChunksCount => this.Size.ProductInt;

    public RectangleTerrainArea2i Area2i
    {
      get
      {
        return new RectangleTerrainArea2i(this.Origin.Tile2i, new RelTile2i(this.Size.X * 64, this.Size.Y * 64));
      }
    }

    public Chunk64Area(Chunk2i origin, Vector2i chunksCount)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Origin = origin;
      this.Size = chunksCount;
    }

    public bool Contains(Chunk2i chunk) => chunk >= this.Origin && chunk < this.Origin + this.Size;

    public Chunk64Area ExtendBy(int chunksCount)
    {
      return new Chunk64Area(this.Origin - chunksCount, this.Size + 2 * chunksCount);
    }

    public Chunk64Area ClampToTerrain(TerrainManager terrainManager)
    {
      return this.Intersect(terrainManager.TerrainAreaChunks);
    }

    public Chunk64Area Intersect(Chunk64Area otherArea)
    {
      Chunk2i origin = this.Origin.Max(otherArea.Origin);
      Chunk2i chunk2i = (this.Origin + this.Size).Min(otherArea.Origin + otherArea.Size);
      return new Chunk64Area(origin, (chunk2i - origin).Max(Vector2i.Zero));
    }

    public Chunk64Area Union(Chunk64Area otherArea)
    {
      Chunk2i origin = this.Origin.Min(otherArea.Origin);
      Chunk2i chunk2i = (this.Origin + this.Size).Max(otherArea.Origin + otherArea.Size);
      return new Chunk64Area(origin, chunk2i - origin);
    }

    [Pure]
    public IEnumerable<Chunk2i> EnumerateChunks()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerable<Chunk2i>) new Chunk64Area.\u003CEnumerateChunks\u003Ed__12(-2)
      {
        \u003C\u003E3__\u003C\u003E4__this = this
      };
    }

    public static bool operator ==(Chunk64Area lhs, Chunk64Area rhs) => lhs.Equals(rhs);

    public static bool operator !=(Chunk64Area lhs, Chunk64Area rhs) => !lhs.Equals(rhs);

    public bool Equals(Chunk64Area other) => this.Origin == other.Origin && this.Size == other.Size;

    public override bool Equals(object obj) => obj is Chunk64Area other && this.Equals(other);

    public override int GetHashCode() => Hash.Combine<Chunk2i, Vector2i>(this.Origin, this.Size);

    public override string ToString()
    {
      return string.Format("Chunk area {0}+{1}", (object) this.Origin, (object) this.Size);
    }
  }
}
