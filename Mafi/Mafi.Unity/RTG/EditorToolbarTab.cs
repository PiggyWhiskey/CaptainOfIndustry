// Decompiled with JetBrains decompiler
// Type: RTG.EditorToolbarTab
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
  public class EditorToolbarTab
  {
    [SerializeField]
    private string _tooltip;
    [SerializeField]
    private string _text;
    [NonSerialized]
    private EditorToolbar _targetToolbar;
    [NonSerialized]
    private List<Settings> _targetSettings;

    public EditorToolbarTab(string text, string tooltip)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._tooltip = "";
      this._text = "";
      this._targetSettings = new List<Settings>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Text = text;
      this.Tooltip = tooltip;
    }

    public string Tooltip
    {
      get => this._tooltip;
      set
      {
        if (value == null)
          return;
        this._tooltip = value;
      }
    }

    public string Text
    {
      get => this._text;
      set
      {
        if (value == null)
          return;
        this._text = value;
      }
    }

    public EditorToolbar TargetToolbar
    {
      get => this._targetToolbar;
      set
      {
        if (value == null)
          return;
        this._targetToolbar = value;
      }
    }

    public int NumTargetSettings => this._targetSettings.Count;

    public void AddTargetSettings(Settings targetSettings)
    {
      if (this._targetSettings.Contains(targetSettings))
        return;
      this._targetSettings.Add(targetSettings);
    }
  }
}
