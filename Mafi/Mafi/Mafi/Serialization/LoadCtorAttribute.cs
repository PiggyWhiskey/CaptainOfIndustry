// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.LoadCtorAttribute
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
  /// Attribute to mark class constructor to be used to create its instances on game load.
  /// Used for structs in <see cref="T:Mafi.Serialization.GenerateSerializer" />
  /// </summary>
  [AttributeUsage(AttributeTargets.Constructor)]
  public sealed class LoadCtorAttribute : Attribute
  {
    public LoadCtorAttribute()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
