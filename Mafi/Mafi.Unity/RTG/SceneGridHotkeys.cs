// Decompiled with JetBrains decompiler
// Type: RTG.SceneGridHotkeys
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
  public class SceneGridHotkeys : Settings
  {
    [SerializeField]
    private Hotkeys _gridUp;
    [SerializeField]
    private Hotkeys _gridDown;
    private Hotkeys _snapToCursorPickPoint;

    public Hotkeys GridUp => this._gridUp;

    public Hotkeys GridDown => this._gridDown;

    public Hotkeys SnapToCursorPickPoint => this._snapToCursorPickPoint;

    public SceneGridHotkeys()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._gridUp = new Hotkeys("Grid up", new HotkeysStaticData()
      {
        CanHaveMouseButtons = false
      })
      {
        Key = KeyCode.RightBracket
      };
      this._gridDown = new Hotkeys("Grid down", new HotkeysStaticData()
      {
        CanHaveMouseButtons = false
      })
      {
        Key = KeyCode.LeftBracket
      };
      this._snapToCursorPickPoint = new Hotkeys("Snap to cursor pick point", new HotkeysStaticData()
      {
        CanHaveMouseButtons = false
      })
      {
        LAlt = true
      };
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
