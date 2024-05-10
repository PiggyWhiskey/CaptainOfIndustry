// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Game.DiffSettingInfo`1
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Localization;
using System;
using System.Reflection;

#nullable disable
namespace Mafi.Core.Game
{
  public abstract class DiffSettingInfo<T> : IDiffSettingInfo
  {
    public readonly LocStrFormatted Tooltip;
    public readonly T[] Options;

    public string ValueMemberName { get; }

    public LocStrFormatted Title { get; }

    public PropertyInfo Property { get; }

    public DiffSettingInfo(
      string valueMemberName,
      LocStrFormatted title,
      LocStrFormatted tooltip,
      T[] options)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ValueMemberName = valueMemberName;
      this.Title = title;
      this.Tooltip = tooltip;
      this.Options = options;
      this.Property = typeof (GameDifficultyConfig).GetProperty(valueMemberName);
      if (this.Property == (PropertyInfo) null)
        throw new InvalidOperationException(this.ValueMemberName + " not found on GameDifficultyConfig!");
    }

    public abstract string CreateLabel(T value);

    public abstract PropDifficultyRating GetDifficultyRating(T value);

    public void SetValue(GameDifficultyConfig config, T value)
    {
      this.Property.SetValue((object) config, (object) value);
    }

    public T GetValue(GameDifficultyConfig config) => (T) this.Property.GetValue((object) config);

    public void OverrideTargetWithSource(GameDifficultyConfig target, GameDifficultyConfig source)
    {
      this.SetValue(target, this.GetValue(source));
    }

    public Option<GameDifficultyOptionChange> GetDiff(
      GameDifficultyConfig before,
      GameDifficultyConfig after)
    {
      T oldValue = this.GetValue(before);
      T newValue = this.GetValue(after);
      DiffSettingInfo<T>.DiffResult diffResult = this.CompareValuesInternal(oldValue, newValue);
      return diffResult == DiffSettingInfo<T>.DiffResult.Same ? Option<GameDifficultyOptionChange>.None : (Option<GameDifficultyOptionChange>) new GameDifficultyOptionChange(this.ValueMemberName, (object) oldValue, (object) newValue, diffResult == DiffSettingInfo<T>.DiffResult.NewIsEasier);
    }

    public bool AreSame(GameDifficultyConfig before, GameDifficultyConfig after)
    {
      return this.CompareValuesInternal(this.GetValue(before), this.GetValue(after)) == DiffSettingInfo<T>.DiffResult.Same;
    }

    public string ConvertValueToString(object value)
    {
      return value is T obj ? this.ValToStringInternal(obj) : value.ToString();
    }

    protected abstract string ValToStringInternal(T value);

    protected abstract DiffSettingInfo<T>.DiffResult CompareValuesInternal(T oldValue, T newValue);

    protected enum DiffResult
    {
      NewIsEasier,
      Same,
      NewIsHarder,
    }
  }
}
