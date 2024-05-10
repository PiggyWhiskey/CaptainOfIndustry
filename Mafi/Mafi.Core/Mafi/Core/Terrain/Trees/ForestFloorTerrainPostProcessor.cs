// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Trees.ForestFloorTerrainPostProcessor
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Map;
using Mafi.Core.Products;
using Mafi.Core.Terrain.Generation;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Terrain.Trees
{
  [GenerateSerializer(false, null, 0)]
  public class ForestFloorTerrainPostProcessor : ITerrainPostProcessor
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private static readonly ThicknessTilesF MAX_FLOOR_THICKNESS_FROM_ONE_TREE;
    private static readonly ThicknessTilesF MIN_FLOOR_THICKNESS_FROM_ONE_TREE;

    public static void Serialize(ForestFloorTerrainPostProcessor value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ForestFloorTerrainPostProcessor>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ForestFloorTerrainPostProcessor.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
    }

    public static ForestFloorTerrainPostProcessor Deserialize(BlobReader reader)
    {
      ForestFloorTerrainPostProcessor terrainPostProcessor;
      if (reader.TryStartClassDeserialization<ForestFloorTerrainPostProcessor>(out terrainPostProcessor))
        reader.EnqueueDataDeserialization((object) terrainPostProcessor, ForestFloorTerrainPostProcessor.s_deserializeDataDelayedAction);
      return terrainPostProcessor;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
    }

    public void PostProcessGeneratedIslandMap(
      IslandMap map,
      TerrainManager terrain,
      DependencyResolver resolver,
      bool gameIsBeingLoaded)
    {
      foreach (TreeData treeData in resolver.Resolve<TreesManager>().Trees.Values)
      {
        if (treeData.CreatedByTerrainGenerator)
          ForestFloorTerrainPostProcessor.processTreeAt(treeData, terrain);
      }
    }

    private static void processTreeAt(TreeData treeData, TerrainManager terrain)
    {
      TreeProto proto = treeData.Proto;
      if (proto.MaxForestFloorRadius.IsNotPositive || proto.ForestFloorMaterial.IsNone)
        return;
      Tile2i position = (Tile2i) treeData.Id.Position;
      int num1 = proto.MinForestFloorRadius.Value;
      Fix32 fix32_1 = (Fix32) num1;
      int num2 = proto.MaxForestFloorRadius.Value;
      Fix32 denominator = (Fix32) (num2 - num1);
      Fix32 fix32_2 = (Fix32) (num1 * num1);
      Fix32 fix32_3 = (Fix32) (num2 * num2);
      TerrainMaterialSlimId slimId = proto.ForestFloorMaterial.Value.SlimId;
      HeightTilesF heightTilesF = terrain.GetHeight(position) + ThicknessTilesF.Half;
      ref TerrainManager.TerrainData local1 = ref terrain.GetMutableTerrainDataRef();
      for (int y = -num2 + 1; y <= num2; ++y)
      {
        Fix32 fix32_4 = treeData.PositionWithinTile.Y - (Fix32) y;
        Fix32 fix32_5 = fix32_4 * fix32_4;
        for (int x = -num2 + 1; x <= num2; ++x)
        {
          Fix32 fix32_6 = treeData.PositionWithinTile.X - (Fix32) x;
          Fix32 fix32_7 = fix32_6 * fix32_6 + fix32_5;
          if (!(fix32_7 > fix32_3))
          {
            Tile2i tile2i = position + new RelTile2i(x, y);
            if (terrain.IsValidCoord(tile2i))
            {
              Tile2iIndex tileIndex = terrain.GetTileIndex(tile2i);
              if (!(terrain.GetHeight(tileIndex) > heightTilesF))
              {
                ThicknessTilesF thickness = ForestFloorTerrainPostProcessor.MAX_FLOOR_THICKNESS_FROM_ONE_TREE;
                if (fix32_7 > fix32_2)
                {
                  Percent scale = Percent.Hundred - Percent.FromRatio(fix32_7.Sqrt() - fix32_1, denominator);
                  Assert.That<Percent>(scale).IsWithin0To100PercIncl();
                  thickness = thickness.ScaledBy(scale);
                  if (thickness < ForestFloorTerrainPostProcessor.MIN_FLOOR_THICKNESS_FROM_ONE_TREE)
                    continue;
                }
                ref TileMaterialLayers local2 = ref local1.MaterialLayers[tileIndex.Value];
                if (local2.First.SlimId == slimId)
                {
                  if (local2.First.Thickness < TreesManager.MAX_FLOOR_THICKNESS_TOTAL)
                    local2.First += thickness;
                }
                else
                  local1.PushNewFirstLayer(ref local2, new TerrainMaterialThicknessSlim(slimId, thickness));
              }
            }
          }
        }
      }
    }

    public ForestFloorTerrainPostProcessor()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static ForestFloorTerrainPostProcessor()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ForestFloorTerrainPostProcessor.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ForestFloorTerrainPostProcessor) obj).SerializeData(writer));
      ForestFloorTerrainPostProcessor.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ForestFloorTerrainPostProcessor) obj).DeserializeData(reader));
      ForestFloorTerrainPostProcessor.MAX_FLOOR_THICKNESS_FROM_ONE_TREE = 0.8.TilesThick();
      ForestFloorTerrainPostProcessor.MIN_FLOOR_THICKNESS_FROM_ONE_TREE = 0.05.TilesThick();
    }
  }
}
