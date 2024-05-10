// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.Emissions.MachinesEmissionsUpdater
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Factory.Machines;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities.Emissions
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class MachinesEmissionsUpdater : IEntityMbUpdater
  {
    private readonly Dict<IEntity, ImmutableArray<MachinesEmissionsController>> m_controllers;
    private readonly Lyst<MachinesEmissionsController> m_controllersTmp;

    public IReadOnlyDictionary<IEntity, ImmutableArray<MachinesEmissionsController>> Controllers
    {
      get
      {
        return (IReadOnlyDictionary<IEntity, ImmutableArray<MachinesEmissionsController>>) this.m_controllers;
      }
    }

    public void AddOnSyncIfNeeded(EntityMb entityMb)
    {
      if (!(entityMb.Entity is Machine entity))
        return;
      GameObject[] gameObjectArray = new GameObject[entityMb.gameObject.transform.childCount];
      for (int index = 0; index < gameObjectArray.Length; ++index)
        gameObjectArray[index] = entityMb.gameObject.transform.GetChild(index).gameObject;
      this.m_controllersTmp.Clear();
      foreach (EmissionParams emissionsParam in entity.Prototype.Graphics.EmissionsParams)
      {
        EmissionParams emissionParams = emissionsParam;
        Lyst<Material> lyst = new Lyst<Material>();
        if (!entity.Prototype.Graphics.UseSemiInstancedRendering)
        {
          entityMb.gameObject.InstantiateMaterials((Predicate<MeshRenderer>) (x =>
          {
            if (emissionParams.GameObjectsIds.IsEmpty)
              return true;
            return emissionParams.GameObjectsIds.Contains(x.gameObject.name) && x.sharedMaterial.IsKeywordEnabled("_EMISSION");
          }), lyst);
          if (lyst.IsEmpty)
          {
            Log.Error(string.Format("No emissions found for {0}", (object) entity.Prototype.Id));
          }
          else
          {
            ImmutableArray<Material> immutableArray = lyst.Distinct<Material, Material>((Func<Material, Material>) (x => x)).ToImmutableArray<Material>();
            this.m_controllersTmp.Add(new MachinesEmissionsController(entity, immutableArray, emissionParams));
          }
        }
        else
          this.m_controllersTmp.Add(new MachinesEmissionsController(entity, ImmutableArray<Material>.Empty, emissionParams));
      }
      if (!this.m_controllersTmp.IsNotEmpty)
        return;
      this.m_controllers.Add((IEntity) entity, this.m_controllersTmp.ToImmutableArrayAndClear());
    }

    public void RemoveOnSyncIfNeeded(EntityMb entityMb)
    {
      this.m_controllers.TryRemove(entityMb.Entity, out ImmutableArray<MachinesEmissionsController> _);
    }

    public void RenderUpdate(GameTime time)
    {
      foreach (ImmutableArray<MachinesEmissionsController> immutableArray in this.m_controllers.Values)
      {
        foreach (MachinesEmissionsController emissionsController in immutableArray)
          emissionsController.RenderUpdate(time);
      }
    }

    public void SyncUpdate(GameTime time)
    {
      foreach (ImmutableArray<MachinesEmissionsController> immutableArray in this.m_controllers.Values)
      {
        foreach (MachinesEmissionsController emissionsController in immutableArray)
          emissionsController.SyncUpdate(time);
      }
    }

    public MachinesEmissionsUpdater()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_controllers = new Dict<IEntity, ImmutableArray<MachinesEmissionsController>>();
      this.m_controllersTmp = new Lyst<MachinesEmissionsController>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
