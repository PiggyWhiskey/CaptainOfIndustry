// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Buildings.ThermalStorages.ThermalStorageProto
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System;

#nullable disable
namespace Mafi.Base.Prototypes.Buildings.ThermalStorages
{
  public class ThermalStorageProto : 
    LayoutEntityProto,
    IProtoWithUiRecipes,
    IProtoWithPowerConsumption
  {
    public readonly ThermalStorageProto.Gfx Graphics;
    /// <summary>
    /// Capacity is defined as the total quantity of product (e.g. hp steam) this storage can
    /// output. So its thermal capacity changes based on product stored. The reason is that if
    /// this can store 1 ton of 300 degrees salt it can store 1 ton of 600 degrees salt. Also
    /// if we wouldn't have this, pipes would get more advantage for super press steam.
    /// </summary>
    public readonly Quantity Capacity;
    public readonly ImmutableArray<ThermalStorageProto.ProductData> SupportedProducts;
    public readonly VirtualProductProto HeatProduct;
    public readonly ProductProto ProductToCharge;
    public readonly char ProductToChargePort;
    public readonly ProductProto DepletedProduct;
    public readonly char DepletedProductPort;
    public readonly Percent HeatLossPerMonthIfNotOperating;

    public override Type EntityType => typeof (ThermalStorage);

    public Electricity ElectricityConsumed { get; }

    public IIndexable<IRecipeForUi> Recipes { get; private set; }

    public ThermalStorageProto(
      StaticEntityProto.ID id,
      Proto.Str strings,
      EntityLayout layout,
      EntityCosts costs,
      Electricity electricityConsumed,
      Quantity capacity,
      ImmutableArray<ThermalStorageProto.ProductData> supportedProducts,
      VirtualProductProto heatProduct,
      ProductProto productToCharge,
      char productToChargePort,
      ProductProto depletedProduct,
      char depletedProductPort,
      Percent heatLossPerMonthIfNotOperating,
      ThermalStorageProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, layout, costs, (LayoutEntityProto.Gfx) graphics);
      ThermalStorageProto thermalStorageProto = this;
      this.ElectricityConsumed = electricityConsumed;
      this.Capacity = capacity;
      this.SupportedProducts = supportedProducts;
      this.HeatProduct = heatProduct;
      this.ProductToCharge = productToCharge;
      this.ProductToChargePort = productToChargePort;
      this.DepletedProduct = depletedProduct;
      this.DepletedProductPort = depletedProductPort;
      this.HeatLossPerMonthIfNotOperating = heatLossPerMonthIfNotOperating;
      this.Graphics = graphics;
      Lyst<IRecipeForUi> lyst = new Lyst<IRecipeForUi>();
      foreach (ThermalStorageProto.ProductData supportedProduct in supportedProducts)
      {
        ThermalStorageProto.ProductData data = supportedProduct;
        RecipeForUiData recipeForUiData1 = new RecipeForUiData(Duration.OneTick, ImmutableArray.Create<RecipeInput>(new RecipeInput(data.Product, 1.Quantity())), ImmutableArray.Create<RecipeOutput>(new RecipeOutput((ProductProto) heatProduct, data.HeatCreatedPerOneInput.Quantity()), new RecipeOutput(depletedProduct, 1.Quantity())));
        lyst.Add((IRecipeForUi) recipeForUiData1);
        RecipeForUiData recipeForUiData2 = new RecipeForUiData(Duration.OneTick, ImmutableArray.Create<RecipeInput>(new RecipeInput(productToCharge, 1.Quantity()), new RecipeInput((ProductProto) heatProduct, data.HeatConsumedPerOneOutput.Quantity())), ImmutableArray.Create<RecipeOutput>(new RecipeOutput(data.Product, 1.Quantity())));
        lyst.Add((IRecipeForUi) recipeForUiData2);
        if ((Proto) data.Product == (Proto) productToCharge)
          throw new ProtoBuilderException(string.Format("ProductToCharge cannot be equal to {0}!", (object) data.Product));
        if ((Proto) data.Product == (Proto) depletedProduct)
          throw new ProtoBuilderException(string.Format("DepletedProduct cannot be equal to {0}!", (object) data.Product));
        if (this.Ports.Count((Func<IoPortTemplate, bool>) (x => (int) x.Name != (int) productToChargePort && x.Type == IoPortType.Input && x.Spec.Shape.AllowedProductType == data.Product.Type)) == 0)
          throw new ProtoBuilderException(string.Format("No input port for {0}", (object) data.Product));
        if (this.Ports.Count((Func<IoPortTemplate, bool>) (x => (int) x.Name != (int) depletedProductPort && x.Type == IoPortType.Output && x.Spec.Shape.AllowedProductType == data.Product.Type)) == 0)
          throw new ProtoBuilderException(string.Format("No input port for {0}", (object) data.Product));
        if (data.HeatCreatedPerOneInput > data.HeatConsumedPerOneOutput)
          throw new ProtoBuilderException(string.Format("HeatCreatedPerOneInput: {0}", (object) data.HeatCreatedPerOneInput) + string.Format(" > HeatConsumedPerOneOutput {0}", (object) data.HeatConsumedPerOneOutput));
      }
      this.Recipes = (IIndexable<IRecipeForUi>) lyst;
      if (this.Ports.Count((Func<IoPortTemplate, bool>) (x => (int) x.Name == (int) productToChargePort && x.Type == IoPortType.Input && x.Spec.Shape.AllowedProductType == thermalStorageProto.ProductToCharge.Type)) != 1)
        throw new ProtoBuilderException("ProductToCharge port mismatch!");
      if (this.Ports.Count((Func<IoPortTemplate, bool>) (x => (int) x.Name == (int) depletedProductPort && x.Type == IoPortType.Output && x.Spec.Shape.AllowedProductType == thermalStorageProto.DepletedProduct.Type)) != 1)
        throw new ProtoBuilderException("DepletedProduct port mismatch!");
    }

