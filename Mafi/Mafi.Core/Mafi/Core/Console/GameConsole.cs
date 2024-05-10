// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Console.GameConsole
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi.Core.Console
{
  /// <summary>
  /// Game console implementation, invokes <see cref="E:Mafi.Core.Console.GameConsole.OnMessage" /> event for each message.
  /// </summary>
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public class GameConsole : IGameConsole
  {
    public event Action<GameConsole.ConsoleMessage> OnMessage;

    public void WriteLine(string text) => this.WriteLine(text, ColorRgba.White);

    public void WriteLine(string text, ColorRgba color)
    {
      Action<GameConsole.ConsoleMessage> onMessage = this.OnMessage;
      if (onMessage != null)
        onMessage(new GameConsole.ConsoleMessage(text, color));
      Log.Info(text);
    }

    public GameConsole()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public readonly struct ConsoleMessage
    {
      public readonly string Message;
      public readonly ColorRgba Color;

      public ConsoleMessage(string message, ColorRgba color)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Message = message;
        this.Color = color;
      }
    }
  }
}
