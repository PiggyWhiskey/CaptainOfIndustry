// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.EntitiesAudioUpdater
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Unity.Audio;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class EntitiesAudioUpdater : IEntityMbUpdater
  {
    private readonly Dict<IEntity, EntityAudioController> m_controllers;
    private readonly EntityAudioManager m_audioManager;

    public EntitiesAudioUpdater(EntityAudioManager audioManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_controllers = new Dict<IEntity, EntityAudioController>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_audioManager = audioManager;
    }

    public void AddOnSyncIfNeeded(EntityMb entityMb)
    {
      IEntity entity1 = entityMb.Entity;
      if (!(entity1 is IEntityWithSound entity2) || !entity2.SoundParams.HasValue)
        return;
      EntityAudioController entityAudioController = new EntityAudioController(entity2, entityMb, this.m_audioManager);
      this.m_controllers.Add(entity1, entityAudioController);
    }

    public void RemoveOnSyncIfNeeded(EntityMb entityMb)
    {
      this.m_controllers.TryRemove(entityMb.Entity, out EntityAudioController _);
    }

    public void RenderUpdate(GameTime time)
    {
      foreach (EntityAudioController entityAudioController in this.m_controllers.Values)
        entityAudioController.RenderUpdate(time);
    }

    public void SyncUpdate(GameTime time)
    {
      foreach (EntityAudioController entityAudioController in this.m_controllers.Values)
        entityAudioController.UpdateSimState();
    }
  }
}
