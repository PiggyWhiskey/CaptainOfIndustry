// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Terrain.TerrainSurfaceTextureManager
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.GameLoop;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Surfaces;
using Mafi.Unity.Utils;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Terrain
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class TerrainSurfaceTextureManager : IDisposable
  {
    private readonly AssetsDb m_assetsDb;
    private readonly TileSurfacesSlimIdManager m_surfaceSlimIdManager;
    private readonly TileSurfaceDecalsSlimIdManager m_surfaceDecalsSlimIdManager;

    public Texture2DArray DiffuseSurfacesArray { get; private set; }

    public Texture2DArray NormalSurfacesArray { get; private set; }

    public Texture2DArray SmoothMetalSurfaceArray { get; private set; }

    public Texture2DArray DiffuseSurfacesEdgeArrayFull { get; private set; }

    public Texture2DArray NormalSurfacesEdgeArrayFull { get; private set; }

    public Texture2DArray SmoothMetalSurfacesEdgeArrayFull { get; private set; }

    public Texture2DArray DiffuseSurfacesEdgeArrayHorizontal { get; private set; }

    public Texture2DArray NormalSurfacesEdgeArrayHorizontal { get; private set; }

    public Texture2DArray SmoothMetalSurfacesEdgeArrayHorizontal { get; private set; }

    public Texture2DArray DiffuseSurfacesEdgeArrayVertical { get; private set; }

    public Texture2DArray NormalSurfacesEdgeArrayVertical { get; private set; }

    public Texture2DArray SmoothMetalSurfacesEdgeArrayVertical { get; private set; }

    public Texture2DArray DiffuseSurfacesEdgeArrayCorners { get; private set; }

    public Texture2DArray NormalSurfacesEdgeArrayCorners { get; private set; }

    public Texture2DArray SmoothMetalSurfacesEdgeArrayCorners { get; private set; }

    public Texture2DArray DiffuseSurfaceDecalsArray { get; private set; }

    public TerrainSurfaceTextureManager(
      IGameLoopEvents gameLoopEvents,
      AssetsDb assetsDb,
      TileSurfacesSlimIdManager surfaceSlimIdManager,
      TileSurfaceDecalsSlimIdManager surfaceDecalsSlimIdManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_assetsDb = assetsDb;
      this.m_surfaceSlimIdManager = surfaceSlimIdManager;
      this.m_surfaceDecalsSlimIdManager = surfaceDecalsSlimIdManager;
      gameLoopEvents.RegisterRendererInitState((object) this, new Action(this.initState));
    }

    private void initState()
    {
      ImmutableArray<TerrainTileSurfaceProto> immutableArray = this.m_surfaceSlimIdManager.ManagedProtos.RemoveAt(0);
      Lyst<string> lyst1 = new Lyst<string>();
      Lyst<string> lyst2 = new Lyst<string>();
      Lyst<string> lyst3 = new Lyst<string>();
      foreach (TerrainTileSurfaceProto tileSurfaceProto in immutableArray)
      {
        lyst1.AddRange(tileSurfaceProto.Graphics.TextureSpec.AlbedoTexturePaths);
        lyst2.AddRange(tileSurfaceProto.Graphics.TextureSpec.NormalTexturePaths);
        lyst3.AddRange(tileSurfaceProto.Graphics.TextureSpec.SmoothMetalTexturePaths);
      }
      this.DiffuseSurfacesArray = TextureArrayUtils.Create2DArrayOrThrow("TileSurfaceDiffArray", lyst1.ToArrayAndClear(), this.m_assetsDb, false);
      this.NormalSurfacesArray = TextureArrayUtils.Create2DArrayOrThrow("TileSurfaceNormArray", lyst2.ToArrayAndClear(), this.m_assetsDb, true);
      this.SmoothMetalSurfaceArray = TextureArrayUtils.Create2DArrayOrThrow("TileSurfaceSmoothMetalArray", lyst3.ToArrayAndClear(), this.m_assetsDb, true);
      this.DiffuseSurfacesEdgeArrayFull = TextureArrayUtils.Create2DArrayOrThrow("TileSurfaceEdgeDiffArray", immutableArray.MapArray<string>((Func<TerrainTileSurfaceProto, string>) (x => x.Graphics.EdgesSpec.AlbedoTextureEdgeFullPath)), this.m_assetsDb, false);
      this.NormalSurfacesEdgeArrayFull = TextureArrayUtils.Create2DArrayOrThrow("TileSurfaceEdgeNormArray", immutableArray.MapArray<string>((Func<TerrainTileSurfaceProto, string>) (x => x.Graphics.EdgesSpec.NormalsTextureEdgeFullPath)), this.m_assetsDb, true);
      this.SmoothMetalSurfacesEdgeArrayFull = TextureArrayUtils.Create2DArrayOrThrow("TileSurfaceEdgeSmoothMetalArray", immutableArray.MapArray<string>((Func<TerrainTileSurfaceProto, string>) (x => x.Graphics.EdgesSpec.SmoothMetalTextureEdgeFullPath)), this.m_assetsDb, true);
      this.DiffuseSurfacesEdgeArrayHorizontal = TextureArrayUtils.Create2DArrayOrThrow("TileSurfaceEdgeDiffArray", immutableArray.MapArray<string>((Func<TerrainTileSurfaceProto, string>) (x => x.Graphics.EdgesSpec.AlbedoTextureEdgeHorizontalPath)), this.m_assetsDb, false);
      this.NormalSurfacesEdgeArrayHorizontal = TextureArrayUtils.Create2DArrayOrThrow("TileSurfaceEdgeNormArray", immutableArray.MapArray<string>((Func<TerrainTileSurfaceProto, string>) (x => x.Graphics.EdgesSpec.NormalsTextureEdgeHorizontalPath)), this.m_assetsDb, true);
      this.SmoothMetalSurfacesEdgeArrayHorizontal = TextureArrayUtils.Create2DArrayOrThrow("TileSurfaceEdgeSmoothMetalArray", immutableArray.MapArray<string>((Func<TerrainTileSurfaceProto, string>) (x => x.Graphics.EdgesSpec.SmoothMetalTextureEdgeHorizontalPath)), this.m_assetsDb, true);
      this.DiffuseSurfacesEdgeArrayVertical = TextureArrayUtils.Create2DArrayOrThrow("TileSurfaceEdgeDiffArray", immutableArray.MapArray<string>((Func<TerrainTileSurfaceProto, string>) (x => x.Graphics.EdgesSpec.AlbedoTextureEdgeVerticalPath)), this.m_assetsDb, false);
      this.NormalSurfacesEdgeArrayVertical = TextureArrayUtils.Create2DArrayOrThrow("TileSurfaceEdgeNormArray", immutableArray.MapArray<string>((Func<TerrainTileSurfaceProto, string>) (x => x.Graphics.EdgesSpec.NormalsTextureEdgeVerticalPath)), this.m_assetsDb, true);
      this.SmoothMetalSurfacesEdgeArrayVertical = TextureArrayUtils.Create2DArrayOrThrow("TileSurfaceEdgeSmoothMetalArray", immutableArray.MapArray<string>((Func<TerrainTileSurfaceProto, string>) (x => x.Graphics.EdgesSpec.SmoothMetalTextureEdgeVerticalPath)), this.m_assetsDb, true);
      this.DiffuseSurfacesEdgeArrayCorners = TextureArrayUtils.Create2DArrayOrThrow("TileSurfaceEdgeDiffArray", immutableArray.MapArray<string>((Func<TerrainTileSurfaceProto, string>) (x => x.Graphics.EdgesSpec.AlbedoTextureEdgeCornersPath)), this.m_assetsDb, false);
      this.NormalSurfacesEdgeArrayCorners = TextureArrayUtils.Create2DArrayOrThrow("TileSurfaceEdgeNormArray", immutableArray.MapArray<string>((Func<TerrainTileSurfaceProto, string>) (x => x.Graphics.EdgesSpec.NormalsTextureEdgeCornersPath)), this.m_assetsDb, true);
      string[] textureAssetPaths1 = immutableArray.MapArray<string>((Func<TerrainTileSurfaceProto, string>) (x => x.Graphics.EdgesSpec.SmoothMetalTextureEdgeCornersPath));
      AssetsDb assetsDb1 = this.m_assetsDb;
      TextureFormat? enforceFormat1 = new TextureFormat?();
      TextureWrapMode? nullable = new TextureWrapMode?();
      TextureWrapMode? enforceWarpMode1 = nullable;
      FilterMode? enforceFilterMode1 = new FilterMode?();
      int? enforceAnisoLevel1 = new int?();
      this.SmoothMetalSurfacesEdgeArrayCorners = TextureArrayUtils.Create2DArrayOrThrow("TileSurfaceEdgeSmoothMetalArray", textureAssetPaths1, assetsDb1, true, enforceFormat1, enforceWarpMode1, enforceFilterMode1, enforceAnisoLevel1);
      string[] textureAssetPaths2 = this.m_surfaceDecalsSlimIdManager.ManagedProtos.RemoveAt(0).MapArray<string>((Func<TerrainTileSurfaceDecalProto, string>) (x => x.Graphics.AlbedoTexturePath));
      AssetsDb assetsDb2 = this.m_assetsDb;
      nullable = new TextureWrapMode?(TextureWrapMode.Clamp);
      TextureFormat? enforceFormat2 = new TextureFormat?();
      TextureWrapMode? enforceWarpMode2 = nullable;
      FilterMode? enforceFilterMode2 = new FilterMode?();
      int? enforceAnisoLevel2 = new int?();
      this.DiffuseSurfaceDecalsArray = TextureArrayUtils.Create2DArrayOrThrow("TileSurfaceDecalDiffArray", textureAssetPaths2, assetsDb2, false, enforceFormat2, enforceWarpMode2, enforceFilterMode2, enforceAnisoLevel2);
    }

    public void Dispose()
    {
      this.DiffuseSurfacesArray.DestroyIfNotNull();
      this.NormalSurfacesArray.DestroyIfNotNull();
      this.SmoothMetalSurfaceArray.DestroyIfNotNull();
      this.DiffuseSurfacesEdgeArrayFull.DestroyIfNotNull();
      this.NormalSurfacesEdgeArrayFull.DestroyIfNotNull();
      this.SmoothMetalSurfacesEdgeArrayFull.DestroyIfNotNull();
      this.DiffuseSurfacesEdgeArrayHorizontal.DestroyIfNotNull();
      this.NormalSurfacesEdgeArrayHorizontal.DestroyIfNotNull();
      this.SmoothMetalSurfacesEdgeArrayHorizontal.DestroyIfNotNull();
      this.DiffuseSurfacesEdgeArrayVertical.DestroyIfNotNull();
      this.NormalSurfacesEdgeArrayVertical.DestroyIfNotNull();
      this.SmoothMetalSurfacesEdgeArrayVertical.DestroyIfNotNull();
      this.DiffuseSurfacesEdgeArrayCorners.DestroyIfNotNull();
      this.NormalSurfacesEdgeArrayCorners.DestroyIfNotNull();
      this.SmoothMetalSurfacesEdgeArrayCorners.DestroyIfNotNull();
      this.DiffuseSurfaceDecalsArray.DestroyIfNotNull();
    }
  }
}
