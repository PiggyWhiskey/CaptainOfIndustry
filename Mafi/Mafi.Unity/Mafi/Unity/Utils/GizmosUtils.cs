// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Utils.GizmosUtils
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;

#nullable disable
namespace Mafi.Unity.Utils
{
  public static class GizmosUtils
  {
    public static void DrawBounds(Bounds bounds)
    {
      GizmosUtils.DrawBounds(bounds, Random.ColorHSV(0.0f, 1f, 0.3f, 0.7f));
    }

    public static void DrawBounds(Bounds bounds, Color clr)
    {
      Vector3 min = bounds.min;
      Vector3 max = bounds.max;
      Vector3 vector3_1 = new Vector3(min.x, min.y, min.z);
      Vector3 vector3_2 = new Vector3(max.x, min.y, min.z);
      Vector3 vector3_3 = new Vector3(max.x, min.y, max.z);
      Vector3 vector3_4 = new Vector3(min.x, min.y, max.z);
      Vector3 vector3_5 = new Vector3(min.x, max.y, min.z);
      Vector3 vector3_6 = new Vector3(max.x, max.y, min.z);
      Vector3 vector3_7 = new Vector3(max.x, max.y, max.z);
      Vector3 vector3_8 = new Vector3(min.x, max.y, max.z);
      Debug.DrawLine(vector3_1, vector3_2, clr);
      Debug.DrawLine(vector3_2, vector3_3, clr);
      Debug.DrawLine(vector3_3, vector3_4, clr);
      Debug.DrawLine(vector3_4, vector3_1, clr);
      Debug.DrawLine(vector3_5, vector3_6, clr);
      Debug.DrawLine(vector3_6, vector3_7, clr);
      Debug.DrawLine(vector3_7, vector3_8, clr);
      Debug.DrawLine(vector3_8, vector3_5, clr);
      Debug.DrawLine(vector3_1, vector3_5, clr);
      Debug.DrawLine(vector3_2, vector3_6, clr);
      Debug.DrawLine(vector3_3, vector3_7, clr);
      Debug.DrawLine(vector3_4, vector3_8, clr);
      Debug.DrawLine(vector3_1, vector3_3, clr);
      Debug.DrawLine(vector3_2, vector3_4, clr);
      Debug.DrawLine(vector3_5, vector3_7, clr);
      Debug.DrawLine(vector3_6, vector3_8, clr);
    }
  }
}
