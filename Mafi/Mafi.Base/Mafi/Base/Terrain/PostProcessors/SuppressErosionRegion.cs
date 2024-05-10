// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Terrain.PostProcessors.SuppressErosionRegion
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Numerics;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Base.Terrain.PostProcessors
{
  [GenerateSerializer(false, null, 0)]
  public class SuppressErosionRegion
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public static void Serialize(SuppressErosionRegion value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SuppressErosionRegion>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SuppressErosionRegion.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Polygon2fMutable.Serialize(this.Polygon, writer);
    }

    public static SuppressErosionRegion Deserialize(BlobReader reader)
    {
      SuppressErosionRegion suppressErosionRegion;
      if (reader.TryStartClassDeserialization<SuppressErosionRegion>(out suppressErosionRegion))
        reader.EnqueueDataDeserialization((object) suppressErosionRegion, SuppressErosionRegion.s_deserializeDataDelayedAction);
      return suppressErosionRegion;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.Polygon = Polygon2fMutable.Deserialize(reader);
    }

    public Polygon2fMutable Polygon { get; set; }

    public SuppressErosionRegion()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static SuppressErosionRegion()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      SuppressErosionRegion.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((SuppressErosionRegion) obj).SerializeData(writer));
      SuppressErosionRegion.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((SuppressErosionRegion) obj).DeserializeData(reader));
    }
  }
}
