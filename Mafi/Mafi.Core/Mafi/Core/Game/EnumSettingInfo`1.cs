// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Game.EnumSettingInfo`1
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Localization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Game
{
  public class EnumSettingInfo<T> : DiffSettingInfo<T> where T : Enum
  {
    private readonly LocStrFormatted[] m_labels;

    public EnumSettingInfo(
      string valueMemberName,
      LocStrFormatted title,
      LocStrFormatted tooltip,
      LocStrFormatted[] labels)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(valueMemberName, title, tooltip, ((IEnumerable<T>) Enum.GetValues(typeof (T))).OrderBy<T, int>((Func<T, int>) (x => Convert.ToInt32((object) x))).ToArray<T>());
      this.m_labels = labels;
    }

    public override string CreateLabel(T value)
    {
      return this.m_labels[Array.IndexOf<T>(this.Options, value)].Value;
    }

    public override PropDifficultyRating GetDifficultyRating(T value)
    {
      int int32 = Convert.ToInt32((object) value);
      if (int32 < 0)
        return PropDifficultyRating.Easy;
      return int32 > 0 ? PropDifficultyRating.Hard : PropDifficultyRating.Standard;
    }

    protected override string ValToStringInternal(T value)
    {
      int index = Array.IndexOf<T>(this.Options, value);
      if (index >= 0)
        return this.m_labels[index].Value;
      Log.Error(string.Format("Incorrect value {0}", (object) value));
      return value.ToString();
    }

    protected override DiffSettingInfo<T>.DiffResult CompareValuesInternal(T oldValue, T newValue)
    {
      int num1 = Array.IndexOf<T>(this.Options, oldValue);
      int num2 = Array.IndexOf<T>(this.Options, newValue);
      if (num1 == num2)
        return DiffSettingInfo<T>.DiffResult.Same;
      return num1 < num2 ? DiffSettingInfo<T>.DiffResult.NewIsHarder : DiffSettingInfo<T>.DiffResult.NewIsEasier;
    }
  }
}
