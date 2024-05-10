// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.Generators.TreesResourceGenerator
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Map;
using Mafi.Core.Products;
using Mafi.Core.Terrain.Trees;
using Mafi.Core.Utils;
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
  public class TreesResourceGenerator : ITerrainResourceGenerator, ITerrainResource
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private static readonly HeightTilesF MAX_HEIGHT_DELTA;
    public readonly Tile2i From;
    public readonly Tile2i To;
    public readonly int DeltaPriority;
    public readonly ForestProto ForestProto;
    public readonly RelTile1i ResourceRadius;
    public readonly RelTile1i TransitionRadius;
    public readonly Fix32 BaseNoisePeriod;
    public readonly SimplexNoise2dParams SpacingRadiusNoiseParams;
    public readonly Fix32 MinSpacingRadius;
    public readonly Fix32 MaxSpacingRadius;
    public readonly SimplexNoise2dSeed NoiseSeed;
    public readonly bool LimitToParentCellHeight;
    public readonly Option<TerrainMaterialProto> LimitToMaterialProto;
    private readonly XorRsr128PlusGenerator m_random;
    private INoise2D m_baseNoise;
    private INoise2D m_spacingNoise;
    private Line2i m_line;
    private long m_maxTotalRadiusSqr;
    private HeightTilesF? m_parentCellHeight;

    public static void Serialize(TreesResourceGenerator value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TreesResourceGenerator>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TreesResourceGenerator.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Fix32.Serialize(this.BaseNoisePeriod, writer);
      writer.WriteInt(this.DeltaPriority);
      writer.WriteGeneric<ForestProto>(this.ForestProto);
      Tile2i.Serialize(this.From, writer);
      Option<TerrainMaterialProto>.Serialize(this.LimitToMaterialProto, writer);
      writer.WriteBool(this.LimitToParentCellHeight);
      writer.WriteGeneric<INoise2D>(this.m_baseNoise);
      Line2i.Serialize(this.m_line, writer);
      writer.WriteLong(this.m_maxTotalRadiusSqr);
      writer.WriteNullableStruct<HeightTilesF>(this.m_parentCellHeight);
      XorRsr128PlusGenerator.Serialize(this.m_random, writer);
      writer.WriteGeneric<INoise2D>(this.m_spacingNoise);
      RelTile1i.Serialize(this.MaxRadius, writer);
      Fix32.Serialize(this.MaxSpacingRadius, writer);
      Fix32.Serialize(this.MinSpacingRadius, writer);
      SimplexNoise2dSeed.Serialize(this.NoiseSeed, writer);
      Tile3i.Serialize(this.Position, writer);
      RelTile1i.Serialize(this.ResourceRadius, writer);
      SimplexNoise2dParams.Serialize(this.SpacingRadiusNoiseParams, writer);
      Tile2i.Serialize(this.To, writer);
      RelTile1i.Serialize(this.TransitionRadius, writer);
    }

    public static TreesResourceGenerator Deserialize(BlobReader reader)
    {
      TreesResourceGenerator resourceGenerator;
      if (reader.TryStartClassDeserialization<TreesResourceGenerator>(out resourceGenerator))
        reader.EnqueueDataDeserialization((object) resourceGenerator, TreesResourceGenerator.s_deserializeDataDelayedAction);
      return resourceGenerator;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<TreesResourceGenerator>(this, "BaseNoisePeriod", (object) Fix32.Deserialize(reader));
      reader.SetField<TreesResourceGenerator>(this, "DeltaPriority", (object) reader.ReadInt());
      reader.SetField<TreesResourceGenerator>(this, "ForestProto", (object) reader.ReadGenericAs<ForestProto>());
      reader.SetField<TreesResourceGenerator>(this, "From", (object) Tile2i.Deserialize(reader));
      reader.SetField<TreesResourceGenerator>(this, "LimitToMaterialProto", (object) Option<TerrainMaterialProto>.Deserialize(reader));
      reader.SetField<TreesResourceGenerator>(this, "LimitToParentCellHeight", (object) reader.ReadBool());
      this.m_baseNoise = reader.ReadGenericAs<INoise2D>();
      this.m_line = Line2i.Deserialize(reader);
      this.m_maxTotalRadiusSqr = reader.ReadLong();
      this.m_parentCellHeight = reader.ReadNullableStruct<HeightTilesF>();
      reader.SetField<TreesResourceGenerator>(this, "m_random", (object) XorRsr128PlusGenerator.Deserialize(reader));
      this.m_spacingNoise = reader.ReadGenericAs<INoise2D>();
      this.MaxRadius = RelTile1i.Deserialize(reader);
      reader.SetField<TreesResourceGenerator>(this, "MaxSpacingRadius", (object) Fix32.Deserialize(reader));
      reader.SetField<TreesResourceGenerator>(this, "MinSpacingRadius", (object) Fix32.Deserialize(reader));
      reader.SetField<TreesResourceGenerator>(this, "NoiseSeed", (object) SimplexNoise2dSeed.Deserialize(reader));
      this.Position = Tile3i.Deserialize(reader);
      reader.SetField<TreesResourceGenerator>(this, "ResourceRadius", (object) RelTile1i.Deserialize(reader));
      reader.SetField<TreesResourceGenerator>(this, "SpacingRadiusNoiseParams", (object) SimplexNoise2dParams.Deserialize(reader));
      reader.SetField<TreesResourceGenerator>(this, "To", (object) Tile2i.Deserialize(reader));
      reader.SetField<TreesResourceGenerator>(this, "TransitionRadius", (object) RelTile1i.Deserialize(reader));
    }

    public string Name
    {
      get
      {
        return string.Format("Trees resource '{0}' from {1} to {2}", (object) this.ForestProto.Id, (object) this.From, (object) this.To);
      }
    }

    public Tile3i Position { get; private set; }

    public RelTile1i MaxRadius { get; private set; }

    public int Priority => 3000 + this.DeltaPriority;

    public ColorRgba ResourceColor => ColorRgba.Green;

    public TreesResourceGenerator(
      Tile2i from,
      Tile2i to,
      int deltaPriority,
      ForestProto forestProto,
      RelTile1i resourceRadius,
      RelTile1i transitionRadius,
      Fix32 baseNoisePeriod,
      SimplexNoise2dParams spacingRadiusNoiseParams,
      Fix32 minSpacingRadius,
      Fix32 maxSpacingRadius,
      SimplexNoise2dSeed noiseSeed,
      bool limitToParentCellHeight,
      Option<TerrainMaterialProto> limitToMaterialProto)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_random = new XorRsr128PlusGenerator(RandomGeneratorType.Unrestricted, 1UL, 2UL);
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.From = from;
      this.To = to;
      this.DeltaPriority = deltaPriority;
      this.ForestProto = forestProto;
      this.ResourceRadius = resourceRadius.CheckPositive();
      this.TransitionRadius = transitionRadius.CheckPositive();
      this.BaseNoisePeriod = baseNoisePeriod.CheckPositive();
      this.SpacingRadiusNoiseParams = spacingRadiusNoiseParams;
      this.MinSpacingRadius = minSpacingRadius.CheckPositive();
      this.MaxSpacingRadius = maxSpacingRadius.CheckWithinIncl(this.MinSpacingRadius, (Fix32) 8);
      this.NoiseSeed = noiseSeed.CheckNotDefaultStruct<SimplexNoise2dSeed>();
      this.LimitToParentCellHeight = limitToParentCellHeight;
      this.LimitToMaterialProto = limitToMaterialProto;
    }

    public void Initialize(IslandMap map, bool isEditorPreview = false)
    {
      this.m_line = new Line2i(this.From.Vector2i, this.To.Vector2i);
      this.m_baseNoise = (INoise2D) new SimplexNoise2D(this.NoiseSeed, Fix32.Zero, Fix32.One, this.BaseNoisePeriod);
      this.m_spacingNoise = !this.SpacingRadiusNoiseParams.Period.IsPositive ? (INoise2D) new ConstantNoise2D(this.MinSpacingRadius) : (INoise2D) new SimplexNoise2D(this.NoiseSeed, new SimplexNoise2dParams(this.SpacingRadiusNoiseParams.MeanValue, this.SpacingRadiusNoiseParams.Amplitude, this.SpacingRadiusNoiseParams.Period)).Sum((INoise2D) new LineDistanceNoise(this.m_line.Line2f, (Fix32) this.ResourceRadius.Value, (Fix32) this.TransitionRadius.Value, (Fix64) 0L, true, this.MaxSpacingRadius.ToFix64(), true, LineTransitionFn.Linear));
      MapCell closestCell = map.GetClosestCell(new Tile2i(this.m_line.CenterPt));
      this.m_parentCellHeight = this.LimitToParentCellHeight ? new HeightTilesF?(closestCell.GroundHeight.HeightTilesF) : new HeightTilesF?();
      this.Position = new Tile3i(new Tile2i(this.m_line.CenterPt), closestCell.GroundHeight.Value);
      RelTile1i relTile1i = this.ResourceRadius + this.TransitionRadius;
      this.MaxRadius = new RelTile1i((this.m_line.SegmentLength / 2).ToIntCeiled()) + relTile1i;
      this.m_maxTotalRadiusSqr = relTile1i.Squared;
    }

    public ITerrainResourceChunkGenerator CreateChunkGenerator(Chunk2i chunkCoord)
    {
      TreesResourceGenerator.ChunkGenerator chunkGenerator = new TreesResourceGenerator.ChunkGenerator();
      chunkGenerator.Initialize(chunkCoord, this);
      return (ITerrainResourceChunkGenerator) chunkGenerator;
    }

    static TreesResourceGenerator()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TreesResourceGenerator.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((TreesResourceGenerator) obj).SerializeData(writer));
      TreesResourceGenerator.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((TreesResourceGenerator) obj).DeserializeData(reader));
      TreesResourceGenerator.MAX_HEIGHT_DELTA = new HeightTilesF(0.5.ToFix32() * 3);
    }

    private class ChunkGenerator : ITerrainResourceChunkGenerator
    {
      private Chunk2i m_chunkCoord;
      private TreesResourceGenerator m_parentGenerator;
      private IRandom m_random;

      public void Initialize(Chunk2i chunkCoord, TreesResourceGenerator parentGenerator)
      {
        this.m_chunkCoord = chunkCoord;
        this.m_parentGenerator = parentGenerator;
        this.m_random = parentGenerator.m_random.Clone();
      }

      public void GenerateResource(Tile2i coord, TerrainGenerationBuffer resultBuffer)
      {
        if (this.m_parentGenerator.m_parentCellHeight.HasValue && resultBuffer.SurfaceHeight != this.m_parentGenerator.m_parentCellHeight.Value || resultBuffer.TopMaterial.HasValue && (!resultBuffer.TopMaterial.Value.IsFarmable || this.m_parentGenerator.LimitToMaterialProto.HasValue && resultBuffer.TopMaterial != this.m_parentGenerator.LimitToMaterialProto) || this.m_parentGenerator.m_line.DistanceSqrToLineSegmentApprox(coord.Vector2i) > this.m_parentGenerator.m_maxTotalRadiusSqr)
          return;
        Fix32 fix32 = this.m_parentGenerator.m_spacingNoise.GetValue(coord.Vector2f).ToFix32();
        if (fix32 > this.m_parentGenerator.MaxSpacingRadius)
          return;
        this.m_random.SeedFast(coord.X, coord.Y, this.m_parentGenerator.NoiseSeed.SeedX.RawValue, this.m_parentGenerator.NoiseSeed.SeedY.RawValue);
        if (fix32 <= this.m_parentGenerator.MinSpacingRadius)
          fix32 = this.m_parentGenerator.MinSpacingRadius;
        int intFloored = fix32.ToIntFloored();
        if (this.m_random.NextFix32Between01() < fix32.FractionalPart)
          ++intFloored;
        INoise2D baseNoise = this.m_parentGenerator.m_baseNoise;
        Fix64 fix64 = baseNoise.GetValue(coord.Vector2f);
        int num1 = intFloored * intFloored;
        for (int y = -intFloored; y <= intFloored; ++y)
        {
          int num2 = y * y;
          for (int x = -intFloored; x <= intFloored; ++x)
          {
            if (num2 + x * x <= num1 && baseNoise.GetValue((coord + new RelTile2i(x, y)).Vector2f) > fix64)
              return;
          }
        }
        TreeProto treeProto = this.m_parentGenerator.ForestProto.Trees.SampleRandomOrDefault(this.m_random);
        TerrainGenerationBuffer generationBuffer = resultBuffer;
        TreeProto proto = treeProto;
        Tile2f position = coord.CenterTile2f + this.m_random.NextRelTile2f(Fix32.Zero, Fix32.Half);
        AngleSlim angleSlim = this.m_random.NextAngleSlim();
        Percent randomBaseScale = treeProto.GetRandomBaseScale(this.m_random);
        HeightTilesF surfaceHeight = resultBuffer.SurfaceHeight;
        Percent baseScale = randomBaseScale;
        AngleSlim rotation = angleSlim;
        TreeData treeData = new TreeData(proto, position, surfaceHeight, baseScale, -720000, rotation, true);
        generationBuffer.TreeData = treeData;
      }

      public void ChunkGenerationDone(ChunkTerrainData data)
      {
        for (int index1 = 0; index1 < 64; ++index1)
        {
          int num = index1 * 64;
          for (int index2 = 0; index2 < 64; ++index2)
          {
            int index3 = num + index2;
            if (data.Data[index3].TreeData.IsValid)
            {
              HeightTilesF plantedAtHeight = data.Data[index3].TreeData.PlantedAtHeight;
              if (index2 >= 3 && !data.Data[index3 - 3].SurfaceHeight.IsNear(plantedAtHeight, TreesResourceGenerator.MAX_HEIGHT_DELTA))
                data.Data[index3].TreeData = new TreeData();
              else if (index1 >= 3 && !data.Data[index3 - 192].SurfaceHeight.IsNear(plantedAtHeight, TreesResourceGenerator.MAX_HEIGHT_DELTA))
                data.Data[index3].TreeData = new TreeData();
              else if (index2 < 61 && !data.Data[index3 + 3].SurfaceHeight.IsNear(plantedAtHeight, TreesResourceGenerator.MAX_HEIGHT_DELTA))
                data.Data[index3].TreeData = new TreeData();
              else if (index1 < 61 && !data.Data[index3 + 192].SurfaceHeight.IsNear(plantedAtHeight, TreesResourceGenerator.MAX_HEIGHT_DELTA))
                data.Data[index3].TreeData = new TreeData();
            }
          }
        }
        this.m_parentGenerator = (TreesResourceGenerator) null;
        this.m_random = (IRandom) null;
      }

      public ChunkGenerator()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }
  }
}
