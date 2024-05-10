// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InstancedRendering.InstancedMeshesRenderer`1
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Unity.Utils;
using Mafi.Utils;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InstancedRendering
{
  /// <summary>
  /// Efficient rendering of large number of meshes using instanced with LOD support.
  /// This class can be also registered for custom highlighting via the <see cref="T:Mafi.Unity.ICustomHighlightsRenderer" /> interface.
  /// </summary>
  /// <typeparam name="TInstanceData">Data passed to the GPU buffer.</typeparam>
  public class InstancedMeshesRenderer<TInstanceData> : 
    IDisposable,
    ICustomHighlightsRenderer,
    IReloadAfterAssetUpdate
    where TInstanceData : struct
  {
    private static readonly int VERTICES_PER_MESH_SHADER_ID;
    private static readonly int INSTANCES_PER_MESH_SHADER_ID;
    private static readonly int MESH_COUNT_SHADER_ID;
    private readonly int m_instanceDataSizeBytes;
    private Mesh[] m_sharedMeshLods;
    private Material[] m_materials;
    private readonly MaterialPropertyBlock[] m_materialPropertyBlocks;
    private readonly int[] m_instancesPerMesh;
    private readonly bool m_receiveShadows;
    private ShadowCastingMode m_shadowCastingMode;
    private readonly int m_layer;
    private ArrayDataStorageSlim<TInstanceData> m_data;
    private int m_removalCounter;
    private bool m_needsUpdate;
    private ComputeBuffer m_drawArgsBuffer;
    private readonly uint[] m_drawArgs;
    private ComputeBuffer m_detailsDataBuffer;
    private int m_numToRender;

    public string MeshName { get; private set; }

    public Material[] Materials => this.m_materials;

    public bool IsInitialized => this.m_sharedMeshLods != null;

    public ReadOnlyArraySlice<TInstanceData> Data => this.m_data.Data;

    public int InstancesCount => this.m_data.Count;

    public int RenderedInstancesCount => this.m_data.HighestUsedCount;

    public int IndicesCountForLod0 => (int) this.m_drawArgs[0];

    public InstancedMeshesRenderer(
      Mesh[] sharedMeshLods,
      Material nonSharedMaterial,
      int initialCapacity = 0,
      Layer layer = Layer.Unity00Default,
      bool doNotReceiveShadows = false,
      ShadowCastingMode shadowCastingMode = ShadowCastingMode.On)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      this.\u002Ector(initialCapacity, layer, doNotReceiveShadows, shadowCastingMode);
      this.DelayedInitialize(sharedMeshLods, nonSharedMaterial);
    }

    public InstancedMeshesRenderer(
      Mesh[] sharedMeshLods,
      Material[] nonSharedMaterials,
      int initialCapacity = 0,
      Layer layer = Layer.Unity00Default,
      bool doNotReceiveShadows = false,
      ShadowCastingMode shadowCastingMode = ShadowCastingMode.On)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      this.\u002Ector(initialCapacity, layer, doNotReceiveShadows, shadowCastingMode);
      this.DelayedInitialize(sharedMeshLods, nonSharedMaterials);
    }

    public InstancedMeshesRenderer(
      Pair<Mesh, Material>[] sharedMeshMatLods,
      Material overrideNonSharedMaterial,
      int initialCapacity = 0,
      Layer layer = Layer.Unity00Default,
      bool doNotReceiveShadows = false,
      ShadowCastingMode shadowCastingMode = ShadowCastingMode.On)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      this.\u002Ector(initialCapacity, layer, doNotReceiveShadows, shadowCastingMode);
      Mesh[] sharedMeshLods = new Mesh[sharedMeshMatLods.Length];
      Material[] nonSharedMaterials = new Material[sharedMeshMatLods.Length];
      for (int index = 0; index < sharedMeshMatLods.Length; ++index)
      {
        sharedMeshLods[index] = sharedMeshMatLods[index].First;
        nonSharedMaterials[index] = overrideNonSharedMaterial;
      }
      this.DelayedInitialize(sharedMeshLods, nonSharedMaterials);
    }

    private InstancedMeshesRenderer(
      int initialCapacity = 0,
      Layer layer = Layer.Unity00Default,
      bool doNotReceiveShadows = false,
      ShadowCastingMode shadowCastingMode = ShadowCastingMode.On)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_materialPropertyBlocks = new MaterialPropertyBlock[7];
      this.m_instancesPerMesh = new int[7];
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_instanceDataSizeBytes = Marshal.SizeOf(typeof (TInstanceData));
      ExpectedStructSizeAttribute customAttribute = typeof (TInstanceData).GetCustomAttribute<ExpectedStructSizeAttribute>();
      if (customAttribute == null)
        Log.Error("Type '" + typeof (TInstanceData).FullName + "' is missing ExpectedStructSizeAttribute attribute.");
      else if (customAttribute.SizeBytes != this.m_instanceDataSizeBytes)
        Log.Error("Type '" + typeof (TInstanceData).FullName + "' has invalid byte size," + string.Format(" the {0} attribute specifies {1} but", (object) "ExpectedStructSizeAttribute", (object) customAttribute.SizeBytes) + string.Format(" the struct is actually {0} bytes, unexpected packing?", (object) this.m_instanceDataSizeBytes));
      this.m_receiveShadows = !doNotReceiveShadows;
      this.m_shadowCastingMode = shadowCastingMode;
      this.m_layer = layer.ToId();
      this.m_drawArgs = new uint[35];
      if (initialCapacity <= 0)
        return;
      this.m_data.EnsureCapacity(initialCapacity);
    }

    public static InstancedMeshesRenderer<TInstanceData> CreateUninitialized(
      int initialCapacity = 0,
      Layer layer = Layer.Unity00Default,
      bool doNotReceiveShadows = false,
      ShadowCastingMode shadowCastingMode = ShadowCastingMode.On)
    {
      return new InstancedMeshesRenderer<TInstanceData>(initialCapacity, layer, doNotReceiveShadows, shadowCastingMode);
    }

    public void DelayedInitialize(Mesh[] sharedMeshLods, Material nonSharedMaterial)
    {
      Material[] nonSharedMaterials = new Material[sharedMeshLods.Length];
      for (int index = 0; index < nonSharedMaterials.Length; ++index)
        nonSharedMaterials[index] = nonSharedMaterial;
      this.DelayedInitialize(sharedMeshLods, nonSharedMaterials);
    }

    public void DelayedInitialize(Mesh[] sharedMeshLods, Material[] nonSharedMaterials)
    {
      Assert.That<int>(nonSharedMaterials.Length).IsEqualTo(sharedMeshLods.Length);
      foreach (Material nonSharedMaterial in nonSharedMaterials)
        Assert.That<Material>(nonSharedMaterial).IsValidUnityObject<Material>();
      if (sharedMeshLods.Length != 7)
      {
        Log.Error(string.Format("Mesh count {0} does not match LODs count {1}.", (object) sharedMeshLods.Length, (object) 7));
        Array.Resize<Mesh>(ref sharedMeshLods, 7);
      }
      bool expected = nonSharedMaterials[0].HasProperty(InstancedMeshesRenderer<TInstanceData>.INSTANCES_PER_MESH_SHADER_ID) && nonSharedMaterials[0].HasProperty(InstancedMeshesRenderer<TInstanceData>.VERTICES_PER_MESH_SHADER_ID) && nonSharedMaterials[0].HasProperty(InstancedMeshesRenderer<TInstanceData>.MESH_COUNT_SHADER_ID);
      for (int index = 1; index < nonSharedMaterials.Length; ++index)
        Assert.That<bool>(nonSharedMaterials[index].HasProperty(InstancedMeshesRenderer<TInstanceData>.INSTANCES_PER_MESH_SHADER_ID) && nonSharedMaterials[index].HasProperty(InstancedMeshesRenderer<TInstanceData>.VERTICES_PER_MESH_SHADER_ID) && nonSharedMaterials[index].HasProperty(InstancedMeshesRenderer<TInstanceData>.MESH_COUNT_SHADER_ID)).IsEqualTo<bool>(expected);
      for (int index = 0; index < 7; ++index)
        this.m_materialPropertyBlocks[index] = new MaterialPropertyBlock();
      this.MeshName = sharedMeshLods[0]?.name ?? "n/a";
      if (this.MeshName.Length == 0)
        Log.Warning("Mesh name is empty");
      if (expected)
      {
        this.m_sharedMeshLods = new Mesh[7];
        for (int index1 = 0; index1 < 7; ++index1)
        {
          Mesh mesh = sharedMeshLods[index1];
          int length;
          int num;
          if (mesh.isReadable)
          {
            length = 256.CeilDivPositive(mesh.vertexCount);
            num = mesh.vertexCount;
          }
          else
          {
            Log.Warning("Instanced mesh " + this.MeshName + " with batched instancing properties is not readable.");
            length = 1;
            GraphicsBuffer vertexBuffer = mesh.GetVertexBuffer(0);
            num = vertexBuffer.count;
            vertexBuffer.Release();
          }
          if (length != 1)
          {
            CombineInstance[] combine = new CombineInstance[length];
            for (int index2 = 0; index2 < length; ++index2)
              combine[index2].mesh = mesh;
            mesh = new Mesh();
            mesh.CombineMeshes(combine, true, false);
          }
          this.m_sharedMeshLods[index1] = mesh;
          this.m_materialPropertyBlocks[index1].SetInteger(InstancedMeshesRenderer<TInstanceData>.VERTICES_PER_MESH_SHADER_ID, num);
          this.m_materialPropertyBlocks[index1].SetInteger(InstancedMeshesRenderer<TInstanceData>.INSTANCES_PER_MESH_SHADER_ID, length);
          this.m_instancesPerMesh[index1] = length;
        }
      }
      else
      {
        this.m_sharedMeshLods = sharedMeshLods;
        for (int index = 0; index < 7; ++index)
          this.m_instancesPerMesh[index] = 1;
      }
      this.m_materials = new Material[7];
      for (int index = 0; index < 7; ++index)
        this.m_materials[index] = nonSharedMaterials[index];
      for (int index3 = 0; index3 < 7; ++index3)
      {
        int index4 = index3 * 5;
        Mesh mesh = this.m_sharedMeshLods[index3];
        if ((UnityEngine.Object) mesh == (UnityEngine.Object) null)
        {
          Log.Error(string.Format("Mesh LOD at index {0} is null ({1}).", (object) index3, (object) this.MeshName));
          if (index3 > 0)
            mesh = this.m_sharedMeshLods[index3] = this.m_sharedMeshLods[index3 - 1];
        }
        this.m_drawArgs[index4] = mesh != null ? mesh.GetIndexCount(0) : 0U;
        this.m_drawArgs[index4 + 2] = mesh != null ? mesh.GetIndexStart(0) : 0U;
        this.m_drawArgs[index4 + 3] = mesh != null ? mesh.GetBaseVertex(0) : 0U;
      }
      this.m_drawArgsBuffer = new ComputeBuffer(7, 20, ComputeBufferType.DrawIndirect);
    }

    public void Dispose()
    {
      foreach (Material material in this.m_materials)
        material.DestroyIfNotNull();
      this.m_detailsDataBuffer?.Dispose();
      this.m_detailsDataBuffer = (ComputeBuffer) null;
      this.m_drawArgsBuffer?.Dispose();
      this.m_drawArgsBuffer = (ComputeBuffer) null;
      this.m_sharedMeshLods = (Mesh[]) null;
      this.m_materials = (Material[]) null;
    }

    public void SetShadowCastingMode(ShadowCastingMode shadowCastingMode)
    {
      this.m_shadowCastingMode = shadowCastingMode;
    }

    /// <summary>
    /// Adds an instance to the rendering buffer. Returns index of the inserted element in the backing array.
    /// GPU buffer update is deferred to the <see cref="M:Mafi.Unity.InstancedRendering.InstancedMeshesRenderer`1.Render(UnityEngine.Bounds,System.Int32)" /> call.
    /// </summary>
    public ushort AddInstance(TInstanceData data)
    {
      this.m_needsUpdate = true;
      return this.m_data.Add(data);
    }

    /// <summary>
    /// Updates an existing instance on the given index. It is callers responsibility to ensure that the given index
    /// is valid.
    /// </summary>
    public void UpdateInstance(ushort index, TInstanceData newData)
    {
      this.m_needsUpdate = true;
      this.m_data.UpdateAt(index, newData);
    }

    /// <summary>
    /// When instance data changes without this class knowing about it, for example when using <see cref="M:Mafi.Unity.InstancedRendering.InstancedMeshesRenderer`1.GetDataRef(System.UInt16)" />.
    /// </summary>
    public void NotifyDataUpdated(ushort instanceId) => this.m_needsUpdate = true;

    /// <summary>Returns data on the given index.</summary>
    public TInstanceData GetData(ushort index) => this.m_data[(int) index];

    /// <summary>
    /// If you change returned ref data, don't forget to call <see cref="M:Mafi.Unity.InstancedRendering.InstancedMeshesRenderer`1.NotifyDataUpdated(System.UInt16)" />.
    /// </summary>
    public ref TInstanceData GetDataRef(ushort index)
    {
      return ref this.m_data.GetBackingArray()[(int) index];
    }

    /// <summary>
    /// Removes an instance to the rendering buffer. Caller must ensure that the index is valid. Removing the same index
    /// more than once is not supported and will result in corrupted datastructure.
    /// GPU buffer update is deferred to the <see cref="M:Mafi.Unity.InstancedRendering.InstancedMeshesRenderer`1.Render(UnityEngine.Bounds,System.Int32)" /> call.
    /// </summary>
    public void RemoveInstance(ushort index)
    {
      this.m_needsUpdate = true;
      this.m_data.Remove(index);
      ++this.m_removalCounter;
      if (this.m_removalCounter < 256)
        return;
      this.m_removalCounter = 0;
      this.m_data.RecomputeHighestUsedCount();
    }

    /// <summary>Efficiently clears all data.</summary>
    public void Clear()
    {
      this.m_needsUpdate = true;
      this.m_data.Clear();
    }

    public void ClearFromIndex(int index)
    {
      this.m_needsUpdate = true;
      this.m_data.ClearFromIndex(index);
    }

    private bool updateRenderData()
    {
      this.m_needsUpdate = false;
      if (this.m_data.IsEmpty)
        return false;
      bool flag = false;
      int highestUsedCount = this.m_data.HighestUsedCount;
      if (this.m_detailsDataBuffer == null || this.m_detailsDataBuffer.count < highestUsedCount)
      {
        this.m_detailsDataBuffer?.Dispose();
        if (this.m_materials == null)
        {
          Log.Error("Failed to update data, materials is null, mesh '" + this.MeshName + "'.");
          this.m_detailsDataBuffer = (ComputeBuffer) null;
          return false;
        }
        this.m_detailsDataBuffer = new ComputeBuffer(this.m_data.Capacity, this.m_instanceDataSizeBytes);
        Material material1 = (Material) null;
        foreach (Material material2 in this.m_materials)
        {
          if ((UnityEngine.Object) material2 == (UnityEngine.Object) null)
          {
            Log.Error("Failed to update data, materials is null, mesh '" + this.MeshName + "'.");
            this.m_detailsDataBuffer = (ComputeBuffer) null;
            return false;
          }
          if (!((UnityEngine.Object) material2 == (UnityEngine.Object) material1))
          {
            material2.SetBuffer(MafiMaterials.INSTANCE_DATA_SHADER_ID, this.m_detailsDataBuffer);
            material1 = material2;
          }
        }
        flag = true;
        this.m_detailsDataBuffer.SetData((Array) this.m_data.GetBackingArray(), 0, 0, highestUsedCount);
      }
      else
        this.m_detailsDataBuffer.SetData((Array) this.m_data.GetBackingArray(), 0, 0, highestUsedCount);
      return flag;
    }

    private void updateNumToRender(int numToRender)
    {
      this.m_numToRender = numToRender;
      for (int index = 0; index < 7; ++index)
        this.m_drawArgs[5 * index + 1] = (uint) numToRender.CeilDivPositive(this.m_instancesPerMesh[index]);
      this.m_drawArgsBuffer.SetData((Array) this.m_drawArgs);
      Material material1 = (Material) null;
      foreach (Material material2 in this.m_materials)
      {
        if (!((UnityEngine.Object) material2 == (UnityEngine.Object) material1))
        {
          material2.SetInteger(InstancedMeshesRenderer<TInstanceData>.MESH_COUNT_SHADER_ID, numToRender);
          material1 = material2;
        }
      }
    }

    public void UpdateRenderDataIfNeeded()
    {
      if (!this.m_needsUpdate)
        return;
      this.updateRenderData();
    }

    /// <summary>
    /// Updates GPU instancing buffers if they were changed and schedules instancing draw call with
    /// <see cref="M:UnityEngine.Graphics.DrawMeshInstancedIndirect(UnityEngine.Mesh,System.Int32,UnityEngine.Material,UnityEngine.Bounds,UnityEngine.ComputeBuffer,System.Int32,UnityEngine.MaterialPropertyBlock,UnityEngine.Rendering.ShadowCastingMode,System.Boolean,System.Int32,UnityEngine.Camera,UnityEngine.Rendering.LightProbeUsage,UnityEngine.LightProbeProxyVolume)" />.
    /// </summary>
    public RenderStats Render(Bounds bounds, int lod)
    {
      if (this.m_drawArgs == null || this.m_drawArgsBuffer == null)
      {
        Log.Error("Failed to UpdateDataIfNeeded, instanced mesh renderer for '" + this.MeshName + "' is already disposed.");
        return new RenderStats();
      }
      if (this.m_needsUpdate)
        this.updateRenderData();
      return this.renderNoDataUpdate(bounds, lod, this.m_data.HighestUsedCount);
    }

    public RenderStats Render(Bounds bounds, int lod, int numToRender)
    {
      if (this.m_drawArgs == null || this.m_drawArgsBuffer == null)
      {
        Log.Error("Failed to UpdateDataIfNeeded, instanced mesh renderer for '" + this.MeshName + "' is already disposed.");
        return new RenderStats();
      }
      if (this.m_needsUpdate)
        this.updateRenderData();
      return this.renderNoDataUpdate(bounds, lod, numToRender);
    }

    public RenderStats RenderNoDataUpdate(Bounds bounds, int lod)
    {
      if (this.m_drawArgs != null && this.m_drawArgsBuffer != null)
        return this.renderNoDataUpdate(bounds, lod, this.m_data.HighestUsedCount);
      Log.Error("Failed to UpdateDataIfNeeded, instanced mesh renderer for '" + this.MeshName + "' is already disposed.");
      return new RenderStats();
    }

    public RenderStats RenderNoDataUpdate(Bounds bounds, int lod, int numToRender)
    {
      if (this.m_drawArgs != null && this.m_drawArgsBuffer != null)
        return this.renderNoDataUpdate(bounds, lod, numToRender);
      Log.Error("Failed to UpdateDataIfNeeded, instanced mesh renderer for '" + this.MeshName + "' is already disposed.");
      return new RenderStats();
    }

    private RenderStats renderNoDataUpdate(Bounds bounds, int lod, int numToRender)
    {
      if (this.m_materials == null || (UnityEngine.Object) this.m_materials[lod] == (UnityEngine.Object) null)
      {
        Log.Error("Failed to render instanced for '" + this.MeshName + "', material is invalid.");
        return new RenderStats();
      }
      if (lod < 0 || lod > 6)
      {
        Log.Error(string.Format("Invalid LOD {0} for renderer '{1}'.", (object) lod, (object) this.MeshName));
        lod = lod.Clamp(0, 6);
      }
      if (this.m_numToRender != numToRender)
        this.updateNumToRender(numToRender);
      if (numToRender <= 0)
        return new RenderStats();
      Graphics.DrawMeshInstancedIndirect(this.m_sharedMeshLods[lod], 0, this.m_materials[lod], bounds, this.m_drawArgsBuffer, lod * 20, this.m_materialPropertyBlocks[lod], this.m_shadowCastingMode, this.m_receiveShadows, this.m_layer, (Camera) null, LightProbeUsage.Off, (LightProbeProxyVolume) null);
      return new RenderStats(1, this.m_data.Count, (int) this.m_drawArgs[1], (int) this.m_drawArgs[1] * (int) this.m_drawArgs[5 * lod]);
    }

    /// <summary>
    /// Sets new material for all LODs and returns the old one at LOD0.
    /// </summary>
    public Material SetMaterial(Material mat)
    {
      Material material = this.m_materials[0];
      for (int index = 0; index < this.m_materials.Length; ++index)
        this.m_materials[index] = mat;
      if (this.m_detailsDataBuffer != null)
        mat.SetBuffer(MafiMaterials.INSTANCE_DATA_SHADER_ID, this.m_detailsDataBuffer);
      this.m_numToRender = 0;
      return material;
    }

    /// <summary>
    /// Set a float in the material which will be uniform for all instances.
    /// </summary>
    public void SetMaterialFloat(int nameId, float value)
    {
      Material material1 = (Material) null;
      foreach (Material material2 in this.m_materials)
      {
        if (!((UnityEngine.Object) material2 == (UnityEngine.Object) material1))
        {
          material2.SetFloat(nameId, value);
          material1 = material2;
        }
      }
    }

    bool ICustomHighlightsRenderer.UpdateDataIfNeeded()
    {
      if (!this.m_needsUpdate)
        return false;
      if (this.m_drawArgs == null || this.m_drawArgsBuffer == null)
      {
        Log.Error("Failed to UpdateDataIfNeeded, instanced mesh renderer for '" + this.MeshName + "' is already disposed.");
        return true;
      }
      bool flag = this.updateRenderData();
      if (this.m_numToRender != this.m_data.HighestUsedCount)
        this.updateNumToRender(this.m_data.HighestUsedCount);
      return flag;
    }

    void ICustomHighlightsRenderer.RegisterRendering(CommandBuffer commandBuffer)
    {
      if (this.m_drawArgs == null || this.m_drawArgsBuffer == null)
        Log.Error("Instanced mesh renderer for '" + this.MeshName + "' already disposed.");
      else if (this.m_materials == null || (UnityEngine.Object) this.m_materials[0] == (UnityEngine.Object) null)
        Log.Error("Failed to register instanced rendering for '" + this.MeshName + "', material is invalid.");
      else
        commandBuffer.DrawMeshInstancedIndirect(this.m_sharedMeshLods[0], 0, this.m_materials[0], 0, this.m_drawArgsBuffer, 0, this.m_materialPropertyBlocks[0]);
    }

    void IReloadAfterAssetUpdate.ReloadAfterAssetUpdate()
    {
      if (this.m_materials == null || this.m_detailsDataBuffer == null)
        return;
      Material material1 = (Material) null;
      foreach (Material material2 in this.m_materials)
      {
        if (!((UnityEngine.Object) material2 == (UnityEngine.Object) material1))
        {
          material2.SetBuffer(MafiMaterials.INSTANCE_DATA_SHADER_ID, this.m_detailsDataBuffer);
          material1 = material2;
        }
      }
    }

    static InstancedMeshesRenderer()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      InstancedMeshesRenderer<TInstanceData>.VERTICES_PER_MESH_SHADER_ID = Shader.PropertyToID("_VerticesPerMesh");
      InstancedMeshesRenderer<TInstanceData>.INSTANCES_PER_MESH_SHADER_ID = Shader.PropertyToID("_InstancesPerMesh");
      InstancedMeshesRenderer<TInstanceData>.MESH_COUNT_SHADER_ID = Shader.PropertyToID("_TotalMeshCount");
    }
  }
}
