// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.CursorPickingManager
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.GameLoop;
using Mafi.Core.Ports.Io;
using Mafi.Core.Terrain.Trees;
using Mafi.Unity.Camera;
using Mafi.Unity.Entities;
using Mafi.Unity.Ports.Io;
using Mafi.Unity.Terrain;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class CursorPickingManager
  {
    private readonly TreeRenderer m_treeRenderer;
    private Option<GameObject> m_pickedGameObject;
    private bool m_pickedObjectValid;
    private readonly EntitiesRenderingManager m_entitiesRenderer;
    private readonly IoPortsRenderer m_portsRenderer;
    public readonly UnityEngine.Camera MainCamera;
    private RaycastHit[] m_hits;
    private ulong m_highlightId;

    public Option<GameObject> PickedGameObject
    {
      get
      {
        if (!this.m_pickedObjectValid)
          this.pickObject();
        return this.m_pickedGameObject;
      }
    }

    /// <summary>
    /// Last valid picked game object. This object may not be selected any more.
    /// </summary>
    public GameObject LastPickedGameObject { get; private set; }

    /// <summary>Collider of last valid picked game object.</summary>
    public Collider LastPickedCollider { get; private set; }

    /// <summary>
    /// Intersection coordinate of last valid picked game object.
    /// </summary>
    public Tile3f LastPickedTile => this.LastPickedCoord.ToTile3f();

    public Vector3 LastPickedCoord { get; private set; }

    public CursorPickingManager(
      IGameLoopEvents gameLoopEvents,
      EntitiesRenderingManager entitiesRenderer,
      TreeRenderer treeRenderer,
      IoPortsRenderer portsRenderer,
      CameraController cameraController)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_hits = new RaycastHit[3];
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_treeRenderer = treeRenderer;
      this.m_entitiesRenderer = entitiesRenderer.CheckNotNull<EntitiesRenderingManager>();
      this.m_portsRenderer = portsRenderer.CheckNotNull<IoPortsRenderer>();
      this.MainCamera = cameraController.Camera.CheckNotNull<UnityEngine.Camera>();
      gameLoopEvents.RenderUpdate.AddNonSaveable<CursorPickingManager>(this, new Action<GameTime>(this.renderUpdate));
    }

    private void renderUpdate(GameTime time) => this.m_pickedObjectValid = false;

    public void ClearPicked()
    {
      this.RemovePickedHighlight();
      if (this.m_pickedGameObject.IsNone)
        return;
      this.m_pickedGameObject = Option<GameObject>.None;
      this.m_pickedObjectValid = false;
    }

    public void RemovePickedHighlight()
    {
      if (this.m_highlightId <= 0UL)
        return;
      this.m_entitiesRenderer.RemoveHighlight(this.m_highlightId);
      this.m_highlightId = 0UL;
    }

    /// <summary>
    /// Returns requested <see cref="T:Mafi.Core.Entities.Entity" /> of an object under the mouse cursor if any.
    /// </summary>
    public Option<T> PickEntity<T>() where T : class, IRenderedEntity
    {
      T entity;
      return this.PickedGameObject.HasValue && this.m_entitiesRenderer.TryGetPickableEntityAs<T>(this.PickedGameObject.Value, out entity) ? (Option<T>) entity : Option<T>.None;
    }

    /// <summary>
    /// Returns requested <see cref="T:Mafi.Core.Terrain.Trees.TreeId" /> of an object under the mouse cursor if any.
    /// </summary>
    public TreeId? PickTree(Predicate<TreeMb> predicate)
    {
      if (this.PickedGameObject.IsNone)
        return new TreeId?();
      TreeMb treeMb;
      return this.m_treeRenderer.TryGetTreeMbFromGO(this.PickedGameObject.Value, out treeMb) && predicate(treeMb) ? new TreeId?(treeMb.TreeId) : new TreeId?();
    }

    public bool TryPickEntity<T>(out T entity) where T : class, IRenderedEntity
    {
      if (!this.PickedGameObject.IsNone)
        return this.m_entitiesRenderer.TryGetPickableEntityAs<T>(this.PickedGameObject.Value, out entity);
      entity = default (T);
      return false;
    }

    internal bool TryPickEntityPreferVehicle<T>(out T entity) where T : class, IRenderedEntity
    {
      if (!this.TryPickEntity<T>(out entity))
      {
        entity = default (T);
        return false;
      }
      if ((object) entity is Mafi.Core.Entities.Dynamic.Vehicle || !(entity is ILayoutEntity le))
        return true;
      int num = Physics.RaycastNonAlloc(this.MainCamera.ScreenPointToRay(Input.mousePosition), this.m_hits);
      for (int index = 0; index < num; ++index)
      {
        GameObject pickedGo = CursorPickingManager.processHit(this.m_hits[index]);
        T entity1;
        if ((bool) (UnityEngine.Object) pickedGo && !(pickedGo == this.m_pickedGameObject) && this.m_entitiesRenderer.TryGetPickableEntityAs<T>(pickedGo, out entity1) && entity1 is Mafi.Core.Entities.Dynamic.Vehicle vehicle && isVehicleInsideOfEntity(le, vehicle))
        {
          entity = entity1;
          break;
        }
      }
      return true;

      static bool isVehicleInsideOfEntity(ILayoutEntity le, Mafi.Core.Entities.Dynamic.Vehicle vehicle)
      {
        return le.Prototype.Layout.GetBoundingBox2i(le.Transform).ContainsTile(vehicle.GroundPositionTile2i);
      }
    }

    /// <summary>
    /// Returns requested <see cref="T:Mafi.Core.Entities.Entity" /> of an object under the mouse cursor if any.
    /// </summary>
    public Option<T> PickEntity<T>(Predicate<T> predicate) where T : class, IRenderedEntity
    {
      T entity;
      return this.PickedGameObject.IsNone || !this.m_entitiesRenderer.TryGetPickableEntityAs<T>(this.PickedGameObject.Value, out entity) || !predicate(entity) ? Option<T>.None : (Option<T>) entity;
    }

    /// <summary>
    /// Returns an instance of <see cref="T:Mafi.Core.Entities.IEntity" /> that is under the mouse cursor and fulfills given <paramref name="predicate" />. Object is additionally highlighted by the color returned from the <paramref name="predicate" /> regardless of whether it was picked or not.
    /// </summary>
    public Option<T> PickEntityAndSelect<T>(
      CursorPickingManager.EntityPredicateReturningColor<T> predicate)
      where T : class, IRenderedEntity
    {
      T entity;
      if (this.PickedGameObject.IsNone || !this.m_entitiesRenderer.TryGetPickableEntityAs<T>(this.PickedGameObject.Value, out entity))
        return Option<T>.None;
      ColorRgba color;
      bool flag = predicate(entity, out color);
      this.RemovePickedHighlight();
      if (color.IsNotEmpty)
        this.m_highlightId = this.m_entitiesRenderer.AddHighlight((IRenderedEntity) entity, color);
      return !flag ? Option<T>.None : (Option<T>) entity;
    }

    /// <summary>
    /// Returns requested <see cref="T:Mafi.Core.Ports.Io.IoPort" /> of an object under the mouse cursor that fulfills given <paramref name="predicate" /> and selects (highlights) the object with returned color if true.
    /// </summary>
    public Option<IoPort> PickPortAndSelect<T>(
      CursorPickingManager.PortPredicateReturningColor predicate)
    {
      if (this.PickedGameObject.IsNone)
        return (Option<IoPort>) Option.None;
      Option<IoPort> portForArrow = this.m_portsRenderer.GetPortForArrow<T>(this.PickedGameObject.Value);
      if (portForArrow.IsNone)
        return (Option<IoPort>) Option.None;
      ColorRgba color;
      return !predicate(portForArrow.Value, out color) ? (Option<IoPort>) Option.None : portForArrow;
    }

    /// <summary>
    /// Consider using <see cref="P:Mafi.Unity.InputControl.CursorPickingManager.PickedGameObject" /> which is cached.
    /// </summary>
    public bool TryPickGoUnderCursor(out GameObject gameObject)
    {
      return CursorPickingManager.TryPickGoUnderCursor(this.MainCamera, out gameObject, out RaycastHit _);
    }

    /// <summary>
    /// Consider using <see cref="P:Mafi.Unity.InputControl.CursorPickingManager.PickedGameObject" /> which is cached.
    /// </summary>
    public static bool TryPickGoUnderCursor(
      UnityEngine.Camera camera,
      out GameObject gameObject,
      out RaycastHit hit)
    {
      if (!Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
      {
        gameObject = (GameObject) null;
        return false;
      }
      gameObject = CursorPickingManager.processHit(hit);
      return true;
    }

    private static GameObject processHit(RaycastHit hit)
    {
      GameObject gameObject = hit.collider.gameObject;
      Assert.That<GameObject>(gameObject).IsValidUnityObject<GameObject>();
      Transform parent;
      for (; gameObject.HasTag(UnityTag.PiPa); gameObject = parent.gameObject)
      {
        parent = gameObject.transform.parent;
        if (!(bool) (UnityEngine.Object) gameObject.transform.parent)
        {
          Log.Warning(string.Format("Picked parent tag 'PiPa' on object '{0}' without parent.", (object) gameObject));
          break;
        }
      }
      return gameObject;
    }

    private void pickObject()
    {
      GameObject gameObject;
      RaycastHit hit;
      Option<GameObject> option = CursorPickingManager.TryPickGoUnderCursor(this.MainCamera, out gameObject, out hit) ? (Option<GameObject>) gameObject : Option<GameObject>.None;
      if (option == this.m_pickedGameObject)
      {
        if (option.HasValue)
        {
          this.LastPickedCollider = hit.collider;
          this.LastPickedCoord = hit.point;
        }
        this.m_pickedObjectValid = true;
      }
      else
      {
        this.ClearPicked();
        this.m_pickedObjectValid = true;
        if (option.IsNone)
          return;
        this.m_pickedGameObject = option;
        this.LastPickedGameObject = option.Value;
        this.LastPickedCollider = hit.collider;
        this.LastPickedCoord = hit.point;
      }
    }

    public delegate bool EntityPredicateReturningColor<T>(T entity, out ColorRgba color) where T : IEntity;

    public delegate bool PortPredicateReturningColor(IoPort port, out ColorRgba color);
  }
}
