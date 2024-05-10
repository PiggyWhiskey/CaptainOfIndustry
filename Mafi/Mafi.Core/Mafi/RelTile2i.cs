// Decompiled with JetBrains decompiler
// Type: Mafi.RelTile2i
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Serialization;
using System;
using System.Diagnostics;
using System.Text;

#nullable disable
namespace Mafi
{
  /// <summary>
  /// Immutable 2D relative tile coordinate. Represents relative offset from absolute coordinate.
  /// </summary>
  [DebuggerDisplay("({X}, {Y})")]
  [ManuallyWrittenSerialization]
  [DebuggerStepThrough]
  public struct RelTile2i : IEquatable<RelTile2i>, IComparable<RelTile2i>
  {
    /// <summary>Vector (0, 0).</summary>
    public static readonly RelTile2i Zero;
    /// <summary>Vector (1, 1).</summary>
    public static readonly RelTile2i One;
    /// <summary>Vector (1, 0).</summary>
    public static readonly RelTile2i UnitX;
    /// <summary>Vector (0, 1).</summary>
    public static readonly RelTile2i UnitY;
    /// <summary>Vector (int.MinValue, int.MinValue).</summary>
    public static readonly RelTile2i MinValue;
    /// <summary>Vector (int.MaxValue, int.MaxValue).</summary>
    public static readonly RelTile2i MaxValue;
    /// <summary>The X component of this vector.</summary>
    public readonly int X;
    /// <summary>The Y component of this vector.</summary>
    public readonly int Y;
    /// <summary>All 4 neighbors array.</summary>
    /// <remarks>We can not wrap this immediately because of type initialization loop.</remarks>
    private static readonly RelTile2i[] s_all4Neighbors;
    /// <summary>All 8 neighbors array.</summary>
    /// <remarks>We can not wrap this immediately because of type initialization loop.</remarks>
    private static readonly RelTile2i[] s_all8Neighbors;

    /// <summary>Creates new RelTile2i from raw components.</summary>
    public RelTile2i(int x, int y)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.X = x;
      this.Y = y;
    }

    /// <summary>Converts this type to Vector2i.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector2i Vector2i => new Vector2i(this.X, this.Y);

    /// <summary>Sum of all components of this vector.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int Sum => this.X + this.Y;

    /// <summary>Product of all components of this vector.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public long Product => (long) this.X * (long) this.Y;

    /// <summary>Product of all components of this vector.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int ProductInt
    {
      get
      {
        Assert.That<long>(this.Product).IsLessOrEqual((long) int.MaxValue, "Integer overflow while computing product as integer!");
        return this.X * this.Y;
      }
    }

    /// <summary>
    /// Euclidean length of this vector.
    /// PERF: Expensive, uses sqrt. Consider using <see cref="P:Mafi.RelTile2i.LengthSqr" /> whenever possible (when comparing
    /// lengths, etc.).
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Fix32 Length => Fix32.FromDouble(Math.Sqrt((double) this.LengthSqr));

    /// <summary>
    /// Approximate euclidean length of this vector as integer.
    /// PERF: Expensive, uses sqrt, consider using <see cref="P:Mafi.RelTile2i.LengthSqr" /> whenever possible.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int LengthInt => (int) Math.Round(Math.Sqrt((double) this.LengthSqr));

    /// <summary>
    /// Euclidean length squared of this vector.
    /// PERF: Cheaper than <see cref="P:Mafi.RelTile2i.Length" />, does not require expensive sqrt.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int LengthSqrInt => this.X * this.X + this.Y * this.Y;

    /// <summary>
    /// Euclidean length squared of this vector.
    /// PERF: Cheaper than <see cref="P:Mafi.RelTile2i.Length" />, does not require expensive sqrt.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public long LengthSqr => (long) this.X * (long) this.X + (long) this.Y * (long) this.Y;

    /// <summary>Whether this vector has all components equal to zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsZero => this.X == 0 && this.Y == 0;

    /// <summary>e
    /// Whether this vector has at least one components not equal to zero.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNotZero => this.X != 0 || this.Y != 0;

