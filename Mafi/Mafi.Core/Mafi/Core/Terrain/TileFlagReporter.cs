// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.TileFlagReporter
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Terrain
{
  /// <summary>Class for reporting and testing tile flags.</summary>
  [GenerateSerializer(false, null, 0)]
  public sealed class TileFlagReporter
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly string Name;
    public readonly TerrainManager TerrainManager;
    public readonly uint FlagMask;
    public readonly bool BlocksBuildings;
    public readonly bool BlocksVehicles;
    public readonly bool IsSaved;

    public static void Serialize(TileFlagReporter value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TileFlagReporter>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TileFlagReporter.s_serializeDataDelayedAction);
    }

    private void SerializeData(BlobWriter writer)
    {
      writer.WriteBool(this.BlocksBuildings);
      writer.WriteBool(this.BlocksVehicles);
      writer.WriteUInt(this.FlagMask);
      writer.WriteBool(this.IsSaved);
      writer.WriteString(this.Name);
      TerrainManager.Serialize(this.TerrainManager, writer);
    }

    public static TileFlagReporter Deserialize(BlobReader reader)
    {
      TileFlagReporter tileFlagReporter;
      if (reader.TryStartClassDeserialization<TileFlagReporter>(out tileFlagReporter))
        reader.EnqueueDataDeserialization((object) tileFlagReporter, TileFlagReporter.s_deserializeDataDelayedAction);
      return tileFlagReporter;
    }

    private void DeserializeData(BlobReader reader)
    {
      reader.SetField<TileFlagReporter>(this, "BlocksBuildings", (object) reader.ReadBool());
      reader.SetField<TileFlagReporter>(this, "BlocksVehicles", (object) reader.ReadBool());
      reader.SetField<TileFlagReporter>(this, "FlagMask", (object) reader.ReadUInt());
      reader.SetField<TileFlagReporter>(this, "IsSaved", (object) reader.ReadBool());
      reader.SetField<TileFlagReporter>(this, "Name", (object) reader.ReadString());
      reader.SetField<TileFlagReporter>(this, "TerrainManager", (object) TerrainManager.Deserialize(reader));
    }

    internal TileFlagReporter(
      string name,
      TerrainManager terrainManager,
      uint flagMask,
      bool blocksBuildings,
      bool blocksVehicles,
      bool isSaved)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Name = name.CheckNotNullOrEmpty();
      this.TerrainManager = terrainManager;
      this.FlagMask = flagMask;
      this.BlocksBuildings = blocksBuildings;
      this.BlocksVehicles = blocksVehicles;
      this.IsSaved = isSaved;
    }

    public bool IsSet(Tile2iIndex index)
    {
      return this.TerrainManager.HasAnyTileFlagSet(index, this.FlagMask);
    }

    public bool IsNotSet(Tile2iIndex index)
    {
      return !this.TerrainManager.HasAnyTileFlagSet(index, this.FlagMask);
    }

    public void SetFlag(Tile2iAndIndex tileAndIndex)
    {
      this.TerrainManager.SetTileFlags(tileAndIndex, this.FlagMask);
    }

    public void SetFlagNoEvents(Tile2iIndex index)
    {
      this.TerrainManager.SetTileFlagsNoEvents(index, this.FlagMask);
    }

    public bool SetFlagsNoEventsReportChanged(Tile2iIndex index)
    {
      return this.TerrainManager.SetTileFlagsNoEventsReportChanged(index, this.FlagMask);
    }

    public void ClearFlag(Tile2iAndIndex tileAndIndex)
    {
      this.TerrainManager.ClearTileFlags(tileAndIndex, this.FlagMask);
    }

    public void ClearFlagNoEvents(Tile2iIndex index)
    {
      this.TerrainManager.ClearTileFlagsNoEvents(index, this.FlagMask);
    }

    public bool ClearFlagNoEventsReportChanged(Tile2iIndex index)
    {
      return this.TerrainManager.ClearTileFlagsNoEventsReportChanged(index, this.FlagMask);
    }

    /// <summary>
    /// Prefer using <see cref="M:Mafi.Core.Terrain.TileFlagReporter.SetFlag(Mafi.Tile2iAndIndex)" /> or <see cref="M:Mafi.Core.Terrain.TileFlagReporter.ClearFlag(Mafi.Tile2iAndIndex)" /> if the <paramref name="isSet" /> parameter
    /// is a constant.
    /// </summary>
    public void ReportFlag(Tile2iAndIndex tileAndIndex, bool isSet)
    {
      if (isSet)
        this.TerrainManager.SetTileFlags(tileAndIndex, this.FlagMask);
      else
        this.TerrainManager.ClearTileFlags(tileAndIndex, this.FlagMask);
    }

    static TileFlagReporter()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TileFlagReporter.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((TileFlagReporter) obj).SerializeData(writer));
      TileFlagReporter.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((TileFlagReporter) obj).DeserializeData(reader));
    }
  }
}
