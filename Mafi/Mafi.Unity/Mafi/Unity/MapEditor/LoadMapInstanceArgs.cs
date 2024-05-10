// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MapEditor.LoadMapInstanceArgs
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Mods;
using Mafi.Core.Terrain.Generation;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MapEditor
{
  public class LoadMapInstanceArgs
  {
    public readonly MapEditorConfig Config;
    public readonly IWorldRegionMap Map;
    public readonly IWorldRegionMapPreviewData PreviewData;
    public readonly IWorldRegionMapAdditionalData AdditionalData;
    public readonly IFileSystemHelper FsHelper;
    /// <summary>
    /// Extra mods are mods that are not in Core or DLC mods. Core and DLC mods are always loaded.
    /// When this is not null only the given Extra mods will be used. If this is null, mods will
    /// be used as defined in the save file.
    /// </summary>
    public readonly ImmutableArray<ModData> ExtraModsToAdd;

    public LoadMapInstanceArgs(
      MapEditorConfig config,
      IWorldRegionMap map,
      IWorldRegionMapPreviewData previewData,
      IWorldRegionMapAdditionalData additionalData,
      IFileSystemHelper fsHelper,
      ImmutableArray<ModData> extraModsToAdd)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.Config = config;
      this.Map = map;
      this.PreviewData = previewData;
      this.AdditionalData = additionalData;
      this.FsHelper = fsHelper;
      this.ExtraModsToAdd = extraModsToAdd;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
