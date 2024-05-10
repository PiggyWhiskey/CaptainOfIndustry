// Decompiled with JetBrains decompiler
// Type: Mafi.Core.World.Entities.WorldMapVillage
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Buildings.Cargo;
using Mafi.Core.Buildings.Settlements;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Notifications;
using Mafi.Core.Population;
using Mafi.Core.Products;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Simulation;
using Mafi.Core.Utils;
using Mafi.Core.World.QuickTrade;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Linq;

#nullable disable
namespace Mafi.Core.World.Entities
{
  [GenerateSerializer(false, null, 0)]
  public class WorldMapVillage : 
    WorldMapRepairableEntity,
    IUpgradableWorldEntity,
    IWorldMapEntity,
    IEntity,
    IIsSafeAsHashKey
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private static readonly Duration DURATION_TO_UPGRADE;
    public readonly WorldMapVillageProto Prototype;
    public readonly Lyst<QuickTradeProvider> QuickTrades;
    private readonly WorldMapManager m_worldMapManager;
    private readonly IUpointsManager m_upointsManager;
    private readonly IInstaBuildManager m_instaBuildManager;
    private readonly SettlementsManager m_settlementsManager;
    private readonly INotificationsManager m_notificationsManager;
    private readonly TickTimer m_timerTillNextPop;

    public static void Serialize(WorldMapVillage value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<WorldMapVillage>(value))
        return;
      writer.EnqueueDataSerialization((object) value, WorldMapVillage.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteGeneric<IInstaBuildManager>(this.m_instaBuildManager);
      writer.WriteGeneric<INotificationsManager>(this.m_notificationsManager);
      SettlementsManager.Serialize(this.m_settlementsManager, writer);
      TickTimer.Serialize(this.m_timerTillNextPop, writer);
      writer.WriteGeneric<IUpointsManager>(this.m_upointsManager);
      WorldMapManager.Serialize(this.m_worldMapManager, writer);
      writer.WriteInt(this.PopsAvailable);
      writer.WriteGeneric<WorldMapVillageProto>(this.Prototype);
      Lyst<QuickTradeProvider>.Serialize(this.QuickTrades, writer);
      writer.WriteInt(this.Reputation);
    }

    public static WorldMapVillage Deserialize(BlobReader reader)
    {
      WorldMapVillage worldMapVillage;
      if (reader.TryStartClassDeserialization<WorldMapVillage>(out worldMapVillage))
        reader.EnqueueDataDeserialization((object) worldMapVillage, WorldMapVillage.s_deserializeDataDelayedAction);
      return worldMapVillage;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<WorldMapVillage>(this, "m_instaBuildManager", (object) reader.ReadGenericAs<IInstaBuildManager>());
      reader.SetField<WorldMapVillage>(this, "m_notificationsManager", (object) reader.ReadGenericAs<INotificationsManager>());
      reader.SetField<WorldMapVillage>(this, "m_settlementsManager", (object) SettlementsManager.Deserialize(reader));
      reader.SetField<WorldMapVillage>(this, "m_timerTillNextPop", (object) TickTimer.Deserialize(reader));
      reader.SetField<WorldMapVillage>(this, "m_upointsManager", (object) reader.ReadGenericAs<IUpointsManager>());
      reader.SetField<WorldMapVillage>(this, "m_worldMapManager", (object) WorldMapManager.Deserialize(reader));
      this.PopsAvailable = reader.ReadInt();
      reader.SetField<WorldMapVillage>(this, "Prototype", (object) reader.ReadGenericAs<WorldMapVillageProto>());
      reader.SetField<WorldMapVillage>(this, "QuickTrades", (object) Lyst<QuickTradeProvider>.Deserialize(reader));
      this.Reputation = reader.ReadInt();
      reader.RegisterInitAfterLoad<WorldMapVillage>(this, "initAfterLoad", InitPriority.Normal);
    }

    public override bool CanBePaused => false;

    public override AssetValue CostToRepair => AssetValue.Empty;

    protected override bool IsEnabledNow => !this.IsPaused && this.IsRepaired;

    public IIndexable<QuickTradeProvider> QuickTradesIndexable
    {
      get => (IIndexable<QuickTradeProvider>) this.QuickTrades;
    }

    public bool IsPopsAdoptionSupported => this.Prototype.MinReputationNeededToAdopt >= 0;

