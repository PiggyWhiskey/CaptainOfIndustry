// Decompiled with JetBrains decompiler
// Type: RTG.RTCustomObjectInteractionSettings
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
  public class RTCustomObjectInteractionSettings : Settings
  {
    /// <summary>
    /// The custom interaction system needs to know the size of objects that have no
    /// volume. This defines a volume for these objects in the 3D world so that they
    /// can still be involved in raycasts, overlap tests etc.
    /// </summary>
    [SerializeField]
    private Vector3 _noVolumeObjectSize;

    public Vector3 NoVolumeObjectSize
    {
      get => this._noVolumeObjectSize;
      set
      {
        if (Application.isPlaying)
          return;
        this._noVolumeObjectSize = Vector3.Max(Vector3.zero, value);
      }
    }

    public RTCustomObjectInteractionSettings()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._noVolumeObjectSize = Vector3Ex.FromValue(0.5f);
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
