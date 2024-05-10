// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.Goals.GoalToActivateEdict
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Population.Edicts;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Messages.Goals
{
  [GenerateSerializer(false, null, 0)]
  public class GoalToActivateEdict : Goal
  {
    private readonly GoalToActivateEdict.Proto m_goalProto;
    private readonly EdictsManager m_edictsManager;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public GoalToActivateEdict(GoalToActivateEdict.Proto goalProto, EdictsManager edictsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector((GoalProto) goalProto);
      this.m_goalProto = goalProto;
      this.m_edictsManager = edictsManager;
      this.Title = this.m_goalProto.Title.Value;
    }

    protected override bool UpdateInternal()
    {
      foreach (Edict allEdict in this.m_edictsManager.AllEdicts)
      {
        if ((Mafi.Core.Prototypes.Proto) allEdict.Prototype == (Mafi.Core.Prototypes.Proto) this.m_goalProto.EdictToActivate)
          return allEdict.IsActive;
      }
      return false;
    }

    protected override void UpdateTitleOnLoad() => this.Title = this.m_goalProto.Title.Value;

    public static void Serialize(GoalToActivateEdict value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<GoalToActivateEdict>(value))
        return;
      writer.EnqueueDataSerialization((object) value, GoalToActivateEdict.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EdictsManager.Serialize(this.m_edictsManager, writer);
      writer.WriteGeneric<GoalToActivateEdict.Proto>(this.m_goalProto);
    }

    public static GoalToActivateEdict Deserialize(BlobReader reader)
    {
      GoalToActivateEdict goalToActivateEdict;
      if (reader.TryStartClassDeserialization<GoalToActivateEdict>(out goalToActivateEdict))
        reader.EnqueueDataDeserialization((object) goalToActivateEdict, GoalToActivateEdict.s_deserializeDataDelayedAction);
      return goalToActivateEdict;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<GoalToActivateEdict>(this, "m_edictsManager", (object) EdictsManager.Deserialize(reader));
      reader.SetField<GoalToActivateEdict>(this, "m_goalProto", (object) reader.ReadGenericAs<GoalToActivateEdict.Proto>());
    }

    static GoalToActivateEdict()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      GoalToActivateEdict.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Goal) obj).SerializeData(writer));
      GoalToActivateEdict.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Goal) obj).DeserializeData(reader));
    }

    public class Proto : GoalProto
    {
      public readonly LocStrFormatted Title;
      public readonly EdictProto EdictToActivate;

      public override Type Implementation => typeof (GoalToActivateEdict);

      public Proto(
        string id,
        LocStrFormatted title,
        EdictProto edictToActivate,
        int lockedByIndex = -1)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(id, new Mafi.Core.Prototypes.Proto.ID?(), lockedByIndex);
        this.EdictToActivate = edictToActivate;
        this.Title = title;
      }
    }
  }
}
