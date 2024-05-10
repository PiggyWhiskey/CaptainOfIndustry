// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.Generators.CellEdgeTerrainGeneratorFactory
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Map;
using Mafi.Core.Products;
using Mafi.Random.Noise;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Terrain.Generation.Generators
{
  [GenerateSerializer(false, null, 0)]
  public class CellEdgeTerrainGeneratorFactory : IMapCellEdgeTerrainFactory
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly int DeltaPriority;
    public readonly TerrainMaterialProto CliffResourceProto;
    public readonly RelTile1i TransitionRadius;
    public readonly RelTile1i ExtraTransitionRadiusPerHeightDiff;
    public readonly Percent BoundaryComplexity;
    public readonly RelTile1i TopRadius;
    public readonly ThicknessTilesI ExtraThickness;
    public readonly SimplexNoise2dSeed RandomSeed;
    public readonly NoiseTurbulenceParams TurbulenceParams;
    public readonly bool ClampAboveTopCell;

    public static void Serialize(CellEdgeTerrainGeneratorFactory value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<CellEdgeTerrainGeneratorFactory>(value))
        return;
      writer.EnqueueDataSerialization((object) value, CellEdgeTerrainGeneratorFactory.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Percent.Serialize(this.BoundaryComplexity, writer);
      writer.WriteBool(this.ClampAboveTopCell);
      writer.WriteGeneric<TerrainMaterialProto>(this.CliffResourceProto);
      writer.WriteInt(this.DeltaPriority);
      ThicknessTilesI.Serialize(this.ExtraThickness, writer);
      RelTile1i.Serialize(this.ExtraTransitionRadiusPerHeightDiff, writer);
      SimplexNoise2dSeed.Serialize(this.RandomSeed, writer);
      RelTile1i.Serialize(this.TopRadius, writer);
      RelTile1i.Serialize(this.TransitionRadius, writer);
      NoiseTurbulenceParams.Serialize(this.TurbulenceParams, writer);
    }

    public static CellEdgeTerrainGeneratorFactory Deserialize(BlobReader reader)
    {
      CellEdgeTerrainGeneratorFactory generatorFactory;
      if (reader.TryStartClassDeserialization<CellEdgeTerrainGeneratorFactory>(out generatorFactory))
        reader.EnqueueDataDeserialization((object) generatorFactory, CellEdgeTerrainGeneratorFactory.s_deserializeDataDelayedAction);
      return generatorFactory;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<CellEdgeTerrainGeneratorFactory>(this, "BoundaryComplexity", (object) Percent.Deserialize(reader));
      reader.SetField<CellEdgeTerrainGeneratorFactory>(this, "ClampAboveTopCell", (object) reader.ReadBool());
      reader.SetField<CellEdgeTerrainGeneratorFactory>(this, "CliffResourceProto", (object) reader.ReadGenericAs<TerrainMaterialProto>());
      reader.SetField<CellEdgeTerrainGeneratorFactory>(this, "DeltaPriority", (object) reader.ReadInt());
      reader.SetField<CellEdgeTerrainGeneratorFactory>(this, "ExtraThickness", (object) ThicknessTilesI.Deserialize(reader));
      reader.SetField<CellEdgeTerrainGeneratorFactory>(this, "ExtraTransitionRadiusPerHeightDiff", (object) RelTile1i.Deserialize(reader));
      reader.SetField<CellEdgeTerrainGeneratorFactory>(this, "RandomSeed", (object) SimplexNoise2dSeed.Deserialize(reader));
      reader.SetField<CellEdgeTerrainGeneratorFactory>(this, "TopRadius", (object) RelTile1i.Deserialize(reader));
      reader.SetField<CellEdgeTerrainGeneratorFactory>(this, "TransitionRadius", (object) RelTile1i.Deserialize(reader));
      reader.SetField<CellEdgeTerrainGeneratorFactory>(this, "TurbulenceParams", (object) NoiseTurbulenceParams.Deserialize(reader));
    }

    public CellEdgeTerrainGeneratorFactory(
      int deltaPriority,
      TerrainMaterialProto cliffResourceProto,
      RelTile1i transitionRadius,
      RelTile1i extraTransitionRadiusPerHeightDiff,
      Percent boundaryComplexity,
      RelTile1i topRadius,
      ThicknessTilesI extraThickness,
      SimplexNoise2dSeed randomSeed,
      NoiseTurbulenceParams turbulenceParams,
      bool clampAboveTopCell)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.DeltaPriority = deltaPriority;
      this.CliffResourceProto = cliffResourceProto;
      this.TransitionRadius = transitionRadius;
      this.ExtraTransitionRadiusPerHeightDiff = extraTransitionRadiusPerHeightDiff;
      this.BoundaryComplexity = boundaryComplexity;
      this.TopRadius = topRadius;
      this.ExtraThickness = extraThickness;
      this.RandomSeed = randomSeed;
      this.TurbulenceParams = turbulenceParams;
      this.ClampAboveTopCell = clampAboveTopCell;
    }

    public void GenerateEdgeTerrainGeneratorsFor(
      MapCell mapCell,
      Lyst<ITerrainResourceGenerator> outGenerators)
    {
      foreach (MapCell validNeighbor in mapCell.ValidNeighbors)
      {
        if (!(validNeighbor.GroundHeight >= mapCell.GroundHeight))
          outGenerators.Add((ITerrainResourceGenerator) new CellEdgeTerrainGenerator(mapCell.Id, validNeighbor.Id, this.DeltaPriority, this.CliffResourceProto, this.TransitionRadius, this.ExtraTransitionRadiusPerHeightDiff, this.BoundaryComplexity, this.TopRadius, this.ExtraThickness, this.RandomSeed, this.TurbulenceParams, this.ClampAboveTopCell));
      }
    }

    static CellEdgeTerrainGeneratorFactory()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      CellEdgeTerrainGeneratorFactory.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((CellEdgeTerrainGeneratorFactory) obj).SerializeData(writer));
      CellEdgeTerrainGeneratorFactory.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((CellEdgeTerrainGeneratorFactory) obj).DeserializeData(reader));
    }
  }
}
