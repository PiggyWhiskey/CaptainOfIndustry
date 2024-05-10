// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.CustomTerrainPostProcessorV2
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  public abstract class CustomTerrainPostProcessorV2 : ITerrainPostProcessorV2, ITerrainFeatureBase
  {
    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteInt(this.Id);
      writer.WriteBool(this.IsDisabled);
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.Id = reader.ReadInt();
      this.IsDisabled = reader.ReadBool();
    }

    public abstract string Name { get; }

    public int Id { get; set; }

    public bool IsDisabled { get; set; }

    public abstract bool IsUnique { get; }

    public abstract bool IsImportable { get; }

    public TerrainPostProcessorParallelizationStrategy ParallelizationStrategy
    {
      get => TerrainPostProcessorParallelizationStrategy.CustomSchedule;
    }

    public abstract int SortingPriority { get; }

    public int PassCount => 1;

    [DoNotSave(0, null)]
    public TimeSpan LastGenerationTime { get; set; }

    public abstract RectangleTerrainArea2i? GetBoundingBox();

    public abstract bool Initialize(
      Chunk64Area generatedArea,
      RelTile2i terrainSize,
      int initialMapCreationSaveVersion,
      IResolver resolver,
      ITerrainExtraDataRegistrator extraDataReg);

    public abstract void Reset();

    public abstract void ClearCaches();

    public abstract void TranslateBy(RelTile3f delta);

    public abstract void RotateBy(AngleDegrees1f delta);

    public void AnalyzeChunkThreadSafe(
      Chunk2i chunk,
      Tile2i dataOrigin,
      TerrainManager.TerrainData dataReadOnly,
      int pass)
    {
      throw new InvalidOperationException("Custom post-processors should be invoked via `ProcessCustomSchedule`.");
    }

    public void ApplyChunkChanges(
      Chunk2i chunk,
      Tile2i dataOrigin,
      ref TerrainManager.TerrainData dataRef,
      int pass)
    {
      throw new InvalidOperationException("Custom post-processors should be invoked via `ProcessCustomSchedule`.");
    }

    public abstract IEnumerator<Percent> ProcessCustomSchedule(
      Chunk64Area areaChunks,
      TerrainGenerationContext terrainData,
      ParallelOptions parallelOptions,
      int pass,
      int reportProgressFrequencyMs);

    public abstract void EnsureAllBackgroundOperationsAreCancelled();

    protected CustomTerrainPostProcessorV2()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
