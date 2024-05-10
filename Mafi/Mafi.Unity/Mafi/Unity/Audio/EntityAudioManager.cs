// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Audio.EntityAudioManager
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.GameLoop;
using Mafi.Unity.Camera;
using Mafi.Unity.Entities;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Audio
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class EntityAudioManager : IEntityAudioManagerFriend
  {
    private readonly Dict<int, Lyst<IEntitySoundFriend>> m_allSounds;
    private readonly AudioDb m_audioDb;
    private readonly GameObject m_listener;
    /// <summary>
    /// Position of the camera at the time we've last updated the playing sounds.
    /// </summary>
    private Vector3 m_prevListenerPosition;

    public EntityAudioManager(
      AudioDb audioDb,
      CameraController cameraController,
      IGameLoopEvents gameLoopEvents)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_allSounds = new Dict<int, Lyst<IEntitySoundFriend>>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_audioDb = audioDb;
      gameLoopEvents.RenderUpdate.AddNonSaveable<EntityAudioManager>(this, new Action<GameTime>(this.renderUpdate));
      this.m_listener = cameraController.Listener.gameObject;
      this.m_prevListenerPosition = this.m_listener.transform.position;
    }

    public Option<EntitySoundMb> CreateSound(EntityMb owner, SoundParams soundParams)
    {
      if (soundParams.SoundPrefabPath == "EMPTY")
        return Option<EntitySoundMb>.None;
      if (!this.m_audioDb.ContainsAsset(soundParams.SoundPrefabPath))
      {
        Log.Error("Sound " + soundParams.SoundPrefabPath + " not found!");
        return Option<EntitySoundMb>.None;
      }
      AudioSource clonedAudio = this.m_audioDb.GetClonedAudio(soundParams.SoundPrefabPath, AudioChannel.Machines, (Option<GameObject>) owner.gameObject);
      EntitySoundMb sound = clonedAudio.gameObject.AddComponent<EntitySoundMb>();
      sound.Initialize(clonedAudio, (IEntityAudioManagerFriend) this, soundParams);
      if (!soundParams.DoNotLimit)
      {
        Lyst<IEntitySoundFriend> lyst;
        if (!this.m_allSounds.TryGetValue(sound.Key, out lyst))
        {
          lyst = new Lyst<IEntitySoundFriend>();
          this.m_allSounds[sound.Key] = lyst;
        }
        lyst.Add((IEntitySoundFriend) sound);
      }
      return (Option<EntitySoundMb>) sound;
    }

    /// <summary>
    /// TODO: this could be more effective - for example we should not go through all unhearable sounds, but only
    /// through those close to the listener when updating hearable sounds
    /// </summary>
    private void renderUpdate(GameTime gameTime)
    {
      if (gameTime.IsGamePaused)
        return;
      Vector3 position = this.m_listener.transform.position;
      if ((double) Vector3.Distance(position, this.m_prevListenerPosition) <= 12.0)
        return;
      this.m_prevListenerPosition = position;
      this.updateSounds();
    }

    private void updateSounds()
    {
      foreach (Lyst<IEntitySoundFriend> sounds in this.m_allSounds.Values)
        this.updateGroupOfSounds(sounds);
    }

    /// <summary>
    /// TODO: Consider treating z-elevation in special way and fade-out sounds more quickly
    /// TODO: Consider disabling unhearable sounds
    /// TODO: Consider going through unhearable sounds less often
    /// </summary>
    private void updateGroupOfSounds(Lyst<IEntitySoundFriend> sounds)
    {
      Vector3 position = this.m_listener.transform.position;
      int num = 0;
      foreach (IEntitySoundFriend sound in sounds)
      {
        sound.ListenerDistance = Vector3.Distance(sound.Transform.position, position);
        if (sound.IsPlaying && (double) sound.MaxDistance > (double) sound.ListenerDistance)
          ++num;
      }
      float volumeMultiplier = 1f;
      if (num > 1)
        volumeMultiplier = ((float) (1.0 - (double) (num - 1) * 0.079999998211860657)).Max(0.3f);
      foreach (IEntitySoundFriend sound in sounds)
      {
        if (sound.IsPlaying && (double) sound.MaxDistance > (double) sound.ListenerDistance)
          sound.SetVolumeMultiplier(volumeMultiplier);
        else
          sound.SetVolumeMultiplier(0.0f);
      }
    }

    void IEntityAudioManagerFriend.EntitySoundStarted(IEntitySoundFriend sound)
    {
      Lyst<IEntitySoundFriend> sounds;
      if (!this.m_allSounds.TryGetValue(sound.Key, out sounds))
        return;
      this.updateGroupOfSounds(sounds);
    }

    void IEntityAudioManagerFriend.EntitySoundStopped(IEntitySoundFriend sound)
    {
      Lyst<IEntitySoundFriend> sounds;
      if (!this.m_allSounds.TryGetValue(sound.Key, out sounds))
        return;
      this.updateGroupOfSounds(sounds);
    }

    void IEntityAudioManagerFriend.EntitySoundDestroyed(IEntitySoundFriend sound)
    {
      Lyst<IEntitySoundFriend> sounds;
      if (!this.m_allSounds.TryGetValue(sound.Key, out sounds))
        return;
      sounds.Remove(sound);
      this.updateGroupOfSounds(sounds);
    }
  }
}
