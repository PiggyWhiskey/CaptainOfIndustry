// Decompiled with JetBrains decompiler
// Type: Mafi.Matrix2i
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

#nullable disable
namespace Mafi
{
  [DebuggerStepThrough]
  [StructLayout(LayoutKind.Explicit)]
  public readonly struct Matrix2i : IEquatable<Matrix2i>
  {
    public static readonly Matrix2i Zero;
    public static readonly Matrix2i Identity;
    /// <summary>Row 0, col 0.</summary>
    [FieldOffset(0)]
    public readonly int M00;
    /// <summary>Row 0, col 1.</summary>
    [FieldOffset(4)]
    public readonly int M01;
    /// <summary>Row 1, col 0.</summary>
    [FieldOffset(8)]
    public readonly int M10;
    /// <summary>Row 1, col 1.</summary>
    [FieldOffset(12)]
    public readonly int M11;

    public Matrix2i(int m00, int m01, int m10, int m11)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.M00 = m00;
      this.M01 = m01;
      this.M10 = m10;
      this.M11 = m11;
    }

    public static Matrix2i FromRotationFlip(Rotation90 rotation, bool reflectX)
    {
      int num = reflectX ? -1 : 1;
      switch (rotation.AngleIndex)
      {
        case 0:
          return new Matrix2i(num, 0, 0, 1);
        case 1:
          return new Matrix2i(0, -1, num, 0);
        case 2:
          return new Matrix2i(-1 * num, 0, 0, -1);
        case 3:
          return new Matrix2i(0, 1, -1 * num, 0);
        default:
          Assert.Fail(string.Format("Unknown rotation value {0}.", (object) rotation));
          return Matrix2i.Identity;
      }
    }

    public Matrix2i Inverted()
    {
      int num = this.Determinant();
      if (num != 0)
        return new Matrix2i(this.M11 / num, -this.M10 / num, -this.M01 / num, this.M00 / num);
      Log.Error("Matrix cannot be inverted!");
      return this;
    }

    public int Determinant() => this.M00 * this.M11 - this.M01 * this.M10;

    [Pure]
    public Vector2i Transform(Vector2i v)
    {
      return new Vector2i(this.M00 * v.X + this.M01 * v.Y, this.M10 * v.X + this.M11 * v.Y);
    }

    [Pure]
    public Vector2f Transform(Vector2f v)
    {
      return new Vector2f(this.M00 * v.X + this.M01 * v.Y, this.M10 * v.X + this.M11 * v.Y);
    }

    public bool Equals(Matrix2i other)
    {
      return this.M00 == other.M00 && this.M01 == other.M01 && this.M10 == other.M10 && this.M11 == other.M11;
    }

    static Matrix2i()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      Matrix2i.Zero = new Matrix2i();
      Matrix2i.Identity = new Matrix2i(1, 0, 0, 1);
    }
  }
}
