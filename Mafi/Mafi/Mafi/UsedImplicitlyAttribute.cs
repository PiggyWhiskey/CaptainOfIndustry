// Decompiled with JetBrains decompiler
// Type: Mafi.UsedImplicitlyAttribute
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi
{
  /// <summary>
  /// Indicates that the marked symbol is used implicitly (e.g. via reflection, in external library),
  /// so this symbol will not be reported as unused (as well as by other usage inspections).
  /// </summary>
  [AttributeUsage(AttributeTargets.All, Inherited = false)]
  public sealed class UsedImplicitlyAttribute : Attribute
  {
    public UsedImplicitlyAttribute()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      this.\u002Ector(ImplicitUseKindFlags.Default, ImplicitUseTargetFlags.Default);
    }

    public UsedImplicitlyAttribute(ImplicitUseKindFlags useKindFlags)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      this.\u002Ector(useKindFlags, ImplicitUseTargetFlags.Default);
    }

    public UsedImplicitlyAttribute(ImplicitUseTargetFlags targetFlags)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      this.\u002Ector(ImplicitUseKindFlags.Default, targetFlags);
    }

    public UsedImplicitlyAttribute(
      ImplicitUseKindFlags useKindFlags,
      ImplicitUseTargetFlags targetFlags)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.UseKindFlags = useKindFlags;
      this.TargetFlags = targetFlags;
    }

    public ImplicitUseKindFlags UseKindFlags { get; }

    public ImplicitUseTargetFlags TargetFlags { get; }
  }
}
