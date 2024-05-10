﻿// Decompiled with JetBrains decompiler
// Type: Mafi.Random.Noise.ExpBendNoise2DExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

#nullable disable
namespace Mafi.Random.Noise
{
  public static class ExpBendNoise2DExtensions
  {
    public static INoise2D BendExp(this INoise2D noise, ExpBendNoiseParams bendParams)
    {
      return !bendParams.Amount.IsPositive ? noise : (INoise2D) new ExpBendNoise2D(noise, bendParams.Amount.ToFix64() / 10, bendParams.Bias.ToFix64());
    }
  }
}
