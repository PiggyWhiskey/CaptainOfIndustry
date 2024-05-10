// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Population.Edicts.PopsBoostEdict
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.Offices;
using Mafi.Core.Buildings.Settlements;
using Mafi.Core.Simulation;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Population.Edicts
{
  [GenerateSerializer(false, null, 0)]
  public class PopsBoostEdict : Edict
  {
    public readonly PopsBoostEdictProto Prototype;
    private static readonly Percent MIN_REQUIRED_HEALTH;
    private readonly SettlementsManager m_settlementsManager;
    private readonly PopsHealthManager m_popsHealthManager;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public PopsBoostEdict(
      PopsBoostEdictProto edictProto,
      UpointsManager upointsManager,
      SettlementsManager settlementsManager,
      PopsHealthManager popsHealthManager,
      CaptainOfficeManager captainOfficeManager,
      ICalendar calendar)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector((EdictProto) edictProto, upointsManager, calendar, captainOfficeManager);
      this.Prototype = edictProto;
      this.m_settlementsManager = settlementsManager;
      this.m_popsHealthManager = popsHealthManager;
    }

    protected override void OnNewMonthInternal()
    {
      base.OnNewMonthInternal();
      if (!this.IsActive)
        return;
      if (this.m_settlementsManager.ArePeopleStarving)
        this.m_popsHealthManager.AddBirthIncrease(IdsCore.BirthRateCategories.Edicts, Percent.Zero, new Percent?(this.Prototype.PopsGrowthBoost));
      else
        this.m_popsHealthManager.AddBirthIncrease(IdsCore.BirthRateCategories.Edicts, this.Prototype.PopsGrowthBoost);
    }

    protected override bool CanReactivateForNewMonth(out string reasonForNotActive)
    {
      bool flag = this.m_popsHealthManager.HealthStats.HealthLastMonth >= PopsBoostEdict.MIN_REQUIRED_HEALTH;
      if (this.m_settlementsManager.FreeHousingCapacity <= 0)
      {
        reasonForNotActive = TrCore.EdictReason__HousingFull.TranslatedString;
        return false;
      }
      if (!flag)
      {
        reasonForNotActive = TrCore.EdictReason__HealthLow.Format(this.m_popsHealthManager.HealthStats.HealthLastMonth.ToIntPercentRounded().ToStringCached(), PopsBoostEdict.MIN_REQUIRED_HEALTH.ToIntPercentRounded().ToStringCached()).Value;
        return false;
      }
      reasonForNotActive = "";
      return true;
    }

    protected override Edict.EdictEnableCheckResult CanBeEnabledInternal()
    {
      bool flag = this.m_popsHealthManager.HealthStats.HealthLastMonth >= PopsBoostEdict.MIN_REQUIRED_HEALTH;
      return new Edict.EdictEnableCheckResult()
      {
        CanBeEnabled = flag,
        Explanation = flag ? "" : TrCore.EdictReason__HealthLow.Format(this.m_popsHealthManager.HealthStats.HealthLastMonth.ToIntPercentRounded().ToStringCached(), PopsBoostEdict.MIN_REQUIRED_HEALTH.ToIntPercentRounded().ToStringCached()).Value
      };
    }

    public static void Serialize(PopsBoostEdict value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<PopsBoostEdict>(value))
        return;
      writer.EnqueueDataSerialization((object) value, PopsBoostEdict.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      PopsHealthManager.Serialize(this.m_popsHealthManager, writer);
      SettlementsManager.Serialize(this.m_settlementsManager, writer);
      writer.WriteGeneric<PopsBoostEdictProto>(this.Prototype);
    }

    public static PopsBoostEdict Deserialize(BlobReader reader)
    {
      PopsBoostEdict popsBoostEdict;
      if (reader.TryStartClassDeserialization<PopsBoostEdict>(out popsBoostEdict))
        reader.EnqueueDataDeserialization((object) popsBoostEdict, PopsBoostEdict.s_deserializeDataDelayedAction);
      return popsBoostEdict;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<PopsBoostEdict>(this, "m_popsHealthManager", (object) PopsHealthManager.Deserialize(reader));
      reader.SetField<PopsBoostEdict>(this, "m_settlementsManager", (object) SettlementsManager.Deserialize(reader));
      reader.SetField<PopsBoostEdict>(this, "Prototype", (object) reader.ReadGenericAs<PopsBoostEdictProto>());
    }

    static PopsBoostEdict()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      PopsBoostEdict.MIN_REQUIRED_HEALTH = 1.Percent();
      PopsBoostEdict.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Edict) obj).SerializeData(writer));
      PopsBoostEdict.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Edict) obj).DeserializeData(reader));
    }
  }
}
