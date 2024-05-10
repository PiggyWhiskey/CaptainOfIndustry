// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Particles.MachineParticlesController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Particles
{
  internal class MachineParticlesController
  {
    private readonly Machine m_machine;
    private readonly ParticleSystem m_particleSystem;
    private readonly ParticlesParams m_params;
    private readonly HybridSet<RecipeProto> m_recipes;
    private readonly ParticleSystem.MainModule[] m_mainModules;
    private Percent m_syncUtilization;
    private bool m_syncCanWork;
    private Percent m_lastSetUtil;
    private bool m_pendingUnPause;
    private Fix32 m_syncTimePerformedInTicks;
    private bool m_isCurrentRecipeAllowed;
    private Option<RecipeProto> m_lastSeenRecipe;
    private int m_lastSpeedMult;
    private readonly int m_delayInTicks;
    private readonly int m_endTimeInTicks;
    private readonly float[] m_initialSpeeds;

    public MachineParticlesController(Machine machine, ParticleSystem system, ParticlesParams info)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_lastSetUtil = Percent.Hundred;
      this.m_lastSpeedMult = 1;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_machine = machine.CheckNotNull<Machine>();
      this.m_particleSystem = system.CheckNotNull<ParticleSystem>();
      this.m_params = info;
      this.m_mainModules = this.m_particleSystem.gameObject.GetComponentsInChildren<ParticleSystem>().MapArray<ParticleSystem, ParticleSystem.MainModule>((Func<ParticleSystem, ParticleSystem.MainModule>) (x => x.main));
      this.m_initialSpeeds = this.m_mainModules.MapArray<ParticleSystem.MainModule, float>((Func<ParticleSystem.MainModule, float>) (x => x.simulationSpeed));
      this.m_recipes = !info.SupportedRecipesSelector.HasValue ? HybridSet<RecipeProto>.Empty : HybridSet<RecipeProto>.From(machine.Prototype.Recipes.Where<RecipeProto>((Func<RecipeProto, bool>) (x => info.SupportedRecipesSelector.Value(x))));
      this.m_delayInTicks = this.m_params.Delay.Ticks;
      int ticks = this.m_params.Duration.Ticks;
      this.m_endTimeInTicks = ticks > 0 ? this.m_delayInTicks + ticks : int.MaxValue;
    }

    public void SyncUpdate(GameTime time)
    {
      this.m_syncTimePerformedInTicks = (Fix32) this.m_machine.RecipeProductionTicks.Ticks;
      this.m_syncUtilization = this.m_machine.Utilization;
      if (this.m_lastSeenRecipe != this.m_machine.LastRecipeInProgress)
      {
        this.m_lastSeenRecipe = this.m_machine.LastRecipeInProgress;
        this.m_isCurrentRecipeAllowed = this.m_recipes.IsEmpty || this.m_lastSeenRecipe.HasValue && this.m_recipes.Contains(this.m_lastSeenRecipe.Value);
        if (this.m_lastSeenRecipe.HasValue && this.m_params.ColorSelector.HasValue)
        {
          Color color = this.m_params.ColorSelector.Value(this.m_machine.LastRecipeInProgress.Value).ToColor();
          for (int index = 0; index < this.m_mainModules.Length; ++index)
            this.m_mainModules[index].startColor = (ParticleSystem.MinMaxGradient) color;
        }
      }
      this.m_syncCanWork = this.m_machine.WorkedThisTick && this.m_isCurrentRecipeAllowed;
      if (this.m_lastSpeedMult == time.GameSpeedMult)
        return;
      this.m_lastSpeedMult = time.GameSpeedMult;
      for (int index = 0; index < this.m_mainModules.Length; ++index)
        this.m_mainModules[index].simulationSpeed = this.m_initialSpeeds[index] * (float) this.m_lastSpeedMult;
    }

    public void RenderUpdate(GameTime time)
    {
      if (this.handleGamePause(time))
        return;
      if (this.m_syncCanWork && this.m_syncTimePerformedInTicks >= this.m_delayInTicks && this.m_syncTimePerformedInTicks < this.m_endTimeInTicks)
      {
        this.play();
        this.setAlphaIfNeeded();
      }
      else
        this.stop();
    }

    private void setAlphaIfNeeded()
    {
      if (!this.m_params.UseUtilizationOnAlpha || this.m_lastSetUtil == this.m_syncUtilization)
        return;
      this.m_lastSetUtil = this.m_syncUtilization;
      float num = this.m_syncUtilization.ToFloat() * 0.5f;
      for (int index = 0; index < this.m_mainModules.Length; ++index)
      {
        Color color = this.m_mainModules[index].startColor.color with
        {
          a = 0.5f + num
        };
        this.m_mainModules[index].startColor = new ParticleSystem.MinMaxGradient(color);
      }
    }

    private bool handleGamePause(GameTime time)
    {
      if (time.IsGamePaused)
      {
        if (this.m_particleSystem.isEmitting)
        {
          this.m_pendingUnPause = this.m_particleSystem.isEmitting;
          this.pause();
        }
        return true;
      }
      if (this.m_pendingUnPause)
      {
        this.play();
        this.m_pendingUnPause = false;
      }
      return false;
    }

    private void play()
    {
      if (this.m_particleSystem.isEmitting)
        return;
      this.m_particleSystem.Play(true);
    }

    private void pause()
    {
      if (this.m_particleSystem.isPaused)
        return;
      this.m_particleSystem.Pause(true);
    }

    private void stop()
    {
      if (!this.m_particleSystem.isEmitting)
        return;
      this.m_particleSystem.Stop(true);
    }
  }
}
