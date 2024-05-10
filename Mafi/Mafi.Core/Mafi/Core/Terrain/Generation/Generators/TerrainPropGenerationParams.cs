// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.Generators.TerrainPropGenerationParams
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Map;
using Mafi.Random.Noise;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Terrain.Generation.Generators
{
  /// <summary>Parameters used for placing props.</summary>
  [GenerateSerializer(false, null, 0)]
  public class TerrainPropGenerationParams
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly Fix32 BaseNoisePeriod;
    public readonly SimplexNoise2dParams SpacingRadiusNoiseParams;
    public readonly int MinSpacingRadius;
    public readonly int MaxSpacingRadius;
    public readonly SimplexNoise2dSeed NoiseSeed;
    private INoise2D m_baseNoise;
    private INoise2D m_spacingNoise;

    public static void Serialize(TerrainPropGenerationParams value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TerrainPropGenerationParams>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TerrainPropGenerationParams.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Fix32.Serialize(this.BaseNoisePeriod, writer);
      writer.WriteGeneric<INoise2D>(this.m_baseNoise);
      writer.WriteGeneric<INoise2D>(this.m_spacingNoise);
      writer.WriteInt(this.MaxSpacingRadius);
      writer.WriteInt(this.MinSpacingRadius);
      SimplexNoise2dSeed.Serialize(this.NoiseSeed, writer);
      SimplexNoise2dParams.Serialize(this.SpacingRadiusNoiseParams, writer);
    }

    public static TerrainPropGenerationParams Deserialize(BlobReader reader)
    {
      TerrainPropGenerationParams generationParams;
      if (reader.TryStartClassDeserialization<TerrainPropGenerationParams>(out generationParams))
        reader.EnqueueDataDeserialization((object) generationParams, TerrainPropGenerationParams.s_deserializeDataDelayedAction);
      return generationParams;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<TerrainPropGenerationParams>(this, "BaseNoisePeriod", (object) Fix32.Deserialize(reader));
      this.m_baseNoise = reader.ReadGenericAs<INoise2D>();
      this.m_spacingNoise = reader.ReadGenericAs<INoise2D>();
      reader.SetField<TerrainPropGenerationParams>(this, "MaxSpacingRadius", (object) reader.ReadInt());
      reader.SetField<TerrainPropGenerationParams>(this, "MinSpacingRadius", (object) reader.ReadInt());
      reader.SetField<TerrainPropGenerationParams>(this, "NoiseSeed", (object) SimplexNoise2dSeed.Deserialize(reader));
      reader.SetField<TerrainPropGenerationParams>(this, "SpacingRadiusNoiseParams", (object) SimplexNoise2dParams.Deserialize(reader));
    }

    public INoise2D BaseNoise => this.m_baseNoise;

    public INoise2D SpacingNoise => this.m_spacingNoise;

    public TerrainPropGenerationParams()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.SpacingRadiusNoiseParams = new SimplexNoise2dParams(20.ToFix32(), 4.ToFix32(), Fix32.FromRaw(153600));
      this.NoiseSeed = new SimplexNoise2dSeed(Fix32.FromRaw(474), Fix32.FromRaw(375));
      this.BaseNoisePeriod = 15.ToFix32();
      this.MinSpacingRadius = 16;
      this.MaxSpacingRadius = 24.CheckWithinIncl(this.MinSpacingRadius, 24);
      this.m_baseNoise = (INoise2D) new SimplexNoise2D(this.NoiseSeed, Fix32.Zero, Fix32.One, this.BaseNoisePeriod);
      if (this.SpacingRadiusNoiseParams.Period.IsPositive)
        this.m_spacingNoise = (INoise2D) new SimplexNoise2D(this.NoiseSeed, new SimplexNoise2dParams(this.SpacingRadiusNoiseParams.MeanValue, this.SpacingRadiusNoiseParams.Amplitude, this.SpacingRadiusNoiseParams.Period));
      else
        this.m_spacingNoise = (INoise2D) new ConstantNoise2D((Fix32) this.MinSpacingRadius);
    }

    public void Initialize(IslandMap map)
    {
      this.m_baseNoise = (INoise2D) new SimplexNoise2D(this.NoiseSeed, Fix32.Zero, Fix32.One, this.BaseNoisePeriod);
      if (this.SpacingRadiusNoiseParams.Period.IsPositive)
        this.m_spacingNoise = (INoise2D) new SimplexNoise2D(this.NoiseSeed, new SimplexNoise2dParams(this.SpacingRadiusNoiseParams.MeanValue, this.SpacingRadiusNoiseParams.Amplitude, this.SpacingRadiusNoiseParams.Period));
      else
        this.m_spacingNoise = (INoise2D) new ConstantNoise2D((Fix32) this.MinSpacingRadius);
    }

    static TerrainPropGenerationParams()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TerrainPropGenerationParams.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((TerrainPropGenerationParams) obj).SerializeData(writer));
      TerrainPropGenerationParams.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((TerrainPropGenerationParams) obj).DeserializeData(reader));
    }
  }
}
