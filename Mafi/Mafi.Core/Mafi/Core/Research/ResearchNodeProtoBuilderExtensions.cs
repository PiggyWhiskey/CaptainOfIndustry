// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Research.ResearchNodeProtoBuilderExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Buildings.Farms;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Factory.Transports;
using Mafi.Core.Fleet;
using Mafi.Core.Population.Edicts;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Trees;
using Mafi.Core.UnlockingTree;
using System;
using System.Linq;

#nullable disable
namespace Mafi.Core.Research
{
  public static class ResearchNodeProtoBuilderExtensions
  {
    public static ResearchNodeProtoBuilder.State AddVehicleCapIncrease(
      this ResearchNodeProtoBuilder.State builderState,
      int vehicleLimitIncrease,
      string iconPath)
    {
      builderState.AddUnit((IUnlockNodeUnit) new VehicleLimitIncreaseUnlock(vehicleLimitIncrease, iconPath));
      builderState.AddIcon(Option<Proto>.None, iconPath);
      return builderState;
    }

    public static ResearchNodeProtoBuilder.State AddRecyclingRatioIncrease(
      this ResearchNodeProtoBuilder.State builderState,
      Percent recyclingIncrease,
      string iconPath)
    {
      builderState.AddUnit((IUnlockNodeUnit) new RecyclingRatioIncreaseUnlock(recyclingIncrease, iconPath));
      builderState.AddIcon(Option<Proto>.None, iconPath);
      return builderState;
    }

    /// <summary>
    /// Adds proto that will be unlocked by the research node.
    /// </summary>
    public static ResearchNodeProtoBuilder.State AddLayoutEntityToUnlock(
      this ResearchNodeProtoBuilder.State builderState,
      StaticEntityProto.ID protoId,
      bool hideInUi = false)
    {
      return builderState.AddProtoToUnlock<LayoutEntityProto>((Proto.ID) protoId, hideInUi);
    }

    /// <summary>
    /// Adds proto that will be unlocked by the research node.
    /// </summary>
    public static ResearchNodeProtoBuilder.State AddEdictToUnlock(
      this ResearchNodeProtoBuilder.State builderState,
      Proto.ID protoId)
    {
      return builderState.AddProtoToUnlock<EdictProto>(protoId);
    }

    /// <summary>
    /// Adds proto that will be unlocked by the research node.
    /// </summary>
    public static ResearchNodeProtoBuilder.State AddTechnologyToUnlock(
      this ResearchNodeProtoBuilder.State builderState,
      Proto.ID protoId)
    {
      return builderState.AddProtoToUnlock<TechnologyProto>(protoId);
    }

    /// <summary>
    /// Adds proto that will be unlocked by the research node.
    /// </summary>
    public static ResearchNodeProtoBuilder.State AddMachineToUnlock(
      this ResearchNodeProtoBuilder.State builderState,
      MachineProto.ID protoId,
      bool unlockAllRecipes = false)
    {
      if (unlockAllRecipes)
        builderState.AddAllRecipesOfMachineToUnlock(protoId);
      builderState.AddLayoutEntityToUnlock((StaticEntityProto.ID) protoId);
      return builderState;
    }

    /// <summary>
    /// Adds proto that will be unlocked by the research node.
    /// </summary>
    public static ResearchNodeProtoBuilder.State AddCropToUnlock(
      this ResearchNodeProtoBuilder.State builderState,
      Proto.ID protoId)
    {
      return builderState.AddProtoToUnlock<CropProto>(protoId);
    }

    /// <summary>
    /// Adds proto that will be unlocked by the research node.
    /// </summary>
    public static ResearchNodeProtoBuilder.State AddSurfaceToUnlock(
      this ResearchNodeProtoBuilder.State builderState,
      Proto.ID protoId)
    {
      return builderState.AddProtoToUnlock<TerrainTileSurfaceProto>(protoId);
    }

    /// <summary>
    /// Adds proto that will be unlocked by the research node.
    /// </summary>
    public static ResearchNodeProtoBuilder.State AddShipPartToUnlock(
      this ResearchNodeProtoBuilder.State builderState,
      FleetEntityPartProto.ID protoId)
    {
      return builderState.AddProtoToUnlock<FleetEntityPartProto>((Proto.ID) protoId);
    }

    /// <summary>
    /// Adds proto that will be unlocked by the research node.
    /// </summary>
    public static ResearchNodeProtoBuilder.State AddProductToUnlock(
      this ResearchNodeProtoBuilder.State builderState,
      ProductProto.ID protoId,
      bool addIconToNode = false,
      bool hideInUi = false)
    {
      ProductProto orThrow = builderState.Builder.ProtosDb.GetOrThrow<ProductProto>((Proto.ID) protoId);
      if (addIconToNode)
        builderState.AddIcon((Option<Proto>) (Proto) orThrow, orThrow.Graphics.IconPath);
      builderState.AddUnit((IUnlockNodeUnit) new ProductUnlock(orThrow, hideInUi));
      return builderState;
    }

