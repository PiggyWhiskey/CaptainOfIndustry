// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Audio.EntitySoundMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Unity.Entities;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Audio
{
  public class EntitySoundMb : MonoBehaviour, IEntitySoundFriend, IDestroyableEntityMb
  {
    /// <summary>Volume state we are fading to.</summary>
    private float m_volumeTarget;
    private AudioSource m_audioSource;
    private float m_originalVolume;
    private float m_fadingStep;
    private IEntityAudioManagerFriend m_audioManager;
    private bool m_isFading;
    private SoundParams m_soundParams;
    private float m_internalVolumeTarget;
    private float m_volumeMultiplier;
    private bool m_gamePaused;

    public float OriginalVolume => this.m_originalVolume;

    public bool IsPlaying { get; private set; }

    Transform IEntitySoundFriend.Transform => this.transform;

    float IEntitySoundFriend.ListenerDistance { get; set; }

    public bool FadeOnChange => this.m_soundParams.FadeOnChange;

    public bool IsDestroyed { get; private set; }

    public int MaxDistance { get; private set; }

    public int Key { get; private set; }

    public string Desc { get; private set; }

    internal void Initialize(
      AudioSource audioSource,
      IEntityAudioManagerFriend audioManager,
      SoundParams soundParams)
    {
      this.Key = soundParams.SoundPrefabPath.GetHashCode();
      this.Desc = soundParams.SoundPrefabPath;
      Assert.That<AudioSource>(this.m_audioSource).IsNull<AudioSource>();
      this.m_audioSource = audioSource.CheckNotNull<AudioSource>();
      this.m_audioManager = audioManager.CheckNotNull<IEntityAudioManagerFriend>();
      this.m_soundParams = soundParams;
      this.m_originalVolume = this.m_audioSource.volume;
      this.m_volumeMultiplier = 1f;
      this.m_volumeTarget = this.m_originalVolume;
      audioSource.loop = soundParams.Loop;
      audioSource.rolloffMode = AudioRolloffMode.Linear;
      audioSource.minDistance = 0.0f;
      switch (soundParams.Significance)
      {
        case SoundSignificance.Normal:
          audioSource.maxDistance = 260f;
          audioSource.priority = 250;
          break;
        case SoundSignificance.Medium:
          audioSource.maxDistance = 620f;
          audioSource.priority = 245;
          break;
        case SoundSignificance.High:
          audioSource.maxDistance = 920f;
          audioSource.priority = 240;
          break;
        default:
          audioSource.maxDistance = 160f;
          audioSource.priority = (int) byte.MaxValue;
          break;
      }
      this.MaxDistance = audioSource.maxDistance.RoundToInt();
      this.m_fadingStep = this.m_originalVolume * 1f;
    }

    public void Play() => this.UpdatePlay(true);

    public void UpdatePlay(bool isPlaying)
    {
      if (!this.m_soundParams.Loop && !this.m_audioSource.isPlaying)
        this.IsPlaying = false;
      if (this.IsPlaying == isPlaying)
        return;
      this.IsPlaying = isPlaying;
      if (isPlaying)
      {
        this.m_internalVolumeTarget = this.m_originalVolume;
        this.m_volumeTarget = this.m_internalVolumeTarget * this.m_volumeMultiplier;
        this.m_isFading |= this.FadeOnChange;
        if (!this.m_isFading)
          this.m_audioSource.volume = this.m_volumeTarget;
        this.m_audioSource.Play();
        if (this.m_soundParams.DoNotLimit)
          return;
        this.m_audioManager.EntitySoundStarted((IEntitySoundFriend) this);
      }
      else
      {
        this.m_internalVolumeTarget = 0.0f;
        this.m_volumeTarget = this.m_internalVolumeTarget * this.m_volumeMultiplier;
        this.m_isFading |= this.FadeOnChange;
        if (!this.m_isFading)
          this.m_audioSource.Stop();
        if (this.m_soundParams.DoNotLimit)
          return;
        this.m_audioManager.EntitySoundStopped((IEntitySoundFriend) this);
      }
    }

    public void SetVolumeMultiplier(float volumeMultiplier)
    {
      this.m_volumeMultiplier = volumeMultiplier;
      this.m_volumeTarget = this.m_internalVolumeTarget * this.m_volumeMultiplier;
      this.m_isFading = true;
    }

    private void updateFading()
    {
      if (!this.m_isFading)
        return;
      float volume = this.m_audioSource.volume;
      if (volume.IsNear(this.m_volumeTarget))
      {
        this.m_isFading = false;
        if (this.IsPlaying)
          return;
        this.m_audioSource.Stop();
      }
      else
      {
        float num;
        if ((double) volume > (double) this.m_volumeTarget)
        {
          num = volume - this.m_fadingStep * Time.deltaTime;
          if ((double) num <= (double) this.m_volumeTarget)
          {
            num = this.m_volumeTarget;
            this.m_audioSource.priority = (int) byte.MaxValue;
          }
        }
        else
        {
          num = volume + this.m_fadingStep * Time.deltaTime;
          if ((double) num >= (double) this.m_volumeTarget)
            num = this.m_volumeTarget;
        }
        this.m_audioSource.volume = num;
      }
    }

    public void RenderUpdate(GameTime gameTime)
    {
      if (this.IsNullOrDestroyed())
        return;
      if (this.m_gamePaused != gameTime.IsGamePaused)
      {
        this.m_gamePaused = gameTime.IsGamePaused;
        if (gameTime.IsGamePaused)
          this.m_audioSource.Pause();
        else
          this.m_audioSource.UnPause();
      }
      if (gameTime.IsGamePaused)
        return;
      this.updateFading();
    }

    public void SetPitch(float pitch) => this.m_audioSource.pitch = pitch;

    public void FadeToVolume(float volume)
    {
      this.m_internalVolumeTarget = volume;
      this.m_volumeTarget = this.m_internalVolumeTarget * this.m_volumeMultiplier;
      this.m_isFading = true;
    }

    public void RenderUpdate(GameTime gameTime, bool isPlaying)
    {
      if (this.m_gamePaused != gameTime.IsGamePaused)
      {
        this.m_gamePaused = gameTime.IsGamePaused;
        if (gameTime.IsGamePaused)
          this.m_audioSource.Pause();
        else
          this.m_audioSource.UnPause();
      }
      if (gameTime.IsGamePaused)
        return;
      this.UpdatePlay(isPlaying);
      this.updateFading();
    }

    public void Destroy()
    {
      this.IsDestroyed = true;
      this.m_audioManager.EntitySoundDestroyed((IEntitySoundFriend) this);
    }

    public EntitySoundMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
