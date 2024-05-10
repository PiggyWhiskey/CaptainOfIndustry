// Decompiled with JetBrains decompiler
// Type: RTG.RTScene
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class RTScene : MonoSingleton<RTScene>
  {
    [SerializeField]
    private SceneSettings _settings;
    private List<IHoverableSceneEntityContainer> _hoverableSceneEntityContainers;
    private SceneTree _sceneTree;
    private HashSet<GameObject> _ignoredRootObjects;
    private List<GameObject> _childrenAndSelfBuffer;
    private List<GameObject> _rootGameObjectsBuffer;
    private List<GameObjectRayHit> _objectHitBuffer;

    public SceneSettings Settings => this._settings;

    public void SetRootObjectIgnored(GameObject root, bool ignored)
    {
      if (this.Settings.PhysicsMode != ScenePhysicsMode.RTG)
        return;
      if (ignored)
        this._ignoredRootObjects.Add(root);
      else
        this._ignoredRootObjects.Remove(root);
    }

    public void OnGameObjectWillBeDestroyed(GameObject gameObject)
    {
      if (this.Settings.PhysicsMode != ScenePhysicsMode.RTG || this._ignoredRootObjects.Contains(gameObject))
        return;
      gameObject.GetAllChildrenAndSelf(this._childrenAndSelfBuffer);
      int count = this._childrenAndSelfBuffer.Count;
      for (int index = 0; index < count; ++index)
        this._sceneTree.UnregisterObject(this._childrenAndSelfBuffer[index]);
    }

    public AABB CalculateBounds()
    {
      List<GameObject> rootGameObjects = new List<GameObject>(Mathf.Max(10, SceneManager.GetActiveScene().rootCount));
      SceneManager.GetActiveScene().GetRootGameObjects(rootGameObjects);
      ObjectBounds.QueryConfig queryConfig = new ObjectBounds.QueryConfig();
      queryConfig.NoVolumeSize = Vector3.zero;
      queryConfig.ObjectTypes = GameObjectType.Mesh | GameObjectType.Sprite;
      AABB bounds = new AABB();
      foreach (GameObject gameObject1 in rootGameObjects)
      {
        foreach (GameObject gameObject2 in gameObject1.GetAllChildrenAndSelf())
        {
          AABB aabb = ObjectBounds.CalcWorldAABB(gameObject2, queryConfig);
          if (aabb.IsValid)
          {
            if (bounds.IsValid)
              bounds.Encapsulate(aabb);
            else
              bounds = aabb;
          }
        }
      }
      return bounds;
    }

    public bool IsAnySceneEntityHovered()
    {
      foreach (IHoverableSceneEntityContainer sceneEntityContainer in this._hoverableSceneEntityContainers)
      {
        if (sceneEntityContainer.HasHoveredSceneEntity)
          return true;
      }
      return this.IsAnyUIElementHovered();
    }

    public void RegisterHoverableSceneEntityContainer(IHoverableSceneEntityContainer container)
    {
      if (this._hoverableSceneEntityContainers.Contains(container))
        return;
      this._hoverableSceneEntityContainers.Add(container);
    }

    public bool IsAnyUIElementHovered() => this.GetHoveredUIElements().Count != 0;

    public List<RaycastResult> GetHoveredUIElements()
    {
      if ((UnityEngine.Object) EventSystem.current == (UnityEngine.Object) null)
        return new List<RaycastResult>();
      IInputDevice device = MonoSingleton<RTInputDevice>.Get.Device;
      if (!device.HasPointer())
        return new List<RaycastResult>();
      Vector2 positionYaxisUp = (Vector2) device.GetPositionYAxisUp();
      PointerEventData eventData = new PointerEventData(EventSystem.current);
      eventData.position = new Vector2(positionYaxisUp.x, positionYaxisUp.y);
      List<RaycastResult> raycastResults = new List<RaycastResult>();
      EventSystem.current.RaycastAll(eventData, raycastResults);
      raycastResults.RemoveAll((Predicate<RaycastResult>) (item => (UnityEngine.Object) item.gameObject.GetComponent<RectTransform>() == (UnityEngine.Object) null));
      return raycastResults;
    }

    public GameObject[] GetSceneObjects() => UnityEngine.Object.FindObjectsOfType<GameObject>();

    public bool OverlapBox(OBB obb, List<GameObject> gameObjects)
    {
      if (this.Settings.PhysicsMode != ScenePhysicsMode.UnityColliders)
        return this._sceneTree.OverlapBox(obb, gameObjects);
      gameObjects.Clear();
      foreach (Collider collider in Physics.OverlapBox(obb.Center, obb.Extents, obb.Rotation))
        gameObjects.Add(collider.gameObject);
      AABB aabb = new AABB((IEnumerable<Vector3>) new Plane(Vector3.forward, 0.0f).ProjectAllPoints(obb.GetCornerPoints()));
      return gameObjects.Count != 0;
    }

    public bool OverlapBox(OBB obb, SceneOverlapFilter overlapFilter, List<GameObject> gameObjects)
    {
      this.OverlapBox(obb, gameObjects);
      overlapFilter.FilterOverlaps(gameObjects);
      return gameObjects.Count != 0;
    }

    public SceneRaycastHit Raycast(
      Ray ray,
      SceneRaycastPrecision rtRaycastPrecision,
      SceneRaycastFilter raycastFilter)
    {
      this.RaycastAllObjectsSorted(ray, rtRaycastPrecision, raycastFilter, this._objectHitBuffer);
      return new SceneRaycastHit(this._objectHitBuffer.Count != 0 ? this._objectHitBuffer[0] : (GameObjectRayHit) null, this.RaycastSceneGridIfVisible(ray));
    }

    public bool RaycastAllObjects(
      Ray ray,
      SceneRaycastPrecision rtRaycastPrecision,
      List<GameObjectRayHit> hits)
    {
      if (this.Settings.PhysicsMode != ScenePhysicsMode.UnityColliders)
        return this._sceneTree.RaycastAll(ray, rtRaycastPrecision, hits);
      hits.Clear();
      RaycastHit[] hits3D = Physics.RaycastAll(ray, float.MaxValue);
      GameObjectRayHit.Store(ray, (IEnumerable<RaycastHit>) hits3D, hits);
      return hits.Count != 0;
    }

    public bool RaycastAllObjectsSorted(
      Ray ray,
      SceneRaycastPrecision raycastPresicion,
      List<GameObjectRayHit> hits)
    {
      bool flag = this.RaycastAllObjects(ray, raycastPresicion, hits);
      GameObjectRayHit.SortByHitDistance(hits);
      return flag;
    }

    public bool RaycastAllObjectsSorted(
      Ray ray,
      SceneRaycastPrecision rtRaycastPrecision,
      SceneRaycastFilter raycastFilter,
      List<GameObjectRayHit> hits)
    {
      hits.Clear();
      if (raycastFilter != null && raycastFilter.AllowedObjectTypes.Count == 0)
        return false;
      this.RaycastAllObjectsSorted(ray, rtRaycastPrecision, hits);
      raycastFilter?.FilterHits(hits);
      return hits.Count != 0;
    }

    public GameObjectRayHit RaycastMeshObject(Ray ray, GameObject meshObject)
    {
      if (this.Settings.PhysicsMode != ScenePhysicsMode.UnityColliders)
        return this._sceneTree.RaycastMeshObject(ray, meshObject);
      Collider collider = (Collider) null;
      MeshCollider component = meshObject.GetComponent<MeshCollider>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        collider = (Collider) component;
      if ((UnityEngine.Object) collider == (UnityEngine.Object) null)
        collider = meshObject.GetComponent<Collider>();
      RaycastHit hitInfo;
      return (UnityEngine.Object) collider != (UnityEngine.Object) null && collider.Raycast(ray, out hitInfo, float.MaxValue) ? new GameObjectRayHit(ray, hitInfo) : (GameObjectRayHit) null;
    }

    public GameObjectRayHit RaycastMeshObjectReverseIfFail(Ray ray, GameObject meshObject)
    {
      return this.RaycastMeshObject(ray, meshObject) ?? this.RaycastMeshObject(new Ray(ray.origin, -ray.direction), meshObject);
    }

    public GameObjectRayHit RaycastSpriteObject(Ray ray, GameObject spriteObject)
    {
      return this._sceneTree.RaycastSpriteObject(ray, spriteObject);
    }

    public GameObjectRayHit RaycastTerrainObject(Ray ray, GameObject terrainObject)
    {
      TerrainCollider component = terrainObject.GetComponent<TerrainCollider>();
      if ((UnityEngine.Object) component == (UnityEngine.Object) null)
        return (GameObjectRayHit) null;
      RaycastHit hitInfo;
      return component.Raycast(ray, out hitInfo, float.MaxValue) ? new GameObjectRayHit(ray, hitInfo) : (GameObjectRayHit) null;
    }

    public GameObjectRayHit RaycastTerrainObject(
      Ray ray,
      GameObject terrainObject,
      TerrainCollider terrainCollider)
    {
      RaycastHit hitInfo;
      return terrainCollider.Raycast(ray, out hitInfo, float.MaxValue) ? new GameObjectRayHit(ray, hitInfo) : (GameObjectRayHit) null;
    }

    public GameObjectRayHit RaycastTerrainObjectReverseIfFail(Ray ray, GameObject terrainObject)
    {
      return this.RaycastTerrainObject(ray, terrainObject) ?? this.RaycastTerrainObject(new Ray(ray.origin, -ray.direction), terrainObject);
    }

    public XZGridRayHit RaycastSceneGridIfVisible(Ray ray)
    {
      if (!MonoSingleton<RTSceneGrid>.Get.Settings.IsVisible)
        return (XZGridRayHit) null;
      float t;
      if (!MonoSingleton<RTSceneGrid>.Get.Raycast(ray, out t))
        return (XZGridRayHit) null;
      XZGridCell hitCell = MonoSingleton<RTSceneGrid>.Get.CellFromWorldPoint(ray.GetPoint(t));
      return new XZGridRayHit(ray, hitCell, t);
    }

    public void Update_SystemCall()
    {
      if (this.Settings.PhysicsMode != ScenePhysicsMode.RTG)
        return;
      Scene activeScene = SceneManager.GetActiveScene();
      int rootCount = activeScene.rootCount;
      if (this._rootGameObjectsBuffer.Capacity <= rootCount)
        this._rootGameObjectsBuffer.Capacity = rootCount + 100;
      activeScene.GetRootGameObjects(this._rootGameObjectsBuffer);
      for (int index1 = 0; index1 < rootCount; ++index1)
      {
        GameObject gameObject1 = this._rootGameObjectsBuffer[index1];
        if (!this._ignoredRootObjects.Contains(gameObject1))
        {
          gameObject1.GetAllChildrenAndSelf(this._childrenAndSelfBuffer);
          int count = this._childrenAndSelfBuffer.Count;
          for (int index2 = 0; index2 < count; ++index2)
          {
            GameObject gameObject2 = this._childrenAndSelfBuffer[index2];
            if (!this._sceneTree.IsObjectRegistered(gameObject2))
            {
              this._sceneTree.RegisterObject(gameObject2);
            }
            else
            {
              Transform transform = gameObject2.transform;
              if (transform.hasChanged)
              {
                this._sceneTree.OnObjectTransformChanged(transform);
                transform.hasChanged = false;
              }
            }
          }
        }
      }
    }

    public RTScene()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._settings = new SceneSettings();
      this._hoverableSceneEntityContainers = new List<IHoverableSceneEntityContainer>();
      this._sceneTree = new SceneTree();
      this._ignoredRootObjects = new HashSet<GameObject>();
      this._childrenAndSelfBuffer = new List<GameObject>(100);
      this._rootGameObjectsBuffer = new List<GameObject>();
      this._objectHitBuffer = new List<GameObjectRayHit>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
