// Decompiled with JetBrains decompiler
// Type: RTG.ObjectAlign
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace RTG
{
  public static class ObjectAlign
  {
    public static ObjectAlign.Result AlignToWorldAxis(
      IEnumerable<GameObject> gameObjects,
      Axis axis,
      Vector3 alignmentPlaneOrigin)
    {
      Vector3 inNormal = Vector3.forward;
      switch (axis)
      {
        case Axis.Y:
          inNormal = Vector3.up;
          break;
        case Axis.Z:
          inNormal = Vector3.right;
          break;
      }
      Plane alignmentPlane = new Plane(inNormal, alignmentPlaneOrigin);
      return ObjectAlign.AlignToWorldPlane(gameObjects, alignmentPlane);
    }

    public static ObjectAlign.Result AlignToWorldPlane(
      IEnumerable<GameObject> gameObjects,
      Plane alignmentPlane)
    {
      List<GameObject> roots = GameObjectEx.FilterParentsOnly(gameObjects);
      if (roots.Count == 0)
        return ObjectAlign.Result.Err_NoObjects;
      ObjectAlign.AlignRootsToPlane(roots, alignmentPlane);
      return ObjectAlign.Result.Success;
    }

    private static void AlignRootsToPlane(List<GameObject> roots, Plane alignmentPlane)
    {
      ObjectBounds.QueryConfig queryConfig = new ObjectBounds.QueryConfig();
      queryConfig.NoVolumeSize = Vector3.zero;
      queryConfig.ObjectTypes = GameObjectTypeHelper.AllCombined;
      foreach (GameObject root in roots)
      {
        OBB obb = ObjectBounds.CalcHierarchyWorldOBB(root, queryConfig);
        if (obb.IsValid)
        {
          Vector3 vector3 = alignmentPlane.ProjectPoint(obb.Center);
          root.transform.position += vector3 - obb.Center;
        }
      }
    }

    public enum Result
    {
      Err_NoObjects,
      Success,
    }
  }
}
