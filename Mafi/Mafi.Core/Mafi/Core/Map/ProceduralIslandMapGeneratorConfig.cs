// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Map.ProceduralIslandMapGeneratorConfig
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Game;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Map
{
  [GenerateSerializer(false, null, 0)]
  public class ProceduralIslandMapGeneratorConfig : IConfig
  {
    /// <summary>
    /// Maximum number of cells generated. This is more of a safety guard to not generate way too many cells if some
    /// other parameters are ill-defined.
    /// </summary>
    public const int MAX_CELLS = 1000;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public string MapRandomSeed { get; set; }

    public Proto.ID BedrockMaterialId { get; set; }

    public Fix32 AreaChunksSquaredApprox { get; set; }

    /// <summary>Approximate minimal cell diameter.</summary>
    public RelTile1i MinCellDiameter { get; set; }

    public RelTile1i OceanCellDiameter { get; set; }

    public int CellMinMaxDiameterMult { get; set; }

    public int CellExpansionTrials { get; set; }

    public Fix32 CellSizeGrowthFromStartExp { get; set; }

    public float CellSizeGrowthFromCoastExp { get; set; }

    public HeightTilesI MinCellHeight { get; set; }

    public HeightTilesI CellHeightDiffMean { get; set; }

    public HeightTilesI CellHeightDiffStdDev { get; set; }

    public Fix32 CellHeightMeanDistanceToStartMult { get; set; }

    public Percent CellHeightDiffMaxStdDev { get; set; }

    public OceanShape OceanShape { get; set; }

    /// <summary>
    /// Amount multiplier for starting resources. If not positive, no starting resources will be generated.
    /// </summary>
    public Fix32 StartingTerrainResourcesAmountMult { get; set; }

    /// <summary>
    /// At what distance form starting cell is resource richness multiplied by <see cref="P:Mafi.Core.Map.ProceduralIslandMapGeneratorConfig.ResourcesRichnessMultExpBase" />. Then it continues exponentially, at n-times this distance resources
    /// multiplier will be <c>base^n</c>. Minimum value is 100.
    /// </summary>
    public RelTile1i ResourcesRichnessMultDistance { get; set; }

    /// <summary>
    /// Base of exponential resource growth. Value 2.0 means that each <see cref="P:Mafi.Core.Map.ProceduralIslandMapGeneratorConfig.ResourcesRichnessMultDistance" />
    /// will resources multiplier be doubled.
    /// </summary>
    public Fix32 ResourcesRichnessMultExpBase { get; set; }

    public MapCellSurfaceGeneratorProto.ID DefaultIslandCellSurfaceId { get; set; }

    public MapCellSurfaceGeneratorProto.ID DefaultOceanCellSurfaceId { get; set; }

    public Proto.ID DefaultCliffMaterialId { get; set; }

    public static void Serialize(ProceduralIslandMapGeneratorConfig value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ProceduralIslandMapGeneratorConfig>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ProceduralIslandMapGeneratorConfig.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Fix32.Serialize(this.AreaChunksSquaredApprox, writer);
      Proto.ID.Serialize(this.BedrockMaterialId, writer);
      writer.WriteInt(this.CellExpansionTrials);
      Percent.Serialize(this.CellHeightDiffMaxStdDev, writer);
      HeightTilesI.Serialize(this.CellHeightDiffMean, writer);
      HeightTilesI.Serialize(this.CellHeightDiffStdDev, writer);
      Fix32.Serialize(this.CellHeightMeanDistanceToStartMult, writer);
      writer.WriteInt(this.CellMinMaxDiameterMult);
      writer.WriteFloat(this.CellSizeGrowthFromCoastExp);
      Fix32.Serialize(this.CellSizeGrowthFromStartExp, writer);
      Proto.ID.Serialize(this.DefaultCliffMaterialId, writer);
      MapCellSurfaceGeneratorProto.ID.Serialize(this.DefaultIslandCellSurfaceId, writer);
      MapCellSurfaceGeneratorProto.ID.Serialize(this.DefaultOceanCellSurfaceId, writer);
      writer.WriteString(this.MapRandomSeed);
      RelTile1i.Serialize(this.MinCellDiameter, writer);
      HeightTilesI.Serialize(this.MinCellHeight, writer);
      RelTile1i.Serialize(this.OceanCellDiameter, writer);
      writer.WriteInt((int) this.OceanShape);
      RelTile1i.Serialize(this.ResourcesRichnessMultDistance, writer);
      Fix32.Serialize(this.ResourcesRichnessMultExpBase, writer);
      Fix32.Serialize(this.StartingTerrainResourcesAmountMult, writer);
    }

    public static ProceduralIslandMapGeneratorConfig Deserialize(BlobReader reader)
    {
      ProceduralIslandMapGeneratorConfig mapGeneratorConfig;
      if (reader.TryStartClassDeserialization<ProceduralIslandMapGeneratorConfig>(out mapGeneratorConfig))
        reader.EnqueueDataDeserialization((object) mapGeneratorConfig, ProceduralIslandMapGeneratorConfig.s_deserializeDataDelayedAction);
      return mapGeneratorConfig;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.AreaChunksSquaredApprox = Fix32.Deserialize(reader);
      this.BedrockMaterialId = Proto.ID.Deserialize(reader);
      this.CellExpansionTrials = reader.ReadInt();
      this.CellHeightDiffMaxStdDev = Percent.Deserialize(reader);
      this.CellHeightDiffMean = HeightTilesI.Deserialize(reader);
      this.CellHeightDiffStdDev = HeightTilesI.Deserialize(reader);
      this.CellHeightMeanDistanceToStartMult = Fix32.Deserialize(reader);
      this.CellMinMaxDiameterMult = reader.ReadInt();
      this.CellSizeGrowthFromCoastExp = reader.ReadFloat();
      this.CellSizeGrowthFromStartExp = Fix32.Deserialize(reader);
      this.DefaultCliffMaterialId = Proto.ID.Deserialize(reader);
      this.DefaultIslandCellSurfaceId = MapCellSurfaceGeneratorProto.ID.Deserialize(reader);
      this.DefaultOceanCellSurfaceId = MapCellSurfaceGeneratorProto.ID.Deserialize(reader);
      this.MapRandomSeed = reader.ReadString();
      this.MinCellDiameter = RelTile1i.Deserialize(reader);
      this.MinCellHeight = HeightTilesI.Deserialize(reader);
      this.OceanCellDiameter = RelTile1i.Deserialize(reader);
      this.OceanShape = (OceanShape) reader.ReadInt();
      this.ResourcesRichnessMultDistance = RelTile1i.Deserialize(reader);
      this.ResourcesRichnessMultExpBase = Fix32.Deserialize(reader);
      this.StartingTerrainResourcesAmountMult = Fix32.Deserialize(reader);
    }

    public ProceduralIslandMapGeneratorConfig()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: reference to a compiler-generated field
      this.\u003CMapRandomSeed\u003Ek__BackingField = "MaFi";
      // ISSUE: reference to a compiler-generated field
      this.\u003CBedrockMaterialId\u003Ek__BackingField = IdsCore.TerrainMaterials.Bedrock;
      // ISSUE: reference to a compiler-generated field
      this.\u003CAreaChunksSquaredApprox\u003Ek__BackingField = (Fix32) 50;
      // ISSUE: reference to a compiler-generated field
      this.\u003CMinCellDiameter\u003Ek__BackingField = new RelTile1i(64);
      // ISSUE: reference to a compiler-generated field
      this.\u003COceanCellDiameter\u003Ek__BackingField = new RelTile1i(256);
      // ISSUE: reference to a compiler-generated field
      this.\u003CCellMinMaxDiameterMult\u003Ek__BackingField = 2;
      // ISSUE: reference to a compiler-generated field
      this.\u003CCellExpansionTrials\u003Ek__BackingField = 16;
      // ISSUE: reference to a compiler-generated field
      this.\u003CCellSizeGrowthFromStartExp\u003Ek__BackingField = 0.6.ToFix32();
      // ISSUE: reference to a compiler-generated field
      this.\u003CCellSizeGrowthFromCoastExp\u003Ek__BackingField = 0.7f;
      // ISSUE: reference to a compiler-generated field
      this.\u003CMinCellHeight\u003Ek__BackingField = new HeightTilesI(2);
      // ISSUE: reference to a compiler-generated field
      this.\u003CCellHeightDiffMean\u003Ek__BackingField = new HeightTilesI(4);
      // ISSUE: reference to a compiler-generated field
      this.\u003CCellHeightDiffStdDev\u003Ek__BackingField = new HeightTilesI(3);
      // ISSUE: reference to a compiler-generated field
      this.\u003CCellHeightMeanDistanceToStartMult\u003Ek__BackingField = 8.Over(1000);
      // ISSUE: reference to a compiler-generated field
      this.\u003CCellHeightDiffMaxStdDev\u003Ek__BackingField = 200.Percent();
      // ISSUE: reference to a compiler-generated field
      this.\u003CStartingTerrainResourcesAmountMult\u003Ek__BackingField = (Fix32) 2;
      // ISSUE: reference to a compiler-generated field
      this.\u003CResourcesRichnessMultDistance\u003Ek__BackingField = new RelTile1i(200);
      // ISSUE: reference to a compiler-generated field
      this.\u003CResourcesRichnessMultExpBase\u003Ek__BackingField = 1.8.ToFix32();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static ProceduralIslandMapGeneratorConfig()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ProceduralIslandMapGeneratorConfig.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ProceduralIslandMapGeneratorConfig) obj).SerializeData(writer));
      ProceduralIslandMapGeneratorConfig.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ProceduralIslandMapGeneratorConfig) obj).DeserializeData(reader));
    }
  }
}
