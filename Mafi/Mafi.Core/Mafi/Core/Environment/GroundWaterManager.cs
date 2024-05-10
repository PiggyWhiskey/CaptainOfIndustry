// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Environment.GroundWaterManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Notifications;
using Mafi.Core.Products;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Simulation;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Generation;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Environment
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  [GenerateSerializer(false, null, 0)]
  public class GroundWaterManager
  {
    private static readonly Percent CAPACITY_REPLENISH_PER_DAY;
    private static readonly Quantity MAX_EMERGENCY_WATER_CAPACITY;
    private readonly ICalendar m_calendar;
    private readonly IProductsManager m_productsManager;
    private readonly VirtualResourceManager m_resourceManager;
    private readonly VirtualResourceProductProto m_groundwater;
    private readonly IWeatherManager m_weatherManager;
    private readonly Notificator m_lowWaterNotif;
    [NewInSaveVersion(140, null, null, null, null)]
    private readonly IProperty<Percent> m_groundWaterReplenishWhenLow;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public GroundWaterManager(
      IWeatherManager weatherManager,
      ICalendar calendar,
      IProductsManager productsManager,
      IPropertiesDb propertiesDb,
      INotificationsManager notifManager,
      VirtualResourceManager resourceManager,
      [ProtoDep("Product_Virtual_Groundwater")] VirtualResourceProductProto groundwater)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_weatherManager = weatherManager;
      this.m_calendar = calendar;
      this.m_productsManager = productsManager;
      this.m_resourceManager = resourceManager;
      this.m_groundwater = groundwater;
      this.m_groundWaterReplenishWhenLow = propertiesDb.GetProperty<Percent>(IdsCore.PropertyIds.GroundWaterReplenishWhenLow);
      this.m_lowWaterNotif = notifManager.CreateNotificatorFor(IdsCore.Notifications.LowGroundwater);
      calendar.NewDay.Add<GroundWaterManager>(this, new Action(this.onNewDay));
    }

    [OnlyForSaveCompatibility(null)]
    [InitAfterLoad(InitPriority.Normal)]
    private void initSelf(int saveVersion, DependencyResolver resolver)
    {
      if (saveVersion >= 140)
        return;
      ReflectionUtils.SetField<GroundWaterManager>(this, "m_groundWaterReplenishWhenLow", (object) resolver.Resolve<IPropertiesDb>().GetProperty<Percent>(IdsCore.PropertyIds.GroundWaterReplenishWhenLow));
    }

    private void onNewDay()
    {
      if (this.m_weatherManager.RainIntensity.IsNotPositive)
      {
        Percent p = this.m_groundWaterReplenishWhenLow.Value;
        if (p.IsNotPositive)
          return;
        foreach (IVirtualTerrainResource virtualTerrainResource in this.m_resourceManager.GetAllResourcesFor(this.m_groundwater))
        {
          if (!virtualTerrainResource.Quantity.IsPositive && !(virtualTerrainResource.EmergencyQuantity.IntegerPart > GroundWaterManager.MAX_EMERGENCY_WATER_CAPACITY))
          {
            PartialQuantity partialQuantity = GroundWaterManager.CAPACITY_REPLENISH_PER_DAY.Apply(virtualTerrainResource.ConfiguredCapacity.Value).ScaledByToFix32(p).Quantity();
            virtualTerrainResource.EmergencyQuantity += partialQuantity;
          }
        }
      }
      else
      {
        foreach (IVirtualTerrainResource virtualTerrainResource in this.m_resourceManager.GetAllResourcesFor(this.m_groundwater))
        {
          Quantity quantity = GroundWaterManager.CAPACITY_REPLENISH_PER_DAY.Apply(virtualTerrainResource.ConfiguredCapacity.Value).ScaledByRounded(this.m_weatherManager.RainIntensity).Quantity();
          virtualTerrainResource.AddAsMuchAs(quantity);
        }
      }
    }

    public static void Serialize(GroundWaterManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<GroundWaterManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, GroundWaterManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteGeneric<ICalendar>(this.m_calendar);
      writer.WriteGeneric<VirtualResourceProductProto>(this.m_groundwater);
      writer.WriteGeneric<IProperty<Percent>>(this.m_groundWaterReplenishWhenLow);
      Notificator.Serialize(this.m_lowWaterNotif, writer);
      writer.WriteGeneric<IProductsManager>(this.m_productsManager);
      VirtualResourceManager.Serialize(this.m_resourceManager, writer);
      writer.WriteGeneric<IWeatherManager>(this.m_weatherManager);
    }

    public static GroundWaterManager Deserialize(BlobReader reader)
    {
      GroundWaterManager groundWaterManager;
      if (reader.TryStartClassDeserialization<GroundWaterManager>(out groundWaterManager))
        reader.EnqueueDataDeserialization((object) groundWaterManager, GroundWaterManager.s_deserializeDataDelayedAction);
      return groundWaterManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<GroundWaterManager>(this, "m_calendar", (object) reader.ReadGenericAs<ICalendar>());
      reader.SetField<GroundWaterManager>(this, "m_groundwater", (object) reader.ReadGenericAs<VirtualResourceProductProto>());
      reader.SetField<GroundWaterManager>(this, "m_groundWaterReplenishWhenLow", reader.LoadedSaveVersion >= 140 ? (object) reader.ReadGenericAs<IProperty<Percent>>() : (object) (IProperty<Percent>) null);
      reader.SetField<GroundWaterManager>(this, "m_lowWaterNotif", (object) Notificator.Deserialize(reader));
      reader.SetField<GroundWaterManager>(this, "m_productsManager", (object) reader.ReadGenericAs<IProductsManager>());
      reader.SetField<GroundWaterManager>(this, "m_resourceManager", (object) VirtualResourceManager.Deserialize(reader));
      reader.SetField<GroundWaterManager>(this, "m_weatherManager", (object) reader.ReadGenericAs<IWeatherManager>());
      reader.RegisterInitAfterLoad<GroundWaterManager>(this, "initSelf", InitPriority.Normal);
    }

    static GroundWaterManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      GroundWaterManager.CAPACITY_REPLENISH_PER_DAY = 0.185.Percent();
      GroundWaterManager.MAX_EMERGENCY_WATER_CAPACITY = 200.Quantity();
      GroundWaterManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((GroundWaterManager) obj).SerializeData(writer));
      GroundWaterManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((GroundWaterManager) obj).DeserializeData(reader));
    }
  }
}
