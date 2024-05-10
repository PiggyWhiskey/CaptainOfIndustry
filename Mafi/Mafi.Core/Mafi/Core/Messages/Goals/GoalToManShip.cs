// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.Goals.GoalToManShip
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
  public class GoalToManShip : Goal
  {
    private readonly GoalToManShip.Proto m_goalProto;
    private readonly TravelingFleetManager m_fleetManager;
    private int m_currentCrew;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public GoalToManShip(GoalToManShip.Proto goalProto, TravelingFleetManager fleetManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector((GoalProto) goalProto);
      this.m_goalProto = goalProto;
      this.m_fleetManager = fleetManager;
      this.updateTitle();
    }

    protected override bool UpdateInternal()
    {
      int currentCrew = this.m_fleetManager.HasFleet ? this.m_fleetManager.TravelingFleet.CurrentCrew : 0;
      if (this.m_currentCrew != currentCrew)
      {
        this.m_currentCrew = currentCrew;
        this.updateTitle();
      }
      return this.m_fleetManager.HasFleet && this.m_currentCrew >= this.m_fleetManager.TravelingFleet.CrewRequired;
    }

    private void updateTitle()
    {
      this.Title = this.m_goalProto.Title.Value;
      if (!this.m_fleetManager.HasFleet)
        return;
      this.Title += string.Format(" ({0} / {1})", (object) this.m_currentCrew, (object) this.m_fleetManager.TravelingFleet.CrewRequired);
    }

    protected override void UpdateTitleOnLoad() => this.updateTitle();

    public static void Serialize(GoalToManShip value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<GoalToManShip>(value))
        return;
      writer.EnqueueDataSerialization((object) value, GoalToManShip.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteInt(this.m_currentCrew);
      TravelingFleetManager.Serialize(this.m_fleetManager, writer);
      writer.WriteGeneric<GoalToManShip.Proto>(this.m_goalProto);
    }

    public static GoalToManShip Deserialize(BlobReader reader)
    {
      GoalToManShip goalToManShip;
      if (reader.TryStartClassDeserialization<GoalToManShip>(out goalToManShip))
        reader.EnqueueDataDeserialization((object) goalToManShip, GoalToManShip.s_deserializeDataDelayedAction);
      return goalToManShip;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.m_currentCrew = reader.ReadInt();
      reader.SetField<GoalToManShip>(this, "m_fleetManager", (object) TravelingFleetManager.Deserialize(reader));
      reader.SetField<GoalToManShip>(this, "m_goalProto", (object) reader.ReadGenericAs<GoalToManShip.Proto>());
    }

    static GoalToManShip()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      GoalToManShip.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Goal) obj).SerializeData(writer));
      GoalToManShip.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Goal) obj).DeserializeData(reader));
    }

    public class Proto : GoalProto
    {
      public readonly LocStrFormatted Title;

      public override Type Implementation => typeof (GoalToManShip);

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
