// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Physics.NoTerrainDisruptionSimulator
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Products;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Terrain.Physics
{
  /// <summary>Does no terrain surface simulation.</summary>
  [GenerateSerializer(false, null, 0)]
  public sealed class NoTerrainDisruptionSimulator : ITerrainDisruptionSimulator
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    [DoNotSave(0, null)]
    private Event<MaterialConversionResult> m_dummy;

    public static void Serialize(NoTerrainDisruptionSimulator value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<NoTerrainDisruptionSimulator>(value))
        return;
      writer.EnqueueDataSerialization((object) value, NoTerrainDisruptionSimulator.s_serializeDataDelayedAction);
    }

    private void SerializeData(BlobWriter writer)
    {
    }

    public static NoTerrainDisruptionSimulator Deserialize(BlobReader reader)
    {
      NoTerrainDisruptionSimulator disruptionSimulator;
      if (reader.TryStartClassDeserialization<NoTerrainDisruptionSimulator>(out disruptionSimulator))
        reader.EnqueueDataDeserialization((object) disruptionSimulator, NoTerrainDisruptionSimulator.s_deserializeDataDelayedAction);
      return disruptionSimulator;
    }

    private void DeserializeData(BlobReader reader)
    {
    }

    public bool IsDisabled => false;

    public bool IsProcessingTiles => false;

    public void Initialize(TerrainManager terrainManager)
    {
    }

    public void SetDisabled(bool isDisabled)
    {
    }

    public void Update()
    {
    }

    public void RecoverTile(Tile2iAndIndex tileAndIndex)
    {
    }

    public Event<MaterialConversionResult> GetMaterialRecoveryEvent(TerrainMaterialProto material)
    {
      if (this.m_dummy == null)
        this.m_dummy = new Event<MaterialConversionResult>();
      return this.m_dummy;
    }

    public int GetQueueSize() => 0;

    public void Clear()
    {
    }

    public NoTerrainDisruptionSimulator()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static NoTerrainDisruptionSimulator()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      NoTerrainDisruptionSimulator.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((NoTerrainDisruptionSimulator) obj).SerializeData(writer));
      NoTerrainDisruptionSimulator.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((NoTerrainDisruptionSimulator) obj).DeserializeData(reader));
    }
  }
}
