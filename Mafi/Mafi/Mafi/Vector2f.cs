// Decompiled with JetBrains decompiler
// Type: Mafi.Vector2f
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
  /// <summary>Immutable 2D vector of Fix32 values.</summary>
  /// <remarks>
  /// This is partial struct and this file should contain only specific members for <see cref="T:Mafi.Vector2f" />. All
  /// general members should be added to the generator T4 template.
  /// </remarks>
  [ManuallyWrittenSerialization]
  [DebuggerStepThrough]
  [DebuggerDisplay("({X}, {Y})")]
  public struct Vector2f : IEquatable<Vector2f>, IComparable<Vector2f>
  {
    /// <summary>Vector (0, 0).</summary>
    public static readonly Vector2f Zero;
    /// <summary>Vector (1, 1).</summary>
    public static readonly Vector2f One;
    /// <summary>Vector (1, 0).</summary>
    public static readonly Vector2f UnitX;
    /// <summary>Vector (0, 1).</summary>
    public static readonly Vector2f UnitY;
    /// <summary>Vector (Fix32.MinValue, Fix32.MinValue).</summary>
    public static readonly Vector2f MinValue;
    /// <summary>Vector (Fix32.MaxValue, Fix32.MaxValue).</summary>
    public static readonly Vector2f MaxValue;
    /// <summary>The X component of this vector.</summary>
    public readonly Fix32 X;
    /// <summary>The Y component of this vector.</summary>
    public readonly Fix32 Y;

    /// <summary>Creates new Vector2f from raw components.</summary>
    public Vector2f(Fix32 x, Fix32 y)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.X = x;
      this.Y = y;
    }

    /// <summary>Sum of all components of this vector.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Fix32 Sum => this.X + this.Y;

    /// <summary>Product of all components of this vector.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Fix64 Product => this.X.MultAsFix64(this.Y);

    /// <summary>
    /// Euclidean length of this vector.
    /// PERF: Expensive, uses sqrt. Consider using <see cref="P:Mafi.Vector2f.LengthSqr" /> whenever possible (when comparing
    /// lengths, etc.).
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Fix32 Length => this.LengthSqr.SqrtToFix32();

    /// <summary>
    /// Euclidean length squared of this vector.
    /// PERF: Cheaper than <see cref="P:Mafi.Vector2f.Length" />, does not require expensive sqrt.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Fix64 LengthSqr => this.X.MultAsFix64(this.X) + this.Y.MultAsFix64(this.Y);

    /// <summary>Whether this vector has all components equal to zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsZero => this.X.IsZero && this.Y.IsZero;

    /// <summary>e
    /// Whether this vector has at least one components not equal to zero.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNotZero => this.X.IsNotZero || this.Y.IsNotZero;

    /// <summary>Returns new vector with changed X component.</summary>
    [Pure]
    public Vector2f SetX(Fix32 newX) => new Vector2f(newX, this.Y);

    /// <summary>Returns new vector with changed Y component.</summary>
    [Pure]
    public Vector2f SetY(Fix32 newY) => new Vector2f(this.X, newY);

    /// <summary>Extends this vector a new component.</summary>
    [Pure]
    public Vector3f ExtendZ(Fix32 z) => new Vector3f(this.X, this.Y, z);

    /// <summary>Extends this vector new components.</summary>
    [Pure]
    public Vector4f ExtendZw(Fix32 z, Fix32 w) => new Vector4f(this.X, this.Y, z, w);

    /// <summary>Returns new vector with incremented X component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector2f IncrementX => new Vector2f(this.X + (Fix32) 1, this.Y);

    /// <summary>Returns new vector with incremented Y component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector2f IncrementY => new Vector2f(this.X, this.Y + (Fix32) 1);

    /// <summary>Returns new vector with decremented X component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector2f DecrementX => new Vector2f(this.X - (Fix32) 1, this.Y);

    /// <summary>Returns new vector with decremented Y component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector2f DecrementY => new Vector2f(this.X, this.Y - (Fix32) 1);

    /// <summary>
    /// Returns new vector with given value added to the X component.
    /// </summary>
    [Pure]
    public Vector2f AddX(Fix32 addedX) => new Vector2f(this.X + addedX, this.Y);

    /// <summary>
    /// Returns new vector with given value added to the Y component.
    /// </summary>
    [Pure]
    public Vector2f AddY(Fix32 addedY) => new Vector2f(this.X, this.Y + addedY);

    /// <summary>
    /// Returns new vector with given value added to all components.
    /// </summary>
    [Pure]
    public Vector2f AddXy(Fix32 addedValue)
    {
      return new Vector2f(this.X + addedValue, this.Y + addedValue);
    }

    /// <summary>
    /// Returns new vector with given value multiplied with the X component.
    /// </summary>
    [Pure]
    public Vector2f MultiplyX(Fix32 multX) => new Vector2f(this.X * multX, this.Y);

    /// <summary>
    /// Returns new vector with given value multiplied with the Y component.
    /// </summary>
    [Pure]
    public Vector2f MultiplyY(Fix32 multY) => new Vector2f(this.X, this.Y * multY);

    /// <summary>
    /// Returns new vector with reflected X component (opposite sign).
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector2f ReflectX => new Vector2f(-this.X, this.Y);

    /// <summary>
    /// Returns new vector with reflected Y component (opposite sign).
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector2f ReflectY => new Vector2f(this.X, -this.Y);

    /// <summary>Gets rounded Vector2i representation of this vector.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector2i RoundedVector2i => new Vector2i(this.X.ToIntRounded(), this.Y.ToIntRounded());

    /// <summary>Gets ceiled Vector2i representation of this vector.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector2i CeiledVector2i => new Vector2i(this.X.ToIntCeiled(), this.Y.ToIntCeiled());

    /// <summary>Gets floored Vector2i representation of this vector.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector2i FlooredVector2i => new Vector2i(this.X.ToIntFloored(), this.Y.ToIntFloored());

    /// <summary>
    /// Returns scaled vector to requested length. This method is faster and more intuitive than normalization
    /// followed by multiplication.
    /// </summary>
    [Pure]
    public Vector2f OfLength(Fix32 desiredLength)
    {
      Fix32 length = this.Length;
      Fix64 fix64 = this.X.MultAsFix64(desiredLength);
      Fix32 fix32_1 = fix64.DivToFix32(length);
      fix64 = this.Y.MultAsFix64(desiredLength);
      Fix32 fix32_2 = fix64.DivToFix32(length);
      return new Vector2f(fix32_1, fix32_2);
    }

    [Pure]
    public bool IsNear(Vector2f other) => this.X.IsNear(other.X) && this.Y.IsNear(other.Y);

    /// <summary>
    /// Tests whether corresponding components of this and given vectors are within tolerance.
    /// </summary>
    [Pure]
    public bool IsNear(Vector2f other, Fix32 tolerance)
    {
      return this.X.IsNear(other.X, tolerance) && this.Y.IsNear(other.Y, tolerance);
    }

    [Pure]
    public bool IsNearZero() => this.X.IsNearZero() && this.Y.IsNearZero();

    /// <summary>
    /// Whether this vector length is (nearly) one using default epsilon <see cref="F:Mafi.Fix32.EpsilonNear" />. Note that
    /// This uses efficient check of length squared without the need for square root computation.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNormalized => this.LengthSqr.IsNear(Fix64.One, Fix64.EpsilonFix32NearOneSqr);

    /// <summary>Returns normalized vector.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector2f Normalized
    {
      get
      {
        Fix64 lengthSqr = this.LengthSqr;
        if (lengthSqr.IsNearZero())
        {
          Assert.Fail("Normalizing (near) zero vector.");
          return Vector2f.Zero;
        }
        if (lengthSqr.IsNear(Fix64.One, Fix64.EpsilonFix32NearOneSqr))
          return this;
        Fix64 fix64_1 = lengthSqr.Sqrt();
        Vector2f normalized;
        ref Vector2f local = ref normalized;
        Fix64 fix64_2 = this.X / fix64_1;
        Fix32 fix32_1 = fix64_2.ToFix32();
        fix64_2 = this.Y / fix64_1;
        Fix32 fix32_2 = fix64_2.ToFix32();
        local = new Vector2f(fix32_1, fix32_2);
        Assert.That<bool>(normalized.IsNormalized).IsTrue("Normalization failed, increase epsilon.");
        return normalized;
      }
    }

    /// <summary>Returns dot product of this vector with given vector.</summary>
    [Pure]
    public Fix64 Dot(Vector2f rhs) => this.X.MultAsFix64(rhs.X) + this.Y.MultAsFix64(rhs.Y);

    /// <summary>
    /// Returns distance from this vector to the other vector.
    /// </summary>
    [Pure]
    public Fix32 DistanceTo(Vector2f other)
    {
      return new Vector2f(this.X - other.X, this.Y - other.Y).Length;
    }

    /// <summary>
    /// Returns squared distance from this vector to the other vector.
    /// </summary>
    [Pure]
    public Fix64 DistanceSqrTo(Vector2f other)
    {
      return new Vector2f(this.X - other.X, this.Y - other.Y).LengthSqr;
    }

    /// <summary>
    /// Returns absolute angle of this vector. Returned angle is in range [-τ/2, τ/2].
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public AngleDegrees1f Angle => MafiMath.Atan2(this.Y, this.X);

    /// <summary>
    /// Returns pseudo-cross product of this vector with <paramref name="other" /> vector. That is, Z component of 3D
    /// cross product between the two vectors with Z equal to 0.
    /// </summary>
    /// <remarks>
    /// This operation is very efficient and has following properties:
    /// * v1.PseudoCross(v2) = -v2.PseudoCross(v1).
    /// * v1.PseudoCross(v2) = |v1| |v2| sin(θ) where θ is signed angle from v1 to v2 (<c>v1.AngleTo(v2)</c>).
    /// * Returns 0 for parallel (or anti-parallel) vectors.
    /// * Returns positive values when v2 is to the left of v1.
    /// * Returns negative values when v2 is to the right of v1.
    /// * Is equal to a determinant of matrix <c>[v1,v2]</c>.
    /// * Is equal to <c>v1.LeftOrthogonalVector.Dot(v2)</c>.
    /// </remarks>
    [Pure]
    public Fix64 PseudoCross(Vector2f other)
    {
      return this.X.MultAsFix64(other.Y) - this.Y.MultAsFix64(other.X);
    }

    /// <summary>
    /// Returns rotated vector by given angle. Positive angle values represent in counter-clockwise rotation. This
    /// means that <c>(1, 0).Rotate(90°) == (0, 1)</c>.
    /// </summary>
    [Pure]
    public Vector2f Rotate(AngleDegrees1f angle)
    {
      Fix64 fix64_1 = angle.Cos();
      Fix64 fix64_2 = angle.Sin();
      Fix64 fix64_3 = this.X * fix64_1 - this.Y * fix64_2;
      Fix32 fix32_1 = fix64_3.ToFix32();
      fix64_3 = this.X * fix64_2 + this.Y * fix64_1;
      Fix32 fix32_2 = fix64_3.ToFix32();
      return new Vector2f(fix32_1, fix32_2);
    }

    /// <summary>
    /// Returns rotated vector by given angle. Positive angle values represent in counter-clockwise rotation. This
    /// means that <c>(1, 0).Rotate(90°) == (0, 1)</c>.
    /// </summary>
    [Pure]
    public Vector2f Rotate(Rotation90 angle)
    {
      switch (angle.AngleIndex)
      {
        case 0:
          return this;
        case 1:
          return new Vector2f(-this.Y, this.X);
        case 2:
          return new Vector2f(-this.X, -this.Y);
        case 3:
          return new Vector2f(this.Y, -this.X);
        default:
          Assert.Fail("Invalid rotation passed.");
          return this;
      }
    }

    /// <summary>
    /// Returns rotated vector by given angle around given pivot point. Positive angle values represent in
    /// counter-clockwise rotation. This means that <c>(1, 0).Rotate(90°) == (0, 1)</c>.
    /// </summary>
    [Pure]
    public Vector2f Rotate(AngleDegrees1f angle, Vector2f pivot)
    {
      return (this - pivot).Rotate(angle) + pivot;
    }

    /// <summary>
    /// Returns signed angle from this vector to <paramref name="other" /> vector. Returned angle represents how much
    /// this vector has to be rotated to obtain <paramref name="other" /> vector. Returned value is [-τ/2, τ/2). This
    /// means that <c>v1.AngleTo(v2) == -v2.AngleTo(v1)</c> and <c>v1.Rotate(v1.AngleTo(v2)) == v2</c>.
    /// </summary>
    [Pure]
    public AngleDegrees1f AngleTo(Vector2f other)
    {
      Assert.That<Vector2f>(this).IsNotZero("AngleTo was called on zero vector.");
      Assert.That<Vector2f>(other).IsNotZero("AngleTo was called with zero vector.");
      return MafiMath.Atan2(this.PseudoCross(other), this.Dot(other));
    }

    /// <summary>
    /// Returns absolute angle between this and <see paramref="other" /> vectors. Returned angle is in range [0, τ/2].
    /// </summary>
    [Pure]
    public AngleDegrees1f AngleBetween(Vector2f other) => this.AngleTo(other).Abs;

    /// <summary>
    /// Whether this and <see paramref="other" /> vectors are parallel and not anti-parallel.
    /// </summary>
    [Pure]
    public bool IsParallelTo(Vector2f other)
    {
      Assert.That<Vector2f>(this).IsNotZero("IsParallelTo was called on zero vector.");
      Assert.That<Vector2f>(other).IsNotZero("IsParallelTo was called with zero vector.");
      return this.PseudoCross(other).IsZero && this.Dot(other).IsPositive;
    }

    /// <summary>
    /// Whether this and <see paramref="other" /> vectors are anti-parallel and not parallel.
    /// </summary>
    [Pure]
    public bool IsAntiParallelTo(Vector2f other)
    {
      Assert.That<Vector2f>(this).IsNotZero("IsParallelTo was called on zero vector.");
      Assert.That<Vector2f>(other).IsNotZero("IsParallelTo was called with zero vector.");
      return this.PseudoCross(other).IsZero && this.Dot(other).IsNegative;
    }

    /// <summary>
    /// Whether this and <see paramref="other" /> vectors are parallel or anti-parallel. This is more efficient than
    /// calling <see paramref="IsParallelTo" /> and <see paramref="IsAntiParallelTo" />.
    /// </summary>
    [Pure]
    public bool IsParallelOrAntiParallelTo(Vector2f other)
    {
      Assert.That<Vector2f>(this).IsNotZero("IsParallelTo was called on zero vector.");
      Assert.That<Vector2f>(other).IsNotZero("IsParallelTo was called with zero vector.");
      return this.PseudoCross(other).IsZero;
    }

    /// <summary>
    /// Returns this vector rotated by 90 degrees to the left (counter clockwise).
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector2f LeftOrthogonalVector => new Vector2f(-this.Y, this.X);

    /// <summary>
    /// Returns this vector rotated by 90 degrees to the right (clockwise).
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector2f RightOrthogonalVector => new Vector2f(this.Y, -this.X);

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
    public Vector2f Reflect(Vector2f normal) => this - (2 * this.Dot(normal)).ToFix32() * normal;

    /// <summary>Returns component-wise min of this and given vectors.</summary>
    [Pure]
    public Vector2f Min(Vector2f rhs) => new Vector2f(this.X.Min(rhs.X), this.Y.Min(rhs.Y));

    /// <summary>Returns component-wise min of this and given vectors.</summary>
    [Pure]
    public Vector2f Max(Vector2f rhs) => new Vector2f(this.X.Max(rhs.X), this.Y.Max(rhs.Y));

    /// <summary>Returns component-wise min of this and given vectors.</summary>
    [Pure]
    public Fix32 MinComponent() => this.X.Min(this.Y);

    /// <summary>Returns component-wise max of this and given vectors.</summary>
    [Pure]
    public Fix32 MaxComponent() => this.X.Max(this.Y);

    /// <summary>Returns component-wise clamp of this vectors.</summary>
    [Pure]
    public Vector2f Clamp(Fix32 min, Fix32 max)
    {
      return new Vector2f(this.X.Clamp(min, max), this.Y.Clamp(min, max));
    }

    /// <summary>Returns component-wise absolute value of this vector.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector2f AbsValue => new Vector2f(this.X.Abs(), this.Y.Abs());

    /// <summary>Returns component-wise sign of this vector.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector2i Signs => new Vector2i(this.X.Sign(), this.Y.Sign());

    /// <summary>
    /// Returns component-wise modulo operation on this vector (see <see cref="M:Mafi.MafiMath.Modulo(System.Int32,System.Int32)" />).
    /// </summary>
    [Pure]
    public Vector2f Modulo(Fix32 mod) => new Vector2f(this.X.Modulo(mod), this.Y.Modulo(mod));

    /// <summary>
    /// Returns component-wise modulo operation on this vector (see <see cref="M:Mafi.MafiMath.Modulo(System.Int32,System.Int32)" />).
    /// </summary>
    [Pure]
    public Vector2f Modulo(Vector2f mod)
    {
      return new Vector2f(this.X.Modulo(mod.X), this.Y.Modulo(mod.Y));
    }

    /// <summary>
    /// Returns component-wise average of this and given vectors.
    /// </summary>
    [Pure]
    public Vector2f Average(Vector2f rhs)
    {
      Fix32 fix32 = this.X + rhs.X;
      Fix32 halfFast1 = fix32.HalfFast;
      fix32 = this.Y + rhs.Y;
      Fix32 halfFast2 = fix32.HalfFast;
      return new Vector2f(halfFast1, halfFast2);
    }

    /// <summary>
    /// Linearly interpolates between this and <paramref name="to" /> vectors based on <paramref name="t" />.
    /// Interpolation parameter <paramref name="t" /> is expected to be from 0 to 1.
    /// </summary>
    [Pure]
    public Vector2f Lerp(Vector2f to, Fix32 t, Fix32 scale)
    {
      return new Vector2f(this.X.Lerp(to.X, t, scale), this.Y.Lerp(to.Y, t, scale));
    }

    [Pure]
    public Vector2f Lerp(Vector2f to, Percent t)
    {
      return new Vector2f(this.X.Lerp(to.X, t), this.Y.Lerp(to.Y, t));
    }

    [Pure]
    public bool Equals(Vector2f other) => other == this;

    [Pure]
    public override bool Equals(object other) => other is Vector2f vector2f && vector2f == this;

    [Pure]
    public override int GetHashCode() => this.X.GetHashCode() * 577 ^ this.Y.GetHashCode();

    [Pure]
    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder(20);
      stringBuilder.Append('(');
      stringBuilder.Append(this.X.ToString());
      stringBuilder.Append(", ");
      stringBuilder.Append(this.Y.ToString());
      stringBuilder.Append(')');
      return stringBuilder.ToString();
    }

    [Pure]
    public int CompareTo(Vector2f other)
    {
      if (this.X < other.X)
        return -1;
      if (this.X > other.X)
        return 1;
      if (this.Y < other.Y)
        return -1;
      return this.Y > other.Y ? 1 : 0;
    }

    /// <summary>Exact equality of two vectors.</summary>
    public static bool operator ==(Vector2f lhs, Vector2f rhs) => lhs.X == rhs.X && lhs.Y == rhs.Y;

    /// <summary>Exact inequality of two vectors.</summary>
    public static bool operator !=(Vector2f lhs, Vector2f rhs) => lhs.X != rhs.X || lhs.Y != rhs.Y;

    /// <summary>
    /// Component-wise less-than operator. Returns true if all components of the left-hand side vector are less than
    /// respective components of the right-hand side vector.
    /// WARNING: <c>A &lt; B</c> is not equal to <c>A &gt;= B</c>. For example vectors (1, 2) and (2, 1) are not
    /// less-than nor greater-than-or-equal.
    /// </summary>
    public static bool operator <(Vector2f lhs, Vector2f rhs) => lhs.X < rhs.X && lhs.Y < rhs.Y;

    /// <summary>
    /// Component-wise less-than-or-equal operator. Returns true if all components of the left-hand side vector are
    /// less than or equal to respective components of the right-hand side vector.
    /// WARNING: <c>A &lt;= B</c> is not equal to <c>A &gt; B</c>. For example vectors (1, 2) and (2, 1) are not
    /// less-than-or-equal nor greater-than.
    /// </summary>
    public static bool operator <=(Vector2f lhs, Vector2f rhs) => lhs.X <= rhs.X && lhs.Y <= rhs.Y;

    /// <summary>
    /// Component-wise greater-than operator. Returns true if all components of the left-hand side vector are
    /// greater than respective components of the right-hand side vector.
    /// WARNING: <c>A &gt; B</c> is not equal to <c>A &lt;= B</c>. For example vectors (1, 2) and (2, 1) are not
    /// greater-than nor less-than-or-equal.
    /// </summary>
    public static bool operator >(Vector2f lhs, Vector2f rhs) => lhs.X > rhs.X && lhs.Y > rhs.Y;

    /// <summary>
    /// Component-wise greater-than-or-equal operator. Returns true if all components of the left-hand side vector
    /// are greater than or equal to respective components of the right-hand side vector.
    /// WARNING: <c>A &gt;= B</c> is not equal to <c>A &lt; B</c>. For example vectors (1, 2) and (2, 1) are not
    /// greater-than-or-equal nor less-than.
    /// </summary>
    public static bool operator >=(Vector2f lhs, Vector2f rhs) => lhs.X >= rhs.X && lhs.Y >= rhs.Y;

    public static Vector2f operator +(Fix32 lhs, Vector2f rhs)
    {
      return new Vector2f(lhs + rhs.X, lhs + rhs.Y);
    }

    public static Vector2f operator +(Vector2f lhs, Fix32 rhs)
    {
      return new Vector2f(lhs.X + rhs, lhs.Y + rhs);
    }

    public static Vector2f operator +(Vector2f lhs, Vector2f rhs)
    {
      return new Vector2f(lhs.X + rhs.X, lhs.Y + rhs.Y);
    }

    public static Vector2f operator +(Vector2i lhs, Vector2f rhs)
    {
      return new Vector2f((Fix32) lhs.X + rhs.X, (Fix32) lhs.Y + rhs.Y);
    }

    public static Vector2f operator +(Vector2f lhs, Vector2i rhs)
    {
      return new Vector2f(lhs.X + (Fix32) rhs.X, lhs.Y + (Fix32) rhs.Y);
    }

    public static Vector2f operator -(Vector2f vector) => new Vector2f(-vector.X, -vector.Y);

    public static Vector2f operator -(Fix32 lhs, Vector2f rhs)
    {
      return new Vector2f(lhs - rhs.X, lhs - rhs.Y);
    }

    public static Vector2f operator -(Vector2f lhs, Fix32 rhs)
    {
      return new Vector2f(lhs.X - rhs, lhs.Y - rhs);
    }

    public static Vector2f operator -(Vector2f lhs, Vector2f rhs)
    {
      return new Vector2f(lhs.X - rhs.X, lhs.Y - rhs.Y);
    }

    public static Vector2f operator -(Vector2i lhs, Vector2f rhs)
    {
      return new Vector2f((Fix32) lhs.X - rhs.X, (Fix32) lhs.Y - rhs.Y);
    }

    public static Vector2f operator -(Vector2f lhs, Vector2i rhs)
    {
      return new Vector2f(lhs.X - (Fix32) rhs.X, lhs.Y - (Fix32) rhs.Y);
    }

    public static Vector2f operator *(Vector2f lhs, Fix32 rhs)
    {
      return new Vector2f(lhs.X * rhs, lhs.Y * rhs);
    }

    public static Vector2f operator *(Fix32 lhs, Vector2f rhs)
    {
      return new Vector2f(lhs * rhs.X, lhs * rhs.Y);
    }

    public static Vector2f operator *(Vector2f lhs, int rhs)
    {
      return new Vector2f(lhs.X * rhs, lhs.Y * rhs);
    }

    public static Vector2f operator *(int lhs, Vector2f rhs)
    {
      return new Vector2f(rhs.X * lhs, rhs.Y * lhs);
    }

    public static Vector2f operator *(Vector2f lhs, Percent rhs)
    {
      return new Vector2f(rhs.Apply(lhs.X), rhs.Apply(lhs.Y));
    }

    public static Vector2f operator *(Percent lhs, Vector2f rhs)
    {
      return new Vector2f(lhs.Apply(rhs.X), lhs.Apply(rhs.Y));
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector2f Times2Fast => new Vector2f(this.X.Times2Fast, this.Y.Times2Fast);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector2f Times4Fast => new Vector2f(this.X.Times4Fast, this.Y.Times4Fast);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector2f Times8Fast => new Vector2f(this.X.Times8Fast, this.Y.Times8Fast);

    public static Vector2f operator /(Fix32 lhs, Vector2f rhs)
    {
      return new Vector2f(lhs / rhs.X, lhs / rhs.Y);
    }

    public static Vector2f operator /(Vector2f lhs, Fix32 rhs)
    {
      return new Vector2f(lhs.X / rhs, lhs.Y / rhs);
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector2f HalfFast => new Vector2f(this.X.HalfFast, this.Y.HalfFast);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector2f DivBy4Fast => new Vector2f(this.X.DivBy4Fast, this.Y.DivBy4Fast);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector2f DivBy8Fast => new Vector2f(this.X.DivBy8Fast, this.Y.DivBy8Fast);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector2f DivBy16Fast => new Vector2f(this.X.DivBy16Fast, this.Y.DivBy16Fast);

    /// <summary>Component-wise division of two vectors.</summary>
    public static Vector2f operator /(Vector2f lhs, Vector2f rhs)
    {
      return new Vector2f(lhs.X / rhs.X, lhs.Y / rhs.Y);
    }

    /// <summary>Component-wise division of two vectors.</summary>
    public static Vector2f operator /(Vector2i lhs, Vector2f rhs)
    {
      return new Vector2f(lhs.X / rhs.X, lhs.Y / rhs.Y);
    }

    /// <summary>Component-wise division of two vectors.</summary>
    public static Vector2f operator /(Vector2f lhs, Vector2i rhs)
    {
      return new Vector2f(lhs.X / rhs.X, lhs.Y / rhs.Y);
    }

    public static Vector2f operator %(Fix32 lhs, Vector2f rhs)
    {
      return new Vector2f(lhs % rhs.X, lhs % rhs.Y);
    }

    public static Vector2f operator %(Vector2f lhs, Fix32 rhs)
    {
      return new Vector2f(lhs.X % rhs, lhs.Y % rhs);
    }

    /// <summary>Component-wise modulo of two vectors.</summary>
    public static Vector2f operator %(Vector2f lhs, Vector2f rhs)
    {
      return new Vector2f(lhs.X % rhs.X, lhs.Y % rhs.Y);
    }

    /// <summary>Component-wise modulo of two vectors.</summary>
    public static Vector2f operator %(Vector2i lhs, Vector2f rhs)
    {
      return new Vector2f((Fix32) lhs.X % rhs.X, (Fix32) lhs.Y % rhs.Y);
    }

    /// <summary>Component-wise modulo of two vectors.</summary>
    public static Vector2f operator %(Vector2f lhs, Vector2i rhs)
    {
      return new Vector2f(lhs.X % (Fix32) rhs.X, lhs.Y % (Fix32) rhs.Y);
    }

    public static void Serialize(Vector2f value, BlobWriter writer)
    {
      Fix32.Serialize(value.X, writer);
      Fix32.Serialize(value.Y, writer);
    }

    public static Vector2f Deserialize(BlobReader reader)
    {
      return new Vector2f(Fix32.Deserialize(reader), Fix32.Deserialize(reader));
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector2f Yx => new Vector2f(this.Y, this.X);

    /// <summary>Returns dot product of this vector with itslef.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Fix64 DotSelf => this.X.MultAsFix64(this.X) + this.Y.MultAsFix64(this.Y);

    [Pure]
    public Fix32 DotAsFix32(Vector2f rhs) => this.X * rhs.X + this.Y * rhs.Y;

    static Vector2f()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      Vector2f.Zero = new Vector2f();
      Vector2f.One = new Vector2f(Fix32.FromDouble(1.0), Fix32.FromDouble(1.0));
      Vector2f.UnitX = new Vector2f(Fix32.FromDouble(1.0), Fix32.FromDouble(0.0));
      Vector2f.UnitY = new Vector2f(Fix32.FromDouble(0.0), Fix32.FromDouble(1.0));
      Vector2f.MinValue = new Vector2f(Fix32.MinValue, Fix32.MinValue);
      Vector2f.MaxValue = new Vector2f(Fix32.MaxValue, Fix32.MaxValue);
    }
  }
}
