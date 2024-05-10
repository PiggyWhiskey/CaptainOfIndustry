﻿// Decompiled with JetBrains decompiler
// Type: Mafi.AspMvcAreaAttribute
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
  /// ASP.NET MVC attribute. Indicates that the marked parameter is an MVC area.
  /// Use this attribute for custom wrappers similar to
  /// <c>System.Web.Mvc.Html.ChildActionExtensions.RenderAction(HtmlHelper, String)</c>.
  /// </summary>
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
  public sealed class AspMvcAreaAttribute : Attribute
  {
    public AspMvcAreaAttribute()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public AspMvcAreaAttribute([NotNull] string anonymousProperty)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.AnonymousProperty = anonymousProperty;
    }

    [CanBeNull]
    public string AnonymousProperty { get; }
  }
}
