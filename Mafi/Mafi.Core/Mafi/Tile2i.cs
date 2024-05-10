// Decompiled with JetBrains decompiler
// Type: Mafi.Tile2i
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Terrain;
using Mafi.Serialization;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

#nullable disable
namespace Mafi
{
  /// <summary>
  /// Immutable 2D global tile coordinate. Depending on context, this can represent a terrain vertex (corner of a tile)
  /// or an entire tile area. Consider using <see cref="T:Mafi.Tile2iSlim" /> in cases where there is no worry about overflows
  /// when doing math on the coordinates.
  /// </summary>
  [ManuallyWrittenSerialization]
  [DebuggerDisplay("({X}, {Y})")]
  [DebuggerStepThrough]
  [StructLayout(LayoutKind.Explicit)]
  public readonly struct Tile2i : IEquatable<Tile2i>, IComparable<Tile2i>
  {
    /// <summary>Vector (0, 0).</summary>
    public static readonly Tile2i Zero;
    /// <summary>Vector (1, 1).</summary>
    public static readonly Tile2i One;
    /// <summary>Vector (1, 0).</summary>
    public static readonly Tile2i UnitX;
    /// <summary>Vector (0, 1).</summary>
    public static readonly Tile2i UnitY;
    /// <summary>Vector (int.MinValue, int.MinValue).</summary>
    public static readonly Tile2i MinValue;
    /// <summary>Vector (int.MaxValue, int.MaxValue).</summary>
    public static readonly Tile2i MaxValue;
    /// <summary>The X component of this vector.</summary>
    [FieldOffset(0)]
    public readonly int X;
    /// <summary>The Y component of this vector.</summary>
    [FieldOffset(4)]
    public readonly int Y;
    /// <summary>Packed components.</summary>
    [FieldOffset(0)]
    public readonly ulong XyPacked;

    /// <summary>Creates new Tile2i from raw components.</summary>
    public Tile2i(int x, int y)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.XyPacked = 0UL;
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
    /// PERF: Expensive, uses sqrt. Consider using <see cref="P:Mafi.Tile2i.LengthSqr" /> whenever possible (when comparing
    /// lengths, etc.).
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Fix32 Length => Fix32.FromDouble(Math.Sqrt((double) this.LengthSqr));

    /// <summary>
    /// Approximate euclidean length of this vector as integer.
    /// PERF: Expensive, uses sqrt, consider using <see cref="P:Mafi.Tile2i.LengthSqr" /> whenever possible.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int LengthInt => (int) Math.Round(Math.Sqrt((double) this.LengthSqr));

    /// <summary>
    /// Euclidean length squared of this vector.
    /// PERF: Cheaper than <see cref="P:Mafi.Tile2i.Length" />, does not require expensive sqrt.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int LengthSqrInt => this.X * this.X + this.Y * this.Y;

    /// <summary>
    /// Euclidean length squared of this vector.
    /// PERF: Cheaper than <see cref="P:Mafi.Tile2i.Length" />, does not require expensive sqrt.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public long LengthSqr => (long) this.X * (long) this.X + (long) this.Y * (long) this.Y;

    /// <summary>Whether this vector has all components equal to zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsZero => this.XyPacked == 0UL;

    /// <summary>e
    /// Whether this vector has at least one components not equal to zero.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNotZero => this.XyPacked > 0UL;

    /// <summary>Returns new vector with changed X component.</summary>
    [Pure]
    public Tile2i SetX(int newX) => new Tile2i(newX, this.Y);

    /// <summary>Returns new vector with changed Y component.</summary>
    [Pure]
    public Tile2i SetY(int newY) => new Tile2i(this.X, newY);

    /// <summary>Extends this vector a new component.</summary>
    [Pure]
    public Tile3i ExtendZ(int z) => new Tile3i(this.X, this.Y, z);

    /// <summary>Returns new vector with incremented X component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile2i IncrementX => new Tile2i(this.X + 1, this.Y);

    /// <summary>Returns new vector with incremented Y component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile2i IncrementY => new Tile2i(this.X, this.Y + 1);

    /// <summary>Returns new vector with decremented X component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile2i DecrementX => new Tile2i(this.X - 1, this.Y);

    /// <summary>Returns new vector with decremented Y component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile2i DecrementY => new Tile2i(this.X, this.Y - 1);

    /// <summary>
    /// Returns new vector with given value added to the X component.
    /// </summary>
    [Pure]
    public Tile2i AddX(int addedX) => new Tile2i(this.X + addedX, this.Y);

    /// <summary>
    /// Returns new vector with given value added to the Y component.
    /// </summary>
    [Pure]
    public Tile2i AddY(int addedY) => new Tile2i(this.X, this.Y + addedY);

    /// <summary>
    /// Returns new vector with given value added to all components.
    /// </summary>
    [Pure]
    public Tile2i AddXy(int addedValue) => new Tile2i(this.X + addedValue, this.Y + addedValue);

    /// <summary>
    /// Returns new vector with given value multiplied with the X component.
    /// </summary>
    [Pure]
    public Tile2i MultiplyX(int multX) => new Tile2i(this.X * multX, this.Y);

    /// <summary>
    /// Returns new vector with given value multiplied with the Y component.
    /// </summary>
    [Pure]
    public Tile2i MultiplyY(int multY) => new Tile2i(this.X, this.Y * multY);

    /// <summary>
    /// Returns new vector with reflected X component (opposite sign).
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile2i ReflectX => new Tile2i(-this.X, this.Y);

    /// <summary>
    /// Returns new vector with reflected Y component (opposite sign).
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile2i ReflectY => new Tile2i(this.X, -this.Y);

    /// <summary>
    /// Multiples and divides all components. This method is using long precision to prevent int32 overflows.
    /// </summary>
    public Tile2i MulDiv(long mul, long div)
    {
      return new Tile2i((int) (mul * (long) this.X / div), (int) (mul * (long) this.Y / div));
    }

    /// <summary>
    /// Returns scaled vector to requested length. This method is more precise, faster and more intuitive than
    /// normalization followed by multiplication.
    /// WARNING: Setting length of integer vector may not produce exact requested length do to rounding error.
    /// </summary>
    [Pure]
    public Tile2i OfLength(int desiredLength)
    {
      double num1 = Math.Sqrt((double) this.LengthSqr);
      double num2 = (double) desiredLength / num1;
      return new Tile2i(((double) this.X * num2).RoundToInt(), ((double) this.Y * num2).RoundToInt());
    }

    /// <summary>
    /// Whether corresponding components of this and given vectors are within given tolerance.
    /// </summary>
    [Pure]
    public bool IsNear(Tile2i other, int tolerance)
    {
      return this.X.IsNear(other.X, tolerance) && this.Y.IsNear(other.Y, tolerance);
    }

    /// <summary>Returns dot product of this vector with given vector.</summary>
    [Pure]
    public long Dot(Tile2i rhs) => (long) this.X * (long) rhs.X + (long) this.Y * (long) rhs.Y;

    /// <summary>
    /// Returns dot product of this vector with given vector as int32. Note that result of this method may overflow
    /// if magnitude of any component is larger than ~30,000.
    /// </summary>
    [Pure]
    public int DotInt(Tile2i rhs) => this.X * rhs.X + this.Y * rhs.Y;

    /// <summary>
    /// Returns distance from this vector to the other vector.
    /// </summary>
    [Pure]
    public Fix32 DistanceTo(Tile2i other) => new Tile2i(this.X - other.X, this.Y - other.Y).Length;

    /// <summary>
    /// Returns squared distance from this vector to the other vector.
    /// </summary>
    [Pure]
    public long DistanceSqrTo(Tile2i other)
    {
      return new Tile2i(this.X - other.X, this.Y - other.Y).LengthSqr;
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
    public long PseudoCross(Tile2i other)
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
    public Tile2i Rotate(AngleDegrees1f angle)
    {
      Fix64 fix64_1 = angle.Cos();
      Fix64 fix64_2 = angle.Sin();
      Fix64 fix64_3 = this.X * fix64_1 - this.Y * fix64_2;
      int intRounded1 = fix64_3.ToIntRounded();
      fix64_3 = this.X * fix64_2 + this.Y * fix64_1;
      int intRounded2 = fix64_3.ToIntRounded();
      return new Tile2i(intRounded1, intRounded2);
    }

    /// <summary>
    /// Returns rotated vector by given angle. Positive angle values represent in counter-clockwise rotation. This
    /// means that <c>(1, 0).Rotate(90°) == (0, 1)</c>.
    /// </summary>
    [Pure]
    public Tile2i Rotate(Rotation90 angle)
    {
      switch (angle.AngleIndex)
      {
        case 0:
          return this;
        case 1:
          return new Tile2i(-this.Y, this.X);
        case 2:
          return new Tile2i(-this.X, -this.Y);
        case 3:
          return new Tile2i(this.Y, -this.X);
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
    public Tile2i Rotate(AngleDegrees1f angle, Tile2i pivot)
    {
      return (this - pivot).Rotate(angle) + pivot;
    }

    /// <summary>
    /// Returns signed angle from this vector to <paramref name="other" /> vector. Returned angle represents how much
    /// this vector has to be rotated to obtain <paramref name="other" /> vector. Returned value is [-τ/2, τ/2). This
    /// means that <c>v1.AngleTo(v2) == -v2.AngleTo(v1)</c> and <c>v1.Rotate(v1.AngleTo(v2)) == v2</c>.
    /// </summary>
    [Pure]
    public AngleDegrees1f AngleTo(Tile2i other)
    {
      Assert.That<Tile2i>(this).IsNotZero("AngleTo was called on zero vector.");
      Assert.That<Tile2i>(other).IsNotZero("AngleTo was called with zero vector.");
      return MafiMath.Atan2(this.PseudoCross(other), this.Dot(other));
    }

    /// <summary>
    /// Returns absolute angle between this and <see paramref="other" /> vectors. Returned angle is in range [0, τ/2].
    /// </summary>
    [Pure]
    public AngleDegrees1f AngleBetween(Tile2i other) => this.AngleTo(other).Abs;

    /// <summary>
    /// Whether this and <see paramref="other" /> vectors are parallel and not anti-parallel.
    /// </summary>
    [Pure]
    public bool IsParallelTo(Tile2i other)
    {
      Assert.That<Tile2i>(this).IsNotZero("IsParallelTo was called on zero vector.");
      Assert.That<Tile2i>(other).IsNotZero("IsParallelTo was called with zero vector.");
      return this.PseudoCross(other) == 0L && this.Dot(other) > 0L;
    }

    /// <summary>
    /// Whether this and <see paramref="other" /> vectors are anti-parallel and not parallel.
    /// </summary>
    [Pure]
    public bool IsAntiParallelTo(Tile2i other)
    {
      Assert.That<Tile2i>(this).IsNotZero("IsParallelTo was called on zero vector.");
      Assert.That<Tile2i>(other).IsNotZero("IsParallelTo was called with zero vector.");
      return this.PseudoCross(other) == 0L && this.Dot(other) < 0L;
    }

    /// <summary>
    /// Whether this and <see paramref="other" /> vectors are parallel or anti-parallel. This is more efficient than
    /// calling <see paramref="IsParallelTo" /> and <see paramref="IsAntiParallelTo" />.
    /// </summary>
    [Pure]
    public bool IsParallelOrAntiParallelTo(Tile2i other)
    {
      Assert.That<Tile2i>(this).IsNotZero("IsParallelTo was called on zero vector.");
      Assert.That<Tile2i>(other).IsNotZero("IsParallelTo was called with zero vector.");
      return this.PseudoCross(other) == 0L;
    }

    /// <summary>
    /// Returns this vector rotated by 90 degrees to the left (counter clockwise).
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile2i LeftOrthogonalVector => new Tile2i(-this.Y, this.X);

    /// <summary>
    /// Returns this vector rotated by 90 degrees to the right (clockwise).
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile2i RightOrthogonalVector => new Tile2i(this.Y, -this.X);

    /// <summary>Returns component-wise min of this and given vectors.</summary>
    [Pure]
    public Tile2i Min(Tile2i rhs) => new Tile2i(this.X.Min(rhs.X), this.Y.Min(rhs.Y));

    /// <summary>Returns component-wise min of this and given vectors.</summary>
    [Pure]
    public Tile2i Max(Tile2i rhs) => new Tile2i(this.X.Max(rhs.X), this.Y.Max(rhs.Y));

    /// <summary>Returns component-wise min of this and given vectors.</summary>
    [Pure]
    public int MinComponent() => this.X.Min(this.Y);

    /// <summary>Returns component-wise max of this and given vectors.</summary>
    [Pure]
    public int MaxComponent() => this.X.Max(this.Y);

    /// <summary>Returns component-wise clamp of this vectors.</summary>
    [Pure]
    public Tile2i Clamp(int min, int max)
    {
      return new Tile2i(this.X.Clamp(min, max), this.Y.Clamp(min, max));
    }

    /// <summary>
    /// Returns component-wise average of this and given vectors.
    /// </summary>
    [Pure]
    public Tile2i Average(Tile2i rhs) => new Tile2i(this.X + rhs.X >> 1, this.Y + rhs.Y >> 1);

    [Pure]
    public Tile2i Lerp(Tile2i to, Percent t)
    {
      return new Tile2i(this.X.Lerp(to.X, t), this.Y.Lerp(to.Y, t));
    }

    /// <summary>
    /// Linearly interpolates between this and <paramref name="to" /> vectors based on <paramref name="t" />.
    /// Interpolation parameter <paramref name="t" /> goes from 0 to <paramref name="scale" />.
    /// See <see cref="M:Mafi.MafiMath.Lerp(System.Int32,System.Int32,System.Int64,System.Int64)" /> for details.
    /// </summary>
    [Pure]
    public Tile2i Lerp(Tile2i to, long t, long scale)
    {
      return new Tile2i(this.X.Lerp(to.X, t, scale), this.Y.Lerp(to.Y, t, scale));
    }

    /// <summary>
    /// Linearly interpolates between <paramref name="from" /> and <paramref name="to" /> vectors based on <paramref name="t" />. Interpolation parameter <paramref name="t" /> goes from 0 to <paramref name="scale" />. See <see cref="M:Mafi.MafiMath.Lerp(System.Int32,System.Int32,System.Int64,System.Int64)" /> for details.
    /// </summary>
    public static Tile2i Lerp(Tile2i from, Tile2i to, long t, long scale)
    {
      return from.Lerp(to, t, scale);
    }

    [Pure]
    public bool Equals(Tile2i other) => other == this;

    [Pure]
    public override bool Equals(object other) => other is Tile2i tile2i && tile2i == this;

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
    public int CompareTo(Tile2i other)
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
    public static bool operator ==(Tile2i lhs, Tile2i rhs)
    {
      return (long) lhs.XyPacked == (long) rhs.XyPacked;
    }

    /// <summary>Exact inequality of two vectors.</summary>
    public static bool operator !=(Tile2i lhs, Tile2i rhs)
    {
      return (long) lhs.XyPacked != (long) rhs.XyPacked;
    }

    /// <summary>
    /// Component-wise less-than operator. Returns true if all components of the left-hand side vector are less than
    /// respective components of the right-hand side vector.
    /// WARNING: <c>A &lt; B</c> is not equal to <c>A &gt;= B</c>. For example vectors (1, 2) and (2, 1) are not
    /// less-than nor greater-than-or-equal.
    /// </summary>
    public static bool operator <(Tile2i lhs, Tile2i rhs) => lhs.X < rhs.X && lhs.Y < rhs.Y;

    /// <summary>
    /// Component-wise less-than-or-equal operator. Returns true if all components of the left-hand side vector are
    /// less than or equal to respective components of the right-hand side vector.
    /// WARNING: <c>A &lt;= B</c> is not equal to <c>A &gt; B</c>. For example vectors (1, 2) and (2, 1) are not
    /// less-than-or-equal nor greater-than.
    /// </summary>
    public static bool operator <=(Tile2i lhs, Tile2i rhs) => lhs.X <= rhs.X && lhs.Y <= rhs.Y;

    /// <summary>
    /// Component-wise greater-than operator. Returns true if all components of the left-hand side vector are
    /// greater than respective components of the right-hand side vector.
    /// WARNING: <c>A &gt; B</c> is not equal to <c>A &lt;= B</c>. For example vectors (1, 2) and (2, 1) are not
    /// greater-than nor less-than-or-equal.
    /// </summary>
    public static bool operator >(Tile2i lhs, Tile2i rhs) => lhs.X > rhs.X && lhs.Y > rhs.Y;

    /// <summary>
    /// Component-wise greater-than-or-equal operator. Returns true if all components of the left-hand side vector
    /// are greater than or equal to respective components of the right-hand side vector.
    /// WARNING: <c>A &gt;= B</c> is not equal to <c>A &lt; B</c>. For example vectors (1, 2) and (2, 1) are not
    /// greater-than-or-equal nor less-than.
    /// </summary>
    public static bool operator >=(Tile2i lhs, Tile2i rhs) => lhs.X >= rhs.X && lhs.Y >= rhs.Y;

    public static Tile2i operator +(int lhs, Tile2i rhs) => new Tile2i(lhs + rhs.X, lhs + rhs.Y);

    public static Tile2i operator +(Tile2i lhs, int rhs) => new Tile2i(lhs.X + rhs, lhs.Y + rhs);

    public static Tile2i operator +(Tile2i lhs, RelTile2i rhs)
    {
      return new Tile2i(lhs.X + rhs.X, lhs.Y + rhs.Y);
    }

    public static Tile2i operator +(RelTile2i lhs, Tile2i rhs)
    {
      return new Tile2i(lhs.X + rhs.X, lhs.Y + rhs.Y);
    }

    public static Tile2i operator -(Tile2i vector) => new Tile2i(-vector.X, -vector.Y);

    public static Tile2i operator -(int lhs, Tile2i rhs) => new Tile2i(lhs - rhs.X, lhs - rhs.Y);

    public static Tile2i operator -(Tile2i lhs, int rhs) => new Tile2i(lhs.X - rhs, lhs.Y - rhs);

    public static RelTile2i operator -(Tile2i lhs, Tile2i rhs)
    {
      return new RelTile2i(lhs.X - rhs.X, lhs.Y - rhs.Y);
    }

    public static Tile2i operator -(Tile2i lhs, RelTile2i rhs)
    {
      return new Tile2i(lhs.X - rhs.X, lhs.Y - rhs.Y);
    }

    public static Tile2i operator *(Tile2i lhs, int rhs) => new Tile2i(lhs.X * rhs, lhs.Y * rhs);

    public static Tile2i operator *(int lhs, Tile2i rhs) => new Tile2i(rhs.X * lhs, rhs.Y * lhs);

    public static Tile2i operator *(Tile2i lhs, Percent rhs)
    {
      return new Tile2i(rhs.Apply(lhs.X), rhs.Apply(lhs.Y));
    }

    public static Tile2i operator *(Percent lhs, Tile2i rhs)
    {
      return new Tile2i(lhs.Apply(rhs.X), lhs.Apply(rhs.Y));
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile2i Times2Fast => new Tile2i(this.X << 1, this.Y << 1);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile2i Times4Fast => new Tile2i(this.X << 2, this.Y << 2);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile2i Times8Fast => new Tile2i(this.X << 3, this.Y << 3);

    public static Tile2i operator /(int lhs, Tile2i rhs) => new Tile2i(lhs / rhs.X, lhs / rhs.Y);

    public static Tile2i operator /(Tile2i lhs, int rhs) => new Tile2i(lhs.X / rhs, lhs.Y / rhs);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile2i HalfFast => new Tile2i(this.X >> 1, this.Y >> 1);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile2i DivBy4Fast => new Tile2i(this.X >> 2, this.Y >> 2);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile2i DivBy8Fast => new Tile2i(this.X >> 3, this.Y >> 3);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile2i DivBy16Fast => new Tile2i(this.X >> 4, this.Y >> 4);

    /// <summary>
    /// Computes floor division. Unlike normal division operator in C# this always rounds down.
    /// </summary>
    public Tile2i FloorDiv(int rhs) => new Tile2i(this.X.FloorDiv(rhs), this.Y.FloorDiv(rhs));

    public Tile2i CeilDiv(int rhs) => new Tile2i(this.X.CeilDiv(rhs), this.Y.CeilDiv(rhs));

    public static Tile2i operator %(int lhs, Tile2i rhs) => new Tile2i(lhs % rhs.X, lhs % rhs.Y);

    public static Tile2i operator %(Tile2i lhs, int rhs) => new Tile2i(lhs.X % rhs, lhs.Y % rhs);

    public static void Serialize(Tile2i value, BlobWriter writer)
    {
      writer.WriteInt(value.X);
      writer.WriteInt(value.Y);
    }

    public static Tile2i Deserialize(BlobReader reader)
    {
      return new Tile2i(reader.ReadInt(), reader.ReadInt());
    }

    public Tile2i(Vector2i coord)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.XyPacked = 0UL;
      this.X = coord.X;
      this.Y = coord.Y;
    }

    public Tile2i(RelTile2i coord)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.XyPacked = 0UL;
      this.X = coord.X;
      this.Y = coord.Y;
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile2iSlim AsSlim => new Tile2iSlim((ushort) this.X, (ushort) this.Y);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector2f Vector2f => new Vector2f((Fix32) this.X, (Fix32) this.Y);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public RelTile2i RelTile2i => new RelTile2i(this.X, this.Y);

    /// <summary>
    /// Converts this global tile coordinate to integer coordinate of parent chunk.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Chunk2i ChunkCoord2i => new Chunk2i(this.X >> 6, this.Y >> 6);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Chunk2iSlim ChunkCoord2iSlim
    {
      get => new Chunk2iSlim((ushort) (this.X >> 6), (ushort) (this.Y >> 6));
    }

    /// <summary>
    /// Converts this global tile coordinate to integer relative coordinate of this tile within its parent chunk.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public TileInChunk2i TileInChunkCoord => new TileInChunk2i(this.X & 63, this.Y & 63);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public TileInChunk2iSlim TileInChunkCoordSlim
    {
      get => new TileInChunk2iSlim((byte) (this.X & 63), (byte) (this.Y & 63));
    }

    /// <summary>
    /// Converts this corner-based tile coordinate to fixed point corner-based representation.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile2f CornerTile2f => new Tile2f((Fix32) this.X, (Fix32) this.Y);

    /// <summary>
    /// Converts this corner-based tile coordinate to fixed point center-based representation.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile2f CenterTile2f
    {
      get => new Tile2f((Fix32) this.X + Fix32.Half, (Fix32) this.Y + Fix32.Half);
    }

    public static Tile2i operator +(Tile2i lhs, NeighborCoord rhs)
    {
      return new Tile2i(lhs.X + rhs.Dx, lhs.Y + rhs.Dy);
    }

    public static Tile2i operator +(NeighborCoord lhs, Tile2i rhs)
    {
      return new Tile2i(lhs.Dx + rhs.X, lhs.Dy + rhs.Y);
    }

    public static Tile2i operator &(Tile2i lhs, int rhs) => new Tile2i(lhs.X & rhs, lhs.Y & rhs);

    [Pure]
    public Tile2i FloorToMultipleOf(int value)
    {
      return new Tile2i(this.X.FloorToMultipleOf(value), this.Y.FloorToMultipleOf(value));
    }

    [Pure]
    public Tile2i CeilToMultipleOf(int value)
    {
      return new Tile2i(this.X.CeilToMultipleOf(value), this.Y.CeilToMultipleOf(value));
    }

    [Pure]
    public Tile3i ExtendHeight(HeightTilesI height) => new Tile3i(this.X, this.Y, height.Value);

    [Pure]
    public Tile2iAndIndex ExtendIndex(TerrainManager manager) => manager.ExtendTileIndex(this);

    [Pure]
    public Tile2i Transform(Matrix2i matrix) => new Tile2i(matrix.Transform(this.Vector2i));

    [Pure]
    public RelTile1i DistanceToOrtho(Tile2i other)
    {
      RelTile2i relTile2i = this - other;
      relTile2i = relTile2i.AbsValue;
      return new RelTile1i(relTile2i.Sum);
    }

    [Pure]
    public RelTile1i DistanceToInf(Tile2i other)
    {
      RelTile2i relTile2i = this - other;
      relTile2i = relTile2i.AbsValue;
      return new RelTile1i(relTile2i.MaxComponent());
    }

    [Pure]
    public override int GetHashCode() => this.X ^ this.Y << 16;

    static Tile2i()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Tile2i.Zero = new Tile2i();
      Tile2i.One = new Tile2i(1, 1);
      Tile2i.UnitX = new Tile2i(1, 0);
      Tile2i.UnitY = new Tile2i(0, 1);
      Tile2i.MinValue = new Tile2i(int.MinValue, int.MinValue);
      Tile2i.MaxValue = new Tile2i(int.MaxValue, int.MaxValue);
    }
  }
}