    public bool IsPopsAdoptionAvailable
    {
      get
      {
        return this.IsPopsAdoptionSupported && this.Reputation >= this.Prototype.MinReputationNeededToAdopt;
      }
    }

    public int PopsAvailable { get; private set; }

    /// <summary>Current reputation the player has in this village.</summary>
    public int Reputation { get; private set; }

    public AssetValue PriceToUpgrade => this.Prototype.CostPerLevel(this.Reputation + 1);

    public LocStrFormatted UpgradeTitle => this.Prototype.GetUpgradeTitle(this.Reputation + 1);

    public bool UpgradeExists => this.Reputation < this.Prototype.MaxReputation;

    public string UpgradeIcon => this.Prototype.Graphics.IconPath;

    public WorldMapVillage(
      EntityId entityId,
      WorldMapVillageProto prototype,
      WorldMapLocation location,
      EntityContext context,
      ISimLoopEvents simLoopEvents,
      IAssetTransactionManager assetManager,
      WorldMapManager worldMapManager,
      IUpointsManager upointsManager,
      IInstaBuildManager instaBuildManager,
      IProductsManager productsManager,
      TradeDockManager tradeDockManager,
      SettlementsManager settlementsManager,
      INotificationsManager notificationsManager,
      IPropertiesDb propertiesDb,
      AllImplementationsOf<IVirtualProductQuickTradeHandler> virtualTradeHandlers)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_timerTillNextPop = new TickTimer();
      // ISSUE: explicit constructor call
      base.\u002Ector(entityId, (WorldMapEntityProto) prototype, location, context, worldMapManager, instaBuildManager, productsManager);
      this.Prototype = prototype;
      this.Reputation = this.Prototype.StartingReputation;
      this.m_worldMapManager = worldMapManager;
      this.m_upointsManager = upointsManager;
      this.m_instaBuildManager = instaBuildManager;
      this.m_settlementsManager = settlementsManager;
      this.m_notificationsManager = notificationsManager;
      ImmutableArray<IVirtualProductQuickTradeHandler> implementations = virtualTradeHandlers.Implementations;
      this.QuickTrades = new Lyst<QuickTradeProvider>();
      foreach (QuickTradePairProto quickTrade in this.Prototype.QuickTrades)
      {
        VirtualProductProto virtProd = quickTrade.ProductToBuy.Product as VirtualProductProto;
        Option<IVirtualProductQuickTradeHandler> tradeHandlers;
        if (virtProd != null)
        {
          IVirtualProductQuickTradeHandler quickTradeHandler = implementations.FirstOrDefault((Func<IVirtualProductQuickTradeHandler, bool>) (x => x.IsProductManaged((ProductProto) virtProd)));
          if (quickTradeHandler == null)
          {
            Log.Error(string.Format("No handler found for trade of '{0}'!", (object) virtProd));
            continue;
          }
          tradeHandlers = quickTradeHandler.SomeOption<IVirtualProductQuickTradeHandler>();
        }
        else
          tradeHandlers = (Option<IVirtualProductQuickTradeHandler>) Option.None;
        this.QuickTrades.Add(new QuickTradeProvider(quickTrade, this, tradeDockManager, productsManager, simLoopEvents, assetManager, upointsManager, propertiesDb, tradeHandlers));
      }
      this.PopsAvailable = 0;
      this.MarkRepaired();
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void initAfterLoad(DependencyResolver resolver)
    {
      foreach (QuickTradePairProto quickTrade in this.Prototype.QuickTrades)
      {
        QuickTradePairProto tradeProto = quickTrade;
        if (!(tradeProto.ProductToBuy.Product is VirtualProductProto) && this.QuickTrades.All<QuickTradeProvider>((Func<QuickTradeProvider, bool>) (x => x.Prototype.Id != tradeProto.Id)))
        {
          QuickTradeProvider trade = new QuickTradeProvider(tradeProto, this, resolver.Resolve<TradeDockManager>(), (IProductsManager) resolver.Resolve<ProductsManager>(), resolver.Resolve<ISimLoopEvents>(), resolver.Resolve<IAssetTransactionManager>(), resolver.Resolve<IUpointsManager>(), resolver.Resolve<IPropertiesDb>(), Option<IVirtualProductQuickTradeHandler>.None);
          this.QuickTrades.Add(trade);
          resolver.Resolve<WorldMapManager>().AddTradeAfterLoad(trade);
        }
      }
    }

