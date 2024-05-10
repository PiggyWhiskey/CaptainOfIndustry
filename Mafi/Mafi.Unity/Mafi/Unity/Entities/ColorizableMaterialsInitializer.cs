// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.ColorizableMaterialsInitializer
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Entities;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class ColorizableMaterialsInitializer : IEntityMbInitializer
  {
    private readonly ColorizableMaterialsCache m_colorizableMaterialsCache;

    public ColorizableMaterialsInitializer(
      ColorizableMaterialsCache colorizableMaterialsCache)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_colorizableMaterialsCache = colorizableMaterialsCache;
    }

    public void InitIfNeeded(EntityMb entityMb)
    {
      Assert.That<bool>(entityMb.IsInitialized).IsTrue<EntityMb>("Entity MB '{0}' is  not initialized.", entityMb);
      IEntity entity = entityMb.Entity;
      if (entity == null)
      {
        Log.Error(string.Format("Failed to init colorizable material: Entity of MB '{0}' is null.", (object) entityMb));
      }
      else
      {
        if (!entity.Prototype.Graphics.Color.IsNotEmpty)
          return;
        this.m_colorizableMaterialsCache.SetColorOfAllColorizableMaterials(entityMb.gameObject, entity.Prototype.Graphics.Color.ToColor());
      }
    }
  }
}
