// Decompiled with JetBrains decompiler
// Type: Mafi.Core.World.QuickTrade.QuickTradeProvider
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.Cargo;
using Mafi.Core.Economy;
using Mafi.Core.Population;
using Mafi.Core.Products;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Simulation;
using Mafi.Core.Utils;
using Mafi.Core.World.Entities;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.World.QuickTrade
{
  [GenerateSerializer(false, null, 0)]
  public class QuickTradeProvider
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly QuickTradePairProto Prototype;
    private readonly TradeDockManager m_tradeDockManager;
    private readonly IProductsManager m_productsManager;
    private readonly IAssetTransactionManager m_assetManager;
    private readonly IUpointsManager m_upointsManager;
    private readonly WorldMapVillage m_village;
    private ProductQuantity m_productToPayWithThisStep;
    public Upoints UpointsCost;
    private readonly Option<IVirtualProductQuickTradeHandler> m_tradeHandler;
    private readonly TickTimer m_timerToNextStep;
    private readonly IProperty<Percent> m_tradeVolumeMultiplier;

    public static void Serialize(QuickTradeProvider value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<QuickTradeProvider>(value))
        return;
      writer.EnqueueDataSerialization((object) value, QuickTradeProvider.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteInt(this.CurrentStep);
      writer.WriteGeneric<IAssetTransactionManager>(this.m_assetManager);
      writer.WriteGeneric<IProductsManager>(this.m_productsManager);
      ProductQuantity.Serialize(this.m_productToPayWithThisStep, writer);
      TickTimer.Serialize(this.m_timerToNextStep, writer);
      TradeDockManager.Serialize(this.m_tradeDockManager, writer);
      Option<IVirtualProductQuickTradeHandler>.Serialize(this.m_tradeHandler, writer);
      writer.WriteGeneric<IProperty<Percent>>(this.m_tradeVolumeMultiplier);
      writer.WriteGeneric<IUpointsManager>(this.m_upointsManager);
      WorldMapVillage.Serialize(this.m_village, writer);
      writer.WriteGeneric<QuickTradePairProto>(this.Prototype);
      Upoints.Serialize(this.UpointsCost, writer);
    }

    public static QuickTradeProvider Deserialize(BlobReader reader)
    {
      QuickTradeProvider quickTradeProvider;
      if (reader.TryStartClassDeserialization<QuickTradeProvider>(out quickTradeProvider))
        reader.EnqueueDataDeserialization((object) quickTradeProvider, QuickTradeProvider.s_deserializeDataDelayedAction);
      return quickTradeProvider;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.CurrentStep = reader.ReadInt();
      reader.SetField<QuickTradeProvider>(this, "m_assetManager", (object) reader.ReadGenericAs<IAssetTransactionManager>());
      reader.SetField<QuickTradeProvider>(this, "m_productsManager", (object) reader.ReadGenericAs<IProductsManager>());
      this.m_productToPayWithThisStep = ProductQuantity.Deserialize(reader);
      reader.SetField<QuickTradeProvider>(this, "m_timerToNextStep", (object) TickTimer.Deserialize(reader));
      reader.SetField<QuickTradeProvider>(this, "m_tradeDockManager", (object) TradeDockManager.Deserialize(reader));
      reader.SetField<QuickTradeProvider>(this, "m_tradeHandler", (object) Option<IVirtualProductQuickTradeHandler>.Deserialize(reader));
      reader.SetField<QuickTradeProvider>(this, "m_tradeVolumeMultiplier", (object) reader.ReadGenericAs<IProperty<Percent>>());
      reader.SetField<QuickTradeProvider>(this, "m_upointsManager", (object) reader.ReadGenericAs<IUpointsManager>());
      reader.SetField<QuickTradeProvider>(this, "m_village", (object) WorldMapVillage.Deserialize(reader));
      reader.SetField<QuickTradeProvider>(this, "Prototype", (object) reader.ReadGenericAs<QuickTradePairProto>());
      this.UpointsCost = Upoints.Deserialize(reader);
    }

    public LocStrFormatted DescriptionOfTrade
    {
      get
      {
        return !this.m_tradeHandler.HasValue ? (LocStrFormatted) TrCore.TradeStatus__Info : this.m_tradeHandler.Value.DescriptionOfTrade;
      }
    }

    public LocStrFormatted MessageOnDelivery
    {
      get
      {
        return !this.m_tradeHandler.HasValue ? (LocStrFormatted) TrCore.TradeOfferDelivered : this.m_tradeHandler.Value.MessageOnDelivery;
      }
    }

    public bool HasPriceIncreased
    {
      get => this.m_productToPayWithThisStep.Quantity != this.Prototype.ProductToPayWith.Quantity;
    }

    public bool IsSoldOut => this.CurrentStep >= this.Prototype.MaxSteps;

    public int CurrentStep { get; private set; }

    public QuickTradeProvider(
      QuickTradePairProto prototype,
      WorldMapVillage village,
      TradeDockManager tradeDockManager,
      IProductsManager productsManager,
      ISimLoopEvents simLoopEvents,
      IAssetTransactionManager assetManager,
      IUpointsManager upointsManager,
      IPropertiesDb propertiesDb,
      Option<IVirtualProductQuickTradeHandler> tradeHandlers)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_timerToNextStep = new TickTimer();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Prototype = prototype;
      this.m_village = village;
      this.m_tradeDockManager = tradeDockManager;
      this.m_productsManager = productsManager;
      this.m_assetManager = assetManager;
      this.m_upointsManager = upointsManager;
      this.m_tradeHandler = tradeHandlers;
      this.m_productToPayWithThisStep = this.Prototype.ProductToPayWith;
      this.UpointsCost = this.Prototype.UpointsPerTrade;
      this.m_tradeVolumeMultiplier = propertiesDb.GetProperty<Percent>(IdsCore.PropertyIds.TradeVolumeMultiplier);
      simLoopEvents.Update.Add<QuickTradeProvider>(this, new Action(this.simUpdate));
    }

    public ProductQuantity GetProductToBuy()
    {
      return this.Prototype.IgnoreTradeMultipliers ? this.Prototype.ProductToBuy : this.Prototype.ProductToBuy.ScaledBy(this.m_tradeVolumeMultiplier.Value);
    }

    public ProductQuantity GetProductToPayWith()
    {
      return this.Prototype.IgnoreTradeMultipliers ? this.m_productToPayWithThisStep : this.m_productToPayWithThisStep.ScaledBy(this.m_tradeVolumeMultiplier.Value);
    }

    private void simUpdate()
    {
      if (this.CurrentStep == 0 || this.m_timerToNextStep.Decrement())
        return;
      this.setStep(this.CurrentStep - 1);
      this.m_timerToNextStep.Start(this.GetCooldownForCurrentStep());
    }

    public bool CanAfford(out LocStrFormatted error)
    {
      if (this.m_village.Reputation < this.Prototype.MinReputationRequired)
      {
        error = TrCore.Status_LowReputation.Format(this.Prototype.MinReputationRequired.ToString());
        return false;
      }
      if (this.m_tradeDockManager.TradeDock.IsNone)
      {
        error = (LocStrFormatted) TrCore.TradeStatus__NoTradeDock;
        return false;
      }
      if (!this.m_tradeDockManager.TradeDock.Value.CanTrade)
      {
        error = (LocStrFormatted) TrCore.TradeStatus__TradeDockNotOperational;
        return false;
      }
      if (this.IsSoldOut)
      {
        error = (LocStrFormatted) TrCore.TradeStatus__SoldOut;
        return false;
      }
      ProductQuantity productToPayWith = this.GetProductToPayWith();
      if (this.m_assetManager.GetAvailableQuantityForRemoval(productToPayWith.Product) < productToPayWith.Quantity)
      {
        error = (LocStrFormatted) TrCore.TradeStatus__CantAfford;
        return false;
      }
      if (!this.m_upointsManager.CanConsume(this.UpointsCost))
      {
        error = (LocStrFormatted) TrCore.TradeStatus__NoUnity;
        return false;
      }
      if (this.m_tradeHandler.HasValue)
      {
        ProductQuantity productToBuy = this.GetProductToBuy();
        return this.m_tradeHandler.Value.CanBuy(productToBuy.Product, productToBuy.Quantity, out error);
      }
      error = LocStrFormatted.Empty;
      return true;
    }

    public Duration GetCooldownForCurrentStep()
    {
      return this.CurrentStep <= 1 ? this.Prototype.CooldownPerStep : (this.Prototype.CooldownPerStep.Ticks / ((Fix32) 1 + 0.25.ToFix32() * (this.CurrentStep - 1))).ToIntRounded().Ticks();
    }

    /// <summary>Returns quantity that was bought or none if failed.</summary>
    public ProductQuantity QuickBuy()
    {
      ProductQuantity productToBuy = this.GetProductToBuy();
      ProductQuantity productToPayWith = this.GetProductToPayWith();
      if (!this.CanAfford(out LocStrFormatted _) || !this.m_assetManager.CanRemoveProduct(productToPayWith))
        return ProductQuantity.None;
      ProductQuantity productQuantity = productToBuy;
      if (this.m_tradeHandler.HasValue)
      {
        this.m_tradeHandler.Value.StoreBoughtProduct(productQuantity.Product, productQuantity.Quantity);
      }
      else
      {
        this.m_productsManager.ProductCreated(productToBuy, CreateReason.QuickTrade);
        this.m_tradeDockManager.TradeDock.Value.StoreProduct(productToBuy);
      }
      Assert.That<bool>(this.m_assetManager.TryRemoveProduct(productToPayWith, new DestroyReason?(DestroyReason.QuickTrade))).IsTrue();
      this.setStep(this.CurrentStep + 1);
      this.m_timerToNextStep.Start(this.GetCooldownForCurrentStep());
      if (this.UpointsCost.IsPositive)
        this.m_upointsManager.ConsumeExactly(IdsCore.UpointsCategories.QuickTrade, this.UpointsCost);
      return productQuantity;
    }

    private void setStep(int newStep)
    {
      this.CurrentStep = newStep;
      this.m_productToPayWithThisStep = this.Prototype.ProductToPayWith.Product.WithQuantity(this.getQuantityInStep(this.Prototype.ProductToPayWith.Quantity, this.CurrentStep));
      this.UpointsCost = this.getUnityInStep(this.Prototype.UpointsPerTrade, this.CurrentStep);
    }

    private Quantity getQuantityInStep(Quantity quantityInFirstStep, int step)
    {
      Quantity quantityInStep = quantityInFirstStep;
      for (int index = 0; index < step / this.Prototype.TradesPerStep; ++index)
        quantityInStep = this.Prototype.CostMultiplierPerStep.Apply(quantityInStep.Value).Quantity();
      return quantityInStep;
    }

    private Upoints getUnityInStep(Upoints upointsInFirstStep, int step)
    {
      Upoints unityInStep = upointsInFirstStep;
      for (int index = 0; index < step / this.Prototype.TradesPerStep; ++index)
        unityInStep = unityInStep.ScaledBy(this.Prototype.UnityMultiplierPerStep);
      return unityInStep;
    }

    static QuickTradeProvider()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      QuickTradeProvider.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((QuickTradeProvider) obj).SerializeData(writer));
      QuickTradeProvider.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((QuickTradeProvider) obj).DeserializeData(reader));
    }
  }
}
