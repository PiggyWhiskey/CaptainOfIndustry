// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Machines.MachineCommandsProcessor
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Factory.WellPumps;
using Mafi.Core.Input;
using Mafi.Core.Population;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Factory.Machines
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class MachineCommandsProcessor : 
    ICommandProcessor<MachineSetRecipeActiveCmd>,
    IAction<MachineSetRecipeActiveCmd>,
    ICommandProcessor<MachineBoostToggleCmd>,
    IAction<MachineBoostToggleCmd>,
    ICommandProcessor<MachineToggleRecipeActiveCmd>,
    IAction<MachineToggleRecipeActiveCmd>,
    ICommandProcessor<ReorderRecipeCmd>,
    IAction<ReorderRecipeCmd>,
    ICommandProcessor<WellPumpAlertSetEnabledCmd>,
    IAction<WellPumpAlertSetEnabledCmd>,
    ICommandProcessor<ClearRecipeProductsCmd>,
    IAction<ClearRecipeProductsCmd>
  {
    public static readonly Upoints COST_TO_DISCARD_PRODUCTS;
    private readonly EntitiesManager m_entitiesManager;
    private readonly ProtosDb m_protosDb;
    private readonly IUpointsManager m_upointsManager;

    public MachineCommandsProcessor(
      EntitiesManager entitiesManager,
      ProtosDb protosDb,
      IUpointsManager upointsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entitiesManager = entitiesManager;
      this.m_protosDb = protosDb;
      this.m_upointsManager = upointsManager;
    }

    public void Invoke(MachineSetRecipeActiveCmd cmd)
    {
      RecipeProto recipe;
      Machine machine;
      string error;
      if (!this.tryGetMachineRecipe(cmd.MachineId, (Proto.ID) cmd.RecipeId, out recipe, out machine, out error))
      {
        cmd.SetResultError(error);
      }
      else
      {
        if (cmd.EnableRecipe)
          machine.AssignRecipe(recipe);
        else
          machine.RemoveAssignedRecipe(recipe);
        cmd.SetResultSuccess();
      }
    }

    public void Invoke(MachineToggleRecipeActiveCmd cmd)
    {
      RecipeProto recipe;
      Machine machine;
      string error;
      if (!this.tryGetMachineRecipe(cmd.MachineId, (Proto.ID) cmd.RecipeId, out recipe, out machine, out error))
      {
        cmd.SetResultError(error);
      }
      else
      {
        if (machine.RecipesAssigned.Contains<RecipeProto>(recipe))
          machine.RemoveAssignedRecipe(recipe);
        else
          machine.AssignRecipe(recipe);
        cmd.SetResultSuccess();
      }
    }

    public void Invoke(MachineBoostToggleCmd cmd)
    {
      Machine entity;
      if (!this.m_entitiesManager.TryGetEntity<Machine>(cmd.MachineId, out entity))
      {
        cmd.SetResultError(string.Format("Unknown machine '{0}'.", (object) cmd.MachineId));
      }
      else
      {
        entity.SetBoosted(!entity.IsBoostRequested);
        cmd.SetResultSuccess();
      }
    }

    public void Invoke(WellPumpAlertSetEnabledCmd cmd)
    {
      Machine entity;
      if (!this.m_entitiesManager.TryGetEntity<Machine>(cmd.WellPumpId, out entity))
        cmd.SetResultError(string.Format("Unknown machine '{0}'.", (object) cmd.WellPumpId));
      else if (!(entity is WellPump wellPump))
      {
        cmd.SetResultError("Machine '" + entity.GetTitle() + "' is not a pump.");
      }
      else
      {
        wellPump.NotifyOnLowReserve = cmd.IsEnabled;
        cmd.SetResultSuccess();
      }
    }

    public void Invoke(ReorderRecipeCmd cmd)
    {
      RecipeProto recipe;
      Machine machine;
      string error;
      if (!this.tryGetMachineRecipe(cmd.MachineId, cmd.RecipeId, out recipe, out machine, out error))
      {
        cmd.SetResultError(error);
      }
      else
      {
        machine.ReorderRecipe(recipe, cmd.IndexDiff);
        cmd.SetResultSuccess();
      }
    }

    private bool tryGetMachineRecipe(
      EntityId machineId,
      Proto.ID recipeId,
      out RecipeProto recipe,
      out Machine machine,
      out string error)
    {
      if (!this.m_protosDb.TryGetProto<RecipeProto>(recipeId, out recipe))
      {
        error = string.Format("Unknown recipe proto '{0}'.", (object) recipeId);
        machine = (Machine) null;
        return false;
      }
      if (!this.m_entitiesManager.TryGetEntity<Machine>(machineId, out machine))
      {
        error = string.Format("Unknown machine '{0}'.", (object) machineId);
        recipe = (RecipeProto) null;
        return false;
      }
      if (!machine.Prototype.Recipes.Contains<RecipeProto>(recipe))
      {
        error = string.Format("Recipe '{0}' is invalid for machine '{1}'.", (object) recipeId, (object) machineId);
        return false;
      }
      error = (string) null;
      return true;
    }

    public void Invoke(ClearRecipeProductsCmd cmd)
    {
      RecipeProto recipe;
      Machine machine;
      string error;
      if (!this.tryGetMachineRecipe(cmd.MachineId, cmd.RecipeId, out recipe, out machine, out error))
        cmd.SetResultError(error);
      else if (!machine.HasClearProductsActionFor((IRecipeForUi) recipe))
        cmd.SetResultError("Clear not allowed");
      else if (!this.m_upointsManager.TryConsume(IdsCore.UpointsCategories.QuickRemove, MachineCommandsProcessor.COST_TO_DISCARD_PRODUCTS))
      {
        cmd.SetResultError("Not enough unity");
      }
      else
      {
        machine.ClearProductsForRecipe((IRecipeForUi) recipe);
        cmd.SetResultSuccess();
      }
    }

    static MachineCommandsProcessor()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      MachineCommandsProcessor.COST_TO_DISCARD_PRODUCTS = 0.1.Upoints();
    }
  }
}
