// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.SimpleParticlesController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Entities;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities
{
  public class SimpleParticlesController
  {
    private readonly IEntityWithParticles m_entity;
    private readonly ParticleSystem[] m_particleSystems;
    private readonly ParticleSystem.MainModule[] m_mainModules;
    private bool m_areParticlesEnabled;
    private int m_lastSpeedMult;
    private readonly float[] m_initialSpeeds;

    public SimpleParticlesController(IEntityWithParticles entity, GameObject go)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_lastSpeedMult = 1;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entity = entity;
      this.m_particleSystems = go.GetComponentsInChildren<ParticleSystem>();
      this.m_mainModules = this.m_particleSystems.MapArray<ParticleSystem, ParticleSystem.MainModule>((Func<ParticleSystem, ParticleSystem.MainModule>) (x => x.main));
      this.m_initialSpeeds = this.m_mainModules.MapArray<ParticleSystem.MainModule, float>((Func<ParticleSystem.MainModule, float>) (x => x.simulationSpeed));
      foreach (ParticleSystem particleSystem in this.m_particleSystems)
        particleSystem.Stop(false);
    }

    public void SyncUpdate(GameTime time)
    {
      if (this.m_areParticlesEnabled != this.m_entity.AreParticlesEnabled)
      {
        this.m_areParticlesEnabled = this.m_entity.AreParticlesEnabled;
        if (this.m_areParticlesEnabled)
        {
          foreach (ParticleSystem particleSystem in this.m_particleSystems)
            particleSystem.Play(false);
        }
        else
        {
          foreach (ParticleSystem particleSystem in this.m_particleSystems)
            particleSystem.Stop(false);
        }
      }
      if (this.m_lastSpeedMult == time.GameSpeedMult)
        return;
      this.m_lastSpeedMult = time.GameSpeedMult;
      for (int index = 0; index < this.m_mainModules.Length; ++index)
        this.m_mainModules[index].simulationSpeed = this.m_initialSpeeds[index] * (float) this.m_lastSpeedMult;
    }
  }
}
