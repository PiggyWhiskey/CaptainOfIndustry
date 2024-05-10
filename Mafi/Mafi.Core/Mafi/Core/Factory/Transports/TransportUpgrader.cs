// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Transports.TransportUpgrader
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Notifications;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Factory.Transports
{
  [GenerateSerializer(false, null, 0)]
  public class TransportUpgrader : IUpgrader
  {
    [DoNotSave(0, null)]
    private AssetValue m_priceToUpgradeCache;
    private readonly UnlockedProtosDb m_unlockedProtosDb;
    private readonly UpgradeCostResolver m_upgradeCostResolver;
    private readonly Transport m_entity;
    private TransportProto m_entityProto;
    private EntityNotificator m_upgradeNotif;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public bool UpgradeExists => this.m_entityProto.Upgrade.NextTier.HasValue;

    public Option<Proto> NextTier
    {
      get => (Option<Proto>) (Proto) this.m_entityProto.Upgrade.NextTier.ValueOrNull;
    }

    public AssetValue PriceToUpgrade
    {
      get
      {
        if (this.m_priceToUpgradeCache == new AssetValue())
        {
          this.m_priceToUpgradeCache = !this.m_entityProto.Upgrade.NextTier.HasValue ? AssetValue.Empty : this.m_upgradeCostResolver.GetUpgradeCost(this.m_entityProto.GetPriceFor(this.m_entity.Trajectory.Pivots), this.m_entityProto.Upgrade.NextTier.Value.GetPriceFor(this.m_entity.Trajectory.Pivots), true);
          Assert.That<AssetValue>(this.m_priceToUpgradeCache).IsNotEqualTo<AssetValue>(new AssetValue());
        }
        return this.m_priceToUpgradeCache;
      }
    }

    public AssetValue ConstructionCostToUpgrade => this.PriceToUpgrade;

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

    public TransportUpgrader(
      INotificationsManager notificationsManager,
      UnlockedProtosDb unlockedProtosDb,
      UpgradeCostResolver upgradeCostResolver,
      Transport entity)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_upgradeCostResolver = upgradeCostResolver;
      this.m_unlockedProtosDb = unlockedProtosDb.CheckNotNull<UnlockedProtosDb>();
      this.m_entity = entity.CheckNotNull<Transport>();
      this.m_entityProto = entity.Prototype;
      this.m_upgradeNotif = notificationsManager.CreateNotificatorFor(IdsCore.Notifications.UpgradeInProgress);
    }

    public bool IsUpgradeAvailable(out LocStrFormatted errorMessage)
    {
      errorMessage = LocStrFormatted.Empty;
      return this.IsUpgradeVisible();
    }

    public bool IsUpgradeVisible()
    {
      return this.m_entityProto.Upgrade.NextTier.HasValue && this.m_entity.IsConstructed && this.m_unlockedProtosDb.IsUnlocked((Proto) this.m_entityProto.Upgrade.NextTier.Value);
    }

    public void Upgrade()
    {
      this.m_upgradeNotif.Deactivate((IEntity) this.m_entity);
      ((IUpgradableEntity) this.m_entity).UpgradeSelf();
      this.m_entityProto = this.m_entity.Prototype;
      this.m_priceToUpgradeCache = new AssetValue();
    }

    public void UpgradeStarted() => this.m_upgradeNotif.Activate((IEntity) this.m_entity);

    public void UpgradeCanceled() => this.m_upgradeNotif.Deactivate((IEntity) this.m_entity);

    public static void Serialize(TransportUpgrader value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TransportUpgrader>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TransportUpgrader.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Transport.Serialize(this.m_entity, writer);
      writer.WriteGeneric<TransportProto>(this.m_entityProto);
      UnlockedProtosDb.Serialize(this.m_unlockedProtosDb, writer);
      EntityNotificator.Serialize(this.m_upgradeNotif, writer);
    }

    public static TransportUpgrader Deserialize(BlobReader reader)
    {
      TransportUpgrader transportUpgrader;
      if (reader.TryStartClassDeserialization<TransportUpgrader>(out transportUpgrader))
        reader.EnqueueDataDeserialization((object) transportUpgrader, TransportUpgrader.s_deserializeDataDelayedAction);
      return transportUpgrader;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<TransportUpgrader>(this, "m_entity", (object) Transport.Deserialize(reader));
      this.m_entityProto = reader.ReadGenericAs<TransportProto>();
      reader.SetField<TransportUpgrader>(this, "m_unlockedProtosDb", (object) UnlockedProtosDb.Deserialize(reader));
      reader.RegisterResolvedMember<TransportUpgrader>(this, "m_upgradeCostResolver", typeof (UpgradeCostResolver), true);
      this.m_upgradeNotif = EntityNotificator.Deserialize(reader);
    }

    static TransportUpgrader()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TransportUpgrader.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((TransportUpgrader) obj).SerializeData(writer));
      TransportUpgrader.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((TransportUpgrader) obj).DeserializeData(reader));
    }
  }
}
