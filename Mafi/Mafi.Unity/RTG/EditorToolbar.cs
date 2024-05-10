// Decompiled with JetBrains decompiler
// Type: RTG.EditorToolbar
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
  public class EditorToolbar
  {
    [SerializeField]
    private Color _activeTabColor;
    [SerializeField]
    private int _numTabsPerRow;
    [SerializeField]
    private EditorToolbarTab[] _tabs;
    [SerializeField]
    private int _activeTabIndex;

    public int ActiveTabIndex => this._activeTabIndex;

    public EditorToolbarTab ActiveTab => this._tabs[this._activeTabIndex];

    public Color ActiveTabColor
    {
      get => this._activeTabColor;
      set => this._activeTabColor = value;
    }

    public int NumTabsPerRow
    {
      get => this._numTabsPerRow;
      set => this._numTabsPerRow = Mathf.Max(1, value);
    }

    public int NumTabs => this._tabs.Length;

    public EditorToolbar(EditorToolbarTab[] tabs, int numTabsPerRow, Color activeTabColor)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._activeTabColor = Color.green;
      this._numTabsPerRow = 3;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._tabs = tabs;
      this._activeTabColor = activeTabColor;
      this._numTabsPerRow = numTabsPerRow;
    }

    public EditorToolbarTab GetTabByIndex(int tabIndex) => this._tabs[tabIndex];
  }
}
