// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Game.GameDifficultyOptionChange
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Game
{
  [GenerateSerializer(false, null, 0)]
  public class GameDifficultyOptionChange
  {
    public readonly string ValueMemberName;
    public readonly object OldValue;
    public readonly object NewValue;
    public readonly bool IsNewEasier;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public GameDifficultyOptionChange(
      string valueMemberName,
      object oldValue,
      object newValue,
      bool isNewEasier)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ValueMemberName = valueMemberName;
      this.OldValue = oldValue;
      this.NewValue = newValue;
      this.IsNewEasier = isNewEasier;
    }

    public (LocStrFormatted setting, string oldValue, string newValue) GetStrings()
    {
      IDiffSettingInfo diffSettingInfo = GameDifficultyConfig.AllOptions.FirstOrDefault((Func<IDiffSettingInfo, bool>) (x => x.ValueMemberName == this.ValueMemberName));
      if (diffSettingInfo != null)
        return (diffSettingInfo.Title, diffSettingInfo.ConvertValueToString(this.OldValue), diffSettingInfo.ConvertValueToString(this.NewValue));
      Log.Error("No info found for member " + this.ValueMemberName);
      return (LocStrFormatted.Empty, "", "");
    }

    public string ToNiceString()
    {
      (LocStrFormatted setting, string oldValue, string newValue) strings = this.GetStrings();
      return string.Format("{0}: {1} => {2} [{3}]", (object) strings.setting, (object) strings.oldValue, (object) strings.newValue, (object) (this.IsNewEasier ? "got easier" : "got harder"));
    }

    public static void Serialize(GameDifficultyOptionChange value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<GameDifficultyOptionChange>(value))
        return;
      writer.EnqueueDataSerialization((object) value, GameDifficultyOptionChange.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteBool(this.IsNewEasier);
      writer.WriteGeneric<object>(this.NewValue);
      writer.WriteGeneric<object>(this.OldValue);
      writer.WriteString(this.ValueMemberName);
    }

    public static GameDifficultyOptionChange Deserialize(BlobReader reader)
    {
      GameDifficultyOptionChange difficultyOptionChange;
      if (reader.TryStartClassDeserialization<GameDifficultyOptionChange>(out difficultyOptionChange))
        reader.EnqueueDataDeserialization((object) difficultyOptionChange, GameDifficultyOptionChange.s_deserializeDataDelayedAction);
      return difficultyOptionChange;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<GameDifficultyOptionChange>(this, "IsNewEasier", (object) reader.ReadBool());
      reader.SetField<GameDifficultyOptionChange>(this, "NewValue", reader.ReadGenericAs<object>());
      reader.SetField<GameDifficultyOptionChange>(this, "OldValue", reader.ReadGenericAs<object>());
      reader.SetField<GameDifficultyOptionChange>(this, "ValueMemberName", (object) reader.ReadString());
    }

    static GameDifficultyOptionChange()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      GameDifficultyOptionChange.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((GameDifficultyOptionChange) obj).SerializeData(writer));
      GameDifficultyOptionChange.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((GameDifficultyOptionChange) obj).DeserializeData(reader));
    }
  }
}
