// Decompiled with JetBrains decompiler
// Type: Mafi.Vector3i
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
  /// <summary>Immutable 3D int vector.</summary>
  /// <remarks>
  /// This is partial struct and this file should contain only specific members for <see cref="T:Mafi.Vector3i" />. All general
  /// members should be added to the generator T4 template.
  /// </remarks>
  [ManuallyWrittenSerialization]
  [DebuggerDisplay("({X}, {Y}, {Z})")]
  [DebuggerStepThrough]
  public struct Vector3i : IEquatable<Vector3i>, IComparable<Vector3i>
  {
    /// <summary>Vector (0, 0, 0).</summary>
    public static readonly Vector3i Zero;
    /// <summary>Vector (1, 1, 1).</summary>
    public static readonly Vector3i One;
    /// <summary>Vector (1, 0, 0).</summary>
    public static readonly Vector3i UnitX;
    /// <summary>Vector (0, 1, 0).</summary>
    public static readonly Vector3i UnitY;
    /// <summary>Vector (0, 0, 1).</summary>
    public static readonly Vector3i UnitZ;
    /// <summary>Vector (int.MinValue, int.MinValue, int.MinValue).</summary>
    public static readonly Vector3i MinValue;
    /// <summary>Vector (int.MaxValue, int.MaxValue, int.MaxValue).</summary>
    public static readonly Vector3i MaxValue;
    /// <summary>The X component of this vector.</summary>
    public readonly int X;
    /// <summary>The Y component of this vector.</summary>
    public readonly int Y;
    /// <summary>The Z component of this vector.</summary>
    public readonly int Z;

    /// <summary>Creates new Vector3i from raw components.</summary>
    public Vector3i(int x, int y, int z)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.X = x;
      this.Y = y;
      this.Z = z;
    }

    /// <summary>
    /// Creates new Vector3i from Vector2i and raw components.
    /// </summary>
    public Vector3i(Vector2i vector, int z)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.X = vector.X;
      this.Y = vector.Y;
      this.Z = z;
    }

    /// <summary>Gets the first two components of this vector.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector2i Xy => new Vector2i(this.X, this.Y);

    /// <summary>Sum of all components of this vector.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int Sum => this.X + this.Y + this.Z;

    /// <summary>
    /// Euclidean length of this vector.
    /// PERF: Expensive, uses sqrt. Consider using <see cref="P:Mafi.Vector3i.LengthSqr" /> whenever possible (when comparing
    /// lengths, etc.).
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Fix32 Length => Fix32.FromDouble(Math.Sqrt((double) this.LengthSqr));

    /// <summary>
    /// Approximate euclidean length of this vector as integer.
    /// PERF: Expensive, uses sqrt, consider using <see cref="P:Mafi.Vector3i.LengthSqr" /> whenever possible.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int LengthInt => (int) Math.Round(Math.Sqrt((double) this.LengthSqr));

    /// <summary>
    /// Euclidean length squared of this vector.
    /// PERF: Cheaper than <see cref="P:Mafi.Vector3i.Length" />, does not require expensive sqrt.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int LengthSqrInt => this.X * this.X + this.Y * this.Y + this.Z * this.Z;

    /// <summary>
    /// Euclidean length squared of this vector.
    /// PERF: Cheaper than <see cref="P:Mafi.Vector3i.Length" />, does not require expensive sqrt.
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
    public Vector3i SetX(int newX) => new Vector3i(newX, this.Y, this.Z);

    /// <summary>Returns new vector with changed Y component.</summary>
    [Pure]
    public Vector3i SetY(int newY) => new Vector3i(this.X, newY, this.Z);

    /// <summary>Returns new vector with changed Z component.</summary>
    [Pure]
    public Vector3i SetZ(int newZ) => new Vector3i(this.X, this.Y, newZ);

    /// <summary>Returns new vector with changed X and Y components.</summary>
    [Pure]
    public Vector3i SetXy(int newX, int newY) => new Vector3i(newX, newY, this.Z);

    /// <summary>Returns new vector with changed X and Y components.</summary>
    [Pure]
    public Vector3i SetXy(Vector2i newXy) => new Vector3i(newXy.X, newXy.Y, this.Z);

    /// <summary>
    /// Returns new vector with changed X, Y, and Z components.
    /// </summary>
    [Pure]
    public Vector3i SetXyz(int newX, int newY, int newZ) => new Vector3i(newX, newY, newZ);

    /// <summary>Extends this vector a new component.</summary>
    [Pure]
    public Vector4i ExtendW(int w) => new Vector4i(this.X, this.Y, this.Z, w);

    /// <summary>Returns new vector with incremented X component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector3i IncrementX => new Vector3i(this.X + 1, this.Y, this.Z);

    /// <summary>Returns new vector with incremented Y component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector3i IncrementY => new Vector3i(this.X, this.Y + 1, this.Z);

    /// <summary>Returns new vector with incremented Z component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector3i IncrementZ => new Vector3i(this.X, this.Y, this.Z + 1);

    /// <summary>Returns new vector with decremented X component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector3i DecrementX => new Vector3i(this.X - 1, this.Y, this.Z);

    /// <summary>Returns new vector with decremented Y component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector3i DecrementY => new Vector3i(this.X, this.Y - 1, this.Z);

    /// <summary>Returns new vector with decremented Z component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector3i DecrementZ => new Vector3i(this.X, this.Y, this.Z - 1);

    /// <summary>
    /// Returns new vector with given value added to the X component.
    /// </summary>
    [Pure]
    public Vector3i AddX(int addedX) => new Vector3i(this.X + addedX, this.Y, this.Z);

    /// <summary>
    /// Returns new vector with given value added to the Y component.
    /// </summary>
    [Pure]
    public Vector3i AddY(int addedY) => new Vector3i(this.X, this.Y + addedY, this.Z);

    /// <summary>
    /// Returns new vector with given value added to the Z component.
    /// </summary>
    [Pure]
    public Vector3i AddZ(int addedZ) => new Vector3i(this.X, this.Y, this.Z + addedZ);

    /// <summary>
    /// Returns new vector with given value added to all components.
    /// </summary>
    [Pure]
    public Vector3i AddXyz(int addedValue)
    {
      return new Vector3i(this.X + addedValue, this.Y + addedValue, this.Z + addedValue);
    }

    /// <summary>
    /// Returns new vector with given value multiplied with the X component.
    /// </summary>
    [Pure]
    public Vector3i MultiplyX(int multX) => new Vector3i(this.X * multX, this.Y, this.Z);

    /// <summary>
    /// Returns new vector with given value multiplied with the Y component.
    /// </summary>
    [Pure]
    public Vector3i MultiplyY(int multY) => new Vector3i(this.X, this.Y * multY, this.Z);

    /// <summary>
    /// Returns new vector with given value multiplied with the Z component.
    /// </summary>
    [Pure]
    public Vector3i MultiplyZ(int multZ) => new Vector3i(this.X, this.Y, this.Z * multZ);

    /// <summary>
    /// Returns new vector with reflected X component (opposite sign).
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector3i ReflectX => new Vector3i(-this.X, this.Y, this.Z);

    /// <summary>
    /// Returns new vector with reflected Y component (opposite sign).
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector3i ReflectY => new Vector3i(this.X, -this.Y, this.Z);

    /// <summary>
    /// Returns new vector with reflected Z component (opposite sign).
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector3i ReflectZ => new Vector3i(this.X, this.Y, -this.Z);

    /// <summary>Gets Vector3f representation of this vector.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector3f Vector3f
    {
      get => new Vector3f(Fix32.FromInt(this.X), Fix32.FromInt(this.Y), Fix32.FromInt(this.Z));
    }

    /// <summary>
    /// Multiples and divides all components. This method is using long precision to prevent int32 overflows.
    /// </summary>
    public Vector3i MulDiv(long mul, long div)
    {
      return new Vector3i((int) (mul * (long) this.X / div), (int) (mul * (long) this.Y / div), (int) (mul * (long) this.Z / div));
    }

    /// <summary>
    /// Returns scaled vector to requested length. This method is more precise, faster and more intuitive than
    /// normalization followed by multiplication.
    /// WARNING: Setting length of integer vector may not produce exact requested length do to rounding error.
    /// </summary>
    [Pure]
    public Vector3i OfLength(int desiredLength)
    {
      double num1 = Math.Sqrt((double) this.LengthSqr);
      double num2 = (double) desiredLength / num1;
      return new Vector3i(((double) this.X * num2).RoundToInt(), ((double) this.Y * num2).RoundToInt(), ((double) this.Z * num2).RoundToInt());
    }

    /// <summary>
    /// Whether corresponding components of this and given vectors are within given tolerance.
    /// </summary>
    [Pure]
    public bool IsNear(Vector3i other, int tolerance)
    {
      return this.X.IsNear(other.X, tolerance) && this.Y.IsNear(other.Y, tolerance) && this.Z.IsNear(other.Z, tolerance);
    }

    /// <summary>Returns dot product of this vector with given vector.</summary>
    [Pure]
    public long Dot(Vector3i rhs)
    {
      return (long) this.X * (long) rhs.X + (long) this.Y * (long) rhs.Y + (long) this.Z * (long) rhs.Z;
    }

    /// <summary>
    /// Returns dot product of this vector with given vector as int32. Note that result of this method may overflow
    /// if magnitude of any component is larger than ~30,000.
    /// </summary>
    [Pure]
    public int DotInt(Vector3i rhs) => this.X * rhs.X + this.Y * rhs.Y + this.Z * rhs.Z;

    /// <summary>
    /// Returns distance from this vector to the other vector.
    /// </summary>
    [Pure]
    public Fix32 DistanceTo(Vector3i other)
    {
      return new Vector3i(this.X - other.X, this.Y - other.Y, this.Z - other.Z).Length;
    }

    /// <summary>
    /// Returns squared distance from this vector to the other vector.
    /// </summary>
    [Pure]
    public long DistanceSqrTo(Vector3i other)
    {
      return new Vector3i(this.X - other.X, this.Y - other.Y, this.Z - other.Z).LengthSqr;
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
    public Vector3i Cross(Vector3i rhs)
    {
      return new Vector3i(this.Y * rhs.Z - this.Z * rhs.Y, this.Z * rhs.X - this.X * rhs.Z, this.X * rhs.Y - this.Y * rhs.X);
    }

    /// <summary>
    /// Whether this and <see paramref="other" /> vectors are parallel and not anti-parallel.
    /// </summary>
    [Pure]
    public bool IsParallelTo(Vector3i other)
    {
      Assert.That<Vector3i>(this).IsNotZero("IsParallelTo was called on zero vector.");
      Assert.That<Vector3i>(other).IsNotZero("IsParallelTo was called with zero vector.");
      return this.Cross(other).IsZero && this.Dot(other) > 0L;
    }

    /// <summary>
    /// Whether this and <see paramref="other" /> vectors are anti-parallel and not parallel.
    /// </summary>
    [Pure]
    public bool IsAntiParallelTo(Vector3i other)
    {
      Assert.That<Vector3i>(this).IsNotZero("IsAntiParallelTo was called on zero vector.");
      Assert.That<Vector3i>(other).IsNotZero("IsAntiParallelTo was called with zero vector.");
      return this.Cross(other).IsZero && this.Dot(other) < 0L;
    }

    /// <summary>
    /// Whether this and <see paramref="other" /> vectors are parallel or anti-parallel. This is more efficient than
    /// calling <see paramref="IsParallelTo" /> and <see paramref="IsAntiParallelTo" />.
    /// </summary>
    [Pure]
    public bool IsParallelOrAntiParallelTo(Vector3i other)
    {
      Assert.That<Vector3i>(this).IsNotZero("IsParallelOrAntiParallelTo was called on zero vector.");
      Assert.That<Vector3i>(other).IsNotZero("IsParallelOrAntiParallelTo was called with zero vector.");
      return this.Cross(other).IsZero;
    }

    /// <summary>
    /// Returns absolute angle between this and <see paramref="other" /> vectors. Returned angle is in range [0, τ/2].
    /// </summary>
    [Pure]
    public AngleDegrees1f AngleBetween(Vector3i other)
    {
      return MafiMath.Atan2(this.Cross(other).Length.ToFix64(), (Fix64) this.Dot(other));
    }

    /// <summary>
    /// Signed angle-to is not possible in 3D without some kind of reference vector.
    /// </summary>
    public void AngleTo(Vector3i other)
    {
    }

    /// <summary>
    /// Returns this vector rotated around X-axis by given amount.
    /// WARNING: Please keep in mind that rotating integer vectors may not be precise for vectors with small
    /// magnitudes due to rounding errors.
    /// </summary>
    [Pure]
    public Vector3i RotatedAroundX(AngleDegrees1f angle)
    {
      Fix64 fix64_1 = angle.Cos();
      Fix64 fix64_2 = angle.Sin();
      int x = this.X;
      Fix64 fix64_3 = fix64_1 * this.Y - fix64_2 * this.Z;
      int intRounded1 = fix64_3.ToIntRounded();
      fix64_3 = fix64_2 * this.Y + fix64_1 * this.Z;
      int intRounded2 = fix64_3.ToIntRounded();
      return new Vector3i(x, intRounded1, intRounded2);
    }

    /// <summary>
    /// Returns this vector rotated around Y-axis by given amount.
    /// WARNING: Please keep in mind that rotating integer vectors may not be precise for vectors with small
    /// magnitudes due to rounding errors.
    /// </summary>
    [Pure]
    public Vector3i RotatedAroundY(AngleDegrees1f angle)
    {
      Fix64 fix64_1 = angle.Cos();
      Fix64 fix64_2 = angle.Sin();
      Fix64 fix64_3 = fix64_1 * this.X + fix64_2 * this.Z;
      int intRounded1 = fix64_3.ToIntRounded();
      int y = this.Y;
      fix64_3 = fix64_1 * this.Z - fix64_2 * this.X;
      int intRounded2 = fix64_3.ToIntRounded();
      return new Vector3i(intRounded1, y, intRounded2);
    }

    /// <summary>
    /// Returns this vector rotated around Z-axis by given amount.
    /// WARNING: Please keep in mind that rotating integer vectors may not be precise for vectors with small
    /// magnitudes due to rounding errors.
    /// </summary>
    [Pure]
    public Vector3i RotatedAroundZ(AngleDegrees1f angle)
    {
      Fix64 fix64_1 = angle.Cos();
      Fix64 fix64_2 = angle.Sin();
      Fix64 fix64_3 = fix64_1 * this.X - fix64_2 * this.Y;
      int intRounded1 = fix64_3.ToIntRounded();
      fix64_3 = fix64_2 * this.X + fix64_1 * this.Y;
      int intRounded2 = fix64_3.ToIntRounded();
      int z = this.Z;
      return new Vector3i(intRounded1, intRounded2, z);
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
    public Vector3i RotatedAroundX(Rotation90 angle)
    {
      int y = this.Y;
      int z = this.Z;
      this.rotate2dVector(ref y, ref z, angle);
      return new Vector3i(this.X, y, z);
    }

    /// <summary>
    /// Returns this vector rotated around Y-axis by given amount.
    /// </summary>
    [Pure]
    public Vector3i RotatedAroundY(Rotation90 angle)
    {
      int z = this.Z;
      int x = this.X;
      this.rotate2dVector(ref z, ref x, angle);
      return new Vector3i(x, this.Y, z);
    }

    /// <summary>
    /// Returns this vector rotated around Z-axis by given amount.
    /// </summary>
    [Pure]
    public Vector3i RotatedAroundZ(Rotation90 angle)
    {
      int x = this.X;
      int y = this.Y;
      this.rotate2dVector(ref x, ref y, angle);
      return new Vector3i(x, y, this.Z);
    }

    /// <summary>Returns component-wise min of this and given vectors.</summary>
    [Pure]
    public Vector3i Min(Vector3i rhs)
    {
      return new Vector3i(this.X.Min(rhs.X), this.Y.Min(rhs.Y), this.Z.Min(rhs.Z));
    }

    /// <summary>Returns component-wise min of this and given vectors.</summary>
    [Pure]
    public Vector3i Max(Vector3i rhs)
    {
      return new Vector3i(this.X.Max(rhs.X), this.Y.Max(rhs.Y), this.Z.Max(rhs.Z));
    }

    /// <summary>Returns component-wise min of this and given vectors.</summary>
    [Pure]
    public int MinComponent() => this.X.Min(this.Y).Min(this.Z);

    /// <summary>Returns component-wise max of this and given vectors.</summary>
    [Pure]
    public int MaxComponent() => this.X.Max(this.Y).Max(this.Z);

    /// <summary>Returns component-wise clamp of this vectors.</summary>
    [Pure]
    public Vector3i Clamp(int min, int max)
    {
      return new Vector3i(this.X.Clamp(min, max), this.Y.Clamp(min, max), this.Z.Clamp(min, max));
    }

    /// <summary>Returns component-wise absolute value of this vector.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector3i AbsValue => new Vector3i(this.X.Abs(), this.Y.Abs(), this.Z.Abs());

    /// <summary>Returns component-wise sign of this vector.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector3i Signs => new Vector3i(this.X.Sign(), this.Y.Sign(), this.Z.Sign());

    /// <summary>
    /// Returns component-wise modulo operation on this vector (see <see cref="M:Mafi.MafiMath.Modulo(System.Int32,System.Int32)" />).
    /// </summary>
    [Pure]
    public Vector3i Modulo(int mod)
    {
      return new Vector3i(this.X.Modulo(mod), this.Y.Modulo(mod), this.Z.Modulo(mod));
    }

    /// <summary>
    /// Returns component-wise modulo operation on this vector (see <see cref="M:Mafi.MafiMath.Modulo(System.Int32,System.Int32)" />).
    /// </summary>
    [Pure]
    public Vector3i Modulo(Vector3i mod)
    {
      return new Vector3i(this.X.Modulo(mod.X), this.Y.Modulo(mod.Y), this.Z.Modulo(mod.Z));
    }

    /// <summary>
    /// Returns component-wise average of this and given vectors.
    /// </summary>
    [Pure]
    public Vector3i Average(Vector3i rhs)
    {
      return new Vector3i(this.X + rhs.X >> 1, this.Y + rhs.Y >> 1, this.Z + rhs.Z >> 1);
    }

    [Pure]
    public Vector3i Lerp(Vector3i to, Percent t)
    {
      return new Vector3i(this.X.Lerp(to.X, t), this.Y.Lerp(to.Y, t), this.Z.Lerp(to.Z, t));
    }

    /// <summary>
    /// Linearly interpolates between this and <paramref name="to" /> vectors based on <paramref name="t" />.
    /// Interpolation parameter <paramref name="t" /> goes from 0 to <paramref name="scale" />.
    /// See <see cref="M:Mafi.MafiMath.Lerp(System.Int32,System.Int32,System.Int64,System.Int64)" /> for details.
    /// </summary>
    [Pure]
    public Vector3i Lerp(Vector3i to, long t, long scale)
    {
      return new Vector3i(this.X.Lerp(to.X, t, scale), this.Y.Lerp(to.Y, t, scale), this.Z.Lerp(to.Z, t, scale));
    }

    /// <summary>
    /// Linearly interpolates between <paramref name="from" /> and <paramref name="to" /> vectors based on <paramref name="t" />. Interpolation parameter <paramref name="t" /> goes from 0 to <paramref name="scale" />. See <see cref="M:Mafi.MafiMath.Lerp(System.Int32,System.Int32,System.Int64,System.Int64)" /> for details.
    /// </summary>
    public static Vector3i Lerp(Vector3i from, Vector3i to, long t, long scale)
    {
      return from.Lerp(to, t, scale);
    }

    [Pure]
    public bool Equals(Vector3i other) => other == this;

    [Pure]
    public override bool Equals(object other) => other is Vector3i vector3i && vector3i == this;

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
    public int CompareTo(Vector3i other)
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
    public static bool operator ==(Vector3i lhs, Vector3i rhs)
    {
      return lhs.X == rhs.X && lhs.Y == rhs.Y && lhs.Z == rhs.Z;
    }

    /// <summary>Exact inequality of two vectors.</summary>
    public static bool operator !=(Vector3i lhs, Vector3i rhs)
    {
      return lhs.X != rhs.X || lhs.Y != rhs.Y || lhs.Z != rhs.Z;
    }

    /// <summary>
    /// Component-wise less-than operator. Returns true if all components of the left-hand side vector are less than
    /// respective components of the right-hand side vector.
    /// WARNING: <c>A &lt; B</c> is not equal to <c>A &gt;= B</c>. For example vectors (1, 2, 3) and (3, 2, 1) are not
    /// less-than nor greater-than-or-equal.
    /// </summary>
    public static bool operator <(Vector3i lhs, Vector3i rhs)
    {
      return lhs.X < rhs.X && lhs.Y < rhs.Y && lhs.Z < rhs.Z;
    }

    /// <summary>
    /// Component-wise less-than-or-equal operator. Returns true if all components of the left-hand side vector are
    /// less than or equal to respective components of the right-hand side vector.
    /// WARNING: <c>A &lt;= B</c> is not equal to <c>A &gt; B</c>. For example vectors (1, 2, 3) and (3, 2, 1) are not
    /// less-than-or-equal nor greater-than.
    /// </summary>
    public static bool operator <=(Vector3i lhs, Vector3i rhs)
    {
      return lhs.X <= rhs.X && lhs.Y <= rhs.Y && lhs.Z <= rhs.Z;
    }

    /// <summary>
    /// Component-wise greater-than operator. Returns true if all components of the left-hand side vector are
    /// greater than respective components of the right-hand side vector.
    /// WARNING: <c>A &gt; B</c> is not equal to <c>A &lt;= B</c>. For example vectors (1, 2, 3) and (3, 2, 1) are not
    /// greater-than nor less-than-or-equal.
    /// </summary>
    public static bool operator >(Vector3i lhs, Vector3i rhs)
    {
      return lhs.X > rhs.X && lhs.Y > rhs.Y && lhs.Z > rhs.Z;
    }

    /// <summary>
    /// Component-wise greater-than-or-equal operator. Returns true if all components of the left-hand side vector
    /// are greater than or equal to respective components of the right-hand side vector.
    /// WARNING: <c>A &gt;= B</c> is not equal to <c>A &lt; B</c>. For example vectors (1, 2, 3) and (3, 2, 1) are not
    /// greater-than-or-equal nor less-than.
    /// </summary>
    public static bool operator >=(Vector3i lhs, Vector3i rhs)
    {
      return lhs.X >= rhs.X && lhs.Y >= rhs.Y && lhs.Z >= rhs.Z;
    }

    public static Vector3i operator +(int lhs, Vector3i rhs)
    {
      return new Vector3i(lhs + rhs.X, lhs + rhs.Y, lhs + rhs.Z);
    }

    public static Vector3i operator +(Vector3i lhs, int rhs)
    {
      return new Vector3i(lhs.X + rhs, lhs.Y + rhs, lhs.Z + rhs);
    }

    public static Vector3i operator +(Vector3i lhs, Vector3i rhs)
    {
      return new Vector3i(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z);
    }

    public static Vector3i operator +(Vector3i lhs, Vector2i rhs)
    {
      return new Vector3i(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z);
    }

    public static Vector3i operator -(Vector3i vector)
    {
      return new Vector3i(-vector.X, -vector.Y, -vector.Z);
    }

    public static Vector3i operator -(int lhs, Vector3i rhs)
    {
      return new Vector3i(lhs - rhs.X, lhs - rhs.Y, lhs - rhs.Z);
    }

    public static Vector3i operator -(Vector3i lhs, int rhs)
    {
      return new Vector3i(lhs.X - rhs, lhs.Y - rhs, lhs.Z - rhs);
    }

    public static Vector3i operator -(Vector3i lhs, Vector3i rhs)
    {
      return new Vector3i(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z);
    }

    public static Vector3i operator -(Vector3i lhs, Vector2i rhs)
    {
      return new Vector3i(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z);
    }

    public static Vector3i operator *(Vector3i lhs, int rhs)
    {
      return new Vector3i(lhs.X * rhs, lhs.Y * rhs, lhs.Z * rhs);
    }

    public static Vector3i operator *(int lhs, Vector3i rhs)
    {
      return new Vector3i(rhs.X * lhs, rhs.Y * lhs, rhs.Z * lhs);
    }

    public static Vector3i operator *(Vector3i lhs, Percent rhs)
    {
      return new Vector3i(rhs.Apply(lhs.X), rhs.Apply(lhs.Y), rhs.Apply(lhs.Z));
    }

    public static Vector3i operator *(Percent lhs, Vector3i rhs)
    {
      return new Vector3i(lhs.Apply(rhs.X), lhs.Apply(rhs.Y), lhs.Apply(rhs.Z));
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector3i Times2Fast => new Vector3i(this.X << 1, this.Y << 1, this.Z << 1);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector3i Times4Fast => new Vector3i(this.X << 2, this.Y << 2, this.Z << 2);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector3i Times8Fast => new Vector3i(this.X << 3, this.Y << 3, this.Z << 3);

    public static Vector3i operator /(int lhs, Vector3i rhs)
    {
      return new Vector3i(lhs / rhs.X, lhs / rhs.Y, lhs / rhs.Z);
    }

    public static Vector3i operator /(Vector3i lhs, int rhs)
    {
      return new Vector3i(lhs.X / rhs, lhs.Y / rhs, lhs.Z / rhs);
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector3i HalfFast => new Vector3i(this.X >> 1, this.Y >> 1, this.Z >> 1);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector3i DivBy4Fast => new Vector3i(this.X >> 2, this.Y >> 2, this.Z >> 2);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector3i DivBy8Fast => new Vector3i(this.X >> 3, this.Y >> 3, this.Z >> 3);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector3i DivBy16Fast => new Vector3i(this.X >> 4, this.Y >> 4, this.Z >> 4);

    /// <summary>
    /// Computes floor division. Unlike normal division operator in C# this always rounds down.
    /// </summary>
    public Vector3i FloorDiv(int rhs)
    {
      return new Vector3i(this.X.FloorDiv(rhs), this.Y.FloorDiv(rhs), this.Z.FloorDiv(rhs));
    }

    public Vector3i CeilDiv(int rhs)
    {
      return new Vector3i(this.X.CeilDiv(rhs), this.Y.CeilDiv(rhs), this.Z.CeilDiv(rhs));
    }

    /// <summary>Component-wise division of two vectors.</summary>
    public static Vector3i operator /(Vector3i lhs, Vector3i rhs)
    {
      return new Vector3i(lhs.X / rhs.X, lhs.Y / rhs.Y, lhs.Z / rhs.Z);
    }

    /// <summary>
    /// Computes floor division. Unlike normal division operator in C# this always rounds down.
    /// </summary>
    public Vector3i FloorDiv(Vector3i rhs)
    {
      return new Vector3i(this.X.FloorDiv(rhs.X), this.Y.FloorDiv(rhs.Y), this.Z.FloorDiv(rhs.Z));
    }

    public static Vector3i operator %(int lhs, Vector3i rhs)
    {
      return new Vector3i(lhs % rhs.X, lhs % rhs.Y, lhs % rhs.Z);
    }

    public static Vector3i operator %(Vector3i lhs, int rhs)
    {
      return new Vector3i(lhs.X % rhs, lhs.Y % rhs, lhs.Z % rhs);
    }

    /// <summary>Component-wise modulo of two vectors.</summary>
    public static Vector3i operator %(Vector3i lhs, Vector3i rhs)
    {
      return new Vector3i(lhs.X % rhs.X, lhs.Y % rhs.Y, lhs.Z % rhs.Z);
    }

    public static void Serialize(Vector3i value, BlobWriter writer)
    {
      writer.WriteInt(value.X);
      writer.WriteInt(value.Y);
      writer.WriteInt(value.Z);
    }

    public static Vector3i Deserialize(BlobReader reader)
    {
      return new Vector3i(reader.ReadInt(), reader.ReadInt(), reader.ReadInt());
    }

    /// <summary>
    /// Returns vector with sorted components ascending order using 2-3 ifs.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector3i SortedComponents
    {
      get
      {
        if (this.X < this.Y)
        {
          if (this.X >= this.Z)
            return new Vector3i(this.Z, this.X, this.Y);
          return this.Y >= this.Z ? new Vector3i(this.X, this.Z, this.Y) : new Vector3i(this.X, this.Y, this.Z);
        }
        if (this.Z < this.Y)
          return new Vector3i(this.Z, this.Y, this.X);
        return this.X >= this.Z ? new Vector3i(this.Y, this.Z, this.X) : new Vector3i(this.Y, this.X, this.Z);
      }
    }

    public static Vector3i operator &(Vector3i lhs, int rhs)
    {
      return new Vector3i(lhs.X & rhs, lhs.Y & rhs, lhs.Z & rhs);
    }

    [Pure]
    public Vector3f LerpToFix32(Vector3i to, Percent t)
    {
      return new Vector3f(this.X.LerpToFix32(to.X, t), this.Y.LerpToFix32(to.Y, t), this.Z.LerpToFix32(to.Z, t));
    }

    static Vector3i()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      Vector3i.Zero = new Vector3i();
      Vector3i.One = new Vector3i(1, 1, 1);
      Vector3i.UnitX = new Vector3i(1, 0, 0);
      Vector3i.UnitY = new Vector3i(0, 1, 0);
      Vector3i.UnitZ = new Vector3i(0, 0, 1);
      Vector3i.MinValue = new Vector3i(int.MinValue, int.MinValue, int.MinValue);
      Vector3i.MaxValue = new Vector3i(int.MaxValue, int.MaxValue, int.MaxValue);
    }
  }
}
