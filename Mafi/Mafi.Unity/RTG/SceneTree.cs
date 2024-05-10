// Decompiled with JetBrains decompiler
// Type: RTG.SceneTree
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
  public class SceneTree
  {
    private static float _nonMeshObjectSize;
    private SphereTree<GameObject> _objectTree;
    private List<SphereTreeNodeRayHit<GameObject>> _nodeHitBuffer;
    private List<SphereTreeNode<GameObject>> _nodeBuffer;
    private Dictionary<GameObject, SphereTreeNode<GameObject>> _objectToNode;

    public GameObjectRayHit RaycastMeshObject(Ray ray, GameObject gameObject)
    {
      RTMesh rtMesh = Singleton<RTMeshDb>.Get.GetRTMesh(gameObject.GetMesh());
      if (rtMesh != null)
      {
        MeshRayHit meshRayHit = rtMesh.Raycast(ray, gameObject.transform.localToWorldMatrix);
        if (meshRayHit != null)
          return new GameObjectRayHit(ray, gameObject, meshRayHit);
      }
      else
      {
        MeshCollider component = gameObject.GetComponent<MeshCollider>();
        RaycastHit hitInfo;
        if ((Object) component != (Object) null && component.Raycast(ray, out hitInfo, float.MaxValue))
          return new GameObjectRayHit(ray, hitInfo);
      }
      return (GameObjectRayHit) null;
    }

    public GameObjectRayHit RaycastSpriteObject(Ray ray, GameObject gameObject)
    {
      OBB obb = ObjectBounds.CalcSpriteWorldOBB(gameObject);
      if (!obb.IsValid)
        return (GameObjectRayHit) null;
      float t;
      return BoxMath.Raycast(ray, out t, obb.Center, obb.Size, obb.Rotation) ? new GameObjectRayHit(ray, gameObject, obb.GetPointFaceNormal(ray.GetPoint(t)), t) : (GameObjectRayHit) null;
    }

    public bool RaycastAll(
      Ray ray,
      SceneRaycastPrecision raycastPresicion,
      List<GameObjectRayHit> hits)
    {
      hits.Clear();
      if (!this._objectTree.RaycastAll(ray, this._nodeHitBuffer))
        return false;
      ObjectBounds.QueryConfig queryConfig = new ObjectBounds.QueryConfig();
      queryConfig.ObjectTypes = GameObjectTypeHelper.AllCombined;
      queryConfig.NoVolumeSize = Vector3Ex.FromValue(SceneTree._nonMeshObjectSize);
      Vector3 look = MonoSingleton<RTFocusCamera>.Get.Look;
      switch (raycastPresicion)
      {
        case SceneRaycastPrecision.BestFit:
          using (List<SphereTreeNodeRayHit<GameObject>>.Enumerator enumerator = this._nodeHitBuffer.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              GameObject data = enumerator.Current.HitNode.Data;
              if (!((Object) data == (Object) null) && data.activeInHierarchy)
              {
                Renderer component1 = data.GetComponent<Renderer>();
                if (!((Object) component1 != (Object) null) || component1.isVisible)
                {
                  switch (data.GetGameObjectType())
                  {
                    case GameObjectType.Mesh:
                      GameObjectRayHit gameObjectRayHit1 = this.RaycastMeshObject(ray, data);
                      if (gameObjectRayHit1 != null)
                      {
                        hits.Add(gameObjectRayHit1);
                        continue;
                      }
                      continue;
                    case GameObjectType.Terrain:
                      TerrainCollider component2 = data.GetComponent<TerrainCollider>();
                      RaycastHit hitInfo;
                      if ((Object) component2 != (Object) null && component2.Raycast(ray, out hitInfo, float.MaxValue))
                      {
                        hits.Add(new GameObjectRayHit(ray, hitInfo));
                        continue;
                      }
                      continue;
                    case GameObjectType.Sprite:
                      GameObjectRayHit gameObjectRayHit2 = this.RaycastSpriteObject(ray, data);
                      if (gameObjectRayHit2 != null)
                      {
                        hits.Add(gameObjectRayHit2);
                        continue;
                      }
                      continue;
                    default:
                      OBB obb = ObjectBounds.CalcWorldOBB(data, queryConfig);
                      float t;
                      if (obb.IsValid && BoxMath.Raycast(ray, out t, obb.Center, obb.Size, obb.Rotation))
                      {
                        BoxFaceDesc faceClosestToPoint = BoxMath.GetFaceClosestToPoint(ray.GetPoint(t), obb.Center, obb.Size, obb.Rotation, look);
                        GameObjectRayHit gameObjectRayHit3 = new GameObjectRayHit(ray, data, faceClosestToPoint.Plane.normal, t);
                        hits.Add(gameObjectRayHit3);
                        continue;
                      }
                      continue;
                  }
                }
              }
            }
            break;
          }
        case SceneRaycastPrecision.Box:
          using (List<SphereTreeNodeRayHit<GameObject>>.Enumerator enumerator = this._nodeHitBuffer.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              GameObject data = enumerator.Current.HitNode.Data;
              if (!((Object) data == (Object) null) && data.activeInHierarchy)
              {
                Renderer component = data.GetComponent<Renderer>();
                if (!((Object) component != (Object) null) || component.isVisible)
                {
                  OBB obb = ObjectBounds.CalcWorldOBB(data, queryConfig);
                  float t;
                  if (obb.IsValid && BoxMath.Raycast(ray, out t, obb.Center, obb.Size, obb.Rotation))
                  {
                    BoxFaceDesc faceClosestToPoint = BoxMath.GetFaceClosestToPoint(ray.GetPoint(t), obb.Center, obb.Size, obb.Rotation, look);
                    GameObjectRayHit gameObjectRayHit = new GameObjectRayHit(ray, data, faceClosestToPoint.Plane.normal, t);
                    hits.Add(gameObjectRayHit);
                  }
                }
              }
            }
            break;
          }
      }
      return hits.Count != 0;
    }

    public bool OverlapBox(OBB obb, List<GameObject> gameObjects)
    {
      gameObjects.Clear();
      if (!this._objectTree.OverlapBox(obb, this._nodeBuffer))
        return false;
      ObjectBounds.QueryConfig queryConfig = new ObjectBounds.QueryConfig();
      queryConfig.ObjectTypes = GameObjectTypeHelper.AllCombined;
      queryConfig.NoVolumeSize = Vector3Ex.FromValue(SceneTree._nonMeshObjectSize);
      foreach (SphereTreeNode<GameObject> sphereTreeNode in this._nodeBuffer)
      {
        GameObject data = sphereTreeNode.Data;
        if (!((Object) data == (Object) null) && data.activeInHierarchy)
        {
          OBB otherOBB = ObjectBounds.CalcWorldOBB(data, queryConfig);
          if (obb.IntersectsOBB(otherOBB))
            gameObjects.Add(data);
        }
      }
      return gameObjects.Count != 0;
    }

    public bool IsObjectRegistered(GameObject gameObject)
    {
      return this._objectToNode.ContainsKey(gameObject);
    }

    public bool RegisterObject(GameObject gameObject)
    {
      if (!this.CanRegisterObject(gameObject))
        return false;
      Sphere sphere = new Sphere(ObjectBounds.CalcWorldAABB(gameObject, new ObjectBounds.QueryConfig()
      {
        ObjectTypes = GameObjectTypeHelper.AllCombined,
        NoVolumeSize = Vector3Ex.FromValue(SceneTree._nonMeshObjectSize)
      }));
      SphereTreeNode<GameObject> sphereTreeNode = this._objectTree.AddNode(gameObject, sphere);
      this._objectToNode.Add(gameObject, sphereTreeNode);
      MonoSingleton<RTFocusCamera>.Get.SetObjectVisibilityDirty();
      return true;
    }

    public bool UnregisterObject(GameObject gameObject)
    {
      if (!this.IsObjectRegistered(gameObject))
        return false;
      this._objectTree.RemoveNode(this._objectToNode[gameObject]);
      this._objectToNode.Remove(gameObject);
      MonoSingleton<RTFocusCamera>.Get.SetObjectVisibilityDirty();
      return true;
    }

    public void OnObjectTransformChanged(Transform objectTransform)
    {
      Sphere sphere = new Sphere(ObjectBounds.CalcWorldAABB(objectTransform.gameObject, new ObjectBounds.QueryConfig()
      {
        ObjectTypes = GameObjectTypeHelper.AllCombined,
        NoVolumeSize = Vector3Ex.FromValue(SceneTree._nonMeshObjectSize)
      }));
      SphereTreeNode<GameObject> node = this._objectToNode[objectTransform.gameObject];
      node.Sphere = sphere;
      this._objectTree.OnNodeSphereUpdated(node);
      MonoSingleton<RTFocusCamera>.Get.SetObjectVisibilityDirty();
    }

    public void RemoveNodesWithNullObjects()
    {
      Dictionary<GameObject, SphereTreeNode<GameObject>> dictionary = new Dictionary<GameObject, SphereTreeNode<GameObject>>();
      foreach (KeyValuePair<GameObject, SphereTreeNode<GameObject>> keyValuePair in this._objectToNode)
      {
        if ((Object) keyValuePair.Key == (Object) null)
          this._objectTree.RemoveNode(keyValuePair.Value);
        else
          dictionary.Add(keyValuePair.Key, keyValuePair.Value);
      }
      this._objectToNode.Clear();
      this._objectToNode = dictionary;
    }

    public void DebugDraw() => this._objectTree.DebugDraw();

    private bool CanRegisterObject(GameObject gameObject)
    {
      return !((Object) gameObject == (Object) null) && !this.IsObjectRegistered(gameObject) && !gameObject.IsRTGAppObject() && !((Object) gameObject.GetComponent<RectTransform>() != (Object) null);
    }

    public SceneTree()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._objectTree = new SphereTree<GameObject>();
      this._nodeHitBuffer = new List<SphereTreeNodeRayHit<GameObject>>();
      this._nodeBuffer = new List<SphereTreeNode<GameObject>>();
      this._objectToNode = new Dictionary<GameObject, SphereTreeNode<GameObject>>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static SceneTree()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      SceneTree._nonMeshObjectSize = 0.0001f;
    }
  }
}
