// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Map.MapCellSurfaceGenerator
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Products;
using Mafi.Core.Terrain.Generation;
using Mafi.Random.Noise;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Map
{
  [GenerateSerializer(false, null, 0)]
  public class MapCellSurfaceGenerator : ICellSurfaceGenerator
  {
    public Option<INoise2D> AltSurfaceNoise;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public MapCellSurfaceGeneratorProto Proto { get; private set; }

    public MapCellSurfaceGenerator(MapCellSurfaceGeneratorProto proto)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Proto = proto;
      if (!proto.AltSurfaceNoiseParams.HasValue)
        return;
      SimplexNoise2dSeed seed = new SimplexNoise2dSeed((proto.Id.Value.GetHashCode() & (int) ushort.MaxValue).Over((int) ushort.MaxValue), (proto.AltSurfaceNoiseParams.GetHashCode() & (int) ushort.MaxValue).Over((int) ushort.MaxValue));
      INoise2D baseNoise = (INoise2D) new SimplexNoise2D(seed, proto.AltSurfaceNoiseParams.Value);
      if (proto.AltSurfNoiseTurbulenceParams.HasValue)
        baseNoise = baseNoise.Turbulence(seed, proto.AltSurfNoiseTurbulenceParams.Value);
      this.AltSurfaceNoise = baseNoise.SomeOption<INoise2D>();
    }

    public void GenerateSurfaceAt(
      MapCell cell,
      Tile2i coord,
      TerrainGenerationBuffer resultBuffer,
      bool replaceOtherProducts)
    {
      HeightTilesF topHeight = resultBuffer.IsEmpty ? resultBuffer.BaseSurfaceHeight : cell.GroundHeight.HeightTilesF;
      ImmutableArray<KeyValuePair<TerrainMaterialProto, ThicknessTilesF>> immutableArray;
      if (this.AltSurfaceNoise.HasValue)
      {
        Fix64 fix64 = this.AltSurfaceNoise.Value.GetValue(coord.Vector2f);
        if (fix64 <= this.Proto.AltNoiseStartTransition)
          immutableArray = this.Proto.SurfaceMaterialsTopToBottom;
        else if (fix64 >= this.Proto.AltNoiseEndTransition)
        {
          immutableArray = this.Proto.SurfaceMaterialsTopToBottomAlt;
        }
        else
        {
          Percent percent = Percent.FromRatio(fix64 - this.Proto.AltNoiseStartTransition, this.Proto.AltNoiseEndTransition - this.Proto.AltNoiseStartTransition);
          Assert.That<Percent>(percent).IsWithin0To100PercIncl();
          KeyValuePair<TerrainMaterialProto, ThicknessTilesF> first1 = this.Proto.SurfaceMaterialsTopToBottom.First;
          KeyValuePair<TerrainMaterialProto, ThicknessTilesF> first2 = this.Proto.SurfaceMaterialsTopToBottomAlt.First;
          if (percent < Percent.Fifty)
          {
            ThicknessTilesF thicknessTilesF = percent.ToFix32().TilesThick();
            resultBuffer.SetProductInRange(first1.Key, topHeight - first1.Value, topHeight - thicknessTilesF, replaceOtherProducts);
            resultBuffer.SetProductInRange(first2.Key, topHeight - thicknessTilesF, topHeight, replaceOtherProducts);
            return;
          }
          ThicknessTilesF thicknessTilesF1 = ThicknessTilesF.One - percent.ToFix32().TilesThick();
          resultBuffer.SetProductInRange(first2.Key, topHeight - first2.Value, topHeight - thicknessTilesF1, replaceOtherProducts);
          resultBuffer.SetProductInRange(first1.Key, topHeight - thicknessTilesF1, topHeight, replaceOtherProducts);
          return;
        }
      }
      else
        immutableArray = this.Proto.SurfaceMaterialsTopToBottom;
      foreach (KeyValuePair<TerrainMaterialProto, ThicknessTilesF> keyValuePair in immutableArray)
      {
        HeightTilesF bottomHeight = topHeight - keyValuePair.Value;
        resultBuffer.SetProductInRange(keyValuePair.Key, bottomHeight, topHeight, replaceOtherProducts);
        topHeight = bottomHeight;
      }
    }

    public static void Serialize(MapCellSurfaceGenerator value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<MapCellSurfaceGenerator>(value))
        return;
      writer.EnqueueDataSerialization((object) value, MapCellSurfaceGenerator.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Option<INoise2D>.Serialize(this.AltSurfaceNoise, writer);
      writer.WriteGeneric<MapCellSurfaceGeneratorProto>(this.Proto);
    }

    public static MapCellSurfaceGenerator Deserialize(BlobReader reader)
    {
      MapCellSurfaceGenerator surfaceGenerator;
      if (reader.TryStartClassDeserialization<MapCellSurfaceGenerator>(out surfaceGenerator))
        reader.EnqueueDataDeserialization((object) surfaceGenerator, MapCellSurfaceGenerator.s_deserializeDataDelayedAction);
      return surfaceGenerator;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.AltSurfaceNoise = Option<INoise2D>.Deserialize(reader);
      this.Proto = reader.ReadGenericAs<MapCellSurfaceGeneratorProto>();
    }

    static MapCellSurfaceGenerator()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      MapCellSurfaceGenerator.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((MapCellSurfaceGenerator) obj).SerializeData(writer));
      MapCellSurfaceGenerator.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((MapCellSurfaceGenerator) obj).DeserializeData(reader));
    }
  }
}
