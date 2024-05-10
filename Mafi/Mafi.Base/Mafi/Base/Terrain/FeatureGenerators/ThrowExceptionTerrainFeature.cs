// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Terrain.FeatureGenerators.ThrowExceptionTerrainFeature
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Generation;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Base.Terrain.FeatureGenerators
{
  [GenerateSerializer(false, null, 0)]
  public class ThrowExceptionTerrainFeature : ITerrainFeatureGenerator, ITerrainFeatureBase
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly ThrowExceptionTerrainFeature.Configuration ConfigMutable;
    private bool m_isParallelProcessing;

    public static void Serialize(ThrowExceptionTerrainFeature value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ThrowExceptionTerrainFeature>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ThrowExceptionTerrainFeature.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      ThrowExceptionTerrainFeature.Configuration.Serialize(this.ConfigMutable, writer);
      writer.WriteInt(this.Id);
      writer.WriteBool(this.IsDisabled);
      writer.WriteBool(this.m_isParallelProcessing);
    }

    public static ThrowExceptionTerrainFeature Deserialize(BlobReader reader)
    {
      ThrowExceptionTerrainFeature exceptionTerrainFeature;
      if (reader.TryStartClassDeserialization<ThrowExceptionTerrainFeature>(out exceptionTerrainFeature))
        reader.EnqueueDataDeserialization((object) exceptionTerrainFeature, ThrowExceptionTerrainFeature.s_deserializeDataDelayedAction);
      return exceptionTerrainFeature;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<ThrowExceptionTerrainFeature>(this, "ConfigMutable", (object) ThrowExceptionTerrainFeature.Configuration.Deserialize(reader));
      this.Id = reader.ReadInt();
      this.IsDisabled = reader.ReadBool();
      this.m_isParallelProcessing = reader.ReadBool();
    }

    public string Name => "Throw exception";

    public int Id { get; set; }

    public bool IsDisabled { get; set; }

    public bool IsUnique => false;

    public bool IsImportable => false;

    public int SortingPriority => this.ConfigMutable.SortingPriorityAdjustment;

    [DoNotSave(0, null)]
    public TimeSpan LastGenerationTime { get; set; }

    public ITerrainFeatureConfig Config => (ITerrainFeatureConfig) this.ConfigMutable;

    public ThrowExceptionTerrainFeature(ThrowExceptionTerrainFeature.Configuration config)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ConfigMutable = config;
    }

    public RectangleTerrainArea2i? GetBoundingBox() => new RectangleTerrainArea2i?();

    public TerrainFeatureResourceInfo? GetResourceInfo() => new TerrainFeatureResourceInfo?();

    public void TranslateBy(RelTile3f delta)
    {
    }

    public void RotateBy(AngleDegrees1f rotation)
    {
    }

    public void Reset()
    {
    }

    public void ClearCaches()
    {
    }

    public bool Initialize(
      Chunk64Area generatedArea,
      RelTile2i terrainSize,
      int initialMapCreationSaveVersion,
      IResolver resolver,
      ITerrainExtraDataRegistrator extraDataReg)
    {
      this.m_isParallelProcessing = !resolver.Resolve<ITerrainGeneratorV2>().IsSerial();
      if (this.ConfigMutable.ThrowOnInit && (!this.ConfigMutable.ThrowOnlyDuringParallelProcessing || this.m_isParallelProcessing))
      {
        Log.Warning("Throwing due to ThrowOnInit");
        throw new Exception("ThrowOnInit");
      }
      return true;
    }

    public void GenerateChunkThreadSafe(TerrainGeneratorChunkData data)
    {
      if (this.ConfigMutable.ThrowOnGenerateChunk && (!this.ConfigMutable.ThrowOnlyDuringParallelProcessing || this.m_isParallelProcessing))
      {
        Log.Warning("Throwing due to ThrowOnGenerateChunk");
        throw new Exception("ThrowOnGenerateChunk");
      }
    }

    static ThrowExceptionTerrainFeature()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      ThrowExceptionTerrainFeature.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ThrowExceptionTerrainFeature) obj).SerializeData(writer));
      ThrowExceptionTerrainFeature.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ThrowExceptionTerrainFeature) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    public class Configuration : ITerrainFeatureConfig
    {
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public static void Serialize(
        ThrowExceptionTerrainFeature.Configuration value,
        BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<ThrowExceptionTerrainFeature.Configuration>(value))
          return;
        writer.EnqueueDataSerialization((object) value, ThrowExceptionTerrainFeature.Configuration.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        writer.WriteInt(this.SortingPriorityAdjustment);
        writer.WriteBool(this.ThrowOnGenerateChunk);
        writer.WriteBool(this.ThrowOnInit);
        writer.WriteBool(this.ThrowOnlyDuringParallelProcessing);
      }

      public static ThrowExceptionTerrainFeature.Configuration Deserialize(BlobReader reader)
      {
        ThrowExceptionTerrainFeature.Configuration configuration;
        if (reader.TryStartClassDeserialization<ThrowExceptionTerrainFeature.Configuration>(out configuration))
          reader.EnqueueDataDeserialization((object) configuration, ThrowExceptionTerrainFeature.Configuration.s_deserializeDataDelayedAction);
        return configuration;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        this.SortingPriorityAdjustment = reader.ReadInt();
        this.ThrowOnGenerateChunk = reader.ReadBool();
        this.ThrowOnInit = reader.ReadBool();
        this.ThrowOnlyDuringParallelProcessing = reader.ReadBool();
      }

      public bool ThrowOnlyDuringParallelProcessing { get; set; }

      public bool ThrowOnInit { get; set; }

      public bool ThrowOnGenerateChunk { get; set; }

      public int SortingPriorityAdjustment { get; set; }

      public Configuration()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }

      static Configuration()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        ThrowExceptionTerrainFeature.Configuration.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ThrowExceptionTerrainFeature.Configuration) obj).SerializeData(writer));
        ThrowExceptionTerrainFeature.Configuration.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ThrowExceptionTerrainFeature.Configuration) obj).DeserializeData(reader));
      }
    }
  }
}
