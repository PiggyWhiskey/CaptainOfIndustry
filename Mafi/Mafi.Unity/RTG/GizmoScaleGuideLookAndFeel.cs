// Decompiled with JetBrains decompiler
// Type: RTG.GizmoScaleGuideLookAndFeel
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  [Serializable]
  public class GizmoScaleGuideLookAndFeel
  {
    [SerializeField]
    private bool _useZoomFactor;
    [SerializeField]
    private Color _xAxisColor;
    [SerializeField]
    private Color _yAxisColor;
    [SerializeField]
    private Color _zAxisColor;
    [SerializeField]
    private float _axisLength;

    public bool UseZoomFactor
    {
      get => this._useZoomFactor;
      set => this._useZoomFactor = value;
    }

    public Color XAxisColor
    {
      get => this._xAxisColor;
      set => this._xAxisColor = value;
    }

    public Color YAxisColor
    {
      get => this._yAxisColor;
      set => this._yAxisColor = value;
    }

    public Color ZAxisColor
    {
      get => this._zAxisColor;
      set => this._zAxisColor = value;
    }

    public float AxisLength
    {
      get => this._axisLength;
      set => this._axisLength = Mathf.Max(0.0f, value);
    }

    public GizmoScaleGuideLookAndFeel()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._useZoomFactor = true;
      this._xAxisColor = RTSystemValues.XAxisColor;
      this._yAxisColor = RTSystemValues.YAxisColor;
      this._zAxisColor = RTSystemValues.ZAxisColor;
      this._axisLength = 2f;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
