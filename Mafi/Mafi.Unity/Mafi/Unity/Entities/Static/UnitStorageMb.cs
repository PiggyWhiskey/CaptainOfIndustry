// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.Static.UnitStorageMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Buildings.Storages;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Unity.InstancedRendering;
using Mafi.Unity.Terrain;
using Mafi.Unity.Utils;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities.Static
{
  internal class UnitStorageMb : 
    LayoutEntityWithSignMb,
    IEntityMbWithRenderUpdate,
    IEntityMb,
    IDestroyableEntityMb
  {
    private ProductsRenderer m_globalProductsRenderer;
    private Bounds m_bounds;
    private Storage m_storage;
    private InstancedMeshesRenderer<ProductsRenderer.ProductInstanceData> m_productRenderer;
    private InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData> m_rackRenderer;
    private Material m_instancedMaterial;
    private int m_lastRendered;
    private float m_directionMovingAverage;

    public void Initialize(AssetsDb assetsDb, Storage storage, ProductsRenderer productsRenderer)
    {
      this.Initialize(assetsDb, (Mafi.Core.Entities.Static.Layout.LayoutEntity) storage);
      this.m_storage = storage;
      this.m_globalProductsRenderer = productsRenderer;
      this.m_signChildName = "sign";
      this.m_directionMovingAverage = 0.0f;
      this.m_lastRendered = -1;
      Assert.That<Storage>(this.m_storage).IsNotNull<Storage>();
      Assert.That<ProductsRenderer>(this.m_globalProductsRenderer).IsNotNull<ProductsRenderer>();
      if (!(storage.Prototype is UnitStorageProto prototype))
        Log.Error(string.Format("Unit storage prototype {0} is not UnitStorageProto", (object) storage.Prototype));
      else if (!assetsDb.TryGetSharedPrefab(prototype.Graphics.RackPrefabPath, out GameObject _))
      {
        Log.Error(string.Format("Failed to get rack prefab for storage {0}.", (object) storage.Prototype));
      }
      else
      {
        Mesh[] meshes;
        Material sharedMaterial;
        string error;
        if (!InstancingUtils.TryGetMeshLodsAndMaterialFromPrefab(this.m_assetsDb, prototype.Graphics.RackPrefabPath, out meshes, out sharedMaterial, out error))
        {
          Log.Error("Failed to load rack prefab '" + prototype.Graphics.RackPrefabPath + "': " + error);
        }
        else
        {
          this.m_rackRenderer = new InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData>(meshes, InstancingUtils.InstantiateMaterialAndCopyTextures(assetsDb.GetSharedMaterial("Assets/Base/Buildings/Rendering/LayoutEntityInstanced.mat"), sharedMaterial));
          this.m_globalProductsRenderer.ReloadManager.Register((IReloadAfterAssetUpdate) this.m_rackRenderer);
          for (int index1 = 0; index1 < prototype.Graphics.RackRenderOffsets.Length / 2; ++index1)
          {
            for (int index2 = 0; index2 < 2; ++index2)
            {
              System.Numerics.Vector3 rackRenderOffset = prototype.Graphics.RackRenderOffsets[index1 * 2 + index2];
              UnityEngine.Vector3 position = this.transform.position;
              UnityEngine.Vector3 vector3 = this.transform.rotation * new UnityEngine.Vector3(rackRenderOffset.X, rackRenderOffset.Y, rackRenderOffset.Z);
              position.x += vector3.x;
              position.y += vector3.y;
              position.z += vector3.z;
              int num = (int) this.m_rackRenderer.AddInstance(new InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData(position, storage.Transform.Rotation, false, ColorRgba.White, 0.0f));
            }
          }
          this.initializeSign(0.7692308f);
          this.m_bounds = this.gameObject.ComputeMaxBounds();
        }
      }
    }

    public override void Destroy()
    {
      base.Destroy();
      this.m_globalProductsRenderer.ReloadManager.TryUnregisterAndDispose<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData>>(this.m_rackRenderer);
      this.m_globalProductsRenderer.ReloadManager.TryUnregisterAndDispose<InstancedMeshesRenderer<ProductsRenderer.ProductInstanceData>>(this.m_productRenderer);
    }

    private void buildProductRenderData()
    {
      this.m_globalProductsRenderer.ReloadManager.TryUnregisterAndDispose<InstancedMeshesRenderer<ProductsRenderer.ProductInstanceData>>(this.m_productRenderer);
      ProductProto valueOrNull = this.m_storage.StoredProduct.ValueOrNull;
      Assert.That<ProductProto>(valueOrNull).IsNotNull<ProductProto>();
      Mesh meshShared;
      int numCombinedMeshes;
      int verticesPerMesh;
      this.m_globalProductsRenderer.GetMeshAndMaterial(valueOrNull, false, false, (LooseProductMaterialManager) null, out meshShared, out this.m_instancedMaterial, out numCombinedMeshes, out verticesPerMesh);
      Assert.That<Mesh>(meshShared).IsNotNull<Mesh>();
      Assert.That<Material>(this.m_instancedMaterial).IsNotNull<Material>();
      Assert.That<int>(numCombinedMeshes).IsPositive();
      Assert.That<int>(verticesPerMesh).IsPositive();
      Mesh[] sharedMeshLods = new Mesh[7];
      for (int index = 0; index < 7; ++index)
        sharedMeshLods[index] = meshShared;
      this.m_productRenderer = new InstancedMeshesRenderer<ProductsRenderer.ProductInstanceData>(sharedMeshLods, this.m_instancedMaterial);
      this.m_globalProductsRenderer.ReloadManager.Register((IReloadAfterAssetUpdate) this.m_productRenderer);
      if (!(this.m_storage.Prototype.Graphics is UnitStorageProto.Gfx graphics))
      {
        Log.Error(string.Format("Storage prototype's graphics are not UnitStorageProto.Gfx {0}", (object) this.m_storage));
      }
      else
      {
        ImmutableArray<ImmutableArray<ushort>> stackOffsetsPacked = this.m_globalProductsRenderer.StackOffsetsPacked;
        CountableProductProto countableProductProto = this.m_storage.StoredProduct.Value as CountableProductProto;
        if ((Proto) countableProductProto == (Proto) null)
        {
          Log.Error(string.Format("Product prototype is not countable `{0}`.", (object) this.m_storage.StoredProduct.Value));
        }
        else
        {
          uint seed = 1234567;
          int num1 = 0;
          for (int index1 = 0; index1 < graphics.MaxProductRenderCapacity * 3; ++index1)
          {
            ++num1;
            if (num1 >= 1048576)
            {
              Debug.LogError((object) "While loop overflow!");
              break;
            }
            for (int index2 = 0; index2 < 2; ++index2)
            {
              System.Numerics.Vector3 productRenderOffset = graphics.ProductRenderOffsets[index1 / 3 * 2 + index2];
              UnityEngine.Vector3 position = this.transform.position;
              UnityEngine.Vector3 vector3 = this.transform.rotation * new UnityEngine.Vector3(productRenderOffset.X, productRenderOffset.Y, productRenderOffset.Z);
              position.x += vector3.x;
              position.y += vector3.y;
              position.z += vector3.z;
              float y = this.transform.rotation.eulerAngles.y;
              if (index1 / 3 % 2 == 0 && countableProductProto.Graphics.PackingMode == CountableProductStackingMode.Triangle)
                y -= 180f;
              if (index2 == 1)
                y += 180f;
              if (index1 % 2 == 0 && countableProductProto.Graphics.PackingMode == CountableProductStackingMode.StackedAlternating)
                y += 90f;
              uint rotationAndDataPacked = ProductsRenderer.ProductInstanceData.PackYawDegreesToShort(y);
              ProductsRenderer.ProductInstanceData productInstanceData = new ProductsRenderer.ProductInstanceData(position, rotationAndDataPacked);
              ushort offsets = stackOffsetsPacked[(int) this.m_productToRender.Value.SlimId.Value][2 + index1 % 3];
              if (countableProductProto.Graphics.AllowPackingNoise)
              {
                offsets = ProductsRenderer.ProductInstanceDataUtils.ApplyNoiseToOffsets(ref seed, offsets);
                productInstanceData = productInstanceData.ApplyNoiseToYaw(seed, 2);
              }
              int num2 = (int) this.m_productRenderer.AddInstance(productInstanceData.SetOffsetPacked(offsets));
            }
          }
        }
      }
    }

    void IEntityMbWithRenderUpdate.RenderUpdate(GameTime time)
    {
      if (this.m_storage.IsDestroyed)
        return;
      if (this.m_storage.StoredProduct != this.m_productToRender)
      {
        this.m_productToRender = this.m_storage.StoredProduct;
        if (this.m_productToRender.HasValue)
          this.buildProductRenderData();
        this.updateSign();
      }
      if (this.m_productRenderer == null || this.m_storage.StoredProduct.IsNone)
        return;
      if (!(this.m_storage.Prototype.Graphics is UnitStorageProto.Gfx graphics))
        Log.WarningOnce(string.Format("Storage prototype's graphics are not UnitStorageProto.Gfx {0}", (object) this.m_storage));
      else if (this.m_storage.Capacity.Value == 0)
      {
        Log.WarningOnce(string.Format("Storage has no capacity {0}", (object) this.m_storage));
      }
      else
      {
        float num = (float) this.m_storage.CurrentQuantity.Value / (float) this.m_storage.Capacity.Value;
        int numToRender1 = Mathf.CeilToInt((float) graphics.MaxProductRenderCapacity * num) * 2;
        if (this.m_lastRendered == -1)
          this.m_lastRendered = numToRender1;
        this.m_directionMovingAverage = (float) ((double) this.m_directionMovingAverage * 0.949999988079071 + (double) (numToRender1 - this.m_lastRendered) * 0.05000000074505806);
        if ((double) Mathf.Abs(this.m_directionMovingAverage) < 1.0)
        {
          numToRender1 = this.m_lastRendered;
          if (numToRender1 == 0)
            return;
        }
        int numToRender2 = Mathf.CeilToInt((float) (graphics.RackLayers * numToRender1) / (2f * (float) graphics.MaxProductRenderCapacity)) * 2;
        this.m_productRenderer.Render(this.m_bounds, 0, numToRender1);
        if (this.m_rackRenderer != null)
          this.m_rackRenderer.Render(this.m_bounds, 0, numToRender2);
        this.m_lastRendered = numToRender1;
      }
    }

    public UnitStorageMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
