// Decompiled with JetBrains decompiler
// Type: Mafi.Tile3i
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;
using System.Diagnostics;
using System.Text;

#nullable disable
namespace Mafi
{
  /// <summary>
  /// Immutable 3D global tile coordinate that represents index of a 3D terrain tileS.
  /// </summary>
  [DebuggerStepThrough]
  [ManuallyWrittenSerialization]
  [DebuggerDisplay("({X}, {Y}, {Z})")]
  public struct Tile3i : IEquatable<Tile3i>, IComparable<Tile3i>
  {
    /// <summary>Vector (0, 0, 0).</summary>
    public static readonly Tile3i Zero;
    /// <summary>Vector (1, 1, 1).</summary>
    public static readonly Tile3i One;
    /// <summary>Vector (1, 0, 0).</summary>
    public static readonly Tile3i UnitX;
    /// <summary>Vector (0, 1, 0).</summary>
    public static readonly Tile3i UnitY;
    /// <summary>Vector (0, 0, 1).</summary>
    public static readonly Tile3i UnitZ;
    /// <summary>Vector (int.MinValue, int.MinValue, int.MinValue).</summary>
    public static readonly Tile3i MinValue;
    /// <summary>Vector (int.MaxValue, int.MaxValue, int.MaxValue).</summary>
    public static readonly Tile3i MaxValue;
    /// <summary>The X component of this vector.</summary>
    public readonly int X;
    /// <summary>The Y component of this vector.</summary>
    public readonly int Y;
    /// <summary>The Z component of this vector.</summary>
    public readonly int Z;

    /// <summary>Creates new Tile3i from raw components.</summary>
    public Tile3i(int x, int y, int z)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.X = x;
      this.Y = y;
      this.Z = z;
    }

