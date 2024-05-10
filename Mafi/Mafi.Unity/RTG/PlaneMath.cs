// Decompiled with JetBrains decompiler
// Type: RTG.PlaneMath
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;

#nullable disable
namespace RTG
{
  public static class PlaneMath
  {
    public static bool Raycast2D(
      Vector2 rayOrigin,
      Vector2 rayDir,
      Vector2 planeNormal,
      Vector2 ptOnPlane,
      out float t)
    {
      t = 0.0f;
      float f = Vector2.Dot(rayDir, planeNormal);
      if ((double) Mathf.Abs(f) < 9.9999997473787516E-06)
        return false;
      float num = Vector2.Dot(rayOrigin - ptOnPlane, planeNormal);
      t = num / -f;
      return (double) t >= 0.0;
    }
  }
}
