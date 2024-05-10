// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Audio.AudioDb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Audio
{
  /// <summary>
  /// Database that fetches audio using <see cref="T:Mafi.Unity.AssetsDb" /> and stores as <see cref="T:UnityEngine.AudioSource" /> to be easily
  /// retrieved.
  /// </summary>
  public class AudioDb : Db<string, AudioSource>
  {
    public static readonly float MIN_VOLUME_PERCENT;
    public static readonly float MAX_VOLUME_PERCENT;
    private static readonly Lyst<KeyValuePair<AudioChannel, float>> DEFAULT_VOLUMES;
    private readonly AssetsDb m_assetsDb;
    private readonly AudioMixer m_mixer;
    private readonly AudioMixerGroup[] m_channels;
    private readonly string[] m_volumeVariableNames;
    private readonly GameObject m_parentGo;
    private readonly AudioSource m_dummySource;

    internal AudioMixer Mixer => this.m_mixer;

    public AudioDb(AssetsDb assetsDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_assetsDb = assetsDb;
      this.m_parentGo = new GameObject("Audio");
      this.m_parentGo.tag = UnityTag.Persistent.ToString();
      assetsDb.TryGetSharedAsset<AudioMixer>("Assets/Unity/Mixer.mixer", out this.m_mixer);
      if ((UnityEngine.Object) this.m_mixer == (UnityEngine.Object) null)
      {
        Log.Error("No audio mixer found!");
      }
      else
      {
        Array values = Enum.GetValues(typeof (AudioChannel));
        this.m_channels = new AudioMixerGroup[values.Length];
        this.m_volumeVariableNames = new string[values.Length];
        foreach (AudioChannel index in values)
        {
          AudioMixerGroup[] matchingGroups = this.m_mixer.FindMatchingGroups(index.ToString());
          if (matchingGroups.Length == 0)
            Log.Error(string.Format("Could not find group '{0}' on the mixer", (object) index));
          else if ((long) (uint) index >= (long) values.Length)
          {
            Log.Error(string.Format("Invalid channel ID '{0}', it must be in range [0, {1}].", (object) index, (object) (values.Length - 1)));
          }
          else
          {
            this.m_channels[(int) index] = matchingGroups[0];
            string str = index.ToString() + "Volume";
            this.m_volumeVariableNames[(int) index] = str;
            if (PlayerPrefs.HasKey(str))
            {
              this.m_mixer.SetFloat(str, PlayerPrefs.GetFloat(str, 1f));
            }
            else
            {
              float volumePercent;
              if (AudioDb.DEFAULT_VOLUMES.TryGetValue<AudioChannel, float>(index, out volumePercent))
                this.SetChannelVolume(index, volumePercent);
              else
                Log.Warning(string.Format("Audio channel '{0}' has no default value.", (object) index));
            }
          }
        }
        this.m_dummySource = new AudioSource();
      }
    }

    public void SetChannelVolume(AudioChannel channel, float volumePercent)
    {
      volumePercent = volumePercent.Clamp(AudioDb.MIN_VOLUME_PERCENT, AudioDb.MAX_VOLUME_PERCENT);
      string volumeVariableName = this.m_volumeVariableNames[(int) channel];
      float db = AudioDb.volumePercentToDB(volumePercent);
      Assert.That<float>(db).IsWithinIncl(-80f, 0.0f, "Volume is out of range");
      this.m_mixer.SetFloat(volumeVariableName, db);
      PlayerPrefs.SetFloat(volumeVariableName, db);
      PlayerPrefs.Save();
    }

    public float GetChannelVolume(AudioChannel channel)
    {
      string volumeVariableName = this.m_volumeVariableNames[(int) channel];
      float defaultValue;
      this.m_mixer.GetFloat(volumeVariableName, out defaultValue);
      float num = PlayerPrefs.GetFloat(volumeVariableName, defaultValue).Clamp(-80f, 0.0f);
      return float.IsNaN(num) ? AudioDb.MIN_VOLUME_PERCENT : AudioDb.volumeDBToPercent(num);
    }

    private static float volumePercentToDB(float percent) => Mathf.Log10(percent) * 20f;

    private static float volumeDBToPercent(float volume) => Mathf.Pow(10f, volume / 20f);

    public AudioSource GetClonedAudio(AudioInfo audio)
    {
      return this.GetClonedAudio(audio.PrefabPath, audio.Channel);
    }

    public bool ContainsAsset(string assetPath) => this.m_assetsDb.ContainsAsset(assetPath);

    public AudioSource GetClonedAudio(
      string prefabPath,
      AudioChannel channel,
      Option<GameObject> parentGo = default (Option<GameObject>))
    {
      if (prefabPath == "TODO")
        return this.m_dummySource;
      GameObject prefab;
      if (!this.m_assetsDb.TryGetClonedPrefab(prefabPath, out prefab))
      {
        Assert.Fail("Could not load audio prefab from '" + prefabPath + "'");
        return this.m_dummySource;
      }
      prefab.transform.SetParent(parentGo.ValueOr(this.m_parentGo).transform, false);
      prefab.SetActive(true);
      AudioSource component = prefab.GetComponent<AudioSource>();
      component.outputAudioMixerGroup = this.m_channels[(int) channel];
      return component;
    }

    public AudioSource GetSharedAudio(AudioInfo audio)
    {
      return this.GetSharedAudio(audio.PrefabPath, audio.Channel);
    }

    public AudioSource GetSharedAudioUi(string prefabPath)
    {
      return this.GetSharedAudio(prefabPath, AudioChannel.UserInterface);
    }

    public AudioSource GetSharedAudio(string prefabPath, AudioChannel channel)
    {
      AudioSource productProto;
      if (!this.TryGet(prefabPath, out productProto))
      {
        productProto = this.GetClonedAudio(prefabPath, channel);
        this.Add(prefabPath, productProto);
      }
      return productProto;
    }

    public bool TryGetSharedClip(string prefabPath, out AudioClip clip)
    {
      if (this.m_assetsDb.TryGetSharedAsset<AudioClip>(prefabPath, out clip))
        return true;
      Assert.Fail("Could not load audio clip from '" + prefabPath + "'");
      return false;
    }

    public AudioMixerGroup GetChannel(AudioChannel channel)
    {
      AudioMixerGroup channel1 = this.m_channels[(int) channel];
      Assert.That<AudioMixerGroup>(channel1).IsValidUnityObject<AudioMixerGroup>("Invalid channel");
      return channel1;
    }

    static AudioDb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      AudioDb.MIN_VOLUME_PERCENT = AudioDb.volumeDBToPercent(-80f);
      AudioDb.MAX_VOLUME_PERCENT = AudioDb.volumeDBToPercent(0.0f);
      Lyst<KeyValuePair<AudioChannel, float>> list = new Lyst<KeyValuePair<AudioChannel, float>>();
      list.Add<AudioChannel, float>(AudioChannel.Master, 0.5f);
      list.Add<AudioChannel, float>(AudioChannel.Music, 0.35f);
      list.Add<AudioChannel, float>(AudioChannel.EffectsGroup, 1f);
      list.Add<AudioChannel, float>(AudioChannel.UserInterface, 0.75f);
      list.Add<AudioChannel, float>(AudioChannel.Machines, 0.3f);
      list.Add<AudioChannel, float>(AudioChannel.Weather, 0.5f);
      AudioDb.DEFAULT_VOLUMES = list;
    }
  }
}
