// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.OnlyForSaveCompatibilityAttribute
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
  /// Marks anything to be only for save compability. When we are upgrading save in backwards incompatible ways,
  /// we can search for this and cleanup the code.
  /// </summary>
  [AttributeUsage(AttributeTargets.All)]
  public sealed class OnlyForSaveCompatibilityAttribute : Attribute
  {
    public OnlyForSaveCompatibilityAttribute(string message = null)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
