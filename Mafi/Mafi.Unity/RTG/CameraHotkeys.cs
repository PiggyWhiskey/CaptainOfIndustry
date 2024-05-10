// Decompiled with JetBrains decompiler
// Type: RTG.CameraHotkeys
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
  public class CameraHotkeys : Settings
  {
    [SerializeField]
    private Hotkeys _moveForward;
    [SerializeField]
    private Hotkeys _moveBack;
    [SerializeField]
    private Hotkeys _strafeLeft;
    [SerializeField]
    private Hotkeys _strafeRight;
    [SerializeField]
    private Hotkeys _moveUp;
    [SerializeField]
    private Hotkeys _moveDown;
    [SerializeField]
    private Hotkeys _pan;
    [SerializeField]
    private Hotkeys _lookAround;
    [SerializeField]
    private Hotkeys _orbit;

    public Hotkeys MoveForward => this._moveForward;

    public Hotkeys MoveBack => this._moveBack;

    public Hotkeys StrafeLeft => this._strafeLeft;

    public Hotkeys StrafeRight => this._strafeRight;

    public Hotkeys MoveUp => this._moveUp;

    public Hotkeys MoveDown => this._moveDown;

    public Hotkeys Pan => this._pan;

    public Hotkeys LookAround => this._lookAround;

    public Hotkeys Orbit => this._orbit;

    public CameraHotkeys()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._moveForward = new Hotkeys("Move forward")
      {
        Key = KeyCode.W,
        UseStrictModifierCheck = false,
        RMouseButton = true
      };
      this._moveBack = new Hotkeys("Move back")
      {
        Key = KeyCode.S,
        UseStrictModifierCheck = false,
        RMouseButton = true
      };
      this._strafeLeft = new Hotkeys("Strafe left")
      {
        Key = KeyCode.A,
        UseStrictModifierCheck = false,
        RMouseButton = true
      };
      this._strafeRight = new Hotkeys("Strafe right")
      {
        Key = KeyCode.D,
        UseStrictModifierCheck = false,
        RMouseButton = true
      };
      this._moveUp = new Hotkeys("Move up")
      {
        Key = KeyCode.E,
        UseStrictModifierCheck = false,
        RMouseButton = true
      };
      this._moveDown = new Hotkeys("Move down")
      {
        Key = KeyCode.Q,
        UseStrictModifierCheck = false,
        RMouseButton = true
      };
      this._pan = new Hotkeys(nameof (Pan))
      {
        UseStrictModifierCheck = false,
        MMouseButton = true
      };
      this._lookAround = new Hotkeys("Look around")
      {
        UseStrictModifierCheck = false,
        RMouseButton = true
      };
      this._orbit = new Hotkeys(nameof (Orbit))
      {
        UseStrictModifierCheck = false,
        LAlt = true,
        RMouseButton = true
      };
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
