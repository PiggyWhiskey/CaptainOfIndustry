// Decompiled with JetBrains decompiler
// Type: RTG.AxisHelper
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;

#nullable disable
namespace RTG
{
  public static class AxisHelper
  {
    public static Vector3 GetWorldAxis(Axis axis)
    {
      if (axis == Axis.X)
        return Vector3.right;
      return axis == Axis.Y ? Vector3.up : Vector3.forward;
    }

    public static Vector3 GetWorldAxis(Axis axis, AxisSign axisSign)
    {
      switch (axis)
      {
        case Axis.X:
          return axisSign != AxisSign.Positive ? -Vector3.right : Vector3.right;
        case Axis.Y:
          return axisSign != AxisSign.Positive ? -Vector3.up : Vector3.up;
        default:
          return axisSign != AxisSign.Positive ? -Vector3.forward : Vector3.forward;
      }
    }
  }
}
