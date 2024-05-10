// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Game.ChangeGameDifficultyCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Game
{
  [GenerateSerializer(false, null, 0)]
  public class ChangeGameDifficultyCmd : InputCommand
  {
    [DoNotSave(0, null)]
    public readonly GameDifficultyConfig NewDifficulty;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public ChangeGameDifficultyCmd(GameDifficultyConfig newDifficulty)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.NewDifficulty = newDifficulty;
    }

    public static void Serialize(ChangeGameDifficultyCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ChangeGameDifficultyCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ChangeGameDifficultyCmd.s_serializeDataDelayedAction);
    }

    public static ChangeGameDifficultyCmd Deserialize(BlobReader reader)
    {
      ChangeGameDifficultyCmd gameDifficultyCmd;
      if (reader.TryStartClassDeserialization<ChangeGameDifficultyCmd>(out gameDifficultyCmd))
        reader.EnqueueDataDeserialization((object) gameDifficultyCmd, ChangeGameDifficultyCmd.s_deserializeDataDelayedAction);
      return gameDifficultyCmd;
    }

    static ChangeGameDifficultyCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ChangeGameDifficultyCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      ChangeGameDifficultyCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
