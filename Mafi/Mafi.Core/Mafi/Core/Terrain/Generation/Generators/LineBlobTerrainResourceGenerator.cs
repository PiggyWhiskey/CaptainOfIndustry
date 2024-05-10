// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.Generators.LineBlobTerrainResourceGenerator
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
  /// <summary>
  /// Resource generator that creates resource in a blobby capsule shape around a given line.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public class LineBlobTerrainResourceGenerator : 
    ITerrainResourceGenerator,
    ITerrainResource,
    ITerrainResourceChunkGenerator
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly Tile2i From;
    public readonly Tile2i To;
    public readonly int DeltaPriority;
    public readonly TerrainMaterialProto ResourceProto;
    public readonly RelTile1i ResourceRadius;
    public readonly RelTile1i TransitionRadius;
    public readonly Percent ExtraFalloffTransitionRadius;
    public readonly LineTransitionFn TransitionFn;
    public readonly SimplexNoise2dParams BaseNoiseParams;
    public readonly Percent BelowSurfaceExtraHeight;
    public readonly ThicknessTilesI BelowSurfaceMaxDepth;
    public readonly ThicknessTilesI ShapeInversionDepth;
    public readonly Percent GroundLevelBias;
    public readonly SimplexNoise2dSeed NoiseSeed;
    public readonly NoiseTurbulenceParams TurbulenceParams;
    public readonly SteppedNoiseParams SteppedNoiseParams;
    public readonly bool IsRidged;
    public readonly bool ReplacePreviousResource;
    public readonly bool OnlyPlaceOnTopAboveGround;
    public readonly bool OnlyReplaceExistingMaterials;
    public readonly bool LimitToParentCellHeight;
    public readonly Fix32 SigmoidCenterDistance;
    public readonly Fix32 SigmoidSmoothness;
    public readonly Option<TerrainMaterialProto> SurfaceCoverResourceProto;
    public readonly SimplexNoise2dParams SurfaceCoverThickness;
    public readonly bool UseMineableResourceConfig;
    public readonly SimplexNoise2dParams CoordWarpNoiseParams;
    public readonly ThicknessTilesF HeightBiasAtFromPoint;
    public readonly ThicknessTilesF HeightBiasAtToPoint;
    private INoise2D m_noise;
    private Line2i m_line;
    private HeightTilesF m_groundHeight;
    private RelTile1i m_maxTotalRadius;
    private Percent m_belowSurfaceHeightMult;
    private ThicknessTilesF m_belowSurfaceMaxDepth;
    private ThicknessTilesF m_shapeInversionDepthTimesTwo;
    private Option<INoise2D> m_surfaceCoverThicknessNoise;
    private MapCell m_parentCell;
    private bool m_onlyPlaceOnTopAboveGround;
    private bool m_onlyReplaceExistingMaterials;
    private bool m_limitToParentCellHeight;
    private Percent m_extraFalloffTransitionRadius;

    public static void Serialize(LineBlobTerrainResourceGenerator value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<LineBlobTerrainResourceGenerator>(value))
        return;
      writer.EnqueueDataSerialization((object) value, LineBlobTerrainResourceGenerator.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      SimplexNoise2dParams.Serialize(this.BaseNoiseParams, writer);
      Percent.Serialize(this.BelowSurfaceExtraHeight, writer);
      ThicknessTilesI.Serialize(this.BelowSurfaceMaxDepth, writer);
      SimplexNoise2dParams.Serialize(this.CoordWarpNoiseParams, writer);
      writer.WriteInt(this.DeltaPriority);
      Percent.Serialize(this.ExtraFalloffTransitionRadius, writer);
      Tile2i.Serialize(this.From, writer);
      Percent.Serialize(this.GroundLevelBias, writer);
      ThicknessTilesF.Serialize(this.HeightBiasAtFromPoint, writer);
      ThicknessTilesF.Serialize(this.HeightBiasAtToPoint, writer);
      writer.WriteBool(this.IsRidged);
      writer.WriteBool(this.LimitToParentCellHeight);
      Percent.Serialize(this.m_belowSurfaceHeightMult, writer);
      ThicknessTilesF.Serialize(this.m_belowSurfaceMaxDepth, writer);
      Percent.Serialize(this.m_extraFalloffTransitionRadius, writer);
      HeightTilesF.Serialize(this.m_groundHeight, writer);
      writer.WriteBool(this.m_limitToParentCellHeight);
      Line2i.Serialize(this.m_line, writer);
      RelTile1i.Serialize(this.m_maxTotalRadius, writer);
      writer.WriteGeneric<INoise2D>(this.m_noise);
      writer.WriteBool(this.m_onlyPlaceOnTopAboveGround);
      writer.WriteBool(this.m_onlyReplaceExistingMaterials);
      MapCell.Serialize(this.m_parentCell, writer);
      ThicknessTilesF.Serialize(this.m_shapeInversionDepthTimesTwo, writer);
      Option<INoise2D>.Serialize(this.m_surfaceCoverThicknessNoise, writer);
      RelTile1i.Serialize(this.MaxRadius, writer);
      SimplexNoise2dSeed.Serialize(this.NoiseSeed, writer);
      writer.WriteBool(this.OnlyPlaceOnTopAboveGround);
      writer.WriteBool(this.OnlyReplaceExistingMaterials);
      Tile3i.Serialize(this.Position, writer);
      writer.WriteBool(this.ReplacePreviousResource);
      writer.WriteGeneric<TerrainMaterialProto>(this.ResourceProto);
      RelTile1i.Serialize(this.ResourceRadius, writer);
      ThicknessTilesI.Serialize(this.ShapeInversionDepth, writer);
      Fix32.Serialize(this.SigmoidCenterDistance, writer);
      Fix32.Serialize(this.SigmoidSmoothness, writer);
      SteppedNoiseParams.Serialize(this.SteppedNoiseParams, writer);
      Option<TerrainMaterialProto>.Serialize(this.SurfaceCoverResourceProto, writer);
      SimplexNoise2dParams.Serialize(this.SurfaceCoverThickness, writer);
      Tile2i.Serialize(this.To, writer);
      writer.WriteInt((int) this.TransitionFn);
      RelTile1i.Serialize(this.TransitionRadius, writer);
      NoiseTurbulenceParams.Serialize(this.TurbulenceParams, writer);
      writer.WriteBool(this.UseMineableResourceConfig);
    }

    public static LineBlobTerrainResourceGenerator Deserialize(BlobReader reader)
    {
      LineBlobTerrainResourceGenerator resourceGenerator;
      if (reader.TryStartClassDeserialization<LineBlobTerrainResourceGenerator>(out resourceGenerator))
        reader.EnqueueDataDeserialization((object) resourceGenerator, LineBlobTerrainResourceGenerator.s_deserializeDataDelayedAction);
      return resourceGenerator;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<LineBlobTerrainResourceGenerator>(this, "BaseNoiseParams", (object) SimplexNoise2dParams.Deserialize(reader));
      reader.SetField<LineBlobTerrainResourceGenerator>(this, "BelowSurfaceExtraHeight", (object) Percent.Deserialize(reader));
      reader.SetField<LineBlobTerrainResourceGenerator>(this, "BelowSurfaceMaxDepth", (object) ThicknessTilesI.Deserialize(reader));
      reader.SetField<LineBlobTerrainResourceGenerator>(this, "CoordWarpNoiseParams", (object) SimplexNoise2dParams.Deserialize(reader));
      reader.SetField<LineBlobTerrainResourceGenerator>(this, "DeltaPriority", (object) reader.ReadInt());
      reader.SetField<LineBlobTerrainResourceGenerator>(this, "ExtraFalloffTransitionRadius", (object) Percent.Deserialize(reader));
      reader.SetField<LineBlobTerrainResourceGenerator>(this, "From", (object) Tile2i.Deserialize(reader));
      reader.SetField<LineBlobTerrainResourceGenerator>(this, "GroundLevelBias", (object) Percent.Deserialize(reader));
      reader.SetField<LineBlobTerrainResourceGenerator>(this, "HeightBiasAtFromPoint", (object) ThicknessTilesF.Deserialize(reader));
      reader.SetField<LineBlobTerrainResourceGenerator>(this, "HeightBiasAtToPoint", (object) ThicknessTilesF.Deserialize(reader));
      reader.SetField<LineBlobTerrainResourceGenerator>(this, "IsRidged", (object) reader.ReadBool());
      reader.SetField<LineBlobTerrainResourceGenerator>(this, "LimitToParentCellHeight", (object) reader.ReadBool());
      this.m_belowSurfaceHeightMult = Percent.Deserialize(reader);
      this.m_belowSurfaceMaxDepth = ThicknessTilesF.Deserialize(reader);
      this.m_extraFalloffTransitionRadius = Percent.Deserialize(reader);
      this.m_groundHeight = HeightTilesF.Deserialize(reader);
      this.m_limitToParentCellHeight = reader.ReadBool();
      this.m_line = Line2i.Deserialize(reader);
      this.m_maxTotalRadius = RelTile1i.Deserialize(reader);
      this.m_noise = reader.ReadGenericAs<INoise2D>();
      this.m_onlyPlaceOnTopAboveGround = reader.ReadBool();
      this.m_onlyReplaceExistingMaterials = reader.ReadBool();
      this.m_parentCell = MapCell.Deserialize(reader);
      this.m_shapeInversionDepthTimesTwo = ThicknessTilesF.Deserialize(reader);
      this.m_surfaceCoverThicknessNoise = Option<INoise2D>.Deserialize(reader);
      this.MaxRadius = RelTile1i.Deserialize(reader);
      reader.SetField<LineBlobTerrainResourceGenerator>(this, "NoiseSeed", (object) SimplexNoise2dSeed.Deserialize(reader));
      reader.SetField<LineBlobTerrainResourceGenerator>(this, "OnlyPlaceOnTopAboveGround", (object) reader.ReadBool());
      reader.SetField<LineBlobTerrainResourceGenerator>(this, "OnlyReplaceExistingMaterials", (object) reader.ReadBool());
      this.Position = Tile3i.Deserialize(reader);
      reader.SetField<LineBlobTerrainResourceGenerator>(this, "ReplacePreviousResource", (object) reader.ReadBool());
      reader.SetField<LineBlobTerrainResourceGenerator>(this, "ResourceProto", (object) reader.ReadGenericAs<TerrainMaterialProto>());
      reader.SetField<LineBlobTerrainResourceGenerator>(this, "ResourceRadius", (object) RelTile1i.Deserialize(reader));
      reader.SetField<LineBlobTerrainResourceGenerator>(this, "ShapeInversionDepth", (object) ThicknessTilesI.Deserialize(reader));
      reader.SetField<LineBlobTerrainResourceGenerator>(this, "SigmoidCenterDistance", (object) Fix32.Deserialize(reader));
      reader.SetField<LineBlobTerrainResourceGenerator>(this, "SigmoidSmoothness", (object) Fix32.Deserialize(reader));
      reader.SetField<LineBlobTerrainResourceGenerator>(this, "SteppedNoiseParams", (object) SteppedNoiseParams.Deserialize(reader));
      reader.SetField<LineBlobTerrainResourceGenerator>(this, "SurfaceCoverResourceProto", (object) Option<TerrainMaterialProto>.Deserialize(reader));
      reader.SetField<LineBlobTerrainResourceGenerator>(this, "SurfaceCoverThickness", (object) SimplexNoise2dParams.Deserialize(reader));
      reader.SetField<LineBlobTerrainResourceGenerator>(this, "To", (object) Tile2i.Deserialize(reader));
      reader.SetField<LineBlobTerrainResourceGenerator>(this, "TransitionFn", (object) (LineTransitionFn) reader.ReadInt());
      reader.SetField<LineBlobTerrainResourceGenerator>(this, "TransitionRadius", (object) RelTile1i.Deserialize(reader));
      reader.SetField<LineBlobTerrainResourceGenerator>(this, "TurbulenceParams", (object) NoiseTurbulenceParams.Deserialize(reader));
      reader.SetField<LineBlobTerrainResourceGenerator>(this, "UseMineableResourceConfig", (object) reader.ReadBool());
    }

    public string Name
    {
      get
      {
        return string.Format("Line resource '{0}' from {1} to {2}", (object) this.ResourceProto.Id, (object) this.From, (object) this.To);
      }
    }

    public Tile3i Position { get; private set; }

    public RelTile1i MaxRadius { get; private set; }

    public int Priority => 2000 + this.DeltaPriority;

    public ColorRgba ResourceColor => this.ResourceProto.Graphics.Color.Rgba;

    public LineBlobTerrainResourceGenerator(
      Tile2i from,
      Tile2i to,
      TerrainMaterialProto resourceProto,
      RelTile1i resourceRadius,
      RelTile1i transitionRadius,
      SimplexNoise2dParams baseNoiseParams,
      SimplexNoise2dSeed noiseSeed,
      ThicknessTilesI belowSurfaceMaxDepth,
      int deltaPriority = 0,
      Percent extraFalloffTransitionRadius = default (Percent),
      LineTransitionFn transitionFn = LineTransitionFn.Linear,
      Percent belowSurfaceExtraHeight = default (Percent),
      ThicknessTilesI shapeInversionDepth = default (ThicknessTilesI),
      Percent groundLevelBias = default (Percent),
      NoiseTurbulenceParams turbulenceParams = default (NoiseTurbulenceParams),
      SteppedNoiseParams steppedNoiseParams = default (SteppedNoiseParams),
      bool isRidged = false,
      bool replacePreviousResource = false,
      bool onlyReplaceExistingMaterials = false,
      bool onlyPlaceOnTopAboveGround = false,
      bool limitToParentCellHeight = false,
      Fix32 sigmoidCenterDistance = default (Fix32),
      Fix32 sigmoidSmoothness = default (Fix32),
      Option<TerrainMaterialProto> surfaceCoverResourceProto = default (Option<TerrainMaterialProto>),
      SimplexNoise2dParams surfaceCoverThickness = default (SimplexNoise2dParams),
      bool useMineableResourceConfig = false,
      SimplexNoise2dParams coordWarpNoiseParams = default (SimplexNoise2dParams),
      ThicknessTilesF heightBiasAtFromPoint = default (ThicknessTilesF),
      ThicknessTilesF heightBiasAtToPoint = default (ThicknessTilesF))
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.From = from;
      this.To = to;
      this.DeltaPriority = deltaPriority;
      this.ResourceProto = resourceProto;
      this.ResourceRadius = resourceRadius.CheckPositive();
      this.TransitionRadius = transitionRadius.CheckPositive();
      this.ExtraFalloffTransitionRadius = extraFalloffTransitionRadius;
      this.TransitionFn = transitionFn;
      this.BaseNoiseParams = baseNoiseParams;
      this.BelowSurfaceExtraHeight = belowSurfaceExtraHeight;
      this.BelowSurfaceMaxDepth = belowSurfaceMaxDepth;
      this.OnlyReplaceExistingMaterials = onlyReplaceExistingMaterials;
      this.ShapeInversionDepth = shapeInversionDepth;
      this.GroundLevelBias = groundLevelBias;
      this.NoiseSeed = noiseSeed;
      this.TurbulenceParams = turbulenceParams;
      this.SteppedNoiseParams = steppedNoiseParams;
      this.IsRidged = isRidged;
      this.ReplacePreviousResource = replacePreviousResource;
      this.OnlyPlaceOnTopAboveGround = onlyPlaceOnTopAboveGround;
      this.LimitToParentCellHeight = limitToParentCellHeight;
      this.SigmoidCenterDistance = sigmoidCenterDistance;
      this.SigmoidSmoothness = sigmoidSmoothness;
      this.SurfaceCoverResourceProto = surfaceCoverResourceProto;
      this.SurfaceCoverThickness = surfaceCoverThickness;
      this.UseMineableResourceConfig = useMineableResourceConfig;
      this.CoordWarpNoiseParams = coordWarpNoiseParams;
      this.HeightBiasAtFromPoint = heightBiasAtFromPoint;
      this.HeightBiasAtToPoint = heightBiasAtToPoint;
    }

    public void Initialize(IslandMap map, bool isEditorPreview = false)
    {
      Percent percent = this.UseMineableResourceConfig ? map.DifficultyConfig.MineableResourceSizeBonus : Percent.Zero;
      this.m_line = new Line2i(this.From.Vector2i, this.To.Vector2i);
      this.m_belowSurfaceHeightMult = Percent.Hundred + this.BelowSurfaceExtraHeight.ScaleBy(Percent.Hundred + percent / 4);
      this.m_belowSurfaceMaxDepth = this.BelowSurfaceMaxDepth.ThicknessTilesF.ScaledBy(Percent.Hundred + percent / 3);
      this.m_shapeInversionDepthTimesTwo = (2 * this.ShapeInversionDepth).ThicknessTilesF.ScaledBy(Percent.Hundred + percent / 3);
      this.m_extraFalloffTransitionRadius = this.ExtraFalloffTransitionRadius + Percent.Fifty.ScaleBy(percent).Max(Percent.Zero);
      if (this.BaseNoiseParams.Amplitude.IsNotZero)
      {
        this.m_noise = (INoise2D) new SimplexNoise2D(this.NoiseSeed, new SimplexNoise2dParams(this.BaseNoiseParams.MeanValue.ScaledBy(Percent.Hundred + percent / 8), this.BaseNoiseParams.Amplitude.ScaledBy(Percent.Hundred + percent / 4), this.BaseNoiseParams.Period));
        if (this.IsRidged)
          this.m_noise = (INoise2D) this.m_noise.Ridged();
      }
      else
        this.m_noise = (INoise2D) new ConstantNoise2D(this.BaseNoiseParams.MeanValue);
      this.m_surfaceCoverThicknessNoise = !this.SurfaceCoverResourceProto.HasValue || !this.SurfaceCoverThickness.MeanValue.IsPositive ? Option<INoise2D>.None : (this.SurfaceCoverThickness.Amplitude.IsNotZero ? (Option<INoise2D>) (INoise2D) new SimplexNoise2D(this.NoiseSeed, this.SurfaceCoverThickness) : (Option<INoise2D>) (INoise2D) new ConstantNoise2D(this.SurfaceCoverThickness.MeanValue));
      RelTile1i relTile1i1 = this.ResourceRadius.ScaledBy(Percent.Hundred + percent / 4);
      RelTile1i relTile1i2 = this.TransitionRadius.ScaledBy(Percent.Hundred + percent / 4);
      this.m_noise = this.m_noise.Turbulence(this.NoiseSeed, this.TurbulenceParams);
      this.m_noise = (INoise2D) this.m_noise.Sum((INoise2D) new LineDistanceNoise(this.m_line.Line2f, (Fix32) relTile1i1.Value, (Fix32) relTile1i2.Value, Fix64.Zero, true, -(this.BaseNoiseParams.Amplitude + this.BaseNoiseParams.MeanValue).ToFix64(), false, this.TransitionFn));
      this.m_noise = this.m_noise.Stepped(this.SteppedNoiseParams);
      this.m_noise = this.m_noise.WarpCoords(this.CoordWarpNoiseParams, new SimplexNoise2dSeed(-this.NoiseSeed.SeedY, this.NoiseSeed.SeedX));
      MapCell closestCell = map.GetClosestCell(new Tile2i(this.m_line.CenterPt));
      this.m_groundHeight = closestCell.GroundHeight.HeightTilesF + new ThicknessTilesF(this.BaseNoiseParams.Amplitude.ScaledBy(this.GroundLevelBias));
      this.m_maxTotalRadius = relTile1i1 + relTile1i2 + relTile1i2.ScaledBy(this.GroundLevelBias.Clamp0To100() + this.m_extraFalloffTransitionRadius);
      this.m_parentCell = map.GetClosestCell(new Tile2i(this.m_line.CenterPt));
      this.Position = new Tile2i(this.m_line.CenterPt).ExtendHeight(closestCell.GroundHeight);
      this.MaxRadius = new RelTile1i((this.m_line.SegmentLength / 2).ToIntCeiled()) + this.m_maxTotalRadius;
      this.m_onlyPlaceOnTopAboveGround = !isEditorPreview & this.OnlyPlaceOnTopAboveGround;
      this.m_onlyReplaceExistingMaterials = !isEditorPreview & this.OnlyReplaceExistingMaterials;
      this.m_limitToParentCellHeight = !isEditorPreview & this.LimitToParentCellHeight;
    }

    public ITerrainResourceChunkGenerator CreateChunkGenerator(Chunk2i chunkCoord)
    {
      return (ITerrainResourceChunkGenerator) this;
    }

    public void GenerateResource(Tile2i coord, TerrainGenerationBuffer resultBuffer)
    {
      if (this.m_limitToParentCellHeight && resultBuffer.SurfaceHeight != this.m_parentCell.GroundHeight.HeightTilesF)
        return;
      Percent closestTtoLine = this.m_line.GetClosestTToLine(coord.Vector2i);
      long self = !closestTtoLine.IsNotPositive ? (!(closestTtoLine >= Percent.Hundred) ? this.m_line.GetPoint(closestTtoLine).DistanceSqrTo(coord.Vector2f).ToLongFloored() : this.m_line.P1.DistanceSqrTo(coord.Vector2i)) : this.m_line.P0.DistanceSqrTo(coord.Vector2i);
      if (self > this.m_maxTotalRadius.Squared)
        return;
      ThicknessTilesF thicknessTilesF1 = new ThicknessTilesF(this.m_noise.GetValue(coord.Vector2f).ToFix32());
      ThicknessTilesF thicknessTilesF2 = -(thicknessTilesF1.ScaledBy(this.m_belowSurfaceHeightMult) + this.m_shapeInversionDepthTimesTwo).Min(this.m_belowSurfaceMaxDepth);
      if (thicknessTilesF1 - thicknessTilesF2 < ThicknessTilesF.One)
        return;
      HeightTilesF heightTilesF1 = this.m_groundHeight + this.HeightBiasAtFromPoint.Lerp(this.HeightBiasAtToPoint, closestTtoLine);
      HeightTilesF heightTilesF2 = heightTilesF1 + thicknessTilesF1;
      HeightTilesF heightTilesF3 = heightTilesF1 + thicknessTilesF2;
      if (this.SigmoidCenterDistance.IsPositive && this.SigmoidSmoothness.IsPositive)
      {
        Fix32 fix32 = ((this.SigmoidCenterDistance - self.Sqrt()) / this.SigmoidSmoothness).Sigmoid() + Fix32.One;
        fix32 = fix32.HalfFast;
        Percent percent = fix32.ToPercent();
        if (percent.IsNotPositive)
          return;
        if (percent < Percent.Hundred)
        {
          HeightTilesF surfaceHeight = resultBuffer.SurfaceHeight;
          heightTilesF2 = surfaceHeight.Lerp(heightTilesF2, percent);
          surfaceHeight = resultBuffer.SurfaceHeight;
          heightTilesF3 = surfaceHeight.Lerp(heightTilesF3, percent);
        }
      }
      if (this.m_onlyReplaceExistingMaterials)
      {
        if (heightTilesF3 >= resultBuffer.SurfaceHeight)
          return;
        if (heightTilesF2 > resultBuffer.SurfaceHeight)
          heightTilesF2 = resultBuffer.SurfaceHeight;
      }
      else if (this.m_onlyPlaceOnTopAboveGround)
      {
        if (heightTilesF2 <= resultBuffer.SurfaceHeight)
          return;
        if (heightTilesF3 < resultBuffer.SurfaceHeight)
          heightTilesF3 = resultBuffer.SurfaceHeight;
      }
      resultBuffer.SetProductInRange(this.ResourceProto, heightTilesF3, heightTilesF2, this.ReplacePreviousResource);
      if (!this.m_surfaceCoverThicknessNoise.HasValue)
        return;
      ThicknessTilesF thicknessTilesF3 = new ThicknessTilesF(this.m_surfaceCoverThicknessNoise.Value.GetValue(coord.Vector2f).ToFix32());
      if (!thicknessTilesF3.IsPositive)
        return;
      resultBuffer.SetProductInRange(this.SurfaceCoverResourceProto.Value, heightTilesF2 - thicknessTilesF3, heightTilesF2, this.ReplacePreviousResource);
    }

    public void ChunkGenerationDone(ChunkTerrainData data)
    {
    }

    static LineBlobTerrainResourceGenerator()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      LineBlobTerrainResourceGenerator.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((LineBlobTerrainResourceGenerator) obj).SerializeData(writer));
      LineBlobTerrainResourceGenerator.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((LineBlobTerrainResourceGenerator) obj).DeserializeData(reader));
    }
  }
}
