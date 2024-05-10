// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Farms.FarmProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#nullable disable
namespace Mafi.Core.Buildings.Farms
{
  [DebuggerDisplay("FarmProto: {Id}")]
  public class FarmProto : 
    LayoutEntityProto,
    IProtoWithUpgrade<FarmProto>,
    IProtoWithUpgrade,
    IProto,
    IProtoWithUiRecipes
  {
    /// <summary>
    /// Amount of water collected every day when it is raining.
    /// </summary>
    public readonly PartialProductQuantity WaterCollectedPerDay;
    /// <summary>
    /// Percentage of missing fertility to replenish each day.
    /// </summary>
    public readonly Percent FertilityReplenishPerDay;
    /// <summary>Typically leverage by greenhouses.</summary>
    public readonly Percent YieldMultiplier;
    /// <summary>Increases demand on water and fertilizer.</summary>
    public readonly Percent DemandsMultiplier;
    /// <summary>Whether this farm accepts water and fertilizer.</summary>
    public readonly bool HasIrrigationAndFertilizerSupport;
    /// <summary>
    /// Whether this farm is a greenhouse (some crops might require it).
    /// </summary>
    public readonly bool IsGreenhouse;
    /// <summary>
    /// Water evaporation when there is nothing growing on the farm.
    /// This gets doubled when water buffer is above 50%
    /// </summary>
    public readonly PartialQuantity WaterEvaporationPerDay;
    public readonly FarmProto.Gfx Graphics;

    public override Type EntityType => typeof (Farm);

    public UpgradeData<FarmProto> Upgrade { get; }

    public IUpgradeData UpgradeNonGeneric => (IUpgradeData) this.Upgrade;

    public IIndexable<IRecipeForUi> Recipes { get; private set; }

    public FarmProto(
      StaticEntityProto.ID id,
      Proto.Str strings,
      EntityLayout layout,
      EntityCosts costs,
      PartialProductQuantity waterCollectedPerDay,
      Percent fertilityReplenishPerDay,
      Percent yieldMultiplier,
      Percent demandsMultiplier,
      bool hasIrrigationAndFertilizerSupport,
      bool isGreenhouse,
      PartialQuantity waterEvaporationPerDay,
      Option<FarmProto> nextTier,
      FarmProto.Gfx graphics,
      Duration? constructionDurationPerProduct = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, layout, costs, (LayoutEntityProto.Gfx) graphics, constructionDurationPerProduct);
      this.WaterCollectedPerDay = waterCollectedPerDay;
      this.FertilityReplenishPerDay = fertilityReplenishPerDay.CheckPositive();
      this.YieldMultiplier = yieldMultiplier.CheckPositive();
      this.DemandsMultiplier = demandsMultiplier.CheckPositive();
      this.HasIrrigationAndFertilizerSupport = hasIrrigationAndFertilizerSupport;
      this.IsGreenhouse = isGreenhouse;
      this.WaterEvaporationPerDay = waterEvaporationPerDay.CheckNotNegative();
      this.Upgrade = new UpgradeData<FarmProto>(this, nextTier);
      this.Graphics = graphics;
    }

    protected override void OnInitialize(ProtosDb protosDb)
    {
      IEnumerable<CropProto> cropProtos = protosDb.All<CropProto>().Where<CropProto>((Func<CropProto, bool>) (x => this.IsGreenhouse || !x.RequiresGreenhouse));
      Lyst<IRecipeForUi> lyst1 = new Lyst<IRecipeForUi>();
      ProductProto orThrow = protosDb.GetOrThrow<ProductProto>((Proto.ID) IdsCore.Products.CleanWater);
      foreach (CropProto cropProto in cropProtos)
      {
        if ((this.IsGreenhouse || !cropProto.RequiresGreenhouse) && !cropProto.ProductProduced.IsEmpty)
        {
          Lyst<RecipeInput> lyst2 = new Lyst<RecipeInput>();
          if (cropProto.ConsumedWaterPerDay.IsPositive)
            lyst2.Add(new RecipeInput(orThrow, Quantity.Zero));
          RecipeForUiData recipeForUiData = new RecipeForUiData(cropProto.DaysToGrow.Days(), lyst2.ToImmutableArray(), ImmutableArray.Create<RecipeOutput>(new RecipeOutput(cropProto.ProductProduced.Product, Quantity.Zero)));
          lyst1.Add((IRecipeForUi) recipeForUiData);
        }
      }
      this.Recipes = (IIndexable<IRecipeForUi>) lyst1;
    }

    public new class Gfx : LayoutEntityProto.Gfx
    {
      public static readonly FarmProto.Gfx Empty;
      public readonly ImmutableArray<RelTile2f> CropPositions;
      public readonly Option<string> SprinklerPrefabPath;
      public readonly Option<string> SprinklerSoundPath;

      public Gfx(
        string prefabPath,
        ImmutableArray<RelTile2f> cropPositions,
        Option<string> sprinklerPrefabPath,
        Option<string> sprinklerSoundPath,
        RelTile3f prefabOrigin = default (RelTile3f),
        Option<string> customIconPath = default (Option<string>),
        ColorRgba color = default (ColorRgba),
        bool hideBlockedPortsIcon = false,
        LayoutEntityProto.VisualizedLayers? visualizedLayers = null,
        ImmutableArray<ToolbarCategoryProto>? categories = null,
        bool useInstancedRendering = false,
        bool useSemiInstancedRendering = false,
        bool disableEmptyChildrenStripping = false)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        string prefabPath1 = prefabPath;
        RelTile3f prefabOrigin1 = prefabOrigin;
        Option<string> customIconPath1 = customIconPath;
        ColorRgba color1 = color;
        int num1 = hideBlockedPortsIcon ? 1 : 0;
        LayoutEntityProto.VisualizedLayers? visualizedLayers1 = visualizedLayers;
        ImmutableArray<ToolbarCategoryProto>? categories1 = categories;
        int num2 = useInstancedRendering ? 1 : 0;
        int num3 = useSemiInstancedRendering ? 1 : 0;
        bool flag = disableEmptyChildrenStripping;
        ImmutableArray<string> instancedRenderingExcludedObjects = new ImmutableArray<string>();
        int num4 = flag ? 1 : 0;
        // ISSUE: explicit constructor call
        base.\u002Ector(prefabPath1, prefabOrigin1, customIconPath1, color1, num1 != 0, visualizedLayers1, categories1, num2 != 0, num3 != 0, instancedRenderingExcludedObjects: instancedRenderingExcludedObjects, disableEmptyChildrenStripping: num4 != 0);
        this.CropPositions = cropPositions;
        this.SprinklerPrefabPath = sprinklerPrefabPath;
        this.SprinklerSoundPath = sprinklerSoundPath;
      }

      static Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        FarmProto.Gfx.Empty = new FarmProto.Gfx("EMPTY", ImmutableArray<RelTile2f>.Empty, Option<string>.None, Option<string>.None);
      }
    }
  }
}
