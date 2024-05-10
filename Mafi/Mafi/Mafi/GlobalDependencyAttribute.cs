// Decompiled with JetBrains decompiler
// Type: Mafi.GlobalDependencyAttribute
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
  /// Marks a class as a global dependency that will be automatically registered to dependency resolver based on given
  /// parameters. Marked class does not have to be public - this might be useful for registering internal class that is
  /// visible to public only under public interfaces or that is usable only within its assembly.
  /// 
  /// The user of this attribute has no control over the registration process and order is undefined. Thus, this
  /// attribute should be used for dependencies that will always be registered and order is insignificant. Otherwise
  /// please use manual registration process that can be controlled by ifs, cycles, and order is guaranteed.
  /// 
  /// All types marked by this interface are registered BEFORE manual registration is invoked.
  /// </summary>
  [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
  public sealed class GlobalDependencyAttribute : Attribute
  {
    /// <summary>
    /// Dependency registration modes specify how to register marked class. Note that individual flags can be
    /// combined.
    /// </summary>
    public readonly RegistrationMode RegistrationMode;
    /// <summary>
    /// Whether this dependency is only registered in debug mode.
    /// </summary>
    public readonly bool OnlyInDebug;
    /// <summary>
    /// Whether this dependency is only registered in debug mode.
    /// </summary>
    public readonly bool OnlyInDevOnly;

    public GlobalDependencyAttribute(
      RegistrationMode registrationMode,
      bool onlyInDebug = false,
      bool onlyInDevOnly = false)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.OnlyInDebug = onlyInDebug;
      this.RegistrationMode = registrationMode;
      this.OnlyInDevOnly = onlyInDevOnly;
    }
  }
}
