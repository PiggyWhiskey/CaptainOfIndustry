// Decompiled with JetBrains decompiler
// Type: RTG.GizmoRotationArc2DLookAndFeel
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
  public class GizmoRotationArc2DLookAndFeel
  {
    [SerializeField]
    private bool _useShortestRotation;
    [SerializeField]
    private GizmoRotationArcFillFlags _fillFlags;
    [SerializeField]
    private Color _color;
    [SerializeField]
    private Color _borderColor;

    public bool UseShortestRotation
    {
      get => this._useShortestRotation;
      set => this._useShortestRotation = value;
    }

    public GizmoRotationArcFillFlags FillFlags
    {
      get => this._fillFlags;
      set => this._fillFlags = value;
    }

    public Color Color
    {
      get => this._color;
      set => this._color = value;
    }

    public Color BorderColor
    {
      get => this._borderColor;
      set => this._borderColor = value;
    }

    public GizmoRotationArc2DLookAndFeel()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._useShortestRotation = true;
      this._fillFlags = GizmoRotationArcFillFlags.Area | GizmoRotationArcFillFlags.ExtremitiesBorder;
      this._color = RTSystemValues.GuideFillColor;
      this._borderColor = RTSystemValues.GuideBorderColor;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
