// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Fleet.PlayersBattleResult
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Fleet
{
  /// <summary>Battle results from the player's view.</summary>
  /// <remarks>
  /// All players results are stored in saved game for statistics. Do not store any references to game objects here.
  /// </remarks>
  [GenerateSerializer(false, null, 0)]
  public class PlayersBattleResult
  {
    public readonly FleetBattleResultStats PlayerFleetStats;
    public readonly FleetBattleResultStats OpponentFleetStats;
    public readonly bool PlayerWon;
    public readonly bool PlayerWasAttacker;
    public readonly int BattleRoundsCount;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public PlayersBattleResult(
      FleetBattleResultStats playerFleetStats,
      FleetBattleResultStats opponentFleetStats,
      bool playerWon,
      bool playerWasAttacker,
      int battleRoundsCount)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.PlayerFleetStats = playerFleetStats.CheckNotNull<FleetBattleResultStats>();
      this.OpponentFleetStats = opponentFleetStats.CheckNotNull<FleetBattleResultStats>();
      this.PlayerWon = playerWon;
      this.PlayerWasAttacker = playerWasAttacker;
      this.BattleRoundsCount = battleRoundsCount;
    }

    public static void Serialize(PlayersBattleResult value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<PlayersBattleResult>(value))
        return;
      writer.EnqueueDataSerialization((object) value, PlayersBattleResult.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteInt(this.BattleRoundsCount);
      FleetBattleResultStats.Serialize(this.OpponentFleetStats, writer);
      FleetBattleResultStats.Serialize(this.PlayerFleetStats, writer);
      writer.WriteBool(this.PlayerWasAttacker);
      writer.WriteBool(this.PlayerWon);
    }

    public static PlayersBattleResult Deserialize(BlobReader reader)
    {
      PlayersBattleResult playersBattleResult;
      if (reader.TryStartClassDeserialization<PlayersBattleResult>(out playersBattleResult))
        reader.EnqueueDataDeserialization((object) playersBattleResult, PlayersBattleResult.s_deserializeDataDelayedAction);
      return playersBattleResult;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<PlayersBattleResult>(this, "BattleRoundsCount", (object) reader.ReadInt());
      reader.SetField<PlayersBattleResult>(this, "OpponentFleetStats", (object) FleetBattleResultStats.Deserialize(reader));
      reader.SetField<PlayersBattleResult>(this, "PlayerFleetStats", (object) FleetBattleResultStats.Deserialize(reader));
      reader.SetField<PlayersBattleResult>(this, "PlayerWasAttacker", (object) reader.ReadBool());
      reader.SetField<PlayersBattleResult>(this, "PlayerWon", (object) reader.ReadBool());
    }

    static PlayersBattleResult()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      PlayersBattleResult.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((PlayersBattleResult) obj).SerializeData(writer));
      PlayersBattleResult.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((PlayersBattleResult) obj).DeserializeData(reader));
    }
  }
}
