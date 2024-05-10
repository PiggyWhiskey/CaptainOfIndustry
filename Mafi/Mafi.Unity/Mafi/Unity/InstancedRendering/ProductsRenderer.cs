// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InstancedRendering.ProductsRenderer
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Factory.Transports;
using Mafi.Core.GameLoop;
using Mafi.Core.Products;
using Mafi.Core.Terrain;
using Mafi.Unity.Terrain;
using Mafi.Unity.Utils;
using Mafi.Utils;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InstancedRendering
{
  /// <summary>Handles rendering for transported products.</summary>
  /// <remarks>
  /// Notes from transport optimization (Change-Id: I9988f39e22133491718cbe14b8135a69ac022232)
  /// 
  /// Transported products are now rendered using efficient GPU-instancing that renders all products of one product
  /// type in one draw-call.
  ///  - Preparation of data for the rendering is done on sim thread, but only when needed. If transport is
  ///    stationary, update is skipped.
  ///  - Transported product data for GPU shader are packed to 4 x 4 bytes struct (per product) to optimize memory
  ///    access and have all 4B floats properly aligned.
  ///  - Transported products shader has two variants: static and dynamic. Static renders products without
  ///    movement and needs half of the data compared to dynamic variant, and avoids any position/rotation
  ///    interpolation computation. Dynamic variant performs smooth interpolation between two sim steps for
  ///    frame-rate independent smooth movement with no CPU interventions.
  /// 
  /// TODO: It would be possible to further optimize products renderer to upload ProductInstanceData[] on GPU and each
  /// product would just have an index to that array. This would reduce the data transfer between CPU and GPU 4x
  /// and lots of CPU time will be also saved by collecting 4x less data when positions change. Stacking would likely
  /// require to have 3 elements per each product (or we can have 3 positions and just one rotation to aid stacking).
  /// 
  /// Even further, each chunk/renderer could keep a separate buffer and if there are less than 65k rendered products,
  /// only ushort would be needed, further reducing the data amount by 2x.
  /// </remarks>
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class ProductsRenderer : IDisposable
  {
    private static readonly int MAIN_TEX_SHADER_ID;
    private static readonly int METAL_GLOSS_MAP_SHADER_ID;
    private static readonly int NORMAL_MAP_SHADER_ID;
    private static readonly int METALLIC_SHADER_ID;
    private static readonly int SMOOTHNESS_SHADER_ID;
    private static readonly int EMISSION_MAP_SHADER_ID;
    private static readonly int EMISSION_COLOR_SHADER_ID;
    private static readonly int MAX_SHADOW_DISTANCE;
    internal static readonly int VERTICES_PER_MESH_SHADER_ID;
    internal static readonly int INSTANCES_PER_MESH_SHADER_ID;
    internal static readonly int MESH_COUNT_SHADER_ID;
    private readonly AssetsDb m_assetsDb;
    private readonly TerrainManager m_terrainManager;
    public readonly ReloadAfterAssetUpdateManager ReloadManager;
    public readonly ImmutableArray<LooseProductSlimId> SlimIdToLoose;
    public readonly ImmutableArray<ImmutableArray<ushort>> StackOffsetsPacked;
    private readonly ImmutableArray<int> m_productToDataIndex;
    private readonly ProductsRenderer.DrawDataMutableStatic[] m_staticDrawData;
    private readonly ProductsRenderer.PreparedDataMutableStatic[] m_staticPreparedData;
    private readonly ProductsRenderer.DrawDataMutableDynamic[] m_dynamicDrawData;
    private readonly ProductsRenderer.PreparedDataMutableDynamic[] m_dynamicPreparedData;
    private readonly ProductsRenderer.DrawQuatDataMutableDynamic[] m_dynamicQuaternionDrawData;
    private readonly ProductsRenderer.PreparedQuatDataMutableDynamic[] m_dynamicQuaternionPreparedData;
    private readonly Lyst<ProductsRenderer.CommonDataMutable> m_dataToDraw;
    private Bounds m_bounds;
    private readonly Material m_instancedMaterialPrefab;

    public ProductsRenderer(
      IGameLoopEvents gameLoopEvents,
      AssetsDb assetsDb,
      ProductsSlimIdManager productsSlimIdManager,
      LooseProductsSlimIdManager looseProductsSlimIdManager,
      TerrainManager terrainManager,
      LooseProductMaterialManager looseProductMaterialManager,
      ReloadAfterAssetUpdateManager reloadManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_dataToDraw = new Lyst<ProductsRenderer.CommonDataMutable>(64);
      // ISSUE: explicit constructor call
      base.\u002Ector();
      ProductsRenderer productsRenderer = this;
      this.m_assetsDb = assetsDb;
      this.m_terrainManager = terrainManager;
      this.ReloadManager = reloadManager;
      this.m_instancedMaterialPrefab = assetsDb.GetSharedMaterial("Assets/Base/Products/ProductInstanced.mat");
      if ((UnityEngine.Object) this.m_instancedMaterialPrefab == (UnityEngine.Object) null)
        Log.Error("Failed to load material for products from Assets/Base/Products/ProductInstanced.mat");
      ImmutableArray<ProductProto> products = productsSlimIdManager.ManagedProtos;
      this.SlimIdToLoose = looseProductsSlimIdManager.SlimIdToLoose;
      Dict<string, int> meshToIndex = new Dict<string, int>(products.Length);
      this.m_productToDataIndex = products.Map<int>((Func<ProductProto, int>) (proto =>
      {
        if (proto.Graphics.PrefabsPath.IsNone)
          return -1;
        int count;
        if (!meshToIndex.TryGetValue(proto.Graphics.PrefabsPath.Value, out count))
        {
          count = meshToIndex.Count;
          meshToIndex.Add(proto.Graphics.PrefabsPath.Value, count);
        }
        return count;
      }));
      int count1 = meshToIndex.Count;
      this.m_staticDrawData = new ProductsRenderer.DrawDataMutableStatic[count1];
      this.m_staticPreparedData = new ProductsRenderer.PreparedDataMutableStatic[count1];
      this.m_dynamicDrawData = new ProductsRenderer.DrawDataMutableDynamic[count1];
      this.m_dynamicPreparedData = new ProductsRenderer.PreparedDataMutableDynamic[count1];
      this.m_dynamicQuaternionDrawData = new ProductsRenderer.DrawQuatDataMutableDynamic[count1];
      this.m_dynamicQuaternionPreparedData = new ProductsRenderer.PreparedQuatDataMutableDynamic[count1];
      for (int index1 = 0; index1 < products.Length; ++index1)
      {
        int index2 = this.m_productToDataIndex[index1];
        if (index2 >= 0)
        {
          ref ProductsRenderer.CommonDataMutable local1 = ref this.m_staticDrawData[index2].Common;
          if ((UnityEngine.Object) local1.Material == (UnityEngine.Object) null)
            local1 = this.initializeDrawData(products[index1], false, false, looseProductMaterialManager);
          ref ProductsRenderer.CommonDataMutable local2 = ref this.m_dynamicDrawData[index2].Common;
          if ((UnityEngine.Object) local2.Material == (UnityEngine.Object) null)
            local2 = this.initializeDrawData(products[index1], true, false, looseProductMaterialManager);
          ref ProductsRenderer.CommonDataMutable local3 = ref this.m_dynamicQuaternionDrawData[index2].Common;
          if ((UnityEngine.Object) local3.Material == (UnityEngine.Object) null)
            local3 = this.initializeDrawData(products[index1], true, true, looseProductMaterialManager);
        }
      }
      this.StackOffsetsPacked = ImmutableArray.FromRange<ImmutableArray<ushort>>(products.Length, (Func<int, ImmutableArray<ushort>>) (i => productsRenderer.m_productToDataIndex[i] >= 0 ? ProductsRenderer.getPackedOffsets(productsRenderer.m_staticDrawData[productsRenderer.m_productToDataIndex[i]].Common, products[i]) : ImmutableArray<ushort>.Empty));
      gameLoopEvents.RegisterRendererInitState((object) this, new Action(this.initState));
      gameLoopEvents.SyncUpdateEnd.AddNonSaveable<ProductsRenderer>(this, new Action<GameTime>(this.syncUpdateEnd));
      gameLoopEvents.RenderUpdate.AddNonSaveable<ProductsRenderer>(this, new Action<GameTime>(this.renderUpdate));
      gameLoopEvents.OnProjectChanged.AddNonSaveable<ProductsRenderer>(this, new Action(this.reassignBuffers));
    }

    public void Dispose()
    {
      for (int index = 0; index < this.m_staticDrawData.Length; ++index)
      {
        this.m_staticDrawData[index].Common.Dispose();
        this.m_staticDrawData[index] = new ProductsRenderer.DrawDataMutableStatic();
        this.m_staticPreparedData[index] = new ProductsRenderer.PreparedDataMutableStatic();
        this.m_dynamicDrawData[index].Common.Dispose();
        this.m_dynamicDrawData[index] = new ProductsRenderer.DrawDataMutableDynamic();
        this.m_dynamicPreparedData[index] = new ProductsRenderer.PreparedDataMutableDynamic();
        this.m_dynamicQuaternionDrawData[index].Common.Dispose();
        this.m_dynamicQuaternionDrawData[index] = new ProductsRenderer.DrawQuatDataMutableDynamic();
        this.m_dynamicQuaternionPreparedData[index] = new ProductsRenderer.PreparedQuatDataMutableDynamic();
      }
      this.m_dataToDraw.Clear();
    }

    private void reassignBuffers()
    {
      foreach (ProductsRenderer.DrawDataMutableStatic dataMutableStatic in this.m_staticDrawData)
      {
        if (dataMutableStatic.Common.DataBuffer != null)
          dataMutableStatic.Common.Material.SetBuffer(MafiMaterials.INSTANCE_DATA_SHADER_ID, dataMutableStatic.Common.DataBuffer);
      }
      foreach (ProductsRenderer.DrawDataMutableDynamic dataMutableDynamic in this.m_dynamicDrawData)
      {
        if (dataMutableDynamic.Common.DataBuffer != null)
          dataMutableDynamic.Common.Material.SetBuffer(MafiMaterials.INSTANCE_DATA_SHADER_ID, dataMutableDynamic.Common.DataBuffer);
      }
      foreach (ProductsRenderer.DrawQuatDataMutableDynamic dataMutableDynamic in this.m_dynamicQuaternionDrawData)
      {
        if (dataMutableDynamic.Common.DataBuffer != null)
          dataMutableDynamic.Common.Material.SetBuffer(MafiMaterials.INSTANCE_DATA_SHADER_ID, dataMutableDynamic.Common.DataBuffer);
      }
    }

    private static ImmutableArray<ushort> getPackedOffsets(
      ProductsRenderer.CommonDataMutable commonData,
      ProductProto product)
    {
      float sizeX = commonData.MeshShared.bounds.extents.x * 2f;
      float sizeY = commonData.MeshShared.bounds.extents.y * 2f;
      float sizeZ = commonData.MeshShared.bounds.extents.z * 2f;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return product.Graphics.GetStackingOffsets(sizeX, sizeY, sizeZ).Map<ushort>(ProductsRenderer.\u003C\u003EO.\u003C0\u003E__PackOffsets ?? (ProductsRenderer.\u003C\u003EO.\u003C0\u003E__PackOffsets = new Func<RelTile3f, ushort>(ProductsRenderer.ProductInstanceDataUtils.PackOffsets)));
    }

    private void initState()
    {
      this.m_bounds = new Bounds(new Vector3((float) (this.m_terrainManager.TerrainWidth * 2) / 2f, 0.0f, (float) (this.m_terrainManager.TerrainHeight * 2) / 2f), new Vector3((float) (this.m_terrainManager.TerrainWidth * 2), 16000f, (float) (this.m_terrainManager.TerrainHeight * 2)));
    }

    public void PrepareRenderStatic(
      ProductSlimId product,
      ProductsRenderer.ProductInstanceData[] productsToRender,
      int count)
    {
      if (count <= 0 || product.IsPhantom)
        return;
      int index = this.m_productToDataIndex[(int) product.Value];
      if (index < 0)
      {
        Log.Warning(string.Format("Trying to render product #{0} that has no mesh.", (object) product));
      }
      else
      {
        Assert.That<int>(productsToRender.Length).IsGreaterOrEqual(count);
        ref ProductsRenderer.PreparedDataMutableStatic local = ref this.m_staticPreparedData[index];
        ArrayUtils.EnsureLengthPow2KeepContents<ProductsRenderer.ProductInstanceData>(ref local.Data, local.WrittenCount + count);
        Array.Copy((Array) productsToRender, 0, (Array) local.Data, local.WrittenCount, count);
        local.WrittenCount += count;
      }
    }

    public void PrepareRenderStaticMixed(
      ProductSlimId[] products,
      ProductsRenderer.ProductInstanceData[] productsToRender,
      int count)
    {
      if (count <= 0)
        return;
      Assert.That<int>(products.Length).IsGreaterOrEqual(count);
      Assert.That<int>(productsToRender.Length).IsGreaterOrEqual(count);
      ProductSlimId productSlimId = products[0];
      int index1 = this.m_productToDataIndex[(int) productSlimId.Value];
      if (index1 < 0)
      {
        Log.Warning(string.Format("Trying to render product #{0} that has no mesh.", (object) productSlimId));
        index1 = 0;
      }
      ref ProductsRenderer.PreparedDataMutableStatic local = ref this.m_staticPreparedData[index1];
      ArrayUtils.EnsureLengthPow2KeepContents<ProductsRenderer.ProductInstanceData>(ref local.Data, local.WrittenCount + count);
      for (int index2 = 0; index2 < count; ++index2)
      {
        ProductSlimId product = products[index2];
        if (!product.IsPhantom)
        {
          if (product != productSlimId)
          {
            productSlimId = product;
            int index3 = this.m_productToDataIndex[(int) product.Value];
            if (index3 < 0)
            {
              Log.Warning(string.Format("Trying to render product #{0} that has no mesh.", (object) product));
              continue;
            }
            local = ref this.m_staticPreparedData[index3];
            ArrayUtils.EnsureLengthPow2KeepContents<ProductsRenderer.ProductInstanceData>(ref local.Data, local.WrittenCount + count - index2);
          }
          local.Data[local.WrittenCount] = productsToRender[index2];
          ++local.WrittenCount;
        }
      }
    }

    public void PrepareRenderDynamic(
      ProductSlimId product,
      ProductsRenderer.ProductInstanceDataPair[] productsToRender,
      int count)
    {
      if (count <= 0)
        return;
      int index = this.m_productToDataIndex[(int) product.Value];
      if (index < 0)
      {
        Log.Warning(string.Format("Trying to render product #{0} that has no mesh.", (object) product));
      }
      else
      {
        Assert.That<int>(productsToRender.Length).IsGreaterOrEqual(count);
        ref ProductsRenderer.PreparedDataMutableDynamic local = ref this.m_dynamicPreparedData[index];
        ArrayUtils.EnsureLengthPow2KeepContents<ProductsRenderer.ProductInstanceDataPair>(ref local.Data, local.WrittenCount + count);
        Array.Copy((Array) productsToRender, 0, (Array) local.Data, local.WrittenCount, count);
        local.WrittenCount += count;
      }
    }

    public void PrepareRenderDynamicMixed(
      ProductSlimId[] products,
      ProductsRenderer.ProductInstanceDataPair[] productsToRender,
      int count)
    {
      if (count <= 0)
        return;
      Assert.That<int>(products.Length).IsGreaterOrEqual(count);
      Assert.That<int>(productsToRender.Length).IsGreaterOrEqual(count);
      ProductSlimId productSlimId = products[0];
      int index1 = this.m_productToDataIndex[(int) productSlimId.Value];
      if (index1 < 0)
      {
        Log.Warning(string.Format("Trying to render product #{0} that has no mesh.", (object) productSlimId));
        index1 = 0;
      }
      ref ProductsRenderer.PreparedDataMutableDynamic local = ref this.m_dynamicPreparedData[index1];
      ArrayUtils.EnsureLengthPow2KeepContents<ProductsRenderer.ProductInstanceDataPair>(ref local.Data, local.WrittenCount + count);
      for (int index2 = 0; index2 < count; ++index2)
      {
        ProductSlimId product = products[index2];
        if (product != productSlimId)
        {
          productSlimId = product;
          int index3 = this.m_productToDataIndex[(int) product.Value];
          if (index3 < 0)
          {
            Log.Warning(string.Format("Trying to render product #{0} that has no mesh.", (object) product));
            continue;
          }
          local = ref this.m_dynamicPreparedData[index3];
          ArrayUtils.EnsureLengthPow2KeepContents<ProductsRenderer.ProductInstanceDataPair>(ref local.Data, local.WrittenCount + count);
        }
        local.Data[local.WrittenCount] = productsToRender[index2];
        ++local.WrittenCount;
      }
    }

    public void PrepareRenderDynamicQuaternions(
      ProductSlimId product,
      ProductsRenderer.ProductInstanceQDataPair[] productsToRender,
      int count)
    {
      if (count <= 0)
        return;
      int index = this.m_productToDataIndex[(int) product.Value];
      if (index < 0)
      {
        Log.Warning(string.Format("Trying to render product #{0} that has no mesh.", (object) product));
      }
      else
      {
        Assert.That<int>(productsToRender.Length).IsGreaterOrEqual(count);
        ref ProductsRenderer.PreparedQuatDataMutableDynamic local = ref this.m_dynamicQuaternionPreparedData[index];
        ArrayUtils.EnsureLengthPow2KeepContents<ProductsRenderer.ProductInstanceQDataPair>(ref local.Data, local.WrittenCount + count);
        Array.Copy((Array) productsToRender, 0, (Array) local.Data, local.WrittenCount, count);
        local.WrittenCount += count;
      }
    }

    private void syncUpdateEnd(GameTime time)
    {
      int length = this.m_staticPreparedData.Length;
      this.m_dataToDraw.Clear();
      for (int index = 0; index < length; ++index)
      {
        ref ProductsRenderer.PreparedDataMutableStatic local1 = ref this.m_staticPreparedData[index];
        if (local1.WrittenCount != 0)
        {
          ref ProductsRenderer.DrawDataMutableStatic local2 = ref this.m_staticDrawData[index];
          ref ProductsRenderer.CommonDataMutable local3 = ref local2.Common;
          Assert.That<uint[]>(local3.DrawArgs).IsNotNull<uint[]>();
          Assert.That<ComputeBuffer>(local3.DrawArgsBuffer).IsNotNull<ComputeBuffer>();
          int writtenCount = local1.WrittenCount;
          local2.Count = writtenCount;
          local1.WrittenCount = 0;
          Swap.Them<ProductsRenderer.ProductInstanceData[]>(ref local1.Data, ref local2.Data);
          local3.DrawArgs[1] = (uint) writtenCount.CeilDivPositive(local3.NumCombinedMeshes);
          if (local3.DataBuffer == null)
          {
            local3.DataBuffer = new ComputeBuffer(writtenCount.CeilToPowerOfTwoOrZero().Max(64), 16);
            local3.Material.SetBuffer(MafiMaterials.INSTANCE_DATA_SHADER_ID, local3.DataBuffer);
            local3.Material.SetInteger(ProductsRenderer.VERTICES_PER_MESH_SHADER_ID, local3.NumVerticesPerMesh);
            local3.Material.SetInteger(ProductsRenderer.INSTANCES_PER_MESH_SHADER_ID, local3.NumCombinedMeshes);
          }
          else if (local3.DataBuffer.count < writtenCount)
          {
            int count = (3 * local3.DataBuffer.count / 2).Max(writtenCount);
            local3.DataBuffer.Dispose();
            local3.DataBuffer = new ComputeBuffer(count, 16);
            local3.Material.SetBuffer(MafiMaterials.INSTANCE_DATA_SHADER_ID, local3.DataBuffer);
          }
          local3.Material.SetInteger(ProductsRenderer.MESH_COUNT_SHADER_ID, local2.Count);
          local3.DataBuffer.SetData((Array) local2.Data, 0, 0, local2.Count);
          this.m_dataToDraw.Add(local3);
        }
      }
      for (int index = 0; index < length; ++index)
      {
        ref ProductsRenderer.PreparedDataMutableDynamic local4 = ref this.m_dynamicPreparedData[index];
        if (local4.WrittenCount != 0)
        {
          ref ProductsRenderer.DrawDataMutableDynamic local5 = ref this.m_dynamicDrawData[index];
          ref ProductsRenderer.CommonDataMutable local6 = ref local5.Common;
          Assert.That<uint[]>(local6.DrawArgs).IsNotNull<uint[]>();
          Assert.That<ComputeBuffer>(local6.DrawArgsBuffer).IsNotNull<ComputeBuffer>();
          int writtenCount = local4.WrittenCount;
          local5.Count = writtenCount;
          local4.WrittenCount = 0;
          Swap.Them<ProductsRenderer.ProductInstanceDataPair[]>(ref local4.Data, ref local5.Data);
          local6.DrawArgs[1] = (uint) writtenCount.CeilDivPositive(local6.NumCombinedMeshes);
          if (local6.DataBuffer == null)
          {
            local6.DataBuffer = new ComputeBuffer(writtenCount.CeilToPowerOfTwoOrZero().Max(64), 32);
            local6.Material.SetBuffer(MafiMaterials.INSTANCE_DATA_SHADER_ID, local6.DataBuffer);
            local6.Material.SetInteger(ProductsRenderer.VERTICES_PER_MESH_SHADER_ID, local6.NumVerticesPerMesh);
            local6.Material.SetInteger(ProductsRenderer.INSTANCES_PER_MESH_SHADER_ID, local6.NumCombinedMeshes);
          }
          else if (local6.DataBuffer.count < writtenCount)
          {
            int count = (3 * local6.DataBuffer.count / 2).Max(writtenCount);
            local6.DataBuffer.Dispose();
            local6.DataBuffer = new ComputeBuffer(count, 32);
            local6.Material.SetBuffer(MafiMaterials.INSTANCE_DATA_SHADER_ID, local6.DataBuffer);
          }
          local6.Material.SetInteger(ProductsRenderer.MESH_COUNT_SHADER_ID, local5.Count);
          local6.DataBuffer.SetData((Array) local5.Data, 0, 0, local5.Count);
          this.m_dataToDraw.Add(local6);
        }
      }
      for (int index = 0; index < length; ++index)
      {
        ref ProductsRenderer.PreparedQuatDataMutableDynamic local7 = ref this.m_dynamicQuaternionPreparedData[index];
        if (local7.WrittenCount != 0)
        {
          ref ProductsRenderer.DrawQuatDataMutableDynamic local8 = ref this.m_dynamicQuaternionDrawData[index];
          ref ProductsRenderer.CommonDataMutable local9 = ref local8.Common;
          Assert.That<uint[]>(local9.DrawArgs).IsNotNull<uint[]>();
          Assert.That<ComputeBuffer>(local9.DrawArgsBuffer).IsNotNull<ComputeBuffer>();
          int writtenCount = local7.WrittenCount;
          local8.Count = writtenCount;
          local7.WrittenCount = 0;
          Swap.Them<ProductsRenderer.ProductInstanceQDataPair[]>(ref local7.Data, ref local8.Data);
          local9.DrawArgs[1] = (uint) writtenCount.CeilDivPositive(local9.NumCombinedMeshes);
          if (local9.DataBuffer == null)
          {
            local9.DataBuffer = new ComputeBuffer(writtenCount.CeilToPowerOfTwoOrZero().Max(64), 64);
            local9.Material.SetBuffer(MafiMaterials.INSTANCE_DATA_SHADER_ID, local9.DataBuffer);
            local9.Material.SetInteger(ProductsRenderer.VERTICES_PER_MESH_SHADER_ID, local9.NumVerticesPerMesh);
            local9.Material.SetInteger(ProductsRenderer.INSTANCES_PER_MESH_SHADER_ID, local9.NumCombinedMeshes);
          }
          else if (local9.DataBuffer.count < writtenCount)
          {
            int count = (3 * local9.DataBuffer.count / 2).Max(writtenCount);
            local9.DataBuffer.Dispose();
            local9.DataBuffer = new ComputeBuffer(count, 64);
            local9.Material.SetBuffer(MafiMaterials.INSTANCE_DATA_SHADER_ID, local9.DataBuffer);
          }
          local9.Material.SetInteger(ProductsRenderer.MESH_COUNT_SHADER_ID, local8.Count);
          local9.DataBuffer.SetData((Array) local8.Data, 0, 0, local8.Count);
          this.m_dataToDraw.Add(local9);
        }
      }
    }

    private void renderUpdate(GameTime time)
    {
      foreach (ProductsRenderer.CommonDataMutable commonDataMutable in this.m_dataToDraw)
        Graphics.DrawMeshInstancedProcedural(commonDataMutable.MeshShared, 0, commonDataMutable.Material, this.m_bounds, (int) commonDataMutable.DrawArgs[1], lightProbeUsage: LightProbeUsage.Off);
    }

    public void GetMeshAndMaterial(
      ProductProto productProto,
      bool useInterpolation,
      bool useQuaternions,
      LooseProductMaterialManager looseProductMaterialManager,
      out Mesh meshShared,
      out Material instancedMaterial,
      out int numCombinedMeshes,
      out int verticesPerMesh)
    {
      GameObject prefab = (GameObject) null;
      if (productProto.Graphics.PrefabsPath.IsNone || !this.m_assetsDb.TryGetSharedPrefab(productProto.Graphics.PrefabsPath.Value, out prefab))
        Log.Error(string.Format("Failed to get prefab for transported product '{0}' ", (object) productProto) + string.Format("form path '{0}'.", (object) productProto.Graphics.PrefabsPath));
      MeshFilter component1 = (MeshFilter) null;
      MeshRenderer component2 = (MeshRenderer) null;
      if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
      {
        if (prefab.transform.childCount != 0)
        {
          Log.Error(string.Format("Transported product '{0}' must be a single GameObject without any children.", (object) productProto));
          prefab = (GameObject) null;
        }
        else if (!prefab.TryGetComponent<MeshFilter>(out component1))
        {
          Log.Error(string.Format("Game object transported product '{0}' is missing MeshFilter.", (object) productProto));
          prefab = (GameObject) null;
        }
        else if (!(bool) (UnityEngine.Object) component1.sharedMesh)
        {
          Log.Error(string.Format("Mesh of transported product '{0}' is missing.", (object) productProto));
          prefab = (GameObject) null;
        }
        else if (!prefab.TryGetComponent<MeshRenderer>(out component2))
        {
          Log.Error(string.Format("Game object transported product '{0}' is missing MeshRenderer.", (object) productProto));
          prefab = (GameObject) null;
        }
        else if (!(bool) (UnityEngine.Object) component2.sharedMaterial)
        {
          Log.Error(string.Format("Material of transported product '{0}' is missing.", (object) productProto));
          prefab = (GameObject) null;
        }
      }
      if ((UnityEngine.Object) prefab == (UnityEngine.Object) null)
      {
        GameObject gameObject = new GameObject();
        MeshBuilder instance = MeshBuilder.Instance;
        Vector3 extents = Vector3.one / 2f;
        instance.AddAaBox(new Vector3(0.0f, extents.y, 0.0f), extents, new Color32((byte) 192, (byte) 192, (byte) 192, byte.MaxValue));
        instance.UpdateMbAndClear((IBuildable) gameObject.AddComponent<BuildableMb>());
        component1 = gameObject.GetComponent<MeshFilter>();
        component2 = gameObject.GetComponent<MeshRenderer>();
        component2.sharedMaterial = this.m_assetsDb.DefaultMaterial;
        gameObject.SetActive(false);
      }
      Assert.That<MeshFilter>(component1).IsNotNull<MeshFilter>();
      Assert.That<MeshRenderer>(component2).IsNotNull<MeshRenderer>();
      Assert.That<Mesh>(component1.sharedMesh).IsNotNull<Mesh>();
      instancedMaterial = UnityEngine.Object.Instantiate<Material>(this.m_instancedMaterialPrefab);
      if (useInterpolation)
        instancedMaterial.EnableKeyword("ENABLE_INTERPOLATION");
      if (useQuaternions)
        instancedMaterial.EnableKeyword("USE_QUATERNIONS");
      instancedMaterial.SetInteger("_MaxShadowDistance", ProductsRenderer.MAX_SHADOW_DISTANCE);
      if (this.SlimIdToLoose[(int) productProto.SlimId.Value].IsNotPhantom)
      {
        instancedMaterial.EnableKeyword("ENABLE_TEX_ARRAY");
        instancedMaterial.EnableKeyword("USE_SCALE_INSTEAD_OF_OFFSET");
        instancedMaterial.SetTexture("_AlbedoTexArray", (Texture) looseProductMaterialManager.AlbedoTexArray);
        instancedMaterial.SetTexture("_NormalsTexArray", (Texture) looseProductMaterialManager.NormalsTexArray);
        instancedMaterial.SetFloat(ProductsRenderer.METALLIC_SHADER_ID, 0.0f);
        instancedMaterial.SetFloat(ProductsRenderer.SMOOTHNESS_SHADER_ID, 0.3f);
      }
      else
      {
        Material sharedMaterial = component2.sharedMaterial;
        Assert.That<bool>(sharedMaterial.HasProperty(ProductsRenderer.MAIN_TEX_SHADER_ID)).IsTrue<ProductProto.ID>("Property '_MainTex' missing on material of '{0}', use Standard material.", productProto.Id);
        Assert.That<bool>(sharedMaterial.HasProperty(ProductsRenderer.METAL_GLOSS_MAP_SHADER_ID)).IsTrue<ProductProto.ID>("Property '_MetallicGlossMap' missing on material of '{0}', use Standard material.", productProto.Id);
        Assert.That<bool>(sharedMaterial.HasProperty(ProductsRenderer.NORMAL_MAP_SHADER_ID)).IsTrue<ProductProto.ID>("Property '_BumpMap' missing on material of '{0}', use Standard material.", productProto.Id);
        Assert.That<bool>(sharedMaterial.HasProperty(ProductsRenderer.METALLIC_SHADER_ID)).IsTrue<ProductProto.ID>("Property '_Metallic' missing on material of '{0}', use Standard material.", productProto.Id);
        Assert.That<bool>(sharedMaterial.HasProperty(ProductsRenderer.SMOOTHNESS_SHADER_ID)).IsTrue<ProductProto.ID>("Property '_Glossiness' missing on material of '{0}', use Standard material.", productProto.Id);
        instancedMaterial.SetTexture(ProductsRenderer.MAIN_TEX_SHADER_ID, sharedMaterial.GetTexture(ProductsRenderer.MAIN_TEX_SHADER_ID));
        if (sharedMaterial.IsKeywordEnabled("_EMISSION"))
        {
          instancedMaterial.EnableKeyword("ENABLE_EMISSION");
          instancedMaterial.SetTexture(ProductsRenderer.EMISSION_MAP_SHADER_ID, sharedMaterial.GetTexture(ProductsRenderer.EMISSION_MAP_SHADER_ID));
          instancedMaterial.SetColor(ProductsRenderer.EMISSION_COLOR_SHADER_ID, sharedMaterial.GetColor(ProductsRenderer.EMISSION_COLOR_SHADER_ID));
        }
        else
        {
          instancedMaterial.SetTexture(ProductsRenderer.NORMAL_MAP_SHADER_ID, sharedMaterial.GetTexture(ProductsRenderer.NORMAL_MAP_SHADER_ID));
          Texture texture = sharedMaterial.GetTexture(ProductsRenderer.METAL_GLOSS_MAP_SHADER_ID);
          if ((bool) (UnityEngine.Object) texture)
          {
            instancedMaterial.SetFloat(ProductsRenderer.METALLIC_SHADER_ID, 0.0f);
            instancedMaterial.SetFloat(ProductsRenderer.SMOOTHNESS_SHADER_ID, 0.0f);
            instancedMaterial.SetTexture(ProductsRenderer.METAL_GLOSS_MAP_SHADER_ID, texture);
          }
          else
          {
            instancedMaterial.SetFloat(ProductsRenderer.METALLIC_SHADER_ID, sharedMaterial.GetFloat(ProductsRenderer.METALLIC_SHADER_ID));
            instancedMaterial.SetFloat(ProductsRenderer.SMOOTHNESS_SHADER_ID, sharedMaterial.GetFloat(ProductsRenderer.SMOOTHNESS_SHADER_ID));
          }
        }
      }
      if (component1.sharedMesh.isReadable)
      {
        numCombinedMeshes = 256.CeilDivPositive(component1.sharedMesh.vertexCount);
        verticesPerMesh = component1.sharedMesh.vertexCount;
      }
      else
      {
        Log.Warning(string.Format("Mesh {0} for product {1} is not readable.", (object) component1.sharedMesh, (object) productProto));
        numCombinedMeshes = 1;
        GraphicsBuffer vertexBuffer = component1.sharedMesh.GetVertexBuffer(0);
        verticesPerMesh = vertexBuffer.count;
        vertexBuffer.Release();
      }
      if (numCombinedMeshes == 1)
      {
        meshShared = component1.sharedMesh;
      }
      else
      {
        CombineInstance[] combine = new CombineInstance[numCombinedMeshes];
        for (int index = 0; index < numCombinedMeshes; ++index)
          combine[index].mesh = component1.sharedMesh;
        meshShared = new Mesh();
        meshShared.CombineMeshes(combine, true, false);
        meshShared.name = component1.sharedMesh.name;
      }
      meshShared.UploadMeshData(false);
    }

    private ProductsRenderer.CommonDataMutable initializeDrawData(
      ProductProto productProto,
      bool useInterpolation,
      bool useQuaternions,
      LooseProductMaterialManager looseProductMaterialManager)
    {
      Mesh meshShared;
      Material instancedMaterial;
      int numCombinedMeshes;
      int verticesPerMesh;
      this.GetMeshAndMaterial(productProto, useInterpolation, useQuaternions, looseProductMaterialManager, out meshShared, out instancedMaterial, out numCombinedMeshes, out verticesPerMesh);
      uint[] drawArgs = new uint[5]
      {
        meshShared.GetIndexCount(0),
        0U,
        meshShared.GetIndexStart(0),
        meshShared.GetBaseVertex(0),
        0U
      };
      ComputeBuffer drawArgsBuffer = new ComputeBuffer(1, drawArgs.Length * 4, ComputeBufferType.DrawIndirect);
      return new ProductsRenderer.CommonDataMutable(meshShared, instancedMaterial, drawArgs, drawArgsBuffer, numCombinedMeshes, verticesPerMesh);
    }

    static ProductsRenderer()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      ProductsRenderer.MAIN_TEX_SHADER_ID = Shader.PropertyToID("_MainTex");
      ProductsRenderer.METAL_GLOSS_MAP_SHADER_ID = Shader.PropertyToID("_MetallicGlossMap");
      ProductsRenderer.NORMAL_MAP_SHADER_ID = Shader.PropertyToID("_BumpMap");
      ProductsRenderer.METALLIC_SHADER_ID = Shader.PropertyToID("_Metallic");
      ProductsRenderer.SMOOTHNESS_SHADER_ID = Shader.PropertyToID("_Glossiness");
      ProductsRenderer.EMISSION_MAP_SHADER_ID = Shader.PropertyToID("_EmissionMap");
      ProductsRenderer.EMISSION_COLOR_SHADER_ID = Shader.PropertyToID("_EmissionColor");
      ProductsRenderer.MAX_SHADOW_DISTANCE = 200;
      ProductsRenderer.VERTICES_PER_MESH_SHADER_ID = Shader.PropertyToID("_VerticesPerMesh");
      ProductsRenderer.INSTANCES_PER_MESH_SHADER_ID = Shader.PropertyToID("_InstancesPerMesh");
      ProductsRenderer.MESH_COUNT_SHADER_ID = Shader.PropertyToID("_TotalMeshCount");
    }

    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public readonly struct ProductInstanceDataUtils
    {
      /// <summary>
      /// Packs offsets that must be within 0-0.5 tiles (0 - 1 meters).
      /// </summary>
      public static ushort PackOffsets(RelTile3f offset)
      {
        return (ushort) (offset.X.RawValue >> 4 & 31 | (offset.Y.RawValue >> 4 & 31) << 5 | (offset.Z.RawValue >> 4 & 31) << 10);
      }

      /// <summary>
      /// Applies slight noise to the given packed offsets. Returns a new seed
      /// </summary>
      public static ushort ApplyNoiseToOffsets(ref uint seed, ushort offsetPacked)
      {
        seed ^= seed << 13;
        seed ^= seed >> 17;
        seed ^= seed << 5;
        offsetPacked ^= (ushort) (seed & 1U);
        offsetPacked ^= (ushort) (seed & 32U);
        return offsetPacked;
      }
    }

    /// <summary>
    /// Per-instance data that is passed to GPU. Layout of this struct must match the `MeshProperties` struct
    /// in the shader.
    /// </summary>
    [ExpectedStructSize(16)]
    public readonly struct ProductInstanceData
    {
      public const int SIZE_BYTES = 16;
      public readonly Vector3 Position;
      /// <summary>
      /// For loose products: stores yaw in first the byte, pitch in the second, scale in third, and tex ID in fourth.
      /// For countable products: stores yaw in first the byte, pitch in the second, offsets in next 15 bits,
      /// and "flip texture" in the final bit.
      /// </summary>
      public readonly uint RotationAndDataPacked;

      public ProductInstanceData(Vector3 position, uint rotationAndDataPacked)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.Position = position;
        this.RotationAndDataPacked = rotationAndDataPacked;
      }

      public static ProductsRenderer.ProductInstanceData FromWaypoint(TransportWaypoint waypoint)
      {
        return new ProductsRenderer.ProductInstanceData(waypoint.Position.ToVector3(), ProductsRenderer.ProductInstanceData.packRotationToShort(waypoint.Rotation));
      }

      public ProductsRenderer.ProductInstanceDataPair PairWith(
        ProductsRenderer.ProductInstanceData other)
      {
        return new ProductsRenderer.ProductInstanceDataPair(this, other);
      }

      public ProductsRenderer.ProductInstanceData ApplyOffsetToYaw(float degrees)
      {
        return new ProductsRenderer.ProductInstanceData(this.Position, (uint) ((int) (this.RotationAndDataPacked >> 8) << 8 | (int) ProductsRenderer.ProductInstanceData.PackYawDegreesToShort((float) ((double) (this.RotationAndDataPacked & (uint) byte.MaxValue) * 360.0 / (double) byte.MaxValue) + degrees) & (int) byte.MaxValue));
      }

      public ProductsRenderer.ProductInstanceData ApplyNoiseToYaw(uint seed, int bits)
      {
        if (bits <= 0)
          return new ProductsRenderer.ProductInstanceData(this.Position, this.RotationAndDataPacked);
        uint num1 = (uint) ((1 << bits) - 1);
        seed ^= seed << 13;
        seed ^= seed >> 17;
        seed ^= seed << 5;
        uint num2 = this.RotationAndDataPacked & (uint) byte.MaxValue;
        return new ProductsRenderer.ProductInstanceData(this.Position, (uint) ((int) (this.RotationAndDataPacked >> 8) << 8 | (((int) seed & 1 << bits + 1) == 0 ? (int) (num2 + (seed & num1)) : (int) (num2 + 256U - (seed & num1))) & (int) byte.MaxValue));
      }

      private static uint packRotationToShort(TransportWaypointRotation rotation)
      {
        return (uint) rotation.Yaw.Raw >> 8 | (uint) rotation.Pitch.Raw & 65280U;
      }

      public static uint PackYawDegreesToShort(float yaw)
      {
        while ((double) yaw < 0.0)
          yaw += 360f;
        while ((double) yaw > 360.0)
          yaw -= 360f;
        return (uint) Mathf.RoundToInt((float) (256.0 * (double) yaw / 360.0)) & (uint) ushort.MaxValue;
      }

      public ProductsRenderer.ProductInstanceData SetTexIdQuantityAndSeq(
        byte texId,
        byte quantitySlim,
        byte seqNumber)
      {
        return new ProductsRenderer.ProductInstanceData(this.Position, (uint) ((int) this.RotationAndDataPacked & (int) ushort.MaxValue | (int) texId << 24 | ((int) quantitySlim & 3) << 16 | ((int) seqNumber & 63) << 18));
      }

      public ProductsRenderer.ProductInstanceData SetAlternativeTexture()
      {
        return new ProductsRenderer.ProductInstanceData(this.Position, (uint) ((int) this.RotationAndDataPacked & int.MaxValue | int.MinValue));
      }

      public ProductsRenderer.ProductInstanceData SetOffsetPacked(ushort offsetPacked)
      {
        return new ProductsRenderer.ProductInstanceData(this.Position, (uint) ((int) this.RotationAndDataPacked & (int) ushort.MaxValue | (int) offsetPacked << 16 | (int) this.RotationAndDataPacked & int.MinValue));
      }

      public override string ToString()
      {
        int num1 = ((double) (this.RotationAndDataPacked & (uint) byte.MaxValue) * (45.0 / 32.0)).RoundToInt();
        int num2 = ((double) (this.RotationAndDataPacked >> 16 & (uint) byte.MaxValue) * (45.0 / 32.0)).RoundToInt();
        return string.Format("({0}, {1}, {2}) ", (object) this.Position.x.Round(2), (object) this.Position.y.Round(2), (object) this.Position.z.Round(2)) + string.Format("rot({0},", (object) num1) + string.Format("{0}) data 0b{1}", (object) num2, (object) Convert.ToString((long) (this.RotationAndDataPacked >> 16), 2));
      }
    }

    /// <summary>
    /// Per-instance data that is passed to GPU. Layout of this struct must match the `MeshProperties` struct
    /// in the shader.
    /// </summary>
    [ExpectedStructSize(32)]
    public readonly struct ProductInstanceDataPair
    {
      public const int SIZE_BYTES = 32;
      public readonly ProductsRenderer.ProductInstanceData From;
      public readonly ProductsRenderer.ProductInstanceData To;

      public ProductInstanceDataPair(
        ProductsRenderer.ProductInstanceData from,
        ProductsRenderer.ProductInstanceData to)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.From = from;
        this.To = to;
      }
    }

    /// <summary>
    /// Per-instance data that is passed to GPU. Layout of this struct must match the `MeshProperties` struct
    /// in the shader.
    /// </summary>
    [ExpectedStructSize(32)]
    public readonly struct ProductInstanceQData
    {
      public const int SIZE_BYTES = 32;
      public readonly Vector3 Position;
      /// <summary>Stores scale in third byte, and tex ID in fourth.</summary>
      public readonly uint RotationAndDataPacked;
      public readonly Vector4 Q;

      public ProductInstanceQData(Vector3 position, Vector4 quaternion, uint rotationAndDataPacked = 0)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.Position = position;
        this.Q = quaternion;
        this.RotationAndDataPacked = rotationAndDataPacked;
      }

      public ProductsRenderer.ProductInstanceQDataPair PairWith(
        ProductsRenderer.ProductInstanceQData other)
      {
        return new ProductsRenderer.ProductInstanceQDataPair(this, other);
      }

      public ProductsRenderer.ProductInstanceQData SetTexIdQuantityAndSeq(
        byte texId,
        byte quantitySlim,
        byte seqNumber)
      {
        return new ProductsRenderer.ProductInstanceQData(this.Position, this.Q, (uint) ((int) this.RotationAndDataPacked & (int) ushort.MaxValue | (int) texId << 24 | ((int) quantitySlim & 3) << 16 | ((int) seqNumber & 63) << 18));
      }

      public ProductsRenderer.ProductInstanceQData ApplyOffsetToYaw(float degrees)
      {
        Quaternion quaternion = new Quaternion(this.Q.x, this.Q.y, this.Q.z, this.Q.w) * Quaternion.Euler(0.0f, degrees, 0.0f);
        return new ProductsRenderer.ProductInstanceQData(this.Position, new Vector4(quaternion.x, quaternion.y, quaternion.z, quaternion.w), this.RotationAndDataPacked);
      }

      /// <summary>
      /// Adds a random angle between -maxChange and maxChange degrees.
      /// </summary>
      public ProductsRenderer.ProductInstanceQData ApplyNoiseToYaw(uint seed, float maxChange)
      {
        if ((double) maxChange <= 0.0)
          return new ProductsRenderer.ProductInstanceQData(this.Position, this.Q, this.RotationAndDataPacked);
        Quaternion quaternion1 = new Quaternion(this.Q.x, this.Q.y, this.Q.z, this.Q.w);
        seed ^= seed << 13;
        seed ^= seed >> 17;
        seed ^= seed << 5;
        float num = (float) (seed & (uint) ushort.MaxValue) / 65536f;
        float y = (float) ((double) maxChange * ((double) num - 0.5) * 2.0);
        Quaternion quaternion2 = quaternion1 * Quaternion.Euler(0.0f, y, 0.0f);
        return new ProductsRenderer.ProductInstanceQData(this.Position, new Vector4(quaternion2.x, quaternion2.y, quaternion2.z, quaternion2.w), this.RotationAndDataPacked);
      }

      public ProductsRenderer.ProductInstanceQData SetOffsetPacked(ushort offsetPacked)
      {
        return new ProductsRenderer.ProductInstanceQData(this.Position, this.Q, (uint) ((int) this.RotationAndDataPacked & (int) ushort.MaxValue | (int) offsetPacked << 16));
      }

      public override string ToString()
      {
        int num1 = ((double) (this.RotationAndDataPacked & (uint) byte.MaxValue) * (45.0 / 32.0)).RoundToInt();
        int num2 = ((double) (this.RotationAndDataPacked >> 16 & (uint) byte.MaxValue) * (45.0 / 32.0)).RoundToInt();
        return string.Format("({0}, {1}, {2}) ", (object) this.Position.x.Round(2), (object) this.Position.y.Round(2), (object) this.Position.z.Round(2)) + string.Format("rot({0},", (object) num1) + string.Format("{0}) data 0b{1}", (object) num2, (object) Convert.ToString((long) (this.RotationAndDataPacked >> 16), 2));
      }
    }

    /// <summary>
    /// Per-instance data that is passed to GPU. Layout of this struct must match the `MeshProperties` struct
    /// in the shader.
    /// </summary>
    [ExpectedStructSize(64)]
    public readonly struct ProductInstanceQDataPair
    {
      public const int SIZE_BYTES = 64;
      public readonly ProductsRenderer.ProductInstanceQData From;
      public readonly ProductsRenderer.ProductInstanceQData To;

      public ProductInstanceQDataPair(
        ProductsRenderer.ProductInstanceQData from,
        ProductsRenderer.ProductInstanceQData to)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.From = from;
        this.To = to;
      }
    }

    private struct CommonDataMutable
    {
      public readonly Mesh MeshShared;
      public readonly Material Material;
      public readonly uint[] DrawArgs;
      public readonly ComputeBuffer DrawArgsBuffer;
      public readonly int NumCombinedMeshes;
      public readonly int NumVerticesPerMesh;
      public ComputeBuffer DataBuffer;

      public CommonDataMutable(
        Mesh meshShared,
        Material material,
        uint[] drawArgs,
        ComputeBuffer drawArgsBuffer,
        int numCombinedMeshes,
        int numVerticesPerMesh)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.MeshShared = meshShared;
        this.Material = material;
        this.DrawArgs = drawArgs;
        this.DrawArgsBuffer = drawArgsBuffer;
        this.NumCombinedMeshes = numCombinedMeshes;
        this.NumVerticesPerMesh = numVerticesPerMesh;
        this.DataBuffer = (ComputeBuffer) null;
      }

      public readonly void Dispose()
      {
        this.Material.DestroyIfNotNull();
        this.DrawArgsBuffer?.Dispose();
        this.DataBuffer?.Dispose();
      }
    }

    private struct DrawDataMutableStatic
    {
      public ProductsRenderer.CommonDataMutable Common;
      public ProductsRenderer.ProductInstanceData[] Data;
      public int Count;
    }

    private struct DrawDataMutableDynamic
    {
      public ProductsRenderer.CommonDataMutable Common;
      public ProductsRenderer.ProductInstanceDataPair[] Data;
      public int Count;
    }

    private struct DrawQuatDataMutableDynamic
    {
      public ProductsRenderer.CommonDataMutable Common;
      public ProductsRenderer.ProductInstanceQDataPair[] Data;
      public int Count;
    }

    private struct PreparedDataMutableStatic
    {
      public ProductsRenderer.ProductInstanceData[] Data;
      public int WrittenCount;
    }

    private struct PreparedDataMutableDynamic
    {
      public ProductsRenderer.ProductInstanceDataPair[] Data;
      public int WrittenCount;
    }

    private struct PreparedQuatDataMutableDynamic
    {
      public ProductsRenderer.ProductInstanceQDataPair[] Data;
      public int WrittenCount;
    }
  }
}
