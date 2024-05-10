// Decompiled with JetBrains decompiler
// Type: RTG.GizmoLineSlider3DCollection
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
  public class GizmoLineSlider3DCollection
  {
    private List<GizmoLineSlider3D> _sliders;
    private Dictionary<int, GizmoLineSlider3D> _handleIdToSlider;

    public int Count => this._sliders.Count;

    public GizmoLineSlider3D this[int id] => this._handleIdToSlider[id];

    public bool Contains(GizmoLineSlider3D slider)
    {
      return this._handleIdToSlider.ContainsKey(slider.HandleId);
    }

    public bool Contains(int sliderHandleId) => this._handleIdToSlider.ContainsKey(sliderHandleId);

    public bool ContainsCapId(int capHandleId)
    {
      return this._sliders.FindAll((Predicate<GizmoLineSlider3D>) (item => item.Cap3DHandleId == capHandleId)).Count != 0;
    }

    public void Add(GizmoLineSlider3D slider)
    {
      if (this.Contains(slider))
        return;
      this._sliders.Add(slider);
      this._handleIdToSlider.Add(slider.HandleId, slider);
    }

    public void Remove(GizmoLineSlider3D slider)
    {
      if (!this.Contains(slider))
        return;
      this._sliders.Remove(slider);
      this._handleIdToSlider.Remove(slider.HandleId);
    }

    public void ApplyZoomFactor(Camera camera)
    {
      foreach (GizmoLineSlider3D slider in this._sliders)
        slider.ApplyZoomFactor(camera);
    }

    public void SetZoomFactorTransform(GizmoTransform zoomFactorTransform)
    {
      foreach (GizmoLineSlider3D slider in this._sliders)
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

    public void SetVisible(bool visible)
    {
      foreach (GizmoSlider slider in this._sliders)
        slider.SetVisible(visible);
    }

    public void Set3DCapsVisible(bool visible)
    {
      foreach (GizmoLineSlider3D slider in this._sliders)
        slider.Set3DCapVisible(visible);
    }

    public void SetDragChannel(GizmoDragChannel dragChannel)
    {
      foreach (GizmoLineSlider3D slider in this._sliders)
        slider.SetDragChannel(dragChannel);
    }

    public void RegisterScalerHandle(int handleId, IEnumerable<int> scaleDragAxisIndices)
    {
      foreach (GizmoLineSlider3D slider in this._sliders)
        slider.RegisterScalerHandle(handleId, scaleDragAxisIndices);
    }

    public List<GizmoLineSlider3D> GetRenderSortedSliders(Camera renderCamera)
    {
      List<GizmoLineSlider3D> renderSortedSliders = new List<GizmoLineSlider3D>((IEnumerable<GizmoLineSlider3D>) this._sliders);
      Vector3 cameraPos = renderCamera.transform.position;
      renderSortedSliders.Sort((Comparison<GizmoLineSlider3D>) ((s0, s1) =>
      {
        float sqrMagnitude = (s0.GetRealEndPosition(s0.GetZoomFactor(renderCamera)) - cameraPos).sqrMagnitude;
        return (s1.GetRealEndPosition(s1.GetZoomFactor(renderCamera)) - cameraPos).sqrMagnitude.CompareTo(sqrMagnitude);
      }));
      return renderSortedSliders;
    }

    public GizmoLineSlider3DCollection()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._sliders = new List<GizmoLineSlider3D>();
      this._handleIdToSlider = new Dictionary<int, GizmoLineSlider3D>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
