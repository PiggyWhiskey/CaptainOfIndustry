﻿// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.SerializeNullAsEmptyArrayAttribute
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
  /// Marks array that should be serialized as empty array when null.
  /// This is handy on structs that may not be initialized by default BUT empty array must have
  /// the same semantic meaning as null.
  /// </summary>
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
  public sealed class SerializeNullAsEmptyArrayAttribute : Attribute
  {
    public SerializeNullAsEmptyArrayAttribute()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
