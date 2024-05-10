// Decompiled with JetBrains decompiler
// Type: RTG.XZGridLookAndFeel
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
  public class XZGridLookAndFeel : Settings
  {
    [SerializeField]
    private Color _lineColor;
    /// <summary>
    /// If this is true, the grid cells will fade in/out based on the distance between
    /// the grid and the camera. This is consistent with how Unity renders the scene grid
    /// inside the Editor.
    /// </summary>
    [SerializeField]
    private bool _useCellFading;

    public Color LineColor
    {
      get => this._lineColor;
      set => this._lineColor = value;
    }

    public bool UseCellFading
    {
      get => this._useCellFading;
      set => this._useCellFading = value;
    }

    public XZGridLookAndFeel()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._lineColor = RTSystemValues.GridLineColor;
      this._useCellFading = true;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
