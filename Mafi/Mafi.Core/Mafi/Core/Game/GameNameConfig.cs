// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Game.GameNameConfig
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.Game
{
  /// <summary>
  /// This is used to pass game name to SaveManager.
  /// This is intentionally not serialized as it is expected to be explicitly passed on load.
  /// </summary>
  public class GameNameConfig : IConfig
  {
    /// <summary>
    /// File used to instance the current game. If null, this is a new game.
    /// </summary>
    public SaveFileInfo? LoadedFile { get; }

    public string GameName { get; }

    public GameNameConfig(SaveFileInfo loadedFile)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.LoadedFile = new SaveFileInfo?(loadedFile);
      this.GameName = this.LoadedFile.Value.GameName;
    }

    public GameNameConfig(string gameName)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.LoadedFile = new SaveFileInfo?();
      this.GameName = gameName;
    }
  }
}
