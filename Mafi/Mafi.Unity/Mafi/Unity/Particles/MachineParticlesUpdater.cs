// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Particles.MachineParticlesUpdater
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Factory.Machines;
using Mafi.Unity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Particles
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class MachineParticlesUpdater : IEntityMbUpdater
  {
    private readonly Dict<IEntity, ImmutableArray<MachineParticlesController>> m_controllers;
    private readonly List<ParticleSystem> m_particleSystemsTmp;
    private readonly Lyst<MachineParticlesController> m_controllersTmp;

    public void AddOnSyncIfNeeded(EntityMb entityMb)
    {
      if (!(entityMb.Entity is Machine entity))
        return;
      this.m_particleSystemsTmp.Clear();
      entityMb.GetComponentsInChildren<ParticleSystem>(this.m_particleSystemsTmp);
      if (this.m_particleSystemsTmp.Count == 0)
        return;
      this.m_controllersTmp.Clear();
      foreach (ParticlesParams particlesParam in entity.Prototype.Graphics.ParticlesParams)
      {
        ParticlesParams particlesParams = particlesParam;
        ParticleSystem system = this.m_particleSystemsTmp.FirstOrDefault<ParticleSystem>((Func<ParticleSystem, bool>) (x => x.name == particlesParams.SystemId));
        if ((UnityEngine.Object) system == (UnityEngine.Object) null)
          Assert.Fail("Could not find particle params for system with id " + particlesParams.SystemId);
        else
          this.m_controllersTmp.Add(new MachineParticlesController(entity, system, particlesParams));
      }
      this.m_particleSystemsTmp.Clear();
      if (!this.m_controllersTmp.IsNotEmpty)
        return;
      this.m_controllers.Add((IEntity) entity, this.m_controllersTmp.ToImmutableArrayAndClear());
    }

    public void RemoveOnSyncIfNeeded(EntityMb entityMb)
    {
      this.m_controllers.TryRemove(entityMb.Entity, out ImmutableArray<MachineParticlesController> _);
    }

    public void RenderUpdate(GameTime time)
    {
      foreach (ImmutableArray<MachineParticlesController> immutableArray in this.m_controllers.Values)
      {
        foreach (MachineParticlesController particlesController in immutableArray)
          particlesController.RenderUpdate(time);
      }
    }

    public void SyncUpdate(GameTime time)
    {
      foreach (ImmutableArray<MachineParticlesController> immutableArray in this.m_controllers.Values)
      {
        foreach (MachineParticlesController particlesController in immutableArray)
          particlesController.SyncUpdate(time);
      }
    }

    public MachineParticlesUpdater()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_controllers = new Dict<IEntity, ImmutableArray<MachineParticlesController>>();
      this.m_particleSystemsTmp = new List<ParticleSystem>();
      this.m_controllersTmp = new Lyst<MachineParticlesController>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
