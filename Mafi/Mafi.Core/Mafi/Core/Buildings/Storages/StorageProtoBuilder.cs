// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Storages.StorageProtoBuilder
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Buildings.Storages
{
  public sealed class StorageProtoBuilder : IProtoBuilder
  {
    public ProtosDb ProtosDb => this.Registrator.PrototypesDb;

    public ProtoRegistrator Registrator { get; }

    public StorageProtoBuilder(ProtoRegistrator registrator)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Registrator = registrator;
    }

    public StorageProtoBuilder.State Start(string name, StaticEntityProto.ID storageId)
    {
      return new StorageProtoBuilder.State(this, storageId, name);
    }

    public class State : LayoutEntityBuilderState<StorageProtoBuilder.State>
    {
      private readonly StaticEntityProto.ID m_protoId;
      private Quantity? m_capacity;
      private Option<StorageProto> m_nextTier;
      private string m_signObjectPath;
      private string m_fluidIndicatorObjectPath;
      private FluidIndicatorGfxParams? m_fluidIndicatorGfxParams;
      private Quantity? m_transferLimit;
      private Duration? m_transferLimitDuration;
      private string m_smoothPileObjectPath;
      private string m_roughPileObjectPath;
      private LoosePileTextureParams? m_pileTextureParams;
      private Electricity m_powerConsumedForProductsExchange;

      public State(StorageProtoBuilder builder, StaticEntityProto.ID protoId, string name)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector((IProtoBuilder) builder, protoId, name);
        this.m_protoId = protoId;
      }

      [MustUseReturnValue]
      public StorageProtoBuilder.State SetCapacity(int capacity)
      {
        this.m_capacity = new Quantity?(new Quantity(capacity).CheckPositive());
        return this;
      }

      [MustUseReturnValue]
      public StorageProtoBuilder.State SetTransferLimit(int quantity, Duration duration)
      {
        this.m_transferLimit = new Quantity?(quantity.Quantity());
        this.m_transferLimitDuration = new Duration?(duration);
        return this;
      }

      [MustUseReturnValue]
      public StorageProtoBuilder.State SetNoTransferLimit()
      {
        this.m_transferLimit = new Quantity?(Quantity.MaxValue);
        this.m_transferLimitDuration = new Duration?(1.Ticks());
        return this;
      }

      [MustUseReturnValue]
      public StorageProtoBuilder.State SetPowerConsumedForProductsExchange(Electricity power)
      {
        this.m_powerConsumedForProductsExchange = power;
        return this;
      }

      [MustUseReturnValue]
      public StorageProtoBuilder.State SetFluidIndicatorGfxParams(
        string signObjectPath,
        string indicatorObjectPath,
        FluidIndicatorGfxParams fluidIndicatorGfxParams)
      {
        this.m_signObjectPath = signObjectPath;
        this.m_fluidIndicatorObjectPath = indicatorObjectPath;
        this.m_fluidIndicatorGfxParams = new FluidIndicatorGfxParams?(fluidIndicatorGfxParams);
        return this;
      }

      [MustUseReturnValue]
      public StorageProtoBuilder.State SetPileGfxParams(
        string smoothPileObjectPath,
        string roughPileObjectPath,
        LoosePileTextureParams pileTextureParams)
      {
        this.m_smoothPileObjectPath = smoothPileObjectPath;
        this.m_roughPileObjectPath = roughPileObjectPath;
        this.m_pileTextureParams = new LoosePileTextureParams?(pileTextureParams);
        return this;
      }

      [MustUseReturnValue]
      public StorageProtoBuilder.State SetNextTier(StorageProto nextTierId)
      {
        this.m_nextTier = (Option<StorageProto>) nextTierId;
        return this;
      }

      public StorageProto BuildAndAdd(ProductType? productType = null)
      {
        return this.AddToDb<StorageProto>(new StorageProto(this.m_protoId, this.Strings, this.LayoutOrThrow, this.ProductsFilterOrThrow, productType, this.ValueOrThrow<Quantity>(this.m_capacity, "Capacity"), this.Costs, this.ValueOrThrow<Quantity>(this.m_transferLimit, "TransferLimit"), this.ValueOrThrow<Duration>(this.m_transferLimitDuration, "TransferLimitDuration"), this.m_powerConsumedForProductsExchange, this.m_nextTier, this.Graphics));
      }

      public StorageProto BuildUnitAndAdd(UnitStorageRackData[] rackData, string rackPrefabPath)
      {
        UnitStorageProto.Gfx graphics = string.IsNullOrEmpty(this.PrefabPath) ? UnitStorageProto.Gfx.Empty : new UnitStorageProto.Gfx(rackData, UnitStorageProductRackPlacementParams.Default, this.PrefabPath, rackPrefabPath, this.PrefabOrigin, this.CustomIconPath, this.MaterialColor, visualizedLayers: new LayoutEntityProto.VisualizedLayers?(this.VisualizedResourcesList), categories: new ImmutableArray<ToolbarCategoryProto>?(this.GetCategoriesOrThrow()));
        return (StorageProto) this.AddToDb<UnitStorageProto>(new UnitStorageProto(this.m_protoId, this.Strings, this.LayoutOrThrow, this.ProductsFilterOrThrow, this.ValueOrThrow<Quantity>(this.m_capacity, "Capacity"), this.Costs, this.ValueOrThrow<Quantity>(this.m_transferLimit, "TransferLimit"), this.ValueOrThrow<Duration>(this.m_transferLimitDuration, "TransferLimitDuration"), this.m_powerConsumedForProductsExchange, this.m_nextTier, graphics));
      }

      public StorageProto BuildAsLooseAndAdd()
      {
        LooseStorageProto.Gfx graphics = string.IsNullOrEmpty(this.PrefabPath) ? LooseStorageProto.Gfx.Empty : new LooseStorageProto.Gfx(this.PrefabPath, this.ValueOrThrow(this.m_smoothPileObjectPath, "SmoothPileObjectPath"), this.ValueOrThrow(this.m_roughPileObjectPath, "RoughPileObjectPath"), this.ValueOrThrow<LoosePileTextureParams>(this.m_pileTextureParams, "PileTextureParams"), this.PrefabOrigin, this.CustomIconPath, this.MaterialColor, visualizedLayers: new LayoutEntityProto.VisualizedLayers?(this.VisualizedResourcesList), categories: new ImmutableArray<ToolbarCategoryProto>?(this.GetCategoriesOrThrow()), useSemiInstancedRendering: this.SemiInstancedRenderingEnabled, instancedRenderingExcludedObjects: this.InstancedRenderingExcludedObjects);
        return (StorageProto) this.AddToDb<LooseStorageProto>(new LooseStorageProto(this.m_protoId, this.Strings, this.LayoutOrThrow, this.ProductsFilterOrThrow, this.ValueOrThrow<Quantity>(this.m_capacity, "Capacity"), this.Costs, this.ValueOrThrow<Quantity>(this.m_transferLimit, "TransferLimit"), this.ValueOrThrow<Duration>(this.m_transferLimitDuration, "TransferLimitDuration"), this.m_powerConsumedForProductsExchange, this.m_nextTier, graphics));
      }

      public StorageProto BuildAsFluidAndAdd()
      {
        FluidStorageProto.Gfx graphics = string.IsNullOrEmpty(this.PrefabPath) ? FluidStorageProto.Gfx.Empty : new FluidStorageProto.Gfx(this.PrefabPath, this.ValueOrThrow(this.m_signObjectPath, "SignObjectPath"), this.ValueOrThrow(this.m_fluidIndicatorObjectPath, "FluidIndicatorObjectPath"), this.ValueOrThrow<FluidIndicatorGfxParams>(this.m_fluidIndicatorGfxParams, "FluidIndicatorParams"), this.PrefabOrigin, this.CustomIconPath, this.MaterialColor, visualizedLayers: new LayoutEntityProto.VisualizedLayers?(this.VisualizedResourcesList), categories: new ImmutableArray<ToolbarCategoryProto>?(this.GetCategoriesOrThrow()));
        return (StorageProto) this.AddToDb<FluidStorageProto>(new FluidStorageProto(this.m_protoId, this.Strings, this.LayoutOrThrow, this.ProductsFilterOrThrow, this.ValueOrThrow<Quantity>(this.m_capacity, "Capacity"), this.Costs, this.ValueOrThrow<Quantity>(this.m_transferLimit, "TransferLimit"), this.ValueOrThrow<Duration>(this.m_transferLimitDuration, "TransferLimitDuration"), this.m_powerConsumedForProductsExchange, this.m_nextTier, graphics));
      }
    }
  }
}
