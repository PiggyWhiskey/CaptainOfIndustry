// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.WellPumps.WellPump
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout.Upgrade;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Maintenance;
using Mafi.Core.Notifications;
using Mafi.Core.Population;
using Mafi.Core.Products;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Generation;
using Mafi.Core.Vehicles;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Linq;

#nullable disable
namespace Mafi.Core.Factory.WellPumps
{
  [GenerateSerializer(false, null, 0)]
  public sealed class WellPump : Machine, IVirtualResourceMiningEntity
  {
    private WellPumpProto m_proto;
    [DoNotSave(0, null)]
    private Percent m_speedWhenResourceDepleted;
    private readonly Option<IVirtualTerrainResource> m_resource;
    private EntityNotificator m_noResourceToMineNotificator;
    private EntityNotificatorWithProtoParam m_lowResourceNotificator;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    [DoNotSave(0, null)]
    public WellPumpProto Prototype
    {
      get => this.m_proto;
      protected set
      {
        this.m_proto = value;
        this.Prototype = (MachineProto) value;
      }
    }

    public string Description => this.Prototype.ReserveDescription;

    public ProductProto ProductToMine => this.Prototype.MinedProduct.Product;

    public Quantity CapacityOfMine
    {
      get
      {
        IVirtualTerrainResource valueOrNull = this.m_resource.ValueOrNull;
        return valueOrNull == null ? Quantity.Zero : valueOrNull.Capacity;
      }
    }

    public Quantity QuantityLeftToMine
    {
      get
      {
        IVirtualTerrainResource valueOrNull = this.m_resource.ValueOrNull;
        return valueOrNull == null ? Quantity.Zero : valueOrNull.Quantity;
      }
    }

    public bool NotifyOnLowReserve { get; set; }

    public WellPump(
      EntityId id,
      WellPumpProto wellPumpProto,
      TileTransform transform,
      EntityContext context,
      VirtualBuffersMap buffersMap,
      IVirtualResourceManager virtualResourceManager,
      INotificationsManager notifsManager,
      ILayoutEntityUpgraderFactory upgraderFactory,
      IUnityConsumerFactory unityConsumerFactory,
      UnlockedProtosDb unlockedProtosDb,
      IVehicleBuffersRegistry vehicleBuffersRegistry,
      IEntityMaintenanceProvidersFactory maintenanceProvidersFactory,
      IAnimationStateFactory animationStateFactory)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: reference to a compiler-generated field
      this.\u003CNotifyOnLowReserve\u003Ek__BackingField = true;
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (MachineProto) wellPumpProto, transform, context, buffersMap, upgraderFactory, unityConsumerFactory, unlockedProtosDb, vehicleBuffersRegistry, maintenanceProvidersFactory, animationStateFactory);
      this.Prototype = wellPumpProto;
      ImmutableArray<IVirtualTerrainResource> immutableArray = virtualResourceManager.RetrieveResourcesAt(this.Prototype.MinedProduct.Product, transform.Position.Tile2i);
      if (immutableArray.Length > 1)
        Log.Error("Multiple virtual resources not supported! Will pick the first one.");
      this.m_resource = Option.Create<IVirtualTerrainResource>(immutableArray.FirstOrDefault());
      this.updateProperties();
      this.m_noResourceToMineNotificator = notifsManager.CreateNotificatorFor(IdsCore.Notifications.NoResourceToExtract);
      this.m_lowResourceNotificator = notifsManager.CreateNotificatorFor(IdsCore.Notifications.ResourceIsLow);
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void initSelf() => this.updateProperties();

    private void updateProperties()
    {
      if (this.Prototype.MinedProduct.Product.Id == IdsCore.Products.CleanWater)
        this.m_speedWhenResourceDepleted = this.Context.PropertiesDb.GetValueAndRegisterForUpdates<Percent>((IEntity) this, IdsCore.PropertyIds.GroundWaterPumpSpeedWhenDepleted);
      else
        this.m_speedWhenResourceDepleted = Percent.Zero;
    }

    protected override void OnPropertiesChanged()
    {
      this.updateProperties();
      base.OnPropertiesChanged();
    }

    protected override void SimUpdateInternal()
    {
      base.SimUpdateInternal();
      this.m_noResourceToMineNotificator.NotifyIff(this.NotifyOnLowReserve && this.IsEnabled && this.m_resource.HasValue && this.m_resource.Value.Quantity.IsZero, (IEntity) this);
      this.m_lowResourceNotificator.NotifyIff((Proto) (this.m_resource.ValueOrNull?.Product.Product ?? this.Prototype.MinedProduct.Product), this.NotifyOnLowReserve && this.IsEnabled && this.m_resource.HasValue && !this.m_noResourceToMineNotificator.IsActive && this.m_resource.Value.PercentFull() < this.Prototype.NotifyWhenBelow, (IEntity) this);
    }

