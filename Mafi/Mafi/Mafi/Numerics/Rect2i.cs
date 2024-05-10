// Decompiled with JetBrains decompiler
// Type: Mafi.Numerics.Rect2i
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Numerics
{
  /// <summary>
  /// 2D rectangle that is represented as min and max points. Min is inclusive, max is exclusive.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public struct Rect2i
  {
    /// <summary>Inclusive min coordinate.</summary>
    public readonly Vector2i Min;
    /// <summary>Exclusive max coordinate.</summary>
    public readonly Vector2i Max;

    public static void Serialize(Rect2i value, BlobWriter writer)
    {
      Vector2i.Serialize(value.Min, writer);
      Vector2i.Serialize(value.Max, writer);
    }

    public static Rect2i Deserialize(BlobReader reader)
    {
      return new Rect2i(Vector2i.Deserialize(reader), Vector2i.Deserialize(reader));
    }

    public Vector2i Size => this.Max - this.Min;

    public Rect2i(Vector2i min, Vector2i max)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      Assert.That<bool>(min < max).IsTrue("Incorrectly sorted Rect2i points. Wanted to use `FromUnordered`?");
      this.Min = min;
      this.Max = max;
    }

    public static Rect2i FromUnordered(Vector2i corner1, Vector2i corner2)
    {
      return new Rect2i(corner1.Min(corner2), corner1.Max(corner2));
    }

    /// <summary>Whether given rectangle intersects this rectangle.</summary>
    [Pure]
    public bool Intersects(Rect2i other)
    {
      return this.Min.X < other.Max.X && this.Max.X > other.Min.X && this.Min.Y < other.Max.Y && this.Max.Y > other.Min.Y;
    }

    /// <summary>Whether given point is contained in this rectangle.</summary>
    [Pure]
    public bool Contains(Vector2i pt)
    {
      return pt.X >= this.Min.X && pt.X < this.Max.X && pt.Y >= this.Min.Y && pt.Y < this.Max.Y;
    }

    /// <summary>
    /// Returns distance from point inside of rectangle to the boundary.
    /// </summary>
    [Pure]
    public int InsideDistanceToBoundary(Vector2i pt)
    {
      Assert.That<bool>(this.Contains(pt)).IsTrue("Point is not inside!");
      return (pt.X - this.Min.X).Min(this.Max.X - pt.X - 1).Min(pt.Y - this.Min.Y).Min(this.Max.Y - pt.Y - 1);
    }

    /// <summary>
    /// Extends this rectangle in all directions by given value.
    /// </summary>
    [Pure]
    public Rect2i ExtendedBy(int radius) => new Rect2i(this.Min - radius, this.Max + radius);
  }
}
