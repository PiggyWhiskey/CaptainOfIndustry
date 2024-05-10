// Decompiled with JetBrains decompiler
// Type: Mafi.Numerics.Line2f
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Numerics
{
  /// <summary>2D line represented as two points.</summary>
  [GenerateSerializer(false, null, 0)]
  public struct Line2f
  {
    public readonly Vector2f P0;
    public readonly Vector2f P1;

    public static void Serialize(Line2f value, BlobWriter writer)
    {
      Vector2f.Serialize(value.P0, writer);
      Vector2f.Serialize(value.P1, writer);
    }

    public static Line2f Deserialize(BlobReader reader)
    {
      return new Line2f(Vector2f.Deserialize(reader), Vector2f.Deserialize(reader));
    }

    public static Line2f FromPointDirection(Vector2f pt, Vector2f direction)
    {
      Assert.That<Vector2f>(direction).IsNotZero();
      return new Line2f(pt, pt + direction);
    }

    public Vector2f Direction => this.P1 - this.P0;

    [LoadCtor]
    public Line2f(Vector2f p0, Vector2f p1)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      Assert.That<Vector2f>(p0).IsNotEqualTo<Vector2f>(p1);
      this.P0 = p0;
      this.P1 = p1;
    }

    /// <summary>
    /// Returns intersection point of this line and other line. Returns null when lines are parallel.
    /// </summary>
    /// <remarks>https://en.wikipedia.org/wiki/Line%E2%80%93line_intersection#Intersection_of_two_lines</remarks>
    [Pure]
    public readonly Vector2f? Intersect(Line2f otherLine)
    {
      Fix32 fix32_1 = this.P0.X - this.P1.X;
      Fix64 fix64_1 = fix32_1.MultAsFix64(otherLine.P0.Y - otherLine.P1.Y);
      fix32_1 = this.P0.Y - this.P1.Y;
      Fix64 fix64_2 = fix32_1.MultAsFix64(otherLine.P0.X - otherLine.P1.X);
      Fix64 fix64_3 = fix64_1 - fix64_2;
      if (fix64_3.ToFix32().IsNearZero())
        return new Vector2f?();
      Fix64 fix64_4 = this.P0.PseudoCross(this.P1);
      Fix32 fix32_2 = fix64_4.DivToFix32(fix64_3);
      fix64_4 = otherLine.P0.PseudoCross(otherLine.P1);
      Fix32 fix32_3 = fix64_4.DivToFix32(fix64_3);
      return new Vector2f?(new Vector2f(fix32_2 * (otherLine.P0.X - otherLine.P1.X) - (this.P0.X - this.P1.X) * fix32_3, fix32_2 * (otherLine.P0.Y - otherLine.P1.Y) - (this.P0.Y - this.P1.Y) * fix32_3));
    }

    /// <summary>
    /// Returns intersection point of this line segment and other line segment. Returns null when there is no intersection.
    /// </summary>
    /// <remarks>https://stackoverflow.com/questions/563198/how-do-you-detect-where-two-line-segments-intersect</remarks>
    [Pure]
    public readonly bool IntersectsWithSegment(Line2f otherLine)
    {
      Vector2f vector2f1 = this.P1 - this.P0;
      Vector2f vector2f2 = otherLine.P1 - otherLine.P0;
      Fix32 fix32_1 = -vector2f2.X * vector2f1.Y + vector2f1.X * vector2f2.Y;
      if (fix32_1.IsNearZero())
        return false;
      Fix32 fix32_2 = -vector2f1.Y * (this.P0.X - otherLine.P0.X) + vector2f1.X * (this.P0.Y - otherLine.P0.Y);
      Fix32 fix32_3 = vector2f2.X * (this.P0.Y - otherLine.P0.Y) - vector2f2.Y * (this.P0.X - otherLine.P0.X);
      if (fix32_1 < 0)
      {
        fix32_2 = -fix32_2;
        fix32_3 = -fix32_3;
      }
      return fix32_2 >= 0 && fix32_2 <= fix32_1 && fix32_3 >= 0 && fix32_3 <= fix32_1;
    }

    [Pure]
    public Fix32 DistanceToLineSegment(Vector2f pt)
    {
      return this.DistanceSqrToLineSegment(pt).SqrtToFix32();
    }

    /// <summary>
    /// Returns squared distance from the given point to a line segment specified by the <see cref="F:Mafi.Numerics.Line2f.P0" /> and <see cref="F:Mafi.Numerics.Line2f.P1" />.
    /// </summary>
    [Pure]
    public Fix64 DistanceSqrToLineSegment(Vector2f pt)
    {
      return this.DistanceSqrToLineSegment(pt, out Percent _);
    }

    /// <summary>
    /// Returns squared distance from the given point to a line segment specified by the <see cref="F:Mafi.Numerics.Line2f.P0" /> and <see cref="F:Mafi.Numerics.Line2f.P1" />. Also provides the distance along the line of the closest point.
    /// </summary>
    [Pure]
    public Fix64 DistanceSqrToLineSegment(Vector2f pt, out Percent distanceAlongLine)
    {
      if (this.P0 == this.P1)
      {
        distanceAlongLine = Percent.Fifty;
        return pt.DistanceSqrTo(this.P0);
      }
      Vector2f direction = this.Direction;
      Fix64 numerator = direction.Dot(pt - this.P0);
      if (numerator.IsNegative)
      {
        distanceAlongLine = Percent.Zero;
        return pt.DistanceSqrTo(this.P0);
      }
      Fix64 lengthSqr = direction.LengthSqr;
      if (numerator > lengthSqr)
      {
        distanceAlongLine = Percent.Hundred;
        return pt.DistanceSqrTo(this.P1);
      }
      distanceAlongLine = Percent.FromRatio(numerator, lengthSqr);
      Vector2f other = this.P0.Lerp(this.P1, distanceAlongLine);
      return pt.DistanceSqrTo(other);
    }

    /// <summary>
    /// Returns distance from the given point to a line specified by the <see cref="F:Mafi.Numerics.Line2f.P0" /> and <see cref="F:Mafi.Numerics.Line2f.P1" />.
    /// </summary>
    [Pure]
    public Fix32 DistanceToLine(Vector2f pt)
    {
      if (this.P0 == this.P1)
        return pt.DistanceTo(this.P0);
      Vector2f direction = this.Direction;
      Vector2f other = this.P0.Lerp(this.P1, Percent.FromRatio(direction.Dot(pt - this.P0), direction.LengthSqr));
      return pt.DistanceTo(other);
    }

    /// <summary>
    /// Returns signed distance to the given point. Positive distances are for points to the left of the line.
    /// </summary>
    [Pure]
    public Fix32 SignedDistanceToLine(Vector2f pt)
    {
      return this.DistanceToLine(pt) * this.IsToTheLeft(pt);
    }

    [Pure]
    public int IsToTheLeft(Vector2f p) => this.Direction.PseudoCross(p - this.P0).Sign();

    /// <summary>
    /// Returns <c>t</c> in range [0, 1] that represents closest point on the line to given point.
    /// </summary>
    [Pure]
    public Percent GetClosestTToLineSegment(Vector2f pt)
    {
      if (this.P0 == this.P1)
        return Percent.Fifty;
      Vector2f direction = this.Direction;
      Fix64 numerator = direction.Dot(pt - this.P0);
      if (numerator.IsNotPositive)
        return Percent.Zero;
      Fix64 lengthSqr = direction.LengthSqr;
      return numerator > lengthSqr ? Percent.Hundred : Percent.FromRatio(numerator, lengthSqr);
    }

    public override string ToString()
    {
      return string.Format("{0} -> {1} ({2}°)", (object) this.P0, (object) this.P1, (object) this.Direction.Angle.Degrees);
    }
  }
}
