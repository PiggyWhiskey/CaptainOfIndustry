// Decompiled with JetBrains decompiler
// Type: Mafi.TileInChunk2iSlim
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

#nullable disable
namespace Mafi
{
  [ManuallyWrittenSerialization]
  [DebuggerStepThrough]
  [DebuggerDisplay("({X}, {Y})")]
  [StructLayout(LayoutKind.Explicit)]
  public struct TileInChunk2iSlim : IEquatable<TileInChunk2iSlim>, IComparable<TileInChunk2iSlim>
  {
    /// <summary>Vector (0, 0).</summary>
    public static readonly TileInChunk2iSlim Zero;
    /// <summary>Vector (1, 1).</summary>
    public static readonly TileInChunk2iSlim One;
    /// <summary>Vector (1, 0).</summary>
    public static readonly TileInChunk2iSlim UnitX;
    /// <summary>Vector (0, 1).</summary>
    public static readonly TileInChunk2iSlim UnitY;
    /// <summary>The X component of this vector.</summary>
    [FieldOffset(0)]
    public readonly byte X;
    /// <summary>The Y component of this vector.</summary>
    [FieldOffset(1)]
    public readonly byte Y;
    /// <summary>Packed components.</summary>
    [FieldOffset(0)]
    public readonly ushort XyPacked;

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
    /// PERF: Expensive, uses sqrt. Consider using <see cref="P:Mafi.TileInChunk2iSlim.LengthSqr" /> whenever possible (when comparing
    /// lengths, etc.).
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Fix32 Length => Fix32.FromDouble(Math.Sqrt((double) this.LengthSqr));

    /// <summary>
    /// Approximate euclidean length of this vector as integer.
    /// PERF: Expensive, uses sqrt, consider using <see cref="P:Mafi.TileInChunk2iSlim.LengthSqr" /> whenever possible.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int LengthInt => (int) Math.Round(Math.Sqrt((double) this.LengthSqr));

    /// <summary>
    /// Euclidean length squared of this vector.
    /// PERF: Cheaper than <see cref="P:Mafi.TileInChunk2iSlim.Length" />, does not require expensive sqrt.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int LengthSqrInt => (int) this.X * (int) this.X + (int) this.Y * (int) this.Y;

    /// <summary>
    /// Euclidean length squared of this vector.
    /// PERF: Cheaper than <see cref="P:Mafi.TileInChunk2iSlim.Length" />, does not require expensive sqrt.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int LengthSqr => (int) this.X * (int) this.X + (int) this.Y * (int) this.Y;

    /// <summary>Whether this vector has all components equal to zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsZero => this.XyPacked == (ushort) 0;

    /// <summary>e
    /// Whether this vector has at least one components not equal to zero.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNotZero => this.XyPacked > (ushort) 0;

    /// <summary>Returns new vector with changed X component.</summary>
    [Pure]
    public TileInChunk2iSlim SetX(byte newX) => new TileInChunk2iSlim(newX, this.Y);

    /// <summary>Returns new vector with changed Y component.</summary>
    [Pure]
    public TileInChunk2iSlim SetY(byte newY) => new TileInChunk2iSlim(this.X, newY);

    /// <summary>Returns new vector with incremented X component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public TileInChunk2iSlim IncrementX
    {
      get => new TileInChunk2iSlim((byte) ((uint) this.X + 1U), this.Y);
    }

    /// <summary>Returns new vector with incremented Y component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public TileInChunk2iSlim IncrementY
    {
      get => new TileInChunk2iSlim(this.X, (byte) ((uint) this.Y + 1U));
    }

    /// <summary>Returns new vector with decremented X component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public TileInChunk2iSlim DecrementX
    {
      get => new TileInChunk2iSlim((byte) ((uint) this.X - 1U), this.Y);
    }

    /// <summary>Returns new vector with decremented Y component.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public TileInChunk2iSlim DecrementY
    {
      get => new TileInChunk2iSlim(this.X, (byte) ((uint) this.Y - 1U));
    }

    /// <summary>
    /// Returns new vector with given value added to the X component.
    /// </summary>
    [Pure]
    public TileInChunk2iSlim AddX(int addedX)
    {
      return new TileInChunk2iSlim((byte) ((uint) this.X + (uint) addedX), this.Y);
    }