    /// <summary>Returns new vector with changed X component.</summary>
    [Pure]
    public RelTile2i SetX(int newX) => new RelTile2i(newX, this.Y);

    /// <summary>Returns new vector with changed Y component.</summary>
    [Pure]
    public RelTile2i SetY(int newY) => new RelTile2i(this.X, newY);

    /// <summary>Extends this vector a new component.</summary>
    [Pure]
    public RelTile3i ExtendZ(int z) => new RelTile3i(this.X, this.Y, z);

    /// <summary>Returns new vector with incremented X component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public RelTile2i IncrementX => new RelTile2i(this.X + 1, this.Y);

    /// <summary>Returns new vector with incremented Y component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public RelTile2i IncrementY => new RelTile2i(this.X, this.Y + 1);

    /// <summary>Returns new vector with decremented X component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public RelTile2i DecrementX => new RelTile2i(this.X - 1, this.Y);

    /// <summary>Returns new vector with decremented Y component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public RelTile2i DecrementY => new RelTile2i(this.X, this.Y - 1);

    /// <summary>
    /// Returns new vector with given value added to the X component.
    /// </summary>
    [Pure]
    public RelTile2i AddX(int addedX) => new RelTile2i(this.X + addedX, this.Y);

    /// <summary>
    /// Returns new vector with given value added to the Y component.
    /// </summary>
    [Pure]
    public RelTile2i AddY(int addedY) => new RelTile2i(this.X, this.Y + addedY);

    /// <summary>
    /// Returns new vector with given value added to all components.
    /// </summary>
    [Pure]
    public RelTile2i AddXy(int addedValue)
    {
      return new RelTile2i(this.X + addedValue, this.Y + addedValue);
    }

    /// <summary>
    /// Returns new vector with given value multiplied with the X component.
    /// </summary>
    [Pure]
    public RelTile2i MultiplyX(int multX) => new RelTile2i(this.X * multX, this.Y);

    /// <summary>
    /// Returns new vector with given value multiplied with the Y component.
    /// </summary>
    [Pure]
    public RelTile2i MultiplyY(int multY) => new RelTile2i(this.X, this.Y * multY);

    /// <summary>
    /// Returns new vector with reflected X component (opposite sign).
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public RelTile2i ReflectX => new RelTile2i(-this.X, this.Y);

    /// <summary>
    /// Returns new vector with reflected Y component (opposite sign).
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public RelTile2i ReflectY => new RelTile2i(this.X, -this.Y);

    /// <summary>
    /// Multiples and divides all components. This method is using long precision to prevent int32 overflows.
    /// </summary>
    public RelTile2i MulDiv(long mul, long div)
    {
      return new RelTile2i((int) (mul * (long) this.X / div), (int) (mul * (long) this.Y / div));
    }

    /// <summary>
    /// Returns scaled vector to requested length. This method is more precise, faster and more intuitive than
    /// normalization followed by multiplication.
    /// WARNING: Setting length of integer vector may not produce exact requested length do to rounding error.
    /// </summary>
    [Pure]
    public RelTile2i OfLength(int desiredLength)
    {
      double num1 = Math.Sqrt((double) this.LengthSqr);
      double num2 = (double) desiredLength / num1;
      return new RelTile2i(((double) this.X * num2).RoundToInt(), ((double) this.Y * num2).RoundToInt());
    }

    /// <summary>
    /// Whether corresponding components of this and given vectors are within given tolerance.
    /// </summary>
    [Pure]
    public bool IsNear(RelTile2i other, int tolerance)
    {
      return this.X.IsNear(other.X, tolerance) && this.Y.IsNear(other.Y, tolerance);
    }

    /// <summary>Returns dot product of this vector with given vector.</summary>
    [Pure]
    public long Dot(RelTile2i rhs) => (long) this.X * (long) rhs.X + (long) this.Y * (long) rhs.Y;

    /// <summary>
    /// Returns dot product of this vector with given vector as int32. Note that result of this method may overflow
    /// if magnitude of any component is larger than ~30,000.
    /// </summary>
    [Pure]
    public int DotInt(RelTile2i rhs) => this.X * rhs.X + this.Y * rhs.Y;

