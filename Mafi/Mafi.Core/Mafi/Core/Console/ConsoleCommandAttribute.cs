// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Console.ConsoleCommandAttribute
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
  /// Annotate any method on global dependency to use is as a console command. This method may be void,
  /// return <see cref="T:System.String" />, or return <see cref="T:Mafi.Core.Console.GameCommandResult" />.
  /// </summary>
  [AttributeUsage(AttributeTargets.Method)]
  public sealed class ConsoleCommandAttribute : Attribute
  {
    public readonly bool InvokeOnMainThread;
    public readonly bool InvokeDuringSync;
    public readonly string Documentation;
    public readonly Option<string> CustomCommandName;

    public ConsoleCommandAttribute(
      bool invokeOnMainThread = false,
      bool invokeDuringSync = false,
      string documentation = null,
      string customCommandName = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.InvokeOnMainThread = invokeOnMainThread;
      this.InvokeDuringSync = invokeDuringSync;
      this.Documentation = documentation ?? "";
      this.CustomCommandName = (Option<string>) customCommandName;
    }
  }
}
