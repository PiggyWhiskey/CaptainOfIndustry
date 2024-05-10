// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.Static.SettlementWasteModuleMbFactory
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Buildings.Settlements;
using Mafi.Unity.Terrain;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities.Static
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class SettlementWasteModuleMbFactory : 
    IEntityMbFactory<SettlementWasteModule>,
    IFactory<SettlementWasteModule, EntityMb>
  {
    private readonly ProtoModelFactory m_modelFactory;
    private readonly LooseProductMaterialManager m_looseProductMaterialManager;

    public SettlementWasteModuleMbFactory(
      ProtoModelFactory modelFactory,
      LooseProductMaterialManager looseProductMaterialManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_modelFactory = modelFactory;
      this.m_looseProductMaterialManager = looseProductMaterialManager;
    }

    public EntityMb Create(SettlementWasteModule module)
    {
      Assert.That<SettlementWasteModule>(module).IsNotNull<SettlementWasteModule>();
      SettlementWasteModuleMb settlementWasteModuleMb = this.m_modelFactory.CreateModelFor<SettlementWasteModuleProto>(module.Prototype).AddComponent<SettlementWasteModuleMb>();
      settlementWasteModuleMb.Initialize(module, this.m_looseProductMaterialManager);
      return (EntityMb) settlementWasteModuleMb;
    }
  }
}
