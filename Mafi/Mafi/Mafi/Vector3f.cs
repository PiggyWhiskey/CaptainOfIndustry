// Decompiled with JetBrains decompiler
// Type: Mafi.Vector3f
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;
using System.Diagnostics;
using System.Text;

#nullable disable
namespace Mafi
{
  /// <summary>Immutable 3D float vector.</summary>
  /// <remarks>
  /// This is partial struct and this file should contain only specific members for <see cref="T:Mafi.Vector3f" />. All general
  /// members should be added to the generator T4 template.
  /// </remarks>
  [DebuggerDisplay("({X}, {Y}, {Z})")]
  [DebuggerStepThrough]
  [ManuallyWrittenSerialization]
  public struct Vector3f : IEquatable<Vector3f>, IComparable<Vector3f>
  {
    /// <summary>Vector (0, 0, 0).</summary>
    public static readonly Vector3f Zero;
    /// <summary>Vector (1, 1, 1).</summary>
    public static readonly Vector3f One;
    /// <summary>Vector (1, 0, 0).</summary>
    public static readonly Vector3f UnitX;
    /// <summary>Vector (0, 1, 0).</summary>
    public static readonly Vector3f UnitY;
    /// <summary>Vector (0, 0, 1).</summary>
    public static readonly Vector3f UnitZ;
    /// <summary>
    /// Vector (Fix32.MinValue, Fix32.MinValue, Fix32.MinValue).
    /// </summary>
    public static readonly Vector3f MinValue;
    /// <summary>
    /// Vector (Fix32.MaxValue, Fix32.MaxValue, Fix32.MaxValue).
    /// </summary>
    public static readonly Vector3f MaxValue;
    /// <summary>The X component of this vector.</summary>
    public readonly Fix32 X;
    /// <summary>The Y component of this vector.</summary>
    public readonly Fix32 Y;
    /// <summary>The Z component of this vector.</summary>
    public readonly Fix32 Z;

    /// <summary>Creates new Vector3f from raw components.</summary>
    public Vector3f(Fix32 x, Fix32 y, Fix32 z)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.X = x;
      this.Y = y;
      this.Z = z;
    }

