// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Terrain.SmoothTerrainPostProcessor
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Map;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Generation;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Base.Terrain
{
  [GenerateSerializer(false, null, 0)]
  public class SmoothTerrainPostProcessor : ITerrainPostProcessor
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private static readonly ThicknessTilesF MAX_DELTA_PEAK;
    private static readonly ThicknessTilesF MAX_DELTA_PEAK_2DN_PASS;

    public static void Serialize(SmoothTerrainPostProcessor value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SmoothTerrainPostProcessor>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SmoothTerrainPostProcessor.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
    }

    public static SmoothTerrainPostProcessor Deserialize(BlobReader reader)
    {
      SmoothTerrainPostProcessor terrainPostProcessor;
      if (reader.TryStartClassDeserialization<SmoothTerrainPostProcessor>(out terrainPostProcessor))
        reader.EnqueueDataDeserialization((object) terrainPostProcessor, SmoothTerrainPostProcessor.s_deserializeDataDelayedAction);
      return terrainPostProcessor;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
    }

    public SmoothTerrainPostProcessor()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public void PostProcessGeneratedIslandMap(
      IslandMap map,
      TerrainManager terrain,
      DependencyResolver resolver,
      bool gameIsBeingLoaded)
    {
      Lyst<Pair<Tile2iIndex, HeightTilesF>> heightChanges = new Lyst<Pair<Tile2iIndex, HeightTilesF>>(1024, true);
      HeightTilesF[] heights = terrain.GetMutableTerrainDataRef().Heights;
      int terrainWidth = terrain.TerrainWidth;
      Pair<Tile2iAndIndexRel, Tile2iAndIndexRel>[] pairArray = new Pair<Tile2iAndIndexRel, Tile2iAndIndexRel>[12]
      {
        Pair.Create<Tile2iAndIndexRel, Tile2iAndIndexRel>(Tile2iAndIndexRel.Create(new RelTile2i(-1, 0), terrainWidth), Tile2iAndIndexRel.Create(new RelTile2i(1, 0), terrainWidth)),
        Pair.Create<Tile2iAndIndexRel, Tile2iAndIndexRel>(Tile2iAndIndexRel.Create(new RelTile2i(0, -1), terrainWidth), Tile2iAndIndexRel.Create(new RelTile2i(0, 1), terrainWidth)),
        Pair.Create<Tile2iAndIndexRel, Tile2iAndIndexRel>(Tile2iAndIndexRel.Create(new RelTile2i(-1, -1), terrainWidth), Tile2iAndIndexRel.Create(new RelTile2i(1, 1), terrainWidth)),
        Pair.Create<Tile2iAndIndexRel, Tile2iAndIndexRel>(Tile2iAndIndexRel.Create(new RelTile2i(-1, 1), terrainWidth), Tile2iAndIndexRel.Create(new RelTile2i(1, -1), terrainWidth)),
        Pair.Create<Tile2iAndIndexRel, Tile2iAndIndexRel>(Tile2iAndIndexRel.Create(new RelTile2i(0, 1), terrainWidth), Tile2iAndIndexRel.Create(new RelTile2i(1, 0), terrainWidth)),
        Pair.Create<Tile2iAndIndexRel, Tile2iAndIndexRel>(Tile2iAndIndexRel.Create(new RelTile2i(0, -1), terrainWidth), Tile2iAndIndexRel.Create(new RelTile2i(-1, 0), terrainWidth)),
        Pair.Create<Tile2iAndIndexRel, Tile2iAndIndexRel>(Tile2iAndIndexRel.Create(new RelTile2i(1, 0), terrainWidth), Tile2iAndIndexRel.Create(new RelTile2i(0, 1), terrainWidth)),
        Pair.Create<Tile2iAndIndexRel, Tile2iAndIndexRel>(Tile2iAndIndexRel.Create(new RelTile2i(-1, 0), terrainWidth), Tile2iAndIndexRel.Create(new RelTile2i(0, -1), terrainWidth)),
        Pair.Create<Tile2iAndIndexRel, Tile2iAndIndexRel>(Tile2iAndIndexRel.Create(new RelTile2i(-1, 1), terrainWidth), Tile2iAndIndexRel.Create(new RelTile2i(-1, -1), terrainWidth)),
        Pair.Create<Tile2iAndIndexRel, Tile2iAndIndexRel>(Tile2iAndIndexRel.Create(new RelTile2i(1, 1), terrainWidth), Tile2iAndIndexRel.Create(new RelTile2i(1, -1), terrainWidth)),
        Pair.Create<Tile2iAndIndexRel, Tile2iAndIndexRel>(Tile2iAndIndexRel.Create(new RelTile2i(-1, -1), terrainWidth), Tile2iAndIndexRel.Create(new RelTile2i(1, -1), terrainWidth)),
        Pair.Create<Tile2iAndIndexRel, Tile2iAndIndexRel>(Tile2iAndIndexRel.Create(new RelTile2i(-1, 1), terrainWidth), Tile2iAndIndexRel.Create(new RelTile2i(1, 1), terrainWidth))
      };
      foreach (Pair<Tile2iAndIndexRel, Tile2iAndIndexRel> pair in pairArray)
        processTerrain(pair.First.IndexDelta, pair.Second.IndexDelta, SmoothTerrainPostProcessor.MAX_DELTA_PEAK);
      foreach (Pair<Tile2iAndIndexRel, Tile2iAndIndexRel> pair in pairArray)
        processTerrain(pair.First.IndexDelta, pair.Second.IndexDelta, SmoothTerrainPostProcessor.MAX_DELTA_PEAK_2DN_PASS);

      void processTerrain(int tileIndexDelta1, int tileIndexDelta2, ThicknessTilesF maxDelta)
      {
        foreach (Tile2iIndex skipBoundaryTile in terrain.EnumerateAllTileIndicesSkipBoundaryTiles())
        {
          HeightTilesF height = heights[skipBoundaryTile.Value];
          ThicknessTilesF thicknessTilesF1 = heights[(skipBoundaryTile + tileIndexDelta1).Value] - height;
          ThicknessTilesF thicknessTilesF2;
          if (thicknessTilesF1 > maxDelta)
          {
            ThicknessTilesF other = heights[(skipBoundaryTile + tileIndexDelta2).Value] - height;
            if (other > maxDelta)
            {
              Lyst<Pair<Tile2iIndex, HeightTilesF>> heightChanges = heightChanges;
              Tile2iIndex first = skipBoundaryTile;
              HeightTilesF heightTilesF = height;
              thicknessTilesF2 = thicknessTilesF1.Average(other);
              ThicknessTilesF halfFast = thicknessTilesF2.HalfFast;
              HeightTilesF second = heightTilesF + halfFast;
              Pair<Tile2iIndex, HeightTilesF> pair = Pair.Create<Tile2iIndex, HeightTilesF>(first, second);
              heightChanges.Add(pair);
            }
          }
          else if (thicknessTilesF1 < -maxDelta)
          {
            ThicknessTilesF other = heights[(skipBoundaryTile + tileIndexDelta2).Value] - height;
            if (other < -maxDelta)
            {
              Lyst<Pair<Tile2iIndex, HeightTilesF>> heightChanges = heightChanges;
              Tile2iIndex first = skipBoundaryTile;
              HeightTilesF heightTilesF = height;
              thicknessTilesF2 = thicknessTilesF1.Average(other);
              ThicknessTilesF halfFast = thicknessTilesF2.HalfFast;
              HeightTilesF second = heightTilesF + halfFast;
              Pair<Tile2iIndex, HeightTilesF> pair = Pair.Create<Tile2iIndex, HeightTilesF>(first, second);
              heightChanges.Add(pair);
            }
          }
        }
        foreach (Pair<Tile2iIndex, HeightTilesF> heightChange in heightChanges)
          heights[heightChange.First.Value] = heightChange.Second;
        heightChanges.Clear();
      }
    }

    static SmoothTerrainPostProcessor()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      SmoothTerrainPostProcessor.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((SmoothTerrainPostProcessor) obj).SerializeData(writer));
      SmoothTerrainPostProcessor.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((SmoothTerrainPostProcessor) obj).DeserializeData(reader));
      SmoothTerrainPostProcessor.MAX_DELTA_PEAK = 0.7.TilesThick();
      SmoothTerrainPostProcessor.MAX_DELTA_PEAK_2DN_PASS = 0.5.TilesThick();
    }
  }
}
