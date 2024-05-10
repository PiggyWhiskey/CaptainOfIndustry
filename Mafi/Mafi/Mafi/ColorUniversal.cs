// Decompiled with JetBrains decompiler
// Type: Mafi.ColorUniversal
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi
{
  /// <summary>
  /// Universal color representation that has both compact byte-based representation as well as float based one.
  /// Use this to avoid conversions to floats which are expensive due to divisions.
  /// </summary>
  public readonly struct ColorUniversal
  {
    public readonly ColorRgba Rgba;
    public readonly float R;
    public readonly float G;
    public readonly float B;
    public readonly float A;
    public readonly Fix32 H;
    public readonly Fix32 S;
    public readonly Fix32 L;

    public ColorUniversal(ColorRgba rgba)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.Rgba = rgba;
      this.R = (float) rgba.R * 0.003921569f;
      this.G = (float) rgba.G * 0.003921569f;
      this.B = (float) rgba.B * 0.003921569f;
      this.A = (float) rgba.A * 0.003921569f;
      rgba.ToHsl(out this.H, out this.S, out this.L);
    }

    public static implicit operator ColorUniversal(ColorRgba val) => new ColorUniversal(val);
  }
}
