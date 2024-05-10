// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Particles.StackerParticlesController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Factory.Transports;
using Mafi.Core.Products;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Particles
{
  internal class StackerParticlesController
  {
    private readonly Stacker m_stacker;
    private readonly ParticleSystem m_particleSystem;
    private readonly ParticlesParams m_params;
    private readonly ParticleSystem.MainModule[] m_mainModules;
    private bool m_syncCanWork;
    private bool m_pendingUnPause;
    private Option<TerrainMaterialProto> m_lastSeenTerrainMaterial;
    private int m_lastSpeedMult;

    public StackerParticlesController(Stacker stacker, ParticleSystem system, ParticlesParams info)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_lastSpeedMult = 1;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_stacker = stacker.CheckNotNull<Stacker>();
      this.m_particleSystem = system.CheckNotNull<ParticleSystem>();
      this.m_params = info;
      ParticleSystem[] componentsInChildren = this.m_particleSystem.gameObject.GetComponentsInChildren<ParticleSystem>();
      this.m_mainModules = componentsInChildren.MapArray<ParticleSystem, ParticleSystem.MainModule>((Func<ParticleSystem, ParticleSystem.MainModule>) (x => x.main));
      this.m_lastSeenTerrainMaterial = stacker.LastDumpedMaterial;
      TerrainMaterialProto valueOrNull = this.m_lastSeenTerrainMaterial.ValueOrNull;
      Color color = valueOrNull != null ? valueOrNull.Graphics.ParticleColor.AsColor() : Color.black;
      foreach (ParticleSystem particleSystem in componentsInChildren)
        particleSystem.main.startColor = (ParticleSystem.MinMaxGradient) color;
      if (!this.m_stacker.IsDumpingActive)
        return;
      this.play();
      foreach (ParticleSystem particleSystem in componentsInChildren)
      {
        ParticleSystem.MainModule main = particleSystem.main;
        particleSystem.Simulate(main.startLifetime.constant);
      }
      this.pause();
    }

    public void SyncUpdate(GameTime time)
    {
      if (this.m_lastSeenTerrainMaterial != this.m_stacker.LastDumpedMaterial)
      {
        this.m_lastSeenTerrainMaterial = this.m_stacker.LastDumpedMaterial;
        if (this.m_lastSeenTerrainMaterial.HasValue)
        {
          Color color = this.m_lastSeenTerrainMaterial.Value.Graphics.ParticleColor.AsColor();
          for (int index = 0; index < this.m_mainModules.Length; ++index)
            this.m_mainModules[index].startColor = (ParticleSystem.MinMaxGradient) color;
        }
      }
      this.m_syncCanWork = this.m_stacker.IsDumpingActive;
      if (this.m_lastSpeedMult == time.GameSpeedMult)
        return;
      this.m_lastSpeedMult = time.GameSpeedMult;
      for (int index = 0; index < this.m_mainModules.Length; ++index)
        this.m_mainModules[index].simulationSpeed = (float) this.m_lastSpeedMult;
    }

    public void RenderUpdate(GameTime time)
    {
      if (this.handleGamePause(time))
        return;
      if (this.m_syncCanWork)
        this.play();
      else
        this.stop();
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
