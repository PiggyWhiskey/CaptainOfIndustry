// Decompiled with JetBrains decompiler
// Type: Mafi.Fix64Extensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

#nullable disable
namespace Mafi
{
  public static class Fix64Extensions
  {
    public static Fix64 NextFix64Between01(this IRandom random)
    {
      return Fix64.FromRaw(random.NextLong(0L, 1048576L));
    }

    public static Fix64 NextFix64(this IRandom random, Fix64 minValueIncl, Fix64 maxValueExcl)
    {
      return Fix64.FromRaw(random.NextLong(minValueIncl.RawValue, maxValueExcl.RawValue));
    }
  }
}
