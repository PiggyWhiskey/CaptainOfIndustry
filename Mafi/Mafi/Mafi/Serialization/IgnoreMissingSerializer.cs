// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.IgnoreMissingSerializer
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
  /// When serializer generator finds a class marked by this attribute it will not throw any errors
  /// in case the class implements an interface that is being saved but such class has no serializer.
  /// Normally this should not happen and if you need to use this you are asking for problem.
  /// </summary>
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
  public sealed class IgnoreMissingSerializer : Attribute
  {
    public IgnoreMissingSerializer()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
