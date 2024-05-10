// Decompiled with JetBrains decompiler
// Type: Mafi.MustUseReturnValueAttribute
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
  /// Indicates that the return value of the method invocation must be used.
  /// </summary>
  /// <remarks>
  /// Methods decorated with this attribute (in contrast to pure methods) might change state,
  /// but make no sense without using their return value. <br />
  /// Similarly to <see cref="T:Mafi.PureAttribute" />, this attribute
  /// will help detecting usages of the method when the return value in not used.
  /// Additionally, you can optionally specify a custom message, which will be used when showing warnings, e.g.
  /// <code>[MustUseReturnValue("Use the return value to...")]</code>.
  /// </remarks>
  [AttributeUsage(AttributeTargets.Method)]
  public sealed class MustUseReturnValueAttribute : Attribute
  {
    public MustUseReturnValueAttribute()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public MustUseReturnValueAttribute([NotNull] string justification)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Justification = justification;
    }

    [CanBeNull]
    public string Justification { get; }
  }
}
