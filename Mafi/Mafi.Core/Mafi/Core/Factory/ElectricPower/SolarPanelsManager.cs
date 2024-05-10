// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.ElectricPower.SolarPanelsManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Entities;
using Mafi.Core.Environment;
using Mafi.Core.PropertiesDb;
using Mafi.Serialization;
using System;
using System.Linq;

#nullable disable
namespace Mafi.Core.Factory.ElectricPower
{
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public sealed class SolarPanelsManager : ISolarPanelsManager
  {
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Dict<ISolarPanelEntity, Electricity> m_entityToPowerMap;
    private LystStruct<SolarPanelsManager.GroupedGenerators> m_groupedGenerators;
    private readonly IProperty<Percent> m_powerMultiplier;
    private readonly ElectricityManager m_electricityManager;
    private readonly IEntitiesManager m_entitiesManager;
    private readonly IWeatherManager m_weatherManager;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public SolarPanelsManager(
      ElectricityManager electricityManager,
      IEntitiesManager entitiesManager,
      IWeatherManager weatherManager,
      IPropertiesDb propertiesDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_entityToPowerMap = new Dict<ISolarPanelEntity, Electricity>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_electricityManager = electricityManager;
      this.m_entitiesManager = entitiesManager;
      this.m_weatherManager = weatherManager;
      entitiesManager.EntityEnabledChanged.Add<SolarPanelsManager>(this, new Action<IEntity, bool>(this.enabledChanged));
      entitiesManager.OnUpgradeJustPerformed.Add<SolarPanelsManager>(this, new Action<IUpgradableEntity, IEntityProto>(this.onUpgradeDone));
      entitiesManager.EntityRemoved.Add<SolarPanelsManager>(this, new Action<IEntity>(this.entityRemoved));
      this.m_powerMultiplier = propertiesDb.GetProperty<Percent>(IdsCore.PropertyIds.SolarPowerMultiplier);
    }

    [InitAfterLoad(InitPriority.Low)]
    private void initSelf()
    {
      foreach (SolarPanelsManager.GroupedGenerators groupedGenerator in this.m_groupedGenerators)
        groupedGenerator.ClearAllPanels();
      foreach (ISolarPanelEntity entity in this.m_entitiesManager.GetAllEntitiesOfType<ISolarPanelEntity>().Where<ISolarPanelEntity>((Func<ISolarPanelEntity, bool>) (x => x.IsEnabled)))
        this.addActivePowerSourceIfNotPresent(entity);
    }

    public Electricity GetMaxPowerOutputFor(ISolarPanelEntity solarPanel)
    {
      return this.scalePower(solarPanel.MaxOutputElectricity);
    }

    public Electricity GetCurrentPowerOutputFor(ISolarPanelEntity solarPanel)
    {
      return this.scalePowerIncludeWeather(solarPanel.MaxOutputElectricity);
    }

    /// <summary>
    /// Applies difficulty, edicts multipliers and current weather.
    /// </summary>
    private Electricity scalePowerIncludeWeather(Electricity input)
    {
      return this.scalePower(input).ScaledBy(this.m_weatherManager.SimSunIntensity);
    }

    /// <summary>Applies difficulty and edicts multipliers.</summary>
    private Electricity scalePower(Electricity input)
    {
      return input.ScaledBy(this.m_powerMultiplier.Value);
    }

    private void enabledChanged(IEntity entity, bool isEnabled)
    {
      if (!(entity is ISolarPanelEntity entity1))
        return;
      if (isEnabled)
      {
        Assert.That<bool>(entity1.IsConstructed).IsTrue();
        this.addActivePowerSourceIfNotPresent(entity1);
      }
      else
        this.removeActivePowerSourceIfPresent(entity1, (IEntityProto) entity1.Prototype);
    }

    private void onUpgradeDone(IUpgradableEntity entity, IEntityProto previousProto)
    {
      if (!(entity is ISolarPanelEntity entity1))
        return;
      this.removeActivePowerSourceIfPresent(entity1, previousProto);
      this.addActivePowerSourceIfNotPresent(entity1);
    }

    private void entityRemoved(IEntity entity)
    {
      if (!(entity is ISolarPanelEntity solarPanelEntity) || !this.m_entityToPowerMap.ContainsKey(solarPanelEntity))
        return;
      Log.Error(string.Format("Panel was removed {0} whilst enabled? That shouldn't happen.", (object) solarPanelEntity));
      this.removeActivePowerSourceIfPresent(solarPanelEntity, (IEntityProto) solarPanelEntity.Prototype);
    }

    private void addActivePowerSourceIfNotPresent(ISolarPanelEntity entity)
    {
      if (!this.m_entityToPowerMap.TryAdd(entity, entity.MaxOutputElectricity))
        return;
      this.getOrCreateGeneratorFor((IEntityProto) entity.Prototype).AddPanel(entity.MaxOutputElectricity);
    }

    private SolarPanelsManager.GroupedGenerators getOrCreateGeneratorFor(IEntityProto proto)
    {
      foreach (SolarPanelsManager.GroupedGenerators groupedGenerator in this.m_groupedGenerators)
      {
        if (groupedGenerator.Prototype == proto)
          return groupedGenerator;
      }
      SolarPanelsManager.GroupedGenerators generator = new SolarPanelsManager.GroupedGenerators(proto, this);
      this.m_electricityManager.CreateAndRegisterFor((IElectricityGeneratingEntityGrouped) generator, 0, true);
      this.m_groupedGenerators.Add(generator);
      return generator;
    }

    private void removeActivePowerSourceIfPresent(ISolarPanelEntity entity, IEntityProto proto)
    {
      Electricity electricity;
      if (!this.m_entityToPowerMap.TryRemove(entity, out electricity))
        return;
      this.getOrCreateGeneratorFor(proto).RemovePanel(electricity);
    }

    public static void Serialize(SolarPanelsManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SolarPanelsManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SolarPanelsManager.s_serializeDataDelayedAction);
    }

