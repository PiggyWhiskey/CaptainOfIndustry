// Decompiled with JetBrains decompiler
// Type: Mafi.EditorRangePercentAttribute
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi
{
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
  public sealed class EditorRangePercentAttribute : EditorRangeAttribute
  {
    public EditorRangePercentAttribute(int minPercent, int maxPercent)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      Percent percent = Percent.FromPercentVal(minPercent);
      double min = percent.ToDouble();
      percent = Percent.FromPercentVal(maxPercent);
      double max = percent.ToDouble();
      // ISSUE: explicit constructor call
      base.\u002Ector(min, max);
    }
  }
}