    /// <summary>
    /// Returns new vector with given value added to the Y component.
    /// </summary>
    [Pure]
    public TileInChunk2iSlim AddY(int addedY)
    {
      return new TileInChunk2iSlim(this.X, (byte) ((uint) this.Y + (uint) addedY));
    }

    /// <summary>
    /// Returns new vector with given value added to all components.
    /// </summary>
    [Pure]
    public TileInChunk2iSlim AddXy(int addedValue)
    {
      return new TileInChunk2iSlim((byte) ((uint) this.X + (uint) addedValue), (byte) ((uint) this.Y + (uint) addedValue));
    }

    /// <summary>
    /// Returns new vector with given value multiplied with the X component.
    /// </summary>
    [Pure]
    public TileInChunk2iSlim MultiplyX(int multX)
    {
      return new TileInChunk2iSlim((byte) ((uint) this.X * (uint) multX), this.Y);
    }

    /// <summary>
    /// Returns new vector with given value multiplied with the Y component.
    /// </summary>
    [Pure]
    public TileInChunk2iSlim MultiplyY(int multY)
    {
      return new TileInChunk2iSlim(this.X, (byte) ((uint) this.Y * (uint) multY));
    }

    /// <summary>
    /// Whether corresponding components of this and given vectors are within given tolerance.
    /// </summary>
    [Pure]
    public bool IsNear(TileInChunk2iSlim other, byte tolerance)
    {
      return this.X.IsNear(other.X, (int) tolerance) && this.Y.IsNear(other.Y, (int) tolerance);
    }

    /// <summary>Returns dot product of this vector with given vector.</summary>
    [Pure]
    public int Dot(TileInChunk2iSlim rhs)
    {
      return (int) this.X * (int) rhs.X + (int) this.Y * (int) rhs.Y;
    }

    /// <summary>
    /// Returns dot product of this vector with given vector as int32. Note that result of this method may overflow
    /// if magnitude of any component is larger than ~30,000.
    /// </summary>
    [Pure]
    public int DotInt(TileInChunk2iSlim rhs)
    {
      return (int) this.X * (int) rhs.X + (int) this.Y * (int) rhs.Y;
    }

    /// <summary>
    /// Returns distance from this vector to the other vector.
    /// </summary>
    [Pure]
    public Fix32 DistanceTo(TileInChunk2iSlim other)
    {
      return new Tile2i((int) this.X - (int) other.X, (int) this.Y - (int) other.Y).Length;
    }

    /// <summary>
    /// Returns squared distance from this vector to the other vector.
    /// </summary>
    [Pure]
    public int DistanceSqrTo(TileInChunk2iSlim other)
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
    public int PseudoCross(TileInChunk2iSlim other)
    {
      return (int) this.X * (int) other.Y - (int) this.Y * (int) other.X;
    }

    /// <summary>
    /// Returns signed angle from this vector to <paramref name="other" /> vector. Returned angle represents how much
    /// this vector has to be rotated to obtain <paramref name="other" /> vector. Returned value is [-τ/2, τ/2). This
    /// means that <c>v1.AngleTo(v2) == -v2.AngleTo(v1)</c> and <c>v1.Rotate(v1.AngleTo(v2)) == v2</c>.
    /// </summary>
    [Pure]
    public AngleDegrees1f AngleTo(TileInChunk2iSlim other)
    {
      Assert.That<TileInChunk2iSlim>(this).IsNotZero("AngleTo was called on zero vector.");
      Assert.That<TileInChunk2iSlim>(other).IsNotZero("AngleTo was called with zero vector.");
      return MafiMath.Atan2(this.PseudoCross(other), this.Dot(other));
    }

    /// <summary>
    /// Returns absolute angle between this and <see paramref="other" /> vectors. Returned angle is in range [0, τ/2].
    /// </summary>
    [Pure]
    public AngleDegrees1f AngleBetween(TileInChunk2iSlim other) => this.AngleTo(other).Abs;

    /// <summary>
    /// Whether this and <see paramref="other" /> vectors are parallel and not anti-parallel.
    /// </summary>
    [Pure]
    public bool IsParallelTo(TileInChunk2iSlim other)
    {
      Assert.That<TileInChunk2iSlim>(this).IsNotZero("IsParallelTo was called on zero vector.");
      Assert.That<TileInChunk2iSlim>(other).IsNotZero("IsParallelTo was called with zero vector.");
      return this.PseudoCross(other) == 0 && this.Dot(other) > 0;
    }

