// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Settlements.SettlementHousingModule
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Entities.Static.Layout.Upgrade;
using Mafi.Core.Notifications;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Settlements
{
  [GenerateSerializer(false, null, 0)]
  public class SettlementHousingModule : 
    LayoutEntity,
    IUpgradableEntity,
    IStaticEntity,
    IEntityWithPosition,
    IRenderedEntity,
    IEntity,
    IIsSafeAsHashKey,
    IAreaSelectableEntity,
    ISettlementSquareModule,
    ILayoutEntity,
    IEntityWithSimUpdate
  {
    private SettlementHousingModuleProto m_proto;
    private EntityNotificator m_noFoodModuleNotif;
    private EntityNotificator m_settlementStarvingNotif;
    private EntityNotificator m_settlementFullOfLandfill;
    private readonly Lyst<SettlementDecorationModule> m_decorationsWithInfluence;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    [DoNotSave(0, null)]
    public SettlementHousingModuleProto Prototype
    {
      get => this.m_proto;
      private set
      {
        this.m_proto = value;
        this.Prototype = (LayoutEntityProto) value;
      }
    }

    public override bool CanBePaused => true;

    public Option<Mafi.Core.Buildings.Settlements.Settlement> Settlement { get; private set; }

    public IUpgrader Upgrader { get; private set; }

    /// <summary>
    /// Index into Prototype.UnityIncreases if a boost was achieved otherwise -1.
    /// </summary>
    public int AchievedUnityIncreaseIndexLastUpdate { get; set; }

    public int Population { get; private set; }

    protected override bool IsEnabledNow => this.IsEnabledNowIgnoreUpgrade;

    public int Capacity => !this.IsEnabled ? 0 : this.Prototype.Capacity;

    public Upoints UpointsCapacity
    {
      get => !this.IsEnabled ? Upoints.Zero : this.Prototype.UpointsCapacity;
    }

    public RelTile2i CoreSize => this.Prototype.Layout.CoreSize;

    public Tile3i Position => this.Transform.Position;

    public SettlementHousingModule(
      EntityId id,
      SettlementHousingModuleProto settlementModuleProto,
      TileTransform transform,
      EntityContext context,
      ILayoutEntityUpgraderFactory upgraderFactory)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_decorationsWithInfluence = new Lyst<SettlementDecorationModule>();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (LayoutEntityProto) settlementModuleProto, transform, context);
      this.Prototype = settlementModuleProto;
      this.Upgrader = upgraderFactory.CreateInstance<SettlementHousingModuleProto, SettlementHousingModule>(this, this.Prototype);
      this.m_noFoodModuleNotif = context.NotificationsManager.CreateNotificatorFor(IdsCore.Notifications.SettlementHasNoFoodModule);
      this.m_settlementStarvingNotif = context.NotificationsManager.CreateNotificatorFor(IdsCore.Notifications.SettlementIsStarving);
      this.m_settlementFullOfLandfill = context.NotificationsManager.CreateNotificatorFor(IdsCore.Notifications.SettlementFullOfLandfill);
    }

    void IEntityWithSimUpdate.SimUpdate()
    {
      this.m_noFoodModuleNotif.NotifyIff(this.IsEnabled && this.Settlement.Value.HasNoFoodModule, (IEntity) this);
      this.m_settlementStarvingNotif.NotifyIff(this.IsEnabled && this.Settlement.Value.ArePeopleStarving, (IEntity) this);
      this.m_settlementFullOfLandfill.NotifyIff(this.IsEnabled && this.Settlement.Value.NominalHealthPenaltyFromWasteLastDay.IsPositive, (IEntity) this);
    }

    public void SetSettlement(Mafi.Core.Buildings.Settlements.Settlement settlement)
    {
      this.Settlement = (Option<Mafi.Core.Buildings.Settlements.Settlement>) settlement;
    }

    public void ClearSettlement() => this.Settlement = Option<Mafi.Core.Buildings.Settlements.Settlement>.None;

    bool IUpgradableEntity.IsUpgradeAvailable(out LocStrFormatted errorMessage)
    {
      errorMessage = LocStrFormatted.Empty;
      return true;
    }

    public void UpgradeSelf()
    {
      if (this.Prototype.Upgrade.NextTier.IsNone)
        Log.Error("Upgrade not available!");
      else
        this.Prototype = this.Prototype.Upgrade.NextTier.Value;
    }

    public void SetPopulation(int population)
    {
      Assert.That<int>(population).IsNotNegative();
      Assert.That<int>(population).IsLessOrEqual(this.Capacity);
      this.Population = population.Min(this.Capacity);
    }

    public Upoints GetUpointsForDecorations()
    {
      Upoints rhs = Upoints.Zero;
      foreach (SettlementDecorationModule decorationModule in this.m_decorationsWithInfluence)
      {
        if (decorationModule.IsConstructed)
          rhs = decorationModule.Prototype.UpointsBonusToNearbyHousing.Max(rhs);
      }
      return rhs;
    }

    public void AddDecoration(SettlementDecorationModule decoration)
    {
      this.m_decorationsWithInfluence.Add(decoration);
    }

    public void ClearDecorations() => this.m_decorationsWithInfluence.Clear();

    public static void Serialize(SettlementHousingModule value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SettlementHousingModule>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SettlementHousingModule.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteInt(this.AchievedUnityIncreaseIndexLastUpdate);
      Lyst<SettlementDecorationModule>.Serialize(this.m_decorationsWithInfluence, writer);
      EntityNotificator.Serialize(this.m_noFoodModuleNotif, writer);
      writer.WriteGeneric<SettlementHousingModuleProto>(this.m_proto);
      EntityNotificator.Serialize(this.m_settlementFullOfLandfill, writer);
      EntityNotificator.Serialize(this.m_settlementStarvingNotif, writer);
      writer.WriteInt(this.Population);
      Option<Mafi.Core.Buildings.Settlements.Settlement>.Serialize(this.Settlement, writer);
      writer.WriteGeneric<IUpgrader>(this.Upgrader);
    }

    public static SettlementHousingModule Deserialize(BlobReader reader)
    {
      SettlementHousingModule settlementHousingModule;
      if (reader.TryStartClassDeserialization<SettlementHousingModule>(out settlementHousingModule))
        reader.EnqueueDataDeserialization((object) settlementHousingModule, SettlementHousingModule.s_deserializeDataDelayedAction);
      return settlementHousingModule;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.AchievedUnityIncreaseIndexLastUpdate = reader.ReadInt();
      reader.SetField<SettlementHousingModule>(this, "m_decorationsWithInfluence", (object) Lyst<SettlementDecorationModule>.Deserialize(reader));
      this.m_noFoodModuleNotif = EntityNotificator.Deserialize(reader);
      this.m_proto = reader.ReadGenericAs<SettlementHousingModuleProto>();
      this.m_settlementFullOfLandfill = EntityNotificator.Deserialize(reader);
      this.m_settlementStarvingNotif = EntityNotificator.Deserialize(reader);
      this.Population = reader.ReadInt();
      this.Settlement = Option<Mafi.Core.Buildings.Settlements.Settlement>.Deserialize(reader);
      this.Upgrader = reader.ReadGenericAs<IUpgrader>();
    }

    static SettlementHousingModule()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      SettlementHousingModule.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      SettlementHousingModule.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }
  }
}
