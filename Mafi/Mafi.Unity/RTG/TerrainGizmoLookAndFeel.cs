// Decompiled with JetBrains decompiler
// Type: RTG.TerrainGizmoLookAndFeel
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
  public class TerrainGizmoLookAndFeel
  {
    [SerializeField]
    private Color _radiusCircleColor;
    [SerializeField]
    private GizmoLineSlider3DLookAndFeel _axisSliderLookAndFeel;
    [SerializeField]
    private GizmoCap3DLookAndFeel _midCapLookAndFeel;
    [SerializeField]
    private GizmoCap2DLookAndFeel _radiusTickLookAndFeel;

    public Color AxisSliderColor
    {
      get => this._axisSliderLookAndFeel.Color;
      set => this._axisSliderLookAndFeel.Color = value;
    }

    public Color AxisSliderHoveredColor
    {
      get => this._axisSliderLookAndFeel.HoveredColor;
      set => this._axisSliderLookAndFeel.HoveredColor = value;
    }

    public Color AxisSliderCapColor
    {
      get => this._axisSliderLookAndFeel.CapLookAndFeel.Color;
      set => this._axisSliderLookAndFeel.CapLookAndFeel.Color = value;
    }

    public Color AxisSliderCapHoveredColor
    {
      get => this._axisSliderLookAndFeel.CapLookAndFeel.HoveredColor;
      set => this._axisSliderLookAndFeel.CapLookAndFeel.HoveredColor = value;
    }

    public GizmoCap3DType AxisSliderCapType
    {
      get => this._axisSliderLookAndFeel.CapLookAndFeel.CapType;
      set => this._axisSliderLookAndFeel.CapLookAndFeel.CapType = value;
    }

    public GizmoLine3DType AxisSliderLineType
    {
      get => this._axisSliderLookAndFeel.LineType;
      set => this._axisSliderLookAndFeel.LineType = value;
    }

    public float AxisSliderLength
    {
      get => this._axisSliderLookAndFeel.Length;
      set => this._axisSliderLookAndFeel.Length = value;
    }

    public GizmoCap3DType MidCapType
    {
      get => this._midCapLookAndFeel.CapType;
      set
      {
        if (value != GizmoCap3DType.Box && value != GizmoCap3DType.Sphere)
          return;
        this._midCapLookAndFeel.CapType = value;
      }
    }

    public float MidCapBoxWidth
    {
      get => this._midCapLookAndFeel.BoxWidth;
      set => this._midCapLookAndFeel.BoxWidth = value;
    }

    public float MidCapBoxHeight
    {
      get => this._midCapLookAndFeel.BoxHeight;
      set => this._midCapLookAndFeel.BoxHeight = value;
    }

    public float MidCapBoxDepth
    {
      get => this._midCapLookAndFeel.BoxDepth;
      set => this._midCapLookAndFeel.BoxDepth = value;
    }

    public float MidSphereRadius
    {
      get => this._midCapLookAndFeel.SphereRadius;
      set => this._midCapLookAndFeel.SphereRadius = value;
    }

    public Color MidCapColor
    {
      get => this._midCapLookAndFeel.Color;
      set => this._midCapLookAndFeel.Color = value;
    }

    public Color MidCapHoveredColor
    {
      get => this._midCapLookAndFeel.HoveredColor;
      set => this._midCapLookAndFeel.HoveredColor = value;
    }

    public Color RadiusCircleColor
    {
      get => this._radiusCircleColor;
      set => this._radiusCircleColor = value;
    }

    public GizmoCap2DType RadiusTickType
    {
      get => this._radiusTickLookAndFeel.CapType;
      set
      {
        if (value != GizmoCap2DType.Circle && value != GizmoCap2DType.Quad)
          return;
        this._radiusTickLookAndFeel.CapType = value;
      }
    }

    public Color RadiusTickColor
    {
      get => this._radiusTickLookAndFeel.Color;
      set => this._radiusTickLookAndFeel.Color = value;
    }

    public Color RadiusTickHoveredColor
    {
      get => this._radiusTickLookAndFeel.HoveredColor;
      set => this._radiusTickLookAndFeel.HoveredColor = value;
    }

    public float RadiusTickQuadWidth
    {
      get => this._radiusTickLookAndFeel.QuadWidth;
      set => this._radiusTickLookAndFeel.QuadWidth = value;
    }

    public float RadiusTickQuadHeight
    {
      get => this._radiusTickLookAndFeel.QuadHeight;
      set => this._radiusTickLookAndFeel.QuadHeight = value;
    }

    public float RadiusTickCircleRadius
    {
      get => this._radiusTickLookAndFeel.CircleRadius;
      set => this._radiusTickLookAndFeel.CircleRadius = value;
    }

    public TerrainGizmoLookAndFeel()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._radiusCircleColor = Color.white;
      this._axisSliderLookAndFeel = new GizmoLineSlider3DLookAndFeel();
      this._midCapLookAndFeel = new GizmoCap3DLookAndFeel();
      this._radiusTickLookAndFeel = new GizmoCap2DLookAndFeel();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.AxisSliderColor = Color.red;
      this.AxisSliderCapColor = Color.red;
      this.AxisSliderCapType = GizmoCap3DType.Cone;
      this.AxisSliderLineType = GizmoLine3DType.Thin;
      this.AxisSliderLength = 5f;
      this.MidCapType = GizmoCap3DType.Box;
      this.MidCapBoxWidth = 0.7f;
      this.MidCapBoxHeight = 0.7f;
      this.MidCapBoxDepth = 0.7f;
      this.MidSphereRadius = 0.35f;
      this.MidCapColor = Color.green;
      this.RadiusTickType = GizmoCap2DType.Quad;
      this.RadiusTickColor = Color.green;
      this.RadiusTickQuadWidth = 8f;
      this.RadiusTickQuadHeight = 8f;
      this.RadiusTickCircleRadius = 4f;
      this._radiusTickLookAndFeel.BorderColor = Color.white.KeepAllButAlpha(0.0f);
    }

    public void ConnectAxisSliderLookAndFeel(GizmoLineSlider3D axisSlider)
    {
      axisSlider.SharedLookAndFeel = this._axisSliderLookAndFeel;
    }

    public void ConnectMidCapLookAndFeel(GizmoCap3D pickPointCap)
    {
      pickPointCap.SharedLookAndFeel = this._midCapLookAndFeel;
    }

    public void ConnectRadiusTickLookAndFeel(GizmoCap2D radiusTick)
    {
      radiusTick.SharedLookAndFeel = this._radiusTickLookAndFeel;
    }
  }
}
