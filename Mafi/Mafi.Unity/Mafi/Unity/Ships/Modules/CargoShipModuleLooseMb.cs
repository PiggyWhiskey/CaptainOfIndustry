// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Ships.Modules.CargoShipModuleLooseMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Buildings.Cargo.Ships.Modules;
using Mafi.Core.Products;
using Mafi.Unity.Terrain;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Ships.Modules
{
  internal class CargoShipModuleLooseMb : CargoShipModuleBaseMb
  {
    private CargoShipModule m_module;
    private CargoShipLooseModuleProto m_proto;
    private Option<Transform> m_activePileTransform;
    private GameObject m_smoothPile;
    private GameObject m_roughPile;
    private LooseProductMultiTextureSetter m_pileMaterialSetter;
    private Option<ProductProto> m_storedProductSync;
    private Quantity m_storedQuantitySync;
    private Quantity m_capacitySync;
    private Option<ProductProto> m_storedProductRender;
    private Quantity m_storedQuantityRender;
    private Quantity m_capacityRender;

    public void Initialize(
      CargoShipModule module,
      CargoShipLooseModuleProto proto,
      LooseProductMaterialManager looseProductMaterialManager)
    {
      this.m_module = module;
      this.m_proto = proto;
      Lyst<Renderer> renderers = new Lyst<Renderer>(2);
      setPile(proto.Graphics.SmoothPilePath, out this.m_smoothPile);
      setPile(proto.Graphics.RoughPilePath, out this.m_roughPile);
      this.m_pileMaterialSetter = looseProductMaterialManager.SetupSharedMaterialFor(renderers.Distinct<Renderer>().ToArray<Renderer>());

      void setPile(string pileName, out GameObject go)
      {
        if (this.gameObject.TryFindChild(pileName, out go))
        {
          Renderer component;
          if (go.TryGetComponent<Renderer>(out component))
            renderers.Add(component);
          else
            Log.Warning(string.Format("Pile '{0}' does not have renderer on ship module '{1}'.", (object) pileName, (object) proto.Id));
        }
        else
        {
          Log.Warning(string.Format("Failed to find pile '{0}' on ship module '{1}'.", (object) pileName, (object) proto.Id));
          go = new GameObject("Dummy pile");
        }
        go.SetActive(false);
      }
    }

    public override void SyncUpdate(GameTime time)
    {
      this.m_storedProductSync = this.m_module.StoredProduct;
      this.m_storedQuantitySync = this.m_module.Quantity;
      this.m_capacitySync = this.m_module.Capacity;
    }

    public override void RenderUpdate(GameTime time)
    {
      if (this.m_storedProductSync != this.m_storedProductRender)
      {
        this.m_storedProductRender = this.m_storedProductSync;
        if (this.m_activePileTransform.HasValue)
        {
          this.m_activePileTransform.Value.gameObject.SetActive(false);
          this.m_activePileTransform = Option<Transform>.None;
        }
        if (this.m_storedProductSync.HasValue)
        {
          if (this.m_storedProductSync.Value is LooseProductProto looseProductProto)
          {
            this.m_pileMaterialSetter.SetTexture(looseProductProto.LooseSlimId);
            this.m_activePileTransform = (Option<Transform>) (looseProductProto.Graphics.UseRoughPileMeshes ? this.m_roughPile : this.m_smoothPile).transform;
            this.m_activePileTransform.Value.gameObject.SetActive(true);
          }
          else
            Log.Warning(string.Format("Storing non-loose product '{0}' in ship loose module.", (object) this.m_storedProductSync.Value));
        }
        this.m_storedQuantityRender = this.m_storedQuantitySync;
        this.m_capacityRender = this.m_capacitySync;
        this.updatePilePosition();
      }
      if (!(this.m_storedQuantitySync != this.m_storedQuantityRender) && !(this.m_capacitySync != this.m_capacityRender))
        return;
      this.m_storedQuantityRender = this.m_storedQuantitySync;
      this.m_capacityRender = this.m_capacitySync;
      this.updatePilePosition();
    }

    private void updatePilePosition()
    {
      if (!this.m_activePileTransform.HasValue)
        return;
      CargoShipLooseModuleProto.Gfx graphics = this.m_proto.Graphics;
      this.m_activePileTransform.Value.localPosition = graphics.OffsetEmpty.Lerp(graphics.OffsetFull, Percent.FromRatio(this.m_storedQuantityRender.Value, this.m_capacityRender.Value)).ToVector3();
    }

    public CargoShipModuleLooseMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
    public class Factory : 
      IFactory<CargoShipModule, CargoShipLooseModuleProto, CargoShipModuleBaseMb>
    {
      private readonly AssetsDb m_assetsDb;
      private readonly LooseProductMaterialManager m_looseProductMaterialManager;

      public Factory(
        AssetsDb assetsDb,
        LooseProductMaterialManager looseProductMaterialManager)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_assetsDb = assetsDb;
        this.m_looseProductMaterialManager = looseProductMaterialManager;
      }

      public CargoShipModuleBaseMb Create(CargoShipModule module, CargoShipLooseModuleProto proto)
      {
        CargoShipModuleLooseMb shipModuleLooseMb = this.m_assetsDb.GetClonedPrefabOrEmptyGo(module.Prototype.Graphics.PrefabPath).AddComponent<CargoShipModuleLooseMb>();
        shipModuleLooseMb.Initialize(module, proto, this.m_looseProductMaterialManager);
        return (CargoShipModuleBaseMb) shipModuleLooseMb;
      }
    }
  }
}
