// Decompiled with JetBrains decompiler
// Type: Mafi.Numerics.Line3f
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Numerics
{
  /// <summary>2D line represented as two points.</summary>
  public struct Line3f
  {
    public readonly Vector3f From;
    public readonly Vector3f To;

    public Vector3f Direction => this.To - this.From;

    public Line3f(Vector3f from, Vector3f to)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      Assert.That<Vector3f>(from).IsNotEqualTo<Vector3f>(to);
      this.From = from;
      this.To = to;
    }

    [Pure]
    public Vector3f GetPoint(Percent t) => this.From.Lerp(this.To, t);

    /// <summary>
    /// Returns distance from this line segment to the given point.
    /// </summary>
    [Pure]
    public Fix32 DistanceLineSegmentToPt(Vector3f pt)
    {
      Vector3f direction = this.Direction;
      Fix64 numerator = direction.Dot(pt - this.From);
      if (numerator.IsNotPositive)
        return pt.DistanceTo(this.From);
      Fix64 lengthSqr = direction.LengthSqr;
      if (numerator >= lengthSqr)
        return pt.DistanceTo(this.To);
      Vector3f other = this.From.Lerp(this.To, Percent.FromRatio(numerator, lengthSqr));
      return pt.DistanceTo(other);
    }

    /// <summary>
    /// Returns squared distance from this line segment to the given point.
    /// </summary>
    [Pure]
    public Fix64 DistanceSqrToLineSegment(Vector3f pt)
    {
      if (this.To == this.From)
        return pt.DistanceSqrTo(this.To);
      Vector3f direction = this.Direction;
      Fix64 numerator = direction.Dot(pt - this.From);
      if (numerator.IsNotPositive)
        return pt.DistanceSqrTo(this.From);
      Fix64 lengthSqr = direction.LengthSqr;
      if (numerator >= lengthSqr)
        return pt.DistanceSqrTo(this.To);
      Vector3f other = this.From.Lerp(this.To, Percent.FromRatio(numerator, lengthSqr));
      return pt.DistanceSqrTo(other);
    }

    /// <summary>
    /// Returns distance squared from this line segment to the given point.
    /// </summary>
    [Pure]
    public Percent GetProjectionToLineSegment(Vector3f pt)
    {
      Vector3f direction = this.Direction;
      Fix64 numerator = direction.Dot(pt - this.From);
      if (numerator.IsNotPositive)
        return Percent.Zero;
      Fix64 lengthSqr = direction.LengthSqr;
      return numerator >= lengthSqr ? Percent.Hundred : Percent.FromRatio(numerator, lengthSqr);
    }
  }
}
