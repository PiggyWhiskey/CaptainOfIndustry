// Decompiled with JetBrains decompiler
// Type: Mafi.Vector4f
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
  /// <summary>Immutable 4D float vector.</summary>
  /// <remarks>
  /// This is partial struct and this file should contain only specific members for <see cref="T:Mafi.Vector4f" />. All general
  /// members should be added to the generator T4 template.
  /// </remarks>
  [DebuggerDisplay("({X}, {Y}, {Z}, {W})")]
  [DebuggerStepThrough]
  [ManuallyWrittenSerialization]
  public struct Vector4f : IEquatable<Vector4f>, IComparable<Vector4f>
  {
    /// <summary>Vector (0, 0, 0, 0).</summary>
    public static readonly Vector4f Zero;
    /// <summary>Vector (1, 1, 1, 1).</summary>
    public static readonly Vector4f One;
    /// <summary>Vector (1, 0, 0, 0).</summary>
    public static readonly Vector4f UnitX;
    /// <summary>Vector (0, 1, 0, 0).</summary>
    public static readonly Vector4f UnitY;
    /// <summary>Vector (0, 0, 1, 0).</summary>
    public static readonly Vector4f UnitZ;
    /// <summary>Vector (0, 0, 0, 1).</summary>
    public static readonly Vector4f UnitW;
    /// <summary>
    /// Vector (Fix32.MinValue, Fix32.MinValue, Fix32.MinValue, Fix32.MinValue).
    /// </summary>
    public static readonly Vector4f MinValue;
    /// <summary>
    /// Vector (Fix32.MaxValue, Fix32.MaxValue, Fix32.MaxValue, Fix32.MaxValue).
    /// </summary>
    public static readonly Vector4f MaxValue;
    /// <summary>The X component of this vector.</summary>
    public readonly Fix32 X;
    /// <summary>The Y component of this vector.</summary>
    public readonly Fix32 Y;
    /// <summary>The Z component of this vector.</summary>
    public readonly Fix32 Z;
    /// <summary>The W component of this vector.</summary>
    public readonly Fix32 W;

    /// <summary>Creates new Vector4f from raw components.</summary>
    public Vector4f(Fix32 x, Fix32 y, Fix32 z, Fix32 w)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.X = x;
      this.Y = y;
      this.Z = z;
      this.W = w;
    }

    /// <summary>
    /// Creates new Vector4f from Vector2f and raw components.
    /// </summary>
    public Vector4f(Vector2f vector, Fix32 z, Fix32 w)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.X = vector.X;
      this.Y = vector.Y;
      this.Z = z;
      this.W = w;
    }

    /// <summary>
    /// Creates new Vector4f from Vector3f and raw components.
    /// </summary>
    public Vector4f(Vector3f vector, Fix32 w)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.X = vector.X;
      this.Y = vector.Y;
      this.Z = vector.Z;
      this.W = w;
    }

    /// <summary>Gets the first two components of this vector.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector2f Xy => new Vector2f(this.X, this.Y);

    /// <summary>Gets the first three components of this vector.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector3f Xyz => new Vector3f(this.X, this.Y, this.Z);

    /// <summary>Sum of all components of this vector.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Fix32 Sum => this.X + this.Y + this.Z + this.W;

    /// <summary>
    /// Euclidean length of this vector.
    /// PERF: Expensive, uses sqrt. Consider using <see cref="P:Mafi.Vector4f.LengthSqr" /> whenever possible (when comparing
    /// lengths, etc.).
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Fix32 Length => this.LengthSqr.SqrtToFix32();

    /// <summary>
    /// Euclidean length squared of this vector.
    /// PERF: Cheaper than <see cref="P:Mafi.Vector4f.Length" />, does not require expensive sqrt.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Fix64 LengthSqr
    {
      get
      {
        return this.X.MultAsFix64(this.X) + this.Y.MultAsFix64(this.Y) + this.Z.MultAsFix64(this.Z) + this.W.MultAsFix64(this.W);
      }
    }

    /// <summary>Whether this vector has all components equal to zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsZero => this.X.IsZero && this.Y.IsZero && this.Z.IsZero && this.W.IsZero;

    /// <summary>e
    /// Whether this vector has at least one components not equal to zero.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNotZero
    {
      get => this.X.IsNotZero || this.Y.IsNotZero || this.Z.IsNotZero || this.W.IsNotZero;
    }

    /// <summary>Returns new vector with changed X component.</summary>
    [Pure]
    public Vector4f SetX(Fix32 newX) => new Vector4f(newX, this.Y, this.Z, this.W);

    /// <summary>Returns new vector with changed Y component.</summary>
    [Pure]
    public Vector4f SetY(Fix32 newY) => new Vector4f(this.X, newY, this.Z, this.W);

    /// <summary>Returns new vector with changed Z component.</summary>
    [Pure]
    public Vector4f SetZ(Fix32 newZ) => new Vector4f(this.X, this.Y, newZ, this.W);

    /// <summary>Returns new vector with changed W component.</summary>
    [Pure]
    public Vector4f SetW(Fix32 newW) => new Vector4f(this.X, this.Y, this.Z, newW);

    /// <summary>Returns new vector with changed X and Y components.</summary>
    [Pure]
    public Vector4f SetXy(Fix32 newX, Fix32 newY) => new Vector4f(newX, newY, this.Z, this.W);

    /// <summary>Returns new vector with changed X and Y components.</summary>
    [Pure]
    public Vector4f SetXy(Vector2f newXy) => new Vector4f(newXy.X, newXy.Y, this.Z, this.W);

    /// <summary>
    /// Returns new vector with changed X, Y, and Z components.
    /// </summary>
    [Pure]
    public Vector4f SetXyz(Fix32 newX, Fix32 newY, Fix32 newZ)
    {
      return new Vector4f(newX, newY, newZ, this.W);
    }

    /// <summary>
    /// Returns new vector with changed X, Y, and Z components.
    /// </summary>
    [Pure]
    public Vector4f SetXyz(Vector3f newXyz) => new Vector4f(newXyz.X, newXyz.Y, newXyz.Z, this.W);

    /// <summary>Returns new vector with incremented X component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector4f IncrementX => new Vector4f(this.X + (Fix32) 1, this.Y, this.Z, this.W);

    /// <summary>Returns new vector with incremented Y component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector4f IncrementY => new Vector4f(this.X, this.Y + (Fix32) 1, this.Z, this.W);

    /// <summary>Returns new vector with incremented Z component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector4f IncrementZ => new Vector4f(this.X, this.Y, this.Z + (Fix32) 1, this.W);

    /// <summary>Returns new vector with incremented W component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector4f IncrementW => new Vector4f(this.X, this.Y, this.Z, this.W + (Fix32) 1);

    /// <summary>Returns new vector with decremented X component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector4f DecrementX => new Vector4f(this.X - (Fix32) 1, this.Y, this.Z, this.W);

    /// <summary>Returns new vector with decremented Y component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector4f DecrementY => new Vector4f(this.X, this.Y - (Fix32) 1, this.Z, this.W);

    /// <summary>Returns new vector with decremented Z component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector4f DecrementZ => new Vector4f(this.X, this.Y, this.Z - (Fix32) 1, this.W);

    /// <summary>Returns new vector with decremented W component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector4f DecrementW => new Vector4f(this.X, this.Y, this.Z, this.W - (Fix32) 1);

    /// <summary>
    /// Returns new vector with given value added to the X component.
    /// </summary>
    [Pure]
    public Vector4f AddX(Fix32 addedX) => new Vector4f(this.X + addedX, this.Y, this.Z, this.W);

    /// <summary>
    /// Returns new vector with given value added to the Y component.
    /// </summary>
    [Pure]
    public Vector4f AddY(Fix32 addedY) => new Vector4f(this.X, this.Y + addedY, this.Z, this.W);

    /// <summary>
    /// Returns new vector with given value added to the Z component.
    /// </summary>
    [Pure]
    public Vector4f AddZ(Fix32 addedZ) => new Vector4f(this.X, this.Y, this.Z + addedZ, this.W);

    /// <summary>
    /// Returns new vector with given value added to the W component.
    /// </summary>
    [Pure]
    public Vector4f AddW(Fix32 addedW) => new Vector4f(this.X, this.Y, this.Z, this.W + addedW);

    /// <summary>
    /// Returns new vector with given value added to all components.
    /// </summary>
    [Pure]
    public Vector4f AddXyzw(Fix32 addedValue)
    {
      return new Vector4f(this.X + addedValue, this.Y + addedValue, this.Z + addedValue, this.W + addedValue);
    }

    /// <summary>
    /// Returns new vector with given value multiplied with the X component.
    /// </summary>
    [Pure]
    public Vector4f MultiplyX(Fix32 multX) => new Vector4f(this.X * multX, this.Y, this.Z, this.W);

    /// <summary>
    /// Returns new vector with given value multiplied with the Y component.
    /// </summary>
    [Pure]
    public Vector4f MultiplyY(Fix32 multY) => new Vector4f(this.X, this.Y * multY, this.Z, this.W);

    /// <summary>
    /// Returns new vector with given value multiplied with the Z component.
    /// </summary>
    [Pure]
    public Vector4f MultiplyZ(Fix32 multZ) => new Vector4f(this.X, this.Y, this.Z * multZ, this.W);

    /// <summary>
    /// Returns new vector with given value multiplied with the W component.
    /// </summary>
    [Pure]
    public Vector4f MultiplyW(Fix32 multW) => new Vector4f(this.X, this.Y, this.Z, this.W * multW);

    /// <summary>
    /// Returns new vector with reflected X component (opposite sign).
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector4f ReflectX => new Vector4f(-this.X, this.Y, this.Z, this.W);

    /// <summary>
    /// Returns new vector with reflected Y component (opposite sign).
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector4f ReflectY => new Vector4f(this.X, -this.Y, this.Z, this.W);

    /// <summary>
    /// Returns new vector with reflected Z component (opposite sign).
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector4f ReflectZ => new Vector4f(this.X, this.Y, -this.Z, this.W);

    /// <summary>
    /// Returns new vector with reflected W component (opposite sign).
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector4f ReflectW => new Vector4f(this.X, this.Y, this.Z, -this.W);

    /// <summary>Gets rounded Vector4i representation of this vector.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector4i RoundedVector4i
    {
      get
      {
        return new Vector4i(this.X.ToIntRounded(), this.Y.ToIntRounded(), this.Z.ToIntRounded(), this.W.ToIntRounded());
      }
    }

    /// <summary>Gets ceiled Vector4i representation of this vector.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector4i CeiledVector4i
    {
      get
      {
        return new Vector4i(this.X.ToIntCeiled(), this.Y.ToIntCeiled(), this.Z.ToIntCeiled(), this.W.ToIntCeiled());
      }
    }

    /// <summary>Gets floored Vector4i representation of this vector.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector4i FlooredVector4i
    {
      get
      {
        return new Vector4i(this.X.ToIntFloored(), this.Y.ToIntFloored(), this.Z.ToIntFloored(), this.W.ToIntFloored());
      }
    }

    /// <summary>
    /// Returns scaled vector to requested length. This method is faster and more intuitive than normalization
    /// followed by multiplication.
    /// </summary>
    [Pure]
    public Vector4f OfLength(Fix32 desiredLength)
    {
      Fix32 length = this.Length;
      Fix64 fix64 = this.X.MultAsFix64(desiredLength);
      Fix32 fix32_1 = fix64.DivToFix32(length);
      fix64 = this.Y.MultAsFix64(desiredLength);
      Fix32 fix32_2 = fix64.DivToFix32(length);
      fix64 = this.Z.MultAsFix64(desiredLength);
      Fix32 fix32_3 = fix64.DivToFix32(length);
      fix64 = this.W.MultAsFix64(desiredLength);
      Fix32 fix32_4 = fix64.DivToFix32(length);
      return new Vector4f(fix32_1, fix32_2, fix32_3, fix32_4);
    }

    [Pure]
    public bool IsNear(Vector4f other)
    {
      return this.X.IsNear(other.X) && this.Y.IsNear(other.Y) && this.Z.IsNear(other.Z) && this.W.IsNear(other.W);
    }

    /// <summary>
    /// Tests whether corresponding components of this and given vectors are within tolerance.
    /// </summary>
    [Pure]
    public bool IsNear(Vector4f other, Fix32 tolerance)
    {
      return this.X.IsNear(other.X, tolerance) && this.Y.IsNear(other.Y, tolerance) && this.Z.IsNear(other.Z, tolerance) && this.W.IsNear(other.W, tolerance);
    }

    [Pure]
    public bool IsNearZero()
    {
      return this.X.IsNearZero() && this.Y.IsNearZero() && this.Z.IsNearZero() && this.W.IsNearZero();
    }

    /// <summary>
    /// Whether this vector length is (nearly) one using default epsilon <see cref="F:Mafi.Fix32.EpsilonNear" />. Note that
    /// This uses efficient check of length squared without the need for square root computation.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNormalized => this.LengthSqr.IsNear(Fix64.One, Fix64.EpsilonFix32NearOneSqr);

    /// <summary>Returns normalized vector.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector4f Normalized
    {
      get
      {
        Fix64 lengthSqr = this.LengthSqr;
        if (lengthSqr.IsNearZero())
        {
          Assert.Fail("Normalizing (near) zero vector.");
          return Vector4f.Zero;
        }
        if (lengthSqr.IsNear(Fix64.One, Fix64.EpsilonFix32NearOneSqr))
          return this;
        Fix64 fix64_1 = lengthSqr.Sqrt();
        Vector4f normalized;
        ref Vector4f local = ref normalized;
        Fix64 fix64_2 = this.X / fix64_1;
        Fix32 fix32_1 = fix64_2.ToFix32();
        fix64_2 = this.Y / fix64_1;
        Fix32 fix32_2 = fix64_2.ToFix32();
        fix64_2 = this.Z / fix64_1;
        Fix32 fix32_3 = fix64_2.ToFix32();
        fix64_2 = this.W / fix64_1;
        Fix32 fix32_4 = fix64_2.ToFix32();
        local = new Vector4f(fix32_1, fix32_2, fix32_3, fix32_4);
        Assert.That<bool>(normalized.IsNormalized).IsTrue("Normalization failed, increase epsilon.");
        return normalized;
      }
    }

    /// <summary>Returns dot product of this vector with given vector.</summary>
    [Pure]
    public Fix64 Dot(Vector4f rhs)
    {
      return this.X.MultAsFix64(rhs.X) + this.Y.MultAsFix64(rhs.Y) + this.Z.MultAsFix64(rhs.Z) + this.W.MultAsFix64(rhs.W);
    }

    /// <summary>
    /// Returns distance from this vector to the other vector.
    /// </summary>
    [Pure]
    public Fix32 DistanceTo(Vector4f other)
    {
      return new Vector4f(this.X - other.X, this.Y - other.Y, this.Z - other.Z, this.W - other.W).Length;
    }

    /// <summary>
    /// Returns squared distance from this vector to the other vector.
    /// </summary>
    [Pure]
    public Fix64 DistanceSqrTo(Vector4f other)
    {
      return new Vector4f(this.X - other.X, this.Y - other.Y, this.Z - other.Z, this.W - other.W).LengthSqr;
    }

    /// <summary>
    /// Returns absolute angle of this vector. Returned angle is in range [-τ/2, τ/2].
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public AngleDegrees1f Angle => MafiMath.Atan2(this.Y, this.X);

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
    public Vector4f Reflect(Vector4f normal) => this - (2 * this.Dot(normal)).ToFix32() * normal;

    /// <summary>Returns component-wise min of this and given vectors.</summary>
    [Pure]
    public Vector4f Min(Vector4f rhs)
    {
      return new Vector4f(this.X.Min(rhs.X), this.Y.Min(rhs.Y), this.Z.Min(rhs.Z), this.W.Min(rhs.W));
    }

    /// <summary>Returns component-wise min of this and given vectors.</summary>
    [Pure]
    public Vector4f Max(Vector4f rhs)
    {
      return new Vector4f(this.X.Max(rhs.X), this.Y.Max(rhs.Y), this.Z.Max(rhs.Z), this.W.Max(rhs.W));
    }

    /// <summary>Returns component-wise min of this and given vectors.</summary>
    [Pure]
    public Fix32 MinComponent()
    {
      Fix32 fix32 = this.X.Min(this.Y);
      fix32 = fix32.Min(this.Z);
      return fix32.Min(this.W);
    }

    /// <summary>Returns component-wise max of this and given vectors.</summary>
    [Pure]
    public Fix32 MaxComponent()
    {
      Fix32 fix32 = this.X.Max(this.Y);
      fix32 = fix32.Max(this.Z);
      return fix32.Max(this.W);
    }

    /// <summary>Returns component-wise clamp of this vectors.</summary>
    [Pure]
    public Vector4f Clamp(Fix32 min, Fix32 max)
    {
      return new Vector4f(this.X.Clamp(min, max), this.Y.Clamp(min, max), this.Z.Clamp(min, max), this.W.Clamp(min, max));
    }

    /// <summary>Returns component-wise absolute value of this vector.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector4f AbsValue
    {
      get => new Vector4f(this.X.Abs(), this.Y.Abs(), this.Z.Abs(), this.W.Abs());
    }

    /// <summary>Returns component-wise sign of this vector.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector4i Signs
    {
      get => new Vector4i(this.X.Sign(), this.Y.Sign(), this.Z.Sign(), this.W.Sign());
    }

    /// <summary>
    /// Returns component-wise modulo operation on this vector (see <see cref="M:Mafi.MafiMath.Modulo(System.Int32,System.Int32)" />).
    /// </summary>
    [Pure]
    public Vector4f Modulo(Fix32 mod)
    {
      return new Vector4f(this.X.Modulo(mod), this.Y.Modulo(mod), this.Z.Modulo(mod), this.W.Modulo(mod));
    }

    /// <summary>
    /// Returns component-wise modulo operation on this vector (see <see cref="M:Mafi.MafiMath.Modulo(System.Int32,System.Int32)" />).
    /// </summary>
    [Pure]
    public Vector4f Modulo(Vector4f mod)
    {
      return new Vector4f(this.X.Modulo(mod.X), this.Y.Modulo(mod.Y), this.Z.Modulo(mod.Z), this.W.Modulo(mod.W));
    }

    /// <summary>
    /// Returns component-wise average of this and given vectors.
    /// </summary>
    [Pure]
    public Vector4f Average(Vector4f rhs)
    {
      Fix32 fix32 = this.X + rhs.X;
      Fix32 halfFast1 = fix32.HalfFast;
      fix32 = this.Y + rhs.Y;
      Fix32 halfFast2 = fix32.HalfFast;
      fix32 = this.Z + rhs.Z;
      Fix32 halfFast3 = fix32.HalfFast;
      fix32 = this.W + rhs.W;
      Fix32 halfFast4 = fix32.HalfFast;
      return new Vector4f(halfFast1, halfFast2, halfFast3, halfFast4);
    }

    /// <summary>
    /// Linearly interpolates between this and <paramref name="to" /> vectors based on <paramref name="t" />.
    /// Interpolation parameter <paramref name="t" /> is expected to be from 0 to 1.
    /// </summary>
    [Pure]
    public Vector4f Lerp(Vector4f to, Fix32 t, Fix32 scale)
    {
      return new Vector4f(this.X.Lerp(to.X, t, scale), this.Y.Lerp(to.Y, t, scale), this.Z.Lerp(to.Z, t, scale), this.W.Lerp(to.W, t, scale));
    }

    [Pure]
    public Vector4f Lerp(Vector4f to, Percent t)
    {
      return new Vector4f(this.X.Lerp(to.X, t), this.Y.Lerp(to.Y, t), this.Z.Lerp(to.Z, t), this.W.Lerp(to.W, t));
    }

    [Pure]
    public bool Equals(Vector4f other) => other == this;

    [Pure]
    public override bool Equals(object other) => other is Vector4f vector4f && vector4f == this;

    [Pure]
    public override int GetHashCode()
    {
      return ((this.X.GetHashCode() * 577 ^ this.Y.GetHashCode()) * 397 ^ this.Z.GetHashCode()) * 331 ^ this.W.GetHashCode();
    }

    [Pure]
    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder(36);
      stringBuilder.Append('(');
      stringBuilder.Append(this.X.ToString());
      stringBuilder.Append(", ");
      stringBuilder.Append(this.Y.ToString());
      stringBuilder.Append(", ");
      stringBuilder.Append(this.Z.ToString());
      stringBuilder.Append(", ");
      stringBuilder.Append(this.W.ToString());
      stringBuilder.Append(')');
      return stringBuilder.ToString();
    }

    [Pure]
    public int CompareTo(Vector4f other)
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
      if (this.Z > other.Z)
        return 1;
      if (this.W < other.W)
        return -1;
      return this.W > other.W ? 1 : 0;
    }

    /// <summary>Exact equality of two vectors.</summary>
    public static bool operator ==(Vector4f lhs, Vector4f rhs)
    {
      return lhs.X == rhs.X && lhs.Y == rhs.Y && lhs.Z == rhs.Z && lhs.W == rhs.W;
    }

    /// <summary>Exact inequality of two vectors.</summary>
    public static bool operator !=(Vector4f lhs, Vector4f rhs)
    {
      return lhs.X != rhs.X || lhs.Y != rhs.Y || lhs.Z != rhs.Z || lhs.W != rhs.W;
    }

    /// <summary>
    /// Component-wise less-than operator. Returns true if all components of the left-hand side vector are less than
    /// respective components of the right-hand side vector.
    /// WARNING: <c>A &lt; B</c> is not equal to <c>A &gt;= B</c>. For example vectors (1, 2, 3, 4) and (4, 3, 2, 1) are not
    /// less-than nor greater-than-or-equal.
    /// </summary>
    public static bool operator <(Vector4f lhs, Vector4f rhs)
    {
      return lhs.X < rhs.X && lhs.Y < rhs.Y && lhs.Z < rhs.Z && lhs.W < rhs.W;
    }

    /// <summary>
    /// Component-wise less-than-or-equal operator. Returns true if all components of the left-hand side vector are
    /// less than or equal to respective components of the right-hand side vector.
    /// WARNING: <c>A &lt;= B</c> is not equal to <c>A &gt; B</c>. For example vectors (1, 2, 3, 4) and (4, 3, 2, 1) are not
    /// less-than-or-equal nor greater-than.
    /// </summary>
    public static bool operator <=(Vector4f lhs, Vector4f rhs)
    {
      return lhs.X <= rhs.X && lhs.Y <= rhs.Y && lhs.Z <= rhs.Z && lhs.W <= rhs.W;
    }

    /// <summary>
    /// Component-wise greater-than operator. Returns true if all components of the left-hand side vector are
    /// greater than respective components of the right-hand side vector.
    /// WARNING: <c>A &gt; B</c> is not equal to <c>A &lt;= B</c>. For example vectors (1, 2, 3, 4) and (4, 3, 2, 1) are not
    /// greater-than nor less-than-or-equal.
    /// </summary>
    public static bool operator >(Vector4f lhs, Vector4f rhs)
    {
      return lhs.X > rhs.X && lhs.Y > rhs.Y && lhs.Z > rhs.Z && lhs.W > rhs.W;
    }

    /// <summary>
    /// Component-wise greater-than-or-equal operator. Returns true if all components of the left-hand side vector
    /// are greater than or equal to respective components of the right-hand side vector.
    /// WARNING: <c>A &gt;= B</c> is not equal to <c>A &lt; B</c>. For example vectors (1, 2, 3, 4) and (4, 3, 2, 1) are not
    /// greater-than-or-equal nor less-than.
    /// </summary>
    public static bool operator >=(Vector4f lhs, Vector4f rhs)
    {
      return lhs.X >= rhs.X && lhs.Y >= rhs.Y && lhs.Z >= rhs.Z && lhs.W >= rhs.W;
    }

    public static Vector4f operator +(Fix32 lhs, Vector4f rhs)
    {
      return new Vector4f(lhs + rhs.X, lhs + rhs.Y, lhs + rhs.Z, lhs + rhs.W);
    }

    public static Vector4f operator +(Vector4f lhs, Fix32 rhs)
    {
      return new Vector4f(lhs.X + rhs, lhs.Y + rhs, lhs.Z + rhs, lhs.W + rhs);
    }

    public static Vector4f operator +(Vector4f lhs, Vector4f rhs)
    {
      return new Vector4f(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z, lhs.W + rhs.W);
    }

    public static Vector4f operator +(Vector4i lhs, Vector4f rhs)
    {
      return new Vector4f((Fix32) lhs.X + rhs.X, (Fix32) lhs.Y + rhs.Y, (Fix32) lhs.Z + rhs.Z, (Fix32) lhs.W + rhs.W);
    }

    public static Vector4f operator +(Vector4f lhs, Vector4i rhs)
    {
      return new Vector4f(lhs.X + (Fix32) rhs.X, lhs.Y + (Fix32) rhs.Y, lhs.Z + (Fix32) rhs.Z, lhs.W + (Fix32) rhs.W);
    }

    public static Vector4f operator +(Vector4f lhs, Vector2f rhs)
    {
      return new Vector4f(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z, lhs.W);
    }

    public static Vector4f operator +(Vector4f lhs, Vector3f rhs)
    {
      return new Vector4f(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z, lhs.W);
    }

    public static Vector4f operator -(Vector4f vector)
    {
      return new Vector4f(-vector.X, -vector.Y, -vector.Z, -vector.W);
    }

    public static Vector4f operator -(Fix32 lhs, Vector4f rhs)
    {
      return new Vector4f(lhs - rhs.X, lhs - rhs.Y, lhs - rhs.Z, lhs - rhs.W);
    }

    public static Vector4f operator -(Vector4f lhs, Fix32 rhs)
    {
      return new Vector4f(lhs.X - rhs, lhs.Y - rhs, lhs.Z - rhs, lhs.W - rhs);
    }

    public static Vector4f operator -(Vector4f lhs, Vector4f rhs)
    {
      return new Vector4f(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z, lhs.W - rhs.W);
    }

    public static Vector4f operator -(Vector4i lhs, Vector4f rhs)
    {
      return new Vector4f((Fix32) lhs.X - rhs.X, (Fix32) lhs.Y - rhs.Y, (Fix32) lhs.Z - rhs.Z, (Fix32) lhs.W - rhs.W);
    }

    public static Vector4f operator -(Vector4f lhs, Vector4i rhs)
    {
      return new Vector4f(lhs.X - (Fix32) rhs.X, lhs.Y - (Fix32) rhs.Y, lhs.Z - (Fix32) rhs.Z, lhs.W - (Fix32) rhs.W);
    }

    public static Vector4f operator -(Vector4f lhs, Vector2f rhs)
    {
      return new Vector4f(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z, lhs.W);
    }

    public static Vector4f operator -(Vector4f lhs, Vector3f rhs)
    {
      return new Vector4f(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z, lhs.W);
    }

    public static Vector4f operator *(Vector4f lhs, Fix32 rhs)
    {
      return new Vector4f(lhs.X * rhs, lhs.Y * rhs, lhs.Z * rhs, lhs.W * rhs);
    }

    public static Vector4f operator *(Fix32 lhs, Vector4f rhs)
    {
      return new Vector4f(lhs * rhs.X, lhs * rhs.Y, lhs * rhs.Z, lhs * rhs.W);
    }

    public static Vector4f operator *(Vector4f lhs, int rhs)
    {
      return new Vector4f(lhs.X * rhs, lhs.Y * rhs, lhs.Z * rhs, lhs.W * rhs);
    }

    public static Vector4f operator *(int lhs, Vector4f rhs)
    {
      return new Vector4f(rhs.X * lhs, rhs.Y * lhs, rhs.Z * lhs, rhs.W * lhs);
    }

    public static Vector4f operator *(Vector4f lhs, Percent rhs)
    {
      return new Vector4f(rhs.Apply(lhs.X), rhs.Apply(lhs.Y), rhs.Apply(lhs.Z), rhs.Apply(lhs.W));
    }

    public static Vector4f operator *(Percent lhs, Vector4f rhs)
    {
      return new Vector4f(lhs.Apply(rhs.X), lhs.Apply(rhs.Y), lhs.Apply(rhs.Z), lhs.Apply(rhs.W));
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector4f Times2Fast
    {
      get
      {
        return new Vector4f(this.X.Times2Fast, this.Y.Times2Fast, this.Z.Times2Fast, this.W.Times2Fast);
      }
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector4f Times4Fast
    {
      get
      {
        return new Vector4f(this.X.Times4Fast, this.Y.Times4Fast, this.Z.Times4Fast, this.W.Times4Fast);
      }
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector4f Times8Fast
    {
      get
      {
        return new Vector4f(this.X.Times8Fast, this.Y.Times8Fast, this.Z.Times8Fast, this.W.Times8Fast);
      }
    }

    public static Vector4f operator /(Fix32 lhs, Vector4f rhs)
    {
      return new Vector4f(lhs / rhs.X, lhs / rhs.Y, lhs / rhs.Z, lhs / rhs.W);
    }

    public static Vector4f operator /(Vector4f lhs, Fix32 rhs)
    {
      return new Vector4f(lhs.X / rhs, lhs.Y / rhs, lhs.Z / rhs, lhs.W / rhs);
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector4f HalfFast
    {
      get => new Vector4f(this.X.HalfFast, this.Y.HalfFast, this.Z.HalfFast, this.W.HalfFast);
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector4f DivBy4Fast
    {
      get
      {
        return new Vector4f(this.X.DivBy4Fast, this.Y.DivBy4Fast, this.Z.DivBy4Fast, this.W.DivBy4Fast);
      }
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector4f DivBy8Fast
    {
      get
      {
        return new Vector4f(this.X.DivBy8Fast, this.Y.DivBy8Fast, this.Z.DivBy8Fast, this.W.DivBy8Fast);
      }
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector4f DivBy16Fast
    {
      get
      {
        return new Vector4f(this.X.DivBy16Fast, this.Y.DivBy16Fast, this.Z.DivBy16Fast, this.W.DivBy16Fast);
      }
    }

    /// <summary>Component-wise division of two vectors.</summary>
    public static Vector4f operator /(Vector4f lhs, Vector4f rhs)
    {
      return new Vector4f(lhs.X / rhs.X, lhs.Y / rhs.Y, lhs.Z / rhs.Z, lhs.W / rhs.W);
    }

    /// <summary>Component-wise division of two vectors.</summary>
    public static Vector4f operator /(Vector4i lhs, Vector4f rhs)
    {
      return new Vector4f(lhs.X / rhs.X, lhs.Y / rhs.Y, lhs.Z / rhs.Z, lhs.W / rhs.W);
    }

    /// <summary>Component-wise division of two vectors.</summary>
    public static Vector4f operator /(Vector4f lhs, Vector4i rhs)
    {
      return new Vector4f(lhs.X / rhs.X, lhs.Y / rhs.Y, lhs.Z / rhs.Z, lhs.W / rhs.W);
    }

    public static Vector4f operator %(Fix32 lhs, Vector4f rhs)
    {
      return new Vector4f(lhs % rhs.X, lhs % rhs.Y, lhs % rhs.Z, lhs % rhs.W);
    }

    public static Vector4f operator %(Vector4f lhs, Fix32 rhs)
    {
      return new Vector4f(lhs.X % rhs, lhs.Y % rhs, lhs.Z % rhs, lhs.W % rhs);
    }

    /// <summary>Component-wise modulo of two vectors.</summary>
    public static Vector4f operator %(Vector4f lhs, Vector4f rhs)
    {
      return new Vector4f(lhs.X % rhs.X, lhs.Y % rhs.Y, lhs.Z % rhs.Z, lhs.W % rhs.W);
    }

    /// <summary>Component-wise modulo of two vectors.</summary>
    public static Vector4f operator %(Vector4i lhs, Vector4f rhs)
    {
      return new Vector4f((Fix32) lhs.X % rhs.X, (Fix32) lhs.Y % rhs.Y, (Fix32) lhs.Z % rhs.Z, (Fix32) lhs.W % rhs.W);
    }

    /// <summary>Component-wise modulo of two vectors.</summary>
    public static Vector4f operator %(Vector4f lhs, Vector4i rhs)
    {
      return new Vector4f(lhs.X % (Fix32) rhs.X, lhs.Y % (Fix32) rhs.Y, lhs.Z % (Fix32) rhs.Z, lhs.W % (Fix32) rhs.W);
    }

    public static void Serialize(Vector4f value, BlobWriter writer)
    {
      Fix32.Serialize(value.X, writer);
      Fix32.Serialize(value.Y, writer);
      Fix32.Serialize(value.Z, writer);
      Fix32.Serialize(value.W, writer);
    }

    public static Vector4f Deserialize(BlobReader reader)
    {
      return new Vector4f(Fix32.Deserialize(reader), Fix32.Deserialize(reader), Fix32.Deserialize(reader), Fix32.Deserialize(reader));
    }

    [Pure]
    private static Vector4f CubicInterpolate(Vector4f[] data, int baseI, Percent t)
    {
      Vector4f vector4f1 = data[baseI - 1];
      Vector4f vector4f2 = data[baseI];
      Vector4f vector4f3 = data[baseI + 1];
      Vector4f vector4f4 = data[baseI + 2];
      return new Vector4f(MafiMath.CubicInterpolate(vector4f1.X, vector4f2.X, vector4f3.X, vector4f4.X, t), MafiMath.CubicInterpolate(vector4f1.Y, vector4f2.Y, vector4f3.Y, vector4f4.Y, t), MafiMath.CubicInterpolate(vector4f1.Z, vector4f2.Z, vector4f3.Z, vector4f4.Z, t), MafiMath.CubicInterpolate(vector4f1.W, vector4f2.W, vector4f3.W, vector4f4.W, t));
    }

    [Pure]
    public static Vector4f BicubicInterpolate(
      Vector4f[] data,
      int stride,
      int baseI,
      Percent tx,
      Percent ty)
    {
      Vector4f vector4f1 = Vector4f.CubicInterpolate(data, baseI - stride, tx);
      Vector4f vector4f2 = Vector4f.CubicInterpolate(data, baseI, tx);
      Vector4f vector4f3 = Vector4f.CubicInterpolate(data, baseI + stride, tx);
      Vector4f vector4f4 = Vector4f.CubicInterpolate(data, baseI + 2 * stride, tx);
      return new Vector4f(MafiMath.CubicInterpolate(vector4f1.X, vector4f2.X, vector4f3.X, vector4f4.X, ty), MafiMath.CubicInterpolate(vector4f1.Y, vector4f2.Y, vector4f3.Y, vector4f4.Y, ty), MafiMath.CubicInterpolate(vector4f1.Z, vector4f2.Z, vector4f3.Z, vector4f4.Z, ty), MafiMath.CubicInterpolate(vector4f1.W, vector4f2.W, vector4f3.W, vector4f4.W, ty));
    }

    static Vector4f()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      Vector4f.Zero = new Vector4f();
      Vector4f.One = new Vector4f(Fix32.FromDouble(1.0), Fix32.FromDouble(1.0), Fix32.FromDouble(1.0), Fix32.FromDouble(1.0));
      Vector4f.UnitX = new Vector4f(Fix32.FromDouble(1.0), Fix32.FromDouble(0.0), Fix32.FromDouble(0.0), Fix32.FromDouble(0.0));
      Vector4f.UnitY = new Vector4f(Fix32.FromDouble(0.0), Fix32.FromDouble(1.0), Fix32.FromDouble(0.0), Fix32.FromDouble(0.0));
      Vector4f.UnitZ = new Vector4f(Fix32.FromDouble(0.0), Fix32.FromDouble(0.0), Fix32.FromDouble(1.0), Fix32.FromDouble(0.0));
      Vector4f.UnitW = new Vector4f(Fix32.FromDouble(0.0), Fix32.FromDouble(0.0), Fix32.FromDouble(0.0), Fix32.FromDouble(1.0));
      Vector4f.MinValue = new Vector4f(Fix32.MinValue, Fix32.MinValue, Fix32.MinValue, Fix32.MinValue);
      Vector4f.MaxValue = new Vector4f(Fix32.MaxValue, Fix32.MaxValue, Fix32.MaxValue, Fix32.MaxValue);
    }
  }
}
