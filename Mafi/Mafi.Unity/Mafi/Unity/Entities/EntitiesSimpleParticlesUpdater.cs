// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.EntitiesSimpleParticlesUpdater
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Entities;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class EntitiesSimpleParticlesUpdater : IEntityMbUpdater
  {
    private readonly Dict<IEntity, SimpleParticlesController> m_controllers;

    public void AddOnSyncIfNeeded(EntityMb entityMb)
    {
      if (!(entityMb.Entity is IEntityWithParticles entity))
        return;
      this.m_controllers.Add((IEntity) entity, new SimpleParticlesController(entity, entityMb.gameObject));
    }

    public void RemoveOnSyncIfNeeded(EntityMb entityMb)
    {
      this.m_controllers.Remove(entityMb.Entity);
    }

    public void RenderUpdate(GameTime time)
    {
    }

    public void SyncUpdate(GameTime time)
    {
      foreach (SimpleParticlesController particlesController in this.m_controllers.Values)
        particlesController.SyncUpdate(time);
    }

    public EntitiesSimpleParticlesUpdater()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_controllers = new Dict<IEntity, SimpleParticlesController>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
