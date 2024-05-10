// Decompiled with JetBrains decompiler
// Type: Mafi.Numerics.Line2i
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
  public readonly struct Line2i
  {
    public readonly Vector2i P0;
    public readonly Vector2i P1;

    public static void Serialize(Line2i value, BlobWriter writer)
    {
      Vector2i.Serialize(value.P0, writer);
      Vector2i.Serialize(value.P1, writer);
    }

    public static Line2i Deserialize(BlobReader reader)
    {
      return new Line2i(Vector2i.Deserialize(reader), Vector2i.Deserialize(reader));
    }

    public Line2i(Vector2i p0, Vector2i p1)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.P0 = p0;
      this.P1 = p1;
    }

    public Vector2i Direction => this.P1 - this.P0;

    public Vector2f DirectionNormalized => (this.P1 - this.P0).Vector2f.Normalized;

    public Vector2i CenterPt => this.P0.Average(this.P1);

    public Fix32 SegmentLength => this.P0.DistanceTo(this.P1);

    public Line2f Line2f
    {
      get
      {
        Vector2i vector2i = this.P0;
        Vector2f vector2f1 = vector2i.Vector2f;
        vector2i = this.P1;
        Vector2f vector2f2 = vector2i.Vector2f;
        return new Line2f(vector2f1, vector2f2);
      }
    }

    /// <summary>
    /// Returns point that is linearly interpolated between <see cref="F:Mafi.Numerics.Line2i.P0" /> and <see cref="F:Mafi.Numerics.Line2i.P1" /> based on <c>t</c>.
    /// </summary>
    [Pure]
    public Vector2f GetPoint(Percent t) => this.P0.Vector2f.Lerp(this.P1.Vector2f, t);

    /// <summary>
    /// Returns distance from the given point to a line segment specified by the <see cref="F:Mafi.Numerics.Line2i.P0" /> and <see cref="F:Mafi.Numerics.Line2i.P1" />.
    /// </summary>
    [Pure]
    public Fix32 DistanceToLineSegment(Vector2i pt)
    {
      return this.DistanceSqrToLineSegment(pt).SqrtToFix32();
    }

    /// <summary>
    /// Returns squared distance from the given point to a line segment specified by the <see cref="F:Mafi.Numerics.Line2i.P0" /> and <see cref="F:Mafi.Numerics.Line2i.P1" />. This is more efficient than <see cref="M:Mafi.Numerics.Line2i.DistanceToLineSegment(Mafi.Vector2i)" /> as it avoids square root
    /// computation.
    /// </summary>
    [Pure]
    public Fix64 DistanceSqrToLineSegment(Vector2i pt)
    {
      Vector2i direction = this.Direction;
      long numerator = direction.Dot(pt - this.P0);
      if (numerator <= 0L)
        return (Fix64) pt.DistanceSqrTo(this.P0);
      long lengthSqr = direction.LengthSqr;
      if (numerator >= lengthSqr)
        return (Fix64) pt.DistanceSqrTo(this.P1);
      Vector2f fix32 = this.P0.LerpToFix32(this.P1, Percent.FromRatio(numerator, lengthSqr));
      return pt.Vector2f.DistanceSqrTo(fix32);
    }

    [Pure]
    public long DistanceSqrToLineSegmentApprox(Vector2i pt)
    {
      Vector2i direction = this.Direction;
      long t = direction.Dot(pt - this.P0);
      if (t <= 0L)
        return pt.DistanceSqrTo(this.P0);
      long lengthSqr = direction.LengthSqr;
      if (t >= lengthSqr)
        return pt.DistanceSqrTo(this.P1);
      Vector2i other = this.P0.Lerp(this.P1, t, lengthSqr);
      return pt.DistanceSqrTo(other);
    }

    [Pure]
    public Fix32 SignedDistanceToLineSegment(Vector2i pt)
    {
      Vector2i direction = this.Direction;
      Vector2i vector2i = pt - this.P0;
      long num = direction.Dot(vector2i);
      long self = this.Direction.PseudoCross(vector2i);
      if (num <= 0L)
        return pt.DistanceTo(this.P0) * self.SignNoZero();
      long lengthSqr = direction.LengthSqr;
      return num >= lengthSqr ? pt.DistanceTo(this.P1) * self.SignNoZero() : self.ToFix64().DivToFix32(lengthSqr.Sqrt());
    }

    [Pure]
    public Vector2i ClosestPointToLineSegment(Vector2i pt)
    {
      Vector2i direction = this.Direction;
      long numerator = direction.Dot(pt - this.P0);
      if (numerator <= 0L)
        return this.P0;
      long lengthSqr = direction.LengthSqr;
      return numerator >= lengthSqr ? this.P1 : this.P0.Lerp(this.P1, Percent.FromRatio(numerator, lengthSqr));
    }

    [Pure]
    public Vector2f ClosestPointToLineSegment(Vector2f pt)
    {
      Vector2f vector2f = this.Direction.Vector2f;
      Fix64 numerator = vector2f.Dot(pt - this.P0);
      if (numerator.IsNotPositive)
        return this.P0.Vector2f;
      Fix64 lengthSqr = vector2f.LengthSqr;
      return numerator >= lengthSqr ? this.P1.Vector2f : this.P0.Vector2f.Lerp(this.P1.Vector2f, Percent.FromRatio(numerator, lengthSqr));
    }

    /// <summary>
    /// Returns <c>t</c> in range [0, 1] that represents a closest point on the line segment to the given point.
    /// </summary>
    [Pure]
    public Percent GetClosestTToLineSegment(Vector2i pt)
    {
      Vector2i direction = this.Direction;
      long numerator = direction.Dot(pt - this.P0);
      if (numerator <= 0L)
        return Percent.Zero;
      long lengthSqr = direction.LengthSqr;
      return numerator > lengthSqr ? Percent.Hundred : Percent.FromRatio(numerator, lengthSqr);
    }

    /// <summary>
    /// Returns <c>t</c> that represents a closest point on the (infinite) line to the given point.
    /// </summary>
    [Pure]
    public Percent GetClosestTToLine(Vector2i pt)
    {
      Vector2i direction = this.Direction;
      return Percent.FromRatio(direction.Dot(pt - this.P0), direction.LengthSqr);
    }

    [Pure]
    public LineRasterizer IterateRasterizedPoints(bool skipFirstPoint = false)
    {
      return new LineRasterizer(this.P0, this.P1, skipFirstPoint);
    }

    /// <summary>
    /// Returns parameter T that captures intersection point <c>P = P0 + T * Direction</c>. If lines are parallel,
    /// null is returned. If returned T is within [0, 1] range, the intersection point in on this line segment
    /// (not necessarily on <paramref name="other" /> line segment tough). Null is returned when lines are parallel.
    /// </summary>
    /// <remarks>https://stackoverflow.com/a/565282/1030376</remarks>
    [Pure]
    public Percent? IntersectionT(Line2i other)
    {
      Vector2i direction1 = this.Direction;
      Vector2i direction2 = other.Direction;
      long denominator = direction1.PseudoCross(direction2);
      return denominator == 0L ? new Percent?() : new Percent?(Percent.FromRatio((other.P0 - this.P0).PseudoCross(direction2), denominator));
    }

    public override string ToString()
    {
      return string.Format("{0} to {1}", (object) this.P0, (object) this.P1);
    }
  }
}
