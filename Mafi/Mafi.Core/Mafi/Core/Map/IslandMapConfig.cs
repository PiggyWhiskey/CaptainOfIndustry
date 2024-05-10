// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Map.IslandMapConfig
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Static;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Map
{
  [GenerateSerializer(false, null, 0)]
  public sealed class IslandMapConfig
  {
    public static readonly IslandMapConfig Default;
    internal static readonly IslandMapConfig DefaultNoOcean;
    /// <summary>
    /// Distance from coast (cell edges that are between ocean and land) on which ocean floor is flat.
    /// After this distance, a slope down of <see cref="F:Mafi.Core.Map.IslandMapConfig.OceanFloorHeightPerDistanceFromCoast" /> is applied.
    /// </summary>
    public readonly RelTile1i OceanFloorFlatDistance;
    /// <summary>
    /// Base ocean floor height before it starts sloping down. This number should be negative and less or
    /// equal to <see cref="F:Mafi.Core.Entities.Static.StaticEntityOceanReservationManager.MAX_OCEAN_FLOOR_HEIGHT" />.
    /// </summary>
    public readonly HeightTilesI OceanFloorBaseHeight;
    /// <summary>
    /// Ocean floor height per distance from coast. This should be negative number so that oceans are deeper
    /// the further away from coast they are.
    /// </summary>
    public readonly ThicknessTilesF OceanFloorHeightPerDistanceFromCoast;
    public readonly bool DoNotCreateOcean;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public IslandMapConfig(
      RelTile1i oceanFloorFlatDistance,
      HeightTilesI oceanFloorBaseHeight,
      ThicknessTilesF oceanFloorHeightPerDistanceFromCoast,
      bool doNotCreateOcean = false)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.OceanFloorFlatDistance = oceanFloorFlatDistance;
      this.OceanFloorBaseHeight = oceanFloorBaseHeight.Min(StaticEntityOceanReservationManager.MAX_OCEAN_FLOOR_HEIGHT.TilesHeightFloored);
      this.OceanFloorHeightPerDistanceFromCoast = oceanFloorHeightPerDistanceFromCoast;
      this.DoNotCreateOcean = doNotCreateOcean;
    }

    public static void Serialize(IslandMapConfig value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<IslandMapConfig>(value))
        return;
      writer.EnqueueDataSerialization((object) value, IslandMapConfig.s_serializeDataDelayedAction);
    }

    private void SerializeData(BlobWriter writer)
    {
      writer.WriteBool(this.DoNotCreateOcean);
      HeightTilesI.Serialize(this.OceanFloorBaseHeight, writer);
      RelTile1i.Serialize(this.OceanFloorFlatDistance, writer);
      ThicknessTilesF.Serialize(this.OceanFloorHeightPerDistanceFromCoast, writer);
    }

    public static IslandMapConfig Deserialize(BlobReader reader)
    {
      IslandMapConfig islandMapConfig;
      if (reader.TryStartClassDeserialization<IslandMapConfig>(out islandMapConfig))
        reader.EnqueueDataDeserialization((object) islandMapConfig, IslandMapConfig.s_deserializeDataDelayedAction);
      return islandMapConfig;
    }

    private void DeserializeData(BlobReader reader)
    {
      reader.SetField<IslandMapConfig>(this, "DoNotCreateOcean", (object) reader.ReadBool());
      reader.SetField<IslandMapConfig>(this, "OceanFloorBaseHeight", (object) HeightTilesI.Deserialize(reader));
      reader.SetField<IslandMapConfig>(this, "OceanFloorFlatDistance", (object) RelTile1i.Deserialize(reader));
      reader.SetField<IslandMapConfig>(this, "OceanFloorHeightPerDistanceFromCoast", (object) ThicknessTilesF.Deserialize(reader));
    }

    static IslandMapConfig()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      IslandMapConfig.Default = new IslandMapConfig(new RelTile1i(10), MapCell.DEFAULT_OCEAN_FLOOR_HEIGHT, -0.1.TilesThick());
      IslandMapConfig.DefaultNoOcean = new IslandMapConfig(new RelTile1i(10), MapCell.DEFAULT_OCEAN_FLOOR_HEIGHT, -0.1.TilesThick(), true);
      IslandMapConfig.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((IslandMapConfig) obj).SerializeData(writer));
      IslandMapConfig.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((IslandMapConfig) obj).DeserializeData(reader));
    }
  }
}
