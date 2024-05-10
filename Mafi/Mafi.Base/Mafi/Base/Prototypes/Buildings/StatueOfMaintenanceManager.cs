// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Buildings.StatueOfMaintenanceManager
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Entities.Static;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Simulation;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Base.Prototypes.Buildings
{
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class StatueOfMaintenanceManager
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public static readonly Percent MAINTENANCE_BONUS_BASE;
    private readonly Lyst<Statue> m_statues;
    private Option<PropertyModifier<Percent>> m_modifier;
    private readonly IProperty<Percent> m_maintenanceProp;

    public static void Serialize(StatueOfMaintenanceManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<StatueOfMaintenanceManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, StatueOfMaintenanceManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Percent.Serialize(this.GlobalMaintenanceBonus, writer);
      writer.WriteGeneric<IProperty<Percent>>(this.m_maintenanceProp);
      Option<PropertyModifier<Percent>>.Serialize(this.m_modifier, writer);
      Lyst<Statue>.Serialize(this.m_statues, writer);
    }

    public static StatueOfMaintenanceManager Deserialize(BlobReader reader)
    {
      StatueOfMaintenanceManager maintenanceManager;
      if (reader.TryStartClassDeserialization<StatueOfMaintenanceManager>(out maintenanceManager))
        reader.EnqueueDataDeserialization((object) maintenanceManager, StatueOfMaintenanceManager.s_deserializeDataDelayedAction);
      return maintenanceManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.GlobalMaintenanceBonus = Percent.Deserialize(reader);
      reader.SetField<StatueOfMaintenanceManager>(this, "m_maintenanceProp", (object) reader.ReadGenericAs<IProperty<Percent>>());
      this.m_modifier = Option<PropertyModifier<Percent>>.Deserialize(reader);
      reader.SetField<StatueOfMaintenanceManager>(this, "m_statues", (object) Lyst<Statue>.Deserialize(reader));
    }

    public Percent GlobalMaintenanceBonus { get; private set; }

    public StatueOfMaintenanceManager(
      ICalendar calendar,
      IConstructionManager constructionManager,
      IPropertiesDb propertiesDb)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      this.m_statues = new Lyst<Statue>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_maintenanceProp = propertiesDb.GetProperty<Percent>(IdsCore.PropertyIds.MaintenanceConsumptionMultiplier);
      constructionManager.EntityConstructed.Add<StatueOfMaintenanceManager>(this, new Action<IStaticEntity>(this.entityConstructed));
      constructionManager.EntityStartedDeconstruction.Add<StatueOfMaintenanceManager>(this, new Action<IStaticEntity>(this.entityDeconstructionStarted));
      calendar.NewDay.Add<StatueOfMaintenanceManager>(this, new Action(this.onNewDay));
    }

    private void onNewDay()
    {
      int n1 = 0;
      int n2 = 0;
      foreach (Statue statue in this.m_statues)
      {
        Assert.That<bool>(statue.IsDestroyed).IsFalse();
        if (statue.IsActive)
          ++n1;
        if (statue.Maintenance.Status.CurrentBreakdownChance > 1.Percent())
          ++n2;
      }
      Percent percent1 = MafiMath.GeometricSum(StatueOfMaintenanceManager.MAINTENANCE_BONUS_BASE, Percent.Fifty, n1);
      Percent percent2 = MafiMath.GeometricSum(StatueOfMaintenanceManager.MAINTENANCE_BONUS_BASE, Percent.Fifty, n2) - percent1;
      if (percent2 == this.GlobalMaintenanceBonus)
        return;
      this.GlobalMaintenanceBonus = percent2;
      if (this.m_modifier.HasValue)
      {
        this.m_maintenanceProp.RemoveModifier(this.m_modifier.Value);
        this.m_modifier = (Option<PropertyModifier<Percent>>) Option.None;
      }
      this.m_modifier = (Option<PropertyModifier<Percent>>) PropertyModifiers.Delta(percent2, "StatueOfMaintenanceManagerBonus", PropertyModifiers.NO_GROUP);
      this.m_maintenanceProp.AddModifier(this.m_modifier.Value);
    }

    private void entityConstructed(IStaticEntity entity)
    {
      if (!(entity is Statue statue) || !(statue.Prototype.Manager == this.GetType()))
        return;
      Assert.That<bool>(this.m_statues.AddIfNotPresent(statue)).IsTrue();
    }

    private void entityDeconstructionStarted(IStaticEntity entity)
    {
      if (!(entity is Statue statue) || !(statue.Prototype.Manager == this.GetType()))
        return;
      this.m_statues.RemoveAndAssert(statue);
    }

    static StatueOfMaintenanceManager()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      StatueOfMaintenanceManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((StatueOfMaintenanceManager) obj).SerializeData(writer));
      StatueOfMaintenanceManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((StatueOfMaintenanceManager) obj).DeserializeData(reader));
      StatueOfMaintenanceManager.MAINTENANCE_BONUS_BASE = 4.Percent();
    }
  }
}
