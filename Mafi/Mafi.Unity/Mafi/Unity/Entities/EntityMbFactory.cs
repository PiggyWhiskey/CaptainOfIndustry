// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.EntityMbFactory
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Entities;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities
{
  /// <summary>
  /// Convenience class for creation of <see cref="T:Mafi.Unity.Entities.EntityMb" />.
  /// </summary>
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class EntityMbFactory
  {
    private readonly DependencyResolver m_resolver;

    public EntityMbFactory(DependencyResolver resolver)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_resolver = resolver;
    }

    public EntityMb CreateMbFor<T>(T entity) where T : IEntity
    {
      try
      {
        return this.m_resolver.InvokeFactoryHierarchy<EntityMb>((object) entity);
      }
      catch (Exception ex)
      {
        Log.Exception(ex, "Failed to create MB for entity '" + entity.GetType().Name + "'. Returning empty.");
        GameObject gameObject = new GameObject(string.Format("[empty]{0}#{1}", (object) entity.GetType().Name, (object) entity.Id));
        EmptyEntityMb mbFor = gameObject.AddComponent<EmptyEntityMb>();
        gameObject.AddComponent<SphereCollider>().radius = 1f;
        mbFor.InitializeEmpty();
        return (EntityMb) mbFor;
      }
    }
  }
}
