// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.RenderingSetting
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Localization;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity
{
  public class RenderingSetting
  {
    public readonly string GlobalSettingKey;
    public readonly LocStr Title;
    public readonly int SortPriority;
    public readonly ImmutableArray<RenderingSettingOption> Options;
    public readonly int DefaultSettingIndex;
    private readonly Action<RenderingSetting> m_defaultCallback;
    public readonly bool UsesRenderingPresets;

    public event Action<RenderingSetting> OnSettingChange;

    public int CurrentIndex { get; private set; }

    public RenderingSettingOption CurrentOption
    {
      get
      {
        return (uint) this.CurrentIndex >= (uint) this.Options.Length ? this.Options[this.DefaultSettingIndex] : this.Options[this.CurrentIndex];
      }
    }

    public RenderingSetting(
      string globalSettingKey,
      LocStr title,
      int sortPriority,
      ImmutableArray<RenderingSettingOption> options,
      int? defaultSettingIndex = null,
      Action<RenderingSetting> defaultCallback = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.UsesRenderingPresets = options.Any((Func<RenderingSettingOption, bool>) (x => x.Preset != 0));
      if (this.UsesRenderingPresets)
      {
        foreach (RenderingSettingPreset renderingSettingPreset in Enum.GetValues(typeof (RenderingSettingPreset)))
          ;
      }
      this.GlobalSettingKey = globalSettingKey;
      this.Title = title;
      this.SortPriority = sortPriority;
      this.Options = options;
      int index;
      this.TryGetIndexForPreset(RenderingSettingPreset.HighQuality, out index);
      this.DefaultSettingIndex = defaultSettingIndex.GetValueOrDefault(index);
      this.m_defaultCallback = defaultCallback;
      int currentValue = PlayerPrefs.GetInt(this.GlobalSettingKey, int.MinValue);
      if (currentValue == int.MinValue)
        this.CurrentIndex = this.DefaultSettingIndex;
      this.CurrentIndex = this.Options.IndexOf((Predicate<RenderingSettingOption>) (x => x.Value == currentValue));
      if (this.CurrentIndex < 0)
        this.CurrentIndex = this.DefaultSettingIndex;
      if (!this.Options[this.CurrentIndex].IsSupported)
        this.CurrentIndex = this.DefaultSettingIndex;
      Assert.That<bool>(this.Options[this.DefaultSettingIndex].IsSupported).IsTrue(string.Format("Default rendering setting for {0} is not supported!", (object) title));
    }

    public bool TryGetIndexForPreset(RenderingSettingPreset preset, out int index)
    {
      for (int index1 = 0; index1 < this.Options.Length; ++index1)
      {
        if ((this.Options[index1].Preset & preset) != RenderingSettingPreset.None)
        {
          index = index1;
          return true;
        }
      }
      index = 0;
      return false;
    }

    public void SetSettingIndex(int index)
    {
      index = index.Clamp(0, this.Options.Length - 1);
      if (index == this.CurrentIndex)
        return;
      this.CurrentIndex = index;
      PlayerPrefs.SetInt(this.GlobalSettingKey, this.Options[index].Value);
      PlayerPrefs.Save();
      try
      {
        this.ForceApply();
      }
      catch (Exception ex)
      {
        Log.Exception(ex, "OnSettingChange change threw");
      }
    }

    public void ResetSavedIndex()
    {
      PlayerPrefs.DeleteKey(this.GlobalSettingKey);
      if (this.CurrentIndex == this.DefaultSettingIndex)
        return;
      this.CurrentIndex = this.DefaultSettingIndex;
      try
      {
        this.ForceApply();
      }
      catch (Exception ex)
      {
        Log.Exception(ex, "OnSettingChange change threw");
      }
    }

    internal void ClearAllCallbacks() => this.OnSettingChange = (Action<RenderingSetting>) null;

    internal void ForceApply()
    {
      Action<RenderingSetting> defaultCallback = this.m_defaultCallback;
      if (defaultCallback != null)
        defaultCallback(this);
      Action<RenderingSetting> onSettingChange = this.OnSettingChange;
      if (onSettingChange == null)
        return;
      onSettingChange(this);
    }
  }
}
