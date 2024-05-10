// Decompiled with JetBrains decompiler
// Type: Mafi.AngleDegrees1fExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System.Diagnostics;

#nullable disable
namespace Mafi
{
  [DebuggerStepThrough]
  public static class AngleDegrees1fExtensions
  {
    public static AngleDegrees1f Degrees(this int value) => AngleDegrees1f.FromDegrees(value);

    public static AngleDegrees1f Degrees(this Fix32 value) => AngleDegrees1f.FromDegrees(value);

    public static AngleDegrees1f Degrees(this float value)
    {
      return AngleDegrees1f.FromDegrees(value.ToFix32());
    }

    public static AngleDegrees1f Degrees(this double value)
    {
      return AngleDegrees1f.FromDegrees(value.ToFix32());
    }

    public static AngleDegrees1f Radians(this float value)
    {
      return AngleDegrees1f.FromRadians(value.ToFix32());
    }

    public static AngleDegrees1f NextAngle(this IRandom random)
    {
      return AngleDegrees1f.FromDegrees(random.NextFix32(Fix32.Zero, Fix32.FromInt(360)));
    }

    public static AngleDegrees1f NextAngle(
      this IRandom random,
      AngleDegrees1f min,
      AngleDegrees1f max)
    {
      return AngleDegrees1f.FromDegrees(random.NextFix32(min.Degrees, max.Degrees));
    }

    public static AngleSlim NextAngleSlim(this IRandom random)
    {
      return AngleSlim.FromRaw(random.NextUShort());
    }
  }
}
