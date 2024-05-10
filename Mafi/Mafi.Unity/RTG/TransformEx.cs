// Decompiled with JetBrains decompiler
// Type: RTG.TransformEx
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace RTG
{
  public static class TransformEx
  {
    public static void TransformPoints(this Transform transform, List<Vector3> points)
    {
      for (int index = 0; index < points.Count; ++index)
        points[index] = transform.TransformPoint(points[index]);
    }

    public static List<Transform> GetGameObjectTransformCollection(
      IEnumerable<GameObject> gameObjects)
    {
      List<Transform> transformCollection = new List<Transform>(10);
      foreach (GameObject gameObject in gameObjects)
        transformCollection.Add(gameObject.transform);
      return transformCollection;
    }

    public static List<Transform> FilterParentsOnly(IEnumerable<Transform> transforms)
    {
      if (transforms == null)
        return new List<Transform>();
      List<Transform> transformList = new List<Transform>(10);
      foreach (Transform transform1 in transforms)
      {
        bool flag = false;
        foreach (Transform transform2 in transforms)
        {
          if ((Object) transform2 != (Object) transform1 && transform1.IsChildOf(transform2.transform))
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          transformList.Add(transform1);
      }
      return transformList;
    }

    public static void SetWorldScale(this Transform transform, Vector3 worldScale)
    {
      transform.localScale = Vector3.one;
      transform.localScale = new Vector3(worldScale.x / transform.lossyScale.x, worldScale.y / transform.lossyScale.y, worldScale.z / transform.lossyScale.z);
    }

    public static void ScaleFromPivot(this Transform transform, Vector3 scaleFactor, Vector3 pivot)
    {
      Vector3 worldScale = Vector3.Scale(transform.lossyScale, scaleFactor);
      transform.SetWorldScale(worldScale);
      Vector3 right = transform.right;
      Vector3 up = transform.up;
      Vector3 forward = transform.forward;
      Vector3 lhs = transform.position - pivot;
      float num1 = Vector3.Dot(lhs, right) * scaleFactor.x;
      float num2 = Vector3.Dot(lhs, up) * scaleFactor.y;
      float num3 = Vector3.Dot(lhs, forward) * scaleFactor.z;
      transform.position = pivot + right * num1 + up * num2 + forward * num3;
    }

    public static void RotateAroundPivot(
      this Transform transform,
      Quaternion rotation,
      Vector3 pivot)
    {
      Vector3 vector3_1 = transform.position - pivot;
      transform.rotation = rotation * transform.rotation;
      Vector3 vector3_2 = rotation * vector3_1;
      transform.position = pivot + vector3_2;
    }

    public static Vector3 GetLocalAxis(this Transform transform, AxisDescriptor axisDesc)
    {
      Vector3 localAxis = transform.right;
      if (axisDesc.Index == 1)
        localAxis = transform.up;
      else if (axisDesc.Index == 2)
        localAxis = transform.forward;
      if (axisDesc.Sign == AxisSign.Negative)
        localAxis = -localAxis;
      return localAxis;
    }

    public static Plane GetLocalPlane(this Transform transform, PlaneDescriptor planeDesc)
    {
      return new Plane(Vector3.Normalize(Vector3.Cross(transform.GetLocalAxis(planeDesc.FirstAxisDescriptor), transform.GetLocalAxis(planeDesc.SecondAxisDescriptor))), transform.position);
    }

    public static Quaternion Align(
      this Transform transform,
      Vector3 normAlignVector,
      TransformAxis alignmentAxis)
    {
      Vector3 vector3 = transform.up;
      switch (alignmentAxis)
      {
        case TransformAxis.PositiveX:
          vector3 = transform.right;
          break;
        case TransformAxis.NegativeX:
          vector3 = -transform.right;
          break;
        case TransformAxis.NegativeY:
          vector3 = -transform.up;
          break;
        case TransformAxis.PositiveZ:
          vector3 = transform.forward;
          break;
        case TransformAxis.NegativeZ:
          vector3 = -transform.forward;
          break;
      }
      float num = Vector3.Dot(vector3, normAlignVector);
      if (1.0 - (double) num < 9.9999997473787516E-06)
        return Quaternion.identity;
      Vector3 axis = Vector3.zero;
      if ((double) num + 1.0 < 9.9999997473787516E-06)
      {
        switch (alignmentAxis)
        {
          case TransformAxis.PositiveX:
            axis = transform.up;
            break;
          case TransformAxis.NegativeX:
            axis = -transform.up;
            break;
          case TransformAxis.PositiveY:
            axis = transform.right;
            break;
          case TransformAxis.NegativeY:
            axis = -transform.right;
            break;
          case TransformAxis.PositiveZ:
            axis = transform.up;
            break;
          case TransformAxis.NegativeZ:
            axis = -transform.up;
            break;
        }
      }
      else
        axis = Vector3.Normalize(Vector3.Cross(vector3, normAlignVector));
      float angle = Vector3Ex.SignedAngle(vector3, normAlignVector, axis);
      transform.Rotate(axis, angle, Space.World);
      return Quaternion.AngleAxis(angle, axis);
    }
  }
}
