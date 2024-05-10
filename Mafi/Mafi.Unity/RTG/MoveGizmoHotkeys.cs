// Decompiled with JetBrains decompiler
// Type: RTG.MoveGizmoHotkeys
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
  public class MoveGizmoHotkeys : Settings
  {
    [SerializeField]
    private Hotkeys _enable2DMode;
    [SerializeField]
    private Hotkeys _enableSnapping;
    [SerializeField]
    private Hotkeys _enableVertexSnapping;

    public Hotkeys Enable2DMode => this._enable2DMode;

    public Hotkeys EnableSnapping => this._enableSnapping;

    public Hotkeys EnableVertexSnapping => this._enableVertexSnapping;

    public MoveGizmoHotkeys()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._enable2DMode = new Hotkeys("Enable 2D mode", new HotkeysStaticData()
      {
        CanHaveMouseButtons = false
      })
      {
        Key = KeyCode.None,
        LShift = true
      };
      this._enableSnapping = new Hotkeys("Enable snapping", new HotkeysStaticData()
      {
        CanHaveMouseButtons = false
      })
      {
        Key = KeyCode.None,
        LCtrl = true
      };
      this._enableVertexSnapping = new Hotkeys("Enable vertex snapping", new HotkeysStaticData()
      {
        CanHaveMouseButtons = false
      })
      {
        UseStrictModifierCheck = false,
        Key = KeyCode.V
      };
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
