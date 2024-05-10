// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.Layout.Upgrade.LayoutEntityUpgrader`2
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Economy;
using Mafi.Core.Entities.Validators;
using Mafi.Core.Notifications;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Entities.Static.Layout.Upgrade
{
  [GenerateSerializer(false, null, 0)]
  [OnlyForSaveCompatibility(null)]
  public class LayoutEntityUpgrader<TProto, TEntity> : IUpgrader
    where TProto : IProtoWithUpgrade<TProto>, LayoutEntityProto
    where TEntity : class, ILayoutEntity, IUpgradableEntity
  {
    private readonly UnlockedProtosDb m_unlockedProtosDb;
    private readonly TerrainOccupancyManager m_occupancyManager;
    private DependencyResolver m_resolver;
    private readonly UpgradeCostResolver m_upgradeCostResolver;
    private readonly TEntity m_entity;
    private TProto m_entityProto;
    private EntityNotificator m_upgradeNotif;
    private readonly LayoutEntityAddRequestFactory m_addRequestFactory;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    [DoNotSave(0, null)]
    public AssetValue PriceToUpgrade => this.computePriceToUpgrade();

    [DoNotSave(0, null)]
    public AssetValue ConstructionCostToUpgrade => this.PriceToUpgrade.TakeNonVirtualOnly();

    public bool UpgradeExists => this.m_entityProto.Upgrade.NextTier.HasValue;

    public Option<Proto> NextTier
    {
      get => (Option<Proto>) (Proto) this.m_entityProto.Upgrade.NextTier.ValueOrNull;
    }

    public LocStr UpgradeTitle
    {
      get
      {
        return !this.m_entityProto.Upgrade.NextTier.HasValue ? LocStr.Empty : this.m_entityProto.Upgrade.NextTier.Value.Strings.Name;
      }
    }

    public string Icon
    {
      get
      {
        return !this.m_entityProto.Upgrade.NextTier.HasValue ? "" : this.m_entityProto.Upgrade.NextTier.Value.Graphics.IconPath;
      }
    }

    public LayoutEntityUpgrader(
      UnlockedProtosDb unlockedProtosDb,
      TerrainOccupancyManager occupancyManager,
      DependencyResolver resolver,
      UpgradeCostResolver upgradeCostResolver,
      LayoutEntityAddRequestFactory addRequestFactory,
      TEntity entity,
      TProto entityProto)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_resolver = resolver;
      this.m_upgradeCostResolver = upgradeCostResolver;
      this.m_addRequestFactory = addRequestFactory;
      this.m_unlockedProtosDb = unlockedProtosDb.CheckNotNull<UnlockedProtosDb>();
      this.m_entity = entity.CheckNotNull<TEntity>();
      this.m_entityProto = entityProto.CheckNotNull<TProto>();
      this.m_occupancyManager = occupancyManager.CheckNotNull<TerrainOccupancyManager>();
      this.m_upgradeNotif = entity.Context.NotificationsManager.CreateNotificatorFor(IdsCore.Notifications.UpgradeInProgress);
    }

    public bool IsUpgradeAvailable(out LocStrFormatted errorMessage)
    {
      if (!this.IsUpgradeVisible())
      {
        errorMessage = LocStrFormatted.Empty;
        return false;
      }
      EntityId id = this.m_entity.Id;
      EntityValidationResult validationResult = ((IEntityAdditionValidator<IEntityWithOccupiedTilesAddRequest>) this.m_occupancyManager).CanAdd((IEntityWithOccupiedTilesAddRequest) this.m_addRequestFactory.CreateRequestFor<TProto>(this.m_entityProto.Upgrade.NextTier.Value, new EntityAddRequestData(this.m_entity.Transform, ignoreForCollisions: (Predicate<EntityId>) (x => x == id))));
      if (!validationResult.IsError)
        return this.m_entity.IsUpgradeAvailable(out errorMessage);
      errorMessage = new LocStrFormatted(validationResult.ErrorMessageForPlayer);
      return false;
    }

    private AssetValue computePriceToUpgrade()
    {
      return this.m_entityProto.Upgrade.NextTier.IsNone ? AssetValue.Empty : this.m_upgradeCostResolver.GetUpgradeCost(this.m_entityProto.Costs.Price, this.m_entityProto.Upgrade.NextTier.Value.Costs.Price);
    }

    public bool IsUpgradeVisible()
    {
      return this.m_entityProto.Upgrade.NextTier.HasValue && this.m_entity.ConstructionState == ConstructionState.Constructed && this.m_unlockedProtosDb.IsUnlocked((Proto) this.m_entityProto.Upgrade.NextTier.Value);
    }

    public void UpgradeStarted() => this.m_upgradeNotif.Activate((IEntity) this.m_entity);

    public void UpgradeCanceled() => this.m_upgradeNotif.Deactivate((IEntity) this.m_entity);

    public void Upgrade()
    {
      this.m_upgradeNotif.Deactivate((IEntity) this.m_entity);
      this.m_entity.UpgradeSelf();
      this.m_entityProto = (TProto) this.m_entity.Prototype;
    }

    public static void Serialize(LayoutEntityUpgrader<TProto, TEntity> value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<LayoutEntityUpgrader<TProto, TEntity>>(value))
        return;
      writer.EnqueueDataSerialization((object) value, LayoutEntityUpgrader<TProto, TEntity>.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteGeneric<TEntity>(this.m_entity);
      writer.WriteGeneric<TProto>(this.m_entityProto);
      TerrainOccupancyManager.Serialize(this.m_occupancyManager, writer);
      DependencyResolver.Serialize(this.m_resolver, writer);
      UnlockedProtosDb.Serialize(this.m_unlockedProtosDb, writer);
      EntityNotificator.Serialize(this.m_upgradeNotif, writer);
    }

    public static LayoutEntityUpgrader<TProto, TEntity> Deserialize(BlobReader reader)
    {
      LayoutEntityUpgrader<TProto, TEntity> layoutEntityUpgrader;
      if (reader.TryStartClassDeserialization<LayoutEntityUpgrader<TProto, TEntity>>(out layoutEntityUpgrader))
        reader.EnqueueDataDeserialization((object) layoutEntityUpgrader, LayoutEntityUpgrader<TProto, TEntity>.s_deserializeDataDelayedAction);
      return layoutEntityUpgrader;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.RegisterResolvedMember<LayoutEntityUpgrader<TProto, TEntity>>(this, "m_addRequestFactory", typeof (LayoutEntityAddRequestFactory), true);
      reader.SetField<LayoutEntityUpgrader<TProto, TEntity>>(this, "m_entity", (object) reader.ReadGenericAs<TEntity>());
      this.m_entityProto = reader.ReadGenericAs<TProto>();
      reader.SetField<LayoutEntityUpgrader<TProto, TEntity>>(this, "m_occupancyManager", (object) TerrainOccupancyManager.Deserialize(reader));
      this.m_resolver = DependencyResolver.Deserialize(reader);
      reader.SetField<LayoutEntityUpgrader<TProto, TEntity>>(this, "m_unlockedProtosDb", (object) UnlockedProtosDb.Deserialize(reader));
      reader.RegisterResolvedMember<LayoutEntityUpgrader<TProto, TEntity>>(this, "m_upgradeCostResolver", typeof (UpgradeCostResolver), true);
      this.m_upgradeNotif = EntityNotificator.Deserialize(reader);
    }

    static LayoutEntityUpgrader()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      LayoutEntityUpgrader<TProto, TEntity>.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((LayoutEntityUpgrader<TProto, TEntity>) obj).SerializeData(writer));
      LayoutEntityUpgrader<TProto, TEntity>.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((LayoutEntityUpgrader<TProto, TEntity>) obj).DeserializeData(reader));
    }
  }
}
