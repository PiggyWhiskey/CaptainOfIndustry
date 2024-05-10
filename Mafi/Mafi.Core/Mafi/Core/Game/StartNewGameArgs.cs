// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Game.StartNewGameArgs
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
  public class StartNewGameArgs
  {
    public readonly ImmutableArray<ModData> Mods;
    public readonly ImmutableArray<IConfig> Configs;
    public readonly IFileSystemHelper FsHelper;

    public StartNewGameArgs(
      ImmutableArray<ModData> mods,
      ImmutableArray<IConfig> configs,
      IFileSystemHelper fsHelper)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Mods = mods;
      this.Configs = configs;
      this.FsHelper = fsHelper;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
