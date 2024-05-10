// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Physics.NoTerrainPhysicsSimulator
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Terrain.Physics
{
  /// <summary>Does no terrain physics simulation.</summary>
  [GenerateSerializer(false, null, 0)]
  public sealed class NoTerrainPhysicsSimulator : ITerrainPhysicsSimulator
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public static void Serialize(NoTerrainPhysicsSimulator value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<NoTerrainPhysicsSimulator>(value))
        return;
      writer.EnqueueDataSerialization((object) value, NoTerrainPhysicsSimulator.s_serializeDataDelayedAction);
    }

    private void SerializeData(BlobWriter writer)
    {
    }

    public static NoTerrainPhysicsSimulator Deserialize(BlobReader reader)
    {
      NoTerrainPhysicsSimulator physicsSimulator;
      if (reader.TryStartClassDeserialization<NoTerrainPhysicsSimulator>(out physicsSimulator))
        reader.EnqueueDataDeserialization((object) physicsSimulator, NoTerrainPhysicsSimulator.s_deserializeDataDelayedAction);
      return physicsSimulator;
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

    public void StartPhysicsSimulationAt(Tile2iAndIndex tileAndIndex)
    {
    }

    public void StopPhysicsSimulationAt(Tile2iAndIndex tileAndIndex)
    {
    }

    public int GetQueueSize() => 0;

    public void Update()
    {
    }

    public void Clear()
    {
    }

    public NoTerrainPhysicsSimulator()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static NoTerrainPhysicsSimulator()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      NoTerrainPhysicsSimulator.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((NoTerrainPhysicsSimulator) obj).SerializeData(writer));
      NoTerrainPhysicsSimulator.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((NoTerrainPhysicsSimulator) obj).DeserializeData(reader));
    }
  }
}
