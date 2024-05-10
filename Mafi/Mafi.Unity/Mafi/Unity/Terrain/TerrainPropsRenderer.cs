// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Terrain.TerrainPropsRenderer
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.GameLoop;
using Mafi.Core.Numerics;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Props;
using Mafi.Unity.InstancedRendering;
using Mafi.Unity.Utils;
using Mafi.Utils;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Terrain
{
  /// <summary>
  /// Renders terrain props using chunk-based instanced rendering.
  /// </summary>
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class TerrainPropsRenderer : IDisposable
  {
    private readonly TerrainPropsManager m_terrainPropsManager;
    private readonly ChunkBasedRenderingManager m_chunksRenderer;
    private readonly AssetsDb m_assetsDb;
    private readonly ObjectHighlighter m_highlighter;
    private readonly TerrainRenderer m_terrainRenderer;
    private readonly ReloadAfterAssetUpdateManager m_reloadManager;
    private readonly ImmutableArray<TerrainPropProto> m_allPropProtos;
    private readonly Dict<TerrainPropId, ushort> m_propInstanceIds;
    private readonly Dict<uint, TerrainPropsRenderer.TerrainPropChunkRenderer> m_chunks;
    private ImmutableArray<Tupple<Option<Mesh[]>, Material, Material>> m_propMeshesAndMaterials;
    private readonly Material m_instancedMaterialShared;
    private readonly Material m_highlightMaterialShared;
    private readonly Lyst<TerrainPropData> m_propsAddedOnSim;
    private readonly Lyst<TerrainPropData> m_propsRemovedOnSim;

    public TerrainPropsRenderer(
      IGameLoopEvents gameLoopEvents,
      TerrainPropsManager terrainPropsManager,
      TerrainManager terrainManager,
      ChunkBasedRenderingManager chunksRenderer,
      ProtosDb protosDb,
      AssetsDb assetsDb,
      ObjectHighlighter highlighter,
      TerrainRenderer terrainRenderer,
      ReloadAfterAssetUpdateManager reloadManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_propInstanceIds = new Dict<TerrainPropId, ushort>();
      this.m_chunks = new Dict<uint, TerrainPropsRenderer.TerrainPropChunkRenderer>();
      this.m_propsAddedOnSim = new Lyst<TerrainPropData>();
      this.m_propsRemovedOnSim = new Lyst<TerrainPropData>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_terrainPropsManager = terrainPropsManager;
      this.m_chunksRenderer = chunksRenderer;
      this.m_assetsDb = assetsDb;
      this.m_highlighter = highlighter;
      this.m_terrainRenderer = terrainRenderer;
      this.m_reloadManager = reloadManager;
      this.m_allPropProtos = protosDb.All<TerrainPropProto>().OrderBy<TerrainPropProto, Proto.ID>((Func<TerrainPropProto, Proto.ID>) (x => x.Id)).ToImmutableArray<TerrainPropProto>();
      this.m_instancedMaterialShared = this.m_assetsDb.GetSharedMaterial("Assets/Base/Terrain/Props/TerrainPropInstanced.mat");
      this.m_highlightMaterialShared = this.m_assetsDb.GetSharedMaterial("Assets/Base/Terrain/Props/TerrainPropHighlightInstanced.mat");
      this.m_instancedMaterialShared.DisableKeyword("USE_TERRAIN_TEXTURES");
      this.m_highlightMaterialShared.DisableKeyword("USE_TERRAIN_TEXTURES");
      gameLoopEvents.RegisterRendererInitState((object) this, new Action(this.initState));
      gameLoopEvents.SyncUpdate.AddNonSaveable<TerrainPropsRenderer>(this, new Action<GameTime>(this.syncUpdate));
      terrainRenderer.TexturesReloaded += new Action(this.terrainTexturesReloaded);
    }

    public void Dispose()
    {
      foreach (TerrainPropsRenderer.TerrainPropChunkRenderer propChunkRenderer in this.m_chunks.Values)
        propChunkRenderer.Dispose();
      this.m_terrainPropsManager.PropRemoved -= new Action<TerrainPropData>(this.removePropOnSim);
      this.m_terrainPropsManager.PropAdded -= new Action<TerrainPropData>(this.addPropOnSim);
    }

    private void removePropOnSim(TerrainPropData propData)
    {
      this.m_propsRemovedOnSim.Add(propData);
    }

    private void addPropOnSim(TerrainPropData propData) => this.m_propsAddedOnSim.Add(propData);

    private bool tryGetOrCreateChunk(
      TerrainPropData propData,
      out TerrainPropsRenderer.TerrainPropChunkRenderer chunk)
    {
      int num = this.m_allPropProtos.IndexOf(propData.Proto);
      Chunk256Index chunkIndex = this.m_chunksRenderer.GetChunkIndex((Tile2i) propData.Id.Position);
      uint key = (uint) (num << 16) | (uint) chunkIndex.Value;
      if (!this.m_chunks.TryGetValue(key, out chunk))
      {
        Tupple<Option<Mesh[]>, Material, Material> meshesAndMaterial = this.m_propMeshesAndMaterials[num];
        if (meshesAndMaterial.First.IsNone || !(bool) (UnityEngine.Object) meshesAndMaterial.Second)
          return false;
        if (!(bool) (UnityEngine.Object) meshesAndMaterial.Third)
          Log.Error("No highlight material found");
        chunk = new TerrainPropsRenderer.TerrainPropChunkRenderer(this.m_chunksRenderer.ExtendChunkCoord(chunkIndex), num, this, meshesAndMaterial.First.Value, meshesAndMaterial.Second, meshesAndMaterial.Third);
        this.m_chunksRenderer.RegisterChunk((IRenderedChunk) chunk);
        this.m_chunks.Add(key, chunk);
      }
      return true;
    }

    private void addProp(TerrainPropData propData)
    {
      TerrainPropsRenderer.TerrainPropChunkRenderer chunk;
      if (!this.tryGetOrCreateChunk(propData, out chunk))
        return;
      this.m_propInstanceIds.AddAndAssertNew(propData.Id, chunk.AddProp(propData));
    }

    private void syncUpdate(GameTime time)
    {
      if (this.m_propsRemovedOnSim.IsEmpty && this.m_propsAddedOnSim.IsEmpty)
        return;
      foreach (TerrainPropData terrainPropData in this.m_propsRemovedOnSim)
      {
        int index = this.m_propsAddedOnSim.IndexOf(terrainPropData);
        if (index >= 0)
        {
          this.m_propsAddedOnSim.RemoveAtReplaceWithLast(index);
        }
        else
        {
          TerrainPropsRenderer.TerrainPropChunkRenderer propChunkRenderer;
          if (!this.m_chunks.TryGetValue((uint) (this.m_allPropProtos.IndexOf(terrainPropData.Proto) << 16) | (uint) this.m_chunksRenderer.GetChunkIndex((Tile2i) terrainPropData.Id.Position).Value, out propChunkRenderer))
          {
            Log.Error(string.Format("Failed to find chunk to remove prop `{0}` at `{1}`.", (object) terrainPropData.Proto, (object) terrainPropData.Position));
          }
          else
          {
            ushort instanceId;
            if (!this.m_propInstanceIds.TryGetValue(terrainPropData.Id, out instanceId))
            {
              Log.Error(string.Format("Failed to find instance ID for prop `{0}` with key `{1}`.", (object) terrainPropData.Proto, (object) terrainPropData.Id));
            }
            else
            {
              propChunkRenderer.RemoveProp(instanceId);
              this.m_propInstanceIds.Remove(terrainPropData.Id);
            }
          }
        }
      }
      this.m_propsRemovedOnSim.Clear();
      if (this.m_propsAddedOnSim.IsEmpty)
        return;
      foreach (TerrainPropData propData in this.m_propsAddedOnSim)
        this.addProp(propData);
      this.m_propsAddedOnSim.Clear();
    }

    private void initState()
    {
      Assert.That<Texture2DArray>(this.m_terrainRenderer.DiffuseArray).IsValidUnityObject<Texture2DArray>();
      Assert.That<Texture2DArray>(this.m_terrainRenderer.NormalSaoArray).IsValidUnityObject<Texture2DArray>();
      this.m_propMeshesAndMaterials = this.m_allPropProtos.Map<Tupple<Option<Mesh[]>, Material, Material>>((Func<TerrainPropProto, Tupple<Option<Mesh[]>, Material, Material>>) (x =>
      {
        Mesh[] meshes;
        Material sharedMaterial;
        string error;
        if (!InstancingUtils.TryGetMeshLodsAndMaterialFromPrefab(this.m_assetsDb, x.Graphics.PrefabPath, out meshes, out sharedMaterial, out error))
        {
          Log.Error("Failed to load prop: " + error);
          return new Tupple<Option<Mesh[]>, Material, Material>();
        }
        Material second;
        Material third;
        if (x.Graphics.UseTerrainTextures)
        {
          second = UnityEngine.Object.Instantiate<Material>(this.m_instancedMaterialShared);
          second.EnableKeyword("USE_TERRAIN_TEXTURES");
          third = UnityEngine.Object.Instantiate<Material>(this.m_highlightMaterialShared);
          third.EnableKeyword("USE_TERRAIN_TEXTURES");
        }
        else
        {
          second = UnityEngine.Object.Instantiate<Material>(sharedMaterial);
          second.DisableKeyword("USE_TERRAIN_TEXTURES");
          third = InstancingUtils.InstantiateMaterialAndCopyTextures(this.m_highlightMaterialShared, sharedMaterial);
          third.DisableKeyword("USE_TERRAIN_TEXTURES");
        }
        return Tupple.Create<Option<Mesh[]>, Material, Material>(Option<Mesh[]>.Create(meshes), second, third);
      }));
      this.m_chunks.Clear();
      foreach (TerrainPropData propData in this.m_terrainPropsManager.TerrainProps.Values)
        this.addProp(propData);
      this.m_terrainPropsManager.PropAdded += new Action<TerrainPropData>(this.addPropOnSim);
      this.m_terrainPropsManager.PropRemoved += new Action<TerrainPropData>(this.removePropOnSim);
      this.m_propsAddedOnSim.Clear();
      this.m_propsRemovedOnSim.Clear();
    }

    private void terrainTexturesReloaded()
    {
      foreach (TerrainPropsRenderer.TerrainPropChunkRenderer propChunkRenderer in this.m_chunks.Values)
        propChunkRenderer.UpdateTerrainTextures();
    }

    public ushort AddHighlight(TerrainPropData propData, ColorRgba color)
    {
      TerrainPropsRenderer.TerrainPropChunkRenderer chunk;
      return !this.tryGetOrCreateChunk(propData, out chunk) ? (ushort) 0 : chunk.AddHighlight(propData, color);
    }

    public void RemoveHighlight(TerrainPropData propData, ushort instanceId)
    {
      TerrainPropsRenderer.TerrainPropChunkRenderer chunk;
      if (!this.tryGetOrCreateChunk(propData, out chunk))
        return;
      chunk.RemoveHighlight(instanceId);
    }

    /// <summary>Handles rendering of one type of terrain prop.</summary>
    private sealed class TerrainPropChunkRenderer : IRenderedChunk, IRenderedChunksBase, IDisposable
    {
      private IRenderedChunksParent m_chunkParent;
      private Bounds m_bounds;
      private float m_minHeight;
      private float m_maxHeight;
      private float m_maxModelDeviation;
      private readonly int m_propIndex;
      private readonly TerrainPropsRenderer m_parentRenderer;
      private readonly TerrainPropProto m_propProto;
      private readonly InstancedMeshesRenderer<TerrainPropsRenderer.PropInstanceData> m_instancedRenderer;
      private readonly InstancedMeshesRenderer<TerrainPropsRenderer.PropInstanceData> m_highlightsChunk;
      private bool m_isRegisteredForHighlight;

      public string Name => this.m_propProto.Id.Value;

      public Vector2 Origin { get; }

      public Chunk256AndIndex CoordAndIndex { get; }

      public bool TrackStoppedRendering => false;

      public float MaxModelDeviationFromChunkBounds => this.m_maxModelDeviation;

      public Vector2 MinMaxHeight => new Vector2(this.m_minHeight, this.m_maxHeight);

      public int InstancesCount => this.m_instancedRenderer.InstancesCount;

      public TerrainPropChunkRenderer(
        Chunk256AndIndex coordAndIndex,
        int propIndex,
        TerrainPropsRenderer parentRenderer,
        Mesh[] meshLods,
        Material sharedMaterial,
        Material sharedHighlightMaterial)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.CoordAndIndex = coordAndIndex;
        this.Origin = this.CoordAndIndex.OriginTile2i.ToVector2();
        this.m_propIndex = propIndex;
        this.m_parentRenderer = parentRenderer;
        this.m_propProto = parentRenderer.m_allPropProtos[propIndex];
        this.m_instancedRenderer = new InstancedMeshesRenderer<TerrainPropsRenderer.PropInstanceData>(meshLods, UnityEngine.Object.Instantiate<Material>(sharedMaterial), layer: Layer.Custom12Terrain);
        this.m_highlightsChunk = new InstancedMeshesRenderer<TerrainPropsRenderer.PropInstanceData>(meshLods, UnityEngine.Object.Instantiate<Material>(sharedHighlightMaterial), layer: Layer.Custom12Terrain, shadowCastingMode: ShadowCastingMode.Off);
        parentRenderer.m_reloadManager.Register((IReloadAfterAssetUpdate) this.m_instancedRenderer);
        parentRenderer.m_reloadManager.Register((IReloadAfterAssetUpdate) this.m_highlightsChunk);
        this.UpdateTerrainTextures();
      }

      public void Dispose()
      {
        this.m_parentRenderer.m_reloadManager.TryUnregisterAndDispose<InstancedMeshesRenderer<TerrainPropsRenderer.PropInstanceData>>(this.m_instancedRenderer);
        if (this.m_isRegisteredForHighlight)
        {
          this.m_isRegisteredForHighlight = false;
          this.m_parentRenderer.m_highlighter.RemoveCustomHighlightsRenderer((ICustomHighlightsRenderer) this.m_highlightsChunk);
        }
        this.m_parentRenderer.m_reloadManager.TryUnregisterAndDispose<InstancedMeshesRenderer<TerrainPropsRenderer.PropInstanceData>>(this.m_highlightsChunk);
      }

      public void ReportAllRenderedInstances(Lyst<RenderedInstancesInfo> instances)
      {
        instances.Add(new RenderedInstancesInfo("Terrain prop: " + this.m_propProto.Id.Value, this.m_instancedRenderer.InstancesCount, this.m_instancedRenderer.IndicesCountForLod0));
      }

      private TerrainPropsRenderer.PropInstanceData packInstanceData(
        TerrainPropData propData,
        ColorRgba color)
      {
        uint num1 = (uint) (((float) (((double) propData.RotationYaw.CosAsFloat() + 1.0) * (double) sbyte.MaxValue)).RoundToInt() | (propData.RotationYaw.SinAsFloat() + 1f).RoundToInt() * (int) sbyte.MaxValue << 8);
        uint num2 = (uint) (((float) (((double) propData.RotationPitch.CosAsFloat() + 1.0) * (double) sbyte.MaxValue)).RoundToInt() | (propData.RotationPitch.SinAsFloat() + 1f).RoundToInt() * (int) sbyte.MaxValue << 8);
        uint num3 = (uint) (((float) (((double) propData.RotationRoll.CosAsFloat() + 1.0) * (double) sbyte.MaxValue)).RoundToInt() | (propData.RotationRoll.SinAsFloat() + 1f).RoundToInt() * (int) sbyte.MaxValue << 8);
        uint num4 = (uint) ((propData.Scale.ToFloat() * 64f).RoundToInt() & (int) byte.MaxValue);
        uint num5 = (uint) ((float) (((double) propData.PlacementHeightOffset.ToUnityUnits() + 4.0) * 32.0)).RoundToInt().Clamp(0, (int) byte.MaxValue);
        uint num6 = propData.Variant.UvOriginAndSizePackedTexIndex;
        if (this.m_propProto.Graphics.UseTerrainTextures)
          num6 = (uint) ((int) num6 & 16777215 | (int) this.m_parentRenderer.m_terrainRenderer.GetMaterialTextureIndex(new TerrainMaterialSlimId((byte) (num6 >> 24))) << 24);
        Fix32 fix32 = propData.Position.X * 2;
        double x = (double) fix32.ToFloat();
        fix32 = propData.Position.Y * 2;
        double y = (double) fix32.ToFloat();
        double unityUnits = (double) propData.PlacedAtHeight.ToUnityUnits();
        int rotCosSinScaleHeightOffsetPacked = (int) num5 << 24 | (int) num4 << 16 | (int) num1;
        int rotPitchRollPacked = (int) num2 | (int) num3 << 16;
        int uvOriginAndSize = (int) num6;
        ColorRgba color1 = color;
        return new TerrainPropsRenderer.PropInstanceData((float) x, (float) y, (float) unityUnits, (uint) rotCosSinScaleHeightOffsetPacked, (uint) rotPitchRollPacked, (uint) uvOriginAndSize, color1);
      }

      public ushort AddProp(TerrainPropData propData)
      {
        ushort num = this.m_instancedRenderer.AddInstance(this.packInstanceData(propData, ColorRgba.Empty));
        this.m_maxModelDeviation = this.m_maxModelDeviation.Max(propData.Proto.Extents.X.ScaledBy(propData.Scale).ToFloat()).Max(propData.Proto.Extents.Y.ScaledBy(propData.Scale).ToFloat());
        float unityUnits = propData.PlacedAtHeight.ToUnityUnits();
        if (this.InstancesCount == 0)
        {
          this.m_minHeight = this.m_maxHeight = unityUnits;
          this.m_bounds = ChunkBasedRenderingManager.GetChunkBounds((IRenderedChunk) this);
          this.m_chunkParent.NotifyHeightRangeChanged(this.m_minHeight, this.m_maxHeight);
        }
        else if ((double) unityUnits < (double) this.m_minHeight)
        {
          this.m_minHeight = unityUnits;
          this.m_bounds = ChunkBasedRenderingManager.GetChunkBounds((IRenderedChunk) this);
          this.m_chunkParent.NotifyHeightRangeChanged(this.m_minHeight, this.m_maxHeight);
        }
        else if ((double) unityUnits > (double) this.m_maxHeight)
        {
          this.m_maxHeight = unityUnits;
          this.m_bounds = ChunkBasedRenderingManager.GetChunkBounds((IRenderedChunk) this);
          this.m_chunkParent.NotifyHeightRangeChanged(this.m_minHeight, this.m_maxHeight);
        }
        return num;
      }

      public void RemoveProp(ushort instanceId)
      {
        this.m_instancedRenderer.RemoveInstance(instanceId);
      }

      public ushort AddHighlight(TerrainPropData propData, ColorRgba color)
      {
        return this.m_highlightsChunk.AddInstance(this.packInstanceData(propData, color));
      }

      public void RemoveHighlight(ushort instanceId)
      {
        this.m_highlightsChunk.RemoveInstance(instanceId);
      }

      public RenderStats Render(GameTime time, float cameraDistance, int lod, float pxPerMeter)
      {
        RenderStats renderStats = this.m_instancedRenderer.Render(this.m_bounds, lod);
        if (this.m_highlightsChunk.InstancesCount > 0)
        {
          if (!this.m_isRegisteredForHighlight)
          {
            this.m_isRegisteredForHighlight = true;
            this.m_parentRenderer.m_highlighter.AddCustomHighlightsRenderer((ICustomHighlightsRenderer) this.m_highlightsChunk);
          }
          renderStats += new RenderStats(1, this.m_highlightsChunk.InstancesCount, this.m_highlightsChunk.RenderedInstancesCount, this.m_highlightsChunk.RenderedInstancesCount * this.m_highlightsChunk.IndicesCountForLod0);
        }
        else if (this.m_isRegisteredForHighlight)
        {
          this.m_isRegisteredForHighlight = false;
          this.m_parentRenderer.m_highlighter.RemoveCustomHighlightsRenderer((ICustomHighlightsRenderer) this.m_highlightsChunk);
        }
        return renderStats;
      }

      public void Register(IRenderedChunksParent parent) => this.m_chunkParent = parent;

      public void NotifyWasNotRendered()
      {
      }

      public void UpdateTerrainTextures()
      {
        if (!this.m_propProto.Graphics.UseTerrainTextures)
          return;
        Material material1 = (Material) null;
        foreach (Material material2 in this.m_instancedRenderer.Materials)
        {
          if (!((UnityEngine.Object) material2 == (UnityEngine.Object) material1))
          {
            Assert.That<bool>(material2.IsKeywordEnabled("USE_TERRAIN_TEXTURES")).IsTrue();
            material2.SetTexture("_DiffuseArray", (Texture) this.m_parentRenderer.m_terrainRenderer.DiffuseArray);
            material2.SetTexture("_NormalSaoArray", (Texture) this.m_parentRenderer.m_terrainRenderer.NormalSaoArray);
            material1 = material2;
          }
        }
      }
    }

    /// <summary>
    /// Per-instance data that is passed to GPU. Layout of this struct must match the shader.
    /// </summary>
    [ExpectedStructSize(28)]
    private readonly struct PropInstanceData
    {
      public readonly float X;
      public readonly float Y;
      public readonly float Z;
      public readonly uint RotCosSinScaleHeightOffsetPacked;
      public readonly uint RotPitchRollPacked;
      public readonly uint UvOriginAndSize;
      public readonly uint Color;

      public PropInstanceData(
        float x,
        float y,
        float z,
        uint rotCosSinScaleHeightOffsetPacked,
        uint rotPitchRollPacked,
        uint uvOriginAndSize,
        ColorRgba color)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.X = x;
        this.Y = y;
        this.Z = z;
        this.RotCosSinScaleHeightOffsetPacked = rotCosSinScaleHeightOffsetPacked;
        this.RotPitchRollPacked = rotPitchRollPacked;
        this.UvOriginAndSize = uvOriginAndSize;
        this.Color = color.Rgba;
      }

      public override string ToString()
      {
        return string.Format("({0}, {1}, {2}) data 0x{3:X8} ", (object) this.X, (object) this.Y, (object) this.Z, (object) this.RotCosSinScaleHeightOffsetPacked) + string.Format("0x{0:X8} uv 0x{1:X8}", (object) this.RotPitchRollPacked, (object) this.UvOriginAndSize);
      }
    }
  }
}
