// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.GameMenu.Settings.AudioTab
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.Audio;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.GameMenu.Settings
{
  public class AudioTab : Column
  {
    private static readonly Px LABEL_WIDTH;
    private static readonly Percent COMBINED_WIDTH;

    public AudioTab()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(2.pt());
      this.AlignItemsStretch<AudioTab>().Fill<AudioTab>().PaddingLeft<AudioTab>(SettingsWindow.SECTION_INDENT).PaddingBottom<AudioTab>(10.pt());
      this.Add((UiComponent) new SettingsTitle((LocStrFormatted) Tr.AudioSettings_Title), (UiComponent) slider(AudioChannel.Master, (LocStrFormatted) Tr.AudioEffectsVolume__Master), (UiComponent) slider(AudioChannel.Music, (LocStrFormatted) Tr.AudioEffectsVolume__Music), (UiComponent) slider(AudioChannel.EffectsGroup, (LocStrFormatted) Tr.AudioEffectsVolume__EffectsGroup), (UiComponent) slider(AudioChannel.UserInterface, (LocStrFormatted) Tr.AudioEffectsVolume__UI, 5.pt()), (UiComponent) slider(AudioChannel.Weather, (LocStrFormatted) Tr.AudioEffectsVolume__Ambient, 5.pt()), (UiComponent) slider(AudioChannel.Machines, (LocStrFormatted) Tr.AudioEffectsVolume__Entities, 5.pt()));

      static Slider slider(AudioChannel channel, LocStrFormatted label, Px indent = default (Px))
      {
        Slider slider = new Slider().Label<Slider>(label).LabelWidth<Slider>(AudioTab.LABEL_WIDTH - indent).Width<Slider>(AudioTab.COMBINED_WIDTH).PaddingLeft<Slider>(indent).Range(AudioDb.MIN_VOLUME_PERCENT, AudioDb.MAX_VOLUME_PERCENT);
        slider.RunWithBuilder((Action<UiBuilder>) (builder =>
        {
          Option<AudioSource> testSound = channel != AudioChannel.Music ? (Option<AudioSource>) builder.AudioDb.GetClonedAudio("Assets/Unity/UserInterface/Audio/ButtonClick.prefab", channel) : (Option<AudioSource>) Option.None;
          slider.Value(builder.AudioDb.GetChannelVolume(channel));
          slider.OnValueChanged((Action<float, float>) ((value, oldValue) =>
          {
            if (!value.IsNear(oldValue))
              builder.AudioDb.SetChannelVolume(channel, value);
            if (!testSound.HasValue)
              return;
            testSound.Value.PlayDelayed(0.05f);
          }));
        }));
        return slider;
      }
    }

    static AudioTab()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      AudioTab.LABEL_WIDTH = 70.pt();
      AudioTab.COMBINED_WIDTH = 70.Percent();
    }
  }
}
