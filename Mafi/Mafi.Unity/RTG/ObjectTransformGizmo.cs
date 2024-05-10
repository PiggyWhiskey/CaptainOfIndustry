// Decompiled with JetBrains decompiler
// Type: RTG.ObjectTransformGizmo
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
  [Serializable]
  public class ObjectTransformGizmo : GizmoBehaviour
  {
    private ObjectTransformGizmo.TargetObjectMode _targetObjectMode;
    private ObjectTransformGizmo.Channels _transformChannelFlags;
    private IEnumerable<GameObject> _targetObjects;
    private GameObject _targetPivotObject;
    private List<LocalTransformSnapshot> _preTransformSnapshots;
    private List<GameObject> _transformableParents;
    private AABB _targetGroupAABBOnDragBegin;
    private GizmoSpace _transformSpace;
    private bool _isTransformSpacePermanent;
    private GizmoObjectTransformPivot _transformPivot;
    private bool _isTransformPivotPermanent;
    private bool _scaleConstraintEnabled;
    private Vector3 _minPositiveScale;
    private Vector3 _customWorldPivot;
    private Dictionary<GameObject, Vector3> _objectToCustomLocalPivot;
    private Dictionary<GameObject, ObjectTransformGizmo.ObjectRestrictions> _objectToRestrictions;
    [SerializeField]
    private ObjectTransformGizmoSettings _settings;
    private ObjectTransformGizmoSettings _sharedSettings;

    public GizmoObjectTransformPivot TransformPivot => this._transformPivot;

    public bool IsTransformPivotPermanent => this._isTransformPivotPermanent;

    public GizmoSpace TransformSpace => this._transformSpace;

    public bool IsTransformSpacePermanent => this._isTransformSpacePermanent;

    public ObjectTransformGizmo.Channels TransformChannelFlags => this._transformChannelFlags;

    public bool CanAffectPosition
    {
      get => (this._transformChannelFlags & ObjectTransformGizmo.Channels.Position) != 0;
    }

    public bool CanAffectRotation
    {
      get => (this._transformChannelFlags & ObjectTransformGizmo.Channels.Rotation) != 0;
    }

    public bool CanAffectScale
    {
      get => (this._transformChannelFlags & ObjectTransformGizmo.Channels.Scale) != 0;
    }

    public Vector3 CustomWorldPivot => this._customWorldPivot;

    public bool ScaleConstraintEnabled
    {
      get => this._scaleConstraintEnabled;
      set => this._scaleConstraintEnabled = value;
    }

    public Vector3 MinPositiveScale
    {
      get => this._minPositiveScale;
      set => this._minPositiveScale = Vector3.Max(value, Vector3Ex.FromValue(1E-05f));
    }

    public ObjectTransformGizmoSettings Settings
    {
      get => this._sharedSettings == null ? this._settings : this._sharedSettings;
    }

    public ObjectTransformGizmoSettings SharedSettings
    {
      get => this._sharedSettings;
      set => this._sharedSettings = value;
    }

    public override void OnAttached()
    {
      MonoSingleton<RTUndoRedo>.Get.UndoEnd += new UndoEndHandler(this.OnUndoRedoEnd);
      MonoSingleton<RTUndoRedo>.Get.RedoEnd += new RedoEndHandler(this.OnUndoRedoEnd);
    }

    public override void OnDetached()
    {
      MonoSingleton<RTUndoRedo>.Get.UndoEnd -= new UndoEndHandler(this.OnUndoRedoEnd);
      MonoSingleton<RTUndoRedo>.Get.RedoEnd -= new RedoEndHandler(this.OnUndoRedoEnd);
    }

    public void MakeTransformSpacePermanent() => this._isTransformSpacePermanent = true;

    public void MakeTransformPivotPermanent() => this._isTransformPivotPermanent = true;

    public bool ContainsRestrictionsForObject(GameObject targetObject)
    {
      return (UnityEngine.Object) targetObject != (UnityEngine.Object) null && this._objectToRestrictions.ContainsKey(targetObject);
    }

    public void RegisterObjectRestrictions(
      GameObject targetObject,
      ObjectTransformGizmo.ObjectRestrictions restrictions)
    {
      if (this.ContainsRestrictionsForObject(targetObject))
        return;
      this._objectToRestrictions.Add(targetObject, restrictions);
    }

    public void RegisterObjectRestrictions(
      List<GameObject> targetObjects,
      ObjectTransformGizmo.ObjectRestrictions restrictions)
    {
      foreach (GameObject targetObject in targetObjects)
        this.RegisterObjectRestrictions(targetObject, restrictions);
    }

    public void UnregisterObjectRestrictions(GameObject targetObject)
    {
      if (!this.ContainsRestrictionsForObject(targetObject))
        return;
      this._objectToRestrictions.Remove(targetObject);
    }

    public ObjectTransformGizmo.ObjectRestrictions GetObjectRestrictions(GameObject targetObject)
    {
      return this.ContainsRestrictionsForObject(targetObject) ? this._objectToRestrictions[targetObject] : (ObjectTransformGizmo.ObjectRestrictions) null;
    }

    public void SetTransformChannelFlags(ObjectTransformGizmo.Channels flags)
    {
      if (this._gizmo.IsDragged)
        return;
      this._transformChannelFlags = flags;
    }

    public void SetCanAffectPosition(bool affectPosition)
    {
      if (this._gizmo.IsDragged)
        return;
      if (affectPosition)
        this._transformChannelFlags |= ObjectTransformGizmo.Channels.Position;
      else
        this._transformChannelFlags &= ~ObjectTransformGizmo.Channels.Position;
    }

    public void SetCanAffectRotation(bool affectRotation)
    {
      if (this._gizmo.IsDragged)
        return;
      if (affectRotation)
        this._transformChannelFlags |= ObjectTransformGizmo.Channels.Rotation;
      else
        this._transformChannelFlags &= ~ObjectTransformGizmo.Channels.Rotation;
    }

    public void SetCanAffectScale(bool affectScale)
    {
      if (this._gizmo.IsDragged)
        return;
      if (affectScale)
        this._transformChannelFlags |= ObjectTransformGizmo.Channels.Scale;
      else
        this._transformChannelFlags &= ~ObjectTransformGizmo.Channels.Scale;
    }

    public void SetTargetPivotObject(GameObject targetPivotObject)
    {
      if (this._gizmo.IsDragged || this._targetObjectMode == ObjectTransformGizmo.TargetObjectMode.Single)
        return;
      this._targetPivotObject = targetPivotObject;
      this.RefreshPositionAndRotation();
    }

    public void SetTargetObjects(IEnumerable<GameObject> targetObjects)
    {
      if (this._gizmo.IsDragged)
        return;
      this._targetObjectMode = ObjectTransformGizmo.TargetObjectMode.Multiple;
      this._targetObjects = targetObjects;
      this.RefreshPositionAndRotation();
    }

    public void SetTargetObject(GameObject targetObject)
    {
      if (this._gizmo.IsDragged)
        return;
      this._targetObjectMode = ObjectTransformGizmo.TargetObjectMode.Single;
      this._targetObjects = (IEnumerable<GameObject>) new List<GameObject>()
      {
        targetObject
      };
      this._targetPivotObject = targetObject;
      this.RefreshPositionAndRotation();
    }

    public void SetTransformPivot(GizmoObjectTransformPivot transformPivot)
    {
      if (this._gizmo.IsDragged || this._isTransformPivotPermanent)
        return;
      this._transformPivot = transformPivot;
      this.RefreshPosition();
    }

    public void SetCustomWorldPivot(Vector3 pivot)
    {
      if (this._gizmo.IsDragged)
        return;
      this._customWorldPivot = pivot;
      this.RefreshPosition();
    }

    public void SetObjectCustomLocalPivot(GameObject gameObj, Vector3 pivot)
    {
      if ((UnityEngine.Object) gameObj == (UnityEngine.Object) null || this._gizmo.IsDragged)
        return;
      if (this._objectToCustomLocalPivot.ContainsKey(gameObj))
        this._objectToCustomLocalPivot[gameObj] = pivot;
      else
        this._objectToCustomLocalPivot.Add(gameObj, pivot);
      this.RefreshPosition();
    }

    public Vector3 GetObjectCustomLocalPivot(GameObject gameObj)
    {
      if ((UnityEngine.Object) gameObj == (UnityEngine.Object) null)
        return Vector3.zero;
      if (this._objectToCustomLocalPivot.ContainsKey(gameObj))
        return this._objectToCustomLocalPivot[gameObj];
      Transform transform = gameObj.transform;
      return transform.InverseTransformPoint(transform.position);
    }

    public void SetTransformSpace(GizmoSpace transformSpace)
    {
      if (this._gizmo.IsDragged || this._isTransformSpacePermanent)
        return;
      this._transformSpace = transformSpace;
      this.RefreshRotation();
    }

    public AABB GetTargetObjectGroupWorldAABB()
    {
      if (this._targetObjects == null)
        return AABB.GetInvalid();
      ObjectBounds.QueryConfig objectBoundsQconfig = this.GetObjectBoundsQConfig();
      AABB objectGroupWorldAabb = AABB.GetInvalid();
      foreach (GameObject targetObject in this._targetObjects)
      {
        AABB aabb = ObjectBounds.CalcWorldAABB(targetObject, objectBoundsQconfig);
        if (objectGroupWorldAabb.IsValid)
          objectGroupWorldAabb.Encapsulate(aabb);
        else
          objectGroupWorldAabb = aabb;
      }
      return objectGroupWorldAabb;
    }

    public int GetNumTransformableParentObjects() => this.GetTransformableParentObjects().Count;

    public void RefreshPosition()
    {
      if (this._targetObjects == null || this._gizmo.IsDragged)
        return;
      GizmoTransform transform = this.Gizmo.Transform;
      if (this._transformPivot == GizmoObjectTransformPivot.ObjectGroupCenter || (UnityEngine.Object) this._targetPivotObject == (UnityEngine.Object) null)
        transform.Position3D = this.GetTargetObjectGroupWorldAABB().Center;
      else if (this._transformPivot == GizmoObjectTransformPivot.ObjectMeshPivot)
        transform.Position3D = !((UnityEngine.Object) this._targetPivotObject == (UnityEngine.Object) null) ? this._targetPivotObject.transform.position : this.GetTargetObjectGroupWorldAABB().Center;
      else if (this._transformPivot == GizmoObjectTransformPivot.ObjectCenterPivot)
      {
        if ((UnityEngine.Object) this._targetPivotObject == (UnityEngine.Object) null)
        {
          transform.Position3D = this.GetTargetObjectGroupWorldAABB().Center;
        }
        else
        {
          AABB aabb = ObjectBounds.CalcWorldAABB(this._targetPivotObject, this.GetObjectBoundsQConfig());
          if (aabb.IsValid)
            transform.Position3D = aabb.Center;
        }
      }
      if (this._transformPivot == GizmoObjectTransformPivot.CustomWorldPivot)
      {
        transform.Position3D = this._customWorldPivot;
      }
      else
      {
        if (this._transformPivot != GizmoObjectTransformPivot.CustomObjectLocalPivot)
          return;
        if ((UnityEngine.Object) this._targetPivotObject == (UnityEngine.Object) null)
          transform.Position3D = this.GetTargetObjectGroupWorldAABB().Center;
        else
          transform.Position3D = this._targetPivotObject.transform.TransformPoint(this.GetObjectCustomLocalPivot(this._targetPivotObject));
      }
    }

    public void RefreshRotation()
    {
      if (this._targetObjects == null || this._gizmo.IsDragged)
        return;
      GizmoTransform transform = this.Gizmo.Transform;
      if (this._transformSpace == GizmoSpace.Global)
        transform.Rotation3D = Quaternion.identity;
      else if ((UnityEngine.Object) this._targetPivotObject == (UnityEngine.Object) null)
        transform.Rotation3D = Quaternion.identity;
      else
        transform.Rotation3D = this._targetPivotObject.transform.rotation;
    }

    public void RefreshPositionAndRotation()
    {
      this.RefreshPosition();
      this.RefreshRotation();
    }

    public override void OnGizmoDragBegin(int handleId)
    {
      this._preTransformSnapshots = LocalTransformSnapshot.GetSnapshotCollection(this._targetObjects);
      this._transformableParents = this.GetTransformableParentObjects();
      this._targetGroupAABBOnDragBegin = this.GetTargetObjectGroupWorldAABB();
    }

    public override void OnGizmoDragUpdate(int handleId)
    {
      if (this.CanAffectPosition && this.Gizmo.ActiveDragChannel == GizmoDragChannel.Offset)
        this.MoveObjects(this.Gizmo.RelativeDragOffset);
      if (this.CanAffectRotation && this.Gizmo.ActiveDragChannel == GizmoDragChannel.Rotation)
        this.RotateObjects(this.Gizmo.RelativeDragRotation);
      if (!this.CanAffectScale || this.Gizmo.ActiveDragChannel != GizmoDragChannel.Scale)
        return;
      this.ScaleObjects();
    }

    public override void OnGizmoDragEnd(int handleId)
    {
      if (this._transformableParents.Count != 0)
        new PostObjectTransformsChangedAction(this._preTransformSnapshots, LocalTransformSnapshot.GetSnapshotCollection(this._targetObjects)).Execute();
      this.RefreshPositionAndRotation();
    }

    private List<GameObject> GetTransformableParentObjects()
    {
      List<GameObject> gameObjectList = GameObjectEx.FilterParentsOnly(this._targetObjects);
      List<GameObject> transformableParentObjects = new List<GameObject>();
      foreach (GameObject gameObject in gameObjectList)
      {
        IRTTransformGizmoListener component = gameObject.GetComponent<IRTTransformGizmoListener>();
        if ((component == null || component.OnCanBeTransformed(this.Gizmo)) && this.Settings.IsLayerTransformable(gameObject.layer) && this.Settings.IsObjectTransformable(gameObject))
          transformableParentObjects.Add(gameObject);
      }
      return transformableParentObjects;
    }

    private void OnUndoRedoEnd(IUndoRedoAction action)
    {
      if (!(action is PostObjectTransformsChangedAction))
        return;
      this.RefreshPositionAndRotation();
    }

    private void MoveObjects(Vector3 moveVector)
    {
      foreach (GameObject transformableParent in this._transformableParents)
        this.MoveObject(transformableParent, moveVector);
    }

    private void MoveObject(GameObject gameObject, Vector3 moveVector)
    {
      ObjectTransformGizmo.ObjectRestrictions objectRestrictions = this.GetObjectRestrictions(gameObject);
      if (objectRestrictions != null)
      {
        if (!objectRestrictions.IsAffectedByHandle(this.Gizmo.DragHandleId))
          return;
        moveVector = objectRestrictions.AdjustMoveVector(moveVector);
      }
      gameObject.transform.position += moveVector;
      gameObject.GetComponent<IRTTransformGizmoListener>()?.OnTransformed(this.Gizmo);
    }

    private void RotateObjects(Quaternion rotation)
    {
      if (this.TransformPivot == GizmoObjectTransformPivot.ObjectGroupCenter)
      {
        foreach (GameObject transformableParent in this._transformableParents)
          this.RotateObject(transformableParent, rotation, this._targetGroupAABBOnDragBegin.Center);
      }
      else if (this.TransformPivot == GizmoObjectTransformPivot.ObjectMeshPivot)
      {
        foreach (GameObject transformableParent in this._transformableParents)
          this.RotateObject(transformableParent, rotation, transformableParent.transform.position);
      }
      else if (this.TransformPivot == GizmoObjectTransformPivot.CustomWorldPivot)
      {
        foreach (GameObject transformableParent in this._transformableParents)
          this.RotateObject(transformableParent, rotation, this.CustomWorldPivot);
      }
      else if (this.TransformPivot == GizmoObjectTransformPivot.ObjectCenterPivot)
      {
        ObjectBounds.QueryConfig objectBoundsQconfig = this.GetObjectBoundsQConfig();
        foreach (GameObject transformableParent in this._transformableParents)
        {
          AABB aabb = ObjectBounds.CalcWorldAABB(transformableParent, objectBoundsQconfig);
          if (aabb.IsValid)
            this.RotateObject(transformableParent, rotation, aabb.Center);
        }
      }
      else
      {
        if (this.TransformPivot != GizmoObjectTransformPivot.CustomObjectLocalPivot)
          return;
        foreach (GameObject transformableParent in this._transformableParents)
        {
          Vector3 rotationPivot = transformableParent.transform.TransformPoint(this.GetObjectCustomLocalPivot(transformableParent));
          this.RotateObject(transformableParent, rotation, rotationPivot);
        }
      }
    }

    private void RotateObject(GameObject gameObject, Quaternion rotation, Vector3 rotationPivot)
    {
      ObjectTransformGizmo.ObjectRestrictions objectRestrictions = this.GetObjectRestrictions(gameObject);
      if (objectRestrictions != null && !objectRestrictions.IsAffectedByHandle(this.Gizmo.DragHandleId))
        return;
      gameObject.transform.RotateAroundPivot(rotation, rotationPivot);
      gameObject.GetComponent<IRTTransformGizmoListener>()?.OnTransformed(this.Gizmo);
    }

    private void ScaleObjects()
    {
      if (this.TransformPivot == GizmoObjectTransformPivot.ObjectGroupCenter)
      {
        foreach (GameObject transformableParent in this._transformableParents)
          this.ScaleObject(transformableParent, this._targetGroupAABBOnDragBegin.Center);
      }
      else if (this.TransformPivot == GizmoObjectTransformPivot.ObjectMeshPivot)
      {
        foreach (GameObject transformableParent in this._transformableParents)
          this.ScaleObject(transformableParent, transformableParent.transform.position);
      }
      else if (this.TransformPivot == GizmoObjectTransformPivot.CustomWorldPivot)
      {
        foreach (GameObject transformableParent in this._transformableParents)
          this.ScaleObject(transformableParent, this.CustomWorldPivot);
      }
      else if (this.TransformPivot == GizmoObjectTransformPivot.ObjectCenterPivot)
      {
        ObjectBounds.QueryConfig objectBoundsQconfig = this.GetObjectBoundsQConfig();
        foreach (GameObject transformableParent in this._transformableParents)
        {
          AABB aabb = ObjectBounds.CalcWorldAABB(transformableParent, objectBoundsQconfig);
          if (aabb.IsValid)
            this.ScaleObject(transformableParent, aabb.Center);
        }
      }
      else
      {
        if (this.TransformPivot != GizmoObjectTransformPivot.CustomObjectLocalPivot)
          return;
        foreach (GameObject transformableParent in this._transformableParents)
        {
          Vector3 scalePivot = transformableParent.transform.TransformPoint(this.GetObjectCustomLocalPivot(transformableParent));
          this.ScaleObject(transformableParent, scalePivot);
        }
      }
    }

    private void ScaleObject(GameObject gameObject, Vector3 scalePivot)
    {
      Transform transform = gameObject.transform;
      Vector3 vector3 = this.Gizmo.RelativeDragScale;
      ObjectTransformGizmo.ObjectRestrictions objectRestrictions = this.GetObjectRestrictions(gameObject);
      if (objectRestrictions != null)
      {
        if (!objectRestrictions.IsAffectedByHandle(this.Gizmo.DragHandleId))
          return;
        vector3 = objectRestrictions.AdjustScaleVector(vector3);
      }
      if (this.ScaleConstraintEnabled)
      {
        Vector3 totalDragScale = this.Gizmo.TotalDragScale;
        if ((double) totalDragScale[0] < 0.0)
          vector3[0] = 1f;
        if ((double) totalDragScale[1] < 0.0)
          vector3[1] = 1f;
        if ((double) totalDragScale[2] < 0.0)
          vector3[2] = 1f;
        transform.ScaleFromPivot(vector3, scalePivot);
        transform.localScale = Vector3.Max(transform.localScale, this._minPositiveScale);
      }
      else
        transform.ScaleFromPivot(vector3, scalePivot);
      gameObject.GetComponent<IRTTransformGizmoListener>()?.OnTransformed(this.Gizmo);
    }

    private ObjectBounds.QueryConfig GetObjectBoundsQConfig()
    {
      return new ObjectBounds.QueryConfig()
      {
        NoVolumeSize = Vector3Ex.FromValue(1E-06f),
        ObjectTypes = GameObjectTypeHelper.AllCombined
      };
    }

    public ObjectTransformGizmo()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._minPositiveScale = Vector3Ex.FromValue(1E-05f);
      this._objectToCustomLocalPivot = new Dictionary<GameObject, Vector3>();
      this._objectToRestrictions = new Dictionary<GameObject, ObjectTransformGizmo.ObjectRestrictions>();
      this._settings = new ObjectTransformGizmoSettings();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public class ObjectRestrictions
    {
      private bool[] _moveAxesMask;
      private bool[] _scaleAxesMask;
      private HashSet<int> _handleMask;

      public bool CanMoveAlongAllAxes()
      {
        return this._moveAxesMask[0] && this._moveAxesMask[1] && this._moveAxesMask[2];
      }

      public bool CanScaleAlongAllAxes()
      {
        return this._scaleAxesMask[0] && this._scaleAxesMask[1] && this._scaleAxesMask[2];
      }

      public bool CanMoveAlongAxis(int axisIndex) => this._moveAxesMask[axisIndex];

      public bool CanScaleAlongAxis(int axisIndex) => this._scaleAxesMask[axisIndex];

      public void SetCanMoveAlongAxis(int axisIndex, bool canMove)
      {
        this._moveAxesMask[axisIndex] = canMove;
      }

      public void SetCanScaleAlongAxis(int axisIndex, bool canScale)
      {
        this._scaleAxesMask[axisIndex] = canScale;
      }

      public bool IsAffectedByHandle(int handleId) => !this._handleMask.Contains(handleId);

      public void SetIsAffectedByHandle(int handleId, bool isAffected)
      {
        if (isAffected)
          this._handleMask.Remove(handleId);
        else
          this._handleMask.Add(handleId);
      }

      public Vector3 AdjustMoveVector(Vector3 moveVector)
      {
        Vector3 vector3 = moveVector;
        if (!this.CanMoveAlongAxis(0))
          vector3[0] = 0.0f;
        if (!this.CanMoveAlongAxis(1))
          vector3[1] = 0.0f;
        if (!this.CanMoveAlongAxis(2))
          vector3[2] = 0.0f;
        return vector3;
      }

      public Vector3 AdjustScaleVector(Vector3 scaleVector)
      {
        Vector3 vector3 = scaleVector;
        if (!this.CanScaleAlongAxis(0))
          vector3[0] = 1f;
        if (!this.CanScaleAlongAxis(1))
          vector3[1] = 1f;
        if (!this.CanScaleAlongAxis(2))
          vector3[2] = 1f;
        return vector3;
      }

      public ObjectRestrictions()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this._moveAxesMask = new bool[3]{ true, true, true };
        this._scaleAxesMask = new bool[3]
        {
          true,
          true,
          true
        };
        this._handleMask = new HashSet<int>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }

    [Flags]
    public enum Channels
    {
      None = 0,
      Position = 1,
      Rotation = 2,
      Scale = 4,
      All = Scale | Rotation | Position, // 0x00000007
    }

    private enum TargetObjectMode
    {
      Multiple,
      Single,
    }
  }
}
