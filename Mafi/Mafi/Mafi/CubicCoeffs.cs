// Decompiled with JetBrains decompiler
// Type: Mafi.CubicCoeffs
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi
{
  public readonly struct CubicCoeffs
  {
    public readonly Fix32 A;
    public readonly Fix32 B;
    public readonly Fix32 V1;
    public readonly Fix32 V20;

    public CubicCoeffs(Fix32 a, Fix32 b, Fix32 v1, Fix32 v20)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.A = a;
      this.B = b;
      this.V1 = v1;
      this.V20 = v20;
    }

    public Fix32 Interpolate(Percent t)
    {
      return this.V1 + Fix32.Half * t.ApplyFast(this.V20 + t.ApplyFast(this.A + t.ApplyFast(this.B)));
    }
  }
}
