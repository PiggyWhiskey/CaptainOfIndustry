// Decompiled with JetBrains decompiler
// Type: Mafi.ColorRgba
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;
using System.Diagnostics;

#nullable disable
namespace Mafi
{
  /// <summary>
  /// Compact representation of RGBA color that uses only one <see cref="T:System.UInt32" />. This is 4x smaller than Unity's
  /// Color.
  /// </summary>
  [DebuggerStepThrough]
  [ManuallyWrittenSerialization]
  public readonly struct ColorRgba : IEquatable<ColorRgba>, IComparable<ColorRgba>
  {
    public static readonly ColorRgba Black;
    public static readonly ColorRgba DarkDarkGray;
    public static readonly ColorRgba DarkGray;
    public static readonly ColorRgba Gray;
    public static readonly ColorRgba LightGray;
    public static readonly ColorRgba White;
    public static readonly ColorRgba Red;
    public static readonly ColorRgba DarkRed;
    public static readonly ColorRgba Brown;
    public static readonly ColorRgba Orange;
    public static readonly ColorRgba Gold;
    public static readonly ColorRgba Yellow;
    public static readonly ColorRgba LightYellow;
    public static readonly ColorRgba DarkYellow;
    public static readonly ColorRgba GreenYellow;
    public static readonly ColorRgba Green;
    public static readonly ColorRgba DarkGreen;
    public static readonly ColorRgba Turquoise;
    public static readonly ColorRgba Cyan;
    public static readonly ColorRgba Blue;
    public static readonly ColorRgba LightBlue;
    public static readonly ColorRgba CornflowerBlue;
    public static readonly ColorRgba Magenta;
    public static readonly ColorRgba Purple;
    /// <summary>Transparent black color.</summary>
    public static readonly ColorRgba Empty;
    /// <summary>Value containing ARGB value packed as unsigned int.</summary>
    public readonly uint Rgba;

    public ColorRgba(uint rgba)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.Rgba = rgba;
    }

