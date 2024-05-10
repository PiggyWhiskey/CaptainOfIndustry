// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Offices.CaptainOfficeManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Simulation;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Offices
{
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class CaptainOfficeManager
  {
    private bool m_wasActive;
    private readonly Event m_onOfficeActiveChanged;
    private readonly IProperty<Percent> m_tradeVolumeMultiplier;
    private Option<PropertyModifier<Percent>> m_tradeVolumeModifier;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public Option<Mafi.Core.Buildings.Offices.CaptainOffice> CaptainOffice { get; private set; }

    public bool OfficeBuilt => this.CaptainOffice.HasValue;

    public bool IsOfficeActive => this.CaptainOffice.HasValue && this.CaptainOffice.Value.IsActive;

    public IEvent OnOfficeActiveChanged => (IEvent) this.m_onOfficeActiveChanged;

    public CaptainOfficeManager(
      ConstructionManager constructionManager,
      EntitiesManager entitiesManager,
      ISimLoopEvents simLoopEvents,
      IPropertiesDb propertiesDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_onOfficeActiveChanged = new Event();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_tradeVolumeMultiplier = propertiesDb.GetProperty<Percent>(IdsCore.PropertyIds.TradeVolumeMultiplier);
      constructionManager.EntityConstructed.Add<CaptainOfficeManager>(this, new Action<IStaticEntity>(this.entityConstructed));
      constructionManager.EntityStartedDeconstruction.Add<CaptainOfficeManager>(this, new Action<IStaticEntity>(this.entityDeconstructionStarted));
      entitiesManager.OnUpgradeJustPerformed.Add<CaptainOfficeManager>(this, new Action<IUpgradableEntity, IEntityProto>(this.onUpgradePerformed));
      simLoopEvents.Update.Add<CaptainOfficeManager>(this, new Action(this.simUpdate));
    }

    private void simUpdate()
    {
      if (this.m_wasActive == this.IsOfficeActive)
        return;
      this.m_wasActive = this.IsOfficeActive;
      this.m_onOfficeActiveChanged.Invoke();
    }

    private void entityConstructed(IEntity entity)
    {
      if (!(entity is Mafi.Core.Buildings.Offices.CaptainOffice captainOffice))
        return;
      Assert.That<Option<Mafi.Core.Buildings.Offices.CaptainOffice>>(this.CaptainOffice).IsNone<Mafi.Core.Buildings.Offices.CaptainOffice>();
      this.CaptainOffice = (Option<Mafi.Core.Buildings.Offices.CaptainOffice>) captainOffice;
      this.addModifierIfCan(captainOffice.Prototype);
    }

    private void entityDeconstructionStarted(IEntity entity)
    {
      if (!(entity is Mafi.Core.Buildings.Offices.CaptainOffice captainOffice))
        return;
      Assert.That<Mafi.Core.Buildings.Offices.CaptainOffice>(captainOffice).IsEqualTo<Mafi.Core.Buildings.Offices.CaptainOffice>(this.CaptainOffice.ValueOrNull);
      this.CaptainOffice = (Option<Mafi.Core.Buildings.Offices.CaptainOffice>) Option.None;
      this.removeModifierIfCan();
    }

    private void onUpgradePerformed(IUpgradableEntity entity, IEntityProto oldProto)
    {
      if (!(entity is Mafi.Core.Buildings.Offices.CaptainOffice captainOffice))
        return;
      if (this.CaptainOffice.ValueOrNull != captainOffice)
      {
        Log.Error("Invalid upgrade journey for captain office!");
      }
      else
      {
        this.removeModifierIfCan();
        this.addModifierIfCan(captainOffice.Prototype);
      }
    }

    private void removeModifierIfCan()
    {
      if (!this.m_tradeVolumeModifier.HasValue)
        return;
      this.m_tradeVolumeMultiplier.RemoveModifier(this.m_tradeVolumeModifier.Value);
      this.m_tradeVolumeModifier = (Option<PropertyModifier<Percent>>) Option.None;
    }

    private void addModifierIfCan(CaptainOfficeProto office)
    {
      if (!office.TradeVolumeDiff.IsPositive)
        return;
      this.m_tradeVolumeModifier = (Option<PropertyModifier<Percent>>) PropertyModifiers.Delta(office.TradeVolumeDiff, office.Id.Value, PropertyModifiers.NO_GROUP);
      this.m_tradeVolumeMultiplier.AddModifier(this.m_tradeVolumeModifier.Value);
    }

    public static void Serialize(CaptainOfficeManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<CaptainOfficeManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, CaptainOfficeManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Option<Mafi.Core.Buildings.Offices.CaptainOffice>.Serialize(this.CaptainOffice, writer);
      Event.Serialize(this.m_onOfficeActiveChanged, writer);
      Option<PropertyModifier<Percent>>.Serialize(this.m_tradeVolumeModifier, writer);
      writer.WriteGeneric<IProperty<Percent>>(this.m_tradeVolumeMultiplier);
      writer.WriteBool(this.m_wasActive);
    }

    public static CaptainOfficeManager Deserialize(BlobReader reader)
    {
      CaptainOfficeManager captainOfficeManager;
      if (reader.TryStartClassDeserialization<CaptainOfficeManager>(out captainOfficeManager))
        reader.EnqueueDataDeserialization((object) captainOfficeManager, CaptainOfficeManager.s_deserializeDataDelayedAction);
      return captainOfficeManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.CaptainOffice = Option<Mafi.Core.Buildings.Offices.CaptainOffice>.Deserialize(reader);
      reader.SetField<CaptainOfficeManager>(this, "m_onOfficeActiveChanged", (object) Event.Deserialize(reader));
      this.m_tradeVolumeModifier = Option<PropertyModifier<Percent>>.Deserialize(reader);
      reader.SetField<CaptainOfficeManager>(this, "m_tradeVolumeMultiplier", (object) reader.ReadGenericAs<IProperty<Percent>>());
      this.m_wasActive = reader.ReadBool();
    }

    static CaptainOfficeManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      CaptainOfficeManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((CaptainOfficeManager) obj).SerializeData(writer));
      CaptainOfficeManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((CaptainOfficeManager) obj).DeserializeData(reader));
    }
  }
}
