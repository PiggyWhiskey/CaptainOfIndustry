// Decompiled with JetBrains decompiler
// Type: Mafi.NotGlobalDependencyAttribute
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
  /// Marks an interface or class to not be global dependency. This is handy when class is registered under all
  /// interfaces but some interfaces are not intended to be dependencies. For classes this is only useful for classes
  /// that are for some reason automatically registered (classes implementing serialization interfaces).
  /// </summary>
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
  public sealed class NotGlobalDependencyAttribute : Attribute
  {
    public NotGlobalDependencyAttribute()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
