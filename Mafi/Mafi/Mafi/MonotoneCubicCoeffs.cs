// Decompiled with JetBrains decompiler
// Type: Mafi.MonotoneCubicCoeffs
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi
{
  public readonly struct MonotoneCubicCoeffs
  {
    public readonly Fix32 V1;
    public readonly Fix64 C1;
    public readonly Fix64 C2;
    public readonly Fix64 C3;

    public MonotoneCubicCoeffs(Fix32 v1, Fix64 c1, Fix64 c2, Fix64 c3)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.V1 = v1;
      this.C1 = c1;
      this.C2 = c2;
      this.C3 = c3;
    }

    public Fix32 Interpolate(Percent t)
    {
      return this.V1 + t.ApplyFast(this.C1 + t.ApplyFast(this.C2 + t.ApplyFast(this.C3))).ToFix32();
    }
  }
}
