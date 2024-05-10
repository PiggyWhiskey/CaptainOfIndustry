// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Map.StaticIslandMapProviderConfig
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
  public sealed class StaticIslandMapProviderConfig : IConfig
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public Type IslandMapType => this.PreviewData.IslandMapDataType;

    public StartingLocation StartingLocation { get; private set; }

    public StaticIslandMapPreviewData PreviewData { get; private set; }

    public StaticIslandMapProviderConfig(
      StartingLocation startingLocation,
      StaticIslandMapPreviewData previewData)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.StartingLocation = startingLocation;
      this.PreviewData = previewData;
    }

    public static void Serialize(StaticIslandMapProviderConfig value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<StaticIslandMapProviderConfig>(value))
        return;
      writer.EnqueueDataSerialization((object) value, StaticIslandMapProviderConfig.s_serializeDataDelayedAction);
    }

    private void SerializeData(BlobWriter writer)
    {
      StaticIslandMapPreviewData.Serialize(this.PreviewData, writer);
      StartingLocation.Serialize(this.StartingLocation, writer);
    }

    public static StaticIslandMapProviderConfig Deserialize(BlobReader reader)
    {
      StaticIslandMapProviderConfig mapProviderConfig;
      if (reader.TryStartClassDeserialization<StaticIslandMapProviderConfig>(out mapProviderConfig))
        reader.EnqueueDataDeserialization((object) mapProviderConfig, StaticIslandMapProviderConfig.s_deserializeDataDelayedAction);
      return mapProviderConfig;
    }

    private void DeserializeData(BlobReader reader)
    {
      this.PreviewData = StaticIslandMapPreviewData.Deserialize(reader);
      this.StartingLocation = StartingLocation.Deserialize(reader);
    }

    static StaticIslandMapProviderConfig()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      StaticIslandMapProviderConfig.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((StaticIslandMapProviderConfig) obj).SerializeData(writer));
      StaticIslandMapProviderConfig.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((StaticIslandMapProviderConfig) obj).DeserializeData(reader));
    }
  }
}