    /// <summary>
    /// Whether this and <see paramref="other" /> vectors are anti-parallel and not parallel.
    /// </summary>
    [Pure]
    public bool IsAntiParallelTo(TileInChunk2iSlim other)
    {
      Assert.That<TileInChunk2iSlim>(this).IsNotZero("IsParallelTo was called on zero vector.");
      Assert.That<TileInChunk2iSlim>(other).IsNotZero("IsParallelTo was called with zero vector.");
      return this.PseudoCross(other) == 0 && this.Dot(other) < 0;
    }

    /// <summary>
    /// Whether this and <see paramref="other" /> vectors are parallel or anti-parallel. This is more efficient than
    /// calling <see paramref="IsParallelTo" /> and <see paramref="IsAntiParallelTo" />.
    /// </summary>
    [Pure]
    public bool IsParallelOrAntiParallelTo(TileInChunk2iSlim other)
    {
      Assert.That<TileInChunk2iSlim>(this).IsNotZero("IsParallelTo was called on zero vector.");
      Assert.That<TileInChunk2iSlim>(other).IsNotZero("IsParallelTo was called with zero vector.");
      return this.PseudoCross(other) == 0;
    }

    /// <summary>Returns component-wise min of this and given vectors.</summary>
    [Pure]
    public TileInChunk2iSlim Min(TileInChunk2iSlim rhs)
    {
      return new TileInChunk2iSlim(this.X.Min(rhs.X), this.Y.Min(rhs.Y));
    }

    /// <summary>Returns component-wise min of this and given vectors.</summary>
    [Pure]
    public TileInChunk2iSlim Max(TileInChunk2iSlim rhs)
    {
      return new TileInChunk2iSlim(this.X.Max(rhs.X), this.Y.Max(rhs.Y));
    }

    /// <summary>Returns component-wise min of this and given vectors.</summary>
    [Pure]
    public byte MinComponent() => this.X.Min(this.Y);

    /// <summary>Returns component-wise max of this and given vectors.</summary>
    [Pure]
    public byte MaxComponent() => this.X.Max(this.Y);

    /// <summary>Returns component-wise clamp of this vectors.</summary>
    [Pure]
    public TileInChunk2iSlim Clamp(byte min, byte max)
    {
      return new TileInChunk2iSlim(this.X.Clamp(min, max), this.Y.Clamp(min, max));
    }

    [Pure]
    public bool Equals(TileInChunk2iSlim other) => other == this;

