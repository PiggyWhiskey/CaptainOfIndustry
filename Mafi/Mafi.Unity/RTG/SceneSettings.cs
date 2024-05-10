// Decompiled with JetBrains decompiler
// Type: RTG.SceneSettings
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
  public class SceneSettings : Settings
  {
    [SerializeField]
    private ScenePhysicsMode _physicsMode;

    public ScenePhysicsMode PhysicsMode
    {
      get => this._physicsMode;
      set
      {
        if (Application.isPlaying)
          return;
        this._physicsMode = value;
      }
    }

    public SceneSettings()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._physicsMode = ScenePhysicsMode.RTG;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
