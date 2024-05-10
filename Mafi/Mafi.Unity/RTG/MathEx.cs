// Decompiled with JetBrains decompiler
// Type: RTG.MathEx
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;

#nullable disable
namespace RTG
{
  public static class MathEx
  {
    public static bool AlmostEqual(float v1, float v2, float epsilon)
    {
      return (double) Mathf.Abs(v1 - v2) < (double) epsilon;
    }

    public static int GetNumDigits(int number)
    {
      return number != 0 ? Mathf.FloorToInt(Mathf.Log10((float) Mathf.Abs(number)) + 1f) : 1;
    }

    public static float SafeAcos(float cosine)
    {
      cosine = Mathf.Max(-1f, Mathf.Min(1f, cosine));
      return Mathf.Acos(cosine);
    }

    public static bool SolveQuadratic(float a, float b, float c, out float t1, out float t2)
    {
      t1 = t2 = 0.0f;
      float f = (float) ((double) b * (double) b - 4.0 * (double) a * (double) c);
      if ((double) f < 0.0)
        return false;
      float num1 = 2f * a;
      if ((double) num1 == 0.0)
        return false;
      if ((double) f == 0.0)
      {
        t1 = t2 = -b / num1;
        return true;
      }
      float num2 = Mathf.Sqrt(f);
      t1 = (-b + num2) / num1;
      t2 = (-b - num2) / num1;
      if ((double) t1 > (double) t2)
      {
        float num3 = t1;
        t1 = t2;
        t2 = num3;
      }
      return true;
    }
  }
}
