// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Population.Refugees.RefugeesManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Buildings.Beacons;
using Mafi.Core.Buildings.Settlements;
using Mafi.Core.Economy;
using Mafi.Core.Entities.Static;
using Mafi.Core.MessageNotifications;
using Mafi.Core.MessageNotifications.Notifications;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Serialization;
using System;
using System.Linq;

#nullable disable
namespace Mafi.Core.Population.Refugees
{
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class RefugeesManager
  {
    private readonly SettlementsManager m_settlementsManager;
    private readonly IAssetTransactionManager m_assetsManager;
    private readonly IProductsManager m_productsManager;
    private readonly IMessageNotificationsManager m_messageNotificationsManager;
    private readonly IRandom m_random;
    private readonly Option<BeaconScheduleProto> m_beaconScheduleProto;
    private int m_nextRewardIndex;
    private Option<RefugeesReward> m_nextReward;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public Option<Mafi.Core.Buildings.Beacons.Beacon> Beacon { get; private set; }

    public Option<RefugeesReward> NextReward
    {
      get
      {
        return !this.m_nextReward.HasValue || !this.Beacon.HasValue || this.m_nextReward.Value.MinimalTier > this.Beacon.Value.Prototype.Tier ? Option<RefugeesReward>.None : this.m_nextReward;
      }
    }

    public Duration StepsDoneSoFar { get; private set; }

    public Duration? DurationLeft
    {
      get
      {
        return !this.NextReward.HasValue ? new Duration?() : new Duration?(this.NextReward.Value.Duration - this.StepsDoneSoFar);
      }
    }

    public RefugeesManager(
      ProtosDb protosDb,
      SettlementsManager settlementsManager,
      IAssetTransactionManager assetsManager,
      IProductsManager productsManager,
      ISimLoopEvents simLoopEvents,
      RandomProvider randomProvider,
      IConstructionManager constructionManager,
      IMessageNotificationsManager messageNotificationsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_settlementsManager = settlementsManager;
      this.m_assetsManager = assetsManager;
      this.m_productsManager = productsManager;
      this.m_messageNotificationsManager = messageNotificationsManager;
      this.m_beaconScheduleProto = (Option<BeaconScheduleProto>) protosDb.All<BeaconScheduleProto>().FirstOrDefault<BeaconScheduleProto>();
      this.m_random = randomProvider.GetSimRandomFor((object) this);
      this.getNextReward();
      constructionManager.EntityConstructed.Add<RefugeesManager>(this, new Action<IStaticEntity>(this.entityConstructed));
      constructionManager.EntityStartedDeconstruction.Add<RefugeesManager>(this, new Action<IStaticEntity>(this.entityDeconstructionStarted));
      simLoopEvents.Update.Add<RefugeesManager>(this, new Action(this.simUpdate));
    }

    private void entityConstructed(IStaticEntity entity)
    {
      if (!(entity is Mafi.Core.Buildings.Beacons.Beacon beacon))
        return;
      if (this.Beacon.HasValue)
      {
        Log.Error("Cannot add another beacon!");
      }
      else
      {
        Assert.That<Option<Mafi.Core.Buildings.Beacons.Beacon>>(this.Beacon).IsNone<Mafi.Core.Buildings.Beacons.Beacon>();
        this.Beacon = (Option<Mafi.Core.Buildings.Beacons.Beacon>) beacon;
      }
    }

    private void entityDeconstructionStarted(IStaticEntity entity)
    {
      if (!(entity is Mafi.Core.Buildings.Beacons.Beacon beacon))
        return;
      if (this.Beacon.HasValue && this.Beacon != beacon)
      {
        Log.Error("Beacon already exists!");
      }
      else
      {
        Assert.That<Mafi.Core.Buildings.Beacons.Beacon>(beacon).IsEqualTo<Mafi.Core.Buildings.Beacons.Beacon>(this.Beacon.ValueOrNull);
        this.Beacon = (Option<Mafi.Core.Buildings.Beacons.Beacon>) Option.None;
      }
    }

    private void simUpdate()
    {
      if (this.Beacon.IsNone)
        return;
      Option<RefugeesReward> nextReward = this.NextReward;
      if (nextReward.IsNone)
        return;
      if (!this.Beacon.Value.TryWork())
      {
        this.StepsDoneSoFar = (this.StepsDoneSoFar - Duration.OneTick).Max(Duration.Zero);
      }
      else
      {
        this.StepsDoneSoFar += Duration.OneTick;
        if (this.StepsDoneSoFar < nextReward.Value.Duration)
          return;
        this.processNewRefugees(nextReward.Value);
      }
    }

    private void getNextReward()
    {
      if (!this.m_beaconScheduleProto.HasValue)
        return;
      this.m_nextReward = this.m_beaconScheduleProto.Value.OffersGenerator(this.m_nextRewardIndex);
      ++this.m_nextRewardIndex;
    }

    private void processNewRefugees(RefugeesReward offer)
    {
      this.m_settlementsManager.AddPops(offer.AmountOfRefugees, PopsAdditionReason.RefugeesOrAdopted);
      ImmutableArray<ProductQuantity> reward = ImmutableArray<ProductQuantity>.Empty;
      if (offer.PossibleRewards.IsNotEmpty)
      {
        reward = offer.PossibleRewards[this.m_random];
        this.addReward(reward);
      }
      this.getNextReward();
      this.StepsDoneSoFar = Duration.Zero;
      this.m_messageNotificationsManager.AddMessage((IMessageNotification) new NewRefugeesMessage(offer.AmountOfRefugees, reward));
    }

    private void addReward(ImmutableArray<ProductQuantity> reward)
    {
      this.m_assetsManager.StoreValue(new AssetValue(new SmallImmutableArray<ProductQuantity>(reward)), new CreateReason?(CreateReason.Loot));
    }

    public void Cheat_FinishCurrentDiscovery()
    {
    }

    public static void Serialize(RefugeesManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<RefugeesManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, RefugeesManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Option<Mafi.Core.Buildings.Beacons.Beacon>.Serialize(this.Beacon, writer);
      writer.WriteGeneric<IAssetTransactionManager>(this.m_assetsManager);
      Option<BeaconScheduleProto>.Serialize(this.m_beaconScheduleProto, writer);
      writer.WriteGeneric<IMessageNotificationsManager>(this.m_messageNotificationsManager);
      Option<RefugeesReward>.Serialize(this.m_nextReward, writer);
      writer.WriteInt(this.m_nextRewardIndex);
      writer.WriteGeneric<IProductsManager>(this.m_productsManager);
      writer.WriteGeneric<IRandom>(this.m_random);
      SettlementsManager.Serialize(this.m_settlementsManager, writer);
      Duration.Serialize(this.StepsDoneSoFar, writer);
    }

    public static RefugeesManager Deserialize(BlobReader reader)
    {
      RefugeesManager refugeesManager;
      if (reader.TryStartClassDeserialization<RefugeesManager>(out refugeesManager))
        reader.EnqueueDataDeserialization((object) refugeesManager, RefugeesManager.s_deserializeDataDelayedAction);
      return refugeesManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.Beacon = Option<Mafi.Core.Buildings.Beacons.Beacon>.Deserialize(reader);
      reader.SetField<RefugeesManager>(this, "m_assetsManager", (object) reader.ReadGenericAs<IAssetTransactionManager>());
      reader.SetField<RefugeesManager>(this, "m_beaconScheduleProto", (object) Option<BeaconScheduleProto>.Deserialize(reader));
      reader.SetField<RefugeesManager>(this, "m_messageNotificationsManager", (object) reader.ReadGenericAs<IMessageNotificationsManager>());
      this.m_nextReward = Option<RefugeesReward>.Deserialize(reader);
      this.m_nextRewardIndex = reader.ReadInt();
      reader.SetField<RefugeesManager>(this, "m_productsManager", (object) reader.ReadGenericAs<IProductsManager>());
      reader.SetField<RefugeesManager>(this, "m_random", (object) reader.ReadGenericAs<IRandom>());
      reader.SetField<RefugeesManager>(this, "m_settlementsManager", (object) SettlementsManager.Deserialize(reader));
      this.StepsDoneSoFar = Duration.Deserialize(reader);
    }

    static RefugeesManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      RefugeesManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((RefugeesManager) obj).SerializeData(writer));
      RefugeesManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((RefugeesManager) obj).DeserializeData(reader));
    }
  }
}
