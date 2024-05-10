// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Game.LoadGameArgsFromFile
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Mods;

#nullable disable
namespace Mafi.Core.Game
{
  public class LoadGameArgsFromFile
  {
    public readonly SaveFileInfo Save;
    public readonly IFileSystemHelper FsHelper;
    /// <summary>
    /// Third party mods are mods that are not in Core or DLC mods. Core and DLC mods are always loaded.
    /// When this is not null only the given Extra mods will be used. If this is null, mods will
    /// be used as defined in the save file.
    /// </summary>
    public readonly ImmutableArray<ModData> ThirdPartyModsToAdd;

    public LoadGameArgsFromFile(
      SaveFileInfo save,
      IFileSystemHelper fsHelper,
      ImmutableArray<ModData> thirdPartyModsToAdd)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Save = save;
      this.FsHelper = fsHelper;
      this.ThirdPartyModsToAdd = thirdPartyModsToAdd;
    }
  }
}