    public override void AddToConfig(EntityConfigData data)
    {
      base.AddToConfig(data);
      data.NotifyOnLowReserve = new bool?(this.NotifyOnLowReserve);
    }

    public override void ApplyConfig(EntityConfigData data)
    {
      base.ApplyConfig(data);
      this.NotifyOnLowReserve = ((int) data.NotifyOnLowReserve ?? (this.NotifyOnLowReserve ? 1 : 0)) != 0;
    }

    protected override Machine.State TryGetNewWork(out Percent utilization)
    {
      utilization = Percent.Zero;
      if (this.m_recipesFast.Count <= 0)
        return Machine.State.NoRecipes;
      if (this.m_resource.IsNone)
        return Machine.State.NotEnoughInput;
      Machine.RecipeWrapper first = this.m_recipesFast.First;
      if (!first.CanStoreToOutputs())
        return Machine.State.OutputFull;
      Machine.RecipeProductQuantity recipeProductQuantity = first.OutputsAtEnd.AsEnumerable().FirstOrDefault<Machine.RecipeProductQuantity>((Func<Machine.RecipeProductQuantity, bool>) (x => (Proto) x.Buffer.Product == (Proto) this.Prototype.MinedProduct.Product));
      ProductQuantity productQuantity = this.m_resource.Value.MineResourceAt(this.Transform.Position.Tile2i, recipeProductQuantity.Quantity);
      if (this.m_speedWhenResourceDepleted.IsPositive)
      {
        if (productQuantity.Quantity.IsZero)
        {
          this.SetBaseSpeedFactor(this.m_speedWhenResourceDepleted);
          if (!(this.m_resource.Value.EmergencyQuantity >= recipeProductQuantity.Quantity))
            return Machine.State.NotEnoughInput;
          productQuantity = this.m_resource.Value.Product.Product.WithQuantity(recipeProductQuantity.Quantity);
          this.m_resource.Value.EmergencyQuantity -= recipeProductQuantity.Quantity.AsPartial;
        }
        else
          this.SetBaseSpeedFactor(Percent.Hundred);
      }
      else if (productQuantity.Quantity.IsZero)
        return Machine.State.NotEnoughInput;
      this.Context.ProductsManager.ProductCreated(productQuantity, CreateReason.MinedFromTerrain);
      this.m_recipeResult.Clear();
      this.m_recipeResult.SetRecipe(first.Recipe, this.DurationMultiplier);
      this.m_recipeResult.ProducedOutputs.Add(productQuantity);
      this.m_recipeResult.Buffers.Add(recipeProductQuantity.Buffer);
      utilization = Percent.Hundred;
      return Machine.State.Working;
    }

    public override LocStrFormatted GetSlowDownMessageForUi()
    {
      LocStrFormatted downMessageForUi = base.GetSlowDownMessageForUi();
      return this.m_resource.Value.Quantity.IsZero && this.m_speedWhenResourceDepleted.IsPositive && downMessageForUi.IsNotEmpty ? new LocStrFormatted(downMessageForUi.Value + string.Format("\n- {0}", (object) TrCore.EntityStatus__ResourceDepleted)) : downMessageForUi;
    }

    public static void Serialize(WellPump value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<WellPump>(value))
        return;
      writer.EnqueueDataSerialization((object) value, WellPump.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityNotificatorWithProtoParam.Serialize(this.m_lowResourceNotificator, writer);
      EntityNotificator.Serialize(this.m_noResourceToMineNotificator, writer);
      writer.WriteGeneric<WellPumpProto>(this.m_proto);
      Option<IVirtualTerrainResource>.Serialize(this.m_resource, writer);
      writer.WriteBool(this.NotifyOnLowReserve);
    }

    public static WellPump Deserialize(BlobReader reader)
    {
      WellPump wellPump;
      if (reader.TryStartClassDeserialization<WellPump>(out wellPump))
        reader.EnqueueDataDeserialization((object) wellPump, WellPump.s_deserializeDataDelayedAction);
      return wellPump;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.m_lowResourceNotificator = EntityNotificatorWithProtoParam.Deserialize(reader);
      this.m_noResourceToMineNotificator = EntityNotificator.Deserialize(reader);
      this.m_proto = reader.ReadGenericAs<WellPumpProto>();
      reader.SetField<WellPump>(this, "m_resource", (object) Option<IVirtualTerrainResource>.Deserialize(reader));
      this.NotifyOnLowReserve = reader.ReadBool();
      reader.RegisterInitAfterLoad<WellPump>(this, "initSelf", InitPriority.Normal);
    }

    static WellPump()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      WellPump.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      WellPump.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }
  }
}
