// Decompiled with JetBrains decompiler
// Type: Mafi.MultiDependencyAttribute
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
  /// Marks an interface to be multi-dependency. Resolver will keep all instances instead of only choosing one.
  /// Dependencies can be resolved with <see cref="T:Mafi.AllImplementationsOf`1" />.
  /// </summary>
  [AttributeUsage(AttributeTargets.Interface)]
  public sealed class MultiDependencyAttribute : Attribute
  {
    public MultiDependencyAttribute()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
