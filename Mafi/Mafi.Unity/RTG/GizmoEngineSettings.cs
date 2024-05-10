// Decompiled with JetBrains decompiler
// Type: RTG.GizmoEngineSettings
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
  public class GizmoEngineSettings : Settings
  {
    [SerializeField]
    private bool _enableGizmoSorting;

    public bool EnableGizmoSorting
    {
      get => this._enableGizmoSorting;
      set => this._enableGizmoSorting = value;
    }

    public GizmoEngineSettings()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._enableGizmoSorting = true;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
