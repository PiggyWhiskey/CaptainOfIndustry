// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.WorldRegionMapPreviewData
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.SaveGame;
using Mafi.Logging;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  [GenerateSerializer(false, null, 0)]
  public class WorldRegionMapPreviewData : IWorldRegionMapPreviewData, IMapInfoProvider
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public const int MAX_NAME_LENGTH = 32;
    public const int MAX_DESCRIPTION_LENGTH = 1000;
    public const int MAX_AUTHOR_LENGTH = 32;

    public static void Serialize(WorldRegionMapPreviewData value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<WorldRegionMapPreviewData>(value))
        return;
      writer.EnqueueDataSerialization((object) value, WorldRegionMapPreviewData.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteString(this.AuthorName);
      writer.WriteDateTime(this.CreatedDateTimeUtc);
      writer.WriteString(this.CreatedInGameVersion);
      writer.WriteInt(this.CreatedInSaveVersion);
      writer.WriteString(this.Description);
      Option<string>.Serialize(this.DescriptionTranslationId, writer);
      writer.WriteInt((int) this.Difficulty);
      writer.WriteBool(this.IsProtected);
      writer.WriteBool(this.IsPublished);
      writer.WriteDateTime(this.LastEditedDateTimeUtc);
      RelTile2i.Serialize(this.MapSize, writer);
      writer.WriteInt(this.MapVersion);
      writer.WriteString(this.Name);
      Option<string>.Serialize(this.NameTranslationId, writer);
      ImmutableArray<ModInfoRaw>.Serialize(this.RequiredMods, writer);
      EncodedImageAndMatrix.Serialize(this.ThumbnailImageData, writer);
    }

    public static WorldRegionMapPreviewData Deserialize(BlobReader reader)
    {
      WorldRegionMapPreviewData regionMapPreviewData;
      if (reader.TryStartClassDeserialization<WorldRegionMapPreviewData>(out regionMapPreviewData))
        reader.EnqueueDataDeserialization((object) regionMapPreviewData, WorldRegionMapPreviewData.s_deserializeDataDelayedAction);
      return regionMapPreviewData;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.AuthorName = reader.ReadString();
      this.CreatedDateTimeUtc = reader.ReadDateTime();
      this.CreatedInGameVersion = reader.ReadString();
      this.CreatedInSaveVersion = reader.LoadedSaveVersion >= 157 ? reader.ReadInt() : 0;
      this.Description = reader.ReadString();
      this.DescriptionTranslationId = reader.LoadedSaveVersion >= 144 ? Option<string>.Deserialize(reader) : new Option<string>();
      this.Difficulty = (StartingLocationDifficulty) reader.ReadInt();
      this.IsProtected = reader.LoadedSaveVersion >= 150 && reader.ReadBool();
      this.IsPublished = reader.ReadBool();
      this.LastEditedDateTimeUtc = reader.ReadDateTime();
      this.MapSize = RelTile2i.Deserialize(reader);
      this.MapVersion = reader.ReadInt();
      this.Name = reader.ReadString();
      this.NameTranslationId = reader.LoadedSaveVersion >= 144 ? Option<string>.Deserialize(reader) : new Option<string>();
      this.RequiredMods = ImmutableArray<ModInfoRaw>.Deserialize(reader);
      this.ThumbnailImageData = EncodedImageAndMatrix.Deserialize(reader);
    }

    public string Name { get; set; }

    [NewInSaveVersion(144, null, null, null, null)]
    public Option<string> NameTranslationId { get; set; }

    public string Description { get; set; }

    [NewInSaveVersion(144, null, null, null, null)]
    public Option<string> DescriptionTranslationId { get; set; }

    public int MapVersion { get; set; }

    [NewInSaveVersion(157, null, null, null, null)]
    public int CreatedInSaveVersion { get; set; }

    public string CreatedInGameVersion { get; set; }

    public string AuthorName { get; set; }

    public bool IsPublished { get; set; }

    public DateTime CreatedDateTimeUtc { get; set; }

    public DateTime LastEditedDateTimeUtc { get; set; }

    public StartingLocationDifficulty Difficulty { get; set; }

    public RelTile2i MapSize { get; set; }

    public EncodedImageAndMatrix ThumbnailImageData { get; set; }

    public ImmutableArray<ModInfoRaw> RequiredMods { get; set; }

    [NewInSaveVersion(150, null, null, null, null)]
    public bool IsProtected { get; set; }

    [DoNotSave(0, null)]
    public Option<string> FilePath { get; private set; }

    public void SetMapFilePath(string path)
    {
      this.FilePath = string.IsNullOrEmpty(path) ? Option<string>.None : (Option<string>) path;
    }

    public WorldRegionMapPreviewData()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: reference to a compiler-generated field
      this.\u003CCreatedInSaveVersion\u003Ek__BackingField = 168;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static WorldRegionMapPreviewData()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      WorldRegionMapPreviewData.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((WorldRegionMapPreviewData) obj).SerializeData(writer));
      WorldRegionMapPreviewData.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((WorldRegionMapPreviewData) obj).DeserializeData(reader));
    }
  }
}
