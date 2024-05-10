// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.Goals.GoalToConstructNumberOfStaticEntities
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Messages.Goals
{
  [GenerateSerializer(false, null, 0)]
  public class GoalToConstructNumberOfStaticEntities : Goal
  {
    private readonly GoalToConstructNumberOfStaticEntities.Proto m_goalProto;
    private readonly EntitiesManager m_entitiesManager;
    private int m_currentCount;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public GoalToConstructNumberOfStaticEntities(
      GoalToConstructNumberOfStaticEntities.Proto goalProto,
      EntitiesManager entitiesManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector((GoalProto) goalProto);
      this.m_goalProto = goalProto;
      this.m_entitiesManager = entitiesManager;
      this.updateTitle();
    }

    protected override bool UpdateInternal()
    {
      int num = 0;
      foreach (StaticEntityProto.ID protosId in this.m_goalProto.ProtosIds)
      {
        StaticEntityProto.ID protoId = protosId;
        num += this.m_entitiesManager.GetCountOf<StaticEntity>((Predicate<StaticEntity>) (x => x.IsConstructed && x.Prototype.Id == protoId));
      }
      if (num != this.m_currentCount)
      {
        this.m_currentCount = num;
        this.updateTitle();
      }
      return this.m_currentCount >= this.m_goalProto.EntitiesCount;
    }

    private void updateTitle()
    {
      this.Title = this.m_goalProto.Title.ToString() + string.Format(" ({0} / {1})", (object) this.m_currentCount, (object) this.m_goalProto.EntitiesCount);
    }

    protected override void UpdateTitleOnLoad() => this.updateTitle();

    public static void Serialize(GoalToConstructNumberOfStaticEntities value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<GoalToConstructNumberOfStaticEntities>(value))
        return;
      writer.EnqueueDataSerialization((object) value, GoalToConstructNumberOfStaticEntities.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteInt(this.m_currentCount);
      EntitiesManager.Serialize(this.m_entitiesManager, writer);
      writer.WriteGeneric<GoalToConstructNumberOfStaticEntities.Proto>(this.m_goalProto);
    }

    public static GoalToConstructNumberOfStaticEntities Deserialize(BlobReader reader)
    {
      GoalToConstructNumberOfStaticEntities ofStaticEntities;
      if (reader.TryStartClassDeserialization<GoalToConstructNumberOfStaticEntities>(out ofStaticEntities))
        reader.EnqueueDataDeserialization((object) ofStaticEntities, GoalToConstructNumberOfStaticEntities.s_deserializeDataDelayedAction);
      return ofStaticEntities;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.m_currentCount = reader.ReadInt();
      reader.SetField<GoalToConstructNumberOfStaticEntities>(this, "m_entitiesManager", (object) EntitiesManager.Deserialize(reader));
      reader.SetField<GoalToConstructNumberOfStaticEntities>(this, "m_goalProto", (object) reader.ReadGenericAs<GoalToConstructNumberOfStaticEntities.Proto>());
    }

    static GoalToConstructNumberOfStaticEntities()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      GoalToConstructNumberOfStaticEntities.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Goal) obj).SerializeData(writer));
      GoalToConstructNumberOfStaticEntities.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Goal) obj).DeserializeData(reader));
    }

    public class Proto : GoalProto
    {
      public readonly ImmutableArray<StaticEntityProto.ID> ProtosIds;
      public readonly int EntitiesCount;
      public readonly LocStrFormatted Title;

      public override Type Implementation => typeof (GoalToConstructNumberOfStaticEntities);

      public Proto(
        string id,
        ImmutableArray<StaticEntityProto.ID> protosIds,
        int entitiesCount,
        LocStrFormatted title,
        Mafi.Core.Prototypes.Proto.ID? tutorial = null,
        int lockedByIndex = -1,
        GoalProto.TutorialUnlockMode tutorialUnlock = GoalProto.TutorialUnlockMode.DoNotUnlock)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        string id1 = id;
        Mafi.Core.Prototypes.Proto.ID? tutorial1 = tutorial;
        int lockedByIndex1 = lockedByIndex;
        GoalProto.TutorialUnlockMode tutorialUnlockMode = tutorialUnlock;
        LocStrFormatted? tip = new LocStrFormatted?();
        int tutorialUnlock1 = (int) tutorialUnlockMode;
        // ISSUE: explicit constructor call
        base.\u002Ector(id1, tutorial1, lockedByIndex1, tip, (GoalProto.TutorialUnlockMode) tutorialUnlock1);
        this.ProtosIds = protosIds;
        this.EntitiesCount = entitiesCount;
        this.Title = title;
      }
    }
  }
}
