// Decompiled with JetBrains decompiler
// Type: Mafi.Assertion`1
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;
using System.Diagnostics;

#nullable disable
namespace Mafi
{
  [DebuggerDisplay("{Value}")]
  public readonly struct Assertion<T>
  {
    public readonly T Value;

    internal Assertion(T value)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.Value = value;
    }

    public override bool Equals(object obj)
    {
      throw new InvalidOperationException("Did you mean to call '.IsEqualTo' instead of '.Equals'?");
    }
  }
}
