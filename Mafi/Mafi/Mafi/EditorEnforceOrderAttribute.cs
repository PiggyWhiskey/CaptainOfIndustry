﻿// Decompiled with JetBrains decompiler
// Type: Mafi.EditorEnforceOrderAttribute
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;
using System.Runtime.CompilerServices;

#nullable disable
namespace Mafi
{
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
  public sealed class EditorEnforceOrderAttribute : Attribute
  {
    public readonly int Order;

    public EditorEnforceOrderAttribute([CallerLineNumber] int order = 0)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Order = order;
    }
  }
}
