// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.InitAfterLoadAttribute
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi.Serialization
{
  /// <summary>
  /// Marked method will be called during or immediately after deserialization.
  /// 
  /// Note: Marked method should be private. It is guaranteed that methods defined on base class are called first
  /// (same ordering as constructors).
  /// </summary>
  [AttributeUsage(AttributeTargets.Method)]
  public sealed class InitAfterLoadAttribute : Attribute
  {
    public readonly InitPriority Priority;

    public InitAfterLoadAttribute(InitPriority priority = InitPriority.Normal)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Priority = priority;
    }
  }
}
