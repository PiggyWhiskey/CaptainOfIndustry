// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.TerrainSurfaceManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Input;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain.Designation;
using Mafi.Core.Terrain.Surfaces;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Terrain
{
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public sealed class TerrainSurfaceManager : 
    ICommandProcessor<AddSurfaceDecalCmd>,
    IAction<AddSurfaceDecalCmd>,
    ICommandProcessor<BatchAddSurfaceDecalCmd>,
    IAction<BatchAddSurfaceDecalCmd>,
    ICommandProcessor<RemoveSurfaceDecalCmd>,
    IAction<RemoveSurfaceDecalCmd>,
    ICommandProcessor<BatchRemoveSurfaceDecalCmd>,
    IAction<BatchRemoveSurfaceDecalCmd>
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private static readonly ThicknessTilesF UNDERMINED_THICKNESS;
    private readonly TerrainManager m_terrainManager;
    private readonly ProtosDb m_protosDb;

    public static void Serialize(TerrainSurfaceManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TerrainSurfaceManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TerrainSurfaceManager.s_serializeDataDelayedAction);
    }

    private void SerializeData(BlobWriter writer)
    {
      TerrainManager.Serialize(this.m_terrainManager, writer);
    }

    public static TerrainSurfaceManager Deserialize(BlobReader reader)
    {
      TerrainSurfaceManager terrainSurfaceManager;
      if (reader.TryStartClassDeserialization<TerrainSurfaceManager>(out terrainSurfaceManager))
        reader.EnqueueDataDeserialization((object) terrainSurfaceManager, TerrainSurfaceManager.s_deserializeDataDelayedAction);
      return terrainSurfaceManager;
    }

    private void DeserializeData(BlobReader reader)
    {
      reader.RegisterResolvedMember<TerrainSurfaceManager>(this, "m_protosDb", typeof (ProtosDb), true);
      reader.SetField<TerrainSurfaceManager>(this, "m_terrainManager", (object) TerrainManager.Deserialize(reader));
    }

    public TerrainSurfaceManager(ProtosDb protosDb, TerrainManager terrainManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_protosDb = protosDb;
      this.m_terrainManager = terrainManager;
      this.m_terrainManager.HeightChanged.Add<TerrainSurfaceManager>(this, new Action<Tile2iAndIndex>(this.heightChanged));
    }

    void IAction<AddSurfaceDecalCmd>.Invoke(AddSurfaceDecalCmd cmd)
    {
      TerrainTileSurfaceDecalProto proto;
      if (!this.m_protosDb.TryGetProto<TerrainTileSurfaceDecalProto>(cmd.ProtoId, out proto))
        return;
      foreach (Tile2i enumerateTile in cmd.Area.EnumerateTiles())
      {
        Tile2iAndIndex tileAndIndex = this.m_terrainManager.ExtendTileIndex(enumerateTile);
        TileSurfaceData tileSurfaceData;
        if (this.m_terrainManager.TryGetTileSurface(tileAndIndex.Index, out tileSurfaceData))
        {
          TileSurfaceData surfData = new TileSurfaceData(tileSurfaceData.RawValue, proto.SlimId, cmd.IsReflected, cmd.Rotation, cmd.ColorKey);
          this.m_terrainManager.SetCustomSurface(tileAndIndex, surfData);
        }
      }
      cmd.SetResultSuccess();
    }

    void IAction<BatchAddSurfaceDecalCmd>.Invoke(BatchAddSurfaceDecalCmd cmd)
    {
      foreach (TileSurfaceCopyPasteData surfaceCopyPasteData in cmd.Data)
      {
        Tile2iAndIndex tileAndIndex = this.m_terrainManager.ExtendTileIndex(surfaceCopyPasteData.Position);
        TileSurfaceData tileSurfaceData;
        if (this.m_terrainManager.TryGetTileSurface(tileAndIndex.Index, out tileSurfaceData))
        {
          TileSurfaceData surfData = new TileSurfaceData(tileSurfaceData.RawValue, surfaceCopyPasteData.SurfaceData.DecalSlimId, surfaceCopyPasteData.SurfaceData.IsDecalFlipped, surfaceCopyPasteData.SurfaceData.DecalRotation.ToRotation(), surfaceCopyPasteData.SurfaceData.ColorKey);
          this.m_terrainManager.SetCustomSurface(tileAndIndex, surfData);
        }
      }
      cmd.SetResultSuccess();
    }

    void IAction<BatchRemoveSurfaceDecalCmd>.Invoke(BatchRemoveSurfaceDecalCmd cmd)
    {
      foreach (TileSurfaceCopyPasteData surfaceCopyPasteData in cmd.Data)
      {
        Tile2iAndIndex tileAndIndex = this.m_terrainManager.ExtendTileIndex(surfaceCopyPasteData.Position);
        TileSurfaceData tileSurfaceData;
        if (!this.m_terrainManager.TryGetTileSurface(tileAndIndex.Index, out tileSurfaceData))
        {
          Log.Warning("Removing surface decal from non-existent surface.");
        }
        else
        {
          Assert.That<bool>(tileSurfaceData.DecalSlimId.IsNotPhantom).IsTrue();
          TileSurfaceData surfData = new TileSurfaceData(tileSurfaceData.RawValue, TileSurfaceDecalSlimId.PhantomId, false, new Rotation90(), 0);
          this.m_terrainManager.SetCustomSurface(tileAndIndex, surfData);
        }
      }
      cmd.SetResultSuccess();
    }

    void IAction<RemoveSurfaceDecalCmd>.Invoke(RemoveSurfaceDecalCmd cmd)
    {
      foreach (Tile2i enumerateTile in cmd.Area.EnumerateTiles())
      {
        Tile2iAndIndex tileAndIndex = this.m_terrainManager.ExtendTileIndex(enumerateTile);
        TileSurfaceData tileSurfaceData;
        if (this.m_terrainManager.TryGetTileSurface(tileAndIndex.Index, out tileSurfaceData))
        {
          TileSurfaceData surfData = new TileSurfaceData(tileSurfaceData.RawValue, TileSurfaceDecalSlimId.PhantomId, false, new Rotation90(), 0);
          this.m_terrainManager.SetCustomSurface(tileAndIndex, surfData);
        }
      }
      cmd.SetResultSuccess();
    }

    private void heightChanged(Tile2iAndIndex tile)
    {
      TileSurfaceData tileSurfaceData;
      if (!this.m_terrainManager.TryGetTileSurface(tile.Index, out tileSurfaceData) || tileSurfaceData.IsAutoPlaced || !(this.m_terrainManager.GetHeight(tile.TileCoord.CenterTile2f) - tileSurfaceData.Height < -TerrainSurfaceManager.UNDERMINED_THICKNESS))
        return;
      this.m_terrainManager.ClearCustomSurface(tile);
    }

    static TerrainSurfaceManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TerrainSurfaceManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((TerrainSurfaceManager) obj).SerializeData(writer));
      TerrainSurfaceManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((TerrainSurfaceManager) obj).DeserializeData(reader));
      TerrainSurfaceManager.UNDERMINED_THICKNESS = ThicknessTilesF.Half;
    }
  }
}
