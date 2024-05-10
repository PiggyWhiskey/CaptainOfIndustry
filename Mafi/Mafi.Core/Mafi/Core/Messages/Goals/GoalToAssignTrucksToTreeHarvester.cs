// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.Goals.GoalToAssignTrucksToTreeHarvester
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Vehicles;
using Mafi.Core.Vehicles.TreeHarvesters;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Linq;

#nullable disable
namespace Mafi.Core.Messages.Goals
{
  [GenerateSerializer(false, null, 0)]
  public class GoalToAssignTrucksToTreeHarvester : Goal
  {
    private readonly GoalToAssignTrucksToTreeHarvester.Proto m_goalProto;
    private readonly VehiclesManager m_vehiclesManager;
    private int m_assignedCurrently;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public GoalToAssignTrucksToTreeHarvester(
      GoalToAssignTrucksToTreeHarvester.Proto goalProto,
      VehiclesManager vehiclesManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector((GoalProto) goalProto);
      this.m_goalProto = goalProto;
      this.m_vehiclesManager = vehiclesManager;
      this.updateTitle();
    }

    protected override bool UpdateInternal()
    {
      int num = this.m_vehiclesManager.TreeHarvesters.Sum<TreeHarvester>((Func<TreeHarvester, int>) (x => x.AllVehicles.Count));
      if (this.m_assignedCurrently != num)
      {
        this.m_assignedCurrently = num;
        this.updateTitle();
      }
      return this.m_assignedCurrently >= this.m_goalProto.NumberOfTrucksRequired;
    }

    private void updateTitle()
    {
      this.Title = this.m_goalProto.Title.Format(string.Format("({0} / {1})", (object) this.m_assignedCurrently, (object) this.m_goalProto.NumberOfTrucksRequired), this.m_goalProto.VehicleToAssignTo.Strings.Name.TranslatedString).Value;
    }

    protected override void UpdateTitleOnLoad() => this.updateTitle();

    public static void Serialize(GoalToAssignTrucksToTreeHarvester value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<GoalToAssignTrucksToTreeHarvester>(value))
        return;
      writer.EnqueueDataSerialization((object) value, GoalToAssignTrucksToTreeHarvester.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteInt(this.m_assignedCurrently);
      writer.WriteGeneric<GoalToAssignTrucksToTreeHarvester.Proto>(this.m_goalProto);
      VehiclesManager.Serialize(this.m_vehiclesManager, writer);
    }

    public static GoalToAssignTrucksToTreeHarvester Deserialize(BlobReader reader)
    {
      GoalToAssignTrucksToTreeHarvester trucksToTreeHarvester;
      if (reader.TryStartClassDeserialization<GoalToAssignTrucksToTreeHarvester>(out trucksToTreeHarvester))
        reader.EnqueueDataDeserialization((object) trucksToTreeHarvester, GoalToAssignTrucksToTreeHarvester.s_deserializeDataDelayedAction);
      return trucksToTreeHarvester;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.m_assignedCurrently = reader.ReadInt();
      reader.SetField<GoalToAssignTrucksToTreeHarvester>(this, "m_goalProto", (object) reader.ReadGenericAs<GoalToAssignTrucksToTreeHarvester.Proto>());
      reader.SetField<GoalToAssignTrucksToTreeHarvester>(this, "m_vehiclesManager", (object) VehiclesManager.Deserialize(reader));
    }

    static GoalToAssignTrucksToTreeHarvester()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      GoalToAssignTrucksToTreeHarvester.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Goal) obj).SerializeData(writer));
      GoalToAssignTrucksToTreeHarvester.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Goal) obj).DeserializeData(reader));
    }

    public class Proto : GoalProto
    {
      public readonly LocStr2 Title;
      public readonly int NumberOfTrucksRequired;
      public readonly TreeHarvesterProto VehicleToAssignTo;

      public override Type Implementation => typeof (GoalToAssignTrucksToTreeHarvester);

      public Proto(
        string id,
        LocStr2 title,
        TreeHarvesterProto proto,
        int numberOfTrucksRequired,
        int lockedByIndex = -1)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(id, new Mafi.Core.Prototypes.Proto.ID?(), lockedByIndex);
        this.VehicleToAssignTo = proto;
        this.NumberOfTrucksRequired = numberOfTrucksRequired;
        this.Title = title;
      }
    }
  }
}
