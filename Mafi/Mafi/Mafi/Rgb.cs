// Decompiled with JetBrains decompiler
// Type: Mafi.Rgb
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi
{
  public struct Rgb
  {
    public readonly byte R;
    public readonly byte G;
    public readonly byte B;

    public Rgb(byte gray)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.R = gray;
      this.G = gray;
      this.B = gray;
    }

    public Rgb(byte r, byte g, byte b)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.R = r;
      this.G = g;
      this.B = b;
    }

    public Rgb BlendWith(ColorRgba color)
    {
      return new Rgb((byte) (((int) this.R * ((int) byte.MaxValue - (int) color.A) + (int) color.R * (int) color.A) / (int) byte.MaxValue), (byte) (((int) this.G * ((int) byte.MaxValue - (int) color.A) + (int) color.G * (int) color.A) / (int) byte.MaxValue), (byte) (((int) this.B * ((int) byte.MaxValue - (int) color.A) + (int) color.B * (int) color.A) / (int) byte.MaxValue));
    }
  }
}
