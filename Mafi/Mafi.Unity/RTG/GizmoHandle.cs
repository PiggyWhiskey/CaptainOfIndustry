// Decompiled with JetBrains decompiler
// Type: RTG.GizmoHandle
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
  /// Gizmo handles represent the core components that are used to define a gizmo. A handle
  /// is essentially a shape, or collection of shapes, both 2D and/or 3D, which can be tapped
  /// and interacted with by the user. A gizmo is, at its core, nothing else but a collection
  /// of handles.
  /// </summary>
  public class GizmoHandle : IGizmoHandle
  {
    /// <summary>
    /// Event fired from 'GetHoverData' to allow subscribers to decide if a handle is allowed
    /// to be hovered,
    /// </summary>
    /// <param name="handleId">The handle id.</param>
    /// <param name="ownerGizmo">The gizmo which owns the handle.</param>
    /// <param name="handleHoverData">Contains useful information about the handle hover state.</param>
    /// <param name="answer">
    /// All handlers must answer with either yes or no to tell the handle if it can be hovered.
    /// </param>
    public GizmoHandleCanHoverHandler CanHover;
    private int _id;
    private Gizmo _gizmo;
    private GizmoTransform _zoomFactorTransform;
    private Priority _genericHoverPriority;
    private Priority _hoverPriority2D;
    private Priority _hoverPriority3D;
    private List<GizmoHandleShape3D> _3DShapes;
    private List<GizmoHandleShape2D> _2DShapes;

    /// <summary>
    /// Returns the handle's id. The id is unique at gizmo scope, but handles that belong
    /// to different gizmos might have the same id.
    /// </summary>
    public int Id => this._id;

    /// <summary>
    /// Returns the gizmo which owns the handle. Only one gizmo can own a handle and this
    /// gizmo must be specified at handle creation time as a constructor parameter. Thus,
    /// the owner gizmo can not be changed after the handle was created.
    /// </summary>
    public Gizmo Gizmo => this._gizmo;

    /// <summary>
    /// The drag session which is associated with the handle. When the user taps one of the
    /// gizmo handles, the gizmo will check if the handle has a drag session associated with
    /// it and if it does, it will attempt to start the session.
    /// </summary>
    public IGizmoDragSession DragSession { get; set; }

    /// <summary>
    /// Returns the handle's generic hover priority. The generic hover priority is used as
    /// a sorting criteria by the hover update algorithm to sort between a mixture of 2D
    /// and 3D handles that are hovered by the cursor.
    /// </summary>
    public Priority GenericHoverPriority => this._genericHoverPriority;

    /// <summary>
    /// Returns the handle's 2D hover priority. The 2D hover priority is used as a sorting
    /// criteria by the hover update algorithm to sort between 2D handles that are hovered
    /// by the cursor.
    /// </summary>
    public Priority HoverPriority2D => this._hoverPriority2D;

    /// <summary>
    /// Returns the handle's 3D hover priority. The 2D hover priority is used as a sorting
    /// criteria by the hover update algorithm to sort between 3D handles that are hovered
    /// by the cursor.
    /// </summary>
    public Priority HoverPriority3D => this._hoverPriority3D;

    /// <summary>
    /// Returns the number of 3D shapes which are associated with the handle.
    /// </summary>
    public int Num3DShapes => this._3DShapes.Count;

    /// <summary>
    /// Returns the number of 2D shapes which are associated with the handle.
    /// </summary>
    public int Num2DShapes => this._2DShapes.Count;

    /// <summary>
    /// Can be used to check if the handle has any 3D shapes associated with it. Shorthand
    /// for checking if the number of 3D shapes is different than 0.
    /// </summary>
    public bool Has3DShapes => this.Num3DShapes != 0;

    /// <summary>
    /// Can be used to check if the handle has any 2D shapes associated with it. Shorthand
    /// for checking if the number of 2D shapes is different than 0.
    /// </summary>
    public bool Has2DShapes => this.Num2DShapes != 0;

    /// <summary>
    /// This property can be used to tell the handle if its 2D shapes are hoverable. For
    /// example, if a handle contains both 3D and 2D shapes, setting this to false will
    /// cause the hover algorithm to ignore 2D shapes and consider only the 3D ones.
    /// </summary>
    public bool Is2DHoverable { get; set; }

    /// <summary>
    /// This property can be used to tell the handle if its 3D shapes are hoverable. For
    /// example, if a handle contains both 3D and 2D shapes, setting this to false will
    /// cause the hover algorithm to ignore 3D shapes and consider only the 2D ones.
    /// </summary>
    public bool Is3DHoverable { get; set; }

    /// <summary>
    /// This property can be used to tell the handle that it should not render any of its
    /// 2D shapes. Note: When this property is set to false, any 2D shapes associated with
    /// the handle will no longer be hoverable even if 'Is2DHoverable' returns true.
    /// </summary>
    public bool Is2DVisible { get; set; }

    /// <summary>
    /// This property can be used to tell the handle that it should not render any of its
    /// 3D shapes. Note: When this property is set to false, any 3D shapes associated with
    /// the handle will no longer be hoverable even if 'Is3DHoverable' returns true.
    /// </summary>
    public bool Is3DVisible { get; set; }

    /// <summary>Constructor.</summary>
    /// <param name="gizmo">The gizmo which owns the handle.</param>
    /// <param name="id">The handle id. Must be unique at gizmo scope.</param>
    public GizmoHandle(Gizmo gizmo, int id)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._genericHoverPriority = new Priority();
      this._hoverPriority2D = new Priority();
      this._hoverPriority3D = new Priority();
      this._3DShapes = new List<GizmoHandleShape3D>();
      this._2DShapes = new List<GizmoHandleShape2D>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._id = id;
      this._gizmo = gizmo;
      this._zoomFactorTransform = this._gizmo.Transform;
      this.SetHoverable(true);
      this.SetVisible(true);
    }

    /// <summary>
    /// Returns the handle's zoom factor for the specified camera. This value can be used to
    /// scale handle shape properties such as sizes for example in order to allow the shapes
    /// to maintain a constant size regardless of the distance from the camera. This function
    /// uses the world position of the specified zoom factor transform to calculate the distance
    /// from the camera position. See also 'SetZoomFactorTransform'.
    /// </summary>
    public float GetZoomFactor(Camera camera)
    {
      return camera.EstimateZoomFactor(this._zoomFactorTransform.Position3D);
    }

    /// <summary>
    /// Allows you to set the zoom factor transform. The world position of this transform is used
    /// by the 'GetZoomFactor' function to calculate the handle zoom factor. If null is specified,
    /// the function will set the zoom factor transform to the transform of the owner gizmo.
    /// </summary>
    public void SetZoomFactorTransform(GizmoTransform transform)
    {
      if (transform == null)
        this._zoomFactorTransform = this._gizmo.Transform;
      else
        this._zoomFactorTransform = transform;
    }

    /// <summary>
    /// Sets the hoverable state of the handle. This is equivalent to setting
    /// the 'Is2DHoverable' and 'Is3DHoverable' to the same value so it can be
    /// considered a shorthand function.
    /// </summary>
    public void SetHoverable(bool isHoverable)
    {
      this.Is2DHoverable = this.Is3DHoverable = isHoverable;
    }

    /// <summary>
    /// Sets the visibility state of the handle. This is equivalent to setting the
    /// 'Is2DVisible' and 'Is3DVisible' to the same value so it can be considered a
    /// shorthand function. Note: If this is set to false, the handle shapes will no
    /// longer be hoverable regardless of the values returned from 'Is2DHoverable' and
    /// 'Is3DHoverable'.
    /// </summary>
    public void SetVisible(bool isVisible) => this.Is2DVisible = this.Is3DVisible = isVisible;

    /// <summary>Returns the 3D shape with the specified index.</summary>
    public Shape3D Get3DShape(int shapeIndex) => this._3DShapes[shapeIndex].Shape;

    /// <summary>Returns the 2D shape with the specified index.</summary>
    public Shape2D Get2DShape(int shapeIndex) => this._2DShapes[shapeIndex].Shape;

    /// <summary>
    /// Sets the visibility state of all the 3D shapes associated with the handle. This
    /// is equivalent to calling the 'Is3DVisible', but the 2 should not be confused.
    /// The reason why this function exists is because you can leave the 'Is3DVisible'
    /// property to true and hide shapes selectively. Later, you can call this function
    /// to restore the visibility state of all shapes as needed. Invisible shapes are also
    /// not hoverable.
    /// </summary>
    public void SetAll3DShapesVisible(bool visible)
    {
      foreach (GizmoHandleShape3D gizmoHandleShape3D in this._3DShapes)
        gizmoHandleShape3D.IsVisible = visible;
    }

    /// <summary>
    /// Allows you to set the visibility state of the 3D shape with the specified index. Not
    /// to be confused with 'Is3DVisible'. 'Is3DVisible' can be used to toggle the visibility
    /// state of the handle whereas this function can be used to toggle the visibility state
    /// of a certain shape that belongs to the handle. Note: If 'Is3DVisible' is false, the
    /// visibility state of the 3D shapes is ignored and all 3D shapes will be considered invisible.
    /// Invisible shapes are also not hoverable.
    /// </summary>
    public void Set3DShapeVisible(int shapeIndex, bool isVisible)
    {
      this._3DShapes[shapeIndex].IsVisible = isVisible;
    }

    /// <summary>
    /// Returns the visibility state of the 3D shape with the specified index. Not to be confused
    /// with 'Is3DVisible'. For example, 'Is3DVisible' can return true, but this function will return
    /// false if a previous call to 'Set3DShapeVisible' was made to hide the shape.
    /// </summary>
    public bool Is3DShapeVisible(int shapeIndex) => this._3DShapes[shapeIndex].IsVisible;

    /// <summary>
    /// Allows you to set the hoverable state of the 3D shape with the specified index. Not to be
    /// confused with 'Is3DHoverable'. 'Is3DHoverable' can be used to toggle the hoverable state
    /// of the handle whereas this function can be used to toggle the hoverable state of a certain
    /// shape that belongs to the handle. Note: If 'Is3DHoverable' is false, the hoverable state of
    /// the 3D shapes is ignored and all 3D shapes will be considered NOT hoverable. Invisible shapes
    /// are not hoverable. So if the same shape was marked as invisible or if 'Is3DVisible' returns
    /// false, the shape is not hoverable.
    /// </summary>
    public void Set3DShapeHoverable(int shapeIndex, bool isHoverable)
    {
      this._3DShapes[shapeIndex].IsHoverable = isHoverable;
    }

    /// <summary>
    /// Sets the visibility state of all the 2D shapes associated with the handle. This
    /// is equivalent to calling the 'Is2DVisible', but the 2 should not be confused.
    /// The reason why this function exists is because you can leave the 'Is2DVisible'
    /// property to true and hide shapes selectively. Later, you can call this function
    /// to restore the visibility state of all shapes as needed. Invisible shapes are also
    /// not hoverable.
    /// </summary>
    public void SetAll2DShapesVisible(bool visible)
    {
      foreach (GizmoHandleShape2D gizmoHandleShape2D in this._2DShapes)
        gizmoHandleShape2D.IsVisible = visible;
    }

    /// <summary>
    /// Allows you to set the visibility state of the 2D shape with the specified index. Not
    /// to be confused with 'Is2DVisible'. 'Is2DVisible' can be used to toggle the visibility
    /// state of the handle whereas this function can be used to toggle the visibility state
    /// of a certain shape that belongs to the handle. Note: If 'Is2DVisible' is false, the
    /// visibility state of the 2D shapes is ignored and all 2D shapes will be considered invisible.
    /// Invisible shapes are also not hoverable.
    /// </summary>
    public void Set2DShapeVisible(int shapeIndex, bool isVisible)
    {
      this._2DShapes[shapeIndex].IsVisible = isVisible;
    }

    /// <summary>
    /// Returns the visibility state of the 2D shape with the specified index. Not to be confused
    /// with 'Is2DVisible'. For example, 'Is2DVisible' can return true, but this function will return
    /// false if a previous call to 'Set2DShapeVisible' was made to hide the shape.
    /// </summary>
    public bool Is2DShapeVisible(int shapeIndex) => this._2DShapes[shapeIndex].IsVisible;

    /// <summary>
    /// Allows you to set the hoverable state of the 2D shape with the specified index. Not to be
    /// confused with 'Is2DHoverable'. 'Is2DHoverable' can be used to toggle the hoverable state
    /// of the handle whereas this function can be used to toggle the hoverable state of a certain
    /// shape that belongs to the handle. Note: If 'Is2DHoverable' is false, the hoverable state of
    /// the 2D shapes is ignored and all 2D shapes will be considered NOT hoverable. Invisible shapes
    /// are not hoverable. So if the same shape was marked as invisible or if 'Is2DVisible' returns
    /// false, the shape is not hoverable.
    /// </summary>
    public void Set2DShapeHoverable(int shapeIndex, bool isHoverable)
    {
      this._2DShapes[shapeIndex].IsHoverable = isHoverable;
    }

    /// <summary>Checks if the handle contains the specified 3D shape.</summary>
    public bool Contains3DShape(Shape3D shape)
    {
      return this._3DShapes.FindAll((Predicate<GizmoHandleShape3D>) (item => item.Shape == shape)).Count != 0;
    }

    /// <summary>Checks if the handle contains the specified 2D shape.</summary>
    public bool Contains2DShape(Shape2D shape)
    {
      return this._2DShapes.FindAll((Predicate<GizmoHandleShape2D>) (item => item.Shape == shape)).Count != 0;
    }

    /// <summary>Adds the specified 3D shape to the handle.</summary>
    /// <returns>
    /// The index of the 3D shape inside the handle's 3D shape collection
    /// or -1 if the shape already exists.
    /// </returns>
    public int Add3DShape(Shape3D shape)
    {
      if (this.Contains3DShape(shape))
        return -1;
      this._3DShapes.Add(new GizmoHandleShape3D(shape));
      return this._3DShapes.Count - 1;
    }

    /// <summary>Adds the specified 2D shape to the handle.</summary>
    /// <returns>
    /// The index of the 2D shape inside the handle's 2D shape collection
    /// or -1 if the shape already exists.
    /// </returns>
    public int Add2DShape(Shape2D shape)
    {
      if (this.Contains2DShape(shape))
        return -1;
      this._2DShapes.Add(new GizmoHandleShape2D(shape));
      return this._2DShapes.Count - 1;
    }

    /// <summary>
    /// Removes the specifed 3D shape. The function has no effect if the handle doesn't contain
    /// the specified shape.
    /// </summary>
    public void Remove3DShape(Shape3D shape)
    {
      this._3DShapes.RemoveAll((Predicate<GizmoHandleShape3D>) (item => item.Shape == shape));
    }

    /// <summary>
    /// Removes the specified 2D shape. The function has no effect if the handle doesn't contain
    /// the specified shape.
    /// </summary>
    public void Remove2DShape(Shape2D shape)
    {
      this._2DShapes.RemoveAll((Predicate<GizmoHandleShape2D>) (item => item.Shape == shape));
    }

    /// <summary>
    /// Renders all 3D shapes as solids. This function will have no effect if 'Is3DVisible'
    /// returns false. Also, the function will ignore shapes which have been marked as invisible.
    /// </summary>
    public void Render3DSolid()
    {
      if (!this.Is3DVisible)
        return;
      foreach (GizmoHandleShape3D gizmoHandleShape3D in this._3DShapes)
      {
        if (gizmoHandleShape3D.IsVisible)
          gizmoHandleShape3D.Shape.RenderSolid();
      }
    }

    /// <summary>
    /// Renders all 3D shapes using a wire representation. This function will have no effect if
    /// 'Is3DVisible' returns false. Also, the function will ignore shapes which have been marked
    /// as invisible.
    /// </summary>
    public void Render3DWire()
    {
      if (!this.Is3DVisible)
        return;
      foreach (GizmoHandleShape3D gizmoHandleShape3D in this._3DShapes)
      {
        if (gizmoHandleShape3D.IsVisible)
          gizmoHandleShape3D.Shape.RenderWire();
      }
    }

    /// <summary>
    /// Renders the 3D shape with the specified index as a solid. The function has no effect if
    /// 'Is3DVisible' returns false or if the shape is invisible.
    /// </summary>
    public void Render3DSolid(int shapeIndex)
    {
      GizmoHandleShape3D gizmoHandleShape3D = this._3DShapes[shapeIndex];
      if (!this.Is3DVisible || !gizmoHandleShape3D.IsVisible)
        return;
      gizmoHandleShape3D.Shape.RenderSolid();
    }

    /// <summary>
    /// Renders the 3D shape with the specified index using a wire representation. The function has
    /// no effect if 'Is3DVisible' returns false or if the shape is invisible.
    /// </summary>
    public void Render3DWire(int shapeIndex)
    {
      GizmoHandleShape3D gizmoHandleShape3D = this._3DShapes[shapeIndex];
      if (!this.Is3DVisible || !gizmoHandleShape3D.IsVisible)
        return;
      gizmoHandleShape3D.Shape.RenderWire();
    }

    /// <summary>
    /// Renders all 2D shapes as solids. This function will have no effect if 'Is2DVisible'
    /// returns false. Also, the function will ignore shapes which have been marked as invisible.
    /// </summary>
    public void Render2DSolid(Camera camera)
    {
      if (!this.Is2DVisible)
        return;
      foreach (GizmoHandleShape2D gizmoHandleShape2D in this._2DShapes)
      {
        if (gizmoHandleShape2D.IsVisible)
          gizmoHandleShape2D.Shape.RenderArea(camera);
      }
    }

    /// <summary>
    /// Renders all 2D shapes using a wire representation. This function will have no effect if
    /// 'Is2DVisible' returns false. Also, the function will ignore shapes which have been marked
    /// as invisible.
    /// </summary>
    public void Render2DWire(Camera camera)
    {
      if (!this.Is2DVisible)
        return;
      foreach (GizmoHandleShape2D gizmoHandleShape2D in this._2DShapes)
      {
        if (gizmoHandleShape2D.IsVisible)
          gizmoHandleShape2D.Shape.RenderBorder(camera);
      }
    }

    /// <summary>
    /// Renders the 2D shape with the specified index as a solid. The function has no effect if
    /// 'Is2DVisible' returns false or if the shape is invisible.
    /// </summary>
    /// <param name="camera">The camera which renders the shape.</param>
    public void Render2DSolid(Camera camera, int shapeIndex)
    {
      GizmoHandleShape2D gizmoHandleShape2D = this._2DShapes[shapeIndex];
      if (!this.Is2DVisible || !gizmoHandleShape2D.IsVisible)
        return;
      gizmoHandleShape2D.Shape.RenderArea(camera);
    }

    /// <summary>
    /// Renders the 2D shape with the specified index using a wire representation. The function has
    /// no effect if 'Is2DVisible' returns false or if the shape is invisible.
    /// </summary>
    /// <param name="camera">The camera which renders the shape.</param>
    public void Render2DWire(Camera camera, int shapeIndex)
    {
      GizmoHandleShape2D gizmoHandleShape2D = this._2DShapes[shapeIndex];
      if (!this.Is2DVisible || !gizmoHandleShape2D.IsVisible)
        return;
      gizmoHandleShape2D.Shape.RenderBorder(camera);
    }

    /// <summary>
    /// This function traverses the handle's 2D and 3D shapes and decides which one is hovered
    /// by the specified ray. It then returns the hover information inside an instance of the
    /// 'GizmoHandleHoverData' class. The ray should be created using the 'Camera.ScreenPointToRay'
    /// function and it represents the ray which is cast out from the screen into the 3D scene.
    /// The function will always give priority to 2D shapes. So for example, if the handle has
    /// a 2D and a 3D shape, and the ray hovers both of them, only the 2D shape will be taken into
    /// account.
    /// </summary>
    /// <param name="hoverRay">
    /// The hover ray. This should be created using the 'Camera.ScreenPointToRay' function. The
    /// function will convert the origin of the ray in screen space to detect the 2D shapes which
    /// are hovered by the ray.
    /// </param>
    /// <returns>
    /// If a shape is hovered by the input ray, the function returns an instance of the
    /// 'GizmoHandleHoverData' class. Otherwise, it returns null. The function also returns
    /// null if there are any subscribers to the 'CanHover' event that don't allow the handle
    /// to be hovered.
    /// </returns>
    public GizmoHandleHoverData GetHoverData(Ray hoverRay)
    {
      float num = float.MaxValue;
      if (this.Is2DHoverable && this.Is2DVisible)
      {
        Vector2 screenPoint = (Vector2) this.Gizmo.GetWorkCamera().WorldToScreenPoint(hoverRay.origin);
        GizmoHandleShape2D gizmoHandleShape2D1 = (GizmoHandleShape2D) null;
        foreach (GizmoHandleShape2D gizmoHandleShape2D2 in this._2DShapes)
        {
          if (gizmoHandleShape2D2.IsVisible && gizmoHandleShape2D2.IsHoverable && gizmoHandleShape2D2.Shape.ContainsPoint(screenPoint))
          {
            float magnitude = (gizmoHandleShape2D2.Shape.GetEncapsulatingRect().center - screenPoint).magnitude;
            if (gizmoHandleShape2D1 == null || (double) magnitude < (double) num)
            {
              gizmoHandleShape2D1 = gizmoHandleShape2D2;
              num = magnitude;
            }
          }
        }
        if (gizmoHandleShape2D1 != null)
        {
          GizmoHandleHoverData handleHoverData = new GizmoHandleHoverData(hoverRay, (IGizmoHandle) this, screenPoint);
          if (this.CanHover != null)
          {
            YesNoAnswer answer = new YesNoAnswer();
            this.CanHover(this.Id, this.Gizmo, handleHoverData, answer);
            if (answer.HasNo)
              return (GizmoHandleHoverData) null;
          }
          return handleHoverData;
        }
      }
      if (this.Is3DHoverable && this.Is3DVisible)
      {
        float hoverEnter3D = float.MaxValue;
        GizmoHandleShape3D gizmoHandleShape3D1 = (GizmoHandleShape3D) null;
        foreach (GizmoHandleShape3D gizmoHandleShape3D2 in this._3DShapes)
        {
          float t;
          if (gizmoHandleShape3D2.IsVisible && gizmoHandleShape3D2.IsHoverable && gizmoHandleShape3D2.Shape.Raycast(hoverRay, out t) && (gizmoHandleShape3D1 == null || (double) t < (double) hoverEnter3D))
          {
            gizmoHandleShape3D1 = gizmoHandleShape3D2;
            hoverEnter3D = t;
          }
        }
        if (gizmoHandleShape3D1 != null)
        {
          GizmoHandleHoverData handleHoverData = new GizmoHandleHoverData(hoverRay, (IGizmoHandle) this, hoverEnter3D);
          if (this.CanHover != null)
          {
            YesNoAnswer answer = new YesNoAnswer();
            this.CanHover(this.Id, this.Gizmo, handleHoverData, answer);
            if (answer.HasNo)
              return (GizmoHandleHoverData) null;
          }
          return handleHoverData;
        }
      }
      return (GizmoHandleHoverData) null;
    }
  }
}
