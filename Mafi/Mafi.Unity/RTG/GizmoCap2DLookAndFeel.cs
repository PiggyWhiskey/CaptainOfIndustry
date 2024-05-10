// Decompiled with JetBrains decompiler
// Type: RTG.GizmoCap2DLookAndFeel
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
  public class GizmoCap2DLookAndFeel
  {
    [SerializeField]
    private GizmoFillMode2D _fillMode;
    [SerializeField]
    private GizmoCap2DType _capType;
    [SerializeField]
    private float _scale;
    [SerializeField]
    private float _circleRadius;
    [SerializeField]
    private float _quadWidth;
    [SerializeField]
    private float _quadHeight;
    [SerializeField]
    private float _arrowBaseRadius;
    [SerializeField]
    private float _arrowHeight;
    [SerializeField]
    private Color _color;
    [SerializeField]
    private Color _hoveredColor;
    [SerializeField]
    private Color _borderColor;
    [SerializeField]
    private Color _hoveredBorderColor;

    public GizmoFillMode2D FillMode
    {
      get => this._fillMode;
      set => this._fillMode = value;
    }

    public GizmoCap2DType CapType
    {
      get => this._capType;
      set => this._capType = value;
    }

    public float Scale
    {
      get => this._scale;
      set => this._scale = Mathf.Max(0.0f, value);
    }

    public float CircleRadius
    {
      get => this._circleRadius;
      set => this._circleRadius = value;
    }

    public float QuadWidth
    {
      get => this._quadWidth;
      set => this._quadWidth = Mathf.Max(0.0f, value);
    }

    public float QuadHeight
    {
      get => this._quadHeight;
      set => this._quadHeight = Mathf.Max(0.0f, value);
    }

    public float ArrowBaseRadius
    {
      get => this._arrowBaseRadius;
      set => this._arrowBaseRadius = Mathf.Max(0.0f, value);
    }

    public float ArrowHeight
    {
      get => this._arrowHeight;
      set => this._arrowHeight = Mathf.Max(0.0f, value);
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

    public GizmoCap2DLookAndFeel()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._fillMode = GizmoFillMode2D.FilledAndBorder;
      this._scale = 1f;
      this._circleRadius = 12f;
      this._quadWidth = 25f;
      this._quadHeight = 25f;
      this._arrowBaseRadius = 5f;
      this._arrowHeight = 20f;
      this._color = Color.white;
      this._hoveredColor = RTSystemValues.HoveredAxisColor;
      this._borderColor = Color.white;
      this._hoveredBorderColor = RTSystemValues.HoveredAxisColor;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
