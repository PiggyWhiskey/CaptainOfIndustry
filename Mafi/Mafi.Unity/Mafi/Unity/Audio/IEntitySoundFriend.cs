// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Audio.IEntitySoundFriend
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;

#nullable disable
namespace Mafi.Unity.Audio
{
  internal interface IEntitySoundFriend
  {
    int MaxDistance { get; }

    bool IsPlaying { get; }

    /// <summary>Position of the sound.</summary>
    Transform Transform { get; }

    /// <summary>Distance from the listener.</summary>
    float ListenerDistance { get; set; }

    int Key { get; }

    void SetVolumeMultiplier(float volumeMultiplier);
  }
}
