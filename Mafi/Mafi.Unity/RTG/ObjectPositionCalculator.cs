// Decompiled with JetBrains decompiler
// Type: RTG.ObjectPositionCalculator
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public static class ObjectPositionCalculator
  {
    private static ObjectBounds.QueryConfig _boundsQConfig;

    static ObjectPositionCalculator()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      ObjectPositionCalculator._boundsQConfig.NoVolumeSize = Vector3.zero;
      ObjectPositionCalculator._boundsQConfig.ObjectTypes = GameObjectTypeHelper.AllCombined;
    }

    public static Vector3 CalculateRootPosition(
      GameObject root,
      Vector3 desiredOBBCenter,
      Vector3 desiredWorldScale,
      Quaternion desiredWorldRotation)
    {
      OBB obb = ObjectBounds.CalcHierarchyWorldOBB(root, ObjectPositionCalculator._boundsQConfig);
      Transform transform = root.transform;
      Matrix4x4 matrix4x4 = Matrix4x4.TRS(Vector3.zero, transform.rotation, transform.lossyScale);
      Vector3 vector3 = (Matrix4x4.TRS(Vector3.zero, desiredWorldRotation, desiredWorldScale) * matrix4x4.inverse).MultiplyVector(transform.position - obb.Center);
      return desiredOBBCenter + vector3;
    }
  }
}