    /// <summary>
    /// Returns distance from this vector to the other vector.
    /// </summary>
    [Pure]
    public Fix32 DistanceTo(RelTile2i other)
    {
      return new RelTile2i(this.X - other.X, this.Y - other.Y).Length;
    }

    /// <summary>
    /// Returns squared distance from this vector to the other vector.
    /// </summary>
    [Pure]
    public long DistanceSqrTo(RelTile2i other)
    {
      return new RelTile2i(this.X - other.X, this.Y - other.Y).LengthSqr;
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
    public long PseudoCross(RelTile2i other)
    {
      return (long) this.X * (long) other.Y - (long) this.Y * (long) other.X;
    }

    /// <summary>
    /// Returns rotated vector by given angle. Positive angle values represent in counter-clockwise rotation. This
    /// means that <c>(1, 0).Rotate(90°) == (0, 1)</c>.
    /// WARNING: Please keep in mind that rotating integer vectors may not be precise for vectors with small
    /// magnitudes due to rounding errors.
    /// </summary>
    [Pure]
    public RelTile2i Rotate(AngleDegrees1f angle)
    {
      Fix64 fix64_1 = angle.Cos();
      Fix64 fix64_2 = angle.Sin();
      Fix64 fix64_3 = this.X * fix64_1 - this.Y * fix64_2;
      int intRounded1 = fix64_3.ToIntRounded();
      fix64_3 = this.X * fix64_2 + this.Y * fix64_1;
      int intRounded2 = fix64_3.ToIntRounded();
      return new RelTile2i(intRounded1, intRounded2);
    }

    /// <summary>
    /// Returns rotated vector by given angle. Positive angle values represent in counter-clockwise rotation. This
    /// means that <c>(1, 0).Rotate(90°) == (0, 1)</c>.
    /// </summary>
    [Pure]
    public RelTile2i Rotate(Rotation90 angle)
    {
      switch (angle.AngleIndex)
      {
        case 0:
          return this;
        case 1:
          return new RelTile2i(-this.Y, this.X);
        case 2:
          return new RelTile2i(-this.X, -this.Y);
        case 3:
          return new RelTile2i(this.Y, -this.X);
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
    public RelTile2i Rotate(AngleDegrees1f angle, RelTile2i pivot)
    {
      return (this - pivot).Rotate(angle) + pivot;
    }

    /// <summary>
    /// Returns signed angle from this vector to <paramref name="other" /> vector. Returned angle represents how much
    /// this vector has to be rotated to obtain <paramref name="other" /> vector. Returned value is [-τ/2, τ/2). This
    /// means that <c>v1.AngleTo(v2) == -v2.AngleTo(v1)</c> and <c>v1.Rotate(v1.AngleTo(v2)) == v2</c>.
    /// </summary>
    [Pure]
    public AngleDegrees1f AngleTo(RelTile2i other)
    {
      Assert.That<RelTile2i>(this).IsNotZero("AngleTo was called on zero vector.");
      Assert.That<RelTile2i>(other).IsNotZero("AngleTo was called with zero vector.");
      return MafiMath.Atan2(this.PseudoCross(other), this.Dot(other));
    }

    /// <summary>
    /// Returns absolute angle between this and <see paramref="other" /> vectors. Returned angle is in range [0, τ/2].
    /// </summary>
    [Pure]
    public AngleDegrees1f AngleBetween(RelTile2i other) => this.AngleTo(other).Abs;

    /// <summary>
    /// Whether this and <see paramref="other" /> vectors are parallel and not anti-parallel.
    /// </summary>
    [Pure]
    public bool IsParallelTo(RelTile2i other)
    {
      Assert.That<RelTile2i>(this).IsNotZero("IsParallelTo was called on zero vector.");
      Assert.That<RelTile2i>(other).IsNotZero("IsParallelTo was called with zero vector.");
      return this.PseudoCross(other) == 0L && this.Dot(other) > 0L;
    }

    /// <summary>
    /// Whether this and <see paramref="other" /> vectors are anti-parallel and not parallel.
    /// </summary>
    [Pure]
    public bool IsAntiParallelTo(RelTile2i other)
    {
      Assert.That<RelTile2i>(this).IsNotZero("IsParallelTo was called on zero vector.");
      Assert.That<RelTile2i>(other).IsNotZero("IsParallelTo was called with zero vector.");
      return this.PseudoCross(other) == 0L && this.Dot(other) < 0L;
    }

    /// <summary>
    /// Whether this and <see paramref="other" /> vectors are parallel or anti-parallel. This is more efficient than
    /// calling <see paramref="IsParallelTo" /> and <see paramref="IsAntiParallelTo" />.
    /// </summary>
    [Pure]
    public bool IsParallelOrAntiParallelTo(RelTile2i other)
    {
      Assert.That<RelTile2i>(this).IsNotZero("IsParallelTo was called on zero vector.");
      Assert.That<RelTile2i>(other).IsNotZero("IsParallelTo was called with zero vector.");
      return this.PseudoCross(other) == 0L;
    }

    /// <summary>
    /// Returns this vector rotated by 90 degrees to the left (counter clockwise).
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public RelTile2i LeftOrthogonalVector => new RelTile2i(-this.Y, this.X);

    /// <summary>
    /// Returns this vector rotated by 90 degrees to the right (clockwise).
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public RelTile2i RightOrthogonalVector => new RelTile2i(this.Y, -this.X);

    /// <summary>Returns component-wise min of this and given vectors.</summary>
    [Pure]
    public RelTile2i Min(RelTile2i rhs) => new RelTile2i(this.X.Min(rhs.X), this.Y.Min(rhs.Y));

    /// <summary>Returns component-wise min of this and given vectors.</summary>
    [Pure]
    public RelTile2i Max(RelTile2i rhs) => new RelTile2i(this.X.Max(rhs.X), this.Y.Max(rhs.Y));

    /// <summary>Returns component-wise min of this and given vectors.</summary>
    [Pure]
    public int MinComponent() => this.X.Min(this.Y);

    /// <summary>Returns component-wise max of this and given vectors.</summary>
    [Pure]
    public int MaxComponent() => this.X.Max(this.Y);

    /// <summary>Returns component-wise clamp of this vectors.</summary>
    [Pure]
    public RelTile2i Clamp(int min, int max)
    {
      return new RelTile2i(this.X.Clamp(min, max), this.Y.Clamp(min, max));
    }

    /// <summary>Returns component-wise absolute value of this vector.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public RelTile2i AbsValue => new RelTile2i(this.X.Abs(), this.Y.Abs());

    /// <summary>Returns component-wise sign of this vector.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public RelTile2i Signs => new RelTile2i(this.X.Sign(), this.Y.Sign());

    /// <summary>
    /// Returns component-wise modulo operation on this vector (see <see cref="M:Mafi.MafiMath.Modulo(System.Int32,System.Int32)" />).
    /// </summary>
    [Pure]
    public RelTile2i Modulo(int mod) => new RelTile2i(this.X.Modulo(mod), this.Y.Modulo(mod));

    /// <summary>
    /// Returns component-wise modulo operation on this vector (see <see cref="M:Mafi.MafiMath.Modulo(System.Int32,System.Int32)" />).
    /// </summary>
    [Pure]
    public RelTile2i Modulo(RelTile2i mod)
    {
      return new RelTile2i(this.X.Modulo(mod.X), this.Y.Modulo(mod.Y));
    }

    /// <summary>
    /// Returns component-wise average of this and given vectors.
    /// </summary>
    [Pure]
    public RelTile2i Average(RelTile2i rhs)
    {
      return new RelTile2i(this.X + rhs.X >> 1, this.Y + rhs.Y >> 1);
    }

    [Pure]
    public RelTile2i Lerp(RelTile2i to, Percent t)
    {
      return new RelTile2i(this.X.Lerp(to.X, t), this.Y.Lerp(to.Y, t));
    }

    /// <summary>
    /// Linearly interpolates between this and <paramref name="to" /> vectors based on <paramref name="t" />.
    /// Interpolation parameter <paramref name="t" /> goes from 0 to <paramref name="scale" />.
    /// See <see cref="M:Mafi.MafiMath.Lerp(System.Int32,System.Int32,System.Int64,System.Int64)" /> for details.
    /// </summary>
    [Pure]
    public RelTile2i Lerp(RelTile2i to, long t, long scale)
    {
      return new RelTile2i(this.X.Lerp(to.X, t, scale), this.Y.Lerp(to.Y, t, scale));
    }

    /// <summary>
    /// Linearly interpolates between <paramref name="from" /> and <paramref name="to" /> vectors based on <paramref name="t" />. Interpolation parameter <paramref name="t" /> goes from 0 to <paramref name="scale" />. See <see cref="M:Mafi.MafiMath.Lerp(System.Int32,System.Int32,System.Int64,System.Int64)" /> for details.
    /// </summary>
    public static RelTile2i Lerp(RelTile2i from, RelTile2i to, long t, long scale)
    {
      return from.Lerp(to, t, scale);
    }

    [Pure]
    public bool Equals(RelTile2i other) => other == this;

    [Pure]
    public override bool Equals(object other) => other is RelTile2i relTile2i && relTile2i == this;

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
    public int CompareTo(RelTile2i other)
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
    public static bool operator ==(RelTile2i lhs, RelTile2i rhs)
    {
      return lhs.X == rhs.X && lhs.Y == rhs.Y;
    }

    /// <summary>Exact inequality of two vectors.</summary>
    public static bool operator !=(RelTile2i lhs, RelTile2i rhs)
    {
      return lhs.X != rhs.X || lhs.Y != rhs.Y;
    }

    /// <summary>
    /// Component-wise less-than operator. Returns true if all components of the left-hand side vector are less than
    /// respective components of the right-hand side vector.
    /// WARNING: <c>A &lt; B</c> is not equal to <c>A &gt;= B</c>. For example vectors (1, 2) and (2, 1) are not
    /// less-than nor greater-than-or-equal.
    /// </summary>
    public static bool operator <(RelTile2i lhs, RelTile2i rhs) => lhs.X < rhs.X && lhs.Y < rhs.Y;

    /// <summary>
    /// Component-wise less-than-or-equal operator. Returns true if all components of the left-hand side vector are
    /// less than or equal to respective components of the right-hand side vector.
    /// WARNING: <c>A &lt;= B</c> is not equal to <c>A &gt; B</c>. For example vectors (1, 2) and (2, 1) are not
    /// less-than-or-equal nor greater-than.
    /// </summary>
    public static bool operator <=(RelTile2i lhs, RelTile2i rhs)
    {
      return lhs.X <= rhs.X && lhs.Y <= rhs.Y;
    }

    /// <summary>
    /// Component-wise greater-than operator. Returns true if all components of the left-hand side vector are
    /// greater than respective components of the right-hand side vector.
    /// WARNING: <c>A &gt; B</c> is not equal to <c>A &lt;= B</c>. For example vectors (1, 2) and (2, 1) are not
    /// greater-than nor less-than-or-equal.
    /// </summary>
    public static bool operator >(RelTile2i lhs, RelTile2i rhs) => lhs.X > rhs.X && lhs.Y > rhs.Y;

    /// <summary>
    /// Component-wise greater-than-or-equal operator. Returns true if all components of the left-hand side vector
    /// are greater than or equal to respective components of the right-hand side vector.
    /// WARNING: <c>A &gt;= B</c> is not equal to <c>A &lt; B</c>. For example vectors (1, 2) and (2, 1) are not
    /// greater-than-or-equal nor less-than.
    /// </summary>
    public static bool operator >=(RelTile2i lhs, RelTile2i rhs)
    {
      return lhs.X >= rhs.X && lhs.Y >= rhs.Y;
    }

    public static RelTile2i operator +(int lhs, RelTile2i rhs)
    {
      return new RelTile2i(lhs + rhs.X, lhs + rhs.Y);
    }

    public static RelTile2i operator +(RelTile2i lhs, int rhs)
    {
      return new RelTile2i(lhs.X + rhs, lhs.Y + rhs);
    }

    public static RelTile2i operator +(RelTile2i lhs, RelTile2i rhs)
    {
      return new RelTile2i(lhs.X + rhs.X, lhs.Y + rhs.Y);
    }

    public static RelTile2i operator -(RelTile2i vector) => new RelTile2i(-vector.X, -vector.Y);

    public static RelTile2i operator -(int lhs, RelTile2i rhs)
    {
      return new RelTile2i(lhs - rhs.X, lhs - rhs.Y);
    }

    public static RelTile2i operator -(RelTile2i lhs, int rhs)
    {
      return new RelTile2i(lhs.X - rhs, lhs.Y - rhs);
    }

    public static RelTile2i operator -(RelTile2i lhs, RelTile2i rhs)
    {
      return new RelTile2i(lhs.X - rhs.X, lhs.Y - rhs.Y);
    }

    public static RelTile2i operator *(RelTile2i lhs, int rhs)
    {
      return new RelTile2i(lhs.X * rhs, lhs.Y * rhs);
    }

    public static RelTile2i operator *(int lhs, RelTile2i rhs)
    {
      return new RelTile2i(rhs.X * lhs, rhs.Y * lhs);
    }

    public static RelTile2i operator *(RelTile2i lhs, Percent rhs)
    {
      return new RelTile2i(rhs.Apply(lhs.X), rhs.Apply(lhs.Y));
    }

    public static RelTile2i operator *(Percent lhs, RelTile2i rhs)
    {
      return new RelTile2i(lhs.Apply(rhs.X), lhs.Apply(rhs.Y));
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public RelTile2i Times2Fast => new RelTile2i(this.X << 1, this.Y << 1);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public RelTile2i Times4Fast => new RelTile2i(this.X << 2, this.Y << 2);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public RelTile2i Times8Fast => new RelTile2i(this.X << 3, this.Y << 3);

    public static RelTile2i operator /(int lhs, RelTile2i rhs)
    {
      return new RelTile2i(lhs / rhs.X, lhs / rhs.Y);
    }

    public static RelTile2i operator /(RelTile2i lhs, int rhs)
    {
      return new RelTile2i(lhs.X / rhs, lhs.Y / rhs);
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public RelTile2i HalfFast => new RelTile2i(this.X >> 1, this.Y >> 1);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public RelTile2i DivBy4Fast => new RelTile2i(this.X >> 2, this.Y >> 2);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public RelTile2i DivBy8Fast => new RelTile2i(this.X >> 3, this.Y >> 3);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public RelTile2i DivBy16Fast => new RelTile2i(this.X >> 4, this.Y >> 4);

    /// <summary>
    /// Computes floor division. Unlike normal division operator in C# this always rounds down.
    /// </summary>
    public RelTile2i FloorDiv(int rhs) => new RelTile2i(this.X.FloorDiv(rhs), this.Y.FloorDiv(rhs));

    public RelTile2i CeilDiv(int rhs) => new RelTile2i(this.X.CeilDiv(rhs), this.Y.CeilDiv(rhs));

    /// <summary>Component-wise division of two vectors.</summary>
    public static RelTile2i operator /(RelTile2i lhs, RelTile2i rhs)
    {
      return new RelTile2i(lhs.X / rhs.X, lhs.Y / rhs.Y);
    }

    /// <summary>
    /// Computes floor division. Unlike normal division operator in C# this always rounds down.
    /// </summary>
    public RelTile2i FloorDiv(RelTile2i rhs)
    {
      return new RelTile2i(this.X.FloorDiv(rhs.X), this.Y.FloorDiv(rhs.Y));
    }

    public static RelTile2i operator %(int lhs, RelTile2i rhs)
    {
      return new RelTile2i(lhs % rhs.X, lhs % rhs.Y);
    }

    public static RelTile2i operator %(RelTile2i lhs, int rhs)
    {
      return new RelTile2i(lhs.X % rhs, lhs.Y % rhs);
    }

    /// <summary>Component-wise modulo of two vectors.</summary>
    public static RelTile2i operator %(RelTile2i lhs, RelTile2i rhs)
    {
      return new RelTile2i(lhs.X % rhs.X, lhs.Y % rhs.Y);
    }

    public static void Serialize(RelTile2i value, BlobWriter writer)
    {
      writer.WriteInt(value.X);
      writer.WriteInt(value.Y);
    }

    public static RelTile2i Deserialize(BlobReader reader)
    {
      return new RelTile2i(reader.ReadInt(), reader.ReadInt());
    }

    /// <summary>
    /// Helper array for looping over 4 neighbors at distance 1.
    /// </summary>
    public static ReadOnlyArray<RelTile2i> All4Neighbors
    {
      get => RelTile2i.s_all4Neighbors.AsReadOnlyArray<RelTile2i>();
    }

    /// <summary>
    /// Helper array for looping over 8 neighbors at distance 1.
    /// </summary>
    public static ReadOnlyArray<RelTile2i> All8Neighbors
    {
      get => RelTile2i.s_all8Neighbors.AsReadOnlyArray<RelTile2i>();
    }

    public RelTile2i(Vector2i vector2i)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.X = vector2i.X;
      this.Y = vector2i.Y;
    }

    public RelTile2i(RelTile1i x, RelTile1i y)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.X = x.Value;
      this.Y = y.Value;
    }

    public RelTile2i(int x, RelTile1i y)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.X = x;
      this.Y = y.Value;
    }

    public RelTile2i(RelTile1i x, int y)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.X = x.Value;
      this.Y = y;
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector2f Vector2f => new Vector2f((Fix32) this.X, (Fix32) this.Y);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public RelTile2f RelTile2f => new RelTile2f((Fix32) this.X, (Fix32) this.Y);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public RelTile2f RelTile2fCenter
    {
      get => new RelTile2f((Fix32) this.X + Fix32.Half, (Fix32) this.Y + Fix32.Half);
    }

    public Direction90 ToDirection90() => new Direction90(this.X, this.Y);

    [Pure]
    public RelTile3i ExtendHeight(ThicknessTilesI h) => new RelTile3i(this.X, this.Y, h.Value);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public float LengthAsFloat => (float) Math.Sqrt((double) this.LengthSqr);

    public static RelTile2f operator -(RelTile2i lhs, RelTile2f rhs)
    {
      return new RelTile2f((Fix32) lhs.X - rhs.X, (Fix32) lhs.Y - rhs.Y);
    }

    public static RelTile2f operator -(RelTile2f lhs, RelTile2i rhs)
    {
      return new RelTile2f(lhs.X - (Fix32) rhs.X, lhs.Y - (Fix32) rhs.Y);
    }

    public static RelTile2i operator &(RelTile2i lhs, int rhs)
    {
      return new RelTile2i(lhs.X & rhs, lhs.Y & rhs);
    }

    public static RelTile2i operator *(RelTile2i lhs, Vector2i rhs)
    {
      return new RelTile2i(lhs.X * rhs.X, lhs.Y * rhs.Y);
    }

    public readonly string ToStringWithTimes()
    {
      return string.Format("{0} × {1}", (object) this.X, (object) this.Y);
    }

    static RelTile2i()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      RelTile2i.Zero = new RelTile2i();
      RelTile2i.One = new RelTile2i(1, 1);
      RelTile2i.UnitX = new RelTile2i(1, 0);
      RelTile2i.UnitY = new RelTile2i(0, 1);
      RelTile2i.MinValue = new RelTile2i(int.MinValue, int.MinValue);
      RelTile2i.MaxValue = new RelTile2i(int.MaxValue, int.MaxValue);
      RelTile2i.s_all4Neighbors = new RelTile2i[4]
      {
        new RelTile2i(1, 0),
        new RelTile2i(0, 1),
        new RelTile2i(-1, 0),
        new RelTile2i(0, -1)
      };
      RelTile2i.s_all8Neighbors = new RelTile2i[8]
      {
        new RelTile2i(1, 0),
        new RelTile2i(1, 1),
        new RelTile2i(0, 1),
        new RelTile2i(-1, 1),
        new RelTile2i(-1, 0),
        new RelTile2i(-1, -1),
        new RelTile2i(0, -1),
        new RelTile2i(1, -1)
      };
    }
  }
}
