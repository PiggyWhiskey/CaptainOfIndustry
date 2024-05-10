// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Messages.GoalToConstructVehicle
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Messages.Goals;
using Mafi.Core.Vehicles.Excavators;
using Mafi.Core.Vehicles.TreeHarvesters;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Linq;

#nullable disable
namespace Mafi.Base.Prototypes.Messages
{
  [GenerateSerializer(false, null, 0)]
  public class GoalToConstructVehicle : Goal
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly GoalToConstructVehicle.Proto m_goalProto;
    private readonly EntitiesManager m_entitiesManager;
    private readonly int m_startingCount;
    private int m_currentCount;

    public static void Serialize(GoalToConstructVehicle value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<GoalToConstructVehicle>(value))
        return;
      writer.EnqueueDataSerialization((object) value, GoalToConstructVehicle.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteInt(this.m_currentCount);
      EntitiesManager.Serialize(this.m_entitiesManager, writer);
      writer.WriteGeneric<GoalToConstructVehicle.Proto>(this.m_goalProto);
      writer.WriteInt(this.m_startingCount);
    }

    public static GoalToConstructVehicle Deserialize(BlobReader reader)
    {
      GoalToConstructVehicle constructVehicle;
      if (reader.TryStartClassDeserialization<GoalToConstructVehicle>(out constructVehicle))
        reader.EnqueueDataDeserialization((object) constructVehicle, GoalToConstructVehicle.s_deserializeDataDelayedAction);
      return constructVehicle;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.m_currentCount = reader.ReadInt();
      reader.SetField<GoalToConstructVehicle>(this, "m_entitiesManager", (object) EntitiesManager.Deserialize(reader));
      reader.SetField<GoalToConstructVehicle>(this, "m_goalProto", (object) reader.ReadGenericAs<GoalToConstructVehicle.Proto>());
      reader.SetField<GoalToConstructVehicle>(this, "m_startingCount", (object) reader.ReadInt());
    }

    public GoalToConstructVehicle(
      GoalToConstructVehicle.Proto goalProto,
      EntitiesManager entitiesManager,
      IStartingFactoryConfig startingFactoryConfig)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector((GoalProto) goalProto);
      this.m_goalProto = goalProto;
      this.m_entitiesManager = entitiesManager;
      this.m_startingCount = !(this.m_goalProto.ProtoToBuild is TruckProto) ? (!(this.m_goalProto.ProtoToBuild is ExcavatorProto) ? (!(this.m_goalProto.ProtoToBuild is TreeHarvesterProto) ? 0 : startingFactoryConfig.InitialTreeHarvesters) : startingFactoryConfig.InitialExcavators) : startingFactoryConfig.InitialTrucks;
      this.updateTitle();
    }

    protected override bool UpdateInternal()
    {
      int num = this.m_entitiesManager.GetAllEntitiesOfType<Vehicle>((Predicate<Vehicle>) (x => (Mafi.Core.Prototypes.Proto) x.Prototype == (Mafi.Core.Prototypes.Proto) this.m_goalProto.ProtoToBuild)).Count<Vehicle>();
      if (this.m_currentCount != num)
      {
        this.m_currentCount = num;
        this.updateTitle();
      }
      return this.m_currentCount >= this.m_startingCount + this.m_goalProto.NumberToOwnSinceStart;
    }

    private void updateTitle()
    {
      int num = this.m_startingCount + this.m_goalProto.NumberToOwnSinceStart - this.m_currentCount;
      string str = num <= 1 ? string.Format("{0}", (object) this.m_goalProto.ProtoToBuild.Strings.Name) : string.Format("{0} ({1}x)", (object) this.m_goalProto.ProtoToBuild.Strings.Name, (object) num);
      this.Title = GoalToConstructVehicle.Proto.TITLE_CONSTRUCT.Format("<bc>" + str + "</bc>").Value + string.Format(" ({0} / {1})", (object) this.m_currentCount, (object) (this.m_startingCount + this.m_goalProto.NumberToOwnSinceStart));
    }

    protected override void UpdateTitleOnLoad() => this.updateTitle();

    static GoalToConstructVehicle()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      GoalToConstructVehicle.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Goal) obj).SerializeData(writer));
      GoalToConstructVehicle.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Goal) obj).DeserializeData(reader));
    }

    public class Proto : GoalProto
    {
      public static readonly LocStr1 TITLE_CONSTRUCT;
      public readonly DynamicGroundEntityProto ProtoToBuild;
      public readonly int NumberToOwnSinceStart;

      public override Type Implementation => typeof (GoalToConstructVehicle);

      public Proto(
        string id,
        DynamicGroundEntityProto protoToBuild,
        int numberToOwnSinceStart,
        Mafi.Core.Prototypes.Proto.ID? tutorial = null,
        int lockedByIndex = -1)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector(id, tutorial, lockedByIndex);
        this.ProtoToBuild = protoToBuild;
        this.NumberToOwnSinceStart = numberToOwnSinceStart;
      }

      static Proto()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        GoalToConstructVehicle.Proto.TITLE_CONSTRUCT = Loc.Str1("Goal__ConstructVehicle", "Construct new {0}", "text for a goal, example use: 'Construct new Excavator'");
      }
    }
  }
}
