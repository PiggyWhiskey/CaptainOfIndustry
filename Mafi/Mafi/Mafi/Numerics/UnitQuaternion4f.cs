// Decompiled with JetBrains decompiler
// Type: Mafi.Numerics.UnitQuaternion4f
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Numerics
{
  /// <summary>
  /// A unit Quaternion that represents an arbitrary rotation in 3D. Multiplication of quaternions is equal to
  /// concatenation of the represented rotations. Multiplication is not commutative as it depends which rotation is
  /// applied first.
  /// </summary>
  /// <remarks>
  /// All methods of this class guarantee to return unit quaternion if performed on a unit quaternion (+- float
  /// imprecision). Unfortunately it is not possible in C# to guarantee that all quaternions are unit, for example
  /// default struct constructor will create zero quaternion.
  /// </remarks>
  [GenerateSerializer(false, null, 0)]
  public struct UnitQuaternion4f : IEquatable<UnitQuaternion4f>
  {
    /// <summary>
    /// Identity quaternion represents rotation of 0 degrees around arbitrary axis.
    /// </summary>
    public static UnitQuaternion4f Identity;
    /// <summary>
    /// Xyz components of the quaternion. This encodes the axis of the rotation but is not the axis as is. Use <see cref="M:Mafi.Numerics.UnitQuaternion4f.ToAxisAngle" /> to get the true angle and axis.
    /// </summary>
    public readonly Vector3f Xyz;
    /// <summary>
    /// W component of the quaternion. This encodes the angle but is not the angle as is. Use <see cref="M:Mafi.Numerics.UnitQuaternion4f.ToAxisAngle" /> to get the true angle and axis.
    /// </summary>
    public readonly Fix32 W;

    public static void Serialize(UnitQuaternion4f value, BlobWriter writer)
    {
      Vector3f.Serialize(value.Xyz, writer);
      Fix32.Serialize(value.W, writer);
    }

    public static UnitQuaternion4f Deserialize(BlobReader reader)
    {
      return new UnitQuaternion4f(Vector3f.Deserialize(reader), Fix32.Deserialize(reader));
    }

    /// <summary>
    /// Constructs a new unit Quaternion from x, y, z, and w components.
    /// </summary>
    public UnitQuaternion4f(Fix32 x, Fix32 y, Fix32 z, Fix32 w)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this = new UnitQuaternion4f(new Vector4f(x, y, z, w));
    }

    /// <summary>
    /// Constructs a new unit Quaternion from a 3D vector and w components.
    /// </summary>
    [LoadCtor]
    public UnitQuaternion4f(Vector3f xyz, Fix32 w)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this = new UnitQuaternion4f(new Vector4f(xyz, w));
    }

    /// <summary>Constructs a new unit Quaternion from a 4D vector.</summary>
    public UnitQuaternion4f(Vector4f vector)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      Fix32 length = vector.Length;
      if (length.IsNearZero())
      {
        Assert.Fail("Creating quaternion from zero vector.");
        this.Xyz = new Vector3f();
        this.W = Fix32.One;
      }
      else
      {
        vector /= length;
        this.Xyz = vector.Xyz;
        this.W = vector.W;
        Assert.That<bool>(this.IsNormalized).IsTrue("Normalization check needs larger epsilon.");
      }
    }

    /// <summary>Y component of the quaternion.</summary>
    public Fix32 X => this.Xyz.X;

    /// <summary>Y component of the quaternion.</summary>
    public Fix32 Y => this.Xyz.Y;

    /// <summary>Z component of the quaternion.</summary>
    public Fix32 Z => this.Xyz.Z;

    /// <summary>Returns 4D vector representing the quaternion.</summary>
    public Vector4f Vector4f => new Vector4f(this.Xyz, this.W);

    /// <summary>Gets the length (magnitude) of the quaternion.</summary>
    /// <seealso cref="P:Mafi.Numerics.UnitQuaternion4f.LengthSqr" />
    public Fix32 Length => this.LengthSqr.SqrtToFix32();

    /// <summary>Gets the square of the quaternion length (magnitude).</summary>
    public Fix64 LengthSqr => this.Xyz.LengthSqr + this.W.Squared();

    /// <summary>
    /// Whether this quaternion is normalized. All operations on the quaternion return normalized quaternion,
    /// however, there are ways how to obtain not normalized one like default struct ctor.
    /// </summary>
    public bool IsNormalized => this.LengthSqr.IsNear(Fix64.One, Fix64.EpsilonFix32NearOneSqr);

    /// <summary>Transforms given vector by this quaternion.</summary>
    /// <remarks>http://gamedev.stackexchange.com/questions/28395/rotating-vector3-by-a-quaternion</remarks>
    public Vector3f TransformVector(Vector3f v)
    {
      Fix64 fix64 = 2 * this.Xyz.Dot(v);
      Vector3f vector3f1 = fix64.ToFix32() * this.Xyz;
      Fix32 fix32_1 = this.W * this.W;
      fix64 = this.Xyz.Dot(this.Xyz);
      Fix32 fix32_2 = fix64.ToFix32();
      Vector3f vector3f2 = (fix32_1 - fix32_2) * v;
      return vector3f1 + vector3f2 + 2 * (this.W * this.Xyz.Cross(v));
    }

    /// <summary>
    /// Convert the current quaternion to axis angle representation.
    /// </summary>
    public void ToAxisAngle(out Vector3f axis, out AngleDegrees1f angle)
    {
      Vector4f axisAngle = this.ToAxisAngle();
      axis = axisAngle.Xyz;
      angle = AngleDegrees1f.FromRadians(axisAngle.W);
    }

    /// <summary>
    /// Convert this instance to an axis-angle representation. Axis is represented with XYZ components of the vector
    /// and angle is W component.
    /// </summary>
    [Pure]
    public Vector4f ToAxisAngle()
    {
      UnitQuaternion4f unitQuaternion4f = this;
      Assert.That<Fix32>(unitQuaternion4f.W.Abs()).IsLessOrEqual(Fix32.One);
      Fix32 w = Fix32.FromDouble(2.0 * Math.Acos(unitQuaternion4f.W.ToDouble()));
      Fix32 fix32 = ((Fix32) 1 - unitQuaternion4f.W * unitQuaternion4f.W).Sqrt();
      return fix32.IsPositive ? new Vector4f(this.Xyz / fix32, w) : new Vector4f(Fix32.One, Fix32.Zero, Fix32.Zero, w);
    }

    /// <summary>
    /// Returns normalized version of this quaternion. If length is near 0 an identity quaternion is returned.
    /// Renormalization might be needed if many operations are performed in a row.
    /// </summary>
    public UnitQuaternion4f Renormalized
    {
      get
      {
        Fix64 lengthSqr = this.LengthSqr;
        if (lengthSqr.IsNearZero())
        {
          Assert.Fail("Normalizing (near) zero vector.");
          return UnitQuaternion4f.Identity;
        }
        if (lengthSqr.IsNear(Fix64.One, Fix64.EpsilonFix32NearOneSqr))
          return this;
        Fix64 fix64_1 = lengthSqr.Sqrt();
        UnitQuaternion4f renormalized;
        ref UnitQuaternion4f local = ref renormalized;
        Fix64 fix64_2 = this.X / fix64_1;
        Fix32 fix32_1 = fix64_2.ToFix32();
        fix64_2 = this.Y / fix64_1;
        Fix32 fix32_2 = fix64_2.ToFix32();
        fix64_2 = this.Z / fix64_1;
        Fix32 fix32_3 = fix64_2.ToFix32();
        fix64_2 = this.W / fix64_1;
        Fix32 fix32_4 = fix64_2.ToFix32();
        local = new UnitQuaternion4f(fix32_1, fix32_2, fix32_3, fix32_4);
        Assert.That<bool>(renormalized.IsNormalized).IsTrue("Normalization failed, increase epsilon.");
        return renormalized;
      }
    }

    /// <summary>
    /// Returns conjugate of this quaternion. Conjugate for unit quaternions is equal to inverse.
    /// </summary>
    public UnitQuaternion4f Conjugated => new UnitQuaternion4f(-this.Xyz, this.W);

    /// <summary>Build a quaternion from the given axis and angle.</summary>
    /// <param name="axis">The axis to rotate about.</param>
    /// <param name="angle">The rotation angle in radians.</param>
    public static UnitQuaternion4f FromAxisAngle(Vector3f axis, AngleDegrees1f angle)
    {
      if (axis.LengthSqr.IsNearZero())
        return UnitQuaternion4f.Identity;
      angle /= 2;
      return new UnitQuaternion4f(axis.Normalized * angle.Sin().ToFix32(), angle.Cos().ToFix32()).Renormalized;
    }

    /// <summary>
    /// Returns shortest-arc rotation from vector u to vector v. The given vectors may be not normalized.
    /// </summary>
    /// <remarks>
    /// http://stackoverflow.com/questions/1171849/finding-quaternion-representing-the-rotation-from-one-vector-to-another
    /// </remarks>
    public static UnitQuaternion4f FromRotationBetweenVectors(Vector3f u, Vector3f v)
    {
      Fix64 fix64_1 = u.Dot(v);
      Fix64 fix64_2 = u.Length.MultAsFix64(v.Length);
      return (fix64_1 / fix64_2).IsNear(-Fix64.One) ? new UnitQuaternion4f(u.Orthogonal, Fix32.Zero) : new UnitQuaternion4f(u.Cross(v), (fix64_1 + fix64_2).ToFix32());
    }

    /// <summary>
    /// Do Spherical linear interpolation between this and given quaternions.
    /// </summary>
    [Pure]
    public UnitQuaternion4f Slerp(UnitQuaternion4f q, Percent t)
    {
      Assert.That<UnitQuaternion4f>(this).IsNormalized();
      Assert.That<UnitQuaternion4f>(q).IsNormalized();
      Fix64 fix64 = this.W.MultAsFix64(q.W) + this.Xyz.Dot(q.Xyz);
      if (fix64 >= Fix64.One || fix64 <= -Fix64.One)
        return this;
      if (fix64.IsNegative)
      {
        q = -q;
        fix64 = -fix64;
      }
      Percent percent1 = Percent.Hundred - t;
      Percent percent2;
      Percent percent3;
      if (fix64 < Fix64.FromFraction(99L, 100L))
      {
        Fix64 val = Fix64.FromDouble(Math.Acos(fix64.ToDouble()));
        Fix64 denominator = Fix64.FromDouble(Math.Sin(val.ToDouble()));
        percent2 = Percent.FromRatio(Fix64.FromDouble(Math.Sin(percent1.Apply(val).ToDouble())), denominator);
        percent3 = Percent.FromRatio(t.Apply(val), denominator);
      }
      else
      {
        percent2 = Percent.Hundred - t;
        percent3 = t;
      }
      return new UnitQuaternion4f(percent2 * this.Xyz + percent3 * q.Xyz, percent2.Apply(this.W) + percent3.Apply(q.W)).Renormalized;
    }

    /// <summary>
    /// Negates the quaternion by inverting the axis and angle. This does NOT change the rotation bu itself, just
    /// internal representation. For inverting the quaternions use <see cref="!:Conjugate" />.
    /// </summary>
    public static UnitQuaternion4f operator -(UnitQuaternion4f right)
    {
      return new UnitQuaternion4f(-right.Xyz, -right.W);
    }

    /// <summary>
    /// Multiplies two instances. This performs concatenation of the rotations represented by the unit quaternions.
    /// </summary>
    public static UnitQuaternion4f operator *(UnitQuaternion4f left, UnitQuaternion4f right)
    {
      return new UnitQuaternion4f(right.W * left.Xyz + left.W * right.Xyz + left.Xyz.Cross(right.Xyz), left.W * right.W - left.Xyz.Dot(right.Xyz).ToFix32());
    }

    public static bool operator ==(UnitQuaternion4f left, UnitQuaternion4f right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(UnitQuaternion4f left, UnitQuaternion4f right)
    {
      return !left.Equals(right);
    }

    public override string ToString()
    {
      return string.Format("V: {0}, W: {1}", (object) this.Xyz, (object) this.W);
    }

    public bool Equals(UnitQuaternion4f other) => this.Xyz == other.Xyz && this.W == other.W;

    public override bool Equals(object other)
    {
      return other is UnitQuaternion4f other1 && this.Equals(other1);
    }

    public override int GetHashCode() => Hash.Combine(this.Xyz.GetHashCode(), this.W.GetHashCode());

    static UnitQuaternion4f()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      UnitQuaternion4f.Identity = new UnitQuaternion4f(Fix32.Zero, Fix32.Zero, Fix32.Zero, Fix32.One);
    }
  }
}
