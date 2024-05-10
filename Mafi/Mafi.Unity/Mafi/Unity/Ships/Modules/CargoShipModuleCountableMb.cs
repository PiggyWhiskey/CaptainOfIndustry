// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Ships.Modules.CargoShipModuleCountableMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Buildings.Cargo.Ships.Modules;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Ships.Modules
{
  internal class CargoShipModuleCountableMb : CargoShipModuleBaseMb
  {
    private GameObject[] m_containerGos;
    private int m_enabledContainersCountSync;
    private int m_enabledContainersCountRender;
    private CargoShipModule m_module;

    public void Initialize(
      AssetsDb assetsDb,
      CargoShipModule module,
      CargoShipCountableModuleProto proto)
    {
      this.m_module = module;
      CargoShipCountableModuleProto.Gfx graphics = proto.Graphics;
      this.m_containerGos = new GameObject[graphics.ContainerPositions.Length];
      this.m_enabledContainersCountSync = this.getEnabledContainersCount();
      this.m_enabledContainersCountRender = this.m_enabledContainersCountSync;
      Transform transform = this.transform;
      for (int index = 0; index < this.m_containerGos.Length; ++index)
      {
        GameObject clonedPrefabOrEmptyGo = assetsDb.GetClonedPrefabOrEmptyGo(graphics.ContainerPrefabPath);
        this.m_containerGos[index] = clonedPrefabOrEmptyGo;
        clonedPrefabOrEmptyGo.transform.SetParent(transform, false);
        clonedPrefabOrEmptyGo.transform.localPosition = graphics.ContainerPositions[index].ToVector3();
        clonedPrefabOrEmptyGo.SetActive(index < this.m_enabledContainersCountRender);
      }
    }

    private int getEnabledContainersCount()
    {
      Assert.That<Quantity>(this.m_module.Quantity).IsLessOrEqual(this.m_module.Capacity);
      return this.m_module.Quantity.Value.CeilDiv(this.m_module.Capacity.Value.CeilDiv(this.m_containerGos.Length)).Min(this.m_containerGos.Length);
    }

    public override void SyncUpdate(GameTime time)
    {
      this.m_enabledContainersCountSync = this.getEnabledContainersCount();
    }

    public override void RenderUpdate(GameTime time)
    {
      if (this.m_enabledContainersCountRender == this.m_enabledContainersCountSync)
        return;
      for (; this.m_enabledContainersCountRender < this.m_enabledContainersCountSync; ++this.m_enabledContainersCountRender)
        this.m_containerGos[this.m_enabledContainersCountRender].SetActive(true);
      while (this.m_enabledContainersCountRender > this.m_enabledContainersCountSync)
      {
        --this.m_enabledContainersCountRender;
        this.m_containerGos[this.m_enabledContainersCountRender].SetActive(false);
      }
      this.m_enabledContainersCountRender = this.m_enabledContainersCountSync;
    }

    public CargoShipModuleCountableMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
    public class Factory : 
      IFactory<CargoShipModule, CargoShipCountableModuleProto, CargoShipModuleBaseMb>
    {
      private readonly AssetsDb m_assetsDb;

      public Factory(AssetsDb assetsDb)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_assetsDb = assetsDb;
      }

      public CargoShipModuleBaseMb Create(
        CargoShipModule module,
        CargoShipCountableModuleProto proto)
      {
        CargoShipModuleCountableMb moduleCountableMb = this.m_assetsDb.GetClonedPrefabOrEmptyGo(module.Prototype.Graphics.PrefabPath).AddComponent<CargoShipModuleCountableMb>();
        moduleCountableMb.Initialize(this.m_assetsDb, module, proto);
        return (CargoShipModuleBaseMb) moduleCountableMb;
      }
    }
  }
}
