// Decompiled with JetBrains decompiler
// Type: RTG.GizmoLineSlider3D
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
  /// <summary>
  /// This class represents a 3D line slider. This type of slider is rendered as a
  /// straight line segment (thin or thick) and it allows for moving, rotating and
  /// scaling entities.
  /// </summary>
  public class GizmoLineSlider3D : GizmoSlider
  {
    private SegmentShape3D _segment;
    private BoxShape3D _box;
    private CylinderShape3D _cylinder;
    private int _segmentIndex;
    private int _boxIndex;
    private int _cylinderIndex;
    private IGizmoLineSlider3DController[] _controllers;
    private GizmoLineSlider3DControllerData _controllerData;
    private GizmoDragChannel _dragChannel;
    private GizmoSglAxisOffsetDrag3D _offsetDrag;
    private GizmoSglAxisRotationDrag3D _rotationDrag;
    private GizmoRotationArc3D _rotationArc;
    private GizmoSglAxisScaleDrag3D _scaleDrag;
    private int _scaleDragAxisIndex;
    private List<GizmoScalerHandle> _scalerHandles;
    private IGizmoDragSession _selectedDragSession;
    private GizmoCap3D _cap3D;
    private GizmoTransform _transform;
    private GizmoTransformAxisMap3D _directionAxisMap;
    private GizmoTransformAxisMap3D _dragRotationAxisMap;
    private GizmoOverrideColor _overrideColor;
    private GizmoLineSlider3DSettings _settings;
    private GizmoLineSlider3DSettings _sharedSettings;
    private GizmoLineSlider3DLookAndFeel _lookAndFeel;
    private GizmoLineSlider3DLookAndFeel _sharedLookAndFeel;

    /// <summary>
    /// Returns the slider's direction. This is a normalized axis which points from the
    /// slider's start point to its end point. When the slider is used for scaling, it is
    /// recommended to use 'GetRealDirection' instead because the scale session can cause
    /// the slider to point in the opposite direction. This is something that is ignored
    /// by this property.
    /// </summary>
    public Vector3 Direction => this._directionAxisMap.Axis;

    /// <summary>
    /// Returns the rotation axis which is used during rotation sessions.
    /// </summary>
    public Vector3 DragRotationAxis => this._dragRotationAxisMap.Axis;

    /// <summary>
    /// Allows the client code to set or retrieve the scale drag axis index. This is the
    /// index of the scale drag vector component that is affected during a scale session.
    /// Format: 0-&gt;X, 1-&gt;Y, 2-&gt;Z;
    /// </summary>
    public int ScaleDragAxisIndex
    {
      get => this._scaleDragAxisIndex;
      set => this._scaleDragAxisIndex = Mathf.Clamp(value, 0, 2);
    }

    /// <summary>Returns the slider's starting position.</summary>
    public Vector3 StartPosition
    {
      get => this._transform.Position3D;
      set => this._transform.Position3D = value;
    }

    /// <summary>
    /// Returns the slider's drag channel. This can be used to inform the client code
    /// about the type of drag that can be performed with the slider (move, rotate or scale).
    /// </summary>
    public GizmoDragChannel DragChannel => this._dragChannel;

    /// <summary>
    /// Checks if the slideer is currently being dragged. In order to get more specific info
    /// about how the slider is being dragged, use the 'IsMoving', 'IsRotating' and 'IsScaling'
    /// properties. The 'DragChannel' property can also be used to identify the type of drag
    /// that is being applied.
    /// </summary>
    public bool IsDragged
    {
      get
      {
        if (!this.Gizmo.IsDragged)
          return false;
        return this.Gizmo.DragHandleId == this.HandleId || this.Gizmo.DragHandleId == this._cap3D.HandleId;
      }
    }

    /// <summary>Checks if the slider is being moved.</summary>
    public bool IsMoving => this._offsetDrag.IsActive;

    /// <summary>Checks if the slider is being rotated.</summary>
    public bool IsRotating => this._rotationDrag.IsActive;

    /// <summary>Checks if the slider is being scaled.</summary>
    public bool IsScaling => this._scaleDrag.IsActive;

    /// <summary>Checks if the slider's 3D cap is visible.</summary>
    public bool Is3DCapVisible => this._cap3D.IsVisible;

    /// <summary>Checks if the slider's 3D cap is hoverable.</summary>
    public bool Is3DCapHoverable => this._cap3D.IsHoverable;

    /// <summary>Returns the id of the cap handle.</summary>
    public int Cap3DHandleId => this._cap3D.HandleId;

    /// <summary>
    /// Returns the total drag offset since the drag session started.
    /// </summary>
    public Vector3 TotalDragOffset => this._offsetDrag.TotalDragOffset;

    /// <summary>
    /// Returns the relative drag offset since the last drag session update.
    /// </summary>
    public Vector3 RelativeDragOffset => this._offsetDrag.RelativeDragOffset;

    /// <summary>
    /// Returns the total drag rotation since the drag session started.
    /// </summary>
    public float TotalDragRotation => this._rotationDrag.TotalRotation;

    /// <summary>
    /// Returns the relative drag rotation since the last drag session update.
    /// </summary>
    public float RelativeDragRotation => this._rotationDrag.RelativeRotation;

    /// <summary>
    /// Returns the total drag scale since the drag session started.
    /// </summary>
    public float TotalDragScale => this._scaleDrag.TotalScale;

    /// <summary>
    /// Returns the relative drag scale since the last drag session update.
    /// </summary>
    public float RelativeDragScale => this._scaleDrag.RelativeScale;

    /// <summary>
    /// Returns the slider's override color. This can be used in situations where more direct
    /// control is required over the slider's color. This property only affects the slider's
    /// color. The cap color can not be  affected by this property. In order to specify an override
    /// color for the slider's cap, you can use 'Cap3DOverrideColor'.
    /// </summary>
    public GizmoOverrideColor OverrideColor => this._overrideColor;

    /// <summary>
    /// Returns the override color for the slider's 3D cap. This can be used in
    /// situations where more direct control is required over the cap's color.
    /// </summary>
    public GizmoOverrideColor Cap3DOverrideColor => this._cap3D.OverrideColor;

    /// <summary>
    /// Returns the slider's functional settings. If the slider uses shared settings,
    /// the shared settings will be returned. Otherwise, the property will return the
    /// settings associated with the slider instance.
    /// </summary>
    public GizmoLineSlider3DSettings Settings
    {
      get => this._sharedSettings == null ? this._settings : this._sharedSettings;
    }

    /// <summary>
    /// Allows the client code to retrieve or set the slider's shared functional settings.
    /// </summary>
    public GizmoLineSlider3DSettings SharedSettings
    {
      get => this._sharedSettings;
      set => this._sharedSettings = value;
    }

    /// <summary>
    /// Returns the slider's look and feel settings. If the slider uses shared settings,
    /// the shared settings will be returned. Otherwise, the property will return the
    /// settings associated with the slider instance.
    /// </summary>
    public GizmoLineSlider3DLookAndFeel LookAndFeel
    {
      get => this._sharedLookAndFeel == null ? this._lookAndFeel : this._sharedLookAndFeel;
    }

    /// <summary>
    /// Allows the client code to retrieve or set the slider's shared look and feel settings.
    /// </summary>
    public GizmoLineSlider3DLookAndFeel SharedLookAndFeel
    {
      get => this._sharedLookAndFeel;
      set
      {
        this._sharedLookAndFeel = value;
        this.SetupSharedLookAndFeel();
      }
    }

    /// <summary>Constructor.</summary>
    /// <param name="gizmo">The gizmo which owns the slider.</param>
    /// <param name="handleId">The id of the slider handle.</param>
    /// <param name="capHandleId">The id of the slider's cap handle.</param>
    public GizmoLineSlider3D(Gizmo gizmo, int handleId, int capHandleId)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._segment = new SegmentShape3D();
      this._box = new BoxShape3D();
      this._cylinder = new CylinderShape3D();
      this._controllers = new IGizmoLineSlider3DController[Enum.GetValues(typeof (GizmoLine3DType)).Length];
      this._controllerData = new GizmoLineSlider3DControllerData();
      this._dragChannel = GizmoDragChannel.Scale;
      this._offsetDrag = new GizmoSglAxisOffsetDrag3D();
      this._rotationDrag = new GizmoSglAxisRotationDrag3D();
      this._rotationArc = new GizmoRotationArc3D();
      this._scaleDrag = new GizmoSglAxisScaleDrag3D();
      this._scalerHandles = new List<GizmoScalerHandle>();
      this._transform = new GizmoTransform();
      this._directionAxisMap = new GizmoTransformAxisMap3D();
      this._dragRotationAxisMap = new GizmoTransformAxisMap3D();
      this._overrideColor = new GizmoOverrideColor();
      this._settings = new GizmoLineSlider3DSettings();
      this._lookAndFeel = new GizmoLineSlider3DLookAndFeel();
      // ISSUE: explicit constructor call
      base.\u002Ector(gizmo, handleId);
      this._segmentIndex = this.Handle.Add3DShape((Shape3D) this._segment);
      this._boxIndex = this.Handle.Add3DShape((Shape3D) this._box);
      this._cylinderIndex = this.Handle.Add3DShape((Shape3D) this._cylinder);
      this._cap3D = new GizmoCap3D(this.Gizmo, capHandleId);
      this.SetupSharedLookAndFeel();
      this.MapDirection(0, AxisSign.Positive);
      this.SetDragChannel(GizmoDragChannel.Offset);
      this._controllerData.Gizmo = this.Gizmo;
      this._controllerData.Slider = this;
      this._controllerData.SliderHandle = this.Handle;
      this._controllerData.Segment = this._segment;
      this._controllerData.Box = this._box;
      this._controllerData.Cylinder = this._cylinder;
      this._controllerData.SegmentIndex = this._segmentIndex;
      this._controllerData.BoxIndex = this._boxIndex;
      this._controllerData.CylinderIndex = this._cylinderIndex;
      this._controllers[0] = (IGizmoLineSlider3DController) new GizmoThinLineSlider3DController(this._controllerData);
      this._controllers[1] = (IGizmoLineSlider3DController) new GizmoBoxLineSlider3DController(this._controllerData);
      this._controllers[2] = (IGizmoLineSlider3DController) new GizmoCylinderLineSlider3DController(this._controllerData);
      this._transform.Changed += new GizmoEntityTransformChangedHandler(this.OnTransformChanged);
      this.Gizmo.PreUpdateBegin += new GizmoPreUpdateBeginHandler(this.OnGizmoPreUpdateBegin);
      this.Gizmo.PreDragBeginAttempt += new GizmoPreDragBeginAttemptHandler(this.OnGizmoAttemptHandleDragBegin);
      this.Gizmo.PreHoverEnter += new GizmoPreHoverEnterHandler(this.OnGizmoHandleHoverEnter);
      this.Gizmo.PreHoverExit += new GizmoPreHoverExitHandler(this.OnGizmoHandleHoverExit);
      this.Gizmo.PostEnabled += new GizmoPostEnabledHandler(this.OnGizmoPostEnabled);
      this.Gizmo.PostDisabled += new GizmoPostDisabledHandler(this.OnGizmoPostDisabled);
      this.AddTargetTransform(this._transform);
      this.AddTargetTransform(this.Gizmo.Transform);
      this._cap3D.RegisterTransformAsDragTarget((IGizmoDragSession) this._offsetDrag);
      this._cap3D.RegisterTransformAsDragTarget((IGizmoDragSession) this._rotationDrag);
      this._cap3D.RegisterTransformAsDragTarget((IGizmoDragSession) this._scaleDrag);
      this._transform.SetParent(this.Gizmo.Transform);
    }

    /// <summary>
    /// Checks if the slider contains a record of a scaler handle with the specified id.
    /// </summary>
    public bool IsScalerHandleRegistered(int handleId)
    {
      return this._scalerHandles.FindAll((Predicate<GizmoScalerHandle>) (item => item.HandleId == handleId)).Count != 0;
    }

    /// <summary>
    /// Checks if the slider contains a record of a scaler handle with the specified id
    /// which also affects the specified scale drag axis.
    /// </summary>
    public bool IsScalerHandleRegistered(int handleId, int scaleDragAxisIndex)
    {
      List<GizmoScalerHandle> all = this._scalerHandles.FindAll((Predicate<GizmoScalerHandle>) (item => item.HandleId == handleId));
      return all.Count != 0 && all[0].ContainsScaleDragAxisIndex(scaleDragAxisIndex);
    }

    /// <summary>
    /// This function is useful when the slider needs to have its size affected by scale
    /// sessions which originate from other handles. For example, the mid cap of a scale
    /// gizmo affects the length of the axes sliders. The function receives the id of the
    /// scaler handle (mid cap in this example) and a list of scale drag axis indices which
    /// represent the indices of the scale drag vector which are affected by the scaler
    /// handle. The function has no effect if a scaler handle with the same id already exists.
    /// </summary>
    /// <param name="handleId">The scaler handle id.</param>
    /// <param name="scaleDragAxisIndices">
    /// A collection of scale drag axis indices which represent the indices of the scale drag
    /// vector components which are affected by the scaler handle. The important thing to
    /// remember is that the slider is only affected by the scale drag vector component with
    /// index = ScaleDragAxisIndex.
    /// </param>
    public void RegisterScalerHandle(int handleId, IEnumerable<int> scaleDragAxisIndices)
    {
      if (this.IsScalerHandleRegistered(handleId))
        return;
      this._scalerHandles.Add(new GizmoScalerHandle(handleId, scaleDragAxisIndices));
    }

    /// <summary>
    /// Unregisters the scaler handle with the specified handle id. The function has no
    /// effect if a scaler handle with the specified id doesn't exist. After this function
    /// is called, any scale sessions which originate from the handle with the specified
    /// id will no longer affect the slider length.
    /// </summary>
    public void UnregisterScalerHandle(int handleId)
    {
      this._scalerHandles.RemoveAll((Predicate<GizmoScalerHandle>) (item => item.HandleId == handleId));
    }

    /// <summary>
    /// Can be used to enable/disable snapping for all drag channels.
    /// </summary>
    public override void SetSnapEnabled(bool isEnabled)
    {
      this._offsetDrag.IsSnapEnabled = isEnabled;
      this._rotationDrag.IsSnapEnabled = isEnabled;
      this._scaleDrag.IsSnapEnabled = isEnabled;
    }

    /// <summary>
    /// Sets the visibility state of the 3D cap. A visible cap will be rendered, and
    /// it can also be hovered if it is set to be hoverable (see 'Set3DCapHoverable').
    /// </summary>
    public void Set3DCapVisible(bool isVisible) => this._cap3D.SetVisible(isVisible);

    /// <summary>
    /// Sets the hoverable state of the 3D cap. A hoverable cap can be hovered ONLY if
    /// it is visible (see 'Set3DCapVisible'). So passing true to this function will only
    /// allow the cap to be hovered if it is also visible.
    /// </summary>
    public void Set3DCapHoverable(bool isHoverable) => this._cap3D.SetHoverable(isHoverable);

    /// <summary>
    /// Sets the zoom factor transform. The world position of the zoom factor transform is
    /// used to calculate a slider zoom factor. The zoom factor is used in order to allow
    /// the slider to maintain a constant size regardless of its distance from the camera.
    /// </summary>
    /// <param name="transform">
    /// The zoom factor transform. If null, the function will default to using the transform
    /// of the owner gizmo instead.
    /// </param>
    public void SetZoomFactorTransform(GizmoTransform transform)
    {
      this.Handle.SetZoomFactorTransform(transform);
    }

    /// <summary>
    /// Calculates and returns the slider zoom factor for the specified camera. The zoom factor
    /// is calculated using the current zoom factor transform. See 'SetZoomFactorTransform'. This
    /// value can be useful for scaling different values such as sizes for example in order to
    /// allow them to have a constant magnitude in relation to the distance between the camera and
    /// the position of the zoom factor transform. The function returns a value of 1.0f if the look
    /// and feel settings have been set up to ignore the zoom factor.
    /// </summary>
    public float GetZoomFactor(Camera camera)
    {
      return !this.LookAndFeel.UseZoomFactor ? 1f : this.Handle.GetZoomFactor(camera);
    }

    /// <summary>
    /// Returns the real direction axis of the slider. This function has the same purpose as the
    /// 'Direction' property, but it also takes into account the scaling which is applied during
    /// a scale session which can cause the slider to point in the opposite direction. If you know
    /// that a slider will never be used for scaling, you can use the 'Direction' property instead
    /// as it is faster.
    /// </summary>
    public Vector3 GetRealDirection()
    {
      float num = 1f;
      if (this._scaleDrag.IsActive)
        num = Mathf.Sign(this.TotalDragScale);
      else if (this.Gizmo.IsDragged && this.IsScalerHandleRegistered(this.Gizmo.DragHandleId, this.ScaleDragAxisIndex))
        num = Mathf.Sign(this.Gizmo.TotalDragScale[this.ScaleDragAxisIndex]);
      return this.Direction * num;
    }

    /// <summary>
    /// Returns the size (length and thickness) of the slider along the specified direction
    /// axis. The function will take into accoun the look and feel settings such as the slider
    /// type (line, box, cylinder), scale and whether or not a zoom factor should be applied.
    /// </summary>
    /// <param name="camera">
    /// This is the camera needed to calculate the slider zoom factor if necessary.
    /// </param>
    /// <param name="direction">
    /// The direction along which the slider size will be calculated.
    /// </param>
    public float GetRealSizeAlongDirection(Camera camera, Vector3 direction)
    {
      return this._controllers[(int) this.LookAndFeel.LineType].GetRealSizeAlongDirection(direction, this.GetZoomFactor(this.Gizmo.GetWorkCamera()));
    }

    /// <summary>
    /// Returns the real length of the slider. The real length takes into account all the look
    /// and feel settings such as length, scale and zoom factor and combines them to return a
    /// value that represents the real length of the slider. If the slider is participating in
    /// a scale drag session, the total scale applied since session begin will also be used to
    /// calculate the length.
    /// </summary>
    /// <param name="zoomFactor">
    /// The slider's zoom factor. Can be calculated using 'GetZoomFactor'.
    /// </param>
    public float GetRealLength(float zoomFactor)
    {
      if (!this.LookAndFeel.UseZoomFactor)
        zoomFactor = 1f;
      float realLength = this.LookAndFeel.Length * this.LookAndFeel.Scale * zoomFactor;
      if (this._scaleDrag.IsActive)
        realLength *= this._scaleDrag.TotalScale;
      else if (this.Gizmo.IsDragged && this.IsScalerHandleRegistered(this.Gizmo.DragHandleId, this.ScaleDragAxisIndex))
        realLength *= this.Gizmo.TotalDragScale[this.ScaleDragAxisIndex];
      return realLength;
    }

    /// <summary>
    /// Same as 'GetRealLength', but it also takes the length of the 3D cap into account.
    /// </summary>
    /// <param name="zoomFactor">
    /// The slider's zoom factor. Can be calculated using 'GetZoomFactor'.
    /// </param>
    public float GetRealLengthWith3DCap(float zoomFactor)
    {
      return this._cap3D.GetSliderAlignedRealLength(zoomFactor) + this.GetRealLength(zoomFactor);
    }

    /// <summary>
    /// Returns the slider's real end point. Works in pretty much the same way as
    /// 'GetRealLength' but it can be used to calculate the end point instead. In
    /// fact this function uses 'GetRealLength' under the hood to travel from the
    /// start poition to the end.
    /// </summary>
    /// <param name="zoomFactor">
    /// The slider's zoom factor. Can be calculated using 'GetZoomFactor'.
    /// </param>
    public Vector3 GetRealEndPosition(float zoomFactor)
    {
      return this.StartPosition + this.Direction * this.GetRealLength(zoomFactor);
    }

    /// <summary>
    /// Same as 'GetRealEndPosition' but it also takes into account the slider's 3D cap.
    /// </summary>
    /// <param name="zoomFactor">
    /// The slider's zoom factor. Can be calculated using 'GetZoomFactor'.
    /// </param>
    public Vector3 GetRealEndPositionWith3DCap(float zoomFactor)
    {
      return this.StartPosition + this.Direction * this.GetRealLengthWith3DCap(zoomFactor);
    }

    /// <summary>
    /// Returns the real height of the slider's box. This is useful when the slider
    /// type is set to box and you need to know the real height of the box. The real
    /// height takes into account scale and zoom factor if necessary.
    /// </summary>
    /// <param name="zoomFactor">
    /// The slider's zoom factor. Can be calculated using 'GetZoomFactor'.
    /// </param>
    public float GetRealBoxHeight(float zoomFactor)
    {
      if (!this.LookAndFeel.UseZoomFactor)
        zoomFactor = 1f;
      return this.LookAndFeel.BoxHeight * this.LookAndFeel.Scale * zoomFactor;
    }

    /// <summary>
    /// Returns the real depth of the slider's box. This is useful when the slider
    /// type is set to box and you need to know the real depth of the box. The real
    /// depth takes into account scale and zoom factor if necessary.
    /// </summary>
    /// <param name="zoomFactor">
    /// The slider's zoom factor. Can be calculated using 'GetZoomFactor'.
    /// </param>
    public float GetRealBoxDepth(float zoomFactor)
    {
      if (!this.LookAndFeel.UseZoomFactor)
        zoomFactor = 1f;
      return this.LookAndFeel.BoxDepth * this.LookAndFeel.Scale * zoomFactor;
    }

    /// <summary>
    /// Returns the real radius of the slider's cylinder. This is useful when the
    /// slider type is set to cylinder and you need to know the real radius. The real
    /// radius takes into account scale and zoom factor if necessary.
    /// </summary>
    /// <param name="zoomFactor">
    /// The slider's zoom factor. Can be calculated using 'GetZoomFactor'.
    /// </param>
    public float GetRealCylinderRadius(float zoomFactor)
    {
      if (!this.LookAndFeel.UseZoomFactor)
        zoomFactor = 1f;
      return this.LookAndFeel.CylinderRadius * this.LookAndFeel.Scale * zoomFactor;
    }

    /// <summary>
    /// This function can be used to link the slider's direction axis to one its transform
    /// axes. For example, if you wish the direction axis to always point along the slider's
    /// transform up vector, you would call MapDirection(1, AxisSign.Positive). Linking the
    /// direction axis to the slider's transform like this has the advantage that no matter
    /// how the slider is rotated, the direction axis will follow. The function has no effect
    /// if the slider is currently involved in a drag operation.
    /// </summary>
    /// <param name="axisIndex">
    /// The index of the slider transform axis which will act as the direction axis. The format
    /// is: 0-&gt;X, 1-&gt;Y, 2-&gt;Z.
    /// </param>
    /// <param name="axisSign">
    /// The sign of the axis. Allows you to differentiate between positive and negative axes.
    /// </param>
    public void MapDirection(int axisIndex, AxisSign axisSign)
    {
      if (this.IsDragged)
        return;
      this._directionAxisMap.Map(this._transform, axisIndex, axisSign);
    }

    /// <summary>
    /// Similar to 'MapDirection', but this function applies to the slider's drag rotation axis.
    /// This is the axis around which the slider will rotate during a rotation session. The other
    /// difference is that this function requires you to pass a transform and the rest of the
    /// parameters identify the axis inside this transform. The function has no effect if the slider
    /// is currently involved in a drag operation.
    /// </summary>
    public void MapDragRotationAxis(GizmoTransform mapTransform, int axisIndex, AxisSign axisSign)
    {
      if (this.IsDragged)
        return;
      this._dragRotationAxisMap.Map(mapTransform, axisIndex, axisSign);
    }

    /// <summary>
    /// Removes the mapping for the drag rotation axis which was created using a previous call
    /// to 'MapDragRotationAxis'. The function has no effect if the slider is currently involved
    /// in a drag operation.
    /// </summary>
    public void UnmapDragRotationAxis()
    {
      if (this.IsDragged)
        return;
      this._dragRotationAxisMap.Unmap();
    }

    /// <summary>
    /// Sets the direction of the slider. This is useful when you have access to a direction
    /// vector and you would like the slider to point along this vector. Calling this function
    /// will change the slider transform's rotation in order to align the direction axis with
    /// the desired vector. The function has no effect if the slider is currently involved in
    /// a drag operation.
    /// </summary>
    /// <param name="directionAxis">
    /// The slider's direction axis. The function will automatically normalize this vector.
    /// </param>
    public void SetDirection(Vector3 directionAxis)
    {
      if (this.IsDragged)
        return;
      this._directionAxisMap.SetAxis(directionAxis);
    }

    /// <summary>
    /// Sets the drag rotation axis. This is useful when you have access to a rotation axis
    /// vector and you would like to use that during rotation sessions. Calling this function
    /// will cancel the mapping created via a previous call to 'MapDragRotationAxis'. The
    /// function has no effect if the slider is currently involved in a drag operation.
    /// </summary>
    /// <param name="rotationAxis"></param>
    public void SetDragRotationAxis(Vector3 rotationAxis)
    {
      if (this.IsDragged)
        return;
      this._dragRotationAxisMap.Unmap();
      this._dragRotationAxisMap.SetAxis(rotationAxis);
    }

    /// <summary>
    /// Call this function when you have a transform that you would like to be affected
    /// by the slider's drag sessions. After calling this function, the world position and
    /// rotation of the specified transform will be affected by the slider's offset and
    /// rotation drag sessions. The function has no effect if the transform has already
    /// been registered via a previous call to 'AddTargetTransform'.
    /// </summary>
    public void AddTargetTransform(GizmoTransform transform)
    {
      this._offsetDrag.AddTargetTransform(transform);
      this._rotationDrag.AddTargetTransform(transform);
      this._scaleDrag.AddTargetTransform(transform);
    }

    /// <summary>
    /// Call this function when you have a transform that you would like to be affected
    /// by one of the slider's drag sessions. The specified drag channel identifies the
    /// type of session that will affect the specified transform. The function has no
    /// effect if the transform has already been registered via a previous call to
    /// 'AddTargetTransform'.
    /// </summary>
    public void AddTargetTransform(GizmoTransform transform, GizmoDragChannel dragChannel)
    {
      if (dragChannel == GizmoDragChannel.Offset)
        this._offsetDrag.AddTargetTransform(transform);
      else if (dragChannel == GizmoDragChannel.Rotation)
      {
        this._rotationDrag.AddTargetTransform(transform);
      }
      else
      {
        if (this._dragChannel != GizmoDragChannel.Scale)
          return;
        this._scaleDrag.AddTargetTransform(transform);
      }
    }

    /// <summary>
    /// Removes the specified target transform. After this function is called, the specified
    /// transform will no longer be affected by any of the slider's drag sessions.
    /// </summary>
    public void RemoveTargetTransform(GizmoTransform transform)
    {
      this._offsetDrag.RemoveTargetTransform(transform);
      this._rotationDrag.RemoveTargetTransform(transform);
      this._scaleDrag.RemoveTargetTransform(transform);
    }

    /// <summary>
    /// Removes the specified target transform. After this function is called, the specified
    /// transform will no longer be affected by the drag session which corresponds to the
    /// specified drag channel.
    /// </summary>
    public void RemoveTargetTransform(GizmoTransform transform, GizmoDragChannel dragChannel)
    {
      if (dragChannel == GizmoDragChannel.Offset)
        this._offsetDrag.RemoveTargetTransform(transform);
      else if (dragChannel == GizmoDragChannel.Rotation)
      {
        this._rotationDrag.RemoveTargetTransform(transform);
      }
      else
      {
        if (this._dragChannel != GizmoDragChannel.Scale)
          return;
        this._scaleDrag.RemoveTargetTransform(transform);
      }
    }

    /// <summary>
    /// Sets the slider's drag channel. This is how the client code can switch between an offset
    /// slider, a rotation slider or a scale slider.
    /// </summary>
    public void SetDragChannel(GizmoDragChannel dragChannel)
    {
      this._dragChannel = dragChannel;
      if (this._dragChannel == GizmoDragChannel.Offset)
        this._selectedDragSession = (IGizmoDragSession) this._offsetDrag;
      else if (this._dragChannel == GizmoDragChannel.Rotation)
        this._selectedDragSession = (IGizmoDragSession) this._rotationDrag;
      else if (this._dragChannel == GizmoDragChannel.Scale)
        this._selectedDragSession = (IGizmoDragSession) this._scaleDrag;
      this.Handle.DragSession = this._selectedDragSession;
      this._cap3D.DragSession = this._selectedDragSession;
    }

    /// <summary>
    /// This function calculates the slider's zoom factor for the specified camera and uses
    /// it to adjust the slider shape transforms such as size for example. The zoom factor
    /// is automatically applied during each gizmo 'UpdateBegin' step, so the ONLY time when
    /// this function should be called is when more than one camera can render the slider.
    /// In that case, before calling 'Render, the client code should call this function to
    /// ensure that the slider is rendered correctly. The function has no effect if the look
    /// and feel settings' 'UseZoomFactor' property returns false.
    /// </summary>
    public void ApplyZoomFactor(Camera camera)
    {
      if (!this.LookAndFeel.UseZoomFactor)
        return;
      float zoomFactor = this.GetZoomFactor(camera);
      this._controllers[(int) this.LookAndFeel.LineType].UpdateTransforms(zoomFactor);
      this._cap3D.ApplyZoomFactor(camera);
      this._cap3D.CapSlider3D(this.GetRealDirection(), this.GetRealEndPosition(zoomFactor));
    }

    /// <summary>
    /// Renders the slider with the specified camera. This function should be called
    /// directly or indirectly from a Monobehaviour's 'OnRenderObject' function.
    /// </summary>
    public override void Render(Camera camera)
    {
      if (!this.IsVisible && !this.Is3DCapVisible)
        return;
      Color color1 = new Color();
      Color color2 = this.OverrideColor.IsActive ? this.OverrideColor.Color : (this.Gizmo.HoverHandleId != this.HandleId ? this.LookAndFeel.Color : this.LookAndFeel.HoveredColor);
      if (this.LookAndFeel.IsRotationArcVisible && this.IsRotating)
      {
        this._rotationArc.RotationAngle = this._rotationDrag.TotalRotation;
        this._rotationArc.Radius = this.GetRealLength(this.GetZoomFactor(camera));
        this._rotationArc.Render(this.LookAndFeel.RotationArcLookAndFeel);
      }
      bool flag = !camera.IsPointFacingCamera(this.GetRealEndPosition(this.GetZoomFactor(camera)), this.GetRealDirection());
      if (this.Is3DCapVisible & flag)
        this._cap3D.Render(camera);
      if (this.IsVisible)
      {
        if (this.LookAndFeel.FillMode == GizmoFillMode3D.Filled)
        {
          bool isLit = this.LookAndFeel.ShadeMode == GizmoShadeMode.Lit && this.LookAndFeel.LineType != 0;
          GizmoSolidMaterial get = Singleton<GizmoSolidMaterial>.Get;
          get.ResetValuesToSensibleDefaults();
          get.SetLit(isLit);
          if (isLit)
            get.SetLightDirection(camera.transform.forward);
          get.SetColor(color2);
          get.SetPass(0);
          this.Handle.Render3DSolid();
        }
        else
        {
          GizmoLineMaterial get = Singleton<GizmoLineMaterial>.Get;
          get.ResetValuesToSensibleDefaults();
          get.SetColor(color2);
          get.SetPass(0);
          this.Handle.Render3DWire();
        }
      }
      if (!this.Is3DCapVisible || flag)
        return;
      this._cap3D.Render(camera);
    }

    /// <summary>
    /// This function can be called after changing functional or look and feel settings
    /// to ensure that these settings take effect.
    /// </summary>
    public void Refresh()
    {
      float zoomFactor = this.GetZoomFactor(this.Gizmo.GetWorkCamera());
      this._controllers[(int) this.LookAndFeel.LineType].UpdateHandles();
      this._controllers[(int) this.LookAndFeel.LineType].UpdateEpsilons(zoomFactor);
      this._controllers[(int) this.LookAndFeel.LineType].UpdateTransforms(zoomFactor);
      this._cap3D.CapSlider3D(this.GetRealDirection(), this.GetRealEndPosition(zoomFactor));
    }

    protected override void OnVisibilityStateChanged()
    {
      this._controllers[(int) this.LookAndFeel.LineType].UpdateHandles();
      float zoomFactor = this.GetZoomFactor(this.Gizmo.GetWorkCamera());
      this._controllers[(int) this.LookAndFeel.LineType].UpdateEpsilons(zoomFactor);
      this._controllers[(int) this.LookAndFeel.LineType].UpdateTransforms(zoomFactor);
      this._cap3D.CapSlider3D(this.GetRealDirection(), this.GetRealEndPosition(zoomFactor));
    }

    protected override void OnHoverableStateChanged() => this.Handle.SetHoverable(this.IsHoverable);

    private void OnGizmoPreUpdateBegin(Gizmo gizmo)
    {
      int lineType = (int) this.LookAndFeel.LineType;
      this._controllers[lineType].UpdateHandles();
      float zoomFactor = this.GetZoomFactor(gizmo.FocusCamera);
      this._offsetDrag.Sensitivity = this.Settings.OffsetSensitivity;
      this._rotationDrag.Sensitivity = this.Settings.RotationSensitivity;
      this._scaleDrag.Sensitivity = this.Settings.ScaleSensitivity;
      this._controllers[lineType].UpdateTransforms(zoomFactor);
      this._controllers[lineType].UpdateEpsilons(zoomFactor);
      this._cap3D.GenericHoverPriority.Value = this.GenericHoverPriority.Value;
      this._cap3D.HoverPriority2D.Value = this.HoverPriority2D.Value;
      this._cap3D.HoverPriority3D.Value = this.HoverPriority3D.Value;
      this._cap3D.CapSlider3D(this.GetRealDirection(), this.GetRealEndPosition(zoomFactor));
    }

    private void OnGizmoAttemptHandleDragBegin(Gizmo gizmo, int handleId)
    {
      if (handleId != this.Handle.Id && handleId != this._cap3D.HandleId)
        return;
      if (this._dragChannel == GizmoDragChannel.Offset)
        this._offsetDrag.SetWorkData(new GizmoSglAxisOffsetDrag3D.WorkData()
        {
          Axis = this.Direction,
          DragOrigin = this.StartPosition,
          SnapStep = this.Settings.OffsetSnapStep
        });
      else if (this._dragChannel == GizmoDragChannel.Rotation)
      {
        this._rotationDrag.SetWorkData(new GizmoSglAxisRotationDrag3D.WorkData()
        {
          Axis = this.DragRotationAxis,
          RotationPlanePos = this.StartPosition,
          SnapStep = this.Settings.RotationSnapStep,
          SnapMode = this.Settings.RotationSnapMode
        });
        this._rotationArc.SetArcData(this.DragRotationAxis, this.StartPosition, this.StartPosition + this.Direction, this.GetRealLength(this.GetZoomFactor(this.Gizmo.FocusCamera)));
      }
      else
      {
        if (this._dragChannel != GizmoDragChannel.Scale)
          return;
        this._scaleDrag.SetWorkData(new GizmoSglAxisScaleDrag3D.WorkData()
        {
          Axis = this.Direction,
          DragOrigin = this.StartPosition,
          SnapStep = this.Settings.ScaleSnapStep,
          AxisIndex = this.ScaleDragAxisIndex,
          EntityScale = 1f
        });
      }
    }

    private void OnTransformChanged(GizmoTransform transform, GizmoTransform.ChangeData changeData)
    {
      if (changeData.ChangeReason != GizmoTransform.ChangeReason.ParentChange && changeData.TRSDimension != GizmoDimension.Dim3D)
        return;
      float zoomFactor = this.GetZoomFactor(this.Gizmo.GetWorkCamera());
      this._controllers[(int) this.LookAndFeel.LineType].UpdateTransforms(zoomFactor);
      this._cap3D.CapSlider3D(this.GetRealDirection(), this.GetRealEndPosition(zoomFactor));
    }

    private void OnGizmoHandleHoverEnter(Gizmo gizmo, int handleId)
    {
      if (handleId == this.HandleId)
      {
        this._cap3D.OverrideColor.IsActive = true;
        this._cap3D.OverrideColor.Color = this.LookAndFeel.CapLookAndFeel.HoveredColor;
      }
      else
      {
        if (handleId != this._cap3D.HandleId)
          return;
        this.OverrideColor.IsActive = true;
        this.OverrideColor.Color = this.LookAndFeel.HoveredColor;
      }
    }

    private void OnGizmoPostEnabled(Gizmo gizmo) => this.Refresh();

    private void OnGizmoPostDisabled(Gizmo gizmo)
    {
      this.OverrideColor.IsActive = false;
      this._cap3D.OverrideColor.IsActive = false;
    }

    private void OnGizmoHandleHoverExit(Gizmo gizmo, int handleId)
    {
      if (handleId == this.HandleId)
      {
        this._cap3D.OverrideColor.IsActive = false;
      }
      else
      {
        if (handleId != this._cap3D.HandleId)
          return;
        this.OverrideColor.IsActive = false;
      }
    }

    private void SetupSharedLookAndFeel()
    {
      this._cap3D.SharedLookAndFeel = this.LookAndFeel.CapLookAndFeel;
    }
  }
}
