// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Population.Edicts.FoodConsumptionEdict
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
  public class FoodConsumptionEdict : Edict
  {
    private static string MODIFIER_GROUP;
    public readonly FoodConsumptionEdictProto Prototype;
    private readonly SettlementsManager m_settlementsManager;
    private readonly IProperty<Percent> m_foodConsumptionMultiplier;
    private readonly PropertyModifier<Percent> m_foodConsumptionModifier;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public FoodConsumptionEdict(
      FoodConsumptionEdictProto edictProto,
      UpointsManager upointsManager,
      SettlementsManager settlementsManager,
      CaptainOfficeManager captainOfficeManager,
      IPropertiesDb propsDb,
      ICalendar calendar)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector((EdictProto) edictProto, upointsManager, calendar, captainOfficeManager);
      this.Prototype = edictProto;
      this.m_settlementsManager = settlementsManager;
      this.m_foodConsumptionMultiplier = propsDb.GetProperty<Percent>(IdsCore.PropertyIds.FoodConsumptionMultiplier);
      this.m_foodConsumptionModifier = PropertyModifiers.Delta(this.Prototype.ConsumptionDiff, this.Prototype.Id.Value, (Option<string>) FoodConsumptionEdict.MODIFIER_GROUP);
    }

    protected override void OnActiveChanged(bool isActiveNow)
    {
      if (isActiveNow)
        this.m_foodConsumptionMultiplier.AddModifier(this.m_foodConsumptionModifier);
      else
        this.m_foodConsumptionMultiplier.RemoveModifier(this.m_foodConsumptionModifier);
    }

    protected override bool CanReactivateForNewMonth(out string reasonForNotActive)
    {
      if (this.Prototype.ConsumptionDiff.IsNegative)
      {
        reasonForNotActive = "";
        return true;
      }
      reasonForNotActive = this.m_settlementsManager.ArePeopleStarving ? "Population is starving" : "";
      return !this.m_settlementsManager.ArePeopleStarving;
    }

    protected override Edict.EdictEnableCheckResult CanBeEnabledInternal()
    {
      if (this.Prototype.ConsumptionDiff.IsNegative)
        return new Edict.EdictEnableCheckResult()
        {
          CanBeEnabled = true,
          Explanation = ""
        };
      return new Edict.EdictEnableCheckResult()
      {
        CanBeEnabled = !this.m_settlementsManager.ArePeopleStarving,
        Explanation = this.m_settlementsManager.ArePeopleStarving ? "Population is starving" : ""
      };
    }

    public static void Serialize(FoodConsumptionEdict value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<FoodConsumptionEdict>(value))
        return;
      writer.EnqueueDataSerialization((object) value, FoodConsumptionEdict.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      PropertyModifier<Percent>.Serialize(this.m_foodConsumptionModifier, writer);
      writer.WriteGeneric<IProperty<Percent>>(this.m_foodConsumptionMultiplier);
      SettlementsManager.Serialize(this.m_settlementsManager, writer);
      writer.WriteGeneric<FoodConsumptionEdictProto>(this.Prototype);
    }

    public static FoodConsumptionEdict Deserialize(BlobReader reader)
    {
      FoodConsumptionEdict consumptionEdict;
      if (reader.TryStartClassDeserialization<FoodConsumptionEdict>(out consumptionEdict))
        reader.EnqueueDataDeserialization((object) consumptionEdict, FoodConsumptionEdict.s_deserializeDataDelayedAction);
      return consumptionEdict;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<FoodConsumptionEdict>(this, "m_foodConsumptionModifier", (object) PropertyModifier<Percent>.Deserialize(reader));
      reader.SetField<FoodConsumptionEdict>(this, "m_foodConsumptionMultiplier", (object) reader.ReadGenericAs<IProperty<Percent>>());
      reader.SetField<FoodConsumptionEdict>(this, "m_settlementsManager", (object) SettlementsManager.Deserialize(reader));
      reader.SetField<FoodConsumptionEdict>(this, "Prototype", (object) reader.ReadGenericAs<FoodConsumptionEdictProto>());
    }

    static FoodConsumptionEdict()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      FoodConsumptionEdict.MODIFIER_GROUP = nameof (FoodConsumptionEdict);
      FoodConsumptionEdict.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Edict) obj).SerializeData(writer));
      FoodConsumptionEdict.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Edict) obj).DeserializeData(reader));
    }
  }
}
