// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.TrashDiseaseTrigger
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.Settlements;
using Mafi.Core.Population;
using Mafi.Core.Simulation;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Base.Prototypes
{
  [GenerateSerializer(false, null, 0)]
  public class TrashDiseaseTrigger : IDiseaseTrigger
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly TrashDiseaseProto m_diseaseProto;
    private readonly SettlementsManager m_settlementsManager;
    private readonly ICalendar m_calendar;

    public static void Serialize(TrashDiseaseTrigger value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TrashDiseaseTrigger>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TrashDiseaseTrigger.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteGeneric<ICalendar>(this.m_calendar);
      writer.WriteGeneric<TrashDiseaseProto>(this.m_diseaseProto);
      SettlementsManager.Serialize(this.m_settlementsManager, writer);
    }

    public static TrashDiseaseTrigger Deserialize(BlobReader reader)
    {
      TrashDiseaseTrigger trashDiseaseTrigger;
      if (reader.TryStartClassDeserialization<TrashDiseaseTrigger>(out trashDiseaseTrigger))
        reader.EnqueueDataDeserialization((object) trashDiseaseTrigger, TrashDiseaseTrigger.s_deserializeDataDelayedAction);
      return trashDiseaseTrigger;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<TrashDiseaseTrigger>(this, "m_calendar", (object) reader.ReadGenericAs<ICalendar>());
      reader.SetField<TrashDiseaseTrigger>(this, "m_diseaseProto", (object) reader.ReadGenericAs<TrashDiseaseProto>());
      reader.SetField<TrashDiseaseTrigger>(this, "m_settlementsManager", (object) SettlementsManager.Deserialize(reader));
    }

    public TrashDiseaseTrigger(
      TrashDiseaseProto popNeedDiseaseProto,
      SettlementsManager settlementsManager,
      ICalendar calendar)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_diseaseProto = popNeedDiseaseProto;
      this.m_settlementsManager = settlementsManager;
      this.m_calendar = calendar;
    }

    public Option<DiseaseProto> TryToTrigger(int monthSinceLastDisease, int totalPopulation)
    {
      if (this.m_calendar.CurrentDate.Year <= 12 || monthSinceLastDisease < 18 || totalPopulation < 300)
        return Option<DiseaseProto>.None;
      foreach (Settlement settlement in (IEnumerable<Settlement>) this.m_settlementsManager.Settlements)
      {
        if (settlement.LandfillInSettlement.IntegerPart > settlement.LandfillLimitForCurrentPopulation.ScaledBy(120.Percent()))
          return (Option<DiseaseProto>) (DiseaseProto) this.m_diseaseProto;
      }
      return Option<DiseaseProto>.None;
    }

    static TrashDiseaseTrigger()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      TrashDiseaseTrigger.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((TrashDiseaseTrigger) obj).SerializeData(writer));
      TrashDiseaseTrigger.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((TrashDiseaseTrigger) obj).DeserializeData(reader));
    }
  }
}