    /// <summary>Creates new Tile3i from Tile2i and raw components.</summary>
    public Tile3i(Tile2i vector, int z)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.X = vector.X;
      this.Y = vector.Y;
      this.Z = z;
    }

    /// <summary>Gets the first two components of this vector.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile2i Xy => new Tile2i(this.X, this.Y);

    /// <summary>Converts this type to Vector3i.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector3i Vector3i => new Vector3i(this.X, this.Y, this.Z);

    /// <summary>Sum of all components of this vector.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int Sum => this.X + this.Y + this.Z;

    /// <summary>
    /// Euclidean length of this vector.
    /// PERF: Expensive, uses sqrt. Consider using <see cref="P:Mafi.Tile3i.LengthSqr" /> whenever possible (when comparing
    /// lengths, etc.).
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Fix32 Length => Fix32.FromDouble(Math.Sqrt((double) this.LengthSqr));

    /// <summary>
    /// Approximate euclidean length of this vector as integer.
    /// PERF: Expensive, uses sqrt, consider using <see cref="P:Mafi.Tile3i.LengthSqr" /> whenever possible.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int LengthInt => (int) Math.Round(Math.Sqrt((double) this.LengthSqr));

    /// <summary>
    /// Euclidean length squared of this vector.
    /// PERF: Cheaper than <see cref="P:Mafi.Tile3i.Length" />, does not require expensive sqrt.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int LengthSqrInt => this.X * this.X + this.Y * this.Y + this.Z * this.Z;

    /// <summary>
    /// Euclidean length squared of this vector.
    /// PERF: Cheaper than <see cref="P:Mafi.Tile3i.Length" />, does not require expensive sqrt.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public long LengthSqr
    {
      get
      {
        return (long) this.X * (long) this.X + (long) this.Y * (long) this.Y + (long) this.Z * (long) this.Z;
      }
    }

    /// <summary>Whether this vector has all components equal to zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsZero => this.X == 0 && this.Y == 0 && this.Z == 0;

    /// <summary>e
    /// Whether this vector has at least one components not equal to zero.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNotZero => this.X != 0 || this.Y != 0 || this.Z != 0;

    /// <summary>Returns new vector with changed X component.</summary>
    [Pure]
    public Tile3i SetX(int newX) => new Tile3i(newX, this.Y, this.Z);

    /// <summary>Returns new vector with changed Y component.</summary>
    [Pure]
    public Tile3i SetY(int newY) => new Tile3i(this.X, newY, this.Z);

    /// <summary>Returns new vector with changed Z component.</summary>
    [Pure]
    public Tile3i SetZ(int newZ) => new Tile3i(this.X, this.Y, newZ);

    /// <summary>Returns new vector with changed X and Y components.</summary>
    [Pure]
    public Tile3i SetXy(int newX, int newY) => new Tile3i(newX, newY, this.Z);

    /// <summary>Returns new vector with changed X and Y components.</summary>
    [Pure]
    public Tile3i SetXy(Tile2i newXy) => new Tile3i(newXy.X, newXy.Y, this.Z);

    /// <summary>
    /// Returns new vector with changed X, Y, and Z components.
    /// </summary>
    [Pure]
    public Tile3i SetXyz(int newX, int newY, int newZ) => new Tile3i(newX, newY, newZ);

    /// <summary>Returns new vector with incremented X component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile3i IncrementX => new Tile3i(this.X + 1, this.Y, this.Z);

    /// <summary>Returns new vector with incremented Y component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile3i IncrementY => new Tile3i(this.X, this.Y + 1, this.Z);

    /// <summary>Returns new vector with incremented Z component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile3i IncrementZ => new Tile3i(this.X, this.Y, this.Z + 1);

    /// <summary>Returns new vector with decremented X component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile3i DecrementX => new Tile3i(this.X - 1, this.Y, this.Z);

    /// <summary>Returns new vector with decremented Y component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile3i DecrementY => new Tile3i(this.X, this.Y - 1, this.Z);

    /// <summary>Returns new vector with decremented Z component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile3i DecrementZ => new Tile3i(this.X, this.Y, this.Z - 1);

    /// <summary>
    /// Returns new vector with given value added to the X component.
    /// </summary>
    [Pure]
    public Tile3i AddX(int addedX) => new Tile3i(this.X + addedX, this.Y, this.Z);

    /// <summary>
    /// Returns new vector with given value added to the Y component.
    /// </summary>
    [Pure]
    public Tile3i AddY(int addedY) => new Tile3i(this.X, this.Y + addedY, this.Z);

    /// <summary>
    /// Returns new vector with given value added to the Z component.
    /// </summary>
    [Pure]
    public Tile3i AddZ(int addedZ) => new Tile3i(this.X, this.Y, this.Z + addedZ);

    /// <summary>
    /// Returns new vector with given value added to all components.
    /// </summary>
    [Pure]
    public Tile3i AddXyz(int addedValue)
    {
      return new Tile3i(this.X + addedValue, this.Y + addedValue, this.Z + addedValue);
    }

    /// <summary>
    /// Returns new vector with given value multiplied with the X component.
    /// </summary>
    [Pure]
    public Tile3i MultiplyX(int multX) => new Tile3i(this.X * multX, this.Y, this.Z);

    /// <summary>
    /// Returns new vector with given value multiplied with the Y component.
    /// </summary>
    [Pure]
    public Tile3i MultiplyY(int multY) => new Tile3i(this.X, this.Y * multY, this.Z);

    /// <summary>
    /// Returns new vector with given value multiplied with the Z component.
    /// </summary>
    [Pure]
    public Tile3i MultiplyZ(int multZ) => new Tile3i(this.X, this.Y, this.Z * multZ);

    /// <summary>
    /// Returns new vector with reflected X component (opposite sign).
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile3i ReflectX => new Tile3i(-this.X, this.Y, this.Z);

    /// <summary>
    /// Returns new vector with reflected Y component (opposite sign).
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile3i ReflectY => new Tile3i(this.X, -this.Y, this.Z);

    /// <summary>
    /// Returns new vector with reflected Z component (opposite sign).
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile3i ReflectZ => new Tile3i(this.X, this.Y, -this.Z);

    /// <summary>
    /// Multiples and divides all components. This method is using long precision to prevent int32 overflows.
    /// </summary>
    public Tile3i MulDiv(long mul, long div)
    {
      return new Tile3i((int) (mul * (long) this.X / div), (int) (mul * (long) this.Y / div), (int) (mul * (long) this.Z / div));
    }

    /// <summary>
    /// Returns scaled vector to requested length. This method is more precise, faster and more intuitive than
    /// normalization followed by multiplication.
    /// WARNING: Setting length of integer vector may not produce exact requested length do to rounding error.
    /// </summary>
    [Pure]
    public Tile3i OfLength(int desiredLength)
    {
      double num1 = Math.Sqrt((double) this.LengthSqr);
      double num2 = (double) desiredLength / num1;
      return new Tile3i(((double) this.X * num2).RoundToInt(), ((double) this.Y * num2).RoundToInt(), ((double) this.Z * num2).RoundToInt());
    }

    /// <summary>
    /// Whether corresponding components of this and given vectors are within given tolerance.
    /// </summary>
    [Pure]
    public bool IsNear(Tile3i other, int tolerance)
    {
      return this.X.IsNear(other.X, tolerance) && this.Y.IsNear(other.Y, tolerance) && this.Z.IsNear(other.Z, tolerance);
    }

    /// <summary>Returns dot product of this vector with given vector.</summary>
    [Pure]
    public long Dot(Tile3i rhs)
    {
      return (long) this.X * (long) rhs.X + (long) this.Y * (long) rhs.Y + (long) this.Z * (long) rhs.Z;
    }

    /// <summary>
    /// Returns dot product of this vector with given vector as int32. Note that result of this method may overflow
    /// if magnitude of any component is larger than ~30,000.
    /// </summary>
    [Pure]
    public int DotInt(Tile3i rhs) => this.X * rhs.X + this.Y * rhs.Y + this.Z * rhs.Z;

    /// <summary>
    /// Returns distance from this vector to the other vector.
    /// </summary>
    [Pure]
    public Fix32 DistanceTo(Tile3i other)
    {
      return new Tile3i(this.X - other.X, this.Y - other.Y, this.Z - other.Z).Length;
    }

    /// <summary>
    /// Returns squared distance from this vector to the other vector.
    /// </summary>
    [Pure]
    public long DistanceSqrTo(Tile3i other)
    {
      return new Tile3i(this.X - other.X, this.Y - other.Y, this.Z - other.Z).LengthSqr;
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
    public Tile3i Cross(Tile3i rhs)
    {
      return new Tile3i(this.Y * rhs.Z - this.Z * rhs.Y, this.Z * rhs.X - this.X * rhs.Z, this.X * rhs.Y - this.Y * rhs.X);
    }

    /// <summary>
    /// Whether this and <see paramref="other" /> vectors are parallel and not anti-parallel.
    /// </summary>
    [Pure]
    public bool IsParallelTo(Tile3i other)
    {
      Assert.That<Tile3i>(this).IsNotZero("IsParallelTo was called on zero vector.");
      Assert.That<Tile3i>(other).IsNotZero("IsParallelTo was called with zero vector.");
      return this.Cross(other).IsZero && this.Dot(other) > 0L;
    }

    /// <summary>
    /// Whether this and <see paramref="other" /> vectors are anti-parallel and not parallel.
    /// </summary>
    [Pure]
    public bool IsAntiParallelTo(Tile3i other)
    {
      Assert.That<Tile3i>(this).IsNotZero("IsAntiParallelTo was called on zero vector.");
      Assert.That<Tile3i>(other).IsNotZero("IsAntiParallelTo was called with zero vector.");
      return this.Cross(other).IsZero && this.Dot(other) < 0L;
    }

    /// <summary>
    /// Whether this and <see paramref="other" /> vectors are parallel or anti-parallel. This is more efficient than
    /// calling <see paramref="IsParallelTo" /> and <see paramref="IsAntiParallelTo" />.
    /// </summary>
    [Pure]
    public bool IsParallelOrAntiParallelTo(Tile3i other)
    {
      Assert.That<Tile3i>(this).IsNotZero("IsParallelOrAntiParallelTo was called on zero vector.");
      Assert.That<Tile3i>(other).IsNotZero("IsParallelOrAntiParallelTo was called with zero vector.");
      return this.Cross(other).IsZero;
    }

    /// <summary>
    /// Returns absolute angle between this and <see paramref="other" /> vectors. Returned angle is in range [0, τ/2].
    /// </summary>
    [Pure]
    public AngleDegrees1f AngleBetween(Tile3i other)
    {
      return MafiMath.Atan2(this.Cross(other).Length.ToFix64(), (Fix64) this.Dot(other));
    }

    /// <summary>
    /// Signed angle-to is not possible in 3D without some kind of reference vector.
    /// </summary>
    public void AngleTo(Tile3i other)
    {
    }

    /// <summary>
    /// Returns this vector rotated around X-axis by given amount.
    /// WARNING: Please keep in mind that rotating integer vectors may not be precise for vectors with small
    /// magnitudes due to rounding errors.
    /// </summary>
    [Pure]
    public Tile3i RotatedAroundX(AngleDegrees1f angle)
    {
      Fix64 fix64_1 = angle.Cos();
      Fix64 fix64_2 = angle.Sin();
      int x = this.X;
      Fix64 fix64_3 = fix64_1 * this.Y - fix64_2 * this.Z;
      int intRounded1 = fix64_3.ToIntRounded();
      fix64_3 = fix64_2 * this.Y + fix64_1 * this.Z;
      int intRounded2 = fix64_3.ToIntRounded();
      return new Tile3i(x, intRounded1, intRounded2);
    }

    /// <summary>
    /// Returns this vector rotated around Y-axis by given amount.
    /// WARNING: Please keep in mind that rotating integer vectors may not be precise for vectors with small
    /// magnitudes due to rounding errors.
    /// </summary>
    [Pure]
    public Tile3i RotatedAroundY(AngleDegrees1f angle)
    {
      Fix64 fix64_1 = angle.Cos();
      Fix64 fix64_2 = angle.Sin();
      Fix64 fix64_3 = fix64_1 * this.X + fix64_2 * this.Z;
      int intRounded1 = fix64_3.ToIntRounded();
      int y = this.Y;
      fix64_3 = fix64_1 * this.Z - fix64_2 * this.X;
      int intRounded2 = fix64_3.ToIntRounded();
      return new Tile3i(intRounded1, y, intRounded2);
    }

    /// <summary>
    /// Returns this vector rotated around Z-axis by given amount.
    /// WARNING: Please keep in mind that rotating integer vectors may not be precise for vectors with small
    /// magnitudes due to rounding errors.
    /// </summary>
    [Pure]
    public Tile3i RotatedAroundZ(AngleDegrees1f angle)
    {
      Fix64 fix64_1 = angle.Cos();
      Fix64 fix64_2 = angle.Sin();
      Fix64 fix64_3 = fix64_1 * this.X - fix64_2 * this.Y;
      int intRounded1 = fix64_3.ToIntRounded();
      fix64_3 = fix64_2 * this.X + fix64_1 * this.Y;
      int intRounded2 = fix64_3.ToIntRounded();
      int z = this.Z;
      return new Tile3i(intRounded1, intRounded2, z);
    }

    /// <summary>
    /// Helper function that rotates a 2d vector, it gets as first two parameters by given angle.
    /// (1, 0) when rotated by 90 degrees gives (0, 1)
    /// </summary>
    private void rotate2dVector(ref int x, ref int y, Rotation90 angle)
    {
      switch (angle.AngleIndex)
      {
        case 1:
          int num1 = x;
          x = -y;
          y = num1;
          break;
        case 2:
          x = -x;
          y = -y;
          break;
        case 3:
          int num2 = x;
          x = y;
          y = -num2;
          break;
      }
    }

    /// <summary>
    /// Returns this vector rotated around X-axis by given amount.
    /// </summary>
    [Pure]
    public Tile3i RotatedAroundX(Rotation90 angle)
    {
      int y = this.Y;
      int z = this.Z;
      this.rotate2dVector(ref y, ref z, angle);
      return new Tile3i(this.X, y, z);
    }

    /// <summary>
    /// Returns this vector rotated around Y-axis by given amount.
    /// </summary>
    [Pure]
    public Tile3i RotatedAroundY(Rotation90 angle)
    {
      int z = this.Z;
      int x = this.X;
      this.rotate2dVector(ref z, ref x, angle);
      return new Tile3i(x, this.Y, z);
    }

    /// <summary>
    /// Returns this vector rotated around Z-axis by given amount.
    /// </summary>
    [Pure]
    public Tile3i RotatedAroundZ(Rotation90 angle)
    {
      int x = this.X;
      int y = this.Y;
      this.rotate2dVector(ref x, ref y, angle);
      return new Tile3i(x, y, this.Z);
    }

    /// <summary>Returns component-wise min of this and given vectors.</summary>
    [Pure]
    public Tile3i Min(Tile3i rhs)
    {
      return new Tile3i(this.X.Min(rhs.X), this.Y.Min(rhs.Y), this.Z.Min(rhs.Z));
    }

    /// <summary>Returns component-wise min of this and given vectors.</summary>
    [Pure]
    public Tile3i Max(Tile3i rhs)
    {
      return new Tile3i(this.X.Max(rhs.X), this.Y.Max(rhs.Y), this.Z.Max(rhs.Z));
    }

    /// <summary>Returns component-wise min of this and given vectors.</summary>
    [Pure]
    public int MinComponent() => this.X.Min(this.Y).Min(this.Z);

    /// <summary>Returns component-wise max of this and given vectors.</summary>
    [Pure]
    public int MaxComponent() => this.X.Max(this.Y).Max(this.Z);

    /// <summary>Returns component-wise clamp of this vectors.</summary>
    [Pure]
    public Tile3i Clamp(int min, int max)
    {
      return new Tile3i(this.X.Clamp(min, max), this.Y.Clamp(min, max), this.Z.Clamp(min, max));
    }

    /// <summary>
    /// Returns component-wise average of this and given vectors.
    /// </summary>
    [Pure]
    public Tile3i Average(Tile3i rhs)
    {
      return new Tile3i(this.X + rhs.X >> 1, this.Y + rhs.Y >> 1, this.Z + rhs.Z >> 1);
    }

    [Pure]
    public Tile3i Lerp(Tile3i to, Percent t)
    {
      return new Tile3i(this.X.Lerp(to.X, t), this.Y.Lerp(to.Y, t), this.Z.Lerp(to.Z, t));
    }

    /// <summary>
    /// Linearly interpolates between this and <paramref name="to" /> vectors based on <paramref name="t" />.
    /// Interpolation parameter <paramref name="t" /> goes from 0 to <paramref name="scale" />.
    /// See <see cref="M:Mafi.MafiMath.Lerp(System.Int32,System.Int32,System.Int64,System.Int64)" /> for details.
    /// </summary>
    [Pure]
    public Tile3i Lerp(Tile3i to, long t, long scale)
    {
      return new Tile3i(this.X.Lerp(to.X, t, scale), this.Y.Lerp(to.Y, t, scale), this.Z.Lerp(to.Z, t, scale));
    }

    /// <summary>
    /// Linearly interpolates between <paramref name="from" /> and <paramref name="to" /> vectors based on <paramref name="t" />. Interpolation parameter <paramref name="t" /> goes from 0 to <paramref name="scale" />. See <see cref="M:Mafi.MafiMath.Lerp(System.Int32,System.Int32,System.Int64,System.Int64)" /> for details.
    /// </summary>
    public static Tile3i Lerp(Tile3i from, Tile3i to, long t, long scale)
    {
      return from.Lerp(to, t, scale);
    }

    [Pure]
    public bool Equals(Tile3i other) => other == this;

    [Pure]
    public override bool Equals(object other) => other is Tile3i tile3i && tile3i == this;

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
    public int CompareTo(Tile3i other)
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
    public static bool operator ==(Tile3i lhs, Tile3i rhs)
    {
      return lhs.X == rhs.X && lhs.Y == rhs.Y && lhs.Z == rhs.Z;
    }

    /// <summary>Exact inequality of two vectors.</summary>
    public static bool operator !=(Tile3i lhs, Tile3i rhs)
    {
      return lhs.X != rhs.X || lhs.Y != rhs.Y || lhs.Z != rhs.Z;
    }

    /// <summary>
    /// Component-wise less-than operator. Returns true if all components of the left-hand side vector are less than
    /// respective components of the right-hand side vector.
    /// WARNING: <c>A &lt; B</c> is not equal to <c>A &gt;= B</c>. For example vectors (1, 2, 3) and (3, 2, 1) are not
    /// less-than nor greater-than-or-equal.
    /// </summary>
    public static bool operator <(Tile3i lhs, Tile3i rhs)
    {
      return lhs.X < rhs.X && lhs.Y < rhs.Y && lhs.Z < rhs.Z;
    }

    /// <summary>
    /// Component-wise less-than-or-equal operator. Returns true if all components of the left-hand side vector are
    /// less than or equal to respective components of the right-hand side vector.
    /// WARNING: <c>A &lt;= B</c> is not equal to <c>A &gt; B</c>. For example vectors (1, 2, 3) and (3, 2, 1) are not
    /// less-than-or-equal nor greater-than.
    /// </summary>
    public static bool operator <=(Tile3i lhs, Tile3i rhs)
    {
      return lhs.X <= rhs.X && lhs.Y <= rhs.Y && lhs.Z <= rhs.Z;
    }

    /// <summary>
    /// Component-wise greater-than operator. Returns true if all components of the left-hand side vector are
    /// greater than respective components of the right-hand side vector.
    /// WARNING: <c>A &gt; B</c> is not equal to <c>A &lt;= B</c>. For example vectors (1, 2, 3) and (3, 2, 1) are not
    /// greater-than nor less-than-or-equal.
    /// </summary>
    public static bool operator >(Tile3i lhs, Tile3i rhs)
    {
      return lhs.X > rhs.X && lhs.Y > rhs.Y && lhs.Z > rhs.Z;
    }

    /// <summary>
    /// Component-wise greater-than-or-equal operator. Returns true if all components of the left-hand side vector
    /// are greater than or equal to respective components of the right-hand side vector.
    /// WARNING: <c>A &gt;= B</c> is not equal to <c>A &lt; B</c>. For example vectors (1, 2, 3) and (3, 2, 1) are not
    /// greater-than-or-equal nor less-than.
    /// </summary>
    public static bool operator >=(Tile3i lhs, Tile3i rhs)
    {
      return lhs.X >= rhs.X && lhs.Y >= rhs.Y && lhs.Z >= rhs.Z;
    }

    public static Tile3i operator +(int lhs, Tile3i rhs)
    {
      return new Tile3i(lhs + rhs.X, lhs + rhs.Y, lhs + rhs.Z);
    }

    public static Tile3i operator +(Tile3i lhs, int rhs)
    {
      return new Tile3i(lhs.X + rhs, lhs.Y + rhs, lhs.Z + rhs);
    }

    public static Tile3i operator +(Tile3i lhs, RelTile3i rhs)
    {
      return new Tile3i(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z);
    }

    public static Tile3i operator +(RelTile3i lhs, Tile3i rhs)
    {
      return new Tile3i(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z);
    }

    public static Tile3i operator -(Tile3i vector) => new Tile3i(-vector.X, -vector.Y, -vector.Z);

    public static Tile3i operator -(int lhs, Tile3i rhs)
    {
      return new Tile3i(lhs - rhs.X, lhs - rhs.Y, lhs - rhs.Z);
    }

    public static Tile3i operator -(Tile3i lhs, int rhs)
    {
      return new Tile3i(lhs.X - rhs, lhs.Y - rhs, lhs.Z - rhs);
    }

    public static RelTile3i operator -(Tile3i lhs, Tile3i rhs)
    {
      return new RelTile3i(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z);
    }

    public static Tile3i operator -(Tile3i lhs, RelTile3i rhs)
    {
      return new Tile3i(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z);
    }

    public static Tile3i operator *(Tile3i lhs, int rhs)
    {
      return new Tile3i(lhs.X * rhs, lhs.Y * rhs, lhs.Z * rhs);
    }

    public static Tile3i operator *(int lhs, Tile3i rhs)
    {
      return new Tile3i(rhs.X * lhs, rhs.Y * lhs, rhs.Z * lhs);
    }

    public static Tile3i operator *(Tile3i lhs, Percent rhs)
    {
      return new Tile3i(rhs.Apply(lhs.X), rhs.Apply(lhs.Y), rhs.Apply(lhs.Z));
    }

    public static Tile3i operator *(Percent lhs, Tile3i rhs)
    {
      return new Tile3i(lhs.Apply(rhs.X), lhs.Apply(rhs.Y), lhs.Apply(rhs.Z));
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile3i Times2Fast => new Tile3i(this.X << 1, this.Y << 1, this.Z << 1);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile3i Times4Fast => new Tile3i(this.X << 2, this.Y << 2, this.Z << 2);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile3i Times8Fast => new Tile3i(this.X << 3, this.Y << 3, this.Z << 3);

    public static Tile3i operator /(int lhs, Tile3i rhs)
    {
      return new Tile3i(lhs / rhs.X, lhs / rhs.Y, lhs / rhs.Z);
    }

    public static Tile3i operator /(Tile3i lhs, int rhs)
    {
      return new Tile3i(lhs.X / rhs, lhs.Y / rhs, lhs.Z / rhs);
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile3i HalfFast => new Tile3i(this.X >> 1, this.Y >> 1, this.Z >> 1);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile3i DivBy4Fast => new Tile3i(this.X >> 2, this.Y >> 2, this.Z >> 2);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile3i DivBy8Fast => new Tile3i(this.X >> 3, this.Y >> 3, this.Z >> 3);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile3i DivBy16Fast => new Tile3i(this.X >> 4, this.Y >> 4, this.Z >> 4);

    /// <summary>
    /// Computes floor division. Unlike normal division operator in C# this always rounds down.
    /// </summary>
    public Tile3i FloorDiv(int rhs)
    {
      return new Tile3i(this.X.FloorDiv(rhs), this.Y.FloorDiv(rhs), this.Z.FloorDiv(rhs));
    }

    public Tile3i CeilDiv(int rhs)
    {
      return new Tile3i(this.X.CeilDiv(rhs), this.Y.CeilDiv(rhs), this.Z.CeilDiv(rhs));
    }

    public static Tile3i operator %(int lhs, Tile3i rhs)
    {
      return new Tile3i(lhs % rhs.X, lhs % rhs.Y, lhs % rhs.Z);
    }

    public static Tile3i operator %(Tile3i lhs, int rhs)
    {
      return new Tile3i(lhs.X % rhs, lhs.Y % rhs, lhs.Z % rhs);
    }

    public static void Serialize(Tile3i value, BlobWriter writer)
    {
      writer.WriteInt(value.X);
      writer.WriteInt(value.Y);
      writer.WriteInt(value.Z);
    }

    public static Tile3i Deserialize(BlobReader reader)
    {
      return new Tile3i(reader.ReadInt(), reader.ReadInt(), reader.ReadInt());
    }

    public Tile3i(Vector3i tileCoord)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.X = tileCoord.X;
      this.Y = tileCoord.Y;
      this.Z = tileCoord.Z;
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile3iSlim AsSlim => new Tile3iSlim((ushort) this.X, (ushort) this.Y, (short) this.Z);

    /// <summary>Discrete height.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public HeightTilesI Height => new HeightTilesI(this.Z);

    /// <summary>
    /// Gets 2D tile index as X and Y coordinates of this coord.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile2i Tile2i => new Tile2i(this.X, this.Y);

    /// <summary>
    /// Converts this global tile coordinate to integer coordinate of patent chunk.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Chunk2i ParentChunkCoord => new Chunk2i(this.X.FloorDiv(64), this.Y.FloorDiv(64));

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector3f Vector3f => new Vector3f((Fix32) this.X, (Fix32) this.Y, (Fix32) this.Z);

    /// <summary>
    /// Converts tile coordinate to corner-based Fix32 tile coordinate.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile3f CornerTile3f => new Tile3f((Fix32) this.X, (Fix32) this.Y, (Fix32) this.Z);

    /// <summary>
    /// Converts tile coordinate to center-based Fix32 tile coordinate.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile3f CenterTile3f
    {
      get
      {
        return new Tile3f((Fix32) this.X + Fix32.Half, (Fix32) this.Y + Fix32.Half, (Fix32) this.Z + Fix32.Half);
      }
    }

    /// <summary>
    /// Converts tile coordinate to XY-plane center-based, z corner based Fix32 tile coordinate.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile3f CenterXyFloorZTile3f
    {
      get => new Tile3f((Fix32) this.X + Fix32.Half, (Fix32) this.Y + Fix32.Half, (Fix32) this.Z);
    }

    /// <summary>
    /// Converts this global tile coordinate to integer relative coordinate of this tile within its patent chunk.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public TileInChunk2i TileInChunkCoord
    {
      get => new TileInChunk2i(this.X.ModuloPowerOfTwo(64), this.Y.ModuloPowerOfTwo(64));
    }

    [Pure]
    public Tile3i AddXy(RelTile2i addedValue)
    {
      return new Tile3i(this.X + addedValue.X, this.Y + addedValue.Y, this.Z);
    }

    [Pure]
    public Tile3i AddXy(int addedValue)
    {
      return new Tile3i(this.X + addedValue, this.Y + addedValue, this.Z);
    }

    [Pure]
    public int ManhattanDistanceTo(Tile3i other)
    {
      RelTile3i relTile3i = this - other;
      relTile3i = relTile3i.AbsValue;
      return relTile3i.Sum;
    }

    public static Tile3i operator +(Tile3i lhs, ThicknessTilesI rhs)
    {
      return new Tile3i(lhs.X, lhs.Y, lhs.Z + rhs.Value);
    }

    public static Tile3i operator +(ThicknessTilesI lhs, Tile3i rhs)
    {
      return new Tile3i(rhs.X, rhs.Y, lhs.Value + rhs.Z);
    }

    public static Tile3i operator -(Tile3i lhs, ThicknessTilesI rhs)
    {
      return new Tile3i(lhs.X, lhs.Y, lhs.Z - rhs.Value);
    }

    public static Tile3i operator -(ThicknessTilesI lhs, Tile3i rhs)
    {
      return new Tile3i(rhs.X, rhs.Y, lhs.Value - rhs.Z);
    }

    static Tile3i()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Tile3i.Zero = new Tile3i();
      Tile3i.One = new Tile3i(1, 1, 1);
      Tile3i.UnitX = new Tile3i(1, 0, 0);
      Tile3i.UnitY = new Tile3i(0, 1, 0);
      Tile3i.UnitZ = new Tile3i(0, 0, 1);
      Tile3i.MinValue = new Tile3i(int.MinValue, int.MinValue, int.MinValue);
      Tile3i.MaxValue = new Tile3i(int.MaxValue, int.MaxValue, int.MaxValue);
    }
  }
}
