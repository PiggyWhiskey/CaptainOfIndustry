// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.Static.TombOfCaptainsMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Base.Prototypes.Buildings;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Entities.Static;
using Mafi.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities.Static
{
  internal class TombOfCaptainsMb : 
    StaticEntityMb,
    IEntityMbWithSyncUpdate,
    IEntityMb,
    IDestroyableEntityMb
  {
    private TombOfCaptains m_tomb;
    private ParticleSystem[] m_allParticles;
    private ParticleSystem.MainModule[] m_allMainModules;
    private int m_lastSpeedMult;
    private Percent m_lastFirePerc;
    private ImmutableArray<GameObject> m_flowers;
    private Percent m_lastFlowersPerc;

    public void Initialize(TombOfCaptains tomb)
    {
      this.Initialize((ILayoutEntity) tomb);
      this.m_tomb = tomb;
      this.m_allParticles = ((IEnumerable<ParticleSystem>) this.GetComponentsInChildren<ParticleSystem>()).Where<ParticleSystem>((Func<ParticleSystem, bool>) (x => x.gameObject.name.StartsWith("fire", StringComparison.OrdinalIgnoreCase))).ToArray<ParticleSystem>();
      this.m_allMainModules = ((IEnumerable<ParticleSystem>) this.m_allParticles).SelectMany<ParticleSystem, ParticleSystem.MainModule>((Func<ParticleSystem, IEnumerable<ParticleSystem.MainModule>>) (x => (IEnumerable<ParticleSystem.MainModule>) x.GetComponentsInChildren<ParticleSystem>().MapArray<ParticleSystem, ParticleSystem.MainModule>((Func<ParticleSystem, ParticleSystem.MainModule>) (x => x.main)))).ToArray<ParticleSystem.MainModule>();
      foreach (ParticleSystem allParticle in this.m_allParticles)
        allParticle.Stop(true);
      Transform transform = this.transform;
      Lyst<GameObject> lyst = new Lyst<GameObject>(128);
      for (int index = 0; index < transform.childCount; ++index)
      {
        Transform child = transform.GetChild(index);
        if (child.gameObject.name.StartsWith("flower", StringComparison.OrdinalIgnoreCase))
        {
          lyst.Add(child.gameObject);
          child.gameObject.SetActive(false);
        }
      }
      XorRsr128PlusGenerator random = new XorRsr128PlusGenerator(RandomGeneratorType.Unrestricted, (ulong) (uint) tomb.Id.Value, (ulong) (uint) tomb.CenterTile.X | (ulong) (uint) tomb.CenterTile.Y << 32);
      lyst.Shuffle((IRandom) random);
      this.m_flowers = lyst.ToImmutableArrayAndClear();
      foreach (GameObject flower in this.m_flowers)
      {
        if (flower.transform.rotation == Quaternion.identity)
          flower.transform.rotation = Quaternion.Euler(0.0f, (float) random.NextInt(360), 0.0f);
      }
    }

    void IEntityMbWithSyncUpdate.SyncUpdate(GameTime time)
    {
      if (this.m_tomb.FireBurningPerc != this.m_lastFirePerc)
      {
        this.m_lastFirePerc = this.m_tomb.FireBurningPerc;
        int num = this.m_lastFirePerc.Apply(this.m_allParticles.Length);
        for (int index = 0; index < this.m_allParticles.Length; ++index)
        {
          if (index < num)
            this.m_allParticles[index].Play(true);
          else
            this.m_allParticles[index].Stop(true);
        }
      }
      if (this.m_lastSpeedMult != time.GameSpeedMult)
      {
        this.m_lastSpeedMult = time.GameSpeedMult;
        float gameSpeedMult = (float) time.GameSpeedMult;
        for (int index = 0; index < this.m_allMainModules.Length; ++index)
          this.m_allMainModules[index].simulationSpeed = gameSpeedMult;
      }
      if (!(this.m_tomb.FlowersPerc != this.m_lastFlowersPerc))
        return;
      this.m_lastFlowersPerc = this.m_tomb.FlowersPerc;
      int num1 = this.m_lastFlowersPerc.Apply(this.m_flowers.Length);
      for (int index = 0; index < this.m_flowers.Length; ++index)
        this.m_flowers[index].SetActive(index < num1);
    }

    public TombOfCaptainsMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_lastSpeedMult = 1;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
