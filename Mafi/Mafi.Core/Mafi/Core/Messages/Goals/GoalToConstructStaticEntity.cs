// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.Goals.GoalToConstructStaticEntity
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Messages.Goals
{
  [GenerateSerializer(false, null, 0)]
  public class GoalToConstructStaticEntity : Goal
  {
    private readonly GoalToConstructStaticEntity.Proto m_goalProto;
    private readonly EntitiesManager m_entitiesManager;
    private readonly int[] m_currentCounts;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public GoalToConstructStaticEntity(
      GoalToConstructStaticEntity.Proto goalProto,
      EntitiesManager entitiesManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector((GoalProto) goalProto);
      this.m_goalProto = goalProto;
      this.m_entitiesManager = entitiesManager;
      this.m_currentCounts = new int[this.m_goalProto.ProtosToBuild.Length];
      this.updateTitle();
    }

    protected override bool UpdateInternal()
    {
      int index = 0;
      bool flag1 = false;
      bool flag2 = true;
      foreach (KeyValuePair<StaticEntityProto, int> keyValuePair in this.m_goalProto.ProtosToBuild)
      {
        KeyValuePair<StaticEntityProto, int> pair = keyValuePair;
        int countOf;
        if (pair.Key is IProtoWithUpgrade key && key.UpgradeNonGeneric.NextTierNonGeneric.HasValue)
        {
          IProtoWithUpgrade nextTier = key.UpgradeNonGeneric.NextTierNonGeneric.Value;
          countOf = this.m_entitiesManager.GetCountOf<StaticEntity>((Predicate<StaticEntity>) (x =>
          {
            if (!x.IsConstructed)
              return false;
            return (Mafi.Core.Prototypes.Proto) x.Prototype == (Mafi.Core.Prototypes.Proto) pair.Key || x.Prototype == nextTier;
          }));
        }
        else
          countOf = this.m_entitiesManager.GetCountOf<StaticEntity>((Predicate<StaticEntity>) (x => x.IsConstructed && (Mafi.Core.Prototypes.Proto) x.Prototype == (Mafi.Core.Prototypes.Proto) pair.Key));
        if (this.m_currentCounts[index] != countOf)
        {
          this.m_currentCounts[index] = countOf;
          flag1 = true;
        }
        flag2 &= countOf >= pair.Value;
        ++index;
      }
      if (flag1)
        this.updateTitle();
      return flag2;
    }

    private void updateTitle() => this.Title = this.m_goalProto.TitleFunc(this.m_currentCounts);

    protected override void UpdateTitleOnLoad() => this.updateTitle();

    public static void Serialize(GoalToConstructStaticEntity value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<GoalToConstructStaticEntity>(value))
        return;
      writer.EnqueueDataSerialization((object) value, GoalToConstructStaticEntity.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteArray<int>(this.m_currentCounts);
      EntitiesManager.Serialize(this.m_entitiesManager, writer);
      writer.WriteGeneric<GoalToConstructStaticEntity.Proto>(this.m_goalProto);
    }

    public static GoalToConstructStaticEntity Deserialize(BlobReader reader)
    {
      GoalToConstructStaticEntity constructStaticEntity;
      if (reader.TryStartClassDeserialization<GoalToConstructStaticEntity>(out constructStaticEntity))
        reader.EnqueueDataDeserialization((object) constructStaticEntity, GoalToConstructStaticEntity.s_deserializeDataDelayedAction);
      return constructStaticEntity;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<GoalToConstructStaticEntity>(this, "m_currentCounts", (object) reader.ReadArray<int>());
      reader.SetField<GoalToConstructStaticEntity>(this, "m_entitiesManager", (object) EntitiesManager.Deserialize(reader));
      reader.SetField<GoalToConstructStaticEntity>(this, "m_goalProto", (object) reader.ReadGenericAs<GoalToConstructStaticEntity.Proto>());
    }

    static GoalToConstructStaticEntity()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      GoalToConstructStaticEntity.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Goal) obj).SerializeData(writer));
      GoalToConstructStaticEntity.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Goal) obj).DeserializeData(reader));
    }

    public class Proto : GoalProto
    {
      public static readonly LocStr1 TITLE_BUILD;
      public static readonly LocStr1 TITLE_BUILD_ANOTHER;
      public static readonly LocStr1 TITLE_BUILD_AND_CONNECT;
      public static readonly LocStr1 TITLE_RESEARCH_AND_BUILD;
      public static readonly LocStr1 TITLE_RESEARCH_AND_UPGRADE;
      public readonly ImmutableArray<KeyValuePair<StaticEntityProto, int>> ProtosToBuild;
      public readonly Func<int[], string> TitleFunc;

      public override Type Implementation => typeof (GoalToConstructStaticEntity);

      public Proto(
        string id,
        KeyValuePair<StaticEntityProto, int> protoToBuild,
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
        GoalToConstructStaticEntity.Proto proto = this;
        this.ProtosToBuild = ImmutableArray.Create<KeyValuePair<StaticEntityProto, int>>(protoToBuild);
        this.TitleFunc = (Func<int[], string>) (counts => proto.titleWith0Arg(title, counts));
      }

      public Proto(
        string id,
        KeyValuePair<StaticEntityProto, int> protoToBuild,
        LocStr1 title,
        Mafi.Core.Prototypes.Proto.ID? tutorial = null,
        int lockedByIndex = -1,
        LocStrFormatted? tip = null,
        GoalProto.TutorialUnlockMode tutorialUnlock = GoalProto.TutorialUnlockMode.DoNotUnlock)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(id, tutorial, lockedByIndex, tip, tutorialUnlock);
        GoalToConstructStaticEntity.Proto proto = this;
        this.ProtosToBuild = ImmutableArray.Create<KeyValuePair<StaticEntityProto, int>>(protoToBuild);
        this.TitleFunc = (Func<int[], string>) (counts => proto.titleWith1Arg(title, counts));
      }

      public Proto(
        string id,
        KeyValuePair<StaticEntityProto, int> proto1,
        KeyValuePair<StaticEntityProto, int> proto2,
        LocStr2 title,
        Mafi.Core.Prototypes.Proto.ID? tutorial = null,
        int lockedByIndex = -1,
        LocStrFormatted? tip = null)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(id, tutorial, lockedByIndex, tip);
        GoalToConstructStaticEntity.Proto proto = this;
        this.ProtosToBuild = ImmutableArray.Create<KeyValuePair<StaticEntityProto, int>>(proto1, proto2);
        this.TitleFunc = (Func<int[], string>) (counts => proto.titleWith2Args(title, counts));
      }

      public Proto(
        string id,
        KeyValuePair<StaticEntityProto, int> proto1,
        KeyValuePair<StaticEntityProto, int> proto2,
        Func<int[], string> titleFunc,
        Mafi.Core.Prototypes.Proto.ID? tutorial = null,
        int lockedByIndex = -1,
        LocStrFormatted? tip = null)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(id, tutorial, lockedByIndex, tip);
        this.ProtosToBuild = ImmutableArray.Create<KeyValuePair<StaticEntityProto, int>>(proto1, proto2);
        this.TitleFunc = titleFunc;
      }

      public Proto(
        string id,
        KeyValuePair<StaticEntityProto, int> proto1,
        KeyValuePair<StaticEntityProto, int> proto2,
        KeyValuePair<StaticEntityProto, int> proto3,
        LocStr3 title,
        Mafi.Core.Prototypes.Proto.ID? tutorial = null,
        int lockedByIndex = -1)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(id, tutorial, lockedByIndex);
        GoalToConstructStaticEntity.Proto proto = this;
        this.ProtosToBuild = ImmutableArray.Create<KeyValuePair<StaticEntityProto, int>>(proto1, proto2, proto3);
        this.TitleFunc = (Func<int[], string>) (counts => proto.titleWith3Args(title, counts));
      }

      private string titleWith0Arg(LocStrFormatted source, int[] entitiesCounts)
      {
        return this.ProtosToBuild[0].Value <= 1 ? source.Value : source.ToString() + string.Format(" ({0} / {1})", (object) entitiesCounts[0], (object) this.ProtosToBuild[0].Value);
      }

      private string titleWith1Arg(LocStr1 source, int[] entitiesCounts)
      {
        return source.Format(string.Format("<bc>{0}</bc>", (object) this.ProtosToBuild[0].Key.Strings.Name)).Value + string.Format(" ({0} / {1})", (object) entitiesCounts[0], (object) this.ProtosToBuild[0].Value);
      }

      private string titleWith2Args(LocStr2 source, int[] entitiesCounts)
      {
        ref LocStr2 local1 = ref source;
        KeyValuePair<StaticEntityProto, int> keyValuePair1 = this.ProtosToBuild[0];
        // ISSUE: variable of a boxed type
        __Boxed<LocStr> name1 = (ValueType) keyValuePair1.Key.Strings.Name;
        // ISSUE: variable of a boxed type
        __Boxed<int> entitiesCount1 = (ValueType) entitiesCounts[0];
        keyValuePair1 = this.ProtosToBuild[0];
        // ISSUE: variable of a boxed type
        __Boxed<int> local2 = (ValueType) keyValuePair1.Value;
        string str1 = string.Format("<bc>{0}</bc> ({1} / {2})", (object) name1, (object) entitiesCount1, (object) local2);
        KeyValuePair<StaticEntityProto, int> keyValuePair2 = this.ProtosToBuild[1];
        // ISSUE: variable of a boxed type
        __Boxed<LocStr> name2 = (ValueType) keyValuePair2.Key.Strings.Name;
        // ISSUE: variable of a boxed type
        __Boxed<int> entitiesCount2 = (ValueType) entitiesCounts[1];
        keyValuePair2 = this.ProtosToBuild[1];
        // ISSUE: variable of a boxed type
        __Boxed<int> local3 = (ValueType) keyValuePair2.Value;
        string str2 = string.Format("<bc>{0}</bc> ({1} / {2})", (object) name2, (object) entitiesCount2, (object) local3);
        return local1.Format(str1, str2).Value;
      }

      private string titleWith3Args(LocStr3 source, int[] entitiesCounts)
      {
        ref LocStr3 local1 = ref source;
        KeyValuePair<StaticEntityProto, int> keyValuePair1 = this.ProtosToBuild[0];
        // ISSUE: variable of a boxed type
        __Boxed<LocStr> name1 = (ValueType) keyValuePair1.Key.Strings.Name;
        // ISSUE: variable of a boxed type
        __Boxed<int> entitiesCount1 = (ValueType) entitiesCounts[0];
        keyValuePair1 = this.ProtosToBuild[0];
        // ISSUE: variable of a boxed type
        __Boxed<int> local2 = (ValueType) keyValuePair1.Value;
        string str1 = string.Format("<bc>{0}</bc> ({1} / {2})", (object) name1, (object) entitiesCount1, (object) local2);
        KeyValuePair<StaticEntityProto, int> keyValuePair2 = this.ProtosToBuild[1];
        // ISSUE: variable of a boxed type
        __Boxed<LocStr> name2 = (ValueType) keyValuePair2.Key.Strings.Name;
        // ISSUE: variable of a boxed type
        __Boxed<int> entitiesCount2 = (ValueType) entitiesCounts[1];
        keyValuePair2 = this.ProtosToBuild[1];
        // ISSUE: variable of a boxed type
        __Boxed<int> local3 = (ValueType) keyValuePair2.Value;
        string str2 = string.Format("<bc>{0}</bc> ({1} / {2})", (object) name2, (object) entitiesCount2, (object) local3);
        KeyValuePair<StaticEntityProto, int> keyValuePair3 = this.ProtosToBuild[2];
        // ISSUE: variable of a boxed type
        __Boxed<LocStr> name3 = (ValueType) keyValuePair3.Key.Strings.Name;
        // ISSUE: variable of a boxed type
        __Boxed<int> entitiesCount3 = (ValueType) entitiesCounts[2];
        keyValuePair3 = this.ProtosToBuild[2];
        // ISSUE: variable of a boxed type
        __Boxed<int> local4 = (ValueType) keyValuePair3.Value;
        string str3 = string.Format("<bc>{0}</bc> ({1} / {2})", (object) name3, (object) entitiesCount3, (object) local4);
        return local1.Format(str1, str2, str3).Value;
      }

      static Proto()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        GoalToConstructStaticEntity.Proto.TITLE_BUILD = Loc.Str1("Goal__Build", "Build {0}", "text for a goal, {0} replaced with title of a building / machine");
        GoalToConstructStaticEntity.Proto.TITLE_BUILD_ANOTHER = Loc.Str1("Goal__BuildAnother", "Build another {0}", "text for a goal, {0} replaced with title of a building / machine");
        GoalToConstructStaticEntity.Proto.TITLE_BUILD_AND_CONNECT = Loc.Str1("Goal__BuildAndConnect", "Build and connect {0}", "text for a goal, {0} replaced with title of a building / machine");
        GoalToConstructStaticEntity.Proto.TITLE_RESEARCH_AND_BUILD = Loc.Str1("Goal__ResearchAndBuild", "Research and build {0}", "text for a goal, {0} replaced with title of a building / machine");
        GoalToConstructStaticEntity.Proto.TITLE_RESEARCH_AND_UPGRADE = Loc.Str1("Goal__ResearchAndUpgrade", "Research and upgrade to {0}", "text for a goal, {0} replaced with title of a building / machine");
      }
    }
  }
}
