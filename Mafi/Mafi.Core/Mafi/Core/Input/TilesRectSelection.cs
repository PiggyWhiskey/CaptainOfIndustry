// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Input.TilesRectSelection
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Input
{
  [GenerateSerializer(false, null, 0)]
  public struct TilesRectSelection
  {
    public readonly Tile2i Origin;
    public readonly RelTile2i Size;

    public TilesRectSelection(Tile2i origin, RelTile2i size)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Origin = origin;
      this.Size = size;
    }

    public int Area => this.Size.ProductInt;

    public int VerticesCount => (this.Size.X + 1) * (this.Size.Y + 1);

    public Tile2i Center => this.Origin + this.Size / 2;

    public TilesRectSelection SetOrigin(Tile2i newOrigin)
    {
      return new TilesRectSelection(newOrigin, this.Size);
    }

    public TilesRectSelection SetSize(RelTile2i newSize)
    {
      return new TilesRectSelection(this.Origin, newSize);
    }

    public TilesRectSelection AddSize(RelTile2i additionalSize)
    {
      return new TilesRectSelection(this.Origin, this.Size + additionalSize);
    }

    /// <summary>
    /// Enumerates vertices of this area rather than tile coordinates that are enumerated directly of this type.
    /// </summary>
    [Pure]
    public TilesRectSelection EnumerateVertices()
    {
      return new TilesRectSelection(this.Origin, this.Size + RelTile2i.One);
    }

    public TilesRectSelection.Enumerator GetEnumerator() => new TilesRectSelection.Enumerator(this);

    public static bool operator ==(TilesRectSelection lhs, TilesRectSelection rhs)
    {
      return lhs.Origin == rhs.Origin && lhs.Size == rhs.Size;
    }

    public static bool operator !=(TilesRectSelection lhs, TilesRectSelection rhs)
    {
      return lhs.Origin != rhs.Origin || lhs.Size != rhs.Size;
    }

    public override int GetHashCode() => this.Origin.GetHashCode() * 13 + this.Size.GetHashCode();

    public override bool Equals(object obj)
    {
      return obj is TilesRectSelection tilesRectSelection && tilesRectSelection == this;
    }

    public static void Serialize(TilesRectSelection value, BlobWriter writer)
    {
      Tile2i.Serialize(value.Origin, writer);
      RelTile2i.Serialize(value.Size, writer);
    }

    public static TilesRectSelection Deserialize(BlobReader reader)
    {
      return new TilesRectSelection(Tile2i.Deserialize(reader), RelTile2i.Deserialize(reader));
    }

    public struct Enumerator
    {
      private readonly Tile2i m_origin;
      private readonly RelTile2i m_size;
      private int m_x;
      private int m_y;

      internal Enumerator(TilesRectSelection trs)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.m_origin = trs.Origin;
        this.m_size = trs.Size;
        this.m_x = 0;
        this.m_y = 0;
        this.Current = Tile2i.Zero;
      }

      public Tile2i Current { get; private set; }

      public bool MoveNext()
      {
        if (this.m_x >= this.m_size.X)
        {
          this.m_x = 0;
          ++this.m_y;
        }
        this.Current = new Tile2i(this.m_origin.X + this.m_x, this.m_origin.Y + this.m_y);
        ++this.m_x;
        return this.m_y < this.m_size.Y;
      }
    }
  }
}
