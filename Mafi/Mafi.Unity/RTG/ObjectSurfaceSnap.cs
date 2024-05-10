// Decompiled with JetBrains decompiler
// Type: RTG.ObjectSurfaceSnap
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class ObjectSurfaceSnap
  {
    public static ObjectSurfaceSnap.SnapResult SnapHierarchy(
      GameObject root,
      ObjectSurfaceSnap.SnapConfig snapConfig)
    {
      bool flag1 = root.HierarchyHasMesh();
      bool flag2 = root.HierarchyHasSprite();
      if (!flag1 && !flag2)
      {
        Transform transform = root.transform;
        transform.position = snapConfig.SurfaceHitPlane.ProjectPoint(transform.position) + snapConfig.OffsetFromSurface * snapConfig.SurfaceHitNormal;
        return new ObjectSurfaceSnap.SnapResult(snapConfig.SurfaceHitPlane, transform.position);
      }
      ObjectBounds.QueryConfig queryConfig = new ObjectBounds.QueryConfig();
      queryConfig.ObjectTypes = GameObjectType.Mesh | GameObjectType.Sprite;
      bool flag3 = snapConfig.SurfaceType == ObjectSurfaceSnap.Type.SphericalMesh;
      bool flag4 = snapConfig.SurfaceType == ObjectSurfaceSnap.Type.UnityTerrain || snapConfig.SurfaceType == ObjectSurfaceSnap.Type.TerrainMesh;
      bool flag5 = snapConfig.SurfaceType == ObjectSurfaceSnap.Type.UnityTerrain;
      ObjectSurfaceSnap.SurfaceRaycaster surfaceRaycaster = ObjectSurfaceSnap.CreateSurfaceRaycaster(snapConfig.SurfaceType, snapConfig.SurfaceObject, true);
      if (snapConfig.SurfaceType != ObjectSurfaceSnap.Type.SceneGrid)
      {
        Transform transform1 = root.transform;
        if (snapConfig.AlignAxis)
        {
          if (flag4)
          {
            transform1.Align(Vector3.up, snapConfig.AlignmentAxis);
            OBB obb = ObjectBounds.CalcHierarchyWorldOBB(root, queryConfig);
            if (!obb.IsValid)
              return new ObjectSurfaceSnap.SnapResult();
            BoxFace mostAlignedFace = BoxMath.GetMostAlignedFace(obb.Center, obb.Size, obb.Rotation, -Vector3.up);
            List<Vector3> vector3List = ObjectVertexCollect.CollectHierarchyVerts(root, mostAlignedFace, 1f / 1000f, 0.01f);
            if (vector3List.Count != 0)
            {
              Ray ray = new Ray(Vector3Ex.GetPointCloudCenter((IEnumerable<Vector3>) vector3List) + Vector3.up * (1f / 1000f), -Vector3.up);
              GameObjectRayHit gameObjectRayHit = surfaceRaycaster.Raycast(ray);
              if (gameObjectRayHit != null)
              {
                Vector3 vector3 = gameObjectRayHit.HitNormal;
                if (flag5)
                  vector3 = snapConfig.SurfaceObject.GetComponent<Terrain>().GetInterpolatedNormal(gameObjectRayHit.HitPoint);
                Quaternion quat = transform1.Align(vector3, snapConfig.AlignmentAxis);
                obb = ObjectBounds.CalcHierarchyWorldOBB(root, queryConfig);
                quat.RotatePoints(vector3List, transform1.position);
                Vector3 sitOnSurfaceOffset = ObjectSurfaceSnap.CalculateSitOnSurfaceOffset(obb, new Plane(Vector3.up, gameObjectRayHit.HitPoint), 0.1f);
                transform1.position += sitOnSurfaceOffset;
                obb.Center += sitOnSurfaceOffset;
                Vector3Ex.OffsetPoints(vector3List, sitOnSurfaceOffset);
                Vector3 embedVector = ObjectSurfaceSnap.CalculateEmbedVector(vector3List, snapConfig.SurfaceObject, -Vector3.up, snapConfig.SurfaceType);
                transform1.position += embedVector + vector3 * snapConfig.OffsetFromSurface;
                return new ObjectSurfaceSnap.SnapResult(new Plane(vector3, gameObjectRayHit.HitPoint), gameObjectRayHit.HitPoint);
              }
            }
          }
          else if (!flag3)
          {
            transform1.Align(snapConfig.SurfaceHitNormal, snapConfig.AlignmentAxis);
            OBB obb = ObjectBounds.CalcHierarchyWorldOBB(root, queryConfig);
            if (!obb.IsValid)
              return new ObjectSurfaceSnap.SnapResult();
            BoxFace mostAlignedFace = BoxMath.GetMostAlignedFace(obb.Center, obb.Size, obb.Rotation, -snapConfig.SurfaceHitNormal);
            List<Vector3> ptCloud = ObjectVertexCollect.CollectHierarchyVerts(root, mostAlignedFace, 1f / 1000f, 0.01f);
            if (ptCloud.Count != 0)
            {
              Vector3 pointCloudCenter = Vector3Ex.GetPointCloudCenter((IEnumerable<Vector3>) ptCloud);
              float magnitude = ObjectBounds.CalcMeshWorldAABB(snapConfig.SurfaceObject).Extents.magnitude;
              Ray ray = new Ray(pointCloudCenter + snapConfig.SurfaceHitNormal * magnitude, -snapConfig.SurfaceHitNormal);
              GameObjectRayHit gameObjectRayHit = surfaceRaycaster.Raycast(ray);
              if (gameObjectRayHit != null)
              {
                Vector3 hitNormal = gameObjectRayHit.HitNormal;
                transform1.Align(hitNormal, snapConfig.AlignmentAxis);
                obb = ObjectBounds.CalcHierarchyWorldOBB(root, queryConfig);
                Vector3 sitOnSurfaceOffset = ObjectSurfaceSnap.CalculateSitOnSurfaceOffset(obb, gameObjectRayHit.HitPlane, 0.0f);
                transform1.position += sitOnSurfaceOffset;
                transform1.position += hitNormal * snapConfig.OffsetFromSurface;
                return new ObjectSurfaceSnap.SnapResult(new Plane(hitNormal, gameObjectRayHit.HitPoint), gameObjectRayHit.HitPoint);
              }
              Vector3 surfaceHitNormal = snapConfig.SurfaceHitNormal;
              transform1.Align(surfaceHitNormal, snapConfig.AlignmentAxis);
              obb = ObjectBounds.CalcHierarchyWorldOBB(root, queryConfig);
              Vector3 sitOnSurfaceOffset1 = ObjectSurfaceSnap.CalculateSitOnSurfaceOffset(obb, snapConfig.SurfaceHitPlane, 0.0f);
              transform1.position += sitOnSurfaceOffset1;
              transform1.position += surfaceHitNormal * snapConfig.OffsetFromSurface;
              return new ObjectSurfaceSnap.SnapResult(snapConfig.SurfaceHitPlane, snapConfig.SurfaceHitPlane.ProjectPoint(pointCloudCenter));
            }
          }
          else
          {
            Transform transform2 = snapConfig.SurfaceObject.transform;
            Vector3 position = transform2.position;
            Vector3 normalized = (transform1.position - position).normalized;
            float num = transform2.lossyScale.GetMaxAbsComp() * 0.5f;
            transform1.Align(normalized, snapConfig.AlignmentAxis);
            OBB obb = ObjectBounds.CalcHierarchyWorldOBB(root, queryConfig);
            if (!obb.IsValid)
              return new ObjectSurfaceSnap.SnapResult();
            BoxFace mostAlignedFace = BoxMath.GetMostAlignedFace(obb.Center, obb.Size, obb.Rotation, -normalized);
            List<Vector3> points = ObjectVertexCollect.CollectHierarchyVerts(root, mostAlignedFace, 1f / 1000f, 0.01f);
            Vector3 vector3 = position + normalized * num;
            Plane plane = new Plane(normalized, vector3);
            Vector3 sitOnSurfaceOffset = ObjectSurfaceSnap.CalculateSitOnSurfaceOffset(obb, plane, 0.0f);
            transform1.position += sitOnSurfaceOffset;
            obb.Center += sitOnSurfaceOffset;
            Vector3Ex.OffsetPoints(points, sitOnSurfaceOffset);
            transform1.position += normalized * snapConfig.OffsetFromSurface;
            return new ObjectSurfaceSnap.SnapResult(plane, vector3);
          }
        }
        else
        {
          OBB obb = ObjectBounds.CalcHierarchyWorldOBB(root, queryConfig);
          if (!obb.IsValid)
            return new ObjectSurfaceSnap.SnapResult();
          if (flag4 || !flag3 && snapConfig.SurfaceType == ObjectSurfaceSnap.Type.Mesh)
          {
            Ray ray = new Ray(obb.Center, flag4 ? -Vector3.up : -snapConfig.SurfaceHitNormal);
            GameObjectRayHit gameObjectRayHit = surfaceRaycaster.Raycast(ray);
            if (gameObjectRayHit != null)
            {
              Vector3 sitOnSurfaceOffset = ObjectSurfaceSnap.CalculateSitOnSurfaceOffset(obb, gameObjectRayHit.HitPlane, 0.0f);
              transform1.position += sitOnSurfaceOffset;
              if (flag4)
              {
                obb.Center += sitOnSurfaceOffset;
                BoxFace mostAlignedFace = BoxMath.GetMostAlignedFace(obb.Center, obb.Size, obb.Rotation, -gameObjectRayHit.HitNormal);
                Vector3 embedVector = ObjectSurfaceSnap.CalculateEmbedVector(ObjectVertexCollect.CollectHierarchyVerts(root, mostAlignedFace, 1f / 1000f, 0.01f), snapConfig.SurfaceObject, -Vector3.up, snapConfig.SurfaceType);
                transform1.position += embedVector;
              }
              transform1.position += gameObjectRayHit.HitNormal * snapConfig.OffsetFromSurface;
              return new ObjectSurfaceSnap.SnapResult(gameObjectRayHit.HitPlane, gameObjectRayHit.HitPoint);
            }
            if (!flag3 && snapConfig.SurfaceType == ObjectSurfaceSnap.Type.Mesh)
            {
              Vector3 sitOnSurfaceOffset = ObjectSurfaceSnap.CalculateSitOnSurfaceOffset(obb, snapConfig.SurfaceHitPlane, 0.0f);
              transform1.position += sitOnSurfaceOffset;
              transform1.position += snapConfig.SurfaceHitNormal * snapConfig.OffsetFromSurface;
              return new ObjectSurfaceSnap.SnapResult(snapConfig.SurfaceHitPlane, snapConfig.SurfaceHitPlane.ProjectPoint(obb.Center));
            }
          }
          else if (flag3)
          {
            Transform transform3 = snapConfig.SurfaceObject.transform;
            Vector3 position = transform3.position;
            Vector3 normalized = (transform1.position - position).normalized;
            float num = transform3.lossyScale.GetMaxAbsComp() * 0.5f;
            BoxFace mostAlignedFace = BoxMath.GetMostAlignedFace(obb.Center, obb.Size, obb.Rotation, -normalized);
            List<Vector3> points = ObjectVertexCollect.CollectHierarchyVerts(root, mostAlignedFace, 1f / 1000f, 0.01f);
            Vector3 vector3 = position + normalized * num;
            Plane plane = new Plane(normalized, vector3);
            Vector3 sitOnSurfaceOffset = ObjectSurfaceSnap.CalculateSitOnSurfaceOffset(obb, plane, 0.0f);
            transform1.position += sitOnSurfaceOffset;
            obb.Center += sitOnSurfaceOffset;
            Vector3Ex.OffsetPoints(points, sitOnSurfaceOffset);
            transform1.position += normalized * snapConfig.OffsetFromSurface;
            return new ObjectSurfaceSnap.SnapResult(plane, vector3);
          }
        }
      }
      if (snapConfig.SurfaceType != ObjectSurfaceSnap.Type.SceneGrid)
        return new ObjectSurfaceSnap.SnapResult();
      OBB obb1 = ObjectBounds.CalcHierarchyWorldOBB(root, queryConfig);
      if (!obb1.IsValid)
        return new ObjectSurfaceSnap.SnapResult();
      Transform transform4 = root.transform;
      if (snapConfig.AlignAxis)
      {
        transform4.Align(snapConfig.SurfaceHitNormal, snapConfig.AlignmentAxis);
        obb1 = ObjectBounds.CalcHierarchyWorldOBB(root, queryConfig);
      }
      transform4.position += ObjectSurfaceSnap.CalculateSitOnSurfaceOffset(obb1, snapConfig.SurfaceHitPlane, snapConfig.OffsetFromSurface);
      return new ObjectSurfaceSnap.SnapResult(snapConfig.SurfaceHitPlane, snapConfig.SurfaceHitPlane.ProjectPoint(obb1.Center));
    }

    public static Vector3 CalculateSitOnSurfaceOffset(
      OBB obb,
      Plane surfacePlane,
      float offsetFromSurface)
    {
      List<Vector3> cornerPoints = obb.GetCornerPoints();
      int index = surfacePlane.GetFurthestPtBehind(cornerPoints);
      if (index < 0)
        index = surfacePlane.GetClosestPtInFrontOrOnPlane(cornerPoints);
      if (index < 0)
        return Vector3.zero;
      Vector3 pt = cornerPoints[index];
      return surfacePlane.ProjectPoint(pt) - pt + surfacePlane.normal * offsetFromSurface;
    }

    public static Vector3 CalculateSitOnSurfaceOffset(
      AABB aabb,
      Plane surfacePlane,
      float offsetFromSurface)
    {
      List<Vector3> cornerPoints = aabb.GetCornerPoints();
      int index = surfacePlane.GetFurthestPtBehind(cornerPoints);
      if (index < 0)
        index = surfacePlane.GetClosestPtInFrontOrOnPlane(cornerPoints);
      if (index < 0)
        return Vector3.zero;
      Vector3 pt = cornerPoints[index];
      return surfacePlane.ProjectPoint(pt) - pt + surfacePlane.normal * offsetFromSurface;
    }

    public static Vector3 CalculateEmbedVector(
      List<Vector3> embedPoints,
      GameObject embedSurface,
      Vector3 embedDirection,
      ObjectSurfaceSnap.Type surfaceType)
    {
      ObjectSurfaceSnap.SurfaceRaycaster surfaceRaycaster = ObjectSurfaceSnap.CreateSurfaceRaycaster(surfaceType, embedSurface, false);
      bool flag = false;
      float f = float.MinValue;
      foreach (Vector3 embedPoint in embedPoints)
      {
        Ray ray = new Ray(embedPoint, -embedDirection);
        if (surfaceRaycaster.Raycast(ray) == null)
        {
          ray = new Ray(embedPoint, embedDirection);
          GameObjectRayHit gameObjectRayHit = surfaceRaycaster.Raycast(ray);
          if (gameObjectRayHit != null)
          {
            float sqrMagnitude = (embedPoint - gameObjectRayHit.HitPoint).sqrMagnitude;
            if ((double) sqrMagnitude > (double) f)
            {
              f = sqrMagnitude;
              flag = true;
            }
          }
        }
      }
      return flag ? embedDirection * Mathf.Sqrt(f) : Vector3.zero;
    }

    private static ObjectSurfaceSnap.SurfaceRaycaster CreateSurfaceRaycaster(
      ObjectSurfaceSnap.Type surfaceType,
      GameObject surfaceObject,
      bool raycastReverse)
    {
      switch (surfaceType)
      {
        case ObjectSurfaceSnap.Type.UnityTerrain:
          return (ObjectSurfaceSnap.SurfaceRaycaster) new ObjectSurfaceSnap.TerrainSurfaceRaycaster(surfaceObject, raycastReverse);
        case ObjectSurfaceSnap.Type.Mesh:
        case ObjectSurfaceSnap.Type.TerrainMesh:
        case ObjectSurfaceSnap.Type.SphericalMesh:
          return (ObjectSurfaceSnap.SurfaceRaycaster) new ObjectSurfaceSnap.MeshSurfaceRaycaster(surfaceObject, raycastReverse);
        default:
          return (ObjectSurfaceSnap.SurfaceRaycaster) null;
      }
    }

    public ObjectSurfaceSnap()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public enum Type
    {
      UnityTerrain,
      Mesh,
      TerrainMesh,
      SphericalMesh,
      SceneGrid,
    }

    public struct SnapConfig
    {
      public bool AlignAxis;
      public TransformAxis AlignmentAxis;
      public ObjectSurfaceSnap.Type SurfaceType;
      public float OffsetFromSurface;
      public Vector3 SurfaceHitPoint;
      public Vector3 SurfaceHitNormal;
      public Plane SurfaceHitPlane;
      public GameObject SurfaceObject;

      public bool IsSurfaceMesh()
      {
        return this.SurfaceType == ObjectSurfaceSnap.Type.Mesh || this.SurfaceType == ObjectSurfaceSnap.Type.SphericalMesh || this.SurfaceType == ObjectSurfaceSnap.Type.TerrainMesh;
      }
    }

    public struct SnapResult
    {
      public bool Success;
      public Plane SittingPlane;
      public Vector3 SittingPoint;

      public SnapResult(Plane sittingPlane, Vector3 sittingPoint)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.Success = true;
        this.SittingPlane = sittingPlane;
        this.SittingPoint = sittingPoint;
      }
    }

    private abstract class SurfaceRaycaster
    {
      protected GameObject _surfaceObject;
      protected bool _raycastReverse;

      public SurfaceRaycaster(GameObject surfaceObject, bool raycastReverse)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this._surfaceObject = surfaceObject;
        this._raycastReverse = raycastReverse;
      }

      public abstract GameObjectRayHit Raycast(Ray ray);
    }

    private class MeshSurfaceRaycaster : ObjectSurfaceSnap.SurfaceRaycaster
    {
      public MeshSurfaceRaycaster(GameObject surfaceObject, bool raycastReverse)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector(surfaceObject, raycastReverse);
      }

      public override GameObjectRayHit Raycast(Ray ray)
      {
        int num = this._raycastReverse ? 1 : 0;
        return MonoSingleton<RTScene>.Get.RaycastMeshObject(ray, this._surfaceObject);
      }
    }

    private class TerrainSurfaceRaycaster : ObjectSurfaceSnap.SurfaceRaycaster
    {
      private TerrainCollider _terrainCollider;

      public TerrainSurfaceRaycaster(GameObject surfaceObject, bool raycastReverse)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector(surfaceObject, raycastReverse);
        this._terrainCollider = surfaceObject.GetComponent<TerrainCollider>();
      }

      public override GameObjectRayHit Raycast(Ray ray)
      {
        return this._raycastReverse ? MonoSingleton<RTScene>.Get.RaycastTerrainObjectReverseIfFail(ray, this._surfaceObject) : MonoSingleton<RTScene>.Get.RaycastTerrainObject(ray, this._surfaceObject, this._terrainCollider);
      }
    }
  }
}
