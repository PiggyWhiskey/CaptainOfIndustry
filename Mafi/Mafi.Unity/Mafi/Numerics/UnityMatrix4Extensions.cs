// Decompiled with JetBrains decompiler
// Type: Mafi.Numerics.UnityMatrix4Extensions
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;

#nullable disable
namespace Mafi.Numerics
{
  public static class UnityMatrix4Extensions
  {
    public static Matrix4x4 ToUnityMatrix(this UnityMatrix4 m)
    {
      return new Matrix4x4(new Vector4(m.M00, m.M10, m.M20, m.M30), new Vector4(m.M01, m.M11, m.M21, m.M31), new Vector4(m.M02, m.M12, m.M22, m.M32), new Vector4(m.M03, m.M13, m.M23, m.M33));
    }

    public static UnityMatrix4 ToMafiMatrix(this Matrix4x4 m)
    {
      return new UnityMatrix4(m.m00, m.m10, m.m20, m.m30, m.m01, m.m11, m.m21, m.m31, m.m02, m.m12, m.m22, m.m32, m.m03, m.m13, m.m23, m.m33);
    }

    public static Pose ToUnityCameraPose(this UnityCameraPose p, out float verticalFieldOfView)
    {
      verticalFieldOfView = p.VerticalFieldOfView;
      return new Pose(new Vector3(p.PositionX, p.PositionY, p.PositionZ), new Quaternion(p.RotationX, p.RotationY, p.RotationZ, p.RotationW));
    }

    public static UnityCameraPose ToMafiCameraPose(this Pose p, float verticalFieldOfView)
    {
      return new UnityCameraPose(p.position.x, p.position.y, p.position.z, p.rotation.x, p.rotation.y, p.rotation.z, p.rotation.w, verticalFieldOfView);
    }
  }
}
