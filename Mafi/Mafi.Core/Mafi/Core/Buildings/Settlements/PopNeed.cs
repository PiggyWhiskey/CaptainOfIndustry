// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Settlements.PopNeed
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Population;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Stats;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Settlements
{
  [GenerateSerializer(false, null, 0)]
  public class PopNeed
  {
    public readonly Lyst<ISettlementServiceModule> ModulesProvidingTheNeed;
    public int LastUsedModuleIndex;
    public readonly Lyst<DailyUpointsRecord> DailyRecords;
    public Percent PercentSatisfiedAfterLastUpdate;
    private readonly Option<IProperty<Percent>> m_consumptionProp;
    private readonly Option<IProperty<Percent>> m_unityProp;
    public readonly Fix32AvgStats CoverageStats;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public bool IsFoodNeed => this.Proto.IsFoodNeed;

    public bool IsHealthcareNeed => this.Proto.IsHealthcareNeed;

    public bool ShouldBeShown { get; set; }

    public PopNeedProto Proto { get; private set; }

    public Upoints UnityAfterLastUpdate { get; set; }

    public Upoints PossibleMaxAfterLastUpdate { get; set; }

    public bool WasNotFullySatisfiedLastDay { get; set; }

    public Percent PercentSatisfiedLastMonth { get; private set; }

    public Percent NeedIncreaseFromHousing { get; set; }

    public Percent ConsumptionMultiplier
    {
      get
      {
        return !this.m_consumptionProp.HasValue ? Percent.Hundred + this.NeedIncreaseFromHousing : this.m_consumptionProp.Value.Value.Apply(Percent.Hundred + this.NeedIncreaseFromHousing);
      }
    }

    private Percent UnityMultiplier
    {
      get
      {
        IProperty<Percent> valueOrNull = this.m_unityProp.ValueOrNull;
        return valueOrNull == null ? Percent.Hundred : valueOrNull.Value;
      }
    }

    public PopNeed(PopNeedProto proto, IPropertiesDb propsDb, StatsManager statsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.ModulesProvidingTheNeed = new Lyst<ISettlementServiceModule>();
      this.DailyRecords = new Lyst<DailyUpointsRecord>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Proto = proto;
      this.CoverageStats = new Fix32AvgStats((Option<StatsManager>) statsManager, true);
      this.m_consumptionProp = proto.ConsumptionMultiplierProperty.HasValue ? propsDb.GetProperty<Percent>(proto.ConsumptionMultiplierProperty.Value).SomeOption<IProperty<Percent>>() : (Option<IProperty<Percent>>) Option.None;
      this.m_unityProp = proto.UnityMultiplierProperty.HasValue ? propsDb.GetProperty<Percent>(proto.UnityMultiplierProperty.Value).SomeOption<IProperty<Percent>>() : (Option<IProperty<Percent>>) Option.None;
      this.ShouldBeShown = this.IsFoodNeed;
    }

    public Upoints GetUnityMultiplied(Percent globalMultiplier)
    {
      return this.Proto.Unity.ScaledBy(this.UnityMultiplier).ScaledBy(globalMultiplier);
    }

    public void SetPercentSatisfiedLastMonth(Percent value)
    {
      this.PercentSatisfiedLastMonth = value;
      this.CoverageStats.Set(value.ToFix32());
    }

    public void MergeIn(PopNeed other, Settlement newSettlement)
    {
      if ((Mafi.Core.Prototypes.Proto) this.Proto != (Mafi.Core.Prototypes.Proto) other.Proto)
      {
        Log.Error("Merging incorrect need");
      }
      else
      {
        foreach (ISettlementServiceModule settlementServiceModule in other.ModulesProvidingTheNeed)
        {
          settlementServiceModule.ReplaceSettlement(newSettlement);
          this.ModulesProvidingTheNeed.AddAssertNew(settlementServiceModule);
        }
        other.ModulesProvidingTheNeed.Clear();
        this.UnityAfterLastUpdate += other.UnityAfterLastUpdate;
        this.PossibleMaxAfterLastUpdate += other.UnityAfterLastUpdate;
        this.WasNotFullySatisfiedLastDay |= other.WasNotFullySatisfiedLastDay;
      }
    }

    public static void Serialize(PopNeed value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<PopNeed>(value))
        return;
      writer.EnqueueDataSerialization((object) value, PopNeed.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Fix32AvgStats.Serialize(this.CoverageStats, writer);
      Lyst<DailyUpointsRecord>.Serialize(this.DailyRecords, writer);
      writer.WriteInt(this.LastUsedModuleIndex);
      Option<IProperty<Percent>>.Serialize(this.m_consumptionProp, writer);
      Option<IProperty<Percent>>.Serialize(this.m_unityProp, writer);
      Lyst<ISettlementServiceModule>.Serialize(this.ModulesProvidingTheNeed, writer);
      Percent.Serialize(this.NeedIncreaseFromHousing, writer);
      Percent.Serialize(this.PercentSatisfiedAfterLastUpdate, writer);
      Percent.Serialize(this.PercentSatisfiedLastMonth, writer);
      Upoints.Serialize(this.PossibleMaxAfterLastUpdate, writer);
      writer.WriteGeneric<PopNeedProto>(this.Proto);
      writer.WriteBool(this.ShouldBeShown);
      Upoints.Serialize(this.UnityAfterLastUpdate, writer);
      writer.WriteBool(this.WasNotFullySatisfiedLastDay);
    }

    public static PopNeed Deserialize(BlobReader reader)
    {
      PopNeed popNeed;
      if (reader.TryStartClassDeserialization<PopNeed>(out popNeed))
        reader.EnqueueDataDeserialization((object) popNeed, PopNeed.s_deserializeDataDelayedAction);
      return popNeed;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<PopNeed>(this, "CoverageStats", (object) Fix32AvgStats.Deserialize(reader));
      reader.SetField<PopNeed>(this, "DailyRecords", (object) Lyst<DailyUpointsRecord>.Deserialize(reader));
      this.LastUsedModuleIndex = reader.ReadInt();
      reader.SetField<PopNeed>(this, "m_consumptionProp", (object) Option<IProperty<Percent>>.Deserialize(reader));
      reader.SetField<PopNeed>(this, "m_unityProp", (object) Option<IProperty<Percent>>.Deserialize(reader));
      reader.SetField<PopNeed>(this, "ModulesProvidingTheNeed", (object) Lyst<ISettlementServiceModule>.Deserialize(reader));
      this.NeedIncreaseFromHousing = Percent.Deserialize(reader);
      this.PercentSatisfiedAfterLastUpdate = Percent.Deserialize(reader);
      this.PercentSatisfiedLastMonth = Percent.Deserialize(reader);
      this.PossibleMaxAfterLastUpdate = Upoints.Deserialize(reader);
      this.Proto = reader.ReadGenericAs<PopNeedProto>();
      this.ShouldBeShown = reader.ReadBool();
      this.UnityAfterLastUpdate = Upoints.Deserialize(reader);
      this.WasNotFullySatisfiedLastDay = reader.ReadBool();
    }

    static PopNeed()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      PopNeed.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((PopNeed) obj).SerializeData(writer));
      PopNeed.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((PopNeed) obj).DeserializeData(reader));
    }
  }
}
