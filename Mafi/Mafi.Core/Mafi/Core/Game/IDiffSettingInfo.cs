// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Game.IDiffSettingInfo
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Localization;
using System.Reflection;

#nullable disable
namespace Mafi.Core.Game
{
  public interface IDiffSettingInfo
  {
    string ValueMemberName { get; }

    PropertyInfo Property { get; }

    LocStrFormatted Title { get; }

    void OverrideTargetWithSource(GameDifficultyConfig target, GameDifficultyConfig source);

    Option<GameDifficultyOptionChange> GetDiff(
      GameDifficultyConfig before,
      GameDifficultyConfig after);

    string ConvertValueToString(object value);

    bool AreSame(GameDifficultyConfig before, GameDifficultyConfig after);
  }
}