    /// <summary>
    /// Creates new Vector3f from Vector2f and raw components.
    /// </summary>
    public Vector3f(Vector2f vector, Fix32 z)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.X = vector.X;
      this.Y = vector.Y;
      this.Z = z;
    }

    /// <summary>Gets the first two components of this vector.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector2f Xy => new Vector2f(this.X, this.Y);

    /// <summary>Sum of all components of this vector.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Fix32 Sum => this.X + this.Y + this.Z;

    /// <summary>
    /// Euclidean length of this vector.
    /// PERF: Expensive, uses sqrt. Consider using <see cref="P:Mafi.Vector3f.LengthSqr" /> whenever possible (when comparing
    /// lengths, etc.).
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Fix32 Length => this.LengthSqr.SqrtToFix32();

    /// <summary>
    /// Euclidean length squared of this vector.
    /// PERF: Cheaper than <see cref="P:Mafi.Vector3f.Length" />, does not require expensive sqrt.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Fix64 LengthSqr
    {
      get => this.X.MultAsFix64(this.X) + this.Y.MultAsFix64(this.Y) + this.Z.MultAsFix64(this.Z);
    }

    /// <summary>Whether this vector has all components equal to zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsZero => this.X.IsZero && this.Y.IsZero && this.Z.IsZero;

    /// <summary>e
    /// Whether this vector has at least one components not equal to zero.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNotZero => this.X.IsNotZero || this.Y.IsNotZero || this.Z.IsNotZero;

    /// <summary>Returns new vector with changed X component.</summary>
    [Pure]
    public Vector3f SetX(Fix32 newX) => new Vector3f(newX, this.Y, this.Z);

    /// <summary>Returns new vector with changed Y component.</summary>
    [Pure]
    public Vector3f SetY(Fix32 newY) => new Vector3f(this.X, newY, this.Z);

    /// <summary>Returns new vector with changed Z component.</summary>
    [Pure]
    public Vector3f SetZ(Fix32 newZ) => new Vector3f(this.X, this.Y, newZ);

    /// <summary>Returns new vector with changed X and Y components.</summary>
    [Pure]
    public Vector3f SetXy(Fix32 newX, Fix32 newY) => new Vector3f(newX, newY, this.Z);

    /// <summary>Returns new vector with changed X and Y components.</summary>
    [Pure]
    public Vector3f SetXy(Vector2f newXy) => new Vector3f(newXy.X, newXy.Y, this.Z);

    /// <summary>
    /// Returns new vector with changed X, Y, and Z components.
    /// </summary>
    [Pure]
    public Vector3f SetXyz(Fix32 newX, Fix32 newY, Fix32 newZ) => new Vector3f(newX, newY, newZ);

    /// <summary>Extends this vector a new component.</summary>
    [Pure]
    public Vector4f ExtendW(Fix32 w) => new Vector4f(this.X, this.Y, this.Z, w);

    /// <summary>Returns new vector with incremented X component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector3f IncrementX => new Vector3f(this.X + (Fix32) 1, this.Y, this.Z);

    /// <summary>Returns new vector with incremented Y component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector3f IncrementY => new Vector3f(this.X, this.Y + (Fix32) 1, this.Z);

    /// <summary>Returns new vector with incremented Z component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector3f IncrementZ => new Vector3f(this.X, this.Y, this.Z + (Fix32) 1);

    /// <summary>Returns new vector with decremented X component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector3f DecrementX => new Vector3f(this.X - (Fix32) 1, this.Y, this.Z);

    /// <summary>Returns new vector with decremented Y component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector3f DecrementY => new Vector3f(this.X, this.Y - (Fix32) 1, this.Z);

    /// <summary>Returns new vector with decremented Z component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector3f DecrementZ => new Vector3f(this.X, this.Y, this.Z - (Fix32) 1);

    /// <summary>
    /// Returns new vector with given value added to the X component.
    /// </summary>
    [Pure]
    public Vector3f AddX(Fix32 addedX) => new Vector3f(this.X + addedX, this.Y, this.Z);

    /// <summary>
    /// Returns new vector with given value added to the Y component.
    /// </summary>
    [Pure]
    public Vector3f AddY(Fix32 addedY) => new Vector3f(this.X, this.Y + addedY, this.Z);

    /// <summary>
    /// Returns new vector with given value added to the Z component.
    /// </summary>
    [Pure]
    public Vector3f AddZ(Fix32 addedZ) => new Vector3f(this.X, this.Y, this.Z + addedZ);

    /// <summary>
    /// Returns new vector with given value added to all components.
    /// </summary>
    [Pure]
    public Vector3f AddXyz(Fix32 addedValue)
    {
      return new Vector3f(this.X + addedValue, this.Y + addedValue, this.Z + addedValue);
    }

    /// <summary>
    /// Returns new vector with given value multiplied with the X component.
    /// </summary>
    [Pure]
    public Vector3f MultiplyX(Fix32 multX) => new Vector3f(this.X * multX, this.Y, this.Z);

    /// <summary>
    /// Returns new vector with given value multiplied with the Y component.
    /// </summary>
    [Pure]
    public Vector3f MultiplyY(Fix32 multY) => new Vector3f(this.X, this.Y * multY, this.Z);

    /// <summary>
    /// Returns new vector with given value multiplied with the Z component.
    /// </summary>
    [Pure]
    public Vector3f MultiplyZ(Fix32 multZ) => new Vector3f(this.X, this.Y, this.Z * multZ);

    /// <summary>
    /// Returns new vector with reflected X component (opposite sign).
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector3f ReflectX => new Vector3f(-this.X, this.Y, this.Z);

    /// <summary>
    /// Returns new vector with reflected Y component (opposite sign).
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector3f ReflectY => new Vector3f(this.X, -this.Y, this.Z);

    /// <summary>
    /// Returns new vector with reflected Z component (opposite sign).
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector3f ReflectZ => new Vector3f(this.X, this.Y, -this.Z);

    /// <summary>Gets rounded Vector3i representation of this vector.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector3i RoundedVector3i
    {
      get => new Vector3i(this.X.ToIntRounded(), this.Y.ToIntRounded(), this.Z.ToIntRounded());
    }

    /// <summary>Gets ceiled Vector3i representation of this vector.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector3i CeiledVector3i
    {
      get => new Vector3i(this.X.ToIntCeiled(), this.Y.ToIntCeiled(), this.Z.ToIntCeiled());
    }

    /// <summary>Gets floored Vector3i representation of this vector.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector3i FlooredVector3i
    {
      get => new Vector3i(this.X.ToIntFloored(), this.Y.ToIntFloored(), this.Z.ToIntFloored());
    }

    /// <summary>
    /// Returns scaled vector to requested length. This method is faster and more intuitive than normalization
    /// followed by multiplication.
    /// </summary>
    [Pure]
    public Vector3f OfLength(Fix32 desiredLength)
    {
      Fix32 length = this.Length;
      Fix64 fix64 = this.X.MultAsFix64(desiredLength);
      Fix32 fix32_1 = fix64.DivToFix32(length);
      fix64 = this.Y.MultAsFix64(desiredLength);
      Fix32 fix32_2 = fix64.DivToFix32(length);
      fix64 = this.Z.MultAsFix64(desiredLength);
      Fix32 fix32_3 = fix64.DivToFix32(length);
      return new Vector3f(fix32_1, fix32_2, fix32_3);
    }

    [Pure]
    public bool IsNear(Vector3f other)
    {
      return this.X.IsNear(other.X) && this.Y.IsNear(other.Y) && this.Z.IsNear(other.Z);
    }

    /// <summary>
    /// Tests whether corresponding components of this and given vectors are within tolerance.
    /// </summary>
    [Pure]
    public bool IsNear(Vector3f other, Fix32 tolerance)
    {
      return this.X.IsNear(other.X, tolerance) && this.Y.IsNear(other.Y, tolerance) && this.Z.IsNear(other.Z, tolerance);
    }

    [Pure]
    public bool IsNearZero() => this.X.IsNearZero() && this.Y.IsNearZero() && this.Z.IsNearZero();

    /// <summary>
    /// Whether this vector length is (nearly) one using default epsilon <see cref="F:Mafi.Fix32.EpsilonNear" />. Note that
    /// This uses efficient check of length squared without the need for square root computation.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNormalized => this.LengthSqr.IsNear(Fix64.One, Fix64.EpsilonFix32NearOneSqr);

    /// <summary>Returns normalized vector.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector3f Normalized
    {
      get
      {
        Fix64 lengthSqr = this.LengthSqr;
        if (lengthSqr.IsNearZero())
        {
          Assert.Fail("Normalizing (near) zero vector.");
          return Vector3f.Zero;
        }
        if (lengthSqr.IsNear(Fix64.One, Fix64.EpsilonFix32NearOneSqr))
          return this;
        Fix64 fix64_1 = lengthSqr.Sqrt();
        Vector3f normalized;
        ref Vector3f local = ref normalized;
        Fix64 fix64_2 = this.X / fix64_1;
        Fix32 fix32_1 = fix64_2.ToFix32();
        fix64_2 = this.Y / fix64_1;
        Fix32 fix32_2 = fix64_2.ToFix32();
        fix64_2 = this.Z / fix64_1;
        Fix32 fix32_3 = fix64_2.ToFix32();
        local = new Vector3f(fix32_1, fix32_2, fix32_3);
        Assert.That<bool>(normalized.IsNormalized).IsTrue("Normalization failed, increase epsilon.");
        return normalized;
      }
    }

    /// <summary>Returns dot product of this vector with given vector.</summary>
    [Pure]
    public Fix64 Dot(Vector3f rhs)
    {
      return this.X.MultAsFix64(rhs.X) + this.Y.MultAsFix64(rhs.Y) + this.Z.MultAsFix64(rhs.Z);
    }

    /// <summary>
    /// Returns distance from this vector to the other vector.
    /// </summary>
    [Pure]
    public Fix32 DistanceTo(Vector3f other)
    {
      return new Vector3f(this.X - other.X, this.Y - other.Y, this.Z - other.Z).Length;
    }

    /// <summary>
    /// Returns squared distance from this vector to the other vector.
    /// </summary>
    [Pure]
    public Fix64 DistanceSqrTo(Vector3f other)
    {
      return new Vector3f(this.X - other.X, this.Y - other.Y, this.Z - other.Z).LengthSqr;
    }

    /// <summary>
    /// Returns absolute angle of this vector. Returned angle is in range [-τ/2, τ/2].
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public AngleDegrees1f Angle => MafiMath.Atan2(this.Y, this.X);

    /// <summary>
    /// Returns cross product of this vector with given vector.
    /// </summary>
    [Pure]
    public Vector3f Cross(Vector3f rhs)
    {
      return new Vector3f(this.Y * rhs.Z - this.Z * rhs.Y, this.Z * rhs.X - this.X * rhs.Z, this.X * rhs.Y - this.Y * rhs.X);
    }

    /// <summary>
    /// Whether this and <see paramref="other" /> vectors are parallel and not anti-parallel.
    /// </summary>
    [Pure]
    public bool IsParallelTo(Vector3f other)
    {
      Assert.That<Vector3f>(this).IsNotZero("IsParallelTo was called on zero vector.");
      Assert.That<Vector3f>(other).IsNotZero("IsParallelTo was called with zero vector.");
      return this.Cross(other).IsZero && this.Dot(other).IsPositive;
    }

    /// <summary>
    /// Whether this and <see paramref="other" /> vectors are anti-parallel and not parallel.
    /// </summary>
    [Pure]
    public bool IsAntiParallelTo(Vector3f other)
    {
      Assert.That<Vector3f>(this).IsNotZero("IsAntiParallelTo was called on zero vector.");
      Assert.That<Vector3f>(other).IsNotZero("IsAntiParallelTo was called with zero vector.");
      return this.Cross(other).IsZero && this.Dot(other).IsNegative;
    }

    /// <summary>
    /// Whether this and <see paramref="other" /> vectors are parallel or anti-parallel. This is more efficient than
    /// calling <see paramref="IsParallelTo" /> and <see paramref="IsAntiParallelTo" />.
    /// </summary>
    [Pure]
    public bool IsParallelOrAntiParallelTo(Vector3f other)
    {
      Assert.That<Vector3f>(this).IsNotZero("IsParallelOrAntiParallelTo was called on zero vector.");
      Assert.That<Vector3f>(other).IsNotZero("IsParallelOrAntiParallelTo was called with zero vector.");
      return this.Cross(other).IsZero;
    }

    /// <summary>
    /// Returns absolute angle between this and <see paramref="other" /> vectors. Returned angle is in range [0, τ/2].
    /// </summary>
    [Pure]
    public AngleDegrees1f AngleBetween(Vector3f other)
    {
      return MafiMath.Atan2(this.Cross(other).Length.ToFix64(), this.Dot(other));
    }

    /// <summary>
    /// Signed angle-to is not possible in 3D without some kind of reference vector.
    /// </summary>
    public void AngleTo(Vector3f other)
    {
    }

    /// <summary>
    /// Returns this vector rotated around X-axis by given amount.
    /// </summary>
    [Pure]
    public Vector3f RotatedAroundX(AngleDegrees1f angle)
    {
      Fix64 fix64_1 = angle.Cos();
      Fix64 fix64_2 = angle.Sin();
      Fix32 x = this.X;
      Fix64 fix64_3 = fix64_1 * this.Y - fix64_2 * this.Z;
      Fix32 fix32_1 = fix64_3.ToFix32();
      fix64_3 = fix64_2 * this.Y + fix64_1 * this.Z;
      Fix32 fix32_2 = fix64_3.ToFix32();
      return new Vector3f(x, fix32_1, fix32_2);
    }

    /// <summary>
    /// Returns this vector rotated around Y-axis by given amount.
    /// </summary>
    [Pure]
    public Vector3f RotatedAroundY(AngleDegrees1f angle)
    {
      Fix64 fix64_1 = angle.Cos();
      Fix64 fix64_2 = angle.Sin();
      Fix64 fix64_3 = fix64_1 * this.X + fix64_2 * this.Z;
      Fix32 fix32_1 = fix64_3.ToFix32();
      Fix32 y = this.Y;
      fix64_3 = fix64_1 * this.Z - fix64_2 * this.X;
      Fix32 fix32_2 = fix64_3.ToFix32();
      return new Vector3f(fix32_1, y, fix32_2);
    }

    /// <summary>
    /// Returns this vector rotated around Z-axis by given amount.
    /// </summary>
    [Pure]
    public Vector3f RotatedAroundZ(AngleDegrees1f angle)
    {
      Fix64 fix64_1 = angle.Cos();
      Fix64 fix64_2 = angle.Sin();
      Fix64 fix64_3 = fix64_1 * this.X - fix64_2 * this.Y;
      Fix32 fix32_1 = fix64_3.ToFix32();
      fix64_3 = fix64_2 * this.X + fix64_1 * this.Y;
      Fix32 fix32_2 = fix64_3.ToFix32();
      Fix32 z = this.Z;
      return new Vector3f(fix32_1, fix32_2, z);
    }

    /// <summary>
    /// Helper function that rotates a 2d vector, it gets as first two parameters by given angle.
    /// (1, 0) when rotated by 90 degrees gives (0, 1)
    /// </summary>
    private void rotate2dVector(ref Fix32 x, ref Fix32 y, Rotation90 angle)
    {
      switch (angle.AngleIndex)
      {
        case 1:
          Fix32 fix32_1 = x;
          x = -y;
          y = fix32_1;
          break;
        case 2:
          x = -x;
          y = -y;
          break;
        case 3:
          Fix32 fix32_2 = x;
          x = y;
          y = -fix32_2;
          break;
      }
    }

    /// <summary>
    /// Returns this vector rotated around X-axis by given amount.
    /// </summary>
    [Pure]
    public Vector3f RotatedAroundX(Rotation90 angle)
    {
      Fix32 y = this.Y;
      Fix32 z = this.Z;
      this.rotate2dVector(ref y, ref z, angle);
      return new Vector3f(this.X, y, z);
    }

    /// <summary>
    /// Returns this vector rotated around Y-axis by given amount.
    /// </summary>
    [Pure]
    public Vector3f RotatedAroundY(Rotation90 angle)
    {
      Fix32 z = this.Z;
      Fix32 x = this.X;
      this.rotate2dVector(ref z, ref x, angle);
      return new Vector3f(x, this.Y, z);
    }

    /// <summary>
    /// Returns this vector rotated around Z-axis by given amount.
    /// </summary>
    [Pure]
    public Vector3f RotatedAroundZ(Rotation90 angle)
    {
      Fix32 x = this.X;
      Fix32 y = this.Y;
      this.rotate2dVector(ref x, ref y, angle);
      return new Vector3f(x, y, this.Z);
    }

    /// <summary>
    /// Reflection of this vector from plane specified by given normal (has to be normalized). This vector should be
    /// pointing to the plane and reflected vector is pointing away from the plane.
    /// </summary>
    /// <remarks>
    /// <code>
    ///             Normal vector
    ///           *
    /// this   \  |  * reflected
    /// vector  \ | /  vector
    ///      ____*|/____  reflection surface given by the normal
    /// </code>
    /// </remarks>
    [Pure]
    public Vector3f Reflect(Vector3f normal) => this - (2 * this.Dot(normal)).ToFix32() * normal;

    /// <summary>Returns component-wise min of this and given vectors.</summary>
    [Pure]
    public Vector3f Min(Vector3f rhs)
    {
      return new Vector3f(this.X.Min(rhs.X), this.Y.Min(rhs.Y), this.Z.Min(rhs.Z));
    }

    /// <summary>Returns component-wise min of this and given vectors.</summary>
    [Pure]
    public Vector3f Max(Vector3f rhs)
    {
      return new Vector3f(this.X.Max(rhs.X), this.Y.Max(rhs.Y), this.Z.Max(rhs.Z));
    }

    /// <summary>Returns component-wise min of this and given vectors.</summary>
    [Pure]
    public Fix32 MinComponent() => this.X.Min(this.Y).Min(this.Z);

    /// <summary>Returns component-wise max of this and given vectors.</summary>
    [Pure]
    public Fix32 MaxComponent() => this.X.Max(this.Y).Max(this.Z);

    /// <summary>Returns component-wise clamp of this vectors.</summary>
    [Pure]
    public Vector3f Clamp(Fix32 min, Fix32 max)
    {
      return new Vector3f(this.X.Clamp(min, max), this.Y.Clamp(min, max), this.Z.Clamp(min, max));
    }

    /// <summary>Returns component-wise absolute value of this vector.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector3f AbsValue => new Vector3f(this.X.Abs(), this.Y.Abs(), this.Z.Abs());

    /// <summary>Returns component-wise sign of this vector.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector3i Signs => new Vector3i(this.X.Sign(), this.Y.Sign(), this.Z.Sign());

    /// <summary>
    /// Returns component-wise modulo operation on this vector (see <see cref="M:Mafi.MafiMath.Modulo(System.Int32,System.Int32)" />).
    /// </summary>
    [Pure]
    public Vector3f Modulo(Fix32 mod)
    {
      return new Vector3f(this.X.Modulo(mod), this.Y.Modulo(mod), this.Z.Modulo(mod));
    }

    /// <summary>
    /// Returns component-wise modulo operation on this vector (see <see cref="M:Mafi.MafiMath.Modulo(System.Int32,System.Int32)" />).
    /// </summary>
    [Pure]
    public Vector3f Modulo(Vector3f mod)
    {
      return new Vector3f(this.X.Modulo(mod.X), this.Y.Modulo(mod.Y), this.Z.Modulo(mod.Z));
    }

    /// <summary>
    /// Returns component-wise average of this and given vectors.
    /// </summary>
    [Pure]
    public Vector3f Average(Vector3f rhs)
    {
      Fix32 fix32 = this.X + rhs.X;
      Fix32 halfFast1 = fix32.HalfFast;
      fix32 = this.Y + rhs.Y;
      Fix32 halfFast2 = fix32.HalfFast;
      fix32 = this.Z + rhs.Z;
      Fix32 halfFast3 = fix32.HalfFast;
      return new Vector3f(halfFast1, halfFast2, halfFast3);
    }

    /// <summary>
    /// Linearly interpolates between this and <paramref name="to" /> vectors based on <paramref name="t" />.
    /// Interpolation parameter <paramref name="t" /> is expected to be from 0 to 1.
    /// </summary>
    [Pure]
    public Vector3f Lerp(Vector3f to, Fix32 t, Fix32 scale)
    {
      return new Vector3f(this.X.Lerp(to.X, t, scale), this.Y.Lerp(to.Y, t, scale), this.Z.Lerp(to.Z, t, scale));
    }

    [Pure]
    public Vector3f Lerp(Vector3f to, Percent t)
    {
      return new Vector3f(this.X.Lerp(to.X, t), this.Y.Lerp(to.Y, t), this.Z.Lerp(to.Z, t));
    }

    [Pure]
    public bool Equals(Vector3f other) => other == this;

    [Pure]
    public override bool Equals(object other) => other is Vector3f vector3f && vector3f == this;

    [Pure]
    public override int GetHashCode()
    {
      return (this.X.GetHashCode() * 577 ^ this.Y.GetHashCode()) * 397 ^ this.Z.GetHashCode();
    }

    [Pure]
    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder(28);
      stringBuilder.Append('(');
      stringBuilder.Append(this.X.ToString());
      stringBuilder.Append(", ");
      stringBuilder.Append(this.Y.ToString());
      stringBuilder.Append(", ");
      stringBuilder.Append(this.Z.ToString());
      stringBuilder.Append(')');
      return stringBuilder.ToString();
    }

    [Pure]
    public int CompareTo(Vector3f other)
    {
      if (this.X < other.X)
        return -1;
      if (this.X > other.X)
        return 1;
      if (this.Y < other.Y)
        return -1;
      if (this.Y > other.Y)
        return 1;
      if (this.Z < other.Z)
        return -1;
      return this.Z > other.Z ? 1 : 0;
    }

    /// <summary>Exact equality of two vectors.</summary>
    public static bool operator ==(Vector3f lhs, Vector3f rhs)
    {
      return lhs.X == rhs.X && lhs.Y == rhs.Y && lhs.Z == rhs.Z;
    }

    /// <summary>Exact inequality of two vectors.</summary>
    public static bool operator !=(Vector3f lhs, Vector3f rhs)
    {
      return lhs.X != rhs.X || lhs.Y != rhs.Y || lhs.Z != rhs.Z;
    }

    /// <summary>
    /// Component-wise less-than operator. Returns true if all components of the left-hand side vector are less than
    /// respective components of the right-hand side vector.
    /// WARNING: <c>A &lt; B</c> is not equal to <c>A &gt;= B</c>. For example vectors (1, 2, 3) and (3, 2, 1) are not
    /// less-than nor greater-than-or-equal.
    /// </summary>
    public static bool operator <(Vector3f lhs, Vector3f rhs)
    {
      return lhs.X < rhs.X && lhs.Y < rhs.Y && lhs.Z < rhs.Z;
    }

    /// <summary>
    /// Component-wise less-than-or-equal operator. Returns true if all components of the left-hand side vector are
    /// less than or equal to respective components of the right-hand side vector.
    /// WARNING: <c>A &lt;= B</c> is not equal to <c>A &gt; B</c>. For example vectors (1, 2, 3) and (3, 2, 1) are not
    /// less-than-or-equal nor greater-than.
    /// </summary>
    public static bool operator <=(Vector3f lhs, Vector3f rhs)
    {
      return lhs.X <= rhs.X && lhs.Y <= rhs.Y && lhs.Z <= rhs.Z;
    }

    /// <summary>
    /// Component-wise greater-than operator. Returns true if all components of the left-hand side vector are
    /// greater than respective components of the right-hand side vector.
    /// WARNING: <c>A &gt; B</c> is not equal to <c>A &lt;= B</c>. For example vectors (1, 2, 3) and (3, 2, 1) are not
    /// greater-than nor less-than-or-equal.
    /// </summary>
    public static bool operator >(Vector3f lhs, Vector3f rhs)
    {
      return lhs.X > rhs.X && lhs.Y > rhs.Y && lhs.Z > rhs.Z;
    }

    /// <summary>
    /// Component-wise greater-than-or-equal operator. Returns true if all components of the left-hand side vector
    /// are greater than or equal to respective components of the right-hand side vector.
    /// WARNING: <c>A &gt;= B</c> is not equal to <c>A &lt; B</c>. For example vectors (1, 2, 3) and (3, 2, 1) are not
    /// greater-than-or-equal nor less-than.
    /// </summary>
    public static bool operator >=(Vector3f lhs, Vector3f rhs)
    {
      return lhs.X >= rhs.X && lhs.Y >= rhs.Y && lhs.Z >= rhs.Z;
    }

    public static Vector3f operator +(Fix32 lhs, Vector3f rhs)
    {
      return new Vector3f(lhs + rhs.X, lhs + rhs.Y, lhs + rhs.Z);
    }

    public static Vector3f operator +(Vector3f lhs, Fix32 rhs)
    {
      return new Vector3f(lhs.X + rhs, lhs.Y + rhs, lhs.Z + rhs);
    }

    public static Vector3f operator +(Vector3f lhs, Vector3f rhs)
    {
      return new Vector3f(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z);
    }

    public static Vector3f operator +(Vector3i lhs, Vector3f rhs)
    {
      return new Vector3f((Fix32) lhs.X + rhs.X, (Fix32) lhs.Y + rhs.Y, (Fix32) lhs.Z + rhs.Z);
    }

    public static Vector3f operator +(Vector3f lhs, Vector3i rhs)
    {
      return new Vector3f(lhs.X + (Fix32) rhs.X, lhs.Y + (Fix32) rhs.Y, lhs.Z + (Fix32) rhs.Z);
    }

    public static Vector3f operator +(Vector3f lhs, Vector2f rhs)
    {
      return new Vector3f(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z);
    }

    public static Vector3f operator -(Vector3f vector)
    {
      return new Vector3f(-vector.X, -vector.Y, -vector.Z);
    }

    public static Vector3f operator -(Fix32 lhs, Vector3f rhs)
    {
      return new Vector3f(lhs - rhs.X, lhs - rhs.Y, lhs - rhs.Z);
    }

    public static Vector3f operator -(Vector3f lhs, Fix32 rhs)
    {
      return new Vector3f(lhs.X - rhs, lhs.Y - rhs, lhs.Z - rhs);
    }

    public static Vector3f operator -(Vector3f lhs, Vector3f rhs)
    {
      return new Vector3f(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z);
    }

    public static Vector3f operator -(Vector3i lhs, Vector3f rhs)
    {
      return new Vector3f((Fix32) lhs.X - rhs.X, (Fix32) lhs.Y - rhs.Y, (Fix32) lhs.Z - rhs.Z);
    }

    public static Vector3f operator -(Vector3f lhs, Vector3i rhs)
    {
      return new Vector3f(lhs.X - (Fix32) rhs.X, lhs.Y - (Fix32) rhs.Y, lhs.Z - (Fix32) rhs.Z);
    }

    public static Vector3f operator -(Vector3f lhs, Vector2f rhs)
    {
      return new Vector3f(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z);
    }

    public static Vector3f operator *(Vector3f lhs, Fix32 rhs)
    {
      return new Vector3f(lhs.X * rhs, lhs.Y * rhs, lhs.Z * rhs);
    }

    public static Vector3f operator *(Fix32 lhs, Vector3f rhs)
    {
      return new Vector3f(lhs * rhs.X, lhs * rhs.Y, lhs * rhs.Z);
    }

    public static Vector3f operator *(Vector3f lhs, int rhs)
    {
      return new Vector3f(lhs.X * rhs, lhs.Y * rhs, lhs.Z * rhs);
    }

    public static Vector3f operator *(int lhs, Vector3f rhs)
    {
      return new Vector3f(rhs.X * lhs, rhs.Y * lhs, rhs.Z * lhs);
    }

    public static Vector3f operator *(Vector3f lhs, Percent rhs)
    {
      return new Vector3f(rhs.Apply(lhs.X), rhs.Apply(lhs.Y), rhs.Apply(lhs.Z));
    }

    public static Vector3f operator *(Percent lhs, Vector3f rhs)
    {
      return new Vector3f(lhs.Apply(rhs.X), lhs.Apply(rhs.Y), lhs.Apply(rhs.Z));
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector3f Times2Fast
    {
      get => new Vector3f(this.X.Times2Fast, this.Y.Times2Fast, this.Z.Times2Fast);
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector3f Times4Fast
    {
      get => new Vector3f(this.X.Times4Fast, this.Y.Times4Fast, this.Z.Times4Fast);
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector3f Times8Fast
    {
      get => new Vector3f(this.X.Times8Fast, this.Y.Times8Fast, this.Z.Times8Fast);
    }

    public static Vector3f operator /(Fix32 lhs, Vector3f rhs)
    {
      return new Vector3f(lhs / rhs.X, lhs / rhs.Y, lhs / rhs.Z);
    }

    public static Vector3f operator /(Vector3f lhs, Fix32 rhs)
    {
      return new Vector3f(lhs.X / rhs, lhs.Y / rhs, lhs.Z / rhs);
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector3f HalfFast => new Vector3f(this.X.HalfFast, this.Y.HalfFast, this.Z.HalfFast);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector3f DivBy4Fast
    {
      get => new Vector3f(this.X.DivBy4Fast, this.Y.DivBy4Fast, this.Z.DivBy4Fast);
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector3f DivBy8Fast
    {
      get => new Vector3f(this.X.DivBy8Fast, this.Y.DivBy8Fast, this.Z.DivBy8Fast);
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector3f DivBy16Fast
    {
      get => new Vector3f(this.X.DivBy16Fast, this.Y.DivBy16Fast, this.Z.DivBy16Fast);
    }

    /// <summary>Component-wise division of two vectors.</summary>
    public static Vector3f operator /(Vector3f lhs, Vector3f rhs)
    {
      return new Vector3f(lhs.X / rhs.X, lhs.Y / rhs.Y, lhs.Z / rhs.Z);
    }

    /// <summary>Component-wise division of two vectors.</summary>
    public static Vector3f operator /(Vector3i lhs, Vector3f rhs)
    {
      return new Vector3f(lhs.X / rhs.X, lhs.Y / rhs.Y, lhs.Z / rhs.Z);
    }

    /// <summary>Component-wise division of two vectors.</summary>
    public static Vector3f operator /(Vector3f lhs, Vector3i rhs)
    {
      return new Vector3f(lhs.X / rhs.X, lhs.Y / rhs.Y, lhs.Z / rhs.Z);
    }

    public static Vector3f operator %(Fix32 lhs, Vector3f rhs)
    {
      return new Vector3f(lhs % rhs.X, lhs % rhs.Y, lhs % rhs.Z);
    }

    public static Vector3f operator %(Vector3f lhs, Fix32 rhs)
    {
      return new Vector3f(lhs.X % rhs, lhs.Y % rhs, lhs.Z % rhs);
    }

    /// <summary>Component-wise modulo of two vectors.</summary>
    public static Vector3f operator %(Vector3f lhs, Vector3f rhs)
    {
      return new Vector3f(lhs.X % rhs.X, lhs.Y % rhs.Y, lhs.Z % rhs.Z);
    }

    /// <summary>Component-wise modulo of two vectors.</summary>
    public static Vector3f operator %(Vector3i lhs, Vector3f rhs)
    {
      return new Vector3f((Fix32) lhs.X % rhs.X, (Fix32) lhs.Y % rhs.Y, (Fix32) lhs.Z % rhs.Z);
    }

    /// <summary>Component-wise modulo of two vectors.</summary>
    public static Vector3f operator %(Vector3f lhs, Vector3i rhs)
    {
      return new Vector3f(lhs.X % (Fix32) rhs.X, lhs.Y % (Fix32) rhs.Y, lhs.Z % (Fix32) rhs.Z);
    }

    public static void Serialize(Vector3f value, BlobWriter writer)
    {
      Fix32.Serialize(value.X, writer);
      Fix32.Serialize(value.Y, writer);
      Fix32.Serialize(value.Z, writer);
    }

    public static Vector3f Deserialize(BlobReader reader)
    {
      return new Vector3f(Fix32.Deserialize(reader), Fix32.Deserialize(reader), Fix32.Deserialize(reader));
    }

    /// <summary>
    /// Returns an (arbitrary) orthogonal vector. Returned vector is not normalized.
    /// </summary>
    /// <remarks>
    /// Returned vector is a cross product with an axis corresponding to the smallest component of the vector.
    /// </remarks>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector3f Orthogonal
    {
      get
      {
        Vector3f absValue = this.AbsValue;
        return this.Cross(absValue.X < absValue.Y ? (absValue.X < absValue.Z ? Vector3f.UnitX : Vector3f.UnitZ) : (absValue.Y < absValue.Z ? Vector3f.UnitY : Vector3f.UnitZ));
      }
    }

    /// <summary>Euclidean length squared of this vector.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Fix32 LengthSqrAsFix32 => this.X * this.X + this.Y * this.Y + this.Z * this.Z;

    [Pure]
    public bool HasAlmostSameDirection(Vector3f other)
    {
      Assert.That<Vector3f>(this).IsNotZero("HasAlmostSameDirection was called on zero vector.");
      Assert.That<Vector3f>(other).IsNotZero("HasAlmostSameDirection was called with zero vector.");
      return this.Cross(other).IsNearZero() && this.Dot(other).IsPositive;
    }

    static Vector3f()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      Vector3f.Zero = new Vector3f();
      Vector3f.One = new Vector3f(Fix32.FromDouble(1.0), Fix32.FromDouble(1.0), Fix32.FromDouble(1.0));
      Vector3f.UnitX = new Vector3f(Fix32.FromDouble(1.0), Fix32.FromDouble(0.0), Fix32.FromDouble(0.0));
      Vector3f.UnitY = new Vector3f(Fix32.FromDouble(0.0), Fix32.FromDouble(1.0), Fix32.FromDouble(0.0));
      Vector3f.UnitZ = new Vector3f(Fix32.FromDouble(0.0), Fix32.FromDouble(0.0), Fix32.FromDouble(1.0));
      Vector3f.MinValue = new Vector3f(Fix32.MinValue, Fix32.MinValue, Fix32.MinValue);
      Vector3f.MaxValue = new Vector3f(Fix32.MaxValue, Fix32.MaxValue, Fix32.MaxValue);
    }
  }
}
