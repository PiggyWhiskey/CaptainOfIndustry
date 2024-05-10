// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Fleet.BattleResult
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Fleet
{
  /// <summary>
  /// Battle result that has detailed info about the battle.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public class BattleResult
  {
    public readonly BattleFleet Winner;
    public readonly BattleFleet Loser;
    public readonly FleetBattleResultStats WinnerStats;
    public readonly FleetBattleResultStats LoserStats;
    public readonly ImmutableArray<string> BattleLog;
    public readonly Option<PlayersBattleResult> PlayerResult;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public BattleResult(
      BattleFleet winner,
      BattleFleet loser,
      FleetBattleResultStats winnerStats,
      FleetBattleResultStats loserStats,
      ImmutableArray<string> battleLog,
      Option<PlayersBattleResult> playerResult)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Winner = winner;
      this.Loser = loser;
      this.WinnerStats = winnerStats;
      this.LoserStats = loserStats;
      this.BattleLog = battleLog;
      this.PlayerResult = playerResult;
    }

    public static void Serialize(BattleResult value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<BattleResult>(value))
        return;
      writer.EnqueueDataSerialization((object) value, BattleResult.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      ImmutableArray<string>.Serialize(this.BattleLog, writer);
      BattleFleet.Serialize(this.Loser, writer);
      FleetBattleResultStats.Serialize(this.LoserStats, writer);
      Option<PlayersBattleResult>.Serialize(this.PlayerResult, writer);
      BattleFleet.Serialize(this.Winner, writer);
      FleetBattleResultStats.Serialize(this.WinnerStats, writer);
    }

    public static BattleResult Deserialize(BlobReader reader)
    {
      BattleResult battleResult;
      if (reader.TryStartClassDeserialization<BattleResult>(out battleResult))
        reader.EnqueueDataDeserialization((object) battleResult, BattleResult.s_deserializeDataDelayedAction);
      return battleResult;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<BattleResult>(this, "BattleLog", (object) ImmutableArray<string>.Deserialize(reader));
      reader.SetField<BattleResult>(this, "Loser", (object) BattleFleet.Deserialize(reader));
      reader.SetField<BattleResult>(this, "LoserStats", (object) FleetBattleResultStats.Deserialize(reader));
      reader.SetField<BattleResult>(this, "PlayerResult", (object) Option<PlayersBattleResult>.Deserialize(reader));
      reader.SetField<BattleResult>(this, "Winner", (object) BattleFleet.Deserialize(reader));
      reader.SetField<BattleResult>(this, "WinnerStats", (object) FleetBattleResultStats.Deserialize(reader));
    }

    static BattleResult()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      BattleResult.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((BattleResult) obj).SerializeData(writer));
      BattleResult.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((BattleResult) obj).DeserializeData(reader));
    }
  }
}
