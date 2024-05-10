// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.RainwaterHarvesters.RainHarvestingHelper
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Environment;
using Mafi.Core.Products;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Simulation;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.RainwaterHarvesters
{
  [GenerateSerializer(false, null, 0)]
  public class RainHarvestingHelper : IEntityObserver
  {
    private readonly IWeatherManager m_weatherManager;
    private readonly IProductsManager m_productsManager;
    private readonly ICalendar m_calendar;
    private readonly IProductBuffer m_waterBuffer;
    private readonly PartialQuantity m_waterCollectedPerDay;
    private readonly IProperty<Percent> m_rainYieldMultiplier;
    private PartialQuantity m_waterPartialBuffer;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public bool IsEnabled { get; set; }

    public PartialQuantity WaterCollectedPerDayFullRain
    {
      get => this.m_waterCollectedPerDay.ScaledBy(this.m_rainYieldMultiplier.Value);
    }

    public RainHarvestingHelper(
      IWeatherManager weatherManager,
      ICalendar calendar,
      IEntity entity,
      IProductBuffer waterBuffer,
      PartialQuantity waterCollectedPerDay,
      IPropertiesDb propertiesDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_weatherManager = weatherManager;
      this.m_productsManager = entity.Context.ProductsManager;
      this.m_calendar = calendar;
      this.m_waterBuffer = waterBuffer;
      this.m_waterCollectedPerDay = waterCollectedPerDay;
      this.m_rainYieldMultiplier = propertiesDb.GetProperty<Percent>(IdsCore.PropertyIds.RainYieldMultiplier);
      calendar.NewDay.Add<RainHarvestingHelper>(this, new Action(this.onNewDay));
      entity.AddObserver((IEntityObserver) this);
      this.IsEnabled = true;
    }

    private void onNewDay()
    {
      if (!this.IsEnabled)
        return;
      Percent rainIntensity = this.m_weatherManager.RainIntensity;
      if (rainIntensity.IsNotPositive)
        return;
      this.m_waterPartialBuffer += this.WaterCollectedPerDayFullRain.ScaledBy(rainIntensity);
      Quantity integerPart = this.m_waterPartialBuffer.IntegerPart;
      this.m_waterPartialBuffer = this.m_waterPartialBuffer.FractionalPart;
      Quantity quantity1 = this.m_waterBuffer.StoreAsMuchAs(integerPart);
      Quantity quantity2 = integerPart - quantity1;
      if (!quantity2.IsPositive)
        return;
      this.m_productsManager.ProductCreated(this.m_waterBuffer.Product, quantity2, CreateReason.Produced);
    }

    void IEntityObserver.OnEntityDestroy(IEntity entity)
    {
      entity.RemoveObserver((IEntityObserver) this);
      this.m_calendar.NewDay.Remove<RainHarvestingHelper>(this, new Action(this.onNewDay));
    }

    public static void Serialize(RainHarvestingHelper value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<RainHarvestingHelper>(value))
        return;
      writer.EnqueueDataSerialization((object) value, RainHarvestingHelper.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteBool(this.IsEnabled);
      writer.WriteGeneric<ICalendar>(this.m_calendar);
      writer.WriteGeneric<IProductsManager>(this.m_productsManager);
      writer.WriteGeneric<IProperty<Percent>>(this.m_rainYieldMultiplier);
      writer.WriteGeneric<IProductBuffer>(this.m_waterBuffer);
      PartialQuantity.Serialize(this.m_waterCollectedPerDay, writer);
      PartialQuantity.Serialize(this.m_waterPartialBuffer, writer);
      writer.WriteGeneric<IWeatherManager>(this.m_weatherManager);
    }

    public static RainHarvestingHelper Deserialize(BlobReader reader)
    {
      RainHarvestingHelper harvestingHelper;
      if (reader.TryStartClassDeserialization<RainHarvestingHelper>(out harvestingHelper))
        reader.EnqueueDataDeserialization((object) harvestingHelper, RainHarvestingHelper.s_deserializeDataDelayedAction);
      return harvestingHelper;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.IsEnabled = reader.ReadBool();
      reader.SetField<RainHarvestingHelper>(this, "m_calendar", (object) reader.ReadGenericAs<ICalendar>());
      reader.SetField<RainHarvestingHelper>(this, "m_productsManager", (object) reader.ReadGenericAs<IProductsManager>());
      reader.SetField<RainHarvestingHelper>(this, "m_rainYieldMultiplier", (object) reader.ReadGenericAs<IProperty<Percent>>());
      reader.SetField<RainHarvestingHelper>(this, "m_waterBuffer", (object) reader.ReadGenericAs<IProductBuffer>());
      reader.SetField<RainHarvestingHelper>(this, "m_waterCollectedPerDay", (object) PartialQuantity.Deserialize(reader));
      this.m_waterPartialBuffer = PartialQuantity.Deserialize(reader);
      reader.SetField<RainHarvestingHelper>(this, "m_weatherManager", (object) reader.ReadGenericAs<IWeatherManager>());
    }

    static RainHarvestingHelper()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      RainHarvestingHelper.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((RainHarvestingHelper) obj).SerializeData(writer));
      RainHarvestingHelper.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((RainHarvestingHelper) obj).DeserializeData(reader));
    }
  }
}