    private void SerializeData(BlobWriter writer)
    {
      ElectricityManager.Serialize(this.m_electricityManager, writer);
      writer.WriteGeneric<IEntitiesManager>(this.m_entitiesManager);
      LystStruct<SolarPanelsManager.GroupedGenerators>.Serialize(this.m_groupedGenerators, writer);
      writer.WriteGeneric<IProperty<Percent>>(this.m_powerMultiplier);
      writer.WriteGeneric<IWeatherManager>(this.m_weatherManager);
    }

    public static SolarPanelsManager Deserialize(BlobReader reader)
    {
      SolarPanelsManager solarPanelsManager;
      if (reader.TryStartClassDeserialization<SolarPanelsManager>(out solarPanelsManager))
        reader.EnqueueDataDeserialization((object) solarPanelsManager, SolarPanelsManager.s_deserializeDataDelayedAction);
      return solarPanelsManager;
    }

    private void DeserializeData(BlobReader reader)
    {
      reader.SetField<SolarPanelsManager>(this, "m_electricityManager", (object) ElectricityManager.Deserialize(reader));
      reader.SetField<SolarPanelsManager>(this, "m_entitiesManager", (object) reader.ReadGenericAs<IEntitiesManager>());
      reader.SetField<SolarPanelsManager>(this, "m_entityToPowerMap", (object) new Dict<ISolarPanelEntity, Electricity>());
      this.m_groupedGenerators = LystStruct<SolarPanelsManager.GroupedGenerators>.Deserialize(reader);
      reader.SetField<SolarPanelsManager>(this, "m_powerMultiplier", (object) reader.ReadGenericAs<IProperty<Percent>>());
      reader.SetField<SolarPanelsManager>(this, "m_weatherManager", (object) reader.ReadGenericAs<IWeatherManager>());
      reader.RegisterInitAfterLoad<SolarPanelsManager>(this, "initSelf", InitPriority.Low);
    }

    static SolarPanelsManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      SolarPanelsManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((SolarPanelsManager) obj).SerializeData(writer));
      SolarPanelsManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((SolarPanelsManager) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    private sealed class GroupedGenerators : 
      IElectricityGeneratingEntityGrouped,
      IElectricityGenerator
    {
      private readonly SolarPanelsManager m_solarManager;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public IEntityProto Prototype { get; private set; }

      public int GeneratorsTotal { get; private set; }

      public Electricity OutputElectricity { get; private set; }

      public Electricity MaxGenerationCapacity
      {
        get => this.m_solarManager.scalePower(this.OutputElectricity);
      }

      public GroupedGenerators(IEntityProto prototype, SolarPanelsManager solarManager)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.Prototype = prototype;
        this.m_solarManager = solarManager;
      }

      public Electricity GetCurrentMaxGeneration(out bool canGenerate)
      {
        canGenerate = true;
        return this.m_solarManager.scalePowerIncludeWeather(this.OutputElectricity);
      }

      public Electricity GenerateAsMuchAs(
        Electricity freeCapacity,
        Electricity currentMaxGeneration)
      {
        return currentMaxGeneration;
      }

      public void AddPanel(Electricity electricity)
      {
        ++this.GeneratorsTotal;
        this.OutputElectricity += electricity;
      }

      public void RemovePanel(Electricity electricity)
      {
        --this.GeneratorsTotal;
        this.OutputElectricity -= electricity;
        Assert.That<int>(this.GeneratorsTotal).IsNotNegative();
      }

      public void ClearAllPanels()
      {
        this.GeneratorsTotal = 0;
        this.OutputElectricity = Electricity.Zero;
      }

      public static void Serialize(SolarPanelsManager.GroupedGenerators value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<SolarPanelsManager.GroupedGenerators>(value))
          return;
        writer.EnqueueDataSerialization((object) value, SolarPanelsManager.GroupedGenerators.s_serializeDataDelayedAction);
      }

      private void SerializeData(BlobWriter writer)
      {
        writer.WriteInt(this.GeneratorsTotal);
        SolarPanelsManager.Serialize(this.m_solarManager, writer);
        Electricity.Serialize(this.OutputElectricity, writer);
        writer.WriteGeneric<IEntityProto>(this.Prototype);
      }

      public static SolarPanelsManager.GroupedGenerators Deserialize(BlobReader reader)
      {
        SolarPanelsManager.GroupedGenerators groupedGenerators;
        if (reader.TryStartClassDeserialization<SolarPanelsManager.GroupedGenerators>(out groupedGenerators))
          reader.EnqueueDataDeserialization((object) groupedGenerators, SolarPanelsManager.GroupedGenerators.s_deserializeDataDelayedAction);
        return groupedGenerators;
      }

      private void DeserializeData(BlobReader reader)
      {
        this.GeneratorsTotal = reader.ReadInt();
        reader.SetField<SolarPanelsManager.GroupedGenerators>(this, "m_solarManager", (object) SolarPanelsManager.Deserialize(reader));
        this.OutputElectricity = Electricity.Deserialize(reader);
        this.Prototype = reader.ReadGenericAs<IEntityProto>();
      }

      static GroupedGenerators()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        SolarPanelsManager.GroupedGenerators.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((SolarPanelsManager.GroupedGenerators) obj).SerializeData(writer));
        SolarPanelsManager.GroupedGenerators.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((SolarPanelsManager.GroupedGenerators) obj).DeserializeData(reader));
      }
    }
  }
}
