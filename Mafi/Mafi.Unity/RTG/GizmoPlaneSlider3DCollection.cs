// Decompiled with JetBrains decompiler
// Type: RTG.GizmoPlaneSlider3DCollection
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
  public class GizmoPlaneSlider3DCollection
  {
    private List<GizmoPlaneSlider3D> _sliders;
    private Dictionary<int, GizmoPlaneSlider3D> _handleIdToSlider;

    public int Count => this._sliders.Count;

    public GizmoPlaneSlider3D this[int id] => this._handleIdToSlider[id];

    public bool Contains(GizmoPlaneSlider3D slider)
    {
      return this._handleIdToSlider.ContainsKey(slider.HandleId);
    }

    public bool Contains(int sliderHandleId) => this._handleIdToSlider.ContainsKey(sliderHandleId);

    public void Add(GizmoPlaneSlider3D slider)
    {
      if (this.Contains(slider))
        return;
      this._sliders.Add(slider);
      this._handleIdToSlider.Add(slider.HandleId, slider);
    }

    public void Remove(GizmoPlaneSlider3D slider)
    {
      if (!this.Contains(slider))
        return;
      this._sliders.Remove(slider);
      this._handleIdToSlider.Remove(slider.HandleId);
    }

    public void ApplyZoomFactor(Camera camera)
    {
      foreach (GizmoPlaneSlider3D slider in this._sliders)
        slider.ApplyZoomFactor(camera);
    }

    public void SetZoomFactorTransform(GizmoTransform zoomFactorTransform)
    {
      foreach (GizmoPlaneSlider3D slider in this._sliders)
        slider.SetZoomFactorTransform(zoomFactorTransform);
    }

    public void Make3DHoverPriorityLowerThan(Priority priority)
    {
      foreach (GizmoSlider slider in this._sliders)
        slider.HoverPriority3D.MakeLowerThan(priority);
    }

    public void Make3DHoverPriorityHigherThan(Priority priority)
    {
      foreach (GizmoSlider slider in this._sliders)
        slider.HoverPriority3D.MakeHigherThan(priority);
    }

    public void SetSnapEnabled(bool isEnabled)
    {
      foreach (GizmoSlider slider in this._sliders)
        slider.SetSnapEnabled(isEnabled);
    }

    public void SetVisible(bool isVisible, bool includeBorder)
    {
      if (includeBorder)
      {
        foreach (GizmoPlaneSlider3D slider in this._sliders)
        {
          slider.SetVisible(isVisible);
          slider.SetBorderVisible(isVisible);
        }
      }
      else
      {
        foreach (GizmoSlider slider in this._sliders)
          slider.SetVisible(isVisible);
      }
    }

    public void SetBorderVisible(bool isVisible)
    {
      foreach (GizmoPlaneSlider3D slider in this._sliders)
        slider.SetBorderVisible(isVisible);
    }

    public void SetHoverable(bool isHoverable, bool includeBorder)
    {
      if (includeBorder)
      {
        foreach (GizmoPlaneSlider3D slider in this._sliders)
        {
          slider.SetHoverable(isHoverable);
          slider.SetBorderHoverable(isHoverable);
        }
      }
      else
      {
        foreach (GizmoSlider slider in this._sliders)
          slider.SetHoverable(isHoverable);
      }
    }

    public void SetBorderHoverable(bool isHoverable)
    {
      foreach (GizmoPlaneSlider3D slider in this._sliders)
        slider.SetBorderHoverable(isHoverable);
    }

    public List<GizmoPlaneSlider3D> GetRenderSortedSliders(Camera renderCamera)
    {
      List<GizmoPlaneSlider3D> renderSortedSliders = new List<GizmoPlaneSlider3D>((IEnumerable<GizmoPlaneSlider3D>) this._sliders);
      Vector3 cameraPos = renderCamera.transform.position;
      renderSortedSliders.Sort((Comparison<GizmoPlaneSlider3D>) ((s0, s1) =>
      {
        float sqrMagnitude = (s0.Position - cameraPos).sqrMagnitude;
        return (s1.Position - cameraPos).sqrMagnitude.CompareTo(sqrMagnitude);
      }));
      return renderSortedSliders;
    }

    public GizmoPlaneSlider3DCollection()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._sliders = new List<GizmoPlaneSlider3D>();
      this._handleIdToSlider = new Dictionary<int, GizmoPlaneSlider3D>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
