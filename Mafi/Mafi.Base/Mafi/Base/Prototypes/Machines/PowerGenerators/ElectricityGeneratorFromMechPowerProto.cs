// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.PowerGenerators.ElectricityGeneratorFromMechPowerProto
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System;

#nullable disable
namespace Mafi.Base.Prototypes.Machines.PowerGenerators
{
  public class ElectricityGeneratorFromMechPowerProto : 
    LayoutEntityProto,
    IProtoWithUiRecipe,
    IRecipeForUi,
    IProtoWithAnimation
  {
    public readonly MechPower InputMechPower;
    public readonly int GenerationPriority;
    /// <summary>Utilization where the production reaches zero.</summary>
    public readonly Percent MinUtilization;
    public readonly ElectricityGeneratorFromMechPowerProto.Gfx Graphics;

    public override Type EntityType => typeof (ElectricityGeneratorFromMechPower);

    public Electricity OutputElectricity { get; }

    public ImmutableArray<Mafi.Core.Entities.Animations.AnimationParams> AnimationParams { get; }

    public IRecipeForUi Recipe => (IRecipeForUi) this;

    Proto.ID IRecipeForUi.Id => (Proto.ID) this.Id;

    public ImmutableArray<RecipeInput> AllUserVisibleInputs { get; }

    public ImmutableArray<RecipeOutput> AllUserVisibleOutputs { get; }

    Duration IRecipeForUi.Duration { get; }

    public ElectricityGeneratorFromMechPowerProto(
      StaticEntityProto.ID id,
      Proto.Str strings,
      EntityLayout layout,
      EntityCosts costs,
      ProductProto mechPowerProto,
      MechPower inputMechPower,
      ProductProto electricityProto,
      Electricity outputElectricity,
      int generationPriority,
      Percent minUtilization,
      ImmutableArray<Mafi.Core.Entities.Animations.AnimationParams> animationParams,
      ElectricityGeneratorFromMechPowerProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      this.Mafi\u002ECore\u002EFactory\u002ERecipes\u002EIRecipeForUi\u002EDuration = 1.Ticks();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, layout, costs, (LayoutEntityProto.Gfx) graphics);
      Assert.That<int>(outputElectricity.Value).IsLessOrEqual<Electricity, MechPower>(inputMechPower.Value, "Generator will generate more electricity ({0}) than consumes mechanical power ({1}).", outputElectricity, inputMechPower);
      this.InputMechPower = inputMechPower.CheckPositive();
      this.OutputElectricity = outputElectricity.CheckPositive();
      this.GenerationPriority = generationPriority;
      this.MinUtilization = minUtilization.CheckPositive();
      this.AnimationParams = animationParams;
      this.Graphics = graphics;
      this.AllUserVisibleInputs = ImmutableArray.Create<RecipeInput>(new RecipeInput(mechPowerProto, inputMechPower.Quantity));
      this.AllUserVisibleOutputs = ImmutableArray.Create<RecipeOutput>(new RecipeOutput(electricityProto, outputElectricity.Quantity));
    }

    public new class Gfx : LayoutEntityProto.Gfx
    {
      public static readonly ElectricityGeneratorFromMechPowerProto.Gfx Empty;
      /// <summary>The sound the generator makes while it is operating.</summary>
      public readonly Option<string> SoundPrefabPath;

      public Gfx(
        string prefabPath,
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
        this.SoundPrefabPath = soundPrefabPath;
      }

      static Gfx()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        Option<string> customIconPath = (Option<string>) "EMPTY";
        ElectricityGeneratorFromMechPowerProto.Gfx.Empty = new ElectricityGeneratorFromMechPowerProto.Gfx("EMPTY", (Option<string>) Option.None, ImmutableArray<ToolbarCategoryProto>.Empty, customIconPath);
      }
    }
  }
}
