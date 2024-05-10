// Decompiled with JetBrains decompiler
// Type: Mafi.Numerics.Line3i
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Numerics
{
  /// <summary>3D line represented as two points.</summary>
  [GenerateSerializer(false, null, 0)]
  public readonly struct Line3i
  {
    public readonly Vector3i P0;
    public readonly Vector3i P1;

    public static void Serialize(Line3i value, BlobWriter writer)
    {
      Vector3i.Serialize(value.P0, writer);
      Vector3i.Serialize(value.P1, writer);
    }

    public static Line3i Deserialize(BlobReader reader)
    {
      return new Line3i(Vector3i.Deserialize(reader), Vector3i.Deserialize(reader));
    }

    public Line3i(Vector3i p0, Vector3i p1)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.P0 = p0;
      this.P1 = p1;
    }

    public Vector3i Direction => this.P1 - this.P0;

    public Vector3f DirectionNormalized => (this.P1 - this.P0).Vector3f.Normalized;

    public Vector3i CenterPt => this.P0.Average(this.P1);

    public Fix32 SegmentLength => this.P0.DistanceTo(this.P1);

    public Line3f Line3f
    {
      get
      {
        Vector3i vector3i = this.P0;
        Vector3f vector3f1 = vector3i.Vector3f;
        vector3i = this.P1;
        Vector3f vector3f2 = vector3i.Vector3f;
        return new Line3f(vector3f1, vector3f2);
      }
    }

    /// <summary>
    /// Returns squared distance from the given point to a line segment specified by the <see cref="F:Mafi.Numerics.Line3i.P0" /> and <see cref="F:Mafi.Numerics.Line3i.P1" />. This is more efficient than <see cref="!:DistanceToLineSegment" /> as it avoids square root
    /// computation.
    /// </summary>
    [Pure]
    public Fix64 DistanceSqrToLineSegment(Vector3i pt)
    {
      Vector3i direction = this.Direction;
      long numerator = direction.Dot(pt - this.P0);
      if (numerator <= 0L)
        return (Fix64) pt.DistanceSqrTo(this.P0);
      long lengthSqr = direction.LengthSqr;
      if (numerator >= lengthSqr)
        return (Fix64) pt.DistanceSqrTo(this.P1);
      Vector3f fix32 = this.P0.LerpToFix32(this.P1, Percent.FromRatio(numerator, lengthSqr));
      return pt.Vector3f.DistanceSqrTo(fix32);
    }

    public override string ToString()
    {
      return string.Format("{0} to {1}", (object) this.P0, (object) this.P1);
    }
  }
}
