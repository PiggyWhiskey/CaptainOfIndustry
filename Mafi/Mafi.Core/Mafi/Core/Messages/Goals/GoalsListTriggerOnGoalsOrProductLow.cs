// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.Goals.GoalsListTriggerOnGoalsOrProductLow
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Products;
using Mafi.Core.Simulation;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Messages.Goals
{
  /// <summary>
  /// Trigger a goal list when some other goal list was just done or quantity of some product got low.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public class GoalsListTriggerOnGoalsOrProductLow : GoalListTrigger
  {
    private readonly Lyst<Mafi.Core.Prototypes.Proto.ID> m_goalsIdsLeftToSatisfy;
    private readonly GoalsListTriggerOnGoalsOrProductLow.TriggerRule m_rule;
    private readonly ProductStats m_productStats;
    private readonly Quantity m_quantityToTrigger;
    private readonly ICalendar m_calendar;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public GoalsListTriggerOnGoalsOrProductLow(
      GoalsListTriggerOnGoalsOrProductLow.Data triggerData,
      GoalsList goalListToActivate,
      GoalsManager goalsManager,
      IProductsManager productsManager,
      ICalendar calendar)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(goalListToActivate, goalsManager, triggerData.Version);
      this.m_calendar = calendar;
      this.m_goalsIdsLeftToSatisfy = triggerData.GoalListProtosIds.ToLyst();
      this.m_rule = triggerData.Rule;
      this.m_quantityToTrigger = triggerData.ProductQuantityToTrigger.Quantity;
      this.m_productStats = productsManager.GetStatsFor(triggerData.ProductQuantityToTrigger.Product);
      goalsManager.OnGoalFinished.Add<GoalsListTriggerOnGoalsOrProductLow>(this, new Action<GoalsList>(this.onGoalFinished));
      this.m_calendar.NewDayEnd.Add<GoalsListTriggerOnGoalsOrProductLow>(this, new Action(this.onNewDay));
    }

    private void onNewDay()
    {
      if (this.m_calendar.CurrentDate.RelGameDate.TotalMonthsFloored <= 1 || !(this.m_productStats.StoredAvailableQuantity < (QuantityLarge) this.m_quantityToTrigger))
        return;
      this.ActivateGoal();
    }

    private void onGoalFinished(GoalsList finishedGoal)
    {
      if (!this.m_goalsIdsLeftToSatisfy.Remove(finishedGoal.Prototype.Id) || !this.m_goalsIdsLeftToSatisfy.IsEmpty && this.m_rule != GoalsListTriggerOnGoalsOrProductLow.TriggerRule.AnySatisfied)
        return;
      this.ActivateGoal();
    }

    protected override void OnDestroy()
    {
      this.GoalsManager.OnGoalFinished.Remove<GoalsListTriggerOnGoalsOrProductLow>(this, new Action<GoalsList>(this.onGoalFinished));
      this.m_calendar.NewDayEnd.Remove<GoalsListTriggerOnGoalsOrProductLow>(this, new Action(this.onNewDay));
    }

    public static void Serialize(GoalsListTriggerOnGoalsOrProductLow value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<GoalsListTriggerOnGoalsOrProductLow>(value))
        return;
      writer.EnqueueDataSerialization((object) value, GoalsListTriggerOnGoalsOrProductLow.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteGeneric<ICalendar>(this.m_calendar);
      Lyst<Mafi.Core.Prototypes.Proto.ID>.Serialize(this.m_goalsIdsLeftToSatisfy, writer);
      ProductStats.Serialize(this.m_productStats, writer);
      Quantity.Serialize(this.m_quantityToTrigger, writer);
      writer.WriteInt((int) this.m_rule);
    }

    public static GoalsListTriggerOnGoalsOrProductLow Deserialize(BlobReader reader)
    {
      GoalsListTriggerOnGoalsOrProductLow goalsOrProductLow;
      if (reader.TryStartClassDeserialization<GoalsListTriggerOnGoalsOrProductLow>(out goalsOrProductLow))
        reader.EnqueueDataDeserialization((object) goalsOrProductLow, GoalsListTriggerOnGoalsOrProductLow.s_deserializeDataDelayedAction);
      return goalsOrProductLow;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<GoalsListTriggerOnGoalsOrProductLow>(this, "m_calendar", (object) reader.ReadGenericAs<ICalendar>());
      reader.SetField<GoalsListTriggerOnGoalsOrProductLow>(this, "m_goalsIdsLeftToSatisfy", (object) Lyst<Mafi.Core.Prototypes.Proto.ID>.Deserialize(reader));
      reader.SetField<GoalsListTriggerOnGoalsOrProductLow>(this, "m_productStats", (object) ProductStats.Deserialize(reader));
      reader.SetField<GoalsListTriggerOnGoalsOrProductLow>(this, "m_quantityToTrigger", (object) Quantity.Deserialize(reader));
      reader.SetField<GoalsListTriggerOnGoalsOrProductLow>(this, "m_rule", (object) (GoalsListTriggerOnGoalsOrProductLow.TriggerRule) reader.ReadInt());
    }

    static GoalsListTriggerOnGoalsOrProductLow()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      GoalsListTriggerOnGoalsOrProductLow.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((GoalListTrigger) obj).SerializeData(writer));
      GoalsListTriggerOnGoalsOrProductLow.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((GoalListTrigger) obj).DeserializeData(reader));
    }

    public enum TriggerRule
    {
      AllSatisfied,
      AnySatisfied,
    }

    public class Data : IGoalListTriggerData
    {
      public readonly ImmutableArray<Mafi.Core.Prototypes.Proto.ID> GoalListProtosIds;
      public readonly GoalsListTriggerOnGoalsOrProductLow.TriggerRule Rule;
      public readonly ProductQuantity ProductQuantityToTrigger;

      public Type Implementation => typeof (GoalsListTriggerOnGoalsOrProductLow);

      public int Version { get; }

      public Data(
        Mafi.Core.Prototypes.Proto.ID goalListIdProtoDone,
        ProductQuantity productQuantityToTrigger,
        int version = 0)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.GoalListProtosIds = ImmutableArray.Create<Mafi.Core.Prototypes.Proto.ID>(goalListIdProtoDone);
        this.Rule = GoalsListTriggerOnGoalsOrProductLow.TriggerRule.AnySatisfied;
        this.ProductQuantityToTrigger = productQuantityToTrigger;
        this.Version = version;
      }

      public Data(
        ImmutableArray<Mafi.Core.Prototypes.Proto.ID> goalListProtoIds,
        GoalsListTriggerOnGoalsOrProductLow.TriggerRule rule,
        ProductQuantity productQuantityToTrigger)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.GoalListProtosIds = goalListProtoIds;
        this.Rule = rule;
        this.ProductQuantityToTrigger = productQuantityToTrigger;
      }
    }
  }
}
