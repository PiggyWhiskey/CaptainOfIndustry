// Decompiled with JetBrains decompiler
// Type: RTG.ObjectSpawnUtil
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;

#nullable disable
namespace RTG
{
  public static class ObjectSpawnUtil
  {
    public static GameObject SpawnInFrontOfCamera(
      GameObject sourceObject,
      Camera camera,
      float objectSize)
    {
      float num1 = objectSize * 0.5f;
      ObjectBounds.QueryConfig queryConfig = new ObjectBounds.QueryConfig();
      queryConfig.ObjectTypes = GameObjectTypeHelper.AllCombined;
      queryConfig.NoVolumeSize = Vector3Ex.FromValue(1f);
      Transform transform = camera.transform;
      AABB aabb = ObjectBounds.CalcHierarchyWorldAABB(sourceObject, queryConfig);
      if (!aabb.IsValid)
        return (GameObject) null;
      Sphere sphere = new Sphere(aabb);
      Vector3 vector3_1 = sourceObject.transform.position - sphere.Center;
      float num2 = Mathf.Max(camera.nearClipPlane + sphere.Radius, sphere.Radius / num1);
      Vector3 vector3_2 = transform.position + transform.forward * num2;
      GameObject root = Object.Instantiate<GameObject>(sourceObject, vector3_2 + vector3_1, sourceObject.transform.rotation);
      OBB obb = ObjectBounds.CalcHierarchyWorldOBB(root, queryConfig);
      SceneRaycastHit sceneRaycastHit = MonoSingleton<RTScene>.Get.Raycast(new Ray(camera.transform.position, (obb.Center - camera.transform.position).normalized), SceneRaycastPrecision.BestFit, new SceneRaycastFilter()
      {
        AllowedObjectTypes = {
          GameObjectType.Mesh
        }
      });
      if (sceneRaycastHit.WasAnObjectHit)
      {
        Vector3 center = obb.Center;
        obb.Center = sceneRaycastHit.ObjectHit.HitPoint;
        Vector3 vector3_3 = obb.Center - center + ObjectSurfaceSnap.CalculateSitOnSurfaceOffset(obb, sceneRaycastHit.ObjectHit.HitPlane, 0.0f);
        root.transform.position += vector3_3;
      }
      return root;
    }
  }
}
