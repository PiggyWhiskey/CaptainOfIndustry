// Decompiled with JetBrains decompiler
// Type: Mafi.Numerics.Plane
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Numerics
{
  /// <summary>
  /// 3D plane represented as normal and distance from the origin.
  /// </summary>
  public readonly struct Plane
  {
    public static readonly Plane YzPlane;
    public static readonly Plane XyPlane;
    public static readonly Plane XzPlane;
    public readonly Vector3f Normal;
    public readonly Fix32 DistanceFromOrigin;

    public Plane(Vector3f normal, Fix32 distanceFromOrigin)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.Normal = normal;
      this.DistanceFromOrigin = distanceFromOrigin;
    }

    static Plane()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      Plane.YzPlane = new Plane(Vector3f.UnitX, Fix32.Zero);
      Plane.XyPlane = new Plane(Vector3f.UnitZ, Fix32.Zero);
      Plane.XzPlane = new Plane(Vector3f.UnitY, Fix32.Zero);
    }
  }
}
