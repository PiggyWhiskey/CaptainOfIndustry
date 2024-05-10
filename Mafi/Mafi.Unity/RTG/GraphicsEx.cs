// Decompiled with JetBrains decompiler
// Type: RTG.GraphicsEx
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace RTG
{
  public static class GraphicsEx
  {
    public static void DrawQuadBorder2D(
      Vector2 quadCenter,
      Vector2 quadSize,
      float rotationDegrees,
      Camera camera)
    {
      float z = camera.nearClipPlane + 0.01f;
      Vector3 worldPoint1 = camera.ScreenToWorldPoint(new Vector3(quadCenter.x, quadCenter.y, z));
      Vector3 worldPoint2 = camera.ScreenToWorldPoint(new Vector3(quadCenter.x - quadSize.x * 0.5f, quadCenter.y + quadSize.y * 0.5f, z));
      Vector3 worldPoint3 = camera.ScreenToWorldPoint(new Vector3(quadCenter.x + quadSize.x * 0.5f, quadCenter.y - quadSize.y * 0.5f, z));
      Transform transform = camera.transform;
      Vector3 one = Vector3.one with
      {
        x = Mathf.Abs((worldPoint3 - worldPoint2).Dot(transform.right)),
        y = Mathf.Abs((worldPoint3 - worldPoint2).Dot(transform.up))
      };
      Graphics.DrawMeshNow(Singleton<MeshPool>.Get.UnitWireQuadXY, Matrix4x4.TRS(worldPoint1, Quaternion.AngleAxis(rotationDegrees, transform.forward) * transform.rotation, one));
    }

    public static void DrawQuad2D(
      Vector2 quadCenter,
      Vector2 quadSize,
      float rotationDegrees,
      Camera camera)
    {
      float z = camera.nearClipPlane + 0.01f;
      Vector3 worldPoint1 = camera.ScreenToWorldPoint(new Vector3(quadCenter.x, quadCenter.y, z));
      Vector3 worldPoint2 = camera.ScreenToWorldPoint(new Vector3(quadCenter.x - quadSize.x * 0.5f, quadCenter.y + quadSize.y * 0.5f, z));
      Vector3 worldPoint3 = camera.ScreenToWorldPoint(new Vector3(quadCenter.x + quadSize.x * 0.5f, quadCenter.y - quadSize.y * 0.5f, z));
      Transform transform = camera.transform;
      Vector3 one = Vector3.one with
      {
        x = Mathf.Abs((worldPoint3 - worldPoint2).Dot(transform.right)),
        y = Mathf.Abs((worldPoint3 - worldPoint2).Dot(transform.up))
      };
      Graphics.DrawMeshNow(Singleton<MeshPool>.Get.UnitQuadXY, Matrix4x4.TRS(worldPoint1, Quaternion.AngleAxis(rotationDegrees, transform.forward) * transform.rotation, one));
    }

    public static void DrawWireBox(AABB box)
    {
      Graphics.DrawMeshNow(Singleton<MeshPool>.Get.UnitWireBox, box.GetUnitBoxTransform());
    }

    public static void DrawWireCornerBox(AABB box, float wireCornerLinePercentage)
    {
      Mesh unitCoordSystem = Singleton<MeshPool>.Get.UnitCoordSystem;
      List<Vector3> cornerPoints = box.GetCornerPoints();
      wireCornerLinePercentage = Mathf.Clamp(wireCornerLinePercentage, 0.0f, 1f);
      Vector3 vector3 = box.Extents * wireCornerLinePercentage;
      Vector3 s1 = vector3;
      Matrix4x4 matrix1 = Matrix4x4.TRS(cornerPoints[3], Quaternion.identity, s1);
      Graphics.DrawMeshNow(unitCoordSystem, matrix1);
      Vector3 pos1 = cornerPoints[2];
      s1.x *= -1f;
      Matrix4x4 matrix2 = Matrix4x4.TRS(pos1, Quaternion.identity, s1);
      Graphics.DrawMeshNow(unitCoordSystem, matrix2);
      Vector3 pos2 = cornerPoints[1];
      s1.y *= -1f;
      Matrix4x4 matrix3 = Matrix4x4.TRS(pos2, Quaternion.identity, s1);
      Graphics.DrawMeshNow(unitCoordSystem, matrix3);
      Vector3 pos3 = cornerPoints[0];
      Vector3 s2 = vector3;
      s2.y *= -1f;
      Matrix4x4 matrix4 = Matrix4x4.TRS(pos3, Quaternion.identity, s2);
      Graphics.DrawMeshNow(unitCoordSystem, matrix4);
      Vector3 pos4 = cornerPoints[7];
      s2.y = vector3.y;
      s2.x *= -1f;
      s2.z *= -1f;
      Matrix4x4 matrix5 = Matrix4x4.TRS(pos4, Quaternion.identity, s2);
      Graphics.DrawMeshNow(unitCoordSystem, matrix5);
      Vector3 pos5 = cornerPoints[6];
      s2.x = vector3.x;
      Matrix4x4 matrix6 = Matrix4x4.TRS(pos5, Quaternion.identity, s2);
      Graphics.DrawMeshNow(unitCoordSystem, matrix6);
      Vector3 pos6 = cornerPoints[5];
      s2.y *= -1f;
      Matrix4x4 matrix7 = Matrix4x4.TRS(pos6, Quaternion.identity, s2);
      Graphics.DrawMeshNow(unitCoordSystem, matrix7);
      Vector3 pos7 = cornerPoints[4];
      s2.x *= -1f;
      Matrix4x4 matrix8 = Matrix4x4.TRS(pos7, Quaternion.identity, s2);
      Graphics.DrawMeshNow(unitCoordSystem, matrix8);
    }

    public static void DrawWireBox(OBB box)
    {
      Graphics.DrawMeshNow(Singleton<MeshPool>.Get.UnitWireBox, box.GetUnitBoxTransform());
    }

    /// <summary>Renders a wire box with lines meeting at corners.</summary>
    /// <param name="wireCornerLinePercentage">
    /// Can have values in the [0, 1] interval and it controls the length of the
    /// corner lines. A value of 0 draws lines of length 0 and a value of 1
    /// draws lines of length equal to half the box length along a certain axis.
    /// </param>
    public static void DrawWireCornerBox(OBB box, float wireCornerLinePercentage)
    {
      Mesh unitCoordSystem = Singleton<MeshPool>.Get.UnitCoordSystem;
      List<Vector3> cornerPoints = box.GetCornerPoints();
      wireCornerLinePercentage = Mathf.Clamp(wireCornerLinePercentage, 0.0f, 1f);
      Vector3 vector3 = box.Extents * wireCornerLinePercentage;
      Vector3 s1 = vector3;
      Matrix4x4 matrix1 = Matrix4x4.TRS(cornerPoints[3], box.Rotation, s1);
      Graphics.DrawMeshNow(unitCoordSystem, matrix1);
      Vector3 pos1 = cornerPoints[2];
      s1.x *= -1f;
      Matrix4x4 matrix2 = Matrix4x4.TRS(pos1, box.Rotation, s1);
      Graphics.DrawMeshNow(unitCoordSystem, matrix2);
      Vector3 pos2 = cornerPoints[1];
      s1.y *= -1f;
      Matrix4x4 matrix3 = Matrix4x4.TRS(pos2, box.Rotation, s1);
      Graphics.DrawMeshNow(unitCoordSystem, matrix3);
      Vector3 pos3 = cornerPoints[0];
      Vector3 s2 = vector3;
      s2.y *= -1f;
      Matrix4x4 matrix4 = Matrix4x4.TRS(pos3, box.Rotation, s2);
      Graphics.DrawMeshNow(unitCoordSystem, matrix4);
      Vector3 pos4 = cornerPoints[7];
      s2.y = vector3.y;
      s2.x *= -1f;
      s2.z *= -1f;
      Matrix4x4 matrix5 = Matrix4x4.TRS(pos4, box.Rotation, s2);
      Graphics.DrawMeshNow(unitCoordSystem, matrix5);
      Vector3 pos5 = cornerPoints[6];
      s2.x = vector3.x;
      Matrix4x4 matrix6 = Matrix4x4.TRS(pos5, box.Rotation, s2);
      Graphics.DrawMeshNow(unitCoordSystem, matrix6);
      Vector3 pos6 = cornerPoints[5];
      s2.y *= -1f;
      Matrix4x4 matrix7 = Matrix4x4.TRS(pos6, box.Rotation, s2);
      Graphics.DrawMeshNow(unitCoordSystem, matrix7);
      Vector3 pos7 = cornerPoints[4];
      s2.x *= -1f;
      Matrix4x4 matrix8 = Matrix4x4.TRS(pos7, box.Rotation, s2);
      Graphics.DrawMeshNow(unitCoordSystem, matrix8);
    }
  }
}
