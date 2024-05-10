// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.Generators.CellEdgeTerrainGenerator
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Map;
using Mafi.Core.Products;
using Mafi.Numerics;
using Mafi.Random.Noise;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Terrain.Generation.Generators
{
  [GenerateSerializer(false, null, 0)]
  public class CellEdgeTerrainGenerator : 
    ITerrainResourceGenerator,
    ITerrainResource,
    ITerrainResourceChunkGenerator
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly MapCellId Cell1;
    public readonly MapCellId Cell2;
    public readonly int DeltaPriority;
    public readonly TerrainMaterialProto CliffResourceProto;
    public readonly RelTile1i TransitionRadius;
    public readonly RelTile1i ExtraTransitionRadiusPerHeightDiff;
    public readonly Percent BoundaryComplexity;
    /// <summary>
    /// Radius of area on top of the edge before transition starts.
    /// </summary>
    public readonly RelTile1i TopRadius;
    public readonly ThicknessTilesI ExtraThickness;
    public readonly SimplexNoise2dSeed BlobShapeSeed;
    public readonly NoiseTurbulenceParams TurbulenceParams;
    public readonly bool ClampTerrainAboveTopCell;
    private INoise2D m_boundaryNoise;
    private Line2i m_edgeLine;
    private MapCell m_topCell;
    private MapCell m_bottomCell;
    private RelTile1i m_maxTotalRadius;

    public static void Serialize(CellEdgeTerrainGenerator value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<CellEdgeTerrainGenerator>(value))
        return;
      writer.EnqueueDataSerialization((object) value, CellEdgeTerrainGenerator.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      SimplexNoise2dSeed.Serialize(this.BlobShapeSeed, writer);
      Percent.Serialize(this.BoundaryComplexity, writer);
      MapCellId.Serialize(this.Cell1, writer);
      MapCellId.Serialize(this.Cell2, writer);
      writer.WriteBool(this.ClampTerrainAboveTopCell);
      writer.WriteGeneric<TerrainMaterialProto>(this.CliffResourceProto);
      writer.WriteInt(this.DeltaPriority);
      ThicknessTilesI.Serialize(this.ExtraThickness, writer);
      RelTile1i.Serialize(this.ExtraTransitionRadiusPerHeightDiff, writer);
      MapCell.Serialize(this.m_bottomCell, writer);
      writer.WriteGeneric<INoise2D>(this.m_boundaryNoise);
      Line2i.Serialize(this.m_edgeLine, writer);
      RelTile1i.Serialize(this.m_maxTotalRadius, writer);
      MapCell.Serialize(this.m_topCell, writer);
      RelTile1i.Serialize(this.MaxRadius, writer);
      Tile3i.Serialize(this.Position, writer);
      RelTile1i.Serialize(this.TopRadius, writer);
      RelTile1i.Serialize(this.TransitionRadius, writer);
      NoiseTurbulenceParams.Serialize(this.TurbulenceParams, writer);
    }

    public static CellEdgeTerrainGenerator Deserialize(BlobReader reader)
    {
      CellEdgeTerrainGenerator terrainGenerator;
      if (reader.TryStartClassDeserialization<CellEdgeTerrainGenerator>(out terrainGenerator))
        reader.EnqueueDataDeserialization((object) terrainGenerator, CellEdgeTerrainGenerator.s_deserializeDataDelayedAction);
      return terrainGenerator;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<CellEdgeTerrainGenerator>(this, "BlobShapeSeed", (object) SimplexNoise2dSeed.Deserialize(reader));
      reader.SetField<CellEdgeTerrainGenerator>(this, "BoundaryComplexity", (object) Percent.Deserialize(reader));
      reader.SetField<CellEdgeTerrainGenerator>(this, "Cell1", (object) MapCellId.Deserialize(reader));
      reader.SetField<CellEdgeTerrainGenerator>(this, "Cell2", (object) MapCellId.Deserialize(reader));
      reader.SetField<CellEdgeTerrainGenerator>(this, "ClampTerrainAboveTopCell", (object) reader.ReadBool());
      reader.SetField<CellEdgeTerrainGenerator>(this, "CliffResourceProto", (object) reader.ReadGenericAs<TerrainMaterialProto>());
      reader.SetField<CellEdgeTerrainGenerator>(this, "DeltaPriority", (object) reader.ReadInt());
      reader.SetField<CellEdgeTerrainGenerator>(this, "ExtraThickness", (object) ThicknessTilesI.Deserialize(reader));
      reader.SetField<CellEdgeTerrainGenerator>(this, "ExtraTransitionRadiusPerHeightDiff", (object) RelTile1i.Deserialize(reader));
      this.m_bottomCell = MapCell.Deserialize(reader);
      this.m_boundaryNoise = reader.ReadGenericAs<INoise2D>();
      this.m_edgeLine = Line2i.Deserialize(reader);
      this.m_maxTotalRadius = RelTile1i.Deserialize(reader);
      this.m_topCell = MapCell.Deserialize(reader);
      this.MaxRadius = RelTile1i.Deserialize(reader);
      this.Position = Tile3i.Deserialize(reader);
      reader.SetField<CellEdgeTerrainGenerator>(this, "TopRadius", (object) RelTile1i.Deserialize(reader));
      reader.SetField<CellEdgeTerrainGenerator>(this, "TransitionRadius", (object) RelTile1i.Deserialize(reader));
      reader.SetField<CellEdgeTerrainGenerator>(this, "TurbulenceParams", (object) NoiseTurbulenceParams.Deserialize(reader));
    }

    public string Name
    {
      get
      {
        return string.Format("Edge terrain between cells {0} and {1}", (object) this.Cell1, (object) this.Cell2);
      }
    }

    public Tile3i Position { get; private set; }

    public RelTile1i MaxRadius { get; private set; }

    public int Priority => 1000 + this.DeltaPriority;

    public ColorRgba ResourceColor => this.CliffResourceProto.Graphics.Color.Rgba;

    [LoadCtor]
    protected internal CellEdgeTerrainGenerator(
      MapCellId cell1,
      MapCellId cell2,
      int deltaPriority,
      TerrainMaterialProto cliffResourceProto,
      RelTile1i transitionRadius,
      RelTile1i extraTransitionRadiusPerHeightDiff,
      Percent boundaryComplexity,
      RelTile1i topRadius,
      ThicknessTilesI extraThickness,
      SimplexNoise2dSeed blobShapeSeed,
      NoiseTurbulenceParams turbulenceParams,
      bool clampTerrainAboveTopCell)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Assert.That<MapCellId>(cell1).IsNotEqualTo<MapCellId>(cell2, "Invalid cell IDs.");
      this.Cell1 = cell1;
      this.Cell2 = cell2;
      this.DeltaPriority = deltaPriority;
      this.CliffResourceProto = cliffResourceProto;
      this.TransitionRadius = transitionRadius.CheckPositive();
      this.ExtraTransitionRadiusPerHeightDiff = extraTransitionRadiusPerHeightDiff.CheckNotNegative();
      this.BoundaryComplexity = boundaryComplexity.CheckWithinIncl(Percent.Zero, Percent.Hundred);
      this.TopRadius = topRadius.CheckPositive();
      this.BlobShapeSeed = blobShapeSeed.CheckIsValid();
      this.ExtraThickness = extraThickness.CheckNotNegative();
      this.TurbulenceParams = turbulenceParams;
      this.ClampTerrainAboveTopCell = clampTerrainAboveTopCell;
    }

    public void Initialize(IslandMap map, bool isEditorPreview = false)
    {
      if (!map.IsValidCellId(this.Cell1) || !map.IsValidCellId(this.Cell2))
      {
        Log.Error("Invalid cell for cell edge generator.");
      }
      else
      {
        MapCell mapCell1 = map[this.Cell1];
        MapCell mapCell2 = map[this.Cell2];
        if (!mapCell1.ValidNeighbors.Contains(mapCell2))
        {
          Log.Error(string.Format("Configured cells are not neighbors. Cell #{0} does not have neighbor #{1} ", (object) this.Cell1, (object) this.Cell2) + "but it has neighbors: " + mapCell1.NeighborCellsValidIndices.Select<string>((Func<MapCellId, string>) (x => "#" + x.ToString())).JoinStrings(", ") + ".");
        }
        else
        {
          this.m_topCell = mapCell1;
          this.m_bottomCell = mapCell2;
          if (this.m_topCell.GroundHeight < this.m_bottomCell.GroundHeight)
            Swap.Them<MapCell>(ref this.m_topCell, ref this.m_bottomCell);
          this.m_edgeLine = this.m_topCell.GetEdgeToNeighbor(this.m_bottomCell);
          ThicknessTilesI thicknessTilesI = this.m_topCell.GroundHeight - this.m_bottomCell.GroundHeight;
          RelTile1i relTile1i = this.TransitionRadius + this.ExtraTransitionRadiusPerHeightDiff * thicknessTilesI.Value;
          Fix32 fix32;
          if (!this.BlobShapeSeed.IsValid || this.BoundaryComplexity.IsZero)
          {
            this.m_boundaryNoise = (INoise2D) new ConstantNoise2D((Fix32) 0);
          }
          else
          {
            fix32 = (Percent.Hundred - this.BoundaryComplexity).Apply(Fix32.Tau / 4);
            Fix32 period = fix32.Tan() + (Fix32) (this.ExtraTransitionRadiusPerHeightDiff * thicknessTilesI.Value).Value;
            fix32 = period.Log(this.TurbulenceParams.Lacunarity.ToFix32());
            int octavesCount = (fix32.ToIntFloored() - 1).Clamp(0, this.TurbulenceParams.OctavesCount);
            this.m_boundaryNoise = new SimplexNoise2D(this.BlobShapeSeed, Fix32.Zero, Fix32.One, period).Turbulence(this.BlobShapeSeed, new NoiseTurbulenceParams(octavesCount, this.TurbulenceParams.Lacunarity, this.TurbulenceParams.Persistence));
          }
          this.m_boundaryNoise = (INoise2D) this.m_boundaryNoise.Sum((INoise2D) new LineDistanceNoise(this.m_edgeLine.Line2f, (Fix32) this.TopRadius.Value, (Fix32) relTile1i.Value, (Fix64) 2L, true, (Fix64) -1L, false, LineTransitionFn.Linear));
          Tile2i vector = new Tile2i(this.m_edgeLine.CenterPt);
          HeightTilesF heightTilesF = this.m_bottomCell.GroundHeight.HeightTilesF;
          heightTilesF = heightTilesF.Average(this.m_topCell.GroundHeight.HeightTilesF);
          int z = heightTilesF.TilesHeightRounded.Value;
          this.Position = new Tile3i(vector, z);
          this.m_maxTotalRadius = this.TopRadius + relTile1i;
          fix32 = this.m_edgeLine.SegmentLength / 2;
          this.MaxRadius = new RelTile1i(fix32.ToIntCeiled()) + this.m_maxTotalRadius;
        }
      }
    }

    public ITerrainResourceChunkGenerator CreateChunkGenerator(Chunk2i chunkCoord)
    {
      return (ITerrainResourceChunkGenerator) this;
    }

    public void GenerateResource(Tile2i coord, TerrainGenerationBuffer resultBuffer)
    {
      if (this.m_edgeLine.DistanceSqrToLineSegmentApprox(coord.Vector2i) > this.m_maxTotalRadius.Squared)
        return;
      Percent percent = Percent.FromFix64(this.m_boundaryNoise.GetValue(coord.Vector2f));
      HeightTilesI groundHeight = this.m_bottomCell.GroundHeight;
      HeightTilesF heightTilesF1 = groundHeight.HeightTilesF;
      ref HeightTilesF local = ref heightTilesF1;
      groundHeight = this.m_topCell.GroundHeight;
      HeightTilesF heightTilesF2 = groundHeight.HeightTilesF;
      Percent t = percent;
      HeightTilesF topHeight = local.Lerp(heightTilesF2, t);
      groundHeight = this.m_bottomCell.GroundHeight;
      HeightTilesF bottomHeight1 = groundHeight.HeightTilesF.Min(resultBuffer.SurfaceHeight) - this.ExtraThickness.ThicknessTilesF;
      if (topHeight <= bottomHeight1)
        return;
      if (this.ClampTerrainAboveTopCell)
      {
        HeightTilesF heightTilesF3 = topHeight;
        groundHeight = this.m_topCell.GroundHeight;
        HeightTilesF heightTilesF4 = groundHeight.HeightTilesF;
        if (heightTilesF3 >= heightTilesF4)
        {
          this.m_topCell.SurfaceGenerator.GenerateSurfaceAt(this.m_topCell, coord, resultBuffer, true);
          TerrainGenerationBuffer generationBuffer = resultBuffer;
          TerrainMaterialProto cliffResourceProto = this.CliffResourceProto;
          HeightTilesF bottomHeight2 = bottomHeight1;
          groundHeight = this.m_topCell.GroundHeight;
          HeightTilesF heightTilesF5 = groundHeight.HeightTilesF;
          generationBuffer.SetProductInRange(cliffResourceProto, bottomHeight2, heightTilesF5, false);
          return;
        }
      }
      if (!(resultBuffer.SurfaceHeight < topHeight))
        return;
      resultBuffer.SetProductInRange(this.CliffResourceProto, bottomHeight1, topHeight, true);
    }

    public void ChunkGenerationDone(ChunkTerrainData data)
    {
    }

    static CellEdgeTerrainGenerator()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      CellEdgeTerrainGenerator.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((CellEdgeTerrainGenerator) obj).SerializeData(writer));
      CellEdgeTerrainGenerator.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((CellEdgeTerrainGenerator) obj).DeserializeData(reader));
    }
  }
}
