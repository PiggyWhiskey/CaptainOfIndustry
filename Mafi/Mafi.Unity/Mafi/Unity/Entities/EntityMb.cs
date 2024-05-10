// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.EntityMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Entities;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities
{
  /// <summary>
  /// Abstract base class for entities handling audio, emissions, and animations.
  /// 
  /// Warning: Do not try to <c>.AddComponent&lt;EntityMb&gt;()</c> as this is abstract class and unity will happily
  /// return null without any error.
  /// </summary>
  public abstract class EntityMb : MonoBehaviour, IEntityMb, IDestroyableEntityMb
  {
    public IEntity Entity { get; private set; }

    public bool IsInitialized { get; private set; }

    public bool IsDestroyed { get; private set; }

    protected void Initialize(IEntity entity)
    {
      Assert.That<bool>(this.IsInitialized).IsFalse("Entity MB already initialized.");
      Assert.That<bool>(this.IsDestroyed).IsFalse("Entity MB is already destroyed.");
      Assert.That<IEntity>(this.Entity).IsNull<IEntity>();
      this.Entity = entity;
      this.IsInitialized = true;
    }

    protected void InitializeEmpty()
    {
      Assert.That<bool>(this.IsInitialized).IsFalse("Entity MB already initialized.");
      this.IsInitialized = true;
    }

    public virtual void Destroy() => this.IsDestroyed = true;

    protected EntityMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
