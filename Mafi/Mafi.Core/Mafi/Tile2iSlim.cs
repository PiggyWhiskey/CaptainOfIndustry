// Decompiled with JetBrains decompiler
// Type: Mafi.Tile2iSlim
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
  [ManuallyWrittenSerialization]
  [DebuggerDisplay("({X}, {Y})")]
  [DebuggerStepThrough]
  [StructLayout(LayoutKind.Explicit)]
  public struct Tile2iSlim : IEquatable<Tile2iSlim>, IComparable<Tile2iSlim>
  {
    /// <summary>Vector (0, 0).</summary>
    public static readonly Tile2iSlim Zero;
    /// <summary>Vector (1, 1).</summary>
    public static readonly Tile2iSlim One;
    /// <summary>Vector (1, 0).</summary>
    public static readonly Tile2iSlim UnitX;
    /// <summary>Vector (0, 1).</summary>
    public static readonly Tile2iSlim UnitY;
    /// <summary>Vector (ushort.MinValue, ushort.MinValue).</summary>
    public static readonly Tile2iSlim MinValue;
    /// <summary>Vector (ushort.MaxValue, ushort.MaxValue).</summary>
    public static readonly Tile2iSlim MaxValue;
    /// <summary>The X component of this vector.</summary>
    [FieldOffset(0)]
    public readonly ushort X;
    /// <summary>The Y component of this vector.</summary>
    [FieldOffset(2)]
    public readonly ushort Y;
    /// <summary>Packed components.</summary>
    [FieldOffset(0)]
    public readonly uint XyPacked;

    /// <summary>Creates new Tile2iSlim from raw components.</summary>
    public Tile2iSlim(ushort x, ushort y)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.XyPacked = 0U;
      this.X = x;
      this.Y = y;
    }

    /// <summary>Converts this type to Vector2i.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector2i Vector2i => new Vector2i((int) this.X, (int) this.Y);

    /// <summary>Sum of all components of this vector.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int Sum => (int) this.X + (int) this.Y;

    /// <summary>Product of all components of this vector.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int Product => (int) this.X * (int) this.Y;

    /// <summary>
    /// Euclidean length of this vector.
    /// PERF: Expensive, uses sqrt. Consider using <see cref="P:Mafi.Tile2iSlim.LengthSqr" /> whenever possible (when comparing
    /// lengths, etc.).
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Fix32 Length => Fix32.FromDouble(Math.Sqrt((double) this.LengthSqr));

    /// <summary>
    /// Approximate euclidean length of this vector as integer.
    /// PERF: Expensive, uses sqrt, consider using <see cref="P:Mafi.Tile2iSlim.LengthSqr" /> whenever possible.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int LengthInt => (int) Math.Round(Math.Sqrt((double) this.LengthSqr));

    /// <summary>
    /// Euclidean length squared of this vector.
    /// PERF: Cheaper than <see cref="P:Mafi.Tile2iSlim.Length" />, does not require expensive sqrt.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int LengthSqrInt => (int) this.X * (int) this.X + (int) this.Y * (int) this.Y;

    /// <summary>
    /// Euclidean length squared of this vector.
    /// PERF: Cheaper than <see cref="P:Mafi.Tile2iSlim.Length" />, does not require expensive sqrt.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int LengthSqr => (int) this.X * (int) this.X + (int) this.Y * (int) this.Y;

    /// <summary>Whether this vector has all components equal to zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsZero => this.XyPacked == 0U;

    /// <summary>e
    /// Whether this vector has at least one components not equal to zero.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNotZero => this.XyPacked > 0U;

    /// <summary>Returns new vector with changed X component.</summary>
    [Pure]
    public Tile2iSlim SetX(ushort newX) => new Tile2iSlim(newX, this.Y);

    /// <summary>Returns new vector with changed Y component.</summary>
    [Pure]
    public Tile2iSlim SetY(ushort newY) => new Tile2iSlim(this.X, newY);

    /// <summary>Returns new vector with incremented X component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile2iSlim IncrementX => new Tile2iSlim((ushort) ((uint) this.X + 1U), this.Y);

    /// <summary>Returns new vector with incremented Y component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile2iSlim IncrementY => new Tile2iSlim(this.X, (ushort) ((uint) this.Y + 1U));

    /// <summary>Returns new vector with decremented X component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile2iSlim DecrementX => new Tile2iSlim((ushort) ((uint) this.X - 1U), this.Y);

    /// <summary>Returns new vector with decremented Y component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile2iSlim DecrementY => new Tile2iSlim(this.X, (ushort) ((uint) this.Y - 1U));

    /// <summary>
    /// Returns new vector with given value added to the X component.
    /// </summary>
    [Pure]
    public Tile2iSlim AddX(int addedX)
    {
      return new Tile2iSlim((ushort) ((uint) this.X + (uint) addedX), this.Y);
    }

    /// <summary>
    /// Returns new vector with given value added to the Y component.
    /// </summary>
    [Pure]
    public Tile2iSlim AddY(int addedY)
    {
      return new Tile2iSlim(this.X, (ushort) ((uint) this.Y + (uint) addedY));
    }

    /// <summary>
    /// Returns new vector with given value added to all components.
    /// </summary>
    [Pure]
    public Tile2iSlim AddXy(int addedValue)
    {
      return new Tile2iSlim((ushort) ((uint) this.X + (uint) addedValue), (ushort) ((uint) this.Y + (uint) addedValue));
    }

    /// <summary>
    /// Returns new vector with given value multiplied with the X component.
    /// </summary>
    [Pure]
    public Tile2iSlim MultiplyX(int multX)
    {
      return new Tile2iSlim((ushort) ((uint) this.X * (uint) multX), this.Y);
    }

    /// <summary>
    /// Returns new vector with given value multiplied with the Y component.
    /// </summary>
    [Pure]
    public Tile2iSlim MultiplyY(int multY)
    {
      return new Tile2iSlim(this.X, (ushort) ((uint) this.Y * (uint) multY));
    }

    /// <summary>
    /// Whether corresponding components of this and given vectors are within given tolerance.
    /// </summary>
    [Pure]
    public bool IsNear(Tile2iSlim other, ushort tolerance)
    {
      return this.X.IsNear(other.X, (int) tolerance) && this.Y.IsNear(other.Y, (int) tolerance);
    }

    /// <summary>Returns dot product of this vector with given vector.</summary>
    [Pure]
    public int Dot(Tile2iSlim rhs) => (int) this.X * (int) rhs.X + (int) this.Y * (int) rhs.Y;

    /// <summary>
    /// Returns dot product of this vector with given vector as int32. Note that result of this method may overflow
    /// if magnitude of any component is larger than ~30,000.
    /// </summary>
    [Pure]
    public int DotInt(Tile2iSlim rhs) => (int) this.X * (int) rhs.X + (int) this.Y * (int) rhs.Y;

    /// <summary>
    /// Returns distance from this vector to the other vector.
    /// </summary>
    [Pure]
    public Fix32 DistanceTo(Tile2iSlim other)
    {
      return new Tile2i((int) this.X - (int) other.X, (int) this.Y - (int) other.Y).Length;
    }

    /// <summary>
    /// Returns squared distance from this vector to the other vector.
    /// </summary>
    [Pure]
    public int DistanceSqrTo(Tile2iSlim other)
    {
      return new Tile2i((int) this.X - (int) other.X, (int) this.Y - (int) other.Y).LengthSqrInt;
    }

    /// <summary>
    /// Returns absolute angle of this vector. Returned angle is in range [-τ/2, τ/2].
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public AngleDegrees1f Angle => MafiMath.Atan2((int) this.Y, (int) this.X);

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
    public int PseudoCross(Tile2iSlim other)
    {
      return (int) this.X * (int) other.Y - (int) this.Y * (int) other.X;
    }

    /// <summary>
    /// Returns signed angle from this vector to <paramref name="other" /> vector. Returned angle represents how much
    /// this vector has to be rotated to obtain <paramref name="other" /> vector. Returned value is [-τ/2, τ/2). This
    /// means that <c>v1.AngleTo(v2) == -v2.AngleTo(v1)</c> and <c>v1.Rotate(v1.AngleTo(v2)) == v2</c>.
    /// </summary>
    [Pure]
    public AngleDegrees1f AngleTo(Tile2iSlim other)
    {
      Assert.That<Tile2iSlim>(this).IsNotZero("AngleTo was called on zero vector.");
      Assert.That<Tile2iSlim>(other).IsNotZero("AngleTo was called with zero vector.");
      return MafiMath.Atan2(this.PseudoCross(other), this.Dot(other));
    }

    /// <summary>
    /// Returns absolute angle between this and <see paramref="other" /> vectors. Returned angle is in range [0, τ/2].
    /// </summary>
    [Pure]
    public AngleDegrees1f AngleBetween(Tile2iSlim other) => this.AngleTo(other).Abs;

    /// <summary>
    /// Whether this and <see paramref="other" /> vectors are parallel and not anti-parallel.
    /// </summary>
    [Pure]
    public bool IsParallelTo(Tile2iSlim other)
    {
      Assert.That<Tile2iSlim>(this).IsNotZero("IsParallelTo was called on zero vector.");
      Assert.That<Tile2iSlim>(other).IsNotZero("IsParallelTo was called with zero vector.");
      return this.PseudoCross(other) == 0 && this.Dot(other) > 0;
    }

    /// <summary>
    /// Whether this and <see paramref="other" /> vectors are anti-parallel and not parallel.
    /// </summary>
    [Pure]
    public bool IsAntiParallelTo(Tile2iSlim other)
    {
      Assert.That<Tile2iSlim>(this).IsNotZero("IsParallelTo was called on zero vector.");
      Assert.That<Tile2iSlim>(other).IsNotZero("IsParallelTo was called with zero vector.");
      return this.PseudoCross(other) == 0 && this.Dot(other) < 0;
    }

    /// <summary>
    /// Whether this and <see paramref="other" /> vectors are parallel or anti-parallel. This is more efficient than
    /// calling <see paramref="IsParallelTo" /> and <see paramref="IsAntiParallelTo" />.
    /// </summary>
    [Pure]
    public bool IsParallelOrAntiParallelTo(Tile2iSlim other)
    {
      Assert.That<Tile2iSlim>(this).IsNotZero("IsParallelTo was called on zero vector.");
      Assert.That<Tile2iSlim>(other).IsNotZero("IsParallelTo was called with zero vector.");
      return this.PseudoCross(other) == 0;
    }

    /// <summary>Returns component-wise min of this and given vectors.</summary>
    [Pure]
    public Tile2iSlim Min(Tile2iSlim rhs) => new Tile2iSlim(this.X.Min(rhs.X), this.Y.Min(rhs.Y));

    /// <summary>Returns component-wise min of this and given vectors.</summary>
    [Pure]
    public Tile2iSlim Max(Tile2iSlim rhs) => new Tile2iSlim(this.X.Max(rhs.X), this.Y.Max(rhs.Y));

    /// <summary>Returns component-wise min of this and given vectors.</summary>
    [Pure]
    public ushort MinComponent() => this.X.Min(this.Y);

    /// <summary>Returns component-wise max of this and given vectors.</summary>
    [Pure]
    public ushort MaxComponent() => this.X.Max(this.Y);

    /// <summary>Returns component-wise clamp of this vectors.</summary>
    [Pure]
    public Tile2iSlim Clamp(ushort min, ushort max)
    {
      return new Tile2iSlim(this.X.Clamp(min, max), this.Y.Clamp(min, max));
    }

    [Pure]
    public bool Equals(Tile2iSlim other) => other == this;

    [Pure]
    public override bool Equals(object other)
    {
      return other is Tile2iSlim tile2iSlim && tile2iSlim == this;
    }

    [Pure]
    public override int GetHashCode() => this.XyPacked.GetHashCode();

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
    public int CompareTo(Tile2iSlim other)
    {
      if ((int) this.X < (int) other.X)
        return -1;
      if ((int) this.X > (int) other.X)
        return 1;
      if ((int) this.Y < (int) other.Y)
        return -1;
      return (int) this.Y > (int) other.Y ? 1 : 0;
    }

    /// <summary>Exact equality of two vectors.</summary>
    public static bool operator ==(Tile2iSlim lhs, Tile2iSlim rhs)
    {
      return (int) lhs.XyPacked == (int) rhs.XyPacked;
    }

    /// <summary>Exact inequality of two vectors.</summary>
    public static bool operator !=(Tile2iSlim lhs, Tile2iSlim rhs)
    {
      return (int) lhs.XyPacked != (int) rhs.XyPacked;
    }

    /// <summary>
    /// Component-wise less-than operator. Returns true if all components of the left-hand side vector are less than
    /// respective components of the right-hand side vector.
    /// WARNING: <c>A &lt; B</c> is not equal to <c>A &gt;= B</c>. For example vectors (1, 2) and (2, 1) are not
    /// less-than nor greater-than-or-equal.
    /// </summary>
    public static bool operator <(Tile2iSlim lhs, Tile2iSlim rhs)
    {
      return (int) lhs.X < (int) rhs.X && (int) lhs.Y < (int) rhs.Y;
    }

    /// <summary>
    /// Component-wise less-than-or-equal operator. Returns true if all components of the left-hand side vector are
    /// less than or equal to respective components of the right-hand side vector.
    /// WARNING: <c>A &lt;= B</c> is not equal to <c>A &gt; B</c>. For example vectors (1, 2) and (2, 1) are not
    /// less-than-or-equal nor greater-than.
    /// </summary>
    public static bool operator <=(Tile2iSlim lhs, Tile2iSlim rhs)
    {
      return (int) lhs.X <= (int) rhs.X && (int) lhs.Y <= (int) rhs.Y;
    }

    /// <summary>
    /// Component-wise greater-than operator. Returns true if all components of the left-hand side vector are
    /// greater than respective components of the right-hand side vector.
    /// WARNING: <c>A &gt; B</c> is not equal to <c>A &lt;= B</c>. For example vectors (1, 2) and (2, 1) are not
    /// greater-than nor less-than-or-equal.
    /// </summary>
    public static bool operator >(Tile2iSlim lhs, Tile2iSlim rhs)
    {
      return (int) lhs.X > (int) rhs.X && (int) lhs.Y > (int) rhs.Y;
    }

    /// <summary>
    /// Component-wise greater-than-or-equal operator. Returns true if all components of the left-hand side vector
    /// are greater than or equal to respective components of the right-hand side vector.
    /// WARNING: <c>A &gt;= B</c> is not equal to <c>A &lt; B</c>. For example vectors (1, 2) and (2, 1) are not
    /// greater-than-or-equal nor less-than.
    /// </summary>
    public static bool operator >=(Tile2iSlim lhs, Tile2iSlim rhs)
    {
      return (int) lhs.X >= (int) rhs.X && (int) lhs.Y >= (int) rhs.Y;
    }

    public static Tile2iSlim operator +(int lhs, Tile2iSlim rhs)
    {
      return new Tile2iSlim((ushort) ((uint) lhs + (uint) rhs.X), (ushort) ((uint) lhs + (uint) rhs.Y));
    }

    public static Tile2iSlim operator +(Tile2iSlim lhs, int rhs)
    {
      return new Tile2iSlim((ushort) ((uint) lhs.X + (uint) rhs), (ushort) ((uint) lhs.Y + (uint) rhs));
    }

    public static Tile2iSlim operator -(int lhs, Tile2iSlim rhs)
    {
      return new Tile2iSlim((ushort) ((uint) lhs - (uint) rhs.X), (ushort) ((uint) lhs - (uint) rhs.Y));
    }

    public static Tile2iSlim operator -(Tile2iSlim lhs, int rhs)
    {
      return new Tile2iSlim((ushort) ((uint) lhs.X - (uint) rhs), (ushort) ((uint) lhs.Y - (uint) rhs));
    }

    public static void Serialize(Tile2iSlim value, BlobWriter writer)
    {
      writer.WriteUShort(value.X);
      writer.WriteUShort(value.Y);
    }

    public static Tile2iSlim Deserialize(BlobReader reader)
    {
      return new Tile2iSlim(reader.ReadUShort(), reader.ReadUShort());
    }

    public Tile2i AsFull => new Tile2i((int) this.X, (int) this.Y);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile2f CornerTile2f => new Tile2f((Fix32) this.X, (Fix32) this.Y);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile2f CenterTile2f
    {
      get => new Tile2f((Fix32) this.X + Fix32.Half, (Fix32) this.Y + Fix32.Half);
    }

    [Pure]
    public Tile3i ExtendHeight(HeightTilesI height)
    {
      return new Tile3i((int) this.X, (int) this.Y, height.Value);
    }

    [Pure]
    public Tile3i ExtendHeight(HeightTilesISlim height)
    {
      return new Tile3i((int) this.X, (int) this.Y, (int) height.Value);
    }

    [Pure]
    public Tile2iAndIndex ExtendIndex(TerrainManager manager) => manager.ExtendTileIndex(this);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Chunk2i ChunkCoord2i => new Chunk2i((int) this.X >> 6, (int) this.Y >> 6);

    public static implicit operator Tile2i(Tile2iSlim slimTile)
    {
      return new Tile2i((int) slimTile.X, (int) slimTile.Y);
    }

    public static Tile2iSlim operator +(Tile2iSlim lhs, RelTile2i rhs)
    {
      return new Tile2iSlim((ushort) ((uint) lhs.X + (uint) rhs.X), (ushort) ((uint) lhs.Y + (uint) rhs.Y));
    }

    public static Tile2iSlim operator +(RelTile2i lhs, Tile2iSlim rhs)
    {
      return new Tile2iSlim((ushort) ((uint) lhs.X + (uint) rhs.X), (ushort) ((uint) lhs.Y + (uint) rhs.Y));
    }

    public static Tile2iSlim operator &(Tile2iSlim lhs, int rhs)
    {
      return new Tile2iSlim((ushort) ((uint) lhs.X & (uint) rhs), (ushort) ((uint) lhs.Y & (uint) rhs));
    }

    public static bool operator <(Tile2iSlim lhs, Tile2i rhs)
    {
      return (int) lhs.X < rhs.X && (int) lhs.Y < rhs.Y;
    }

    public static bool operator <=(Tile2iSlim lhs, Tile2i rhs)
    {
      return (int) lhs.X <= rhs.X && (int) lhs.Y <= rhs.Y;
    }

    public static bool operator >(Tile2iSlim lhs, Tile2i rhs)
    {
      return (int) lhs.X > rhs.X && (int) lhs.Y > rhs.Y;
    }

    public static bool operator >=(Tile2iSlim lhs, Tile2i rhs)
    {
      return (int) lhs.X >= rhs.X && (int) lhs.Y >= rhs.Y;
    }

    public static bool operator <(Tile2i lhs, Tile2iSlim rhs)
    {
      return lhs.X < (int) rhs.X && lhs.Y < (int) rhs.Y;
    }

    public static bool operator <=(Tile2i lhs, Tile2iSlim rhs)
    {
      return lhs.X <= (int) rhs.X && lhs.Y <= (int) rhs.Y;
    }

    public static bool operator >(Tile2i lhs, Tile2iSlim rhs)
    {
      return lhs.X > (int) rhs.X && lhs.Y > (int) rhs.Y;
    }

    public static bool operator >=(Tile2i lhs, Tile2iSlim rhs)
    {
      return lhs.X >= (int) rhs.X && lhs.Y >= (int) rhs.Y;
    }

    static Tile2iSlim()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Tile2iSlim.Zero = new Tile2iSlim();
      Tile2iSlim.One = new Tile2iSlim((ushort) 1, (ushort) 1);
      Tile2iSlim.UnitX = new Tile2iSlim((ushort) 1, (ushort) 0);
      Tile2iSlim.UnitY = new Tile2iSlim((ushort) 0, (ushort) 1);
      Tile2iSlim.MinValue = new Tile2iSlim((ushort) 0, (ushort) 0);
      Tile2iSlim.MaxValue = new Tile2iSlim(ushort.MaxValue, ushort.MaxValue);
    }
  }
}