    public static ResearchNodeProtoBuilder.State AddTreeToUnlock(
      this ResearchNodeProtoBuilder.State builderState,
      Proto.ID protoId,
      bool hideInUi = false)
    {
      return builderState.AddProtoToUnlock<TreeProto>(protoId, hideInUi);
    }

    /// <summary>
    /// Adds proto that will be unlocked by the research node.
    /// </summary>
    public static ResearchNodeProtoBuilder.State AddRecipeToUnlock(
      this ResearchNodeProtoBuilder.State builderState,
      RecipeProto.ID protoId,
      ProductProto.ID significantProductId)
    {
      builderState.AddProductIcon(significantProductId);
      return builderState.AddRecipeToUnlock(protoId);
    }

    /// <summary>
    /// Adds proto that will be unlocked by the research node.
    /// </summary>
    public static ResearchNodeProtoBuilder.State AddRecipeToUnlock(
      this ResearchNodeProtoBuilder.State builderState,
      RecipeProto.ID protoId,
      bool hideInUi = false)
    {
      RecipeProto proto = builderState.Builder.ProtosDb.GetOrThrow<RecipeProto>((Proto.ID) protoId);
      MachineProto machineProto = builderState.Builder.ProtosDb.All<MachineProto>().FirstOrDefault<MachineProto>((Func<MachineProto, bool>) (x => x.Recipes.Contains<RecipeProto>(proto)));
      if ((Proto) machineProto == (Proto) null)
        throw new InvalidOperationException(string.Format("No machine that can execute {0} the given recipe was found", (object) proto.Id));
      if (!builderState.Units.Any<IUnlockNodeUnit>((Func<IUnlockNodeUnit, bool>) (x => x is RecipeUnlock recipeUnlock && (Proto) recipeUnlock.Proto == (Proto) proto)))
        return builderState.AddUnit((IUnlockNodeUnit) new RecipeUnlock(proto, machineProto, hideInUi));
      Log.Error(string.Format("Adding a same recipe '{0}' twice!", (object) protoId));
      return builderState;
    }

    /// <summary>
    /// Adds proto that will be unlocked by the research node.
    /// </summary>
    public static ResearchNodeProtoBuilder.State AddAllRecipesOfMachineToUnlock(
      this ResearchNodeProtoBuilder.State builderState,
      MachineProto.ID protoId)
    {
      foreach (RecipeProto recipe in builderState.Builder.ProtosDb.GetOrThrow<MachineProto>((Proto.ID) protoId).Recipes)
      {
        if (!recipe.IsObsolete)
          builderState.AddRecipeToUnlock(recipe.Id);
      }
      return builderState;
    }

    /// <summary>
    /// Adds proto that will be unlocked by the research node.
    /// </summary>
    public static ResearchNodeProtoBuilder.State AddTransportToUnlock(
      this ResearchNodeProtoBuilder.State builderState,
      StaticEntityProto.ID protoId)
    {
      return builderState.AddProtoToUnlock<TransportProto>((Proto.ID) protoId);
    }

    /// <summary>
    /// Adds proto that will be unlocked by the research node.
    /// </summary>
    public static ResearchNodeProtoBuilder.State AddVehicleToUnlock(
      this ResearchNodeProtoBuilder.State builderState,
      DynamicEntityProto.ID protoId)
    {
      return builderState.AddProtoToUnlock<DynamicEntityProto>((Proto.ID) protoId);
    }

    public static ResearchNodeProtoBuilder.State AddProtoUnlockNoIcon(
      this ResearchNodeProtoBuilder.State builderState,
      Proto.ID protoId,
      bool hideInUi = false)
    {
      Proto orThrow = builderState.Builder.ProtosDb.GetOrThrow<Proto>(protoId);
      builderState.AddUnit((IUnlockNodeUnit) new ProtoUnlock((IProto) orThrow, hideInUi));
      return builderState;
    }

    public static ResearchNodeProtoBuilder.State AddProtoToUnlock<T>(
      this ResearchNodeProtoBuilder.State builderState,
      Proto.ID protoId,
      bool hideInUi = false)
      where T : IProtoWithIcon, Proto
    {
      T orThrow = builderState.Builder.ProtosDb.GetOrThrow<T>(protoId);
      if (!string.IsNullOrEmpty(orThrow.IconPath))
        builderState.AddIcon((Option<Proto>) (Proto) orThrow, orThrow.IconPath);
      builderState.AddUnit((IUnlockNodeUnit) new ProtoWithIconUnlock((IProtoWithIcon) orThrow, hideInUi));
      return builderState;
    }
  }
}
