// Decompiled with JetBrains decompiler
// Type: Mafi.PublicAPIAttribute
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
  /// This attribute is intended to mark publicly available API
  /// which should not be removed and so is treated as used.
  /// </summary>
  [AttributeUsage(AttributeTargets.All, Inherited = false)]
  [MeansImplicitUse(ImplicitUseTargetFlags.WithMembers)]
  public sealed class PublicAPIAttribute : Attribute
  {
    public PublicAPIAttribute()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public PublicAPIAttribute([NotNull] string comment)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Comment = comment;
    }

    [CanBeNull]
    public string Comment { get; }
  }
}
