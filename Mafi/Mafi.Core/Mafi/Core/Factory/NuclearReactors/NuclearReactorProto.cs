// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.NuclearReactors.NuclearReactorProto
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
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Factory.NuclearReactors
{
  public class NuclearReactorProto : 
    LayoutEntityProto,
    IProtoWithUpgrade<NuclearReactorProto>,
    IProtoWithUpgrade,
    IProto,
    IProtoWithUiRecipes
  {
    public readonly int MaxPowerLevel;
    public readonly ProductQuantity WaterInPerPowerLevel;
    public readonly ProductQuantity SteamOutPerPowerLevel;
    public readonly string WaterInPorts;
    public readonly string SteamOutPorts;
    public readonly Duration ProcessDuration;
    public readonly ImmutableArray<NuclearReactorProto.FuelData> FuelPairs;
    public readonly char FuelInPort;
    public readonly char FuelOutPort;
    public readonly ProductProto CoolantIn;
    public readonly ProductProto CoolantOut;
    public readonly char CoolantInPort;
    public readonly char CoolantOutPort;
    /// <summary>Whether meltdown should cause radiation leak.</summary>
    public readonly bool LeakRadiationOnMeltdown;
    /// <summary>
    /// Whether on meltdown, the fuel will be destroyed instead of just returned as spent fuel.
    /// </summary>
    public readonly bool DestroyFuelOnMeltdown;
    public readonly Option<NuclearReactorProto.EnrichmentData> Enrichment;
    public readonly NuclearReactorProto.Gfx Graphics;

    public override Type EntityType => typeof (NuclearReactor);

    /// <summary>
    /// If this is non-zero, automated regulation using computing is possible, otherwise it is not possible.
    /// </summary>
    public Computing ComputingConsumed { get; }

    public UpgradeData<NuclearReactorProto> Upgrade { get; }

    public IUpgradeData UpgradeNonGeneric => (IUpgradeData) this.Upgrade;

    public IIndexable<IRecipeForUi> Recipes { get; }

    public NuclearReactorProto(
      StaticEntityProto.ID id,
      Proto.Str strings,
      EntityLayout layout,
      EntityCosts costs,
      int maxPowerLevel,
      ProductQuantity waterInPerStep,
      ProductQuantity steamOutPerStep,
      string waterInPorts,
      string steamOutPorts,
      Duration processDuration,
      ImmutableArray<NuclearReactorProto.FuelData> fuelPairs,
      char fuelInPort,
      char fuelOutPort,
      ProductProto coolantIn,
      ProductProto coolantOut,
      char coolantInPort,
      char coolantOutPort,
      bool leakRadiationOnMeltdown,
      bool destroyFuelOnMeltdown,
      Computing computingConsumed,
      Option<NuclearReactorProto.EnrichmentData> enrichment,
      Option<NuclearReactorProto> nextTier,
      NuclearReactorProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, layout, costs, (LayoutEntityProto.Gfx) graphics);
      this.MaxPowerLevel = maxPowerLevel.CheckPositive();
      this.WaterInPerPowerLevel = waterInPerStep;
      this.SteamOutPerPowerLevel = steamOutPerStep;
      this.WaterInPorts = waterInPorts;
      this.SteamOutPorts = steamOutPorts;
      this.ProcessDuration = processDuration.CheckPositive();
      this.FuelPairs = fuelPairs;
      this.FuelInPort = fuelInPort;
      this.FuelOutPort = fuelOutPort;
      this.CoolantIn = coolantIn;
      this.CoolantOut = coolantOut;
      this.CoolantInPort = coolantInPort;
      this.CoolantOutPort = coolantOutPort;
      this.LeakRadiationOnMeltdown = leakRadiationOnMeltdown;
      this.DestroyFuelOnMeltdown = destroyFuelOnMeltdown;
      this.Upgrade = new UpgradeData<NuclearReactorProto>(this, nextTier);
      this.ComputingConsumed = computingConsumed;
      this.Enrichment = enrichment;
      this.Graphics = graphics;
      Lyst<IRecipeForUi> lyst = new Lyst<IRecipeForUi>();
      foreach (NuclearReactorProto.FuelData fuelPair in this.FuelPairs)
      {
        NuclearReactor.Recipe recipe = new NuclearReactor.Recipe(this, fuelPair, this.MaxPowerLevel * Percent.Hundred);
        lyst.Add((IRecipeForUi) recipe);
      }
      Duration duration = lyst.Count > 0 ? lyst.First.Duration : Duration.Zero;
      if (enrichment.HasValue)
      {
        NuclearReactorProto.EnrichmentData enrichmentData = enrichment.Value;
        RecipeForUiData recipeForUiData = new RecipeForUiData(duration, ImmutableArray.Create<RecipeInput>(new RecipeInput(enrichmentData.InputProduct, Quantity.Zero)), ImmutableArray.Create<RecipeOutput>(new RecipeOutput(enrichmentData.OutputProduct, Quantity.Zero)));
        lyst.Add((IRecipeForUi) recipeForUiData);
      }
      this.Recipes = (IIndexable<IRecipeForUi>) lyst;
      ImmutableArray<IoPortTemplate> ports = this.Ports;
      Assert.That<IoPortTemplate>(ports.FirstOrDefault((Func<IoPortTemplate, bool>) (x => x.Type == IoPortType.Input && (int) x.Name == (int) this.CoolantInPort && x.Shape.AllowedProductType == this.CoolantIn.Type))).IsNotNull<IoPortTemplate, char>("Missing or invalid coolant input port with name '{0}'.", this.CoolantInPort);
      ports = this.Ports;
      Assert.That<IoPortTemplate>(ports.FirstOrDefault((Func<IoPortTemplate, bool>) (x => x.Type == IoPortType.Output && (int) x.Name == (int) this.CoolantOutPort && x.Shape.AllowedProductType == this.CoolantOut.Type))).IsNotNull<IoPortTemplate, char>("Missing or invalid coolant input port with name '{0}'.", this.CoolantOutPort);
      ports = this.Ports;
      Assert.That<IoPortTemplate>(ports.FirstOrDefault((Func<IoPortTemplate, bool>) (x => x.Type == IoPortType.Input && (int) x.Name == (int) this.FuelInPort && x.Shape.AllowedProductType == this.FuelPairs.First.FuelInProto.Type))).IsNotNull<IoPortTemplate, char>("Missing or invalid fuel input port with name '{0}'.", this.FuelInPort);
      ports = this.Ports;
      Assert.That<IoPortTemplate>(ports.FirstOrDefault((Func<IoPortTemplate, bool>) (x => x.Type == IoPortType.Output && (int) x.Name == (int) this.FuelOutPort && x.Shape.AllowedProductType == this.FuelPairs.First.SpentFuelOutProto.Type))).IsNotNull<IoPortTemplate, char>("Missing or invalid fuel output port with name '{0}'.", this.FuelOutPort);
      foreach (char waterInPort in waterInPorts)
      {
        char port = waterInPort;
        ports = this.Ports;
        Assert.That<IoPortTemplate>(ports.FirstOrDefault((Func<IoPortTemplate, bool>) (x => x.Type == IoPortType.Input && (int) x.Name == (int) port && x.Shape.AllowedProductType == this.WaterInPerPowerLevel.Product.Type))).IsNotNull<IoPortTemplate, char>("Missing or invalid water input port with name '{0}'.", port);
      }
      foreach (char steamOutPort in steamOutPorts)
      {
        char port = steamOutPort;
        ports = this.Ports;
        Assert.That<IoPortTemplate>(ports.FirstOrDefault((Func<IoPortTemplate, bool>) (x => x.Type == IoPortType.Output && (int) x.Name == (int) port && x.Shape.AllowedProductType == this.SteamOutPerPowerLevel.Product.Type))).IsNotNull<IoPortTemplate, char>("Missing or invalid steam output port with name '{0}'.", port);
      }
    }

    public new class Gfx : LayoutEntityProto.Gfx
    {
      public static readonly NuclearReactorProto.Gfx Empty;
      public readonly Option<string> SoundPrefabPath;
      public readonly Option<string> FuelIconPath;
      public readonly float MaxEmissionIntensity;

      public Gfx(
        string prefabPath,
        ImmutableArray<ToolbarCategoryProto> categories,
        Option<string> soundPrefabPath,
        Option<string> fuelIconPath,
        float maxEmissionIntensity,
        Option<string> customIconPath = default (Option<string>))
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
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
        this.SoundPrefabPath = soundPrefabPath;
        this.FuelIconPath = fuelIconPath;
        this.MaxEmissionIntensity = maxEmissionIntensity;
      }

      static Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        Option<string> customIconPath = (Option<string>) "EMPTY";
        Option<string> none1 = Option<string>.None;
        Option<string> none2 = Option<string>.None;
        NuclearReactorProto.Gfx.Empty = new NuclearReactorProto.Gfx("EMPTY", ImmutableArray<ToolbarCategoryProto>.Empty, none1, none2, 0.0f, customIconPath);
      }
    }

    [GenerateSerializer(false, null, 0)]
    public readonly struct FuelData
    {
      public readonly ProductProto FuelInProto;
      public readonly ProductProto SpentFuelOutProto;
      /// <summary>How long the fuel lasts (at power level 1).</summary>
      public readonly Duration Duration;

      public bool IsEmpty => this.Duration.IsZero;

      public FuelData(ProductProto fuelInProto, ProductProto spentFuelOutProto, Duration duration)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.FuelInProto = fuelInProto;
        this.SpentFuelOutProto = spentFuelOutProto;
        this.Duration = duration;
      }

      public static void Serialize(NuclearReactorProto.FuelData value, BlobWriter writer)
      {
        writer.WriteGeneric<ProductProto>(value.FuelInProto);
        writer.WriteGeneric<ProductProto>(value.SpentFuelOutProto);
        Duration.Serialize(value.Duration, writer);
      }

      public static NuclearReactorProto.FuelData Deserialize(BlobReader reader)
      {
        return new NuclearReactorProto.FuelData(reader.ReadGenericAs<ProductProto>(), reader.ReadGenericAs<ProductProto>(), Duration.Deserialize(reader));
      }
    }

    public sealed class EnrichmentData
    {
      public readonly ProductProto InputProduct;
      public readonly char InPort;
      public readonly ProductProto OutputProduct;
      public readonly char OutPort;
      /// <summary>
      /// How much input gets converted into output per 1 processing step. This helps to define that
      /// e.g. if we spend 1 fuel per step, we also create 1.2 enriched fuel. Note that we currently
      /// don't support consuming fuel to pay for the enrichment, so enrichment is basically free (or
      /// it has to be already factored into the fuel economy of the reactor).
      /// </summary>
      public readonly PartialQuantity ProcessedPerLevel;
      public readonly Quantity BuffersCapacity;
      /// <summary>
      ///  Whether InputProduct and OutputProduct products should be destroyed on meltdown.
      /// </summary>
      public readonly bool DestroyContentOnMeltdown;

      public EnrichmentData(
        ProductProto inputProduct,
        char inPort,
        ProductProto outputProduct,
        char outPort,
        PartialQuantity processedPerLevel,
        Quantity buffersCapacity,
        bool destroyContentOnMeltdown)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.InputProduct = inputProduct;
        this.InPort = inPort;
        this.OutputProduct = outputProduct;
        this.OutPort = outPort;
        this.ProcessedPerLevel = processedPerLevel;
        this.BuffersCapacity = buffersCapacity;
        this.DestroyContentOnMeltdown = destroyContentOnMeltdown;
      }
    }
  }
}
