// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.Goals.GoalToPauseEntity
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Messages.Goals
{
  [GenerateSerializer(false, null, 0)]
  public class GoalToPauseEntity : Goal
  {
    private readonly GoalToPauseEntity.Proto m_goalProto;
    private readonly EntitiesManager m_entitiesManager;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public GoalToPauseEntity(GoalToPauseEntity.Proto goalProto, EntitiesManager entitiesManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector((GoalProto) goalProto);
      this.m_goalProto = goalProto;
      this.m_entitiesManager = entitiesManager;
      this.Title = goalProto.Title.Value;
    }

    protected override bool UpdateInternal()
    {
      bool flag = false;
      foreach (StaticEntity staticEntity in this.m_entitiesManager.GetAllEntitiesOfType<StaticEntity>((Predicate<StaticEntity>) (x => (Mafi.Core.Prototypes.Proto) x.Prototype == (Mafi.Core.Prototypes.Proto) this.m_goalProto.ProtoToPause)))
      {
        flag = true;
        if (staticEntity.IsPaused)
          return true;
      }
      return !flag;
    }

    protected override void UpdateTitleOnLoad() => this.Title = this.m_goalProto.Title.Value;

    public static void Serialize(GoalToPauseEntity value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<GoalToPauseEntity>(value))
        return;
      writer.EnqueueDataSerialization((object) value, GoalToPauseEntity.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntitiesManager.Serialize(this.m_entitiesManager, writer);
      writer.WriteGeneric<GoalToPauseEntity.Proto>(this.m_goalProto);
    }

    public static GoalToPauseEntity Deserialize(BlobReader reader)
    {
      GoalToPauseEntity goalToPauseEntity;
      if (reader.TryStartClassDeserialization<GoalToPauseEntity>(out goalToPauseEntity))
        reader.EnqueueDataDeserialization((object) goalToPauseEntity, GoalToPauseEntity.s_deserializeDataDelayedAction);
      return goalToPauseEntity;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<GoalToPauseEntity>(this, "m_entitiesManager", (object) EntitiesManager.Deserialize(reader));
      reader.SetField<GoalToPauseEntity>(this, "m_goalProto", (object) reader.ReadGenericAs<GoalToPauseEntity.Proto>());
    }

    static GoalToPauseEntity()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      GoalToPauseEntity.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Goal) obj).SerializeData(writer));
      GoalToPauseEntity.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Goal) obj).DeserializeData(reader));
    }

    public class Proto : GoalProto
    {
      public readonly LocStrFormatted Title;
      public readonly StaticEntityProto ProtoToPause;

      public override Type Implementation => typeof (GoalToPauseEntity);

      public Proto(
        string id,
        StaticEntityProto protoToPause,
        LocStrFormatted title,
        int lockedByIndex = -1)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(id, new Mafi.Core.Prototypes.Proto.ID?(), lockedByIndex);
        this.ProtoToPause = protoToPause;
        this.Title = title;
      }
    }
  }
}
