// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Console.GameConsoleCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Console
{
  [GenerateSerializer(false, null, 0)]
  public class GameConsoleCmd : InputCommand
  {
    public readonly string Command;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public GameConsoleCmd(string command)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Command = command;
    }

    public static void Serialize(GameConsoleCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<GameConsoleCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, GameConsoleCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteString(this.Command);
    }

    public static GameConsoleCmd Deserialize(BlobReader reader)
    {
      GameConsoleCmd gameConsoleCmd;
      if (reader.TryStartClassDeserialization<GameConsoleCmd>(out gameConsoleCmd))
        reader.EnqueueDataDeserialization((object) gameConsoleCmd, GameConsoleCmd.s_deserializeDataDelayedAction);
      return gameConsoleCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<GameConsoleCmd>(this, "Command", (object) reader.ReadString());
    }

    static GameConsoleCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      GameConsoleCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      GameConsoleCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
