// Decompiled with JetBrains decompiler
// Type: Mafi.Aabb
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi
{
  /// <summary>Axis-aligned bounding box.</summary>
  public struct Aabb
  {
    public Vector3f Min;
    public Vector3f Max;

    public static Aabb FromCenterSize(Vector3f center, Vector3f size)
    {
      Vector3f halfFast = size.AbsValue.HalfFast;
      return new Aabb(center - halfFast, center + halfFast);
    }

    public Aabb(Vector3f min, Vector3f max)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      Assert.That<bool>(min <= max).IsTrue();
      this.Min = min;
      this.Max = max;
    }

    public bool IsValid => this.Min <= this.Max;

    public Vector3f Size => this.Max - this.Min;

    [Pure]
    public bool Contains(Vector3f v) => v >= this.Min && v <= this.Max;

    public bool IntersectsWith(Aabb bounds)
    {
      return this.Min.X <= bounds.Max.X && this.Min.Y <= bounds.Max.Y && this.Min.Z <= bounds.Max.Z && this.Max.X >= bounds.Min.X && this.Max.Y >= bounds.Min.Y && this.Max.Z >= bounds.Min.Z;
    }
  }
}
