// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Buildings.Statue
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Entities.Static.Layout.Upgrade;
using Mafi.Core.Maintenance;
using Mafi.Core.Ports;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Utils;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Base.Prototypes.Buildings
{
  [GenerateSerializer(false, null, 0)]
  public class Statue : 
    LayoutEntity,
    IUpgradableEntity,
    IStaticEntity,
    IEntityWithPosition,
    IRenderedEntity,
    IEntity,
    IIsSafeAsHashKey,
    IAreaSelectableEntity,
    IMaintainedEntity,
    IEntityWithGeneralPriority,
    IEntityWithSimUpdate,
    IEntityWithParticles,
    IEntityWithPorts
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly IProductsManager m_productsManager;
    private static readonly Quantity BUFFER_CAPACITY;
    private StatueProto m_proto;
    private Option<ProductBuffer> m_buffer;
    private readonly TickTimer m_durationLeft;

    public static void Serialize(Statue value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<Statue>(value))
        return;
      writer.EnqueueDataSerialization((object) value, Statue.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      Option<ProductBuffer>.Serialize(this.m_buffer, writer);
      TickTimer.Serialize(this.m_durationLeft, writer);
      writer.WriteGeneric<IProductsManager>(this.m_productsManager);
      writer.WriteGeneric<StatueProto>(this.m_proto);
      writer.WriteGeneric<IEntityMaintenanceProvider>(this.Maintenance);
      writer.WriteGeneric<IUpgrader>(this.Upgrader);
    }

    public static Statue Deserialize(BlobReader reader)
    {
      Statue statue;
      if (reader.TryStartClassDeserialization<Statue>(out statue))
        reader.EnqueueDataDeserialization((object) statue, Statue.s_deserializeDataDelayedAction);
      return statue;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.m_buffer = Option<ProductBuffer>.Deserialize(reader);
      reader.SetField<Statue>(this, "m_durationLeft", (object) TickTimer.Deserialize(reader));
      reader.SetField<Statue>(this, "m_productsManager", (object) reader.ReadGenericAs<IProductsManager>());
      this.m_proto = reader.ReadGenericAs<StatueProto>();
      this.Maintenance = reader.ReadGenericAs<IEntityMaintenanceProvider>();
      this.Upgrader = reader.ReadGenericAs<IUpgrader>();
    }

    public override bool CanBePaused => false;

    [DoNotSave(0, null)]
    public StatueProto Prototype
    {
      get => this.m_proto;
      protected set
      {
        this.m_proto = value;
        this.Prototype = (LayoutEntityProto) value;
      }
    }

    public IUpgrader Upgrader { get; private set; }

    MaintenanceCosts IMaintainedEntity.MaintenanceCosts => this.Prototype.Costs.Maintenance;

    public IEntityMaintenanceProvider Maintenance { get; private set; }

    protected override bool IsEnabledNow => base.IsEnabledNow && this.Maintenance.CanWork();

    public bool IsIdleForMaintenance => false;

    public bool IsActive => this.m_durationLeft.IsNotFinished;

    public bool AreParticlesEnabled => this.m_durationLeft.IsNotFinished;

    public Option<IProductBuffer> InputBuffer
    {
      get => (Option<IProductBuffer>) (IProductBuffer) this.m_buffer.ValueOrNull;
    }

    public Statue(
      EntityId id,
      StatueProto proto,
      TileTransform transform,
      EntityContext context,
      IProductsManager productsManager,
      ILayoutEntityUpgraderFactory upgraderFactory,
      IEntityMaintenanceProvidersFactory maintenanceProvidersFactory)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      this.m_durationLeft = new TickTimer();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (LayoutEntityProto) proto, transform, context);
      this.m_productsManager = productsManager;
      this.Prototype = proto;
      this.Upgrader = upgraderFactory.CreateInstance<StatueProto, Statue>(this, this.Prototype);
      this.Maintenance = maintenanceProvidersFactory.CreateFor((IMaintainedEntity) this);
      if (!this.Prototype.InputProduct.HasValue)
        return;
      this.m_buffer = (Option<ProductBuffer>) new ProductBuffer(Statue.BUFFER_CAPACITY, this.Prototype.InputProduct.Value);
    }

    public void SimUpdate()
    {
      if (this.m_buffer.IsNone)
        return;
      if (!this.IsEnabled)
      {
        this.m_durationLeft.Reset();
      }
      else
      {
        if (this.m_durationLeft.Decrement())
          return;
        ProductBuffer buffer = this.m_buffer.Value;
        if (!buffer.CanRemove(1.Quantity()))
          return;
        buffer.RemoveExactly(1.Quantity());
        this.m_productsManager.ProductDestroyed(buffer.Product, 1.Quantity(), DestroyReason.General);
        this.m_durationLeft.Start(this.Prototype.DurationPerOneQuantity);
      }
    }

    public bool IsUpgradeAvailable(out LocStrFormatted errorMessage)
    {
      errorMessage = LocStrFormatted.Empty;
      return true;
    }

    public void UpgradeSelf()
    {
      if (this.Prototype.Upgrade.NextTier.IsNone)
      {
        Log.Error("Upgrade not available!");
      }
      else
      {
        this.Prototype = this.Prototype.Upgrade.NextTier.Value;
        if (this.Prototype.InputProduct.HasValue == this.m_buffer.HasValue)
          return;
        if (this.m_buffer.HasValue)
          Log.Error("Upgraded statue does not have input product.");
        else
          this.m_buffer = (Option<ProductBuffer>) new ProductBuffer(Statue.BUFFER_CAPACITY, this.Prototype.InputProduct.Value);
      }
    }

    Quantity IEntityWithPorts.ReceiveAsMuchAsFromPort(ProductQuantity pq, IoPortToken sourcePort)
    {
      return this.m_buffer.HasValue && (Proto) this.m_buffer.Value.Product == (Proto) pq.Product ? this.m_buffer.Value.StoreAsMuchAs(pq) : pq.Quantity;
    }

    static Statue()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      Statue.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      Statue.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
      Statue.BUFFER_CAPACITY = 20.Quantity();
    }
  }
}
