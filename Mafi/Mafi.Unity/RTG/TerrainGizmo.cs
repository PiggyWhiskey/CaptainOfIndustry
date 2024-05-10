// Decompiled with JetBrains decompiler
// Type: RTG.TerrainGizmo
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class TerrainGizmo : GizmoBehaviour
  {
    private TerrainGizmo.TargetTypeFlags _targetTypeFlags;
    private bool _isSnapEnabled;
    private bool _isVisible;
    private Terrain _targetTerrain;
    private TerrainCollider _terrainCollider;
    private float[,] _terrainHeights;
    private float[,] _preChangeTerrainHeights;
    private float _radius;
    private AnimationCurve _elevationCurve;
    private TerrainGizmo.ObjectRotationData _objectRotationData;
    private SceneOverlapFilter _sceneOverlapFilter;
    private TerrainGizmo.Patch _editPatch;
    private List<Vector3> _modelRadiusCirclePoints;
    private List<Vector3> _radiusCirclePoints;
    private HashSet<GameObject> _affectedObjectsSet;
    private List<TerrainGizmoAffectedObject> _affectedObjects;
    private List<LocalTransformSnapshot> _preChangeTransformSnapshots;
    private Vector3 _preChangeGizmoPos;
    private List<GameObject> _objectsInRadius;
    private GizmoLineSlider3D _axisSlider;
    private GizmoCap3D _midCap;
    private TerrainGizmo.RadiusTick _leftRadiusTick;
    private TerrainGizmo.RadiusTick _rightRadiusTick;
    private TerrainGizmo.RadiusTick _backRadiusTick;
    private TerrainGizmo.RadiusTick _forwardRadiusTick;
    private GizmoSglAxisOffsetDrag3D _radiusDrag;
    private GizmoUniformScaleDrag3D _dummyDrag;
    private TerrainGizmoLookAndFeel _lookAndFeel;
    private TerrainGizmoLookAndFeel _sharedLookAndFeel;
    private TerrainGizmoSettings _settings;
    private TerrainGizmoSettings _sharedSettings;
    private TerrainGizmoHotkeys _hotkeys;
    private TerrainGizmoHotkeys _sharedHotkeys;
    private List<GameObject> _objectCollectRadius;

    public TerrainGizmoLookAndFeel LookAndFeel
    {
      get => this._sharedLookAndFeel != null ? this._sharedLookAndFeel : this._lookAndFeel;
    }

    public TerrainGizmoLookAndFeel SharedLookAndFeel
    {
      get => this._sharedLookAndFeel;
      set
      {
        this._sharedLookAndFeel = value;
        this.SetupSharedLookAndFeel();
      }
    }

    public TerrainGizmoSettings Settings
    {
      get => this._sharedSettings != null ? this._sharedSettings : this._settings;
    }

    public TerrainGizmoSettings SharedSettings
    {
      get => this._sharedSettings;
      set => this._sharedSettings = value;
    }

    public TerrainGizmoHotkeys Hotkeys
    {
      get => this._sharedHotkeys != null ? this._sharedHotkeys : this._hotkeys;
    }

    public TerrainGizmoHotkeys SharedHotkeys
    {
      get => this._sharedHotkeys;
      set => this._sharedHotkeys = value;
    }

    public Terrain TargetTerrain => this._targetTerrain;

    public float Radius
    {
      get => this._radius;
      set => this._radius = Mathf.Max(0.0001f, value);
    }

    public AnimationCurve ElevationCurve
    {
      get => this._elevationCurve;
      set
      {
        if (this.Gizmo.IsDragged)
          return;
        this._elevationCurve = value;
      }
    }

    public bool IsSnapEnabled => this._isSnapEnabled || this.Hotkeys.EnableSnapping.IsActive();

    public bool IsRotatingObjects => this.Hotkeys.RotateObjects.IsActive();

    public TerrainGizmo.TargetTypeFlags TargetTypes
    {
      get => this._targetTypeFlags;
      set
      {
        if (this.Gizmo.IsDragged)
          return;
        this._targetTypeFlags = value;
      }
    }

    public bool HasTerrainTarget
    {
      get => (this._targetTypeFlags & TerrainGizmo.TargetTypeFlags.Terrain) != 0;
    }

    public bool HasObjectsInRadiusTarget
    {
      get => (this._targetTypeFlags & TerrainGizmo.TargetTypeFlags.ObjectsInRadius) != 0;
    }

    public void SetSnapEnabled(bool enabled) => this._isSnapEnabled = enabled;

    public void SetTargetTerrain(Terrain terrain)
    {
      this._targetTerrain = terrain;
      if ((UnityEngine.Object) terrain != (UnityEngine.Object) null)
      {
        this._terrainCollider = this._targetTerrain.GetComponent<TerrainCollider>();
        this._terrainHeights = this._targetTerrain.terrainData.GetHeights(0, 0, this._targetTerrain.terrainData.heightmapResolution, this._targetTerrain.terrainData.heightmapResolution);
        this._preChangeTerrainHeights = this._terrainHeights.Clone() as float[,];
      }
      this.SetVisible(false);
    }

    public override void OnAttached()
    {
      this._axisSlider = new GizmoLineSlider3D(this.Gizmo, GizmoHandleId.AxisSlider, GizmoHandleId.AxisSliderCap);
      this._axisSlider.SetDragChannel(GizmoDragChannel.Offset);
      this._axisSlider.MapDirection(1, AxisSign.Positive);
      this._midCap = new GizmoCap3D(this.Gizmo, GizmoHandleId.MidSnapCap);
      this._midCap.HoverPriority3D.MakeHigherThan(this._axisSlider.HoverPriority3D);
      this._midCap.DragSession = (IGizmoDragSession) this._dummyDrag;
      this._leftRadiusTick = new TerrainGizmo.RadiusTick();
      this._leftRadiusTick.DragAxis = -Vector3.right;
      this._leftRadiusTick.Tick = new GizmoCap2D(this.Gizmo, GizmoHandleId.LeftRadiusTick);
      this._leftRadiusTick.Tick.DragSession = (IGizmoDragSession) this._radiusDrag;
      this._rightRadiusTick = new TerrainGizmo.RadiusTick();
      this._rightRadiusTick.DragAxis = Vector3.right;
      this._rightRadiusTick.Tick = new GizmoCap2D(this.Gizmo, GizmoHandleId.RightRadiusTick);
      this._rightRadiusTick.Tick.DragSession = (IGizmoDragSession) this._radiusDrag;
      this._backRadiusTick = new TerrainGizmo.RadiusTick();
      this._backRadiusTick.DragAxis = -Vector3.forward;
      this._backRadiusTick.Tick = new GizmoCap2D(this.Gizmo, GizmoHandleId.BackRadiusTick);
      this._backRadiusTick.Tick.DragSession = (IGizmoDragSession) this._radiusDrag;
      this._forwardRadiusTick = new TerrainGizmo.RadiusTick();
      this._forwardRadiusTick.DragAxis = Vector3.forward;
      this._forwardRadiusTick.Tick = new GizmoCap2D(this.Gizmo, GizmoHandleId.ForwardRadiusTick);
      this._forwardRadiusTick.Tick.DragSession = (IGizmoDragSession) this._radiusDrag;
      this._modelRadiusCirclePoints = PrimitiveFactory.Generate3DCircleBorderPoints(Vector3.zero, 1f, Vector3.right, Vector3.forward, 100);
      for (int index = 0; index < 100; ++index)
        this._radiusCirclePoints.Add(Vector3.zero);
      this.SetVisible(false);
      this.SetupSharedLookAndFeel();
      this._sceneOverlapFilter.AllowedObjectTypes.Add(GameObjectType.Mesh);
      this._sceneOverlapFilter.AllowedObjectTypes.Add(GameObjectType.Light);
      this._sceneOverlapFilter.AllowedObjectTypes.Add(GameObjectType.ParticleSystem);
      this._elevationCurve.AddKey(new Keyframe(0.0f, 0.0f));
      this._elevationCurve.AddKey(new Keyframe(1f, 1f));
      MonoSingleton<RTUndoRedo>.Get.UndoEnd += new UndoEndHandler(this.OnUndoRedoPerformed);
      MonoSingleton<RTUndoRedo>.Get.RedoEnd += new RedoEndHandler(this.OnUndoRedoPerformed);
    }

    public override void OnDisabled()
    {
      MonoSingleton<RTUndoRedo>.Get.UndoEnd -= new UndoEndHandler(this.OnUndoRedoPerformed);
      MonoSingleton<RTUndoRedo>.Get.RedoEnd -= new RedoEndHandler(this.OnUndoRedoPerformed);
    }

    public override void OnEnabled()
    {
      MonoSingleton<RTUndoRedo>.Get.UndoEnd += new UndoEndHandler(this.OnUndoRedoPerformed);
      MonoSingleton<RTUndoRedo>.Get.RedoEnd += new RedoEndHandler(this.OnUndoRedoPerformed);
    }

    public override void OnGizmoUpdateBegin()
    {
      if (!this.IsTargetReady())
        return;
      if (this.IsRotatingObjects)
      {
        if (!this._objectRotationData.RotatingObjects)
        {
          this.CollectObjectsInRadius(this._objectsInRadius);
          this._objectRotationData.RotatingObjects = true;
          this._objectRotationData.GameObjects = this._objectsInRadius;
          this._objectRotationData.PreSnapshots = LocalTransformSnapshot.GetSnapshotCollection((IEnumerable<GameObject>) this._objectRotationData.GameObjects);
        }
        float angle = MonoSingleton<RTInputDevice>.Get.Device.GetFrameDelta().x * this.Settings.RotationSensitivity;
        Vector3 up = this._targetTerrain.transform.up;
        foreach (GameObject gameObject in this._objectRotationData.GameObjects)
        {
          if (this.CanObjectBeRotated(gameObject))
            gameObject.transform.Rotate(up, angle);
        }
      }
      else
      {
        if (this._objectRotationData.RotatingObjects)
        {
          this._objectRotationData.RotatingObjects = false;
          new TerrainGizmoObjectTransformsChangedAction(this._objectRotationData.PreSnapshots, LocalTransformSnapshot.GetSnapshotCollection((IEnumerable<GameObject>) this._objectRotationData.GameObjects)).Execute();
          this._objectRotationData.GameObjects.Clear();
          this._objectRotationData.PreSnapshots.Clear();
        }
        if (MonoSingleton<RTInputDevice>.Get.Device.WasButtonPressedInCurrentFrame(0) && !this.Gizmo.IsHovered)
          this.SnapGizmoToTerrain();
      }
      this._axisSlider.Settings.OffsetSnapStep = this.Settings.OffsetSnapStep;
      this._axisSlider.SetSnapEnabled(this.IsSnapEnabled);
      this._radiusDrag.IsSnapEnabled = this.IsSnapEnabled;
      this.UpdateTicks();
    }

    public override bool OnGizmoCanBeginDrag(int handleId)
    {
      return this.IsTargetReady() && this._isVisible && !this.IsRotatingObjects;
    }

    public override void OnGizmoAttemptHandleDragBegin(int handleId)
    {
      this._affectedObjectsSet.Clear();
      this._affectedObjects.Clear();
      Vector3 circleMinExtents = this.GetRadiusCircleMinExtents();
      Vector3 circleMaxExtents = this.GetRadiusCircleMaxExtents();
      Vector3 vector3_1 = circleMinExtents - this._targetTerrain.transform.position;
      Vector3 vector3_2 = circleMaxExtents - this._targetTerrain.transform.position;
      TerrainData terrainData = this._targetTerrain.terrainData;
      float num1 = vector3_1.x / terrainData.size.x;
      float num2 = vector3_2.x / terrainData.size.x;
      float num3 = vector3_1.z / terrainData.size.z;
      float num4 = vector3_2.z / terrainData.size.z;
      this._editPatch.MinCol = Mathf.Clamp(Mathf.FloorToInt(num1 * (float) terrainData.heightmapResolution), 0, terrainData.heightmapResolution - 1);
      this._editPatch.MaxCol = Mathf.Clamp(Mathf.FloorToInt(num2 * (float) terrainData.heightmapResolution), 0, terrainData.heightmapResolution - 1);
      this._editPatch.MinDepth = Mathf.Clamp(Mathf.FloorToInt(num3 * (float) terrainData.heightmapResolution), 0, terrainData.heightmapResolution - 1);
      this._editPatch.MaxDepth = Mathf.Clamp(Mathf.FloorToInt(num4 * (float) terrainData.heightmapResolution), 0, terrainData.heightmapResolution - 1);
      TerrainGizmo.RadiusTick tickFromHandleId = this.GetRadiusTickFromHandleId(handleId);
      if (tickFromHandleId != null)
        this._radiusDrag.SetWorkData(new GizmoSglAxisOffsetDrag3D.WorkData()
        {
          Axis = tickFromHandleId.DragAxis,
          DragOrigin = tickFromHandleId.WorldPosition,
          SnapStep = this.Settings.RadiusSnapStep
        });
      if (handleId != this._midCap.HandleId)
        return;
      this._preChangeGizmoPos = this.Gizmo.Transform.Position3D;
      this._dummyDrag.SetWorkData(new GizmoUniformScaleDrag3D.WorkData()
      {
        CameraRight = this.Gizmo.GetWorkCamera().transform.right,
        CameraUp = this.Gizmo.GetWorkCamera().transform.up,
        DragOrigin = this.Gizmo.Transform.Position3D,
        SnapStep = 0.0f
      });
      this.CollectObjectsInRadius(this._objectsInRadius);
      this._preChangeTransformSnapshots = LocalTransformSnapshot.GetSnapshotCollection((IEnumerable<GameObject>) this._objectsInRadius);
    }

    public override void OnGizmoDragUpdate(int handleId)
    {
      if (handleId == this._axisSlider.HandleId || handleId == this._axisSlider.Cap3DHandleId)
      {
        if (this.HasTerrainTarget)
          this.OffsetTerrainPatch(this.Gizmo.RelativeDragOffset.y);
        if (!this.HasObjectsInRadiusTarget)
          return;
        this.OffsetObjectsInRadius(this.Gizmo.RelativeDragOffset.y);
      }
      else if (handleId == this._midCap.HandleId)
      {
        this.DragObjectsWithMidCap();
      }
      else
      {
        TerrainGizmo.RadiusTick tickFromHandleId = this.GetRadiusTickFromHandleId(handleId);
        if (tickFromHandleId == null)
          return;
        this._radius += Vector3.Dot(this.Gizmo.RelativeDragOffset, tickFromHandleId.DragAxis);
        this._radius = Mathf.Max(0.0001f, this._radius);
      }
    }

    public override void OnGizmoDragEnd(int handleId)
    {
      if (handleId == this._midCap.HandleId)
      {
        new TerrainGizmoHorizontalOffsetDragEndAction(this, this._preChangeGizmoPos, this._preChangeTransformSnapshots, LocalTransformSnapshot.GetSnapshotCollection((IEnumerable<GameObject>) this._objectsInRadius)).Execute();
      }
      else
      {
        float terrainYpos = this.GetTerrainYPos();
        Vector3 position3D = this.Gizmo.Transform.Position3D;
        if ((double) this.Gizmo.Transform.Position3D.y < (double) terrainYpos)
        {
          position3D.y = terrainYpos;
          this.Gizmo.Transform.Position3D = position3D;
        }
        foreach (TerrainGizmoAffectedObject affectedObject in this._affectedObjects)
          affectedObject.NewPosition = affectedObject.AffectedObject.transform.position;
        new TerrainGizmoVerticalOffsetDragEndAction(this._targetTerrain, this.HasTerrainTarget ? this._preChangeTerrainHeights : (float[,]) null, this.HasTerrainTarget ? this._terrainHeights : (float[,]) null, this._affectedObjects).Execute();
        Array.Copy((Array) this._terrainHeights, (Array) this._preChangeTerrainHeights, this._terrainHeights.GetLength(0) * this._preChangeTerrainHeights.GetLength(1));
      }
      this._affectedObjectsSet.Clear();
      this._affectedObjects.Clear();
      this.ProjectGizmoOnTerrain();
    }

    public override void OnGizmoRender(Camera camera)
    {
      if (!this.IsTargetReady())
        return;
      if (MonoSingleton<RTGizmosEngine>.Get.NumRenderCameras > 1)
        this.UpdateTicks();
      if (this._isVisible)
      {
        GizmoLineMaterial get = Singleton<GizmoLineMaterial>.Get;
        get.ResetValuesToSensibleDefaults();
        get.SetColor(this.LookAndFeel.RadiusCircleColor);
        get.SetPass(0);
        float y = this._targetTerrain.transform.position.y;
        Vector3 position3D = this.Gizmo.Transform.Position3D;
        int count = this._modelRadiusCirclePoints.Count;
        for (int index = 0; index < count; ++index)
        {
          Vector3 vector3 = (this._modelRadiusCirclePoints[index] * this._radius + position3D) with
          {
            y = y + this._targetTerrain.SampleHeight(this._radiusCirclePoints[index])
          };
          this._radiusCirclePoints[index] = vector3;
        }
        GLRenderer.DrawLines3D(this._radiusCirclePoints);
      }
      this._axisSlider.Render(camera);
      this._midCap.Render(camera);
      this._leftRadiusTick.Tick.Render(camera);
      this._rightRadiusTick.Tick.Render(camera);
      this._backRadiusTick.Tick.Render(camera);
      this._forwardRadiusTick.Tick.Render(camera);
    }

    private bool CanObjectBeMovedHrz(GameObject go)
    {
      return (this.Settings.ObjectHrzMoveLayerMask >> go.layer & 1) != 0 && !this.Settings.IsTagIgnoredForHrzMove(go.tag);
    }

    private bool CanObjectBeMovedVert(GameObject go)
    {
      return (this.Settings.ObjectVertMoveLayerMask >> go.layer & 1) != 0 && !this.Settings.IsTagIgnoredForVertMove(go.tag);
    }

    private bool CanObjectBeRotated(GameObject go)
    {
      return (this.Settings.ObjectRotationLayerMask >> go.layer & 1) != 0 && !this.Settings.IsTagIgnoredForRotation(go.tag);
    }

    private TerrainGizmo.RadiusTick GetRadiusTickFromHandleId(int handleId)
    {
      TerrainGizmo.RadiusTick tickFromHandleId = (TerrainGizmo.RadiusTick) null;
      if (handleId == this._leftRadiusTick.Tick.HandleId)
        tickFromHandleId = this._leftRadiusTick;
      else if (handleId == this._rightRadiusTick.Tick.HandleId)
        tickFromHandleId = this._rightRadiusTick;
      else if (handleId == this._forwardRadiusTick.Tick.HandleId)
        tickFromHandleId = this._forwardRadiusTick;
      else if (handleId == this._backRadiusTick.Tick.HandleId)
        tickFromHandleId = this._backRadiusTick;
      return tickFromHandleId;
    }

    private void OnUndoRedoPerformed(IUndoRedoAction action)
    {
      this.ProjectGizmoOnTerrain();
      this._terrainHeights = this._targetTerrain.terrainData.GetHeights(0, 0, this._targetTerrain.terrainData.heightmapResolution, this._targetTerrain.terrainData.heightmapResolution);
      Array.Copy((Array) this._terrainHeights, (Array) this._preChangeTerrainHeights, this._terrainHeights.GetLength(0) * this._preChangeTerrainHeights.GetLength(1));
    }

    private float GetTerrainYPos() => this._targetTerrain.transform.position.y;

    private void ProjectGizmoOnTerrain()
    {
      Vector3 position3D = this.Gizmo.Transform.Position3D;
      position3D.y = this._targetTerrain.SampleHeight(position3D) + this.GetTerrainYPos();
      this.Gizmo.Transform.Position3D = position3D;
    }

    private void DragObjectsWithMidCap()
    {
      RaycastHit hitInfo;
      if (!this._terrainCollider.Raycast(MonoSingleton<RTInputDevice>.Get.Device.GetRay(this.Gizmo.GetWorkCamera()), out hitInfo, float.MaxValue))
        return;
      Vector3 vector3 = hitInfo.point - this._gizmo.Transform.Position3D;
      this._gizmo.Transform.Position3D += vector3;
      float y = this._targetTerrain.transform.position.y;
      foreach (GameObject objectsInRadiu in this._objectsInRadius)
      {
        if (this.CanObjectBeMovedHrz(objectsInRadiu))
        {
          Vector3 position = objectsInRadiu.transform.position;
          float num1 = this._targetTerrain.SampleHeight(position);
          float num2 = position.y - (y + num1);
          Vector3 worldPosition = position + vector3;
          float num3 = this._targetTerrain.SampleHeight(worldPosition);
          worldPosition.y = y + num3 + num2;
          objectsInRadiu.transform.position = worldPosition;
        }
      }
    }

    private void OffsetTerrainPatch(float offset)
    {
      float num1 = offset / this._targetTerrain.terrainData.heightmapScale.y;
      float num2 = this._targetTerrain.terrainData.size.x / (float) this._targetTerrain.terrainData.heightmapResolution;
      float num3 = this._targetTerrain.terrainData.size.x / (float) this._targetTerrain.terrainData.heightmapResolution;
      Vector3 position = this._targetTerrain.transform.position;
      Vector3 zero = Vector3.zero with { y = position.y };
      Vector3 position3D = this.Gizmo.Transform.Position3D with
      {
        y = zero.y
      };
      for (int minDepth = this._editPatch.MinDepth; minDepth <= this._editPatch.MaxDepth; ++minDepth)
      {
        for (int minCol = this._editPatch.MinCol; minCol <= this._editPatch.MaxCol; ++minCol)
        {
          zero.x = position.x + (float) minCol * num2;
          zero.z = position.z + (float) minDepth * num3;
          float magnitude = (zero - position3D).magnitude;
          if ((double) magnitude <= (double) this._radius)
          {
            float num4 = this._elevationCurve.Evaluate(Mathf.Max((float) (1.0 - (double) magnitude / (double) this._radius), 0.0f));
            this._terrainHeights[minDepth, minCol] += num1 * num4;
            this._terrainHeights[minDepth, minCol] = Mathf.Max(this._terrainHeights[minDepth, minCol], 0.0f);
          }
        }
      }
      this._targetTerrain.terrainData.SetHeights(0, 0, this._terrainHeights);
    }

    private void OffsetObjectsInRadius(float offset)
    {
      float terrainYpos = this.GetTerrainYPos();
      Vector3 position3D = this.Gizmo.Transform.Position3D;
      if (this._objectsInRadius.Count == 0)
        return;
      position3D.y = terrainYpos;
      foreach (GameObject objectsInRadiu in this._objectsInRadius)
      {
        if (this.CanObjectBeMovedVert(objectsInRadiu))
        {
          Transform transform = objectsInRadiu.transform;
          float magnitude = (transform.position with
          {
            y = terrainYpos
          } - position3D).magnitude;
          if ((double) magnitude <= (double) this._radius)
          {
            float num = this._elevationCurve.Evaluate(Mathf.Max((float) (1.0 - (double) magnitude / (double) this._radius), 0.0f));
            Vector3 vector3 = transform.position + Vector3.up * offset * num;
            if ((double) vector3.y < (double) terrainYpos)
              vector3.y = terrainYpos;
            if (!this._affectedObjectsSet.Contains(objectsInRadiu))
            {
              this._affectedObjectsSet.Add(objectsInRadiu);
              this._affectedObjects.Add(new TerrainGizmoAffectedObject()
              {
                AffectedObject = objectsInRadiu,
                OriginalObjectPos = transform.position
              });
            }
            transform.position = vector3;
          }
        }
      }
    }

    private void CollectObjectsInRadius(List<GameObject> objectsInRadius)
    {
      objectsInRadius.Clear();
      OBB obb = new OBB();
      obb.Size = new Vector3(this._radius, this._targetTerrain.terrainData.size.y, this._radius) * 2f;
      Vector3 position3D = this.Gizmo.Transform.Position3D;
      obb.Center = position3D;
      MonoSingleton<RTScene>.Get.OverlapBox(obb, this._sceneOverlapFilter, this._objectCollectRadius);
      GameObjectEx.FilterParentsOnly((IEnumerable<GameObject>) this._objectCollectRadius, objectsInRadius);
      objectsInRadius.RemoveAll((Predicate<GameObject>) (item => !this.IsObjectInRadius(item)));
    }

    private bool IsObjectInRadius(GameObject gameObject)
    {
      return (double) (this.Gizmo.Transform.Position3D with
      {
        y = 0.0f
      } - gameObject.transform.position with { y = 0.0f }).magnitude <= (double) this._radius;
    }

    private void UpdateTicks()
    {
      Camera workCamera = this.Gizmo.GetWorkCamera();
      float terrainYpos = this.GetTerrainYPos();
      Vector3 position3D = this.Gizmo.Transform.Position3D;
      Vector3 vector3_1 = position3D - Vector3.right * this._radius;
      vector3_1.y = terrainYpos + this._targetTerrain.SampleHeight(vector3_1);
      this._leftRadiusTick.Tick.Position = (Vector2) workCamera.WorldToScreenPoint(vector3_1);
      this._leftRadiusTick.WorldPosition = vector3_1;
      Vector3 vector3_2 = position3D + Vector3.right * this._radius;
      vector3_2.y = terrainYpos + this._targetTerrain.SampleHeight(vector3_2);
      this._rightRadiusTick.Tick.Position = (Vector2) workCamera.WorldToScreenPoint(vector3_2);
      this._rightRadiusTick.WorldPosition = vector3_2;
      Vector3 vector3_3 = position3D - Vector3.forward * this._radius;
      vector3_3.y = terrainYpos + this._targetTerrain.SampleHeight(vector3_3);
      this._backRadiusTick.Tick.Position = (Vector2) workCamera.WorldToScreenPoint(vector3_3);
      this._backRadiusTick.WorldPosition = vector3_3;
      Vector3 vector3_4 = position3D + Vector3.forward * this._radius;
      vector3_4.y = terrainYpos + this._targetTerrain.SampleHeight(vector3_4);
      this._forwardRadiusTick.Tick.Position = (Vector2) workCamera.WorldToScreenPoint(vector3_4);
      this._forwardRadiusTick.WorldPosition = vector3_4;
    }

    private Vector3 GetRadiusCircleMinExtents()
    {
      return this.Gizmo.Transform.Position3D - Vector3.right * this._radius - Vector3.forward * this._radius;
    }

    private Vector3 GetRadiusCircleMaxExtents()
    {
      return this.Gizmo.Transform.Position3D + Vector3.right * this._radius + Vector3.forward * this._radius;
    }

    private void SnapGizmoToTerrain()
    {
      RaycastHit hitInfo;
      if (!this._terrainCollider.Raycast(MonoSingleton<RTInputDevice>.Get.Device.GetRay(this.Gizmo.GetWorkCamera()), out hitInfo, float.MaxValue))
        return;
      this._gizmo.Transform.Position3D = hitInfo.point;
      this.SetVisible(true);
    }

    private bool IsTargetReady()
    {
      return (UnityEngine.Object) this._targetTerrain != (UnityEngine.Object) null && (UnityEngine.Object) this._terrainCollider != (UnityEngine.Object) null;
    }

    private void SetupSharedLookAndFeel()
    {
      this.LookAndFeel.ConnectAxisSliderLookAndFeel(this._axisSlider);
      this.LookAndFeel.ConnectMidCapLookAndFeel(this._midCap);
      this.LookAndFeel.ConnectRadiusTickLookAndFeel(this._leftRadiusTick.Tick);
      this.LookAndFeel.ConnectRadiusTickLookAndFeel(this._rightRadiusTick.Tick);
      this.LookAndFeel.ConnectRadiusTickLookAndFeel(this._backRadiusTick.Tick);
      this.LookAndFeel.ConnectRadiusTickLookAndFeel(this._forwardRadiusTick.Tick);
    }

    private void SetVisible(bool visible)
    {
      this._axisSlider.SetVisible(visible);
      this._axisSlider.Set3DCapVisible(visible);
      this._midCap.SetVisible(visible);
      this._leftRadiusTick.Tick.SetVisible(visible);
      this._rightRadiusTick.Tick.SetVisible(visible);
      this._backRadiusTick.Tick.SetVisible(visible);
      this._forwardRadiusTick.Tick.SetVisible(visible);
      this._isVisible = visible;
    }

    public TerrainGizmo()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._targetTypeFlags = TerrainGizmo.TargetTypeFlags.All;
      this._radius = 5f;
      this._elevationCurve = new AnimationCurve();
      this._objectRotationData = new TerrainGizmo.ObjectRotationData();
      this._sceneOverlapFilter = new SceneOverlapFilter();
      this._modelRadiusCirclePoints = new List<Vector3>();
      this._radiusCirclePoints = new List<Vector3>();
      this._affectedObjectsSet = new HashSet<GameObject>();
      this._affectedObjects = new List<TerrainGizmoAffectedObject>();
      this._objectsInRadius = new List<GameObject>();
      this._radiusDrag = new GizmoSglAxisOffsetDrag3D();
      this._dummyDrag = new GizmoUniformScaleDrag3D();
      this._lookAndFeel = new TerrainGizmoLookAndFeel();
      this._settings = new TerrainGizmoSettings();
      this._hotkeys = new TerrainGizmoHotkeys();
      this._objectCollectRadius = new List<GameObject>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    [Flags]
    public enum TargetTypeFlags
    {
      Terrain = 1,
      ObjectsInRadius = 2,
      All = ObjectsInRadius | Terrain, // 0x00000003
    }

    private class RadiusTick
    {
      public GizmoCap2D Tick;
      public Vector3 DragAxis;
      public Vector3 WorldPosition;

      public RadiusTick()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }

    private class ObjectRotationData
    {
      public bool RotatingObjects;
      public List<GameObject> GameObjects;
      public List<LocalTransformSnapshot> PreSnapshots;

      public ObjectRotationData()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.GameObjects = new List<GameObject>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }

    private struct Patch
    {
      public int MinCol;
      public int MaxCol;
      public int MinDepth;
      public int MaxDepth;

      public void Clamp(int heightmapRes)
      {
        this.MinCol = Mathf.Clamp(this.MinCol, 0, heightmapRes - 1);
        this.MaxCol = Mathf.Clamp(this.MaxCol, 0, heightmapRes - 1);
        this.MinDepth = Mathf.Clamp(this.MinDepth, 0, heightmapRes - 1);
        this.MaxDepth = Mathf.Clamp(this.MaxDepth, 0, heightmapRes - 1);
      }
    }
  }
}
