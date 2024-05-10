// Decompiled with JetBrains decompiler
// Type: RTG.ObjectVertexCollect
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
  public static class ObjectVertexCollect
  {
    private static List<Vector3> _hierarchyVertsCollectBuffer;

    public static List<Vector3> CollectModelSpriteVerts(Sprite sprite, AABB collectAABB)
    {
      Vector2[] vertices = sprite.vertices;
      List<Vector3> vector3List = new List<Vector3>(7);
      foreach (Vector2 point in vertices)
      {
        if (BoxMath.ContainsPoint((Vector3) point, collectAABB.Center, collectAABB.Size, Quaternion.identity))
          vector3List.Add((Vector3) point);
      }
      return vector3List;
    }

    public static List<Vector3> CollectWorldSpriteVerts(
      Sprite sprite,
      Transform spriteTransform,
      OBB collectOBB)
    {
      List<Vector3> worldVerts = sprite.GetWorldVerts(spriteTransform);
      List<Vector3> vector3List = new List<Vector3>(7);
      foreach (Vector3 point in worldVerts)
      {
        if (BoxMath.ContainsPoint(point, collectOBB.Center, collectOBB.Size, collectOBB.Rotation))
          vector3List.Add(point);
      }
      return vector3List;
    }

    public static List<Vector3> CollectHierarchyVerts(
      GameObject root,
      BoxFace collectFace,
      float collectBoxScale,
      float collectEps)
    {
      List<GameObject> objectsInHierarchy1 = root.GetMeshObjectsInHierarchy();
      List<GameObject> objectsInHierarchy2 = root.GetSpriteObjectsInHierarchy();
      if (objectsInHierarchy1.Count == 0 && objectsInHierarchy2.Count == 0)
        return new List<Vector3>();
      OBB obb1 = ObjectBounds.CalcHierarchyWorldOBB(root, new ObjectBounds.QueryConfig()
      {
        ObjectTypes = GameObjectType.Mesh | GameObjectType.Sprite
      });
      if (!obb1.IsValid)
        return new List<Vector3>();
      int faceAxisIndex = BoxMath.GetFaceAxisIndex(collectFace);
      Vector3 vector3_1 = BoxMath.CalcBoxFaceCenter(obb1.Center, obb1.Size, obb1.Rotation, collectFace);
      Vector3 vector3_2 = BoxMath.CalcBoxFaceNormal(obb1.Center, obb1.Size, obb1.Rotation, collectFace);
      float num = collectEps * 2f;
      Vector3 size = obb1.Size;
      size[faceAxisIndex] = obb1.Size[faceAxisIndex] * collectBoxScale + num;
      size[(faceAxisIndex + 1) % 3] += num;
      size[(faceAxisIndex + 2) % 3] += num;
      OBB obb2 = new OBB(vector3_1 + vector3_2 * ((float) (-(double) size[faceAxisIndex] * 0.5) + collectEps), size);
      obb2.Rotation = obb1.Rotation;
      List<Vector3> vector3List = new List<Vector3>(80);
      foreach (GameObject gameObject in objectsInHierarchy1)
      {
        RTMesh rtMesh = Singleton<RTMeshDb>.Get.GetRTMesh(gameObject.GetMesh());
        if (rtMesh != null)
        {
          rtMesh.OverlapVerts(obb2, gameObject.transform, ObjectVertexCollect._hierarchyVertsCollectBuffer);
          if (ObjectVertexCollect._hierarchyVertsCollectBuffer.Count != 0)
            vector3List.AddRange((IEnumerable<Vector3>) ObjectVertexCollect._hierarchyVertsCollectBuffer);
        }
      }
      foreach (GameObject gameObject in objectsInHierarchy2)
      {
        List<Vector3> collection = ObjectVertexCollect.CollectWorldSpriteVerts(gameObject.GetSprite(), gameObject.transform, obb2);
        if (collection.Count != 0)
          vector3List.AddRange((IEnumerable<Vector3>) collection);
      }
      return vector3List;
    }

    static ObjectVertexCollect()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      ObjectVertexCollect._hierarchyVertsCollectBuffer = new List<Vector3>();
    }
  }
}
