// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.Static.OreSortingPlantMbFactory
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Buildings.OreSorting;
using Mafi.Unity.Terrain;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities.Static
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class OreSortingPlantMbFactory : 
    IEntityMbFactory<OreSortingPlant>,
    IFactory<OreSortingPlant, EntityMb>
  {
    private readonly ProtoModelFactory m_modelFactory;
    private readonly LooseProductMaterialManager m_looseProductMaterialManager;

    public OreSortingPlantMbFactory(
      AssetsDb assetsDb,
      ProtoModelFactory modelFactory,
      LooseProductMaterialManager looseProductMaterialManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_modelFactory = modelFactory;
      this.m_looseProductMaterialManager = looseProductMaterialManager;
    }

    public EntityMb Create(OreSortingPlant sortingPlant)
    {
      Assert.That<OreSortingPlant>(sortingPlant).IsNotNull<OreSortingPlant>();
      OreSortingPlantMb oreSortingPlantMb = this.m_modelFactory.CreateModelFor<OreSortingPlantProto>(sortingPlant.Prototype).AddComponent<OreSortingPlantMb>();
      oreSortingPlantMb.Initialize(sortingPlant, this.m_looseProductMaterialManager);
      return (EntityMb) oreSortingPlantMb;
    }
  }
}
