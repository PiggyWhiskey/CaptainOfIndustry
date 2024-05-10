// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Console.GameCommandResult
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.Console
{
  public class GameCommandResult
  {
    public readonly Option<object> Result;
    public readonly Option<string> ErrorMessage;

    public GameCommandResult(Option<object> result, Option<string> errorMessage)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Result = result;
      this.ErrorMessage = errorMessage;
    }

    public static GameCommandResult Success(object obj)
    {
      return new GameCommandResult((Option<object>) obj, Option<string>.None);
    }

    public static GameCommandResult Error(string error)
    {
      return new GameCommandResult(Option<object>.None, (Option<string>) error);
    }
  }
}
