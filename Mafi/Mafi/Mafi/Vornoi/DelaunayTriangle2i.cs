// Decompiled with JetBrains decompiler
// Type: Mafi.Vornoi.DelaunayTriangle2i
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Vornoi
{
  /// <summary>A 2D triangle with integer coordinates.</summary>
  /// <remarks>This class has been optimized to use in Delaunay triangulation algorithm</remarks>
  public class DelaunayTriangle2i
  {
    public readonly Vector2i P0;
    public readonly int I0;
    public readonly Vector2i P1;
    public readonly int I1;
    public readonly Vector2i P2;
    public readonly int I2;
    /// <summary>
    /// Cached value of 2 * area of this triangle. We intentionally store it as double to keep it integer.
    /// </summary>
    public readonly int TwiceArea;

    public DelaunayTriangle2i(Vector2i p0, int i0, Vector2i p1, int i1, Vector2i p2, int i2)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Assert.That<Vector2i>(p0).IsNotEqualTo<Vector2i>(p1);
      Assert.That<int>(i0).IsNotEqualTo(i1);
      Assert.That<Vector2i>(p0).IsNotEqualTo<Vector2i>(p2);
      Assert.That<int>(i0).IsNotEqualTo(i2);
      Assert.That<Vector2i>(p1).IsNotEqualTo<Vector2i>(p2);
      Assert.That<int>(i1).IsNotEqualTo(i2);
      this.P0 = p0;
      this.I0 = i0;
      this.P1 = p1;
      this.I1 = i1;
      this.P2 = p2;
      this.I2 = i2;
      this.TwiceArea = -p1.Y * p2.X + p0.Y * (-p1.X + p2.X) + p0.X * (p1.Y - p2.Y) + p1.X * p2.Y;
      Assert.That<int>(this.TwiceArea).IsPositive("Triangle does not obey counter-clock wise point ordering!");
    }

    public Vector2i E0 => new Vector2i(this.I0, this.I1);

    public Vector2i E1 => new Vector2i(this.I1, this.I2);

    public Vector2i E2 => new Vector2i(this.I2, this.I0);

    public Vector2i E0Canonic => new Vector2i(this.I0.Min(this.I1), this.I0.Max(this.I1));

    public Vector2i E1Canonic => new Vector2i(this.I1.Min(this.I2), this.I1.Max(this.I2));

    public Vector2i E2Canonic => new Vector2i(this.I2.Min(this.I0), this.I2.Max(this.I0));

    /// <summary>
    /// Whether given point is in the triangle or on its edge.
    /// </summary>
    /// <remarks>
    /// We compute <c>s` = s * 2 Area</c> and <c>t` = t * 2 Area</c> to avoid division. This won't have affect the
    /// positivity check of <c>s</c> and <c>t</c>. Finally, instead of checking positive <c>1-s-t</c> we check
    /// equivalent condition of <c>s` + t`</c> is greater than <c>2 * Area</c>.
    /// http://stackoverflow.com/questions/2049582/how-to-determine-if-a-point-is-in-a-2d-triangle
    /// </remarks>
    public bool Contains(Vector2i p)
    {
      int num1 = this.P0.Y * this.P2.X - this.P0.X * this.P2.Y + (this.P2.Y - this.P0.Y) * p.X + (this.P0.X - this.P2.X) * p.Y;
      if (num1 < 0)
        return false;
      int num2 = this.P0.X * this.P1.Y - this.P0.Y * this.P1.X + (this.P0.Y - this.P1.Y) * p.X + (this.P1.X - this.P0.X) * p.Y;
      return num2 >= 0 && num1 + num2 <= this.TwiceArea;
    }

    /// <summary>
    /// Whether given point is inside the circumcircle of this triangle but not exactly on it.
    /// </summary>
    /// <remarks>https://en.wikipedia.org/wiki/Delaunay_triangulation#Algorithms</remarks>
    public bool IsInCircumcircle(Vector2i p)
    {
      long num1 = (long) (this.P0.X - p.X);
      long num2 = (long) (this.P0.Y - p.Y);
      long num3 = (long) (this.P0.X * this.P0.X - p.X * p.X + this.P0.Y * this.P0.Y - p.Y * p.Y);
      long num4 = (long) (this.P1.X - p.X);
      long num5 = (long) (this.P1.Y - p.Y);
      long num6 = (long) (this.P1.X * this.P1.X - p.X * p.X + this.P1.Y * this.P1.Y - p.Y * p.Y);
      long num7 = (long) (this.P2.X - p.X);
      long num8 = (long) (this.P2.Y - p.Y);
      long num9 = (long) (this.P2.X * this.P2.X - p.X * p.X + this.P2.Y * this.P2.Y - p.Y * p.Y);
      return num1 * (num5 * num9 - num6 * num8) - num2 * (num4 * num9 - num6 * num7) + num3 * (num4 * num8 - num5 * num7) > 0L;
    }
  }
}
