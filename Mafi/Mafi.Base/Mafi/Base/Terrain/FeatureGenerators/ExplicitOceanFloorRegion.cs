// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Terrain.FeatureGenerators.ExplicitOceanFloorRegion
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Numerics;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Base.Terrain.FeatureGenerators
{
  [GenerateSerializer(false, null, 0)]
  public class ExplicitOceanFloorRegion
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public static void Serialize(ExplicitOceanFloorRegion value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ExplicitOceanFloorRegion>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ExplicitOceanFloorRegion.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      ThicknessTilesF.Serialize(this.OceanDepth, writer);
      Polygon2fMutable.Serialize(this.Polygon, writer);
      RelTile1f.Serialize(this.TransitionSize, writer);
    }

    public static ExplicitOceanFloorRegion Deserialize(BlobReader reader)
    {
      ExplicitOceanFloorRegion oceanFloorRegion;
      if (reader.TryStartClassDeserialization<ExplicitOceanFloorRegion>(out oceanFloorRegion))
        reader.EnqueueDataDeserialization((object) oceanFloorRegion, ExplicitOceanFloorRegion.s_deserializeDataDelayedAction);
      return oceanFloorRegion;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.OceanDepth = ThicknessTilesF.Deserialize(reader);
      this.Polygon = Polygon2fMutable.Deserialize(reader);
      this.TransitionSize = RelTile1f.Deserialize(reader);
    }

    public Polygon2fMutable Polygon { get; set; }

    [EditorRange(2.0, 1000.0)]
    public ThicknessTilesF OceanDepth { get; set; }

    [EditorRange(0.0, 256.0)]
    public RelTile1f TransitionSize { get; set; }

    public ExplicitOceanFloorRegion()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: reference to a compiler-generated field
      this.\u003COceanDepth\u003Ek__BackingField = 50.0.TilesThick();
      // ISSUE: reference to a compiler-generated field
      this.\u003CTransitionSize\u003Ek__BackingField = 64.0.Tiles();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static ExplicitOceanFloorRegion()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      ExplicitOceanFloorRegion.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ExplicitOceanFloorRegion) obj).SerializeData(writer));
      ExplicitOceanFloorRegion.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ExplicitOceanFloorRegion) obj).DeserializeData(reader));
    }
  }
}
