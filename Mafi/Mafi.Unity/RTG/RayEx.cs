// Decompiled with JetBrains decompiler
// Type: RTG.RayEx
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;

#nullable disable
namespace RTG
{
  public static class RayEx
  {
    public static Ray InverseTransform(this Ray ray, Matrix4x4 transformMatrix)
    {
      Matrix4x4 inverse = transformMatrix.inverse;
      return new Ray(inverse.MultiplyPoint(ray.origin), inverse.MultiplyVector(ray.direction).normalized);
    }

    public static Ray Mirror(this Ray ray, Vector3 mirrorPoint)
    {
      return ray with
      {
        origin = mirrorPoint + ray.direction * (ray.origin - mirrorPoint).magnitude,
        direction = -ray.direction
      };
    }
  }
}