    public struct ProductData
    {
      public readonly ProductProto Product;
      public readonly int HeatConsumedPerOneOutput;
      public readonly int HeatCreatedPerOneInput;

      public ProductData(
        ProductProto product,
        int heatConsumedPerOneOutput,
        int heatCreatedPerOneInput)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        this.Product = product;
        this.HeatConsumedPerOneOutput = heatConsumedPerOneOutput;
        this.HeatCreatedPerOneInput = heatCreatedPerOneInput;
      }
    }

    public new class Gfx : LayoutEntityProto.Gfx
    {
      public static readonly ThermalStorageProto.Gfx Empty;

      public Gfx(
        string prefabPath,
        ImmutableArray<ToolbarCategoryProto> categories,
        Option<string> customIconPath = default (Option<string>))
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        string prefabPath1 = prefabPath;
        Option<string> option = customIconPath;
        ImmutableArray<ToolbarCategoryProto>? nullable = new ImmutableArray<ToolbarCategoryProto>?(categories);
        RelTile3f prefabOrigin = new RelTile3f();
        Option<string> customIconPath1 = option;
        ColorRgba color = new ColorRgba();
        LayoutEntityProto.VisualizedLayers? visualizedLayers = new LayoutEntityProto.VisualizedLayers?();
        ImmutableArray<ToolbarCategoryProto>? categories1 = nullable;
        ImmutableArray<string> instancedRenderingExcludedObjects = new ImmutableArray<string>();
        // ISSUE: explicit constructor call
        base.\u002Ector(prefabPath1, prefabOrigin, customIconPath1, color, visualizedLayers: visualizedLayers, categories: categories1, instancedRenderingExcludedObjects: instancedRenderingExcludedObjects);
      }

      static Gfx()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        Option<string> customIconPath = (Option<string>) "EMPTY";
        ThermalStorageProto.Gfx.Empty = new ThermalStorageProto.Gfx("EMPTY", ImmutableArray<ToolbarCategoryProto>.Empty, customIconPath);
      }
    }
  }
}
