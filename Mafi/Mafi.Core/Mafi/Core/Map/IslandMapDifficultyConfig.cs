// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Map.IslandMapDifficultyConfig
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Game;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Map
{
  [GenerateSerializer(false, null, 0)]
  [OnlyForSaveCompatibility(null)]
  public sealed class IslandMapDifficultyConfig : IConfig
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public Percent MineableResourceSizeBonus { get; set; }

    public Percent CellHeightsBias { get; set; }

    public static void Serialize(IslandMapDifficultyConfig value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<IslandMapDifficultyConfig>(value))
        return;
      writer.EnqueueDataSerialization((object) value, IslandMapDifficultyConfig.s_serializeDataDelayedAction);
    }

    private void SerializeData(BlobWriter writer)
    {
      Percent.Serialize(this.CellHeightsBias, writer);
      Percent.Serialize(this.MineableResourceSizeBonus, writer);
    }

    public static IslandMapDifficultyConfig Deserialize(BlobReader reader)
    {
      IslandMapDifficultyConfig difficultyConfig;
      if (reader.TryStartClassDeserialization<IslandMapDifficultyConfig>(out difficultyConfig))
        reader.EnqueueDataDeserialization((object) difficultyConfig, IslandMapDifficultyConfig.s_deserializeDataDelayedAction);
      return difficultyConfig;
    }

    private void DeserializeData(BlobReader reader)
    {
      this.CellHeightsBias = Percent.Deserialize(reader);
      this.MineableResourceSizeBonus = Percent.Deserialize(reader);
    }

    public IslandMapDifficultyConfig()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static IslandMapDifficultyConfig()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      IslandMapDifficultyConfig.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((IslandMapDifficultyConfig) obj).SerializeData(writer));
      IslandMapDifficultyConfig.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((IslandMapDifficultyConfig) obj).DeserializeData(reader));
    }
  }
}
