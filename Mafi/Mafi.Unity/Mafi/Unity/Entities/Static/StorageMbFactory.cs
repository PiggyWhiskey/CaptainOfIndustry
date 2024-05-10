// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.Static.StorageMbFactory
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Buildings.Storages;
using Mafi.Core.Entities.Static;
using Mafi.Unity.InstancedRendering;
using Mafi.Unity.Terrain;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities.Static
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class StorageMbFactory : IEntityMbFactory<Storage>, IFactory<Storage, EntityMb>
  {
    private readonly AssetsDb m_assetsDb;
    private readonly ProtoModelFactory m_modelFactory;
    private readonly LooseProductMaterialManager m_looseProductMaterialManager;
    private readonly ProductsRenderer m_productsRenderer;

    public StorageMbFactory(
      AssetsDb assetsDb,
      ProtoModelFactory modelFactory,
      LooseProductMaterialManager looseProductMaterialManager,
      ProductsRenderer productsRenderer)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_assetsDb = assetsDb;
      this.m_modelFactory = modelFactory;
      this.m_looseProductMaterialManager = looseProductMaterialManager;
      this.m_productsRenderer = productsRenderer;
    }

    public EntityMb Create(Storage storage)
    {
      Assert.That<Storage>(storage).IsNotNull<Storage>();
      GameObject modelFor = this.m_modelFactory.CreateModelFor<StorageProto>(storage.Prototype);
      if (storage.Prototype is FluidStorageProto prototype1)
      {
        FluidStorageMb fluidStorageMb = modelFor.AddComponent<FluidStorageMb>();
        fluidStorageMb.Initialize(this.m_assetsDb, storage, prototype1);
        return (EntityMb) fluidStorageMb;
      }
      if (storage.Prototype is LooseStorageProto prototype2)
      {
        LooseStorageMb looseStorageMb = modelFor.AddComponent<LooseStorageMb>();
        looseStorageMb.Initialize(storage, prototype2, this.m_looseProductMaterialManager);
        return (EntityMb) looseStorageMb;
      }
      if (storage.Prototype is UnitStorageProto)
      {
        UnitStorageMb unitStorageMb = modelFor.AddComponent<UnitStorageMb>();
        unitStorageMb.Initialize(this.m_assetsDb, storage, this.m_productsRenderer);
        return (EntityMb) unitStorageMb;
      }
      StaticEntityMb staticEntityMb = modelFor.AddComponent<StaticEntityMb>();
      staticEntityMb.Initialize((ILayoutEntity) storage);
      return (EntityMb) staticEntityMb;
    }
  }
}
