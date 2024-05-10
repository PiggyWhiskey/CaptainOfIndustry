// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Audio.BackgroundMusicManager
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Console;
using Mafi.Core.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Audio
{
  /// <summary>
  /// Manages playback of background music. It's instantiated once per lifetime by the implementation of
  /// <see cref="T:Mafi.Unity.IMain" /> of the game so that the music is not interrupted with scene changes and loading screens.
  /// </summary>
  public class BackgroundMusicManager
  {
    private static readonly ImmutableArray<string> GAME_TRACKS;
    private static readonly ImmutableArray<string> MAIN_MENU_TRACKS;
    private ImmutableArray<string>? m_lastSelectedTracks;
    private readonly AudioDb m_audioDb;
    private readonly IRandom m_random;
    private readonly AudioSource m_audioSource;
    private readonly Lyst<KeyValuePair<string, AudioClip>> m_selectedClips;
    private readonly Lyst<KeyValuePair<string, AudioClip>> m_remainingClips;
    private KeyValuePair<string, AudioClip>? m_lastPlayedClip;
    private bool m_silenceBetweenTracks;
    private float m_remainingWaitSec;

    public BackgroundMusicManager(AudioDb audioDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_selectedClips = new Lyst<KeyValuePair<string, AudioClip>>();
      this.m_remainingClips = new Lyst<KeyValuePair<string, AudioClip>>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_audioDb = audioDb.CheckNotNull<AudioDb>();
      XorRsr128PlusGenerator rsr128PlusGenerator = new XorRsr128PlusGenerator(RandomGeneratorType.Unrestricted, (ulong) Environment.TickCount, 61546524674984UL);
      rsr128PlusGenerator.Jump();
      this.m_random = (IRandom) rsr128PlusGenerator;
      GameObject gameObject = GameObject.Find("Background music manager");
      if (!(bool) (UnityEngine.Object) gameObject)
      {
        gameObject = new GameObject("Background music manager");
        gameObject.tag = UnityTag.Persistent.ToString();
      }
      if (!(bool) (UnityEngine.Object) gameObject.GetComponent<BackgroundMusicManager.MusicPlayerMb>())
      {
        gameObject.AddComponent<BackgroundMusicManager.MusicPlayerMb>().Initialize(this);
        this.m_audioSource = gameObject.AddComponent<AudioSource>();
        this.m_audioSource.volume = 0.5f;
        this.m_audioSource.priority = 10;
        this.m_audioSource.outputAudioMixerGroup = audioDb.GetChannel(AudioChannel.Music);
        this.m_audioSource.spatialBlend = 0.0f;
        this.m_audioSource.loop = false;
      }
      else
        this.m_audioSource = gameObject.GetComponent<AudioSource>();
    }

    [ConsoleCommand(true, false, null, null)]
    private string printCurrentMusicTrack()
    {
      return this.m_lastPlayedClip.HasValue ? Path.GetFileNameWithoutExtension(this.m_lastPlayedClip.Value.Key) : "No music is playing.";
    }

    [ConsoleCommand(true, false, null, null)]
    private string skipCurrentMusicTrack()
    {
      ref KeyValuePair<string, AudioClip>? local = ref this.m_lastPlayedClip;
      string path = (local.HasValue ? local.GetValueOrDefault().Key : (string) null) ?? "";
      this.PlayNextTrack();
      return string.Format("Skipping '{0}', remaining tracks: {1}", (object) Path.GetFileNameWithoutExtension(path), (object) this.m_remainingClips.Count);
    }

    private void update()
    {
      if (!this.m_lastSelectedTracks.HasValue || this.m_lastSelectedTracks.Value.Length == 0)
        return;
      if ((double) this.m_audioDb.GetChannelVolume(AudioChannel.Music) <= 1.0 / 1000.0 || (double) this.m_audioDb.GetChannelVolume(AudioChannel.Master) <= 1.0 / 1000.0)
      {
        if (!this.m_audioSource.isPlaying)
          return;
        this.StopMusic();
      }
      else
      {
        if (this.m_selectedClips.IsEmpty)
        {
          this.loadAndPlayMusic(this.m_lastSelectedTracks.Value, this.m_silenceBetweenTracks);
          if (this.m_selectedClips.IsEmpty)
          {
            Log.Warning("No clips were selected.");
            return;
          }
        }
        if (this.m_audioSource.isPlaying)
        {
          if ((double) this.m_audioSource.volume >= 0.5)
            return;
          this.m_audioSource.volume += (float) (0.5 * (double) Time.deltaTime / 1.0);
          if ((double) this.m_audioSource.volume <= 0.5)
            return;
          this.m_audioSource.volume = 0.5f;
        }
        else if ((double) this.m_remainingWaitSec > 0.0)
        {
          this.m_remainingWaitSec -= Time.deltaTime;
        }
        else
        {
          if (this.m_remainingClips.IsEmpty)
          {
            this.m_remainingClips.AddRange(this.m_selectedClips);
            this.m_remainingClips.Shuffle(this.m_random);
          }
          if (this.m_remainingClips.Count > 1)
          {
            for (int index = 0; (UnityEngine.Object) this.m_remainingClips.Last.Value == (UnityEngine.Object) this.m_audioSource.clip && index < 10; ++index)
              this.m_remainingClips.Shuffle(this.m_random);
            Assert.That<AudioClip>(this.m_remainingClips.Last.Value).IsNotEqualTo<AudioClip>(this.m_audioSource.clip, "Playing the same track twice.");
          }
          KeyValuePair<string, AudioClip> keyValuePair = this.m_remainingClips.PopLast();
          this.m_lastPlayedClip = new KeyValuePair<string, AudioClip>?(keyValuePair);
          this.m_audioSource.volume = 0.0f;
          this.m_audioSource.clip = keyValuePair.Value;
          this.m_audioSource.Play();
          if (this.m_silenceBetweenTracks)
            this.m_remainingWaitSec = this.m_random.NextFloat(5f, 15f);
          Assert.That<bool>(this.m_audioSource.isPlaying).IsTrue();
        }
      }
    }

    private void loadAndPlayMusic(ImmutableArray<string> clipPaths, bool silenceBetweenTracks)
    {
      this.m_selectedClips.Clear();
      this.m_remainingClips.Clear();
      this.m_silenceBetweenTracks = silenceBetweenTracks;
      this.m_lastSelectedTracks = new ImmutableArray<string>?(clipPaths);
      foreach (string clipPath in clipPaths)
      {
        AudioClip clip;
        if (this.m_audioDb.TryGetSharedClip(clipPath, out clip))
          this.m_selectedClips.Add(Make.Kvp<string, AudioClip>(clipPath, clip));
        else
          Log.Error("Failed to find audio clip '" + clipPath + "'.");
      }
      this.PlayNextTrack();
    }

    public void StartMainMenuMusic()
    {
      this.loadAndPlayMusic(BackgroundMusicManager.MAIN_MENU_TRACKS, false);
    }

    public void StartGameMenuMusic()
    {
      this.loadAndPlayMusic(BackgroundMusicManager.GAME_TRACKS, true);
    }

    public void PlayNextTrack()
    {
      this.m_audioSource.Stop();
      this.m_remainingWaitSec = 0.0f;
      this.m_lastPlayedClip = new KeyValuePair<string, AudioClip>?();
    }

    public void StopMusic()
    {
      this.m_audioSource.Stop();
      this.m_selectedClips.Clear();
      this.m_remainingClips.Clear();
      this.m_lastPlayedClip = new KeyValuePair<string, AudioClip>?();
    }

    static BackgroundMusicManager()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      BackgroundMusicManager.GAME_TRACKS = ImmutableArray.Create<string>("Assets/Unity/Soundtrack/01 Rise from ashes.wav", "Assets/Unity/Soundtrack/02 Machine spirit.wav", "Assets/Unity/Soundtrack/03 Beacon.wav", "Assets/Unity/Soundtrack/04 Full steam ahead.wav", "Assets/Unity/Soundtrack/05 Perpetual mining.wav", "Assets/Unity/Soundtrack/06 Planning for tomorrow.wav", "Assets/Unity/Soundtrack/07 Captain of Industry.wav", "Assets/Unity/Soundtrack/08 From dust to stars.wav", "Assets/Unity/Soundtrack/09 Cog in the wheel.wav", "Assets/Unity/Soundtrack/10 Power of unity.wav", "Assets/Unity/Soundtrack/11 Thoughtful planning.wav", "Assets/Unity/Soundtrack/12 Bessemer.wav", "Assets/Unity/Soundtrack/13 Steel must flow.wav", "Assets/Unity/Soundtrack/14 Eureka!.wav", "Assets/Unity/Soundtrack/15 New hope.wav", "Assets/Unity/Soundtrack/16 Quiet hum of industry.wav", "Assets/Unity/Soundtrack/17 T minus 10.wav", "Assets/Unity/Soundtrack/18 Harvest.wav", "Assets/Unity/Soundtrack/19 Planting trees.wav", "Assets/Unity/Soundtrack/20 Bright future.wav", "Assets/Unity/Soundtrack/21 The sound of progress.wav", "Assets/Unity/Soundtrack/22 Distillation.wav", "Assets/Unity/Soundtrack/23 Smooth sailing.wav", "Assets/Unity/Soundtrack/24 Statue of gold.wav", "Assets/Unity/Soundtrack/25 Mineral whispers.wav", "Assets/Unity/Soundtrack/26 The looming foundry.wav", "Assets/Unity/Soundtrack/27 Deploying blueprints.wav", "Assets/Unity/Soundtrack/28 Flourishing settlement.wav", "Assets/Unity/Soundtrack/29 Metallic visions.wav", "Assets/Unity/Soundtrack/30 Forge of progress.wav", "Assets/Unity/Soundtrack/31 Decorating the surfaces.wav", "Assets/Unity/Soundtrack/32 Symphony of gears.wav");
      BackgroundMusicManager.MAIN_MENU_TRACKS = ImmutableArray.Create<string>("Assets/Unity/Soundtrack/03 Beacon.wav", "Assets/Unity/Soundtrack/06 Planning for tomorrow.wav");
    }

    private class MusicPlayerMb : MonoBehaviour
    {
      private BackgroundMusicManager m_manager;

      public void Initialize(BackgroundMusicManager manager) => this.m_manager = manager;

      private void Update() => this.m_manager?.update();

      public MusicPlayerMb()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }
  }
}
