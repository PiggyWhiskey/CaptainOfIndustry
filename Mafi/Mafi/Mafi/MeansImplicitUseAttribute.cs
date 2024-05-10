// Decompiled with JetBrains decompiler
// Type: Mafi.MeansImplicitUseAttribute
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
  /// Can be applied to attributes, type parameters, and parameters of a type assignable from <see cref="T:System.Type" /> .
  /// When applied to an attribute, the decorated attribute behaves the same as <see cref="T:Mafi.UsedImplicitlyAttribute" />.
  /// When applied to a type parameter or to a parameter of type <see cref="T:System.Type" />,  indicates that the corresponding type
  /// is used implicitly.
  /// </summary>
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter | AttributeTargets.GenericParameter)]
  public sealed class MeansImplicitUseAttribute : Attribute
  {
    public MeansImplicitUseAttribute()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      this.\u002Ector(ImplicitUseKindFlags.Default, ImplicitUseTargetFlags.Default);
    }

    public MeansImplicitUseAttribute(ImplicitUseKindFlags useKindFlags)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      this.\u002Ector(useKindFlags, ImplicitUseTargetFlags.Default);
    }

    public MeansImplicitUseAttribute(ImplicitUseTargetFlags targetFlags)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      this.\u002Ector(ImplicitUseKindFlags.Default, targetFlags);
    }

    public MeansImplicitUseAttribute(
      ImplicitUseKindFlags useKindFlags,
      ImplicitUseTargetFlags targetFlags)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.UseKindFlags = useKindFlags;
      this.TargetFlags = targetFlags;
    }

    [UsedImplicitly]
    public ImplicitUseKindFlags UseKindFlags { get; }

    [UsedImplicitly]
    public ImplicitUseTargetFlags TargetFlags { get; }
  }
}
