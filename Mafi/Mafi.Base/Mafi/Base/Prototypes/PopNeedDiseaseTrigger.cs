// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.PopNeedDiseaseTrigger
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.Settlements;
using Mafi.Core.Population;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Base.Prototypes
{
  [GenerateSerializer(false, null, 0)]
  public class PopNeedDiseaseTrigger : IDiseaseTrigger
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly PopNeedDiseaseProto m_diseaseProto;
    private readonly SettlementsManager m_settlementsManager;
    private readonly UnlockedProtosDb m_unlockedProtosDb;
    private readonly ICalendar m_calendar;
    private int m_yearWhenNeedUnlocked;

    public static void Serialize(PopNeedDiseaseTrigger value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<PopNeedDiseaseTrigger>(value))
        return;
      writer.EnqueueDataSerialization((object) value, PopNeedDiseaseTrigger.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteGeneric<ICalendar>(this.m_calendar);
      writer.WriteGeneric<PopNeedDiseaseProto>(this.m_diseaseProto);
      SettlementsManager.Serialize(this.m_settlementsManager, writer);
      UnlockedProtosDb.Serialize(this.m_unlockedProtosDb, writer);
      writer.WriteInt(this.m_yearWhenNeedUnlocked);
    }

    public static PopNeedDiseaseTrigger Deserialize(BlobReader reader)
    {
      PopNeedDiseaseTrigger needDiseaseTrigger;
      if (reader.TryStartClassDeserialization<PopNeedDiseaseTrigger>(out needDiseaseTrigger))
        reader.EnqueueDataDeserialization((object) needDiseaseTrigger, PopNeedDiseaseTrigger.s_deserializeDataDelayedAction);
      return needDiseaseTrigger;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<PopNeedDiseaseTrigger>(this, "m_calendar", (object) reader.ReadGenericAs<ICalendar>());
      reader.SetField<PopNeedDiseaseTrigger>(this, "m_diseaseProto", (object) reader.ReadGenericAs<PopNeedDiseaseProto>());
      reader.SetField<PopNeedDiseaseTrigger>(this, "m_settlementsManager", (object) SettlementsManager.Deserialize(reader));
      reader.SetField<PopNeedDiseaseTrigger>(this, "m_unlockedProtosDb", (object) UnlockedProtosDb.Deserialize(reader));
      this.m_yearWhenNeedUnlocked = reader.ReadInt();
    }

    public PopNeedDiseaseTrigger(
      PopNeedDiseaseProto popNeedDiseaseProto,
      SettlementsManager settlementsManager,
      UnlockedProtosDb unlockedProtosDb,
      ICalendar calendar)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      this.m_yearWhenNeedUnlocked = -1;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_diseaseProto = popNeedDiseaseProto;
      this.m_settlementsManager = settlementsManager;
      this.m_unlockedProtosDb = unlockedProtosDb;
      this.m_calendar = calendar;
    }

    public Option<DiseaseProto> TryToTrigger(int monthSinceLastDisease, int totalPopulation)
    {
      if (!this.m_unlockedProtosDb.IsUnlocked((Proto) this.m_diseaseProto.Need))
        return Option<DiseaseProto>.None;
      if (this.m_yearWhenNeedUnlocked < 0)
        this.m_yearWhenNeedUnlocked = this.m_calendar.CurrentDate.Year;
      if (this.m_calendar.CurrentDate.Year <= this.m_yearWhenNeedUnlocked + 6 || monthSinceLastDisease < 42)
        return Option<DiseaseProto>.None;
      foreach (Settlement settlement in (IEnumerable<Settlement>) this.m_settlementsManager.Settlements)
      {
        if (settlement.Population > 600 && settlement.AllNeeds.FirstOrDefault((Func<PopNeed, bool>) (x => (Proto) x.Proto == (Proto) this.m_diseaseProto.Need)).PercentSatisfiedLastMonth < 10.Percent())
          return (Option<DiseaseProto>) (DiseaseProto) this.m_diseaseProto;
      }
      return Option<DiseaseProto>.None;
    }

    static PopNeedDiseaseTrigger()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      PopNeedDiseaseTrigger.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((PopNeedDiseaseTrigger) obj).SerializeData(writer));
      PopNeedDiseaseTrigger.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((PopNeedDiseaseTrigger) obj).DeserializeData(reader));
    }
  }
}
