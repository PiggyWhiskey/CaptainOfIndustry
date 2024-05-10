// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Farms.FarmCommandsProcessor
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Input;
using Mafi.Core.Prototypes;
using System;
using System.Linq;

#nullable disable
namespace Mafi.Core.Buildings.Farms
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class FarmCommandsProcessor : 
    ICommandProcessor<FarmAssignCropCmd>,
    IAction<FarmAssignCropCmd>,
    ICommandProcessor<FarmForceNextSlotCmd>,
    IAction<FarmForceNextSlotCmd>,
    ICommandProcessor<FarmSetFertilityTargetCmd>,
    IAction<FarmSetFertilityTargetCmd>,
    ICommandProcessor<FarmHarvestNowCmd>,
    IAction<FarmHarvestNowCmd>,
    ICommandProcessor<ToggleFullBufferNotificationCmd>,
    IAction<ToggleFullBufferNotificationCmd>,
    ICommandProcessor<AnimalFarmSetSlaughterLimitCmd>,
    IAction<AnimalFarmSetSlaughterLimitCmd>,
    ICommandProcessor<MoveAnimalsCmd>,
    IAction<MoveAnimalsCmd>,
    ICommandProcessor<AnimalFarmToggleGrowthPauseCmd>,
    IAction<AnimalFarmToggleGrowthPauseCmd>
  {
    private readonly IEntitiesManager m_entitiesManager;
    private readonly ProtosDb m_protosDb;

    public FarmCommandsProcessor(IEntitiesManager entitiesManager, ProtosDb protosDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entitiesManager = entitiesManager;
      this.m_protosDb = protosDb;
    }

    private bool tryGetFarm(EntityId id, out Farm farm, out string error)
    {
      if (!this.m_entitiesManager.TryGetEntity<Farm>(id, out farm))
      {
        error = string.Format("Failed to get farm with ID {0}.", (object) id);
        farm = (Farm) null;
        return false;
      }
      error = "";
      return true;
    }

    private bool tryGetAnimalFarm(EntityId id, out AnimalFarm farm, out string error)
    {
      if (!this.m_entitiesManager.TryGetEntity<AnimalFarm>(id, out farm))
      {
        error = string.Format("Failed to get farm with ID {0}.", (object) id);
        farm = (AnimalFarm) null;
        return false;
      }
      error = "";
      return true;
    }

    public void Invoke(FarmAssignCropCmd cmd)
    {
      Farm farm;
      string error;
      if (!this.tryGetFarm(cmd.FarmId, out farm, out error))
      {
        cmd.SetResultError(error);
      }
      else
      {
        Option<CropProto> crop;
        if (cmd.CropId.HasValue)
        {
          CropProto proto;
          if (!this.m_protosDb.TryGetProto<CropProto>(cmd.CropId.Value, out proto))
          {
            cmd.SetResultError(string.Format("Failed to find crop with ID {0}", (object) proto.Id));
            return;
          }
          crop = (Option<CropProto>) proto;
        }
        else
          crop = (Option<CropProto>) Option.None;
        farm.AssignCropToSlot(crop, cmd.ScheduleSlot);
        cmd.SetResultSuccess();
      }
    }

    public void Invoke(FarmForceNextSlotCmd cmd)
    {
      Farm farm;
      string error;
      if (!this.tryGetFarm(cmd.FarmId, out farm, out error))
      {
        cmd.SetResultError(error);
      }
      else
      {
        farm.ForceNextSlot();
        cmd.SetResultSuccess();
      }
    }

    public void Invoke(FarmSetFertilityTargetCmd cmd)
    {
      Farm farm;
      string error;
      if (!this.tryGetFarm(cmd.FarmId, out farm, out error))
      {
        cmd.SetResultError(error);
      }
      else
      {
        farm.SetFertilityTarget(cmd.Target);
        cmd.SetResultSuccess();
      }
    }

    public void Invoke(FarmHarvestNowCmd cmd)
    {
      Farm farm;
      string error;
      if (!this.tryGetFarm(cmd.FarmId, out farm, out error))
      {
        cmd.SetResultError(error);
      }
      else
      {
        farm.HarvestNow();
        cmd.SetResultSuccess();
      }
    }

    public void Invoke(ToggleFullBufferNotificationCmd cmd)
    {
      Farm farm;
      string error;
      if (!this.tryGetFarm(cmd.FarmId, out farm, out error))
      {
        cmd.SetResultError(error);
      }
      else
      {
        farm.NotifyOnFullBuffer = !farm.NotifyOnFullBuffer;
        cmd.SetResultSuccess();
      }
    }

    public void Invoke(AnimalFarmSetSlaughterLimitCmd cmd)
    {
      AnimalFarm farm;
      string error;
      if (!this.tryGetAnimalFarm(cmd.AnimalFarmId, out farm, out error))
      {
        cmd.SetResultError(error);
      }
      else
      {
        farm.SetSlaughterStep(cmd.SlaughterSliderStep);
        cmd.SetResultSuccess();
      }
    }

    public void Invoke(AnimalFarmToggleGrowthPauseCmd cmd)
    {
      AnimalFarm farm;
      string error;
      if (!this.tryGetAnimalFarm(cmd.AnimalFarmId, out farm, out error))
      {
        cmd.SetResultError(error);
      }
      else
      {
        farm.ToggleGrowthPause();
        cmd.SetResultSuccess();
      }
    }

    public void Invoke(MoveAnimalsCmd cmd)
    {
      string error;
      AnimalFarm farm;
      if (!this.tryGetAnimalFarm(cmd.AnimalFarmId, out farm, out error))
        cmd.SetResultError(error);
      else if (cmd.NumberToMove > 0 && farm.IsNotEnabled)
        cmd.SetResultSuccess();
      else if (cmd.NumberToMove == 0)
      {
        cmd.SetResultSuccess();
      }
      else
      {
        AnimalFarm[] array = this.m_entitiesManager.GetAllEntitiesOfType<AnimalFarm>().Where<AnimalFarm>((Func<AnimalFarm, bool>) (x => (Proto) x.Prototype.Animal == (Proto) farm.Prototype.Animal && x.IsConstructed && x != farm && x.IsEnabled)).ToArray<AnimalFarm>();
        if (array.IsEmpty<AnimalFarm>())
        {
          cmd.SetResultSuccess();
        }
        else
        {
          if (cmd.NumberToMove < 0)
          {
            int num1 = farm.AnimalsCount.Min(cmd.NumberToMove.Abs());
            if (num1 <= 0)
            {
              cmd.SetResultSuccess();
              return;
            }
            int num2 = 0;
            while (num1 > 0 && num2 != num1)
            {
              num2 = num1;
              foreach (AnimalFarm animalFarm in array)
              {
                int count = animalFarm.AddAnimals(1);
                farm.RemoveAnimals(count);
                num1 -= count;
                if (num1 <= 0)
                  break;
              }
            }
          }
          else
          {
            int num3 = (farm.Prototype.AnimalsCapacity - farm.AnimalsCount).Min(cmd.NumberToMove);
            if (num3 <= 0)
            {
              cmd.SetResultSuccess();
              return;
            }
            int num4 = 0;
            while (num3 > 0 && num4 != num3)
            {
              num4 = num3;
              foreach (AnimalFarm animalFarm in array)
              {
                int count = animalFarm.RemoveAnimals(1);
                farm.AddAnimals(count);
                num3 -= count;
                if (num3 <= 0)
                  break;
              }
            }
          }
          cmd.SetResultSuccess();
        }
      }
    }
  }
}
