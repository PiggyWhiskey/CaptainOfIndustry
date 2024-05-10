// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.Goals.GoalToRepairShip
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.World;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Messages.Goals
{
  [GenerateSerializer(false, null, 0)]
  public class GoalToRepairShip : Goal
  {
    private readonly GoalToRepairShip.Proto m_goalProto;
    private readonly TravelingFleetManager m_fleetManager;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public GoalToRepairShip(GoalToRepairShip.Proto goalProto, TravelingFleetManager fleetManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector((GoalProto) goalProto);
      this.m_goalProto = goalProto;
      this.m_fleetManager = fleetManager;
      this.Title = goalProto.Title.Value;
    }

    protected override bool UpdateInternal()
    {
      return this.m_fleetManager.HasFleet && !this.m_fleetManager.TravelingFleet.NeedsRepair;
    }

    protected override void UpdateTitleOnLoad() => this.Title = this.m_goalProto.Title.Value;

    public static void Serialize(GoalToRepairShip value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<GoalToRepairShip>(value))
        return;
      writer.EnqueueDataSerialization((object) value, GoalToRepairShip.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      TravelingFleetManager.Serialize(this.m_fleetManager, writer);
      writer.WriteGeneric<GoalToRepairShip.Proto>(this.m_goalProto);
    }

    public static GoalToRepairShip Deserialize(BlobReader reader)
    {
      GoalToRepairShip goalToRepairShip;
      if (reader.TryStartClassDeserialization<GoalToRepairShip>(out goalToRepairShip))
        reader.EnqueueDataDeserialization((object) goalToRepairShip, GoalToRepairShip.s_deserializeDataDelayedAction);
      return goalToRepairShip;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<GoalToRepairShip>(this, "m_fleetManager", (object) TravelingFleetManager.Deserialize(reader));
      reader.SetField<GoalToRepairShip>(this, "m_goalProto", (object) reader.ReadGenericAs<GoalToRepairShip.Proto>());
    }

    static GoalToRepairShip()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      GoalToRepairShip.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Goal) obj).SerializeData(writer));
      GoalToRepairShip.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Goal) obj).DeserializeData(reader));
    }

    public class Proto : GoalProto
    {
      public readonly LocStrFormatted Title;

      public override Type Implementation => typeof (GoalToRepairShip);

      public Proto(string id, LocStrFormatted title, int lockedByIndex = -1)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(id, new Mafi.Core.Prototypes.Proto.ID?(), lockedByIndex);
        this.Title = title;
      }
    }
  }
}
