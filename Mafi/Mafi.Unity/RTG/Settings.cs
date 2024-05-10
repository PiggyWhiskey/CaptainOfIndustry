// Decompiled with JetBrains decompiler
// Type: RTG.Settings
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
  public abstract class Settings
  {
    [SerializeField]
    private bool _canBeDisplayed;
    [SerializeField]
    protected bool _isExpanded;
    private string _foldoutLabel;

    public bool CanBeDisplayed
    {
      get => this._canBeDisplayed;
      set => this._canBeDisplayed = value;
    }

    public bool UsesFoldout { get; set; }

    public string FoldoutLabel
    {
      get => this._foldoutLabel;
      set
      {
        if (value == null)
          return;
        this._foldoutLabel = value;
      }
    }

    public bool IsExpanded
    {
      get => this._isExpanded;
      set => this._isExpanded = value;
    }

    protected Settings()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._canBeDisplayed = true;
      this._isExpanded = true;
      this._foldoutLabel = nameof (Settings);
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
