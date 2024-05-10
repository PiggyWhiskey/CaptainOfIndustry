// Decompiled with JetBrains decompiler
// Type: RTG.TerrainGizmoSettings
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
  public class TerrainGizmoSettings
  {
    [SerializeField]
    private float _offsetSnapStep;
    [SerializeField]
    private float _radiusSnapStep;
    [SerializeField]
    private float _rotationSensitivity;
    [SerializeField]
    private int _objectHrzMoveLayerMask;
    [SerializeField]
    private int _objectVertMoveLayerMask;
    [SerializeField]
    private int _objectRotationLayerMask;
    [SerializeField]
    private List<string> _objectHrzMoveIgnoreTags;
    [SerializeField]
    private List<string> _objectVertMoveIgnoreTags;
    [SerializeField]
    private List<string> _objectRotationIgnoreTags;

    public float OffsetSnapStep
    {
      get => this._offsetSnapStep;
      set => this._offsetSnapStep = Mathf.Max(0.0001f, value);
    }

    public float RadiusSnapStep
    {
      get => this._radiusSnapStep;
      set => this._radiusSnapStep = Mathf.Max(0.0001f, value);
    }

    public float RotationSensitivity
    {
      get => this._rotationSensitivity;
      set => this._rotationSensitivity = Math.Max(0.0001f, value);
    }

    public int ObjectHrzMoveLayerMask
    {
      get => this._objectHrzMoveLayerMask;
      set => this._objectHrzMoveLayerMask = value;
    }

    public int ObjectVertMoveLayerMask
    {
      get => this._objectVertMoveLayerMask;
      set => this._objectVertMoveLayerMask = value;
    }

    public int ObjectRotationLayerMask
    {
      get => this._objectRotationLayerMask;
      set => this._objectRotationLayerMask = value;
    }

    public void AddObjectHrzMoveIgnoreTag(string tag)
    {
      if (this.IsTagIgnoredForHrzMove(tag))
        return;
      this._objectHrzMoveIgnoreTags.Add(tag);
    }

    public bool IsTagIgnoredForHrzMove(string tag) => this._objectHrzMoveIgnoreTags.Contains(tag);

    public void AddObjectVertMoveIgnoreTag(string tag)
    {
      if (this.IsTagIgnoredForVertMove(tag))
        return;
      this._objectVertMoveIgnoreTags.Add(tag);
    }

    public bool IsTagIgnoredForVertMove(string tag) => this._objectVertMoveIgnoreTags.Contains(tag);

    public void AddObjectRotationIgnoreTag(string tag)
    {
      if (this.IsTagIgnoredForRotation(tag))
        return;
      this._objectRotationIgnoreTags.Add(tag);
    }

    public bool IsTagIgnoredForRotation(string tag) => this._objectRotationIgnoreTags.Contains(tag);

    public TerrainGizmoSettings()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._offsetSnapStep = 1f;
      this._radiusSnapStep = 1f;
      this._rotationSensitivity = 1f;
      this._objectHrzMoveLayerMask = -1;
      this._objectVertMoveLayerMask = -1;
      this._objectRotationLayerMask = -1;
      this._objectHrzMoveIgnoreTags = new List<string>();
      this._objectVertMoveIgnoreTags = new List<string>();
      this._objectRotationIgnoreTags = new List<string>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
