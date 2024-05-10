// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.RainwaterHarvesters.RainwaterHarvester
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.Storages;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Environment;
using Mafi.Core.Products;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.Vehicles;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.RainwaterHarvesters
{
  [GenerateSerializer(false, null, 0)]
  public class RainwaterHarvester : StorageBase, IEntityWithSimUpdate, IEntity, IIsSafeAsHashKey
  {
    public readonly RainwaterHarvesterProto Prototype;
    private readonly RainHarvestingHelper m_harvestingHelper;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public override bool CanBePaused => true;

    public PartialQuantity WaterCollectedPerDayFullRain
    {
      get => this.m_harvestingHelper.WaterCollectedPerDayFullRain;
    }

    public Quantity StoredWater => this.Buffer.Value.Quantity;

    public override LogisticsControl LogisticsOutputControl => LogisticsControl.Enabled;

    protected override bool ReportFullStorageCapacityInStats => false;

    public RainwaterHarvester(
      EntityId id,
      RainwaterHarvesterProto proto,
      TileTransform transform,
      EntityContext context,
      ISimLoopEvents simLoopEvents,
      ICalendar calendar,
      IVehicleBuffersRegistry vehicleBuffersRegistry,
      IWeatherManager weatherManager,
      IPropertiesDb propertiesDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (StorageBaseProto) proto, transform, context, simLoopEvents, vehicleBuffersRegistry);
      this.Prototype = proto;
      Assert.That<Option<LogisticsBuffer>>(this.Buffer).IsNone<LogisticsBuffer>();
      Assert.That<bool>(this.TryAssignProduct((ProductProto) proto.WaterProto)).IsTrue("Failed to assign water product to harvester.");
      this.m_harvestingHelper = new RainHarvestingHelper(weatherManager, calendar, (IEntity) this, (IProductBuffer) this.Buffer.Value, this.Prototype.WaterCollectedPerDay, propertiesDb);
      this.m_harvestingHelper.IsEnabled = this.IsEnabled;
    }

    protected override LogisticsBuffer CreateNewBuffer(Quantity capacity, ProductProto product)
    {
      return (LogisticsBuffer) new GlobalLogisticsOutputBuffer(this.Prototype.Capacity, product, this.Context.ProductsManager, 15, (IEntity) this);
    }

    void IEntityWithSimUpdate.SimUpdate()
    {
      if (this.IsNotEnabled)
        return;
      this.SendAllOutputsIfCan();
    }

    public override bool IsProductSupported(ProductProto product)
    {
      return (Proto) product == (Proto) this.Prototype.WaterProto;
    }

    public void Cheat_MakeFull()
    {
      Assert.That<Option<LogisticsBuffer>>(this.Buffer).HasValue<LogisticsBuffer>();
      this.Context.ProductsManager.ProductCreated(this.Buffer.Value.Product, Quantity.MaxValue - this.Buffer.Value.StoreAsMuchAs(Quantity.MaxValue), CreateReason.Cheated);
    }

    protected override Option<IInputBufferPriorityProvider> GetVehicleInputBufferPriority(
      LogisticsBuffer buffer)
    {
      return (Option<IInputBufferPriorityProvider>) Option.None;
    }

    protected override Option<IOutputBufferPriorityProvider> GetVehicleOutputBufferPriority(
      LogisticsBuffer buffer)
    {
      return (Option<IOutputBufferPriorityProvider>) (IOutputBufferPriorityProvider) StaticPriorityProvider.LowestNoQuantityPreference;
    }

    protected override void OnEnabledChanged()
    {
      base.OnEnabledChanged();
      this.m_harvestingHelper.IsEnabled = this.IsEnabled;
    }

    public static void Serialize(RainwaterHarvester value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<RainwaterHarvester>(value))
        return;
      writer.EnqueueDataSerialization((object) value, RainwaterHarvester.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      RainHarvestingHelper.Serialize(this.m_harvestingHelper, writer);
      writer.WriteGeneric<RainwaterHarvesterProto>(this.Prototype);
    }

    public static RainwaterHarvester Deserialize(BlobReader reader)
    {
      RainwaterHarvester rainwaterHarvester;
      if (reader.TryStartClassDeserialization<RainwaterHarvester>(out rainwaterHarvester))
        reader.EnqueueDataDeserialization((object) rainwaterHarvester, RainwaterHarvester.s_deserializeDataDelayedAction);
      return rainwaterHarvester;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<RainwaterHarvester>(this, "m_harvestingHelper", (object) RainHarvestingHelper.Deserialize(reader));
      reader.SetField<RainwaterHarvester>(this, "Prototype", (object) reader.ReadGenericAs<RainwaterHarvesterProto>());
    }

    static RainwaterHarvester()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      RainwaterHarvester.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      RainwaterHarvester.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }
  }
}
