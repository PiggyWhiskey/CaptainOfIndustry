// Decompiled with JetBrains decompiler
// Type: RTG.DirectionalLightGizmo3DLookAndFeel
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
  public class DirectionalLightGizmo3DLookAndFeel
  {
    [SerializeField]
    private GizmoCap2DLookAndFeel _dirSnapTickLookAndFeel;
    [SerializeField]
    private Color _lightRaysColor;
    [SerializeField]
    private Color _sourceCircleBorderColor;
    [SerializeField]
    private float _sourceCircleRadius;
    [SerializeField]
    private int _numLightRays;
    [SerializeField]
    private float _lightRayLength;
    [SerializeField]
    private Color _dirSnapSegmentColor;

    public static Color DefaultLightRaysColor
    {
      get => ColorEx.FromByteValues((byte) 210, (byte) 210, (byte) 138, byte.MaxValue);
    }

    public static Color DefaultSourceCircleBorderColor
    {
      get => ColorEx.FromByteValues((byte) 210, (byte) 210, (byte) 138, byte.MaxValue);
    }

    public static float DefaultSourceCircleRadius => 2f;

    public static int DefaultNumLightRays => 8;

    public static float DefaultLightRayLength => 10f;

    public static Color DefaultDirSnapSegmentColor => Color.green;

    public static Color DefaultDirSnapTickColor
    {
      get => ColorEx.FromByteValues((byte) 210, (byte) 210, (byte) 138, byte.MaxValue);
    }

    public static float DefaultDirSnapTickQuadWidth => 6f;

    public static float DefaultDirSnapTickQuadHeight => 6f;

    public static float DefaultDirSnapTickCircleRadius => 3f;

    public static GizmoCap2DType DefaultDirSnapTickCapType => GizmoCap2DType.Quad;

    public static Color DefaultDirSnapTickHoveredColor => RTSystemValues.HoveredAxisColor;

    public static Color DefaultDirSnapTickBorderColor => Color.black.KeepAllButAlpha(0.0f);

    public static Color DefaultDirSnapTickHoveredBorderColor => Color.black.KeepAllButAlpha(0.0f);

    public Color LightRaysColor => this._lightRaysColor;

    public Color SourceCircleBorderColor => this._sourceCircleBorderColor;

    public float SourceCircleRadius => this._sourceCircleRadius;

    public int NumLightRays => this._numLightRays;

    public float LightRayLength => this._lightRayLength;

    public Color DirSnapSegmentColor => this._dirSnapSegmentColor;

    public Color DirSnapTickBorderColor => this._dirSnapTickLookAndFeel.BorderColor;

    public Color DirSnapTickHoveredColor => this._dirSnapTickLookAndFeel.HoveredColor;

    public Color DirSnapTickHoveredBorderColor => this._dirSnapTickLookAndFeel.HoveredBorderColor;

    public GizmoCap2DType DirSnapTickType => this._dirSnapTickLookAndFeel.CapType;

    public float DirSnapTickQuadWidth => this._dirSnapTickLookAndFeel.QuadWidth;

    public float DirSnapTickQuadHeight => this._dirSnapTickLookAndFeel.QuadHeight;

    public float DirSnapTickCircleRadius => this._dirSnapTickLookAndFeel.CircleRadius;

    public DirectionalLightGizmo3DLookAndFeel()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._dirSnapTickLookAndFeel = new GizmoCap2DLookAndFeel();
      this._lightRaysColor = DirectionalLightGizmo3DLookAndFeel.DefaultLightRaysColor;
      this._sourceCircleBorderColor = DirectionalLightGizmo3DLookAndFeel.DefaultSourceCircleBorderColor;
      this._sourceCircleRadius = DirectionalLightGizmo3DLookAndFeel.DefaultSourceCircleRadius;
      this._numLightRays = DirectionalLightGizmo3DLookAndFeel.DefaultNumLightRays;
      this._lightRayLength = DirectionalLightGizmo3DLookAndFeel.DefaultLightRayLength;
      this._dirSnapSegmentColor = DirectionalLightGizmo3DLookAndFeel.DefaultDirSnapSegmentColor;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.SetDirSnapTickColor(DirectionalLightGizmo3DLookAndFeel.DefaultDirSnapTickColor);
      this.SetDirSnapTickHoveredColor(DirectionalLightGizmo3DLookAndFeel.DefaultDirSnapTickHoveredColor);
      this.SetDirSnapTickBorderColor(DirectionalLightGizmo3DLookAndFeel.DefaultDirSnapTickBorderColor);
      this.SetDirSnapTickHoveredBorderColor(DirectionalLightGizmo3DLookAndFeel.DefaultDirSnapTickHoveredBorderColor);
      this.SetDirSnapTickQuadWidth(DirectionalLightGizmo3DLookAndFeel.DefaultDirSnapTickQuadWidth);
      this.SetDirSnapTickQuadHeight(DirectionalLightGizmo3DLookAndFeel.DefaultDirSnapTickQuadHeight);
      this.SetDirSnapTickCircleRadius(DirectionalLightGizmo3DLookAndFeel.DefaultDirSnapTickCircleRadius);
      this.SetDirSnapTickType(DirectionalLightGizmo3DLookAndFeel.DefaultDirSnapTickCapType);
    }

    public List<Enum> GetAllowedTickTypes()
    {
      return new List<Enum>()
      {
        (Enum) GizmoCap2DType.Circle,
        (Enum) GizmoCap2DType.Quad
      };
    }

    public bool IsTickTypeAllowed(GizmoCap2DType tickType)
    {
      return tickType == GizmoCap2DType.Circle || tickType == GizmoCap2DType.Quad;
    }

    public void SetNumLightRays(int numLightRays)
    {
      this._numLightRays = Mathf.Max(3, numLightRays);
    }

    public void SetDirSnapSegmentColor(Color color) => this._dirSnapSegmentColor = color;

    public void SetLightRayLength(float length) => this._lightRayLength = Mathf.Max(0.0f, length);

    public void SetSourceCircleRadius(float radius)
    {
      this._sourceCircleRadius = Mathf.Max(0.0001f, radius);
    }

    public void SetLightRaysColor(Color color) => this._lightRaysColor = color;

    public void SetSourceCircleBorderColor(Color color) => this._sourceCircleBorderColor = color;

    public void SetDirSnapTickColor(Color color) => this._dirSnapTickLookAndFeel.Color = color;

    public void SetDirSnapTickBorderColor(Color color)
    {
      this._dirSnapTickLookAndFeel.BorderColor = color;
    }

    public void SetDirSnapTickHoveredColor(Color color)
    {
      this._dirSnapTickLookAndFeel.HoveredColor = color;
    }

    public void SetDirSnapTickHoveredBorderColor(Color color)
    {
      this._dirSnapTickLookAndFeel.HoveredBorderColor = color;
    }

    public void SetDirSnapTickType(GizmoCap2DType tickType)
    {
      this._dirSnapTickLookAndFeel.CapType = tickType;
    }

    public void SetDirSnapTickQuadWidth(float width)
    {
      this._dirSnapTickLookAndFeel.QuadWidth = width;
    }

    public void SetDirSnapTickQuadHeight(float height)
    {
      this._dirSnapTickLookAndFeel.QuadHeight = height;
    }

    public void SetDirSnapTickCircleRadius(float radius)
    {
      this._dirSnapTickLookAndFeel.CircleRadius = radius;
    }

    public void ConnectDirSnapTickLookAndFeel(GizmoCap2D tick)
    {
      tick.SharedLookAndFeel = this._dirSnapTickLookAndFeel;
    }
  }
}
