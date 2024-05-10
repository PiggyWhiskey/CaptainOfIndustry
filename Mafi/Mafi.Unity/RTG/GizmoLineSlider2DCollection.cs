// Decompiled with JetBrains decompiler
// Type: RTG.GizmoLineSlider2DCollection
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
  public class GizmoLineSlider2DCollection
  {
    private List<GizmoLineSlider2D> _sliders;
    private Dictionary<int, GizmoLineSlider2D> _handleIdToSlider;

    public int Count => this._sliders.Count;

    public GizmoLineSlider2D this[int id] => this._handleIdToSlider[id];

    public bool Contains(GizmoLineSlider2D slider)
    {
      return this._handleIdToSlider.ContainsKey(slider.HandleId);
    }

    public bool Contains(int sliderHandleId) => this._handleIdToSlider.ContainsKey(sliderHandleId);

    public bool ContainsCapId(int capHandleId)
    {
      return this._sliders.FindAll((Predicate<GizmoLineSlider2D>) (item => item.Cap2DHandleId == capHandleId)).Count != 0;
    }

    public void Add(GizmoLineSlider2D slider)
    {
      if (this.Contains(slider))
        return;
      this._sliders.Add(slider);
      this._handleIdToSlider.Add(slider.HandleId, slider);
    }

    public void Remove(GizmoLineSlider2D slider)
    {
      if (!this.Contains(slider))
        return;
      this._sliders.Remove(slider);
      this._handleIdToSlider.Remove(slider.HandleId);
    }

    public void Make2DHoverPriorityLowerThan(Priority priority)
    {
      foreach (GizmoSlider slider in this._sliders)
        slider.HoverPriority2D.MakeLowerThan(priority);
    }

    public void Make2DHoverPriorityHigherThan(Priority priority)
    {
      foreach (GizmoSlider slider in this._sliders)
        slider.HoverPriority2D.MakeHigherThan(priority);
    }

    public void SetSnapEnabled(bool isEnabled)
    {
      foreach (GizmoSlider slider in this._sliders)
        slider.SetSnapEnabled(isEnabled);
    }

    public void SetVisible(bool visible)
    {
      foreach (GizmoSlider slider in this._sliders)
        slider.SetVisible(visible);
    }

    public void Set2DCapsVisible(bool visible)
    {
      foreach (GizmoLineSlider2D slider in this._sliders)
        slider.Set2DCapVisible(visible);
    }

    public void SetOffsetDragOrigin(Vector3 dragOrigin)
    {
      foreach (GizmoLineSlider2D slider in this._sliders)
        slider.OffsetDragOrigin = dragOrigin;
    }

    public void Render(Camera camera)
    {
      foreach (GizmoSlider slider in this._sliders)
        slider.Render(camera);
    }

    public GizmoLineSlider2DCollection()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._sliders = new List<GizmoLineSlider2D>();
      this._handleIdToSlider = new Dictionary<int, GizmoLineSlider2D>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
