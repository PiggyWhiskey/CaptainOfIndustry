// Decompiled with JetBrains decompiler
// Type: RTG.SceneGizmoLookAndFeel
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
  public class SceneGizmoLookAndFeel : Settings
  {
    private static readonly float _baseScreenSize;
    private static readonly float _invBaseScreenSize;
    [SerializeField]
    private GizmoCap3DLookAndFeel _midCapLookAndFeel;
    [SerializeField]
    private GizmoCap3DLookAndFeel[] _axesCapsLookAndFeel;
    [SerializeField]
    private SceneGizmoScreenCorner _screenCorner;
    [SerializeField]
    private Vector2 _screenOffset;
    [SerializeField]
    private float _screenSize;
    [SerializeField]
    private Color _axesLabelTint;
    [SerializeField]
    private Color _camPrjSwitchLabelTint;
    [SerializeField]
    private bool _isCamPrjSwitchLabelVisible;

    private GizmoCap3DLookAndFeel AxisCapLookAndFeel => this._axesCapsLookAndFeel[0];

    public SceneGizmoScreenCorner ScreenCorner
    {
      get => this._screenCorner;
      set => this._screenCorner = value;
    }

    public Vector2 ScreenOffset
    {
      get => this._screenOffset;
      set => this._screenOffset = value;
    }

    public float ScreenSize
    {
      get => this._screenSize;
      set
      {
        this._screenSize = Mathf.Max(2f, value);
        this.OnScreenSizeChanged();
      }
    }

    public Color AxesLabelTint
    {
      get => this._axesLabelTint;
      set => this._axesLabelTint = value;
    }

    public Color CamPrjSwitchLabelTint
    {
      get => this._camPrjSwitchLabelTint;
      set => this._camPrjSwitchLabelTint = value;
    }

    public bool IsCamPrjSwitchLabelVisible
    {
      get => this._isCamPrjSwitchLabelVisible;
      set => this._isCamPrjSwitchLabelVisible = value;
    }

    public Texture2D CamPerspModeLabelTexture => Singleton<TexturePool>.Get.CamPerspMode;

    public Texture2D CamOrthoModeLabelTexture => Singleton<TexturePool>.Get.CamOrthoMode;

    public Color HoveredColor => this.AxisCapLookAndFeel.HoveredColor;

    public GizmoCap3DType AxesCapType => this.AxisCapLookAndFeel.CapType;

    public GizmoCap3DType MidCapType => this._midCapLookAndFeel.CapType;

    public float MidCapBoxSize
    {
      get => 1.01f * this._screenSize * SceneGizmoLookAndFeel._invBaseScreenSize;
    }

    public float MidCapSphereRadius
    {
      get => 0.73f * this._screenSize * SceneGizmoLookAndFeel._invBaseScreenSize;
    }

    public float AxisConeHeight
    {
      get
      {
        return GizmoCap3DLookAndFeel.DefaultConeHeight * this._screenSize * SceneGizmoLookAndFeel._invBaseScreenSize;
      }
    }

    public float AxisConeRadius
    {
      get
      {
        return GizmoCap3DLookAndFeel.DefaultConeRadius * this._screenSize * SceneGizmoLookAndFeel._invBaseScreenSize;
      }
    }

    public float AxisPyramidWidth
    {
      get
      {
        return GizmoCap3DLookAndFeel.DefaultPyramidWidth * this._screenSize * SceneGizmoLookAndFeel._invBaseScreenSize;
      }
    }

    public float AxisPyramidHeight
    {
      get
      {
        return GizmoCap3DLookAndFeel.DefaultPyramidHeight * this._screenSize * SceneGizmoLookAndFeel._invBaseScreenSize;
      }
    }

    public float AxisPyramidDepth
    {
      get
      {
        return GizmoCap3DLookAndFeel.DefaultPyramidDepth * this._screenSize * SceneGizmoLookAndFeel._invBaseScreenSize;
      }
    }

    public float AxisLabelScreenSize
    {
      get => 10f * this._screenSize * SceneGizmoLookAndFeel._invBaseScreenSize;
    }

    public float AxisCamAlignFadeOutThreshold => 0.91f;

    public float AxisCamAlignFadeOutDuration => 0.2f;

    public float AxisCamAlignFadeOutAlpha => 0.0f;

    public SceneGizmoLookAndFeel()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._midCapLookAndFeel = new GizmoCap3DLookAndFeel();
      this._axesCapsLookAndFeel = new GizmoCap3DLookAndFeel[6];
      this._screenCorner = SceneGizmoScreenCorner.TopRight;
      this._screenOffset = Vector2.zero;
      this._screenSize = 90f;
      this._axesLabelTint = Color.white;
      this._camPrjSwitchLabelTint = Color.white.KeepAllButAlpha(0.7f);
      this._isCamPrjSwitchLabelVisible = true;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      for (int index = 0; index < this._axesCapsLookAndFeel.Length; ++index)
        this._axesCapsLookAndFeel[index] = new GizmoCap3DLookAndFeel();
      this.SetMidCapColor(RTSystemValues.CenterAxisColor);
      this.SetMidCapType(GizmoCap3DType.Box);
      this.SetAxisCapColor(RTSystemValues.XAxisColor, 0, AxisSign.Positive);
      this.SetAxisCapColor(RTSystemValues.YAxisColor, 1, AxisSign.Positive);
      this.SetAxisCapColor(RTSystemValues.ZAxisColor, 2, AxisSign.Positive);
      this.SetAxisCapColor(RTSystemValues.CenterAxisColor, 0, AxisSign.Negative);
      this.SetAxisCapColor(RTSystemValues.CenterAxisColor, 1, AxisSign.Negative);
      this.SetAxisCapColor(RTSystemValues.CenterAxisColor, 2, AxisSign.Negative);
      this.SetAxisCapType(GizmoCap3DType.Cone);
      this.OnScreenSizeChanged();
    }

    public void SetMidCapColor(Color color) => this._midCapLookAndFeel.Color = color;

    public void SetAxisCapColor(Color color, int axisIndex, AxisSign axisSign)
    {
      this.GetAxisCapLookAndFeel(axisIndex, axisSign).Color = color;
    }

    public Color GetAxisCapColor(int axisIndex, AxisSign axisSign)
    {
      return this.GetAxisCapLookAndFeel(axisIndex, axisSign).Color;
    }

    public void SetHoveredColor(Color hoveredColor)
    {
      foreach (GizmoCap3DLookAndFeel cap3DlookAndFeel in this._axesCapsLookAndFeel)
        cap3DlookAndFeel.HoveredColor = hoveredColor;
      this._midCapLookAndFeel.HoveredColor = hoveredColor;
    }

    public void SetMidCapFillMode(GizmoFillMode3D fillMode)
    {
      this._midCapLookAndFeel.FillMode = fillMode;
    }

    public void SetAxisCapFillMode(GizmoFillMode3D fillMode)
    {
      foreach (GizmoCap3DLookAndFeel cap3DlookAndFeel in this._axesCapsLookAndFeel)
        cap3DlookAndFeel.FillMode = fillMode;
    }

    public void SetMidCapShadeMode(GizmoShadeMode shadeMode)
    {
      this._midCapLookAndFeel.ShadeMode = shadeMode;
    }

    public void SetAxisCapShadeMode(GizmoShadeMode shadeMode)
    {
      foreach (GizmoCap3DLookAndFeel cap3DlookAndFeel in this._axesCapsLookAndFeel)
        cap3DlookAndFeel.ShadeMode = shadeMode;
    }

    public List<Enum> GetAllowedMidCapTypes()
    {
      return new List<Enum>()
      {
        (Enum) GizmoCap3DType.Box,
        (Enum) GizmoCap3DType.Sphere
      };
    }

    public List<Enum> GetAllowedAxesCapTypes()
    {
      return new List<Enum>()
      {
        (Enum) GizmoCap3DType.Cone,
        (Enum) GizmoCap3DType.Pyramid
      };
    }

    public bool IsMidCapTypeAllowed(GizmoCap3DType capType)
    {
      return capType == GizmoCap3DType.Box || capType == GizmoCap3DType.Sphere;
    }

    public void SetMidCapType(GizmoCap3DType capType)
    {
      if (!this.IsMidCapTypeAllowed(capType))
        return;
      this._midCapLookAndFeel.CapType = capType;
    }

    public bool IsAxisCapTypeAllowed(GizmoCap3DType capType)
    {
      return capType == GizmoCap3DType.Cone || capType == GizmoCap3DType.Pyramid;
    }

    public void SetAxisCapType(GizmoCap3DType capType)
    {
      foreach (GizmoCap3DLookAndFeel cap3DlookAndFeel in this._axesCapsLookAndFeel)
        cap3DlookAndFeel.CapType = capType;
    }

    public float GetAxesLabelWorldSize(Camera gizmoCam, Vector3 labelWorldPos)
    {
      return gizmoCam.ScreenToEstimatedWorldSize(labelWorldPos, this.AxisLabelScreenSize);
    }

    public Vector2 CalculateMaxPrjSwitchLabelRectSize()
    {
      return Vector2.Max(new Vector2((float) this.CamOrthoModeLabelTexture.width, (float) this.CamOrthoModeLabelTexture.height), new Vector2((float) this.CamPerspModeLabelTexture.width, (float) this.CamPerspModeLabelTexture.height));
    }

    public void ConnectMidCapLookAndFeel(GizmoCap3D midCap)
    {
      midCap.SharedLookAndFeel = this._midCapLookAndFeel;
    }

    public void ConnectAxisCapLookAndFeel(GizmoCap3D axisCap, int axisIndex, AxisSign axisSign)
    {
      axisCap.SharedLookAndFeel = this.GetAxisCapLookAndFeel(axisIndex, axisSign);
    }

    private GizmoCap3DLookAndFeel GetAxisCapLookAndFeel(int axisIndex, AxisSign axisSign)
    {
      return axisSign == AxisSign.Positive ? this._axesCapsLookAndFeel[axisIndex] : this._axesCapsLookAndFeel[axisIndex + 3];
    }

    private void OnScreenSizeChanged()
    {
      this._midCapLookAndFeel.BoxWidth = this.MidCapBoxSize;
      this._midCapLookAndFeel.BoxHeight = this.MidCapBoxSize;
      this._midCapLookAndFeel.BoxDepth = this.MidCapBoxSize;
      this._midCapLookAndFeel.SphereRadius = this.MidCapSphereRadius;
      foreach (GizmoCap3DLookAndFeel cap3DlookAndFeel in this._axesCapsLookAndFeel)
      {
        cap3DlookAndFeel.ConeHeight = this.AxisConeHeight;
        cap3DlookAndFeel.ConeRadius = this.AxisConeRadius;
      }
    }

    static SceneGizmoLookAndFeel()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      SceneGizmoLookAndFeel._baseScreenSize = 90f;
      SceneGizmoLookAndFeel._invBaseScreenSize = 1f / SceneGizmoLookAndFeel._baseScreenSize;
    }
  }
}
