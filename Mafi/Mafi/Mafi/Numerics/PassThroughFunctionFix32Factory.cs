﻿// Decompiled with JetBrains decompiler
// Type: Mafi.Numerics.PassThroughFunctionFix32Factory
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Numerics
{
  public class PassThroughFunctionFix32Factory : IFunctionFix32Factory
  {
    public readonly IFunctionFix32 Value;

    public PassThroughFunctionFix32Factory(IFunctionFix32 value)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Value = value;
    }

    public IFunctionFix32 Create() => this.Value;
  }
}
