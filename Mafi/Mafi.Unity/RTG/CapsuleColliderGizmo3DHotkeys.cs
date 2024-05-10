// Decompiled with JetBrains decompiler
// Type: RTG.CapsuleColliderGizmo3DHotkeys
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
  public class CapsuleColliderGizmo3DHotkeys
  {
    [SerializeField]
    private Hotkeys _enableSnapping;
    [SerializeField]
    private Hotkeys _scaleFromCenter;

    public Hotkeys EnableSnapping => this._enableSnapping;

    public Hotkeys ScaleFromCenter => this._scaleFromCenter;

    public CapsuleColliderGizmo3DHotkeys()
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
      this._scaleFromCenter = new Hotkeys("Scale from Center", new HotkeysStaticData()
      {
        CanHaveMouseButtons = false
      })
      {
        Key = KeyCode.None,
        LShift = true
      };
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
