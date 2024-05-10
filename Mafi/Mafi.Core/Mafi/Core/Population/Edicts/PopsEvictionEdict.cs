// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Population.Edicts.PopsEvictionEdict
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.Offices;
using Mafi.Core.Simulation;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Population.Edicts
{
  [GenerateSerializer(false, null, 0)]
  public class PopsEvictionEdict : Edict
  {
    public readonly PopsEvictionEdictProto Prototype;
    private readonly PopsHealthManager m_popsHealthManager;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public PopsEvictionEdict(
      PopsEvictionEdictProto edictProto,
      UpointsManager upointsManager,
      PopsHealthManager popsHealthManager,
      CaptainOfficeManager captainOfficeManager,
      ICalendar calendar)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector((EdictProto) edictProto, upointsManager, calendar, captainOfficeManager);
      this.Prototype = edictProto;
      this.m_popsHealthManager = popsHealthManager;
    }

    protected override void OnNewMonthInternal()
    {
      base.OnNewMonthInternal();
      if (!this.IsActive)
        return;
      this.m_popsHealthManager.AddBirthDecrease(IdsCore.BirthRateCategories.Edicts, this.Prototype.MonthlyPopsEvictionRate, new Percent?(this.Prototype.MonthlyPopsEvictionRate));
    }

    protected override bool CanReactivateForNewMonth(out string reasonForNotActive)
    {
      reasonForNotActive = "";
      return true;
    }

    protected override Edict.EdictEnableCheckResult CanBeEnabledInternal()
    {
      return new Edict.EdictEnableCheckResult()
      {
        CanBeEnabled = true,
        Explanation = ""
      };
    }

    public static void Serialize(PopsEvictionEdict value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<PopsEvictionEdict>(value))
        return;
      writer.EnqueueDataSerialization((object) value, PopsEvictionEdict.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      PopsHealthManager.Serialize(this.m_popsHealthManager, writer);
      writer.WriteGeneric<PopsEvictionEdictProto>(this.Prototype);
    }

    public static PopsEvictionEdict Deserialize(BlobReader reader)
    {
      PopsEvictionEdict popsEvictionEdict;
      if (reader.TryStartClassDeserialization<PopsEvictionEdict>(out popsEvictionEdict))
        reader.EnqueueDataDeserialization((object) popsEvictionEdict, PopsEvictionEdict.s_deserializeDataDelayedAction);
      return popsEvictionEdict;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<PopsEvictionEdict>(this, "m_popsHealthManager", (object) PopsHealthManager.Deserialize(reader));
      reader.SetField<PopsEvictionEdict>(this, "Prototype", (object) reader.ReadGenericAs<PopsEvictionEdictProto>());
    }

    static PopsEvictionEdict()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      PopsEvictionEdict.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Edict) obj).SerializeData(writer));
      PopsEvictionEdict.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Edict) obj).DeserializeData(reader));
    }
  }
}
