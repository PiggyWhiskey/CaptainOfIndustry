// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.Rockets.RocketEntityMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.SpaceProgram;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities.Rockets
{
  internal class RocketEntityMb : 
    EntityMb,
    IEntityMbWithRenderUpdate,
    IEntityMb,
    IDestroyableEntityMb,
    IEntityMbWithSyncUpdate
  {
    private RocketEntity m_rocketEntity;
    private MbBasedEntitiesRenderer m_entitiesRenderer;
    private Option<IRocketOwner> m_ownerEntity;
    private Vector3 m_futurePosition;
    private bool m_particlesEnabled;
    private ParticleSystem[] m_particleSystems;
    private int m_syncedGameSpeed;

    public void Initialize(RocketEntity entity, MbBasedEntitiesRenderer entitiesRenderer)
    {
      this.Initialize((IEntity) entity);
      this.m_rocketEntity = entity;
      this.m_entitiesRenderer = entitiesRenderer;
      this.updateOwner();
      this.transform.localPosition = this.m_futurePosition;
      this.m_particleSystems = this.GetComponentsInChildren<ParticleSystem>();
      this.m_particlesEnabled = entity.IsLaunched;
      foreach (ParticleSystem particleSystem in this.m_particleSystems)
      {
        if (this.m_particlesEnabled)
        {
          particleSystem.Simulate(particleSystem.main.startLifetime.constant);
          particleSystem.Play();
        }
        else
          particleSystem.Stop();
      }
    }

    void IEntityMbWithSyncUpdate.SyncUpdate(GameTime time)
    {
      if (this.m_rocketEntity.Owner != this.m_ownerEntity)
        this.updateOwner();
      else if (this.m_ownerEntity.IsNone)
        this.m_futurePosition = this.m_rocketEntity.Position.ToVector3();
      bool flag = this.m_rocketEntity.IsLaunched && !this.m_rocketEntity.IsDestroyed;
      if (this.m_particlesEnabled != flag)
      {
        this.m_particlesEnabled = flag;
        if (this.enabled)
        {
          foreach (ParticleSystem particleSystem in this.m_particleSystems)
            particleSystem.Play();
        }
        else
        {
          foreach (ParticleSystem particleSystem in this.m_particleSystems)
            particleSystem.Stop();
        }
      }
      if (!this.m_particlesEnabled || time.GameSpeedMult == this.m_syncedGameSpeed)
        return;
      this.m_syncedGameSpeed = time.GameSpeedMult;
      float syncedGameSpeed = (float) this.m_syncedGameSpeed;
      foreach (ParticleSystem particleSystem in this.m_particleSystems)
        particleSystem.main.simulationSpeed = syncedGameSpeed;
    }

    private void updateOwner()
    {
      Option<IRocketOwner> ownerEntity = this.m_ownerEntity;
      this.m_ownerEntity = this.m_rocketEntity.Owner;
      if (this.m_ownerEntity.HasValue)
      {
        GameObject gameObject = this.m_entitiesRenderer.GetMbFor((IRenderedEntity) this.m_ownerEntity.Value).ValueOrNull?.gameObject;
        if ((Object) gameObject != (Object) null)
        {
          IRocketHoldingEntityMb component;
          if (gameObject.TryGetComponent<IRocketHoldingEntityMb>(out component))
          {
            this.transform.SetParent(component.RocketParent, true);
            Quaternion? localRotation = component.LocalRotation;
            if (localRotation.HasValue)
              this.transform.localRotation = localRotation.Value;
          }
          else
            this.transform.SetParent(gameObject.transform, true);
          this.m_futurePosition = new Vector3(0.0f, this.m_rocketEntity.Prototype.GroundOffset.ToUnityUnits(), 0.0f);
          if (!ownerEntity.IsNone)
            return;
          this.transform.localPosition = this.m_futurePosition;
          return;
        }
        Log.Error("Failed to find MB for rocket owner.");
      }
      this.transform.SetParent((Transform) null, true);
      this.m_futurePosition = this.m_rocketEntity.Position.ToVector3();
    }

    void IEntityMbWithRenderUpdate.RenderUpdate(GameTime time)
    {
      this.transform.localPosition = Vector3.LerpUnclamped(this.transform.localPosition, this.m_futurePosition, time.RelativeT);
    }

    public RocketEntityMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
