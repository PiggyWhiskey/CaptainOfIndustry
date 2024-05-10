// Decompiled with JetBrains decompiler
// Type: Mafi.RazorPageBaseTypeAttribute
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi
{
  [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
  public sealed class RazorPageBaseTypeAttribute : Attribute
  {
    public RazorPageBaseTypeAttribute([NotNull] string baseType)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.BaseType = baseType;
    }

    public RazorPageBaseTypeAttribute([NotNull] string baseType, string pageName)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.BaseType = baseType;
      this.PageName = pageName;
    }

    [NotNull]
    public string BaseType { get; }

    [CanBeNull]
    public string PageName { get; }
  }
}
