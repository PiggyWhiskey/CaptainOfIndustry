// Decompiled with JetBrains decompiler
// Type: Mafi.Vector4i
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
  /// <summary>Immutable 4D int vector.</summary>
  /// <remarks>
  /// This is partial struct and this file should contain only specific members for <see cref="T:Mafi.Vector4i" />. All general
  /// members should be added to the generator T4 template.
  /// </remarks>
  [DebuggerDisplay("({X}, {Y}, {Z}, {W})")]
  [DebuggerStepThrough]
  [ManuallyWrittenSerialization]
  public struct Vector4i : IEquatable<Vector4i>, IComparable<Vector4i>
  {
    /// <summary>Vector (0, 0, 0, 0).</summary>
    public static readonly Vector4i Zero;
    /// <summary>Vector (1, 1, 1, 1).</summary>
    public static readonly Vector4i One;
    /// <summary>Vector (1, 0, 0, 0).</summary>
    public static readonly Vector4i UnitX;
    /// <summary>Vector (0, 1, 0, 0).</summary>
    public static readonly Vector4i UnitY;
    /// <summary>Vector (0, 0, 1, 0).</summary>
    public static readonly Vector4i UnitZ;
    /// <summary>Vector (0, 0, 0, 1).</summary>
    public static readonly Vector4i UnitW;
    /// <summary>
    /// Vector (int.MinValue, int.MinValue, int.MinValue, int.MinValue).
    /// </summary>
    public static readonly Vector4i MinValue;
    /// <summary>
    /// Vector (int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue).
    /// </summary>
    public static readonly Vector4i MaxValue;
    /// <summary>The X component of this vector.</summary>
    public readonly int X;
    /// <summary>The Y component of this vector.</summary>
    public readonly int Y;
    /// <summary>The Z component of this vector.</summary>
    public readonly int Z;
    /// <summary>The W component of this vector.</summary>
    public readonly int W;

    /// <summary>Creates new Vector4i from raw components.</summary>
    public Vector4i(int x, int y, int z, int w)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.X = x;
      this.Y = y;
      this.Z = z;
      this.W = w;
    }

    /// <summary>
    /// Creates new Vector4i from Vector2i and raw components.
    /// </summary>
    public Vector4i(Vector2i vector, int z, int w)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.X = vector.X;
      this.Y = vector.Y;
      this.Z = z;
      this.W = w;
    }

    /// <summary>
    /// Creates new Vector4i from Vector3i and raw components.
    /// </summary>
    public Vector4i(Vector3i vector, int w)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.X = vector.X;
      this.Y = vector.Y;
      this.Z = vector.Z;
      this.W = w;
    }

    /// <summary>Gets the first two components of this vector.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector2i Xy => new Vector2i(this.X, this.Y);

    /// <summary>Gets the first three components of this vector.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector3i Xyz => new Vector3i(this.X, this.Y, this.Z);

    /// <summary>Sum of all components of this vector.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int Sum => this.X + this.Y + this.Z + this.W;

    /// <summary>
    /// Euclidean length of this vector.
    /// PERF: Expensive, uses sqrt. Consider using <see cref="P:Mafi.Vector4i.LengthSqr" /> whenever possible (when comparing
    /// lengths, etc.).
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Fix32 Length => Fix32.FromDouble(Math.Sqrt((double) this.LengthSqr));

    /// <summary>
    /// Approximate euclidean length of this vector as integer.
    /// PERF: Expensive, uses sqrt, consider using <see cref="P:Mafi.Vector4i.LengthSqr" /> whenever possible.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int LengthInt => (int) Math.Round(Math.Sqrt((double) this.LengthSqr));

    /// <summary>
    /// Euclidean length squared of this vector.
    /// PERF: Cheaper than <see cref="P:Mafi.Vector4i.Length" />, does not require expensive sqrt.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int LengthSqrInt
    {
      get => this.X * this.X + this.Y * this.Y + this.Z * this.Z + this.W * this.W;
    }

    /// <summary>
    /// Euclidean length squared of this vector.
    /// PERF: Cheaper than <see cref="P:Mafi.Vector4i.Length" />, does not require expensive sqrt.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public long LengthSqr
    {
      get
      {
        return (long) this.X * (long) this.X + (long) this.Y * (long) this.Y + (long) this.Z * (long) this.Z + (long) this.W * (long) this.W;
      }
    }

    /// <summary>Whether this vector has all components equal to zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsZero => this.X == 0 && this.Y == 0 && this.Z == 0 && this.W == 0;

    /// <summary>e
    /// Whether this vector has at least one components not equal to zero.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNotZero => this.X != 0 || this.Y != 0 || this.Z != 0 || this.W != 0;

    /// <summary>Returns new vector with changed X component.</summary>
    [Pure]
    public Vector4i SetX(int newX) => new Vector4i(newX, this.Y, this.Z, this.W);

    /// <summary>Returns new vector with changed Y component.</summary>
    [Pure]
    public Vector4i SetY(int newY) => new Vector4i(this.X, newY, this.Z, this.W);

    /// <summary>Returns new vector with changed Z component.</summary>
    [Pure]
    public Vector4i SetZ(int newZ) => new Vector4i(this.X, this.Y, newZ, this.W);

    /// <summary>Returns new vector with changed W component.</summary>
    [Pure]
    public Vector4i SetW(int newW) => new Vector4i(this.X, this.Y, this.Z, newW);

    /// <summary>Returns new vector with changed X and Y components.</summary>
    [Pure]
    public Vector4i SetXy(int newX, int newY) => new Vector4i(newX, newY, this.Z, this.W);

    /// <summary>Returns new vector with changed X and Y components.</summary>
    [Pure]
    public Vector4i SetXy(Vector2i newXy) => new Vector4i(newXy.X, newXy.Y, this.Z, this.W);

    /// <summary>
    /// Returns new vector with changed X, Y, and Z components.
    /// </summary>
    [Pure]
    public Vector4i SetXyz(int newX, int newY, int newZ) => new Vector4i(newX, newY, newZ, this.W);

    /// <summary>
    /// Returns new vector with changed X, Y, and Z components.
    /// </summary>
    [Pure]
    public Vector4i SetXyz(Vector3i newXyz) => new Vector4i(newXyz.X, newXyz.Y, newXyz.Z, this.W);

    /// <summary>Returns new vector with incremented X component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector4i IncrementX => new Vector4i(this.X + 1, this.Y, this.Z, this.W);

    /// <summary>Returns new vector with incremented Y component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector4i IncrementY => new Vector4i(this.X, this.Y + 1, this.Z, this.W);

    /// <summary>Returns new vector with incremented Z component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector4i IncrementZ => new Vector4i(this.X, this.Y, this.Z + 1, this.W);

    /// <summary>Returns new vector with incremented W component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector4i IncrementW => new Vector4i(this.X, this.Y, this.Z, this.W + 1);

    /// <summary>Returns new vector with decremented X component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector4i DecrementX => new Vector4i(this.X - 1, this.Y, this.Z, this.W);

    /// <summary>Returns new vector with decremented Y component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector4i DecrementY => new Vector4i(this.X, this.Y - 1, this.Z, this.W);

    /// <summary>Returns new vector with decremented Z component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector4i DecrementZ => new Vector4i(this.X, this.Y, this.Z - 1, this.W);

    /// <summary>Returns new vector with decremented W component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector4i DecrementW => new Vector4i(this.X, this.Y, this.Z, this.W - 1);

    /// <summary>
    /// Returns new vector with given value added to the X component.
    /// </summary>
    [Pure]
    public Vector4i AddX(int addedX) => new Vector4i(this.X + addedX, this.Y, this.Z, this.W);

    /// <summary>
    /// Returns new vector with given value added to the Y component.
    /// </summary>
    [Pure]
    public Vector4i AddY(int addedY) => new Vector4i(this.X, this.Y + addedY, this.Z, this.W);

    /// <summary>
    /// Returns new vector with given value added to the Z component.
    /// </summary>
    [Pure]
    public Vector4i AddZ(int addedZ) => new Vector4i(this.X, this.Y, this.Z + addedZ, this.W);

    /// <summary>
    /// Returns new vector with given value added to the W component.
    /// </summary>
    [Pure]
    public Vector4i AddW(int addedW) => new Vector4i(this.X, this.Y, this.Z, this.W + addedW);

    /// <summary>
    /// Returns new vector with given value added to all components.
    /// </summary>
    [Pure]
    public Vector4i AddXyzw(int addedValue)
    {
      return new Vector4i(this.X + addedValue, this.Y + addedValue, this.Z + addedValue, this.W + addedValue);
    }

    /// <summary>
    /// Returns new vector with given value multiplied with the X component.
    /// </summary>
    [Pure]
    public Vector4i MultiplyX(int multX) => new Vector4i(this.X * multX, this.Y, this.Z, this.W);

    /// <summary>
    /// Returns new vector with given value multiplied with the Y component.
    /// </summary>
    [Pure]
    public Vector4i MultiplyY(int multY) => new Vector4i(this.X, this.Y * multY, this.Z, this.W);

    /// <summary>
    /// Returns new vector with given value multiplied with the Z component.
    /// </summary>
    [Pure]
    public Vector4i MultiplyZ(int multZ) => new Vector4i(this.X, this.Y, this.Z * multZ, this.W);

    /// <summary>
    /// Returns new vector with given value multiplied with the W component.
    /// </summary>
    [Pure]
    public Vector4i MultiplyW(int multW) => new Vector4i(this.X, this.Y, this.Z, this.W * multW);

    /// <summary>
    /// Returns new vector with reflected X component (opposite sign).
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector4i ReflectX => new Vector4i(-this.X, this.Y, this.Z, this.W);

    /// <summary>
    /// Returns new vector with reflected Y component (opposite sign).
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector4i ReflectY => new Vector4i(this.X, -this.Y, this.Z, this.W);

    /// <summary>
    /// Returns new vector with reflected Z component (opposite sign).
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector4i ReflectZ => new Vector4i(this.X, this.Y, -this.Z, this.W);

    /// <summary>
    /// Returns new vector with reflected W component (opposite sign).
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector4i ReflectW => new Vector4i(this.X, this.Y, this.Z, -this.W);

    /// <summary>Gets Vector4f representation of this vector.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector4f Vector4f
    {
      get
      {
        return new Vector4f(Fix32.FromInt(this.X), Fix32.FromInt(this.Y), Fix32.FromInt(this.Z), Fix32.FromInt(this.W));
      }
    }

    /// <summary>
    /// Multiples and divides all components. This method is using long precision to prevent int32 overflows.
    /// </summary>
    public Vector4i MulDiv(long mul, long div)
    {
      return new Vector4i((int) (mul * (long) this.X / div), (int) (mul * (long) this.Y / div), (int) (mul * (long) this.Z / div), (int) (mul * (long) this.W / div));
    }

    /// <summary>
    /// Returns scaled vector to requested length. This method is more precise, faster and more intuitive than
    /// normalization followed by multiplication.
    /// WARNING: Setting length of integer vector may not produce exact requested length do to rounding error.
    /// </summary>
    [Pure]
    public Vector4i OfLength(int desiredLength)
    {
      double num1 = Math.Sqrt((double) this.LengthSqr);
      double num2 = (double) desiredLength / num1;
      return new Vector4i(((double) this.X * num2).RoundToInt(), ((double) this.Y * num2).RoundToInt(), ((double) this.Z * num2).RoundToInt(), ((double) this.W * num2).RoundToInt());
    }

    /// <summary>
    /// Whether corresponding components of this and given vectors are within given tolerance.
    /// </summary>
    [Pure]
    public bool IsNear(Vector4i other, int tolerance)
    {
      return this.X.IsNear(other.X, tolerance) && this.Y.IsNear(other.Y, tolerance) && this.Z.IsNear(other.Z, tolerance) && this.W.IsNear(other.W, tolerance);
    }

    /// <summary>Returns dot product of this vector with given vector.</summary>
    [Pure]
    public long Dot(Vector4i rhs)
    {
      return (long) this.X * (long) rhs.X + (long) this.Y * (long) rhs.Y + (long) this.Z * (long) rhs.Z + (long) this.W * (long) rhs.W;
    }

    /// <summary>
    /// Returns dot product of this vector with given vector as int32. Note that result of this method may overflow
    /// if magnitude of any component is larger than ~30,000.
    /// </summary>
    [Pure]
    public int DotInt(Vector4i rhs)
    {
      return this.X * rhs.X + this.Y * rhs.Y + this.Z * rhs.Z + this.W * rhs.W;
    }

    /// <summary>
    /// Returns distance from this vector to the other vector.
    /// </summary>
    [Pure]
    public Fix32 DistanceTo(Vector4i other)
    {
      return new Vector4i(this.X - other.X, this.Y - other.Y, this.Z - other.Z, this.W - other.W).Length;
    }

    /// <summary>
    /// Returns squared distance from this vector to the other vector.
    /// </summary>
    [Pure]
    public long DistanceSqrTo(Vector4i other)
    {
      return new Vector4i(this.X - other.X, this.Y - other.Y, this.Z - other.Z, this.W - other.W).LengthSqr;
    }

    /// <summary>
    /// Returns absolute angle of this vector. Returned angle is in range [-τ/2, τ/2].
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public AngleDegrees1f Angle => MafiMath.Atan2(this.Y, this.X);

    /// <summary>Returns component-wise min of this and given vectors.</summary>
    [Pure]
    public Vector4i Min(Vector4i rhs)
    {
      return new Vector4i(this.X.Min(rhs.X), this.Y.Min(rhs.Y), this.Z.Min(rhs.Z), this.W.Min(rhs.W));
    }

    /// <summary>Returns component-wise min of this and given vectors.</summary>
    [Pure]
    public Vector4i Max(Vector4i rhs)
    {
      return new Vector4i(this.X.Max(rhs.X), this.Y.Max(rhs.Y), this.Z.Max(rhs.Z), this.W.Max(rhs.W));
    }

    /// <summary>Returns component-wise min of this and given vectors.</summary>
    [Pure]
    public int MinComponent() => this.X.Min(this.Y).Min(this.Z).Min(this.W);

    /// <summary>Returns component-wise max of this and given vectors.</summary>
    [Pure]
    public int MaxComponent() => this.X.Max(this.Y).Max(this.Z).Max(this.W);

    /// <summary>Returns component-wise clamp of this vectors.</summary>
    [Pure]
    public Vector4i Clamp(int min, int max)
    {
      return new Vector4i(this.X.Clamp(min, max), this.Y.Clamp(min, max), this.Z.Clamp(min, max), this.W.Clamp(min, max));
    }

    /// <summary>Returns component-wise absolute value of this vector.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector4i AbsValue
    {
      get => new Vector4i(this.X.Abs(), this.Y.Abs(), this.Z.Abs(), this.W.Abs());
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
    public Vector4i Modulo(int mod)
    {
      return new Vector4i(this.X.Modulo(mod), this.Y.Modulo(mod), this.Z.Modulo(mod), this.W.Modulo(mod));
    }

    /// <summary>
    /// Returns component-wise modulo operation on this vector (see <see cref="M:Mafi.MafiMath.Modulo(System.Int32,System.Int32)" />).
    /// </summary>
    [Pure]
    public Vector4i Modulo(Vector4i mod)
    {
      return new Vector4i(this.X.Modulo(mod.X), this.Y.Modulo(mod.Y), this.Z.Modulo(mod.Z), this.W.Modulo(mod.W));
    }

    /// <summary>
    /// Returns component-wise average of this and given vectors.
    /// </summary>
    [Pure]
    public Vector4i Average(Vector4i rhs)
    {
      return new Vector4i(this.X + rhs.X >> 1, this.Y + rhs.Y >> 1, this.Z + rhs.Z >> 1, this.W + rhs.W >> 1);
    }

    [Pure]
    public Vector4i Lerp(Vector4i to, Percent t)
    {
      return new Vector4i(this.X.Lerp(to.X, t), this.Y.Lerp(to.Y, t), this.Z.Lerp(to.Z, t), this.W.Lerp(to.W, t));
    }

    /// <summary>
    /// Linearly interpolates between this and <paramref name="to" /> vectors based on <paramref name="t" />.
    /// Interpolation parameter <paramref name="t" /> goes from 0 to <paramref name="scale" />.
    /// See <see cref="M:Mafi.MafiMath.Lerp(System.Int32,System.Int32,System.Int64,System.Int64)" /> for details.
    /// </summary>
    [Pure]
    public Vector4i Lerp(Vector4i to, long t, long scale)
    {
      return new Vector4i(this.X.Lerp(to.X, t, scale), this.Y.Lerp(to.Y, t, scale), this.Z.Lerp(to.Z, t, scale), this.W.Lerp(to.W, t, scale));
    }

    /// <summary>
    /// Linearly interpolates between <paramref name="from" /> and <paramref name="to" /> vectors based on <paramref name="t" />. Interpolation parameter <paramref name="t" /> goes from 0 to <paramref name="scale" />. See <see cref="M:Mafi.MafiMath.Lerp(System.Int32,System.Int32,System.Int64,System.Int64)" /> for details.
    /// </summary>
    public static Vector4i Lerp(Vector4i from, Vector4i to, long t, long scale)
    {
      return from.Lerp(to, t, scale);
    }

    [Pure]
    public bool Equals(Vector4i other) => other == this;

    [Pure]
    public override bool Equals(object other) => other is Vector4i vector4i && vector4i == this;

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
    public int CompareTo(Vector4i other)
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
    public static bool operator ==(Vector4i lhs, Vector4i rhs)
    {
      return lhs.X == rhs.X && lhs.Y == rhs.Y && lhs.Z == rhs.Z && lhs.W == rhs.W;
    }

    /// <summary>Exact inequality of two vectors.</summary>
    public static bool operator !=(Vector4i lhs, Vector4i rhs)
    {
      return lhs.X != rhs.X || lhs.Y != rhs.Y || lhs.Z != rhs.Z || lhs.W != rhs.W;
    }

    /// <summary>
    /// Component-wise less-than operator. Returns true if all components of the left-hand side vector are less than
    /// respective components of the right-hand side vector.
    /// WARNING: <c>A &lt; B</c> is not equal to <c>A &gt;= B</c>. For example vectors (1, 2, 3, 4) and (4, 3, 2, 1) are not
    /// less-than nor greater-than-or-equal.
    /// </summary>
    public static bool operator <(Vector4i lhs, Vector4i rhs)
    {
      return lhs.X < rhs.X && lhs.Y < rhs.Y && lhs.Z < rhs.Z && lhs.W < rhs.W;
    }

    /// <summary>
    /// Component-wise less-than-or-equal operator. Returns true if all components of the left-hand side vector are
    /// less than or equal to respective components of the right-hand side vector.
    /// WARNING: <c>A &lt;= B</c> is not equal to <c>A &gt; B</c>. For example vectors (1, 2, 3, 4) and (4, 3, 2, 1) are not
    /// less-than-or-equal nor greater-than.
    /// </summary>
    public static bool operator <=(Vector4i lhs, Vector4i rhs)
    {
      return lhs.X <= rhs.X && lhs.Y <= rhs.Y && lhs.Z <= rhs.Z && lhs.W <= rhs.W;
    }

    /// <summary>
    /// Component-wise greater-than operator. Returns true if all components of the left-hand side vector are
    /// greater than respective components of the right-hand side vector.
    /// WARNING: <c>A &gt; B</c> is not equal to <c>A &lt;= B</c>. For example vectors (1, 2, 3, 4) and (4, 3, 2, 1) are not
    /// greater-than nor less-than-or-equal.
    /// </summary>
    public static bool operator >(Vector4i lhs, Vector4i rhs)
    {
      return lhs.X > rhs.X && lhs.Y > rhs.Y && lhs.Z > rhs.Z && lhs.W > rhs.W;
    }

    /// <summary>
    /// Component-wise greater-than-or-equal operator. Returns true if all components of the left-hand side vector
    /// are greater than or equal to respective components of the right-hand side vector.
    /// WARNING: <c>A &gt;= B</c> is not equal to <c>A &lt; B</c>. For example vectors (1, 2, 3, 4) and (4, 3, 2, 1) are not
    /// greater-than-or-equal nor less-than.
    /// </summary>
    public static bool operator >=(Vector4i lhs, Vector4i rhs)
    {
      return lhs.X >= rhs.X && lhs.Y >= rhs.Y && lhs.Z >= rhs.Z && lhs.W >= rhs.W;
    }

    public static Vector4i operator +(int lhs, Vector4i rhs)
    {
      return new Vector4i(lhs + rhs.X, lhs + rhs.Y, lhs + rhs.Z, lhs + rhs.W);
    }

    public static Vector4i operator +(Vector4i lhs, int rhs)
    {
      return new Vector4i(lhs.X + rhs, lhs.Y + rhs, lhs.Z + rhs, lhs.W + rhs);
    }

    public static Vector4i operator +(Vector4i lhs, Vector4i rhs)
    {
      return new Vector4i(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z, lhs.W + rhs.W);
    }

    public static Vector4i operator +(Vector4i lhs, Vector2i rhs)
    {
      return new Vector4i(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z, lhs.W);
    }

    public static Vector4i operator +(Vector4i lhs, Vector3i rhs)
    {
      return new Vector4i(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z, lhs.W);
    }

    public static Vector4i operator -(Vector4i vector)
    {
      return new Vector4i(-vector.X, -vector.Y, -vector.Z, -vector.W);
    }

    public static Vector4i operator -(int lhs, Vector4i rhs)
    {
      return new Vector4i(lhs - rhs.X, lhs - rhs.Y, lhs - rhs.Z, lhs - rhs.W);
    }

    public static Vector4i operator -(Vector4i lhs, int rhs)
    {
      return new Vector4i(lhs.X - rhs, lhs.Y - rhs, lhs.Z - rhs, lhs.W - rhs);
    }

    public static Vector4i operator -(Vector4i lhs, Vector4i rhs)
    {
      return new Vector4i(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z, lhs.W - rhs.W);
    }

    public static Vector4i operator -(Vector4i lhs, Vector2i rhs)
    {
      return new Vector4i(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z, lhs.W);
    }

    public static Vector4i operator -(Vector4i lhs, Vector3i rhs)
    {
      return new Vector4i(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z, lhs.W);
    }

    public static Vector4i operator *(Vector4i lhs, int rhs)
    {
      return new Vector4i(lhs.X * rhs, lhs.Y * rhs, lhs.Z * rhs, lhs.W * rhs);
    }

    public static Vector4i operator *(int lhs, Vector4i rhs)
    {
      return new Vector4i(rhs.X * lhs, rhs.Y * lhs, rhs.Z * lhs, rhs.W * lhs);
    }

    public static Vector4i operator *(Vector4i lhs, Percent rhs)
    {
      return new Vector4i(rhs.Apply(lhs.X), rhs.Apply(lhs.Y), rhs.Apply(lhs.Z), rhs.Apply(lhs.W));
    }

    public static Vector4i operator *(Percent lhs, Vector4i rhs)
    {
      return new Vector4i(lhs.Apply(rhs.X), lhs.Apply(rhs.Y), lhs.Apply(rhs.Z), lhs.Apply(rhs.W));
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector4i Times2Fast => new Vector4i(this.X << 1, this.Y << 1, this.Z << 1, this.W << 1);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector4i Times4Fast => new Vector4i(this.X << 2, this.Y << 2, this.Z << 2, this.W << 2);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector4i Times8Fast => new Vector4i(this.X << 3, this.Y << 3, this.Z << 3, this.W << 3);

    public static Vector4i operator /(int lhs, Vector4i rhs)
    {
      return new Vector4i(lhs / rhs.X, lhs / rhs.Y, lhs / rhs.Z, lhs / rhs.W);
    }

    public static Vector4i operator /(Vector4i lhs, int rhs)
    {
      return new Vector4i(lhs.X / rhs, lhs.Y / rhs, lhs.Z / rhs, lhs.W / rhs);
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector4i HalfFast => new Vector4i(this.X >> 1, this.Y >> 1, this.Z >> 1, this.W >> 1);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector4i DivBy4Fast => new Vector4i(this.X >> 2, this.Y >> 2, this.Z >> 2, this.W >> 2);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector4i DivBy8Fast => new Vector4i(this.X >> 3, this.Y >> 3, this.Z >> 3, this.W >> 3);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector4i DivBy16Fast => new Vector4i(this.X >> 4, this.Y >> 4, this.Z >> 4, this.W >> 4);

    /// <summary>
    /// Computes floor division. Unlike normal division operator in C# this always rounds down.
    /// </summary>
    public Vector4i FloorDiv(int rhs)
    {
      return new Vector4i(this.X.FloorDiv(rhs), this.Y.FloorDiv(rhs), this.Z.FloorDiv(rhs), this.W.FloorDiv(rhs));
    }

    public Vector4i CeilDiv(int rhs)
    {
      return new Vector4i(this.X.CeilDiv(rhs), this.Y.CeilDiv(rhs), this.Z.CeilDiv(rhs), this.W.CeilDiv(rhs));
    }

    /// <summary>Component-wise division of two vectors.</summary>
    public static Vector4i operator /(Vector4i lhs, Vector4i rhs)
    {
      return new Vector4i(lhs.X / rhs.X, lhs.Y / rhs.Y, lhs.Z / rhs.Z, lhs.W / rhs.W);
    }

    /// <summary>
    /// Computes floor division. Unlike normal division operator in C# this always rounds down.
    /// </summary>
    public Vector4i FloorDiv(Vector4i rhs)
    {
      return new Vector4i(this.X.FloorDiv(rhs.X), this.Y.FloorDiv(rhs.Y), this.Z.FloorDiv(rhs.Z), this.W.FloorDiv(rhs.W));
    }

    public static Vector4i operator %(int lhs, Vector4i rhs)
    {
      return new Vector4i(lhs % rhs.X, lhs % rhs.Y, lhs % rhs.Z, lhs % rhs.W);
    }

    public static Vector4i operator %(Vector4i lhs, int rhs)
    {
      return new Vector4i(lhs.X % rhs, lhs.Y % rhs, lhs.Z % rhs, lhs.W % rhs);
    }

    /// <summary>Component-wise modulo of two vectors.</summary>
    public static Vector4i operator %(Vector4i lhs, Vector4i rhs)
    {
      return new Vector4i(lhs.X % rhs.X, lhs.Y % rhs.Y, lhs.Z % rhs.Z, lhs.W % rhs.W);
    }

    public static void Serialize(Vector4i value, BlobWriter writer)
    {
      writer.WriteInt(value.X);
      writer.WriteInt(value.Y);
      writer.WriteInt(value.Z);
      writer.WriteInt(value.W);
    }

    public static Vector4i Deserialize(BlobReader reader)
    {
      return new Vector4i(reader.ReadInt(), reader.ReadInt(), reader.ReadInt(), reader.ReadInt());
    }

    static Vector4i()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      Vector4i.Zero = new Vector4i();
      Vector4i.One = new Vector4i(1, 1, 1, 1);
      Vector4i.UnitX = new Vector4i(1, 0, 0, 0);
      Vector4i.UnitY = new Vector4i(0, 1, 0, 0);
      Vector4i.UnitZ = new Vector4i(0, 0, 1, 0);
      Vector4i.UnitW = new Vector4i(0, 0, 0, 1);
      Vector4i.MinValue = new Vector4i(int.MinValue, int.MinValue, int.MinValue, int.MinValue);
      Vector4i.MaxValue = new Vector4i(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue);
    }
  }
}
