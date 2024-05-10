// Decompiled with JetBrains decompiler
// Type: RTG.CharacterControllerGizmo3DLookAndFeel
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
  public class CharacterControllerGizmo3DLookAndFeel
  {
    [SerializeField]
    private Color _wireColor;
    [SerializeField]
    private GizmoCap2DLookAndFeel[] _tickLookAndFeel;
    [SerializeField]
    private float _tickCullAlphaScale;

    public static Color DefaultWireColor
    {
      get => ColorEx.FromByteValues((byte) 153, (byte) 232, (byte) 144, byte.MaxValue);
    }

    public static Color DefaultTickColor
    {
      get => ColorEx.FromByteValues((byte) 153, (byte) 232, (byte) 144, byte.MaxValue);
    }

    public static float DefaultTickQuadWidth => 6f;

    public static float DefaultTickQuadHeight => 6f;

    public static float DefaultTickCircleRadius => 3f;

    public static float DefaultTickCullAlphaScale => 0.3f;

    public static GizmoCap2DType DefaultTickCapType => GizmoCap2DType.Quad;

    public static Color DefaultTickHoveredColor => RTSystemValues.HoveredAxisColor;

    public static Color DefaultTickBorderColor => Color.black.KeepAllButAlpha(0.0f);

    public static Color DefaultTickHoveredBorderColor => Color.black.KeepAllButAlpha(0.0f);

    public Color WireColor => this._wireColor;

    public Color XTickColor => this.GetTickLookAndFeel(0, AxisSign.Positive).Color;

    public Color YTickColor => this.GetTickLookAndFeel(1, AxisSign.Positive).Color;

    public Color ZTickColor => this.GetTickLookAndFeel(2, AxisSign.Positive).Color;

    public Color TickBorderColor => this.GetTickLookAndFeel(0, AxisSign.Positive).BorderColor;

    public Color TickHoveredColor => this.GetTickLookAndFeel(0, AxisSign.Positive).HoveredColor;

    public Color TickHoveredBorderColor
    {
      get => this.GetTickLookAndFeel(0, AxisSign.Positive).HoveredBorderColor;
    }

    public GizmoCap2DType TickType => this.GetTickLookAndFeel(0, AxisSign.Positive).CapType;

    public float TickQuadWidth => this.GetTickLookAndFeel(0, AxisSign.Positive).QuadWidth;

    public float TickQuadHeight => this.GetTickLookAndFeel(0, AxisSign.Positive).QuadHeight;

    public float TickCircleRadius => this.GetTickLookAndFeel(0, AxisSign.Positive).CircleRadius;

    public float TickCullAlphaScale => this._tickCullAlphaScale;

    public CharacterControllerGizmo3DLookAndFeel()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._wireColor = CharacterControllerGizmo3DLookAndFeel.DefaultWireColor;
      this._tickLookAndFeel = new GizmoCap2DLookAndFeel[6];
      this._tickCullAlphaScale = CharacterControllerGizmo3DLookAndFeel.DefaultTickCullAlphaScale;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      for (int index = 0; index < this._tickLookAndFeel.Length; ++index)
        this._tickLookAndFeel[index] = new GizmoCap2DLookAndFeel();
      this.SetTickColor(0, CharacterControllerGizmo3DLookAndFeel.DefaultTickColor);
      this.SetTickColor(1, CharacterControllerGizmo3DLookAndFeel.DefaultTickColor);
      this.SetTickColor(2, CharacterControllerGizmo3DLookAndFeel.DefaultTickColor);
      this.SetTickHoveredColor(CharacterControllerGizmo3DLookAndFeel.DefaultTickHoveredColor);
      this.SetTickBorderColor(CharacterControllerGizmo3DLookAndFeel.DefaultTickBorderColor);
      this.SetTickHoveredBorderColor(CharacterControllerGizmo3DLookAndFeel.DefaultTickHoveredBorderColor);
      this.SetTickQuadWidth(CharacterControllerGizmo3DLookAndFeel.DefaultTickQuadWidth);
      this.SetTickQuadHeight(CharacterControllerGizmo3DLookAndFeel.DefaultTickQuadHeight);
      this.SetTickCircleRadius(CharacterControllerGizmo3DLookAndFeel.DefaultTickCircleRadius);
      this.SetTickType(CharacterControllerGizmo3DLookAndFeel.DefaultTickCapType);
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

    public void SetWireColor(Color color) => this._wireColor = color;

    public void SetTickCullAlphaScale(float alphaScale)
    {
      this._tickCullAlphaScale = Mathf.Clamp(alphaScale, 0.0f, 1f);
    }

    public void SetTickColor(int axisIndex, Color color)
    {
      this.GetTickLookAndFeel(axisIndex, AxisSign.Positive).Color = color;
      this.GetTickLookAndFeel(axisIndex, AxisSign.Negative).Color = color;
    }

    public void SetAllTicksColor(Color color)
    {
      this.GetTickLookAndFeel(0, AxisSign.Positive).Color = color;
      this.GetTickLookAndFeel(0, AxisSign.Negative).Color = color;
      this.GetTickLookAndFeel(1, AxisSign.Positive).Color = color;
      this.GetTickLookAndFeel(1, AxisSign.Negative).Color = color;
      this.GetTickLookAndFeel(2, AxisSign.Positive).Color = color;
      this.GetTickLookAndFeel(2, AxisSign.Negative).Color = color;
    }

    public void SetTickBorderColor(Color color)
    {
      foreach (GizmoCap2DLookAndFeel cap2DlookAndFeel in this._tickLookAndFeel)
        cap2DlookAndFeel.BorderColor = color;
    }

    public void SetTickHoveredColor(Color color)
    {
      foreach (GizmoCap2DLookAndFeel cap2DlookAndFeel in this._tickLookAndFeel)
        cap2DlookAndFeel.HoveredColor = color;
    }

    public void SetTickHoveredBorderColor(Color color)
    {
      foreach (GizmoCap2DLookAndFeel cap2DlookAndFeel in this._tickLookAndFeel)
        cap2DlookAndFeel.HoveredBorderColor = color;
    }

    public void SetTickType(GizmoCap2DType tickType)
    {
      if (!this.IsTickTypeAllowed(tickType))
        return;
      foreach (GizmoCap2DLookAndFeel cap2DlookAndFeel in this._tickLookAndFeel)
        cap2DlookAndFeel.CapType = tickType;
    }

    public void SetTickQuadWidth(float width)
    {
      foreach (GizmoCap2DLookAndFeel cap2DlookAndFeel in this._tickLookAndFeel)
        cap2DlookAndFeel.QuadWidth = width;
    }

    public void SetTickQuadHeight(float height)
    {
      foreach (GizmoCap2DLookAndFeel cap2DlookAndFeel in this._tickLookAndFeel)
        cap2DlookAndFeel.QuadHeight = height;
    }

    public void SetTickCircleRadius(float radius)
    {
      foreach (GizmoCap2DLookAndFeel cap2DlookAndFeel in this._tickLookAndFeel)
        cap2DlookAndFeel.CircleRadius = radius;
    }

    public void ConnectTickLookAndFeel(GizmoCap2D tick, int axisIndex, AxisSign axisSign)
    {
      tick.SharedLookAndFeel = this.GetTickLookAndFeel(axisIndex, axisSign);
    }

    private GizmoCap2DLookAndFeel GetTickLookAndFeel(int axisIndex, AxisSign axisSign)
    {
      return axisSign == AxisSign.Positive ? this._tickLookAndFeel[axisIndex] : this._tickLookAndFeel[axisIndex + 3];
    }
  }
}
