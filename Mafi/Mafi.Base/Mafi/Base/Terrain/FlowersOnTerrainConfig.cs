// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Terrain.FlowersOnTerrainConfig
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Game;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Base.Terrain
{
  [GenerateSerializer(false, null, 0)]
  public class FlowersOnTerrainConfig : IConfig
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public static void Serialize(FlowersOnTerrainConfig value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<FlowersOnTerrainConfig>(value))
        return;
      writer.EnqueueDataSerialization((object) value, FlowersOnTerrainConfig.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      ImmutableArray<FlowersOnTerrainConfig.FlowersConfig>.Serialize(this.FlowersConfigs, writer);
    }

    public static FlowersOnTerrainConfig Deserialize(BlobReader reader)
    {
      FlowersOnTerrainConfig flowersOnTerrainConfig;
      if (reader.TryStartClassDeserialization<FlowersOnTerrainConfig>(out flowersOnTerrainConfig))
        reader.EnqueueDataDeserialization((object) flowersOnTerrainConfig, FlowersOnTerrainConfig.s_deserializeDataDelayedAction);
      return flowersOnTerrainConfig;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.FlowersConfigs = ImmutableArray<FlowersOnTerrainConfig.FlowersConfig>.Deserialize(reader);
    }

    public ImmutableArray<FlowersOnTerrainConfig.FlowersConfig> FlowersConfigs { get; set; }

    public FlowersOnTerrainConfig()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static FlowersOnTerrainConfig()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      FlowersOnTerrainConfig.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((FlowersOnTerrainConfig) obj).SerializeData(writer));
      FlowersOnTerrainConfig.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((FlowersOnTerrainConfig) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    public class FlowersConfig
    {
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
      public Proto.ID FlowerMaterialId;
      public Proto.ID SpawnMaterialId;
      public Percent SpawnProbabilityBase;
      public ThicknessTilesF SpawnMaterialMinThickness;
      public RelTile1f MinDistanceFromOthers;
      public Percent KeepExpandingFromLatestProbab;
      public int CoreSizeMean;
      public int CoreSizeStdDev;
      public int AuxiliarySizeMean;
      public int AuxiliarySizeStdDev;

      public static void Serialize(FlowersOnTerrainConfig.FlowersConfig value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<FlowersOnTerrainConfig.FlowersConfig>(value))
          return;
        writer.EnqueueDataSerialization((object) value, FlowersOnTerrainConfig.FlowersConfig.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        writer.WriteInt(this.AuxiliarySizeMean);
        writer.WriteInt(this.AuxiliarySizeStdDev);
        writer.WriteInt(this.CoreSizeMean);
        writer.WriteInt(this.CoreSizeStdDev);
        Proto.ID.Serialize(this.FlowerMaterialId, writer);
        Percent.Serialize(this.KeepExpandingFromLatestProbab, writer);
        RelTile1f.Serialize(this.MinDistanceFromOthers, writer);
        Proto.ID.Serialize(this.SpawnMaterialId, writer);
        ThicknessTilesF.Serialize(this.SpawnMaterialMinThickness, writer);
        Percent.Serialize(this.SpawnProbabilityBase, writer);
      }

      public static FlowersOnTerrainConfig.FlowersConfig Deserialize(BlobReader reader)
      {
        FlowersOnTerrainConfig.FlowersConfig flowersConfig;
        if (reader.TryStartClassDeserialization<FlowersOnTerrainConfig.FlowersConfig>(out flowersConfig))
          reader.EnqueueDataDeserialization((object) flowersConfig, FlowersOnTerrainConfig.FlowersConfig.s_deserializeDataDelayedAction);
        return flowersConfig;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        this.AuxiliarySizeMean = reader.ReadInt();
        this.AuxiliarySizeStdDev = reader.ReadInt();
        this.CoreSizeMean = reader.ReadInt();
        this.CoreSizeStdDev = reader.ReadInt();
        this.FlowerMaterialId = Proto.ID.Deserialize(reader);
        this.KeepExpandingFromLatestProbab = Percent.Deserialize(reader);
        this.MinDistanceFromOthers = RelTile1f.Deserialize(reader);
        this.SpawnMaterialId = Proto.ID.Deserialize(reader);
        this.SpawnMaterialMinThickness = ThicknessTilesF.Deserialize(reader);
        this.SpawnProbabilityBase = Percent.Deserialize(reader);
      }

      public FlowersConfig()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }

      static FlowersConfig()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        FlowersOnTerrainConfig.FlowersConfig.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((FlowersOnTerrainConfig.FlowersConfig) obj).SerializeData(writer));
        FlowersOnTerrainConfig.FlowersConfig.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((FlowersOnTerrainConfig.FlowersConfig) obj).DeserializeData(reader));
      }
    }
  }
}
