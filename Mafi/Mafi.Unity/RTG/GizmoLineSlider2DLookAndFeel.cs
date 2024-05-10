// Decompiled with JetBrains decompiler
// Type: RTG.GizmoLineSlider2DLookAndFeel
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
  public class GizmoLineSlider2DLookAndFeel
  {
    [SerializeField]
    private GizmoLine2DType _lineType;
    [SerializeField]
    private GizmoFillMode2D _fillMode;
    [SerializeField]
    private float _length;
    [SerializeField]
    private float _scale;
    [SerializeField]
    private float _boxThickness;
    [SerializeField]
    private bool _isRotationArcVisible;
    [SerializeField]
    private Color _color;
    [SerializeField]
    private Color _hoveredColor;
    [SerializeField]
    private Color _borderColor;
    [SerializeField]
    private Color _hoveredBorderColor;
    [SerializeField]
    private GizmoRotationArc2DLookAndFeel _rotationArcLookAndFeel;
    [SerializeField]
    private GizmoCap2DLookAndFeel _capLookAndFeel;

    public GizmoLine2DType LineType
    {
      get => this._lineType;
      set => this._lineType = value;
    }

    public GizmoFillMode2D FillMode
    {
      get => this._fillMode;
      set => this._fillMode = value;
    }

    public float Length
    {
      get => this._length;
      set => this._length = Mathf.Max(0.0f, value);
    }

    public float Scale
    {
      get => this._scale;
      set => this._scale = Mathf.Max(0.0f, value);
    }

    public GizmoCap2DLookAndFeel CapLookAndFeel => this._capLookAndFeel;

    public float BoxThickness
    {
      get => this._boxThickness;
      set => this._boxThickness = Mathf.Max(0.0f, value);
    }

    public bool IsRotationArcVisible
    {
      get => this._isRotationArcVisible;
      set => this._isRotationArcVisible = value;
    }

    public Color Color
    {
      get => this._color;
      set => this._color = value;
    }

    public Color HoveredColor
    {
      get => this._hoveredColor;
      set => this._hoveredColor = value;
    }

    public Color BorderColor
    {
      get => this._borderColor;
      set => this._borderColor = value;
    }

    public Color HoveredBorderColor
    {
      get => this._hoveredBorderColor;
      set => this._hoveredBorderColor = value;
    }

    public GizmoRotationArc2DLookAndFeel RotationArcLookAndFeel => this._rotationArcLookAndFeel;

    public GizmoLineSlider2DLookAndFeel()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._length = 50f;
      this._scale = 1f;
      this._boxThickness = 3f;
      this._isRotationArcVisible = true;
      this._color = Color.white;
      this._hoveredColor = RTSystemValues.HoveredAxisColor;
      this._borderColor = Color.white;
      this._hoveredBorderColor = RTSystemValues.HoveredAxisColor;
      this._rotationArcLookAndFeel = new GizmoRotationArc2DLookAndFeel();
      this._capLookAndFeel = new GizmoCap2DLookAndFeel();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
