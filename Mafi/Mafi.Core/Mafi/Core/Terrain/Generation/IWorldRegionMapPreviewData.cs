// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.IWorldRegionMapPreviewData
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core.SaveGame;
using Mafi.Logging;
using System;

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  /// <summary>Preview data used for listing the maps.</summary>
  public interface IWorldRegionMapPreviewData : IMapInfoProvider
  {
    Option<string> NameTranslationId { get; set; }

    string Description { get; }

    Option<string> DescriptionTranslationId { get; set; }

    string CreatedInGameVersion { get; }

    string AuthorName { get; }

    bool IsPublished { get; }

    DateTime CreatedDateTimeUtc { get; }

    DateTime LastEditedDateTimeUtc { get; }

    StartingLocationDifficulty Difficulty { get; }

    /// <summary>Total map size, including ocean and exclusion zone.</summary>
    RelTile2i MapSize { get; }

    /// <summary>
    /// Small image used for preview in the list of maps (optional). Encoded as JPG.
    /// </summary>
    EncodedImageAndMatrix ThumbnailImageData { get; }

    /// <summary>Required mods (other than core mods).</summary>
    ImmutableArray<ModInfoRaw> RequiredMods { get; }

    bool IsProtected { get; set; }

    Option<string> FilePath { get; }

    /// <summary>
    /// Called by the map loader when this preview data was created from a map that was loaded from a file.
    /// This path should be returned via <see cref="P:Mafi.Core.Terrain.Generation.IWorldRegionMapPreviewData.FilePath" />.
    /// </summary>
    void SetMapFilePath(string path);
  }
}