    protected override void SimUpdateInternal()
    {
      base.SimUpdateInternal();
      if (!this.IsEnabled || this.m_timerTillNextPop.Decrement())
        return;
      if (this.IsPopsAdoptionAvailable && this.PopsAvailable < this.Prototype.PopsToAdoptCapPerReputationLevel(this.Reputation))
        ++this.PopsAvailable;
      this.m_timerTillNextPop.Start(this.Prototype.DurationPerNewPopPerReputationLevel(this.Reputation));
    }

    public bool CanAdopt(int pops, out LocStrFormatted error)
    {
      if (!this.IsEnabled)
      {
        error = new LocStrFormatted("Not enabled");
        return false;
      }
      if (!this.IsPopsAdoptionSupported)
      {
        error = new LocStrFormatted("Adoption not supported");
        return false;
      }
      if (this.Prototype.MinReputationNeededToAdopt > this.Reputation)
      {
        error = TrCore.Status_LowReputation.Format(this.Prototype.MinReputationNeededToAdopt.ToString());
        return false;
      }
      if (pops == 0 || pops > this.PopsAvailable)
      {
        error = (LocStrFormatted) TrCore.PopsToAdoptNotAvailable;
        return false;
      }
      if (!this.m_upointsManager.CanConsume(pops * this.Prototype.UpointsPerPopToAdopt))
      {
        error = (LocStrFormatted) TrCore.TradeStatus__NoUnity;
        return false;
      }
      error = LocStrFormatted.Empty;
      return true;
    }

    public void AdoptPops(int popsToAdop)
    {
      if (!this.IsEnabled)
      {
        Log.Error("Cannot adopt from non-repaired location");
      }
      else
      {
        if (!this.CanAdopt(popsToAdop, out LocStrFormatted _))
          return;
        Upoints unity = popsToAdop * this.Prototype.UpointsPerPopToAdopt;
        this.m_upointsManager.TryConsume(IdsCore.UpointsCategories.PopsAdoption, unity);
        this.PopsAvailable -= popsToAdop;
        this.m_settlementsManager.AddPops(popsToAdop, PopsAdditionReason.RefugeesOrAdopted);
      }
    }

    public void StartUpgrade()
    {
      if (!this.IsUpgradeAvailable(out LocStrFormatted _))
        return;
      if (this.m_instaBuildManager.IsInstaBuildEnabled)
      {
        this.UpgradeSelf();
        this.m_onConstructed.Invoke((IWorldMapRepairableEntity) this);
      }
      else
      {
        AssetValue priceToUpgrade = this.PriceToUpgrade;
        ImmutableArray<ProductBuffer> immutableArray = priceToUpgrade.Products.Map<ProductBuffer>((Func<ProductQuantity, ProductBuffer>) (x => new ProductBuffer(x.Quantity, x.Product))).ToImmutableArray();
        int num = priceToUpgrade.GetQuantitySum().Value;
        Duration durationPerProduct = num <= 0 ? Duration.OneTick : Duration.OneTick.Max(WorldMapVillage.DURATION_TO_UPGRADE / num);
        this.SetUpgradeProgress(new Mafi.Core.Entities.Static.ConstructionProgress((IEntity) this, immutableArray, priceToUpgrade, durationPerProduct, Duration.Zero));
      }
    }

    protected override void OnUpgradeProgressDone() => this.UpgradeSelf();

    public bool IsUpgradeAvailable(out LocStrFormatted error)
    {
      error = LocStrFormatted.Empty;
      return this.IsRepaired && this.UpgradeExists;
    }

    public bool IsUpgradeVisible()
    {
      return this.IsRepaired && this.UpgradeExists && !this.IsBeingUpgraded;
    }

    public bool UpgradeSelf()
    {
      if (!this.IsUpgradeAvailable(out LocStrFormatted _))
        return false;
      ++this.Reputation;
      if (this.Prototype.MinReputationNeededToAdopt > 0 && this.Prototype.MinReputationNeededToAdopt <= this.Reputation)
        this.PopsAvailable = this.Prototype.PopsToAdoptCapPerReputationLevel(this.Reputation);
      return true;
    }

    static WorldMapVillage()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      WorldMapVillage.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      WorldMapVillage.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
      WorldMapVillage.DURATION_TO_UPGRADE = 10.Seconds();
    }
  }
}