    /// <summary>
    /// Creates RGBA color from byte values in range 0 to 255.
    /// </summary>
    public ColorRgba(byte r, byte g, byte b, byte a = 255)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.Rgba = (uint) ((int) r << 24 | (int) g << 16 | (int) b << 8) | (uint) a;
    }

    /// <summary>
    /// Creates RGBA color from int values in range 0 to 255. This is convenience overload so called do not need to
    /// cast ints to bytes.
    /// </summary>
    public ColorRgba(int r, int g, int b, int a = 255)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.Rgba = (uint) ((r & (int) byte.MaxValue) << 24 | (g & (int) byte.MaxValue) << 16 | (b & (int) byte.MaxValue) << 8 | a & (int) byte.MaxValue);
    }

    /// <summary>Creates RGBA color from RGB hex and alpha.</summary>
    public ColorRgba(int rgb, int a = 255)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.Rgba = (uint) (rgb << 8 | a & (int) byte.MaxValue);
    }

    /// <summary>
    /// Creates RGBA color form float values in range 0.0 to 1.0.
    /// </summary>
    public ColorRgba(float r, float g, float b, float a = 1f)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.Rgba = (uint) (((int) (uint) ((double) r * (double) byte.MaxValue) & (int) byte.MaxValue) << 24 | ((int) (uint) ((double) g * (double) byte.MaxValue) & (int) byte.MaxValue) << 16 | ((int) (uint) ((double) b * (double) byte.MaxValue) & (int) byte.MaxValue) << 8 | (int) (uint) ((double) a * (double) byte.MaxValue) & (int) byte.MaxValue);
    }

    public byte R => (byte) (this.Rgba >> 24 & (uint) byte.MaxValue);

    public byte G => (byte) (this.Rgba >> 16 & (uint) byte.MaxValue);

    public byte B => (byte) (this.Rgba >> 8 & (uint) byte.MaxValue);

    public byte A => (byte) (this.Rgba & (uint) byte.MaxValue);

    /// <summary>Whether the color is empty (transparent black).</summary>
    public bool IsEmpty => this.Rgba == 0U;

    public bool IsNotEmpty => this.Rgba > 0U;

    [Pure]
    public ColorRgba SetR(byte r)
    {
      return new ColorRgba((uint) ((int) this.Rgba & 16777215 | (int) r << 24));
    }

    [Pure]
    public ColorRgba SetG(byte g)
    {
      return new ColorRgba((uint) ((int) this.Rgba & -16711681 | (int) g << 16));
    }

    [Pure]
    public ColorRgba SetB(byte b)
    {
      return new ColorRgba((uint) ((int) this.Rgba & -65281 | (int) b << 8));
    }

    [Pure]
    public ColorRgba SetA(byte a) => new ColorRgba(this.Rgba & 4294967040U | (uint) a);

    [Pure]
    public ColorRgba ScaleRgbBy(float scale)
    {
      return new ColorRgba((int) ((float) this.R * scale).Min((float) byte.MaxValue), (int) ((float) this.G * scale).Min((float) byte.MaxValue), (int) ((float) this.B * scale).Min((float) byte.MaxValue), (int) byte.MaxValue);
    }

    /// <summary>Returns #RRGGBBAA string (8 hex digits).</summary>
    [Pure]
    public string ToHex() => "#" + this.Rgba.ToString("X8");

    [Pure]
    public string ToHexNoHash() => this.Rgba.ToString("X8");

    public Rgb ToRgb() => new Rgb(this.R, this.G, this.B);

    public static implicit operator ColorRgba(int val) => new ColorRgba(val);

    public static implicit operator ColorRgba(uint val) => new ColorRgba(val);

    [Pure]
    public ColorRgba Lerp(ColorRgba color, Percent t)
    {
      return new ColorRgba(((int) this.R).Lerp((int) color.R, t), ((int) this.G).Lerp((int) color.G, t), ((int) this.B).Lerp((int) color.B, t), ((int) this.A).Lerp((int) color.A, t));
    }

    /// <summary>
    /// Converts RGB color to HSL (hue, saturation, lightness).
    /// </summary>
    public void ToHsl(out Fix32 h, out Fix32 s, out Fix32 l)
    {
      Fix32 fix32_1 = ((int) byte.MaxValue).ToFix32();
      Fix32 fix32_2 = (int) this.R / fix32_1;
      Fix32 other1 = (int) this.G / fix32_1;
      Fix32 other2 = (int) this.B / fix32_1;
      Fix32 fix32_3 = fix32_2.Max(other1);
      fix32_3 = fix32_3.Max(other2);
      Fix32 fix32_4 = fix32_2.Min(other1);
      fix32_4 = fix32_4.Min(other2);
      l = (fix32_4 + fix32_3) / 2;
      if (l.IsNotPositive)
      {
        h = (Fix32) 0;
        s = (Fix32) 0;
      }
      else
      {
        Fix32 fix32_5 = fix32_3 - fix32_4;
        s = fix32_5;
        if (s.IsPositive)
        {
          s /= l <= Fix32.Half ? fix32_3 + fix32_4 : Fix32.Two - fix32_3 - fix32_4;
          Fix32 fix32_6 = (fix32_3 - fix32_2) / fix32_5;
          Fix32 fix32_7 = (fix32_3 - other1) / fix32_5;
          Fix32 fix32_8 = (fix32_3 - other2) / fix32_5;
          h = !(fix32_2 == fix32_3) ? (!(other1 == fix32_3) ? (fix32_2 == fix32_4 ? (Fix32) 3 + fix32_7 : (Fix32) 5 - fix32_6) : (other2 == fix32_4 ? Fix32.One + fix32_6 : (Fix32) 3 - fix32_8)) : (other1 == fix32_4 ? (Fix32) 5 + fix32_8 : Fix32.One - fix32_7);
          h /= 6;
        }
        else
          h = (Fix32) 0;
      }
    }

    /// <summary>Converts HSL (hue, saturation, lightness) to color.</summary>
    public static ColorRgba FromHsl(Fix32 h, Fix32 s, Fix32 l)
    {
      Fix32 fix32_1 = l;
      Fix32 fix32_2 = l;
      Fix32 fix32_3 = l;
      Fix32 fix32_4 = l <= Fix32.Half ? l * (Fix32.One + s) : l + s - l * s;
      if (fix32_4 > 0)
      {
        Fix32 fix32_5 = l + l - fix32_4;
        Fix32 fix32_6 = (fix32_4 - fix32_5) / fix32_4;
        h *= 6;
        int intFloored = h.ToIntFloored();
        Fix32 fix32_7 = h - (Fix32) intFloored;
        Fix32 fix32_8 = fix32_4 * fix32_6 * fix32_7;
        Fix32 fix32_9 = fix32_5 + fix32_8;
        Fix32 fix32_10 = fix32_4 - fix32_8;
        switch (intFloored)
        {
          case 0:
          case 6:
            fix32_1 = fix32_4;
            fix32_2 = fix32_9;
            fix32_3 = fix32_5;
            break;
          case 1:
            fix32_1 = fix32_10;
            fix32_2 = fix32_4;
            fix32_3 = fix32_5;
            break;
          case 2:
            fix32_1 = fix32_5;
            fix32_2 = fix32_4;
            fix32_3 = fix32_9;
            break;
          case 3:
            fix32_1 = fix32_5;
            fix32_2 = fix32_10;
            fix32_3 = fix32_4;
            break;
          case 4:
            fix32_1 = fix32_9;
            fix32_2 = fix32_5;
            fix32_3 = fix32_4;
            break;
          case 5:
            fix32_1 = fix32_4;
            fix32_2 = fix32_5;
            fix32_3 = fix32_10;
            break;
        }
      }
      Fix32 fix32_11 = ((int) byte.MaxValue).ToFix32();
      return new ColorRgba((byte) (fix32_1 * fix32_11).ToIntRoundedNonNegative(), (byte) (fix32_2 * fix32_11).ToIntRoundedNonNegative(), (byte) (fix32_3 * fix32_11).ToIntRoundedNonNegative(), byte.MaxValue);
    }

    public override string ToString()
    {
      return string.Format("({0}, {1}, {2}, {3})", (object) this.R, (object) this.G, (object) this.B, (object) this.A);
    }

    public bool Equals(ColorRgba other) => (int) this.Rgba == (int) other.Rgba;

    public override bool Equals(object obj) => obj is ColorRgba other && this.Equals(other);

    public override int GetHashCode() => (int) this.Rgba;

    public int CompareTo(ColorRgba other) => this.Rgba.CompareTo(other.Rgba);

    public static bool operator ==(ColorRgba lhs, ColorRgba rhs)
    {
      return (int) lhs.Rgba == (int) rhs.Rgba;
    }

    public static bool operator !=(ColorRgba lhs, ColorRgba rhs)
    {
      return (int) lhs.Rgba != (int) rhs.Rgba;
    }

    public static void Serialize(ColorRgba value, BlobWriter writer)
    {
      writer.WriteUInt(value.Rgba);
    }

    public static ColorRgba Deserialize(BlobReader reader) => new ColorRgba(reader.ReadUInt());

    static ColorRgba()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      ColorRgba.Black = new ColorRgba((uint) byte.MaxValue);
      ColorRgba.DarkDarkGray = new ColorRgba(538976511U);
      ColorRgba.DarkGray = new ColorRgba(1077952767U);
      ColorRgba.Gray = new ColorRgba(2155905279U);
      ColorRgba.LightGray = new ColorRgba(3233857791U);
      ColorRgba.White = new ColorRgba(uint.MaxValue);
      ColorRgba.Red = new ColorRgba(4278190335U);
      ColorRgba.DarkRed = new ColorRgba(3137339579U);
      ColorRgba.Brown = new ColorRgba(2336560127U);
      ColorRgba.Orange = new ColorRgba(4287375615U);
      ColorRgba.Gold = new ColorRgba(4290707711U);
      ColorRgba.Yellow = new ColorRgba(4294902015U);
      ColorRgba.LightYellow = new ColorRgba(4294959359U);
      ColorRgba.DarkYellow = new ColorRgba(2575835903U);
      ColorRgba.GreenYellow = new ColorRgba(3422498559U);
      ColorRgba.Green = new ColorRgba(16711935U);
      ColorRgba.DarkGreen = new ColorRgba(8388863U);
      ColorRgba.Turquoise = new ColorRgba(1088475391U);
      ColorRgba.Cyan = new ColorRgba(16777215U);
      ColorRgba.Blue = new ColorRgba((uint) ushort.MaxValue);
      ColorRgba.LightBlue = new ColorRgba(2278488831U);
      ColorRgba.CornflowerBlue = new ColorRgba(1687547391U);
      ColorRgba.Magenta = new ColorRgba(4278255615U);
      ColorRgba.Purple = new ColorRgba(2281736447U);
    }
  }
}
