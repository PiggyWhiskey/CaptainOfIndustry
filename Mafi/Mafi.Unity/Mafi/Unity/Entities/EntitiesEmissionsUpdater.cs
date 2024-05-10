// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.EntitiesEmissionsUpdater
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
  public class EntitiesEmissionsUpdater : IEntityMbUpdater
  {
    private readonly Dict<IEntity, EmissionController> m_controllers;

    public void AddOnSyncIfNeeded(EntityMb entityMb)
    {
      IEntity entity1 = entityMb.Entity;
      if (!(entity1 is IEntityWithEmission entity2) || !entity2.EmissionIntensity.HasValue)
        return;
      EmissionController emissionController = new EmissionController(entityMb.gameObject, entity2);
      this.m_controllers.Add(entity1, emissionController);
    }

    public void RemoveOnSyncIfNeeded(EntityMb entityMb)
    {
      this.m_controllers.Remove(entityMb.Entity);
    }

    public void RenderUpdate(GameTime time)
    {
      foreach (EmissionController emissionController in this.m_controllers.Values)
        emissionController.RenderUpdate();
    }

    public void SyncUpdate(GameTime time)
    {
      foreach (EmissionController emissionController in this.m_controllers.Values)
        emissionController.SyncUpdate();
    }

    public EntitiesEmissionsUpdater()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_controllers = new Dict<IEntity, EmissionController>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
