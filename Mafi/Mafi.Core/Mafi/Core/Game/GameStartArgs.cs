// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Game.GameStartArgs
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.Game
{
  public readonly struct GameStartArgs
  {
    public static GameStartArgs Empty;
    public readonly Option<string> SaveName;

    public GameStartArgs(string saveName)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.SaveName = (Option<string>) saveName;
    }

    static GameStartArgs() => MBiHIp97M4MqqbtZOh.rMWAw2OR8();
  }
}
