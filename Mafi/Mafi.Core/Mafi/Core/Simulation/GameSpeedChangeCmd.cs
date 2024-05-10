// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Simulation.GameSpeedChangeCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Simulation
{
  [GenerateSerializer(false, null, 0)]
  public class GameSpeedChangeCmd : InputCommand
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly int NewSpeedMultiplier;

    public static void Serialize(GameSpeedChangeCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<GameSpeedChangeCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, GameSpeedChangeCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteInt(this.NewSpeedMultiplier);
    }

    public static GameSpeedChangeCmd Deserialize(BlobReader reader)
    {
      GameSpeedChangeCmd gameSpeedChangeCmd;
      if (reader.TryStartClassDeserialization<GameSpeedChangeCmd>(out gameSpeedChangeCmd))
        reader.EnqueueDataDeserialization((object) gameSpeedChangeCmd, GameSpeedChangeCmd.s_deserializeDataDelayedAction);
      return gameSpeedChangeCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<GameSpeedChangeCmd>(this, "NewSpeedMultiplier", (object) reader.ReadInt());
    }

    public GameSpeedChangeCmd(int newSpeedMultiplier)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.NewSpeedMultiplier = newSpeedMultiplier.CheckPositive();
    }

    static GameSpeedChangeCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      GameSpeedChangeCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      GameSpeedChangeCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
