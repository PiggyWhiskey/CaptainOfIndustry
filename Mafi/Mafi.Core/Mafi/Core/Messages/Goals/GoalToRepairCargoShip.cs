// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.Goals.GoalToRepairCargoShip
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.Cargo.Modules;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Messages.Goals
{
  [GenerateSerializer(false, null, 0)]
  public class GoalToRepairCargoShip : Goal
  {
    private readonly GoalToRepairCargoShip.Proto m_goalProto;
    private readonly CargoDepotManager m_cargoDepotManager;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public GoalToRepairCargoShip(
      GoalToRepairCargoShip.Proto goalProto,
      CargoDepotManager cargoDepotManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector((GoalProto) goalProto);
      this.m_goalProto = goalProto;
      this.m_cargoDepotManager = cargoDepotManager;
      this.Title = this.m_goalProto.Title.Value;
    }

    protected override bool UpdateInternal()
    {
      return this.m_cargoDepotManager.RepairedUnusedShips.Count > 0 || this.m_cargoDepotManager.AmountOfShipsInUse > 0;
    }

    protected override void UpdateTitleOnLoad() => this.Title = this.m_goalProto.Title.Value;

    public static void Serialize(GoalToRepairCargoShip value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<GoalToRepairCargoShip>(value))
        return;
      writer.EnqueueDataSerialization((object) value, GoalToRepairCargoShip.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      CargoDepotManager.Serialize(this.m_cargoDepotManager, writer);
      writer.WriteGeneric<GoalToRepairCargoShip.Proto>(this.m_goalProto);
    }

    public static GoalToRepairCargoShip Deserialize(BlobReader reader)
    {
      GoalToRepairCargoShip toRepairCargoShip;
      if (reader.TryStartClassDeserialization<GoalToRepairCargoShip>(out toRepairCargoShip))
        reader.EnqueueDataDeserialization((object) toRepairCargoShip, GoalToRepairCargoShip.s_deserializeDataDelayedAction);
      return toRepairCargoShip;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<GoalToRepairCargoShip>(this, "m_cargoDepotManager", (object) CargoDepotManager.Deserialize(reader));
      reader.SetField<GoalToRepairCargoShip>(this, "m_goalProto", (object) reader.ReadGenericAs<GoalToRepairCargoShip.Proto>());
    }

    static GoalToRepairCargoShip()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      GoalToRepairCargoShip.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Goal) obj).SerializeData(writer));
      GoalToRepairCargoShip.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Goal) obj).DeserializeData(reader));
    }

    public class Proto : GoalProto
    {
      public readonly LocStrFormatted Title;

      public override Type Implementation => typeof (GoalToRepairCargoShip);

      public Proto(
        string id,
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
        this.Title = title;
      }
    }
  }
}