    [Pure]
    public override bool Equals(object other)
    {
      return other is TileInChunk2iSlim tileInChunk2iSlim && tileInChunk2iSlim == this;
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
    public int CompareTo(TileInChunk2iSlim other)
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
    public static bool operator ==(TileInChunk2iSlim lhs, TileInChunk2iSlim rhs)
    {
      return (int) lhs.XyPacked == (int) rhs.XyPacked;
    }

    /// <summary>Exact inequality of two vectors.</summary>
    public static bool operator !=(TileInChunk2iSlim lhs, TileInChunk2iSlim rhs)
    {
      return (int) lhs.XyPacked != (int) rhs.XyPacked;
    }

    /// <summary>
    /// Component-wise less-than operator. Returns true if all components of the left-hand side vector are less than
    /// respective components of the right-hand side vector.
    /// WARNING: <c>A &lt; B</c> is not equal to <c>A &gt;= B</c>. For example vectors (1, 2) and (2, 1) are not
    /// less-than nor greater-than-or-equal.
    /// </summary>
    public static bool operator <(TileInChunk2iSlim lhs, TileInChunk2iSlim rhs)
    {
      return (int) lhs.X < (int) rhs.X && (int) lhs.Y < (int) rhs.Y;
    }

    /// <summary>
    /// Component-wise less-than-or-equal operator. Returns true if all components of the left-hand side vector are
    /// less than or equal to respective components of the right-hand side vector.
    /// WARNING: <c>A &lt;= B</c> is not equal to <c>A &gt; B</c>. For example vectors (1, 2) and (2, 1) are not
    /// less-than-or-equal nor greater-than.
    /// </summary>
    public static bool operator <=(TileInChunk2iSlim lhs, TileInChunk2iSlim rhs)
    {
      return (int) lhs.X <= (int) rhs.X && (int) lhs.Y <= (int) rhs.Y;
    }

    /// <summary>
    /// Component-wise greater-than operator. Returns true if all components of the left-hand side vector are
    /// greater than respective components of the right-hand side vector.
    /// WARNING: <c>A &gt; B</c> is not equal to <c>A &lt;= B</c>. For example vectors (1, 2) and (2, 1) are not
    /// greater-than nor less-than-or-equal.
    /// </summary>
    public static bool operator >(TileInChunk2iSlim lhs, TileInChunk2iSlim rhs)
    {
      return (int) lhs.X > (int) rhs.X && (int) lhs.Y > (int) rhs.Y;
    }

    /// <summary>
    /// Component-wise greater-than-or-equal operator. Returns true if all components of the left-hand side vector
    /// are greater than or equal to respective components of the right-hand side vector.
    /// WARNING: <c>A &gt;= B</c> is not equal to <c>A &lt; B</c>. For example vectors (1, 2) and (2, 1) are not
    /// greater-than-or-equal nor less-than.
    /// </summary>
    public static bool operator >=(TileInChunk2iSlim lhs, TileInChunk2iSlim rhs)
    {
      return (int) lhs.X >= (int) rhs.X && (int) lhs.Y >= (int) rhs.Y;
    }

    public static TileInChunk2iSlim operator +(int lhs, TileInChunk2iSlim rhs)
    {
      return new TileInChunk2iSlim((byte) ((uint) lhs + (uint) rhs.X), (byte) ((uint) lhs + (uint) rhs.Y));
    }

    public static TileInChunk2iSlim operator +(TileInChunk2iSlim lhs, int rhs)
    {
      return new TileInChunk2iSlim((byte) ((uint) lhs.X + (uint) rhs), (byte) ((uint) lhs.Y + (uint) rhs));
    }

    public static TileInChunk2iSlim operator -(int lhs, TileInChunk2iSlim rhs)
    {
      return new TileInChunk2iSlim((byte) ((uint) lhs - (uint) rhs.X), (byte) ((uint) lhs - (uint) rhs.Y));
    }

    public static TileInChunk2iSlim operator -(TileInChunk2iSlim lhs, int rhs)
    {
      return new TileInChunk2iSlim((byte) ((uint) lhs.X - (uint) rhs), (byte) ((uint) lhs.Y - (uint) rhs));
    }

    public static void Serialize(TileInChunk2iSlim value, BlobWriter writer)
    {
      writer.WriteByte(value.X);
      writer.WriteByte(value.Y);
    }

    public static TileInChunk2iSlim Deserialize(BlobReader reader)
    {
      return new TileInChunk2iSlim(reader.ReadByte(), reader.ReadByte());
    }

    /// <summary>Creates new TileInChunk2i from raw components.</summary>
    public TileInChunk2iSlim(byte x, byte y)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.XyPacked = (ushort) 0;
      this.X = x;
      this.Y = y;
    }

    public static Tile2i operator +(TileInChunk2iSlim localTileCoord, Chunk2i chunkCoord)
    {
      return new Tile2i((chunkCoord.X << 6) + (int) localTileCoord.X, (chunkCoord.Y << 6) + (int) localTileCoord.Y);
    }

    public static Tile2i operator +(Chunk2i chunkCoord, TileInChunk2iSlim localTileCoord)
    {
      return new Tile2i((chunkCoord.X << 6) + (int) localTileCoord.X, (chunkCoord.Y << 6) + (int) localTileCoord.Y);
    }

    static TileInChunk2iSlim()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TileInChunk2iSlim.Zero = new TileInChunk2iSlim();
      TileInChunk2iSlim.One = new TileInChunk2iSlim((byte) 1, (byte) 1);
      TileInChunk2iSlim.UnitX = new TileInChunk2iSlim((byte) 1, (byte) 0);
      TileInChunk2iSlim.UnitY = new TileInChunk2iSlim((byte) 0, (byte) 1);
    }
  }
}
