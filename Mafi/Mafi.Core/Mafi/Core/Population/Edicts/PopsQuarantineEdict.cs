// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Population.Edicts.PopsQuarantineEdict
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.Offices;
using Mafi.Core.Buildings.Settlements;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Simulation;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Population.Edicts
{
  [GenerateSerializer(false, null, 0)]
  public class PopsQuarantineEdict : Edict
  {
    private static string MODIFIER_GROUP;
    public readonly PopsQuarantineEdictProto Prototype;
    private readonly SettlementsManager m_settlementsManager;
    private readonly WorkersManager m_workersManager;
    private readonly PopsHealthManager m_popsHealthManager;
    private readonly IProperty<Percent> m_diseaseEffectMultiplier;
    private readonly PropertyModifier<Percent> m_diffModifier;
    private int m_workersWithheld;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public PopsQuarantineEdict(
      PopsQuarantineEdictProto edictProto,
      UpointsManager upointsManager,
      SettlementsManager settlementsManager,
      WorkersManager workersManager,
      PopsHealthManager popsHealthManager,
      CaptainOfficeManager captainOfficeManager,
      IPropertiesDb propsDb,
      ICalendar calendar)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector((EdictProto) edictProto, upointsManager, calendar, captainOfficeManager);
      this.Prototype = edictProto;
      this.m_settlementsManager = settlementsManager;
      this.m_workersManager = workersManager;
      this.m_popsHealthManager = popsHealthManager;
      this.m_diseaseEffectMultiplier = propsDb.GetProperty<Percent>(IdsCore.PropertyIds.DiseaseEffectsMultiplier);
      this.m_diffModifier = PropertyModifiers.Delta(-edictProto.DiseaseReduction, nameof (PopsQuarantineEdict), (Option<string>) PopsQuarantineEdict.MODIFIER_GROUP);
    }

    protected override void OnActiveChanged(bool isActiveNow)
    {
      if (isActiveNow)
      {
        this.m_workersWithheld = this.m_settlementsManager.GetTotalPopulationWithoutHomeless().ScaledByRounded(this.Prototype.WorkersToWithhold);
        this.m_workersManager.WithholdWorkers(this.m_workersWithheld);
        this.m_diseaseEffectMultiplier.AddModifier(this.m_diffModifier);
      }
      else
      {
        if (this.m_workersWithheld > 0)
          this.m_workersManager.ReturnWithheldWorkers(this.m_workersWithheld);
        this.m_diseaseEffectMultiplier.RemoveModifier(this.m_diffModifier);
      }
      base.OnActiveChanged(isActiveNow);
    }

    protected override bool CanReactivateForNewMonth(out string reasonForNotActive)
    {
      bool hasValue = this.m_popsHealthManager.CurrentDisease.HasValue;
      reasonForNotActive = hasValue ? "" : "No disease active";
      return hasValue;
    }

    protected override Edict.EdictEnableCheckResult CanBeEnabledInternal()
    {
      return new Edict.EdictEnableCheckResult()
      {
        CanBeEnabled = true,
        Explanation = ""
      };
    }

    public static void Serialize(PopsQuarantineEdict value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<PopsQuarantineEdict>(value))
        return;
      writer.EnqueueDataSerialization((object) value, PopsQuarantineEdict.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      PropertyModifier<Percent>.Serialize(this.m_diffModifier, writer);
      writer.WriteGeneric<IProperty<Percent>>(this.m_diseaseEffectMultiplier);
      PopsHealthManager.Serialize(this.m_popsHealthManager, writer);
      SettlementsManager.Serialize(this.m_settlementsManager, writer);
      WorkersManager.Serialize(this.m_workersManager, writer);
      writer.WriteInt(this.m_workersWithheld);
      writer.WriteGeneric<PopsQuarantineEdictProto>(this.Prototype);
    }

    public static PopsQuarantineEdict Deserialize(BlobReader reader)
    {
      PopsQuarantineEdict popsQuarantineEdict;
      if (reader.TryStartClassDeserialization<PopsQuarantineEdict>(out popsQuarantineEdict))
        reader.EnqueueDataDeserialization((object) popsQuarantineEdict, PopsQuarantineEdict.s_deserializeDataDelayedAction);
      return popsQuarantineEdict;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<PopsQuarantineEdict>(this, "m_diffModifier", (object) PropertyModifier<Percent>.Deserialize(reader));
      reader.SetField<PopsQuarantineEdict>(this, "m_diseaseEffectMultiplier", (object) reader.ReadGenericAs<IProperty<Percent>>());
      reader.SetField<PopsQuarantineEdict>(this, "m_popsHealthManager", (object) PopsHealthManager.Deserialize(reader));
      reader.SetField<PopsQuarantineEdict>(this, "m_settlementsManager", (object) SettlementsManager.Deserialize(reader));
      reader.SetField<PopsQuarantineEdict>(this, "m_workersManager", (object) WorkersManager.Deserialize(reader));
      this.m_workersWithheld = reader.ReadInt();
      reader.SetField<PopsQuarantineEdict>(this, "Prototype", (object) reader.ReadGenericAs<PopsQuarantineEdictProto>());
    }

    static PopsQuarantineEdict()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      PopsQuarantineEdict.MODIFIER_GROUP = nameof (PopsQuarantineEdict);
      PopsQuarantineEdict.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Edict) obj).SerializeData(writer));
      PopsQuarantineEdict.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Edict) obj).DeserializeData(reader));
    }
  }
}
