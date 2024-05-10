// Decompiled with JetBrains decompiler
// Type: Mafi.Vornoi.PoissonDiskPointSampler
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using System;

#nullable disable
namespace Mafi.Vornoi
{
  public class PoissonDiskPointSampler
  {
    private readonly IRandom m_random;

    public PoissonDiskPointSampler(IRandom random)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_random = random.CheckNotNull<IRandom>();
    }

    public Lyst<Vector2i> GeneratePoints(
      int maxCount,
      Vector2i initialPt,
      int initialMinDistance,
      int initialMaxDistance,
      int expansionTrials,
      Func<Vector2i, Vector2i> getMinMaxDistanceFn)
    {
      Assert.That<int>(maxCount).IsPositive();
      Assert.That<int>(initialMinDistance).IsPositive();
      Assert.That<int>(initialMaxDistance).IsGreaterOrEqual(initialMinDistance);
      Lyst<Vector2i> resultPts = new Lyst<Vector2i>()
      {
        initialPt
      };
      Lyst<Vector2i> minMaxDists = new Lyst<Vector2i>()
      {
        new Vector2i(initialMinDistance, initialMaxDistance)
      };
      Lyst<int> lyst = new Lyst<int>() { 0 };
label_6:
      while (lyst.IsNotEmpty && resultPts.Count < maxCount)
      {
        Assert.That<int>(minMaxDists.Count).IsEqualTo(resultPts.Count);
        int index1 = this.m_random.NextInt(0, lyst.Count);
        int index2 = lyst[index1];
        lyst.RemoveAtReplaceWithLast(index1);
        Vector2i vector2i1 = resultPts[index2];
        Vector2i vector2i2 = minMaxDists[index2];
        int num = 0;
        while (true)
        {
          if (num < expansionTrials && resultPts.Count < maxCount)
          {
            Vector2i roundedVector2i = this.m_random.SampleCircleCenterBiased(vector2i1.Vector2f, (Fix32) vector2i2.X, (Fix32) vector2i2.Y).RoundedVector2i;
            Vector2i vector2i3 = getMinMaxDistanceFn(roundedVector2i);
            Assert.That<int>(vector2i3.X).IsNotNegative();
            Assert.That<int>(vector2i3.Y).IsGreaterOrEqual(vector2i3.X);
            if (!vector2i3.IsZero && !PoissonDiskPointSampler.collidesWithSomePoint(roundedVector2i, vector2i3.X, resultPts, minMaxDists))
            {
              lyst.Add(resultPts.Count);
              resultPts.Add(roundedVector2i);
              minMaxDists.Add(vector2i3);
            }
            ++num;
          }
          else
            goto label_6;
        }
      }
      Assert.That<bool>(resultPts.Count == maxCount || lyst.IsEmpty).IsTrue();
      return resultPts;
    }

    private static bool collidesWithSomePoint(
      Vector2i newPt,
      int minDistance,
      Lyst<Vector2i> resultPts,
      Lyst<Vector2i> minMaxDists)
    {
      Assert.That<int>(resultPts.Count).IsEqualTo(minMaxDists.Count);
      for (int index = 0; index < resultPts.Count; ++index)
      {
        long num = (long) minDistance.Max(minMaxDists[index].X);
        if (resultPts[index].DistanceSqrTo(newPt) < num * num)
          return true;
      }
      return false;
    }
  }
}
