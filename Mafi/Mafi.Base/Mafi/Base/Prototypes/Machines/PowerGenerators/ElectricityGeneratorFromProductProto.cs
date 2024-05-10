// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.PowerGenerators.ElectricityGeneratorFromProductProto
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System;

#nullable disable
namespace Mafi.Base.Prototypes.Machines.PowerGenerators
{
  public class ElectricityGeneratorFromProductProto : 
    LayoutEntityProto,
    IProtoWithUiRecipe,
    IRecipeForUi,
    IProtoWithAnimation
  {
    public readonly int GenerationPriority;
    public readonly ProductQuantity InputProduct;
    /// <summary>
    /// Multiplier of input quantity to determine buffer size.
    /// </summary>
    public readonly int BufferCapacityMultiplier;
    /// <summary>Empty if there is no output product</summary>
    public readonly ProductQuantity OutputProduct;
    public readonly DestroyReason ProductDestroyReason;
    public readonly ElectricityGeneratorFromProductProto.Gfx Graphics;

    public override Type EntityType => typeof (ElectricityGeneratorFromProduct);

    public Electricity OutputElectricity { get; }

    public Duration Duration { get; }

    public ImmutableArray<Mafi.Core.Entities.Animations.AnimationParams> AnimationParams { get; }

    public IRecipeForUi Recipe => (IRecipeForUi) this;

    Proto.ID IRecipeForUi.Id => (Proto.ID) this.Id;

    public ImmutableArray<RecipeInput> AllUserVisibleInputs { get; }

    public ImmutableArray<RecipeOutput> AllUserVisibleOutputs { get; }

    public ElectricityGeneratorFromProductProto(
      StaticEntityProto.ID id,
      Proto.Str strings,
      EntityLayout layout,
      EntityCosts costs,
      Electricity outputElectricity,
      int generationPriority,
      ProductQuantity inputProduct,
      ProductQuantity? outputProduct,
      ProductProto electricityProto,
      int bufferCapacityMultiplier,
      Duration duration,
      DestroyReason productDestroyReason,
      ImmutableArray<Mafi.Core.Entities.Animations.AnimationParams> animationParams,
      ElectricityGeneratorFromProductProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, layout, costs, (LayoutEntityProto.Gfx) graphics);
      this.OutputElectricity = outputElectricity.CheckPositive();
      this.GenerationPriority = generationPriority;
      this.InputProduct = inputProduct;
      this.OutputProduct = outputProduct.GetValueOrDefault();
      this.BufferCapacityMultiplier = bufferCapacityMultiplier.CheckPositive();
      this.Duration = duration.CheckPositive();
      this.AnimationParams = animationParams;
      this.ProductDestroyReason = productDestroyReason;
      this.Graphics = graphics;
      this.AllUserVisibleInputs = ImmutableArray.Create<RecipeInput>(new RecipeInput(inputProduct.Product, inputProduct.Quantity));
      this.AllUserVisibleOutputs = !outputProduct.HasValue ? ImmutableArray.Create<RecipeOutput>(new RecipeOutput(electricityProto, outputElectricity.Quantity)) : ImmutableArray.Create<RecipeOutput>(new RecipeOutput(electricityProto, outputElectricity.Quantity), new RecipeOutput(outputProduct.Value.Product, outputProduct.Value.Quantity));
      if (this.OutputProduct.IsNotEmpty && this.OutputProduct.Product.Type != VirtualProductProto.ProductType && this.Ports.Count((Func<IoPortTemplate, bool>) (x => x.Type == IoPortType.Output && x.Shape.AllowedProductType == this.OutputProduct.Product.Type)) == 0)
        throw new ProtoInitException(string.Format("No output port found for '{0}' on {1}", (object) this.OutputProduct.Product, (object) this.Id));
    }

    public new class Gfx : LayoutEntityProto.Gfx
    {
      public static readonly ElectricityGeneratorFromProductProto.Gfx Empty;
      /// <summary>Parameters for particles.</summary>
      public readonly ImmutableArray<ParticlesParams> ParticlesParams;
      /// <summary>The sound the generator makes while it is operating.</summary>
      public readonly Option<string> SoundPrefabPath;

      public Gfx(
        string prefabPath,
        ImmutableArray<ParticlesParams> particlesParams,
        Option<string> soundPrefabPath,
        ImmutableArray<ToolbarCategoryProto> categories,
        Option<string> customIconPath = default (Option<string>),
        bool useSemiInstancedRendering = false)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        string prefabPath1 = prefabPath;
        Option<string> option = customIconPath;
        ImmutableArray<ToolbarCategoryProto>? nullable = new ImmutableArray<ToolbarCategoryProto>?(categories);
        bool flag = useSemiInstancedRendering;
        RelTile3f prefabOrigin = new RelTile3f();
        Option<string> customIconPath1 = option;
        ColorRgba color = new ColorRgba();
        LayoutEntityProto.VisualizedLayers? visualizedLayers = new LayoutEntityProto.VisualizedLayers?();
        ImmutableArray<ToolbarCategoryProto>? categories1 = nullable;
        int num = flag ? 1 : 0;
        ImmutableArray<string> instancedRenderingExcludedObjects = new ImmutableArray<string>();
        // ISSUE: explicit constructor call
        base.\u002Ector(prefabPath1, prefabOrigin, customIconPath1, color, visualizedLayers: visualizedLayers, categories: categories1, useSemiInstancedRendering: num != 0, instancedRenderingExcludedObjects: instancedRenderingExcludedObjects);
        this.ParticlesParams = particlesParams.CheckNotDefaultStruct<ImmutableArray<ParticlesParams>>();
        this.SoundPrefabPath = soundPrefabPath;
      }

      static Gfx()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        Option<string> customIconPath = (Option<string>) "EMPTY";
        ElectricityGeneratorFromProductProto.Gfx.Empty = new ElectricityGeneratorFromProductProto.Gfx("EMPTY", ImmutableArray<ParticlesParams>.Empty, (Option<string>) Option.None, ImmutableArray<ToolbarCategoryProto>.Empty, customIconPath);
      }
    }
  }
}
