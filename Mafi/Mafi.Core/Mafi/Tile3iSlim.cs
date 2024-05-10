// Decompiled with JetBrains decompiler
// Type: Mafi.Tile3iSlim
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using Mafi.Utils;
using System;
using System.Diagnostics;
using System.Text;

#nullable disable
namespace Mafi
{
  [ExpectedStructSize(6)]
  [DebuggerDisplay("({X}, {Y}, {Z})")]
  [ManuallyWrittenSerialization]
  [DebuggerStepThrough]
  public readonly struct Tile3iSlim : IEquatable<Tile3iSlim>, IComparable<Tile3iSlim>
  {
    public static readonly Tile3iSlim Zero;
    public static readonly Tile3iSlim One;
    public static readonly Tile3iSlim UnitX;
    public static readonly Tile3iSlim UnitY;
    public static readonly Tile3iSlim UnitZ;
    public static readonly Tile3iSlim MinValue;
    public static readonly Tile3iSlim MaxValue;
    public readonly ushort X;
    public readonly ushort Y;
    public readonly short Z;

    public Tile3iSlim(ushort x, ushort y, short z)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.X = x;
      this.Y = y;
      this.Z = z;
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile2iSlim Xy => new Tile2iSlim(this.X, this.Y);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile3i Tile3i => new Tile3i((int) this.X, (int) this.Y, (int) this.Z);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector3i Vector3i => new Vector3i((int) this.X, (int) this.Y, (int) this.Z);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int Sum => (int) this.X + (int) this.Y + (int) this.Z;

    /// <summary>
    /// Euclidean length of this vector.
    /// PERF: Expensive, uses sqrt. Consider using <see cref="P:Mafi.Tile3iSlim.LengthSqr" /> whenever possible (when comparing
    /// lengths, etc.).
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Fix32 Length => Fix32.FromDouble(Math.Sqrt((double) this.LengthSqr));

    /// <summary>
    /// Approximate euclidean length of this vector as integer.
    /// PERF: Expensive, uses sqrt, consider using <see cref="P:Mafi.Tile3iSlim.LengthSqr" /> whenever possible.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int LengthInt => (int) Math.Round(Math.Sqrt((double) this.LengthSqr));

    /// <summary>
    /// Euclidean length squared of this vector.
    /// PERF: Cheaper than <see cref="P:Mafi.Tile3iSlim.Length" />, does not require expensive sqrt.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int LengthSqr
    {
      get
      {
        return (int) this.X * (int) this.X + (int) this.Y * (int) this.Y + (int) this.Z * (int) this.Z;
      }
    }

    /// <summary>Whether this vector has all components equal to zero.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsZero => this.X == (ushort) 0 && this.Y == (ushort) 0 && this.Z == (short) 0;

    /// <summary>e
    /// Whether this vector has at least one components not equal to zero.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNotZero => this.X != (ushort) 0 || this.Y != (ushort) 0 || this.Z != (short) 0;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public AngleDegrees1f Angle => MafiMath.Atan2((int) this.Y, (int) this.X);

    /// <summary>Returns new vector with changed X component.</summary>
    public Tile3iSlim SetX(ushort newX) => new Tile3iSlim(newX, this.Y, this.Z);

    /// <summary>Returns new vector with changed Y component.</summary>
    public Tile3iSlim SetY(ushort newY) => new Tile3iSlim(this.X, newY, this.Z);

    /// <summary>Returns new vector with changed Z component.</summary>
    public Tile3iSlim SetZ(short newZ) => new Tile3iSlim(this.X, this.Y, newZ);

    /// <summary>Returns new vector with changed X and Y components.</summary>
    public Tile3iSlim SetXy(ushort newX, ushort newY) => new Tile3iSlim(newX, newY, this.Z);

    /// <summary>
    /// Returns new vector with changed X, Y, and Z components.
    /// </summary>
    public Tile3iSlim SetXyz(ushort newX, ushort newY, short newZ)
    {
      return new Tile3iSlim(newX, newY, newZ);
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile3iSlim IncrementX => new Tile3iSlim((ushort) ((uint) this.X + 1U), this.Y, this.Z);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile3iSlim IncrementY => new Tile3iSlim(this.X, (ushort) ((uint) this.Y + 1U), this.Z);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile3iSlim IncrementZ => new Tile3iSlim(this.X, this.Y, (short) ((int) this.Z + 1));

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile3iSlim DecrementX => new Tile3iSlim((ushort) ((uint) this.X - 1U), this.Y, this.Z);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile3iSlim DecrementY => new Tile3iSlim(this.X, (ushort) ((uint) this.Y - 1U), this.Z);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Tile3iSlim DecrementZ => new Tile3iSlim(this.X, this.Y, (short) ((int) this.Z - 1));

    public Tile3iSlim AddX(int addedX)
    {
      return new Tile3iSlim((ushort) ((uint) this.X + (uint) addedX), this.Y, this.Z);
    }

    public Tile3iSlim AddY(int addedY)
    {
      return new Tile3iSlim(this.X, (ushort) ((uint) this.Y + (uint) addedY), this.Z);
    }

    public Tile3iSlim AddZ(int addedZ)
    {
      return new Tile3iSlim(this.X, this.Y, (short) ((int) this.Z + addedZ));
    }

    public Tile3iSlim AddXyz(int addedValue)
    {
      return new Tile3iSlim((ushort) ((uint) this.X + (uint) addedValue), (ushort) ((uint) this.Y + (uint) addedValue), (short) ((int) this.Z + addedValue));
    }

    public Tile3iSlim MultiplyX(int multX)
    {
      return new Tile3iSlim((ushort) ((uint) this.X * (uint) multX), this.Y, this.Z);
    }

    public Tile3iSlim MultiplyY(int multY)
    {
      return new Tile3iSlim(this.X, (ushort) ((uint) this.Y * (uint) multY), this.Z);
    }

    public Tile3iSlim MultiplyZ(int multZ)
    {
      return new Tile3iSlim(this.X, this.Y, (short) ((int) this.Z * multZ));
    }

    public bool IsNear(Tile3iSlim other, int tolerance)
    {
      return this.X.IsNear(other.X, (int) (ushort) tolerance) && this.Y.IsNear(other.Y, (int) (ushort) tolerance) && this.Z.IsNear(other.Z, tolerance);
    }

    public int Dot(Tile3iSlim rhs)
    {
      return (int) this.X * (int) rhs.X + (int) this.Y * (int) rhs.Y + (int) this.Z * (int) rhs.Z;
    }

    public int DotInt(Tile3iSlim rhs)
    {
      return (int) this.X * (int) rhs.X + (int) this.Y * (int) rhs.Y + (int) this.Z * (int) rhs.Z;
    }

    public Fix32 DistanceTo(Tile3iSlim other)
    {
      return new Tile3i((int) this.X - (int) other.X, (int) this.Y - (int) other.Y, (int) this.Z - (int) other.Z).Length;
    }

    public int DistanceSqrTo(Tile3iSlim other)
    {
      return new Tile3i((int) this.X - (int) other.X, (int) this.Y - (int) other.Y, (int) this.Z - (int) other.Z).LengthSqrInt;
    }

    public Tile3i Cross(Tile3iSlim rhs)
    {
      return new Tile3i((int) this.Y * (int) rhs.Z - (int) this.Z * (int) rhs.Y, (int) this.Z * (int) rhs.X - (int) this.X * (int) rhs.Z, (int) this.X * (int) rhs.Y - (int) this.Y * (int) rhs.X);
    }

    /// <summary>
    /// Whether this and <see paramref="other" /> vectors are parallel and not anti-parallel.
    /// </summary>
    [Pure]
    public bool IsParallelTo(Tile3iSlim other)
    {
      Assert.That<Tile3iSlim>(this).IsNotZero("IsParallelTo was called on zero vector.");
      Assert.That<Tile3iSlim>(other).IsNotZero("IsParallelTo was called with zero vector.");
      return this.Cross(other).IsZero && this.Dot(other) > 0;
    }

    /// <summary>
    /// Whether this and <see paramref="other" /> vectors are anti-parallel and not parallel.
    /// </summary>
    [Pure]
    public bool IsAntiParallelTo(Tile3iSlim other)
    {
      Assert.That<Tile3iSlim>(this).IsNotZero("IsAntiParallelTo was called on zero vector.");
      Assert.That<Tile3iSlim>(other).IsNotZero("IsAntiParallelTo was called with zero vector.");
      return this.Cross(other).IsZero && this.Dot(other) < 0;
    }

    /// <summary>
    /// Whether this and <see paramref="other" /> vectors are parallel or anti-parallel. This is more efficient than
    /// calling <see paramref="IsParallelTo" /> and <see paramref="IsAntiParallelTo" />.
    /// </summary>
    [Pure]
    public bool IsParallelOrAntiParallelTo(Tile3iSlim other)
    {
      Assert.That<Tile3iSlim>(this).IsNotZero("IsParallelOrAntiParallelTo was called on zero vector.");
      Assert.That<Tile3iSlim>(other).IsNotZero("IsParallelOrAntiParallelTo was called with zero vector.");
      return this.Cross(other).IsZero;
    }

    /// <summary>
    /// Returns absolute angle between this and <see paramref="other" /> vectors. Returned angle is in range [0, τ/2].
    /// </summary>
    [Pure]
    public AngleDegrees1f AngleBetween(Tile3iSlim other)
    {
      return MafiMath.Atan2(this.Cross(other).Length.ToFix64(), (Fix64) (long) this.Dot(other));
    }

    /// <summary>Returns component-wise min of this and given vectors.</summary>
    [Pure]
    public Tile3iSlim Min(Tile3iSlim rhs)
    {
      return new Tile3iSlim(this.X.Min(rhs.X), this.Y.Min(rhs.Y), this.Z.Min(rhs.Z));
    }

    /// <summary>Returns component-wise min of this and given vectors.</summary>
    [Pure]
    public Tile3iSlim Max(Tile3iSlim rhs)
    {
      return new Tile3iSlim(this.X.Max(rhs.X), this.Y.Max(rhs.Y), this.Z.Max(rhs.Z));
    }

    /// <summary>Returns component-wise min of this and given vectors.</summary>
    [Pure]
    public int MinComponent() => ((int) this.X).Min((int) this.Y).Min((int) this.Z);

    /// <summary>Returns component-wise max of this and given vectors.</summary>
    [Pure]
    public int MaxComponent() => ((int) this.X).Max((int) this.Y).Max((int) this.Z);

    [Pure]
    public bool Equals(Tile3iSlim other) => other == this;

    [Pure]
    public override bool Equals(object other)
    {
      return other is Tile3iSlim tile3iSlim && tile3iSlim == this;
    }

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
    public int CompareTo(Tile3iSlim other)
    {
      if ((int) this.X < (int) other.X)
        return -1;
      if ((int) this.X > (int) other.X)
        return 1;
      if ((int) this.Y < (int) other.Y)
        return -1;
      if ((int) this.Y > (int) other.Y)
        return 1;
      if ((int) this.Z < (int) other.Z)
        return -1;
      return (int) this.Z > (int) other.Z ? 1 : 0;
    }

    public static bool operator ==(Tile3iSlim lhs, Tile3iSlim rhs)
    {
      return (int) lhs.X == (int) rhs.X && (int) lhs.Y == (int) rhs.Y && (int) lhs.Z == (int) rhs.Z;
    }

    public static bool operator !=(Tile3iSlim lhs, Tile3iSlim rhs)
    {
      return (int) lhs.X != (int) rhs.X || (int) lhs.Y != (int) rhs.Y || (int) lhs.Z != (int) rhs.Z;
    }

    /// <summary>
    /// Component-wise less-than operator. Returns true if all components of the left-hand side vector are less than
    /// respective components of the right-hand side vector.
    /// WARNING: <c>A &lt; B</c> is not equal to <c>A &gt;= B</c>. For example vectors (1, 2, 3) and (3, 2, 1) are not
    /// less-than nor greater-than-or-equal.
    /// </summary>
    public static bool operator <(Tile3iSlim lhs, Tile3iSlim rhs)
    {
      return (int) lhs.X < (int) rhs.X && (int) lhs.Y < (int) rhs.Y && (int) lhs.Z < (int) rhs.Z;
    }

    /// <summary>
    /// Component-wise less-than-or-equal operator. Returns true if all components of the left-hand side vector are
    /// less than or equal to respective components of the right-hand side vector.
    /// WARNING: <c>A &lt;= B</c> is not equal to <c>A &gt; B</c>. For example vectors (1, 2, 3) and (3, 2, 1) are not
    /// less-than-or-equal nor greater-than.
    /// </summary>
    public static bool operator <=(Tile3iSlim lhs, Tile3iSlim rhs)
    {
      return (int) lhs.X <= (int) rhs.X && (int) lhs.Y <= (int) rhs.Y && (int) lhs.Z <= (int) rhs.Z;
    }

    /// <summary>
    /// Component-wise greater-than operator. Returns true if all components of the left-hand side vector are
    /// greater than respective components of the right-hand side vector.
    /// WARNING: <c>A &gt; B</c> is not equal to <c>A &lt;= B</c>. For example vectors (1, 2, 3) and (3, 2, 1) are not
    /// greater-than nor less-than-or-equal.
    /// </summary>
    public static bool operator >(Tile3iSlim lhs, Tile3iSlim rhs)
    {
      return (int) lhs.X > (int) rhs.X && (int) lhs.Y > (int) rhs.Y && (int) lhs.Z > (int) rhs.Z;
    }

    /// <summary>
    /// Component-wise greater-than-or-equal operator. Returns true if all components of the left-hand side vector
    /// are greater than or equal to respective components of the right-hand side vector.
    /// WARNING: <c>A &gt;= B</c> is not equal to <c>A &lt; B</c>. For example vectors (1, 2, 3) and (3, 2, 1) are not
    /// greater-than-or-equal nor less-than.
    /// </summary>
    public static bool operator >=(Tile3iSlim lhs, Tile3iSlim rhs)
    {
      return (int) lhs.X >= (int) rhs.X && (int) lhs.Y >= (int) rhs.Y && (int) lhs.Z >= (int) rhs.Z;
    }

    public static Tile3i operator +(int lhs, Tile3iSlim rhs)
    {
      return new Tile3i(lhs + (int) rhs.X, lhs + (int) rhs.Y, lhs + (int) rhs.Z);
    }

    public static Tile3i operator +(Tile3iSlim lhs, int rhs)
    {
      return new Tile3i((int) lhs.X + rhs, (int) lhs.Y + rhs, (int) lhs.Z + rhs);
    }

    public static Tile3i operator -(int lhs, Tile3iSlim rhs)
    {
      return new Tile3i(lhs - (int) rhs.X, lhs - (int) rhs.Y, lhs - (int) rhs.Z);
    }

    public static Tile3i operator -(Tile3iSlim lhs, int rhs)
    {
      return new Tile3i((int) lhs.X - rhs, (int) lhs.Y - rhs, (int) lhs.Z - rhs);
    }

    public static void Serialize(Tile3iSlim value, BlobWriter writer)
    {
      writer.WriteUShort(value.X);
      writer.WriteUShort(value.Y);
      writer.WriteShort(value.Z);
    }

    public static Tile3iSlim Deserialize(BlobReader reader)
    {
      return new Tile3iSlim(reader.ReadUShort(), reader.ReadUShort(), reader.ReadShort());
    }

    static Tile3iSlim()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Tile3iSlim.Zero = new Tile3iSlim();
      Tile3iSlim.One = new Tile3iSlim((ushort) 1, (ushort) 1, (short) 1);
      Tile3iSlim.UnitX = new Tile3iSlim((ushort) 1, (ushort) 0, (short) 0);
      Tile3iSlim.UnitY = new Tile3iSlim((ushort) 0, (ushort) 1, (short) 0);
      Tile3iSlim.UnitZ = new Tile3iSlim((ushort) 0, (ushort) 0, (short) 1);
      Tile3iSlim.MinValue = new Tile3iSlim((ushort) 0, (ushort) 0, short.MinValue);
      Tile3iSlim.MaxValue = new Tile3iSlim(ushort.MaxValue, ushort.MaxValue, short.MaxValue);
    }
  }
}
