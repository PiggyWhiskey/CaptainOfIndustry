// Decompiled with JetBrains decompiler
// Type: RTG.SpotLightGizmo3DLookAndFeel
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
  public class SpotLightGizmo3DLookAndFeel
  {
    [SerializeField]
    private Color _wireColor;
    [SerializeField]
    private GizmoCap2DLookAndFeel _tickLookAndFeel;
    [SerializeField]
    private GizmoCap2DLookAndFeel _dirSnapTickLookAndFeel;
    [SerializeField]
    private Color _dirSnapSegmentColor;

    public static Color DefaultWireColor
    {
      get => ColorEx.FromByteValues((byte) 210, (byte) 210, (byte) 138, byte.MaxValue);
    }

    public static Color DefaultTickColor
    {
      get => ColorEx.FromByteValues((byte) 210, (byte) 210, (byte) 138, byte.MaxValue);
    }

    public static float DefaultTickQuadWidth => 6f;

    public static float DefaultTickQuadHeight => 6f;

    public static float DefaultTickCircleRadius => 3f;

    public static GizmoCap2DType DefaultTickCapType => GizmoCap2DType.Quad;

    public static Color DefaultTickHoveredColor => RTSystemValues.HoveredAxisColor;

    public static Color DefaultTickBorderColor => Color.black.KeepAllButAlpha(0.0f);

    public static Color DefaultTickHoveredBorderColor => Color.black.KeepAllButAlpha(0.0f);

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

    public Color WireColor => this._wireColor;

    public Color DirSnapSegmentColor => this._dirSnapSegmentColor;

    public Color DirSnapTickBorderColor => this._dirSnapTickLookAndFeel.BorderColor;

    public Color DirSnapTickHoveredColor => this._dirSnapTickLookAndFeel.HoveredColor;

    public Color DirSnapTickHoveredBorderColor => this._dirSnapTickLookAndFeel.HoveredBorderColor;

    public GizmoCap2DType DirSnapTickType => this._dirSnapTickLookAndFeel.CapType;

    public float DirSnapTickQuadWidth => this._dirSnapTickLookAndFeel.QuadWidth;

    public float DirSnapTickQuadHeight => this._dirSnapTickLookAndFeel.QuadHeight;

    public float DirSnapTickCircleRadius => this._dirSnapTickLookAndFeel.CircleRadius;

    public Color TickBorderColor => this._tickLookAndFeel.BorderColor;

    public Color TickHoveredColor => this._tickLookAndFeel.HoveredColor;

    public Color TickHoveredBorderColor => this._tickLookAndFeel.HoveredBorderColor;

    public GizmoCap2DType TickType => this._tickLookAndFeel.CapType;

    public float TickQuadWidth => this._tickLookAndFeel.QuadWidth;

    public float TickQuadHeight => this._tickLookAndFeel.QuadHeight;

    public float TickCircleRadius => this._tickLookAndFeel.CircleRadius;

    public SpotLightGizmo3DLookAndFeel()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._wireColor = SpotLightGizmo3DLookAndFeel.DefaultWireColor;
      this._tickLookAndFeel = new GizmoCap2DLookAndFeel();
      this._dirSnapTickLookAndFeel = new GizmoCap2DLookAndFeel();
      this._dirSnapSegmentColor = SpotLightGizmo3DLookAndFeel.DefaultDirSnapSegmentColor;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.SetDirSnapTickColor(SpotLightGizmo3DLookAndFeel.DefaultDirSnapTickColor);
      this.SetDirSnapTickHoveredColor(SpotLightGizmo3DLookAndFeel.DefaultDirSnapTickHoveredColor);
      this.SetDirSnapTickBorderColor(SpotLightGizmo3DLookAndFeel.DefaultDirSnapTickBorderColor);
      this.SetDirSnapTickHoveredBorderColor(SpotLightGizmo3DLookAndFeel.DefaultDirSnapTickHoveredBorderColor);
      this.SetDirSnapTickQuadWidth(SpotLightGizmo3DLookAndFeel.DefaultDirSnapTickQuadWidth);
      this.SetDirSnapTickQuadHeight(SpotLightGizmo3DLookAndFeel.DefaultDirSnapTickQuadHeight);
      this.SetDirSnapTickCircleRadius(SpotLightGizmo3DLookAndFeel.DefaultDirSnapTickCircleRadius);
      this.SetDirSnapTickType(SpotLightGizmo3DLookAndFeel.DefaultDirSnapTickCapType);
      this.SetTickColor(SpotLightGizmo3DLookAndFeel.DefaultTickColor);
      this.SetTickHoveredColor(SpotLightGizmo3DLookAndFeel.DefaultTickHoveredColor);
      this.SetTickBorderColor(SpotLightGizmo3DLookAndFeel.DefaultTickBorderColor);
      this.SetTickHoveredBorderColor(SpotLightGizmo3DLookAndFeel.DefaultTickHoveredBorderColor);
      this.SetTickQuadWidth(SpotLightGizmo3DLookAndFeel.DefaultTickQuadWidth);
      this.SetTickQuadHeight(SpotLightGizmo3DLookAndFeel.DefaultTickQuadHeight);
      this.SetTickCircleRadius(SpotLightGizmo3DLookAndFeel.DefaultTickCircleRadius);
      this.SetTickType(SpotLightGizmo3DLookAndFeel.DefaultTickCapType);
    }

    public void SetWireColor(Color color) => this._wireColor = color;

    public void SetTickColor(Color color) => this._tickLookAndFeel.Color = color;

    public void SetTickBorderColor(Color color) => this._tickLookAndFeel.BorderColor = color;

    public void SetTickHoveredColor(Color color) => this._tickLookAndFeel.HoveredColor = color;

    public void SetTickHoveredBorderColor(Color color)
    {
      this._tickLookAndFeel.HoveredBorderColor = color;
    }

    public void SetTickType(GizmoCap2DType tickType)
    {
      if (!this.IsTickTypeAllowed(tickType))
        return;
      this._tickLookAndFeel.CapType = tickType;
    }

    public void SetTickQuadWidth(float width) => this._tickLookAndFeel.QuadWidth = width;

    public void SetTickQuadHeight(float height) => this._tickLookAndFeel.QuadHeight = height;

    public void SetTickCircleRadius(float radius) => this._tickLookAndFeel.CircleRadius = radius;

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

    public void SetDirSnapSegmentColor(Color color) => this._dirSnapSegmentColor = color;

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

    public void ConnectTickLookAndFeel(GizmoCap2D tick)
    {
      tick.SharedLookAndFeel = this._tickLookAndFeel;
    }
  }
}
