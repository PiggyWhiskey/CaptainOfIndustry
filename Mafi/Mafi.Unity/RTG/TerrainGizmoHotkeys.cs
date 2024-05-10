// Decompiled with JetBrains decompiler
// Type: RTG.TerrainGizmoHotkeys
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
  public class TerrainGizmoHotkeys
  {
    [SerializeField]
    private Hotkeys _enableSnapping;
    private Hotkeys _rotateObjects;

    public Hotkeys EnableSnapping => this._enableSnapping;

    public Hotkeys RotateObjects => this._rotateObjects;

    public TerrainGizmoHotkeys()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._enableSnapping = new Hotkeys("Enable snapping", new HotkeysStaticData()
      {
        CanHaveMouseButtons = false
      })
      {
        Key = KeyCode.None,
        LCtrl = true
      };
      this._rotateObjects = new Hotkeys("Enable object rotation", new HotkeysStaticData()
      {
        CanHaveMouseButtons = false
      })
      {
        Key = KeyCode.C
      };
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
