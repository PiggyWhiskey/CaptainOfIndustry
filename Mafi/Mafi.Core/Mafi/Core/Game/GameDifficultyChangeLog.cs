// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Game.GameDifficultyChangeLog
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Game
{
  [GenerateSerializer(false, null, 0)]
  public class GameDifficultyChangeLog
  {
    private readonly Lyst<GameDifficultyChangeLog.ChangeSet> m_allChangeSets;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    /// <summary>Oldest change is first.</summary>
    public IIndexable<GameDifficultyChangeLog.ChangeSet> AllChangeSets
    {
      get => (IIndexable<GameDifficultyChangeLog.ChangeSet>) this.m_allChangeSets;
    }

    public GameDifficultyChangeLog()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_allChangeSets = new Lyst<GameDifficultyChangeLog.ChangeSet>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public GameDate? GetLastChangeFor(IDiffSettingInfo diffInfo)
    {
      return this.getLastChangeFor(diffInfo.ValueMemberName);
    }

    public GameDate? GetLastChangeFor(GameDifficultyOptionChange optionChange)
    {
      return this.getLastChangeFor(optionChange.ValueMemberName);
    }

    private GameDate? getLastChangeFor(string memberName)
    {
      GameDate? lastChangeFor = new GameDate?();
      foreach (GameDifficultyChangeLog.ChangeSet allChangeSet in this.m_allChangeSets)
      {
        foreach (GameDifficultyOptionChange change in allChangeSet.Changes)
        {
          if (change.ValueMemberName == memberName)
            lastChangeFor = new GameDate?(allChangeSet.DateChanged);
        }
      }
      return lastChangeFor;
    }

    public void AddChangeSet(GameDifficultyChangeLog.ChangeSet diffRecord)
    {
      this.m_allChangeSets.Add(diffRecord);
    }

    public static void Serialize(GameDifficultyChangeLog value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<GameDifficultyChangeLog>(value))
        return;
      writer.EnqueueDataSerialization((object) value, GameDifficultyChangeLog.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Lyst<GameDifficultyChangeLog.ChangeSet>.Serialize(this.m_allChangeSets, writer);
    }

    public static GameDifficultyChangeLog Deserialize(BlobReader reader)
    {
      GameDifficultyChangeLog difficultyChangeLog;
      if (reader.TryStartClassDeserialization<GameDifficultyChangeLog>(out difficultyChangeLog))
        reader.EnqueueDataDeserialization((object) difficultyChangeLog, GameDifficultyChangeLog.s_deserializeDataDelayedAction);
      return difficultyChangeLog;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<GameDifficultyChangeLog>(this, "m_allChangeSets", (object) Lyst<GameDifficultyChangeLog.ChangeSet>.Deserialize(reader));
    }

    static GameDifficultyChangeLog()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      GameDifficultyChangeLog.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((GameDifficultyChangeLog) obj).SerializeData(writer));
      GameDifficultyChangeLog.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((GameDifficultyChangeLog) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    public struct ChangeSet
    {
      public readonly GameDate DateChanged;
      public readonly ImmutableArray<GameDifficultyOptionChange> Changes;

      public ChangeSet(GameDate dateChanged, ImmutableArray<GameDifficultyOptionChange> changes)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.DateChanged = dateChanged;
        this.Changes = changes;
      }

      public static void Serialize(GameDifficultyChangeLog.ChangeSet value, BlobWriter writer)
      {
        GameDate.Serialize(value.DateChanged, writer);
        ImmutableArray<GameDifficultyOptionChange>.Serialize(value.Changes, writer);
      }

      public static GameDifficultyChangeLog.ChangeSet Deserialize(BlobReader reader)
      {
        return new GameDifficultyChangeLog.ChangeSet(GameDate.Deserialize(reader), ImmutableArray<GameDifficultyOptionChange>.Deserialize(reader));
      }
    }
  }
}
