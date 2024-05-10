// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Map.StaticIslandMapPreviewData
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Products;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Map
{
  [GenerateSerializer(false, null, 0)]
  public sealed class StaticIslandMapPreviewData
  {
    private static readonly LocStr IslandMapDifficulty__Easy;
    private static readonly LocStr IslandMapDifficulty__Medium;
    private static readonly LocStr IslandMapDifficulty__Hard;
    private static readonly LocStr IslandMapDifficulty__Insane;
    private static readonly LocStr IslandMapDifficulty__EasyTooltip;
    private static readonly LocStr IslandMapDifficulty__MediumTooltip;
    private static readonly LocStr IslandMapDifficulty__HardTooltip;
    private static readonly LocStr IslandMapDifficulty__InsaneTooltip;
    public readonly LocStr Name;
    public readonly LocStr Description;
    public readonly IslandMapDifficulty Difficulty;
    public readonly Type IslandMapDataType;
    public readonly ImmutableArray<StartingLocation> StartingLocations;
    public readonly string PreviewPrefabPath;
    /// <summary>Statistics of available resources on the map.</summary>
    public readonly ImmutableArray<KeyValuePair<ProductProto.ID, QuantityLarge>> ResourcesStats;
    /// <summary>
    /// Each entry contains number of tiles at or above certain elevation. Note that some heights might be missing
    /// if there are no tiles at that particular elevation.
    /// </summary>
    public readonly ImmutableArray<KeyValuePair<HeightTilesI, int>> TilesAtOrAboveElevationDataSorted;
    /// <summary>Total map size, including ocean.</summary>
    public readonly RelTile2i MapSize;
    /// <summary>Number of tiles that are not ocean.</summary>
    public readonly int NonOceanTilesCount;
    /// <summary>Number of tiles that are flat.</summary>
    public readonly int FlatNonOceanTilesCount;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public static LocStr GetDifficultyLabel(IslandMapDifficulty difficulty)
    {
      switch (difficulty)
      {
        case IslandMapDifficulty.Medium:
          return StaticIslandMapPreviewData.IslandMapDifficulty__Medium;
        case IslandMapDifficulty.Hard:
          return StaticIslandMapPreviewData.IslandMapDifficulty__Hard;
        case IslandMapDifficulty.Insane:
          return StaticIslandMapPreviewData.IslandMapDifficulty__Insane;
        default:
          return StaticIslandMapPreviewData.IslandMapDifficulty__Easy;
      }
    }

    public static LocStr GetDifficultyDescription(IslandMapDifficulty difficulty)
    {
      switch (difficulty)
      {
        case IslandMapDifficulty.Medium:
          return StaticIslandMapPreviewData.IslandMapDifficulty__MediumTooltip;
        case IslandMapDifficulty.Hard:
          return StaticIslandMapPreviewData.IslandMapDifficulty__HardTooltip;
        case IslandMapDifficulty.Insane:
          return StaticIslandMapPreviewData.IslandMapDifficulty__InsaneTooltip;
        default:
          return StaticIslandMapPreviewData.IslandMapDifficulty__EasyTooltip;
      }
    }

    public StaticIslandMapPreviewData(
      LocStr name,
      LocStr description,
      IslandMapDifficulty difficulty,
      Type islandMapDataType,
      ImmutableArray<StartingLocation> startingLocations,
      string previewPrefabPath,
      ImmutableArray<KeyValuePair<ProductProto.ID, QuantityLarge>> resourcesStats,
      ImmutableArray<KeyValuePair<HeightTilesI, int>> tilesAtOrAboveElevationDataSorted,
      RelTile2i mapSize,
      int nonOceanTilesCount,
      int flatNonOceanTilesCount)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Name = name;
      this.Description = description;
      this.Difficulty = difficulty;
      this.IslandMapDataType = islandMapDataType;
      this.StartingLocations = startingLocations;
      this.PreviewPrefabPath = previewPrefabPath;
      this.ResourcesStats = resourcesStats;
      this.TilesAtOrAboveElevationDataSorted = tilesAtOrAboveElevationDataSorted;
      this.MapSize = mapSize;
      this.NonOceanTilesCount = nonOceanTilesCount;
      this.FlatNonOceanTilesCount = flatNonOceanTilesCount;
    }

    public StaticIslandMapProviderConfig GetConfig(int startingLocationIndex)
    {
      return new StaticIslandMapProviderConfig(this.StartingLocations[startingLocationIndex], this);
    }

    public int GetTilesCountAtOrAboveElevation(HeightTilesI height)
    {
      if (this.TilesAtOrAboveElevationDataSorted.IsEmpty)
      {
        Log.Warning("No tile height data available.");
        return 0;
      }
      HeightTilesI heightTilesI = height;
      KeyValuePair<HeightTilesI, int> keyValuePair = this.TilesAtOrAboveElevationDataSorted.Last;
      HeightTilesI key = keyValuePair.Key;
      if (heightTilesI > key)
        return 0;
      for (int index = this.TilesAtOrAboveElevationDataSorted.Length - 1; index >= 0; --index)
      {
        keyValuePair = this.TilesAtOrAboveElevationDataSorted[index];
        if (keyValuePair.Key <= height)
        {
          keyValuePair = this.TilesAtOrAboveElevationDataSorted[index];
          return keyValuePair.Value;
        }
      }
      keyValuePair = this.TilesAtOrAboveElevationDataSorted.First;
      return keyValuePair.Value;
    }

    public override string ToString()
    {
      return this.Name.TranslatedString + " (" + this.IslandMapDataType.Name + ")";
    }

    public static void Serialize(StaticIslandMapPreviewData value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<StaticIslandMapPreviewData>(value))
        return;
      writer.EnqueueDataSerialization((object) value, StaticIslandMapPreviewData.s_serializeDataDelayedAction);
    }

    private void SerializeData(BlobWriter writer)
    {
      LocStr.Serialize(this.Description, writer);
      writer.WriteInt((int) this.Difficulty);
      writer.WriteInt(this.FlatNonOceanTilesCount);
      writer.WriteGeneric<Type>(this.IslandMapDataType);
      RelTile2i.Serialize(this.MapSize, writer);
      LocStr.Serialize(this.Name, writer);
      writer.WriteInt(this.NonOceanTilesCount);
      writer.WriteString(this.PreviewPrefabPath);
      ImmutableArray<KeyValuePair<ProductProto.ID, QuantityLarge>>.Serialize(this.ResourcesStats, writer);
      ImmutableArray<StartingLocation>.Serialize(this.StartingLocations, writer);
      ImmutableArray<KeyValuePair<HeightTilesI, int>>.Serialize(this.TilesAtOrAboveElevationDataSorted, writer);
    }

    public static StaticIslandMapPreviewData Deserialize(BlobReader reader)
    {
      StaticIslandMapPreviewData islandMapPreviewData;
      if (reader.TryStartClassDeserialization<StaticIslandMapPreviewData>(out islandMapPreviewData))
        reader.EnqueueDataDeserialization((object) islandMapPreviewData, StaticIslandMapPreviewData.s_deserializeDataDelayedAction);
      return islandMapPreviewData;
    }

    private void DeserializeData(BlobReader reader)
    {
      reader.SetField<StaticIslandMapPreviewData>(this, "Description", (object) LocStr.Deserialize(reader));
      reader.SetField<StaticIslandMapPreviewData>(this, "Difficulty", (object) (IslandMapDifficulty) reader.ReadInt());
      reader.SetField<StaticIslandMapPreviewData>(this, "FlatNonOceanTilesCount", (object) reader.ReadInt());
      reader.SetField<StaticIslandMapPreviewData>(this, "IslandMapDataType", (object) reader.ReadGenericAs<Type>());
      reader.SetField<StaticIslandMapPreviewData>(this, "MapSize", (object) RelTile2i.Deserialize(reader));
      reader.SetField<StaticIslandMapPreviewData>(this, "Name", (object) LocStr.Deserialize(reader));
      reader.SetField<StaticIslandMapPreviewData>(this, "NonOceanTilesCount", (object) reader.ReadInt());
      reader.SetField<StaticIslandMapPreviewData>(this, "PreviewPrefabPath", (object) reader.ReadString());
      reader.SetField<StaticIslandMapPreviewData>(this, "ResourcesStats", (object) ImmutableArray<KeyValuePair<ProductProto.ID, QuantityLarge>>.Deserialize(reader));
      reader.SetField<StaticIslandMapPreviewData>(this, "StartingLocations", (object) ImmutableArray<StartingLocation>.Deserialize(reader));
      reader.SetField<StaticIslandMapPreviewData>(this, "TilesAtOrAboveElevationDataSorted", (object) ImmutableArray<KeyValuePair<HeightTilesI, int>>.Deserialize(reader));
    }

    static StaticIslandMapPreviewData()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      StaticIslandMapPreviewData.IslandMapDifficulty__Easy = Loc.Str(nameof (IslandMapDifficulty__Easy), "Standard", "label explaining how difficult a selected island map is (this is the easiest one)");
      StaticIslandMapPreviewData.IslandMapDifficulty__Medium = Loc.Str(nameof (IslandMapDifficulty__Medium), "Advanced", "label explaining how difficult a selected island map is");
      StaticIslandMapPreviewData.IslandMapDifficulty__Hard = Loc.Str(nameof (IslandMapDifficulty__Hard), "Difficult", "label explaining how difficult a selected island map is");
      StaticIslandMapPreviewData.IslandMapDifficulty__Insane = Loc.Str(nameof (IslandMapDifficulty__Insane), "Challenging", "label explaining how difficult a selected island map is (this is the hardest one)");
      StaticIslandMapPreviewData.IslandMapDifficulty__EasyTooltip = Loc.Str(nameof (IslandMapDifficulty__EasyTooltip), "A balanced map. Great for beginners or casual players.", "tooltip explaining how difficult a selected island map is");
      StaticIslandMapPreviewData.IslandMapDifficulty__MediumTooltip = Loc.Str(nameof (IslandMapDifficulty__MediumTooltip), "A slightly more advanced map that may require some extra planning.", "tooltip explaining how difficult a selected island map is");
      StaticIslandMapPreviewData.IslandMapDifficulty__HardTooltip = Loc.Str(nameof (IslandMapDifficulty__HardTooltip), "For skilled players who have some experience with the game already. Requires planning and trade-offs.", "tooltip explaining how difficult a selected island map is");
      StaticIslandMapPreviewData.IslandMapDifficulty__InsaneTooltip = Loc.Str(nameof (IslandMapDifficulty__InsaneTooltip), "For players who are ready to fail and like to push their limits. Do not take success for granted.", "tooltip explaining how difficult a selected island map is");
      StaticIslandMapPreviewData.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((StaticIslandMapPreviewData) obj).SerializeData(writer));
      StaticIslandMapPreviewData.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((StaticIslandMapPreviewData) obj).DeserializeData(reader));
    }
  }
}
