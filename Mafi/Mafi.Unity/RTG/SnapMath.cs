// Decompiled with JetBrains decompiler
// Type: RTG.SnapMath
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;

#nullable disable
namespace RTG
{
  public static class SnapMath
  {
    public static NumSnapSteps CalculateNumSnapSteps(float snapStep, float total)
    {
      NumSnapSteps numSnapSteps = new NumSnapSteps()
      {
        FltNumSteps = total / snapStep
      };
      numSnapSteps.AbsFltNumSteps = Mathf.Abs(numSnapSteps.FltNumSteps);
      numSnapSteps.IntNumSteps = (int) numSnapSteps.FltNumSteps;
      numSnapSteps.AbsIntNumSteps = Mathf.Abs(numSnapSteps.IntNumSteps);
      numSnapSteps.AbsFracSteps = numSnapSteps.AbsFltNumSteps - (float) numSnapSteps.AbsIntNumSteps;
      return numSnapSteps;
    }

    public static bool CanExtractSnap(float snapStep, float accumulated)
    {
      return (double) Mathf.Abs(accumulated) >= (double) snapStep;
    }

    public static float ExtractSnap(float snapStep, ref float accumulated)
    {
      float snap = (float) (int) ((double) accumulated / (double) snapStep) * snapStep;
      accumulated -= snap;
      return snap;
    }

    public static float ExtractSnap(float snapStep, float accumulated)
    {
      return (float) (int) ((double) accumulated / (double) snapStep) * snapStep;
    }
  }
}
