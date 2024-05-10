// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Utils.CoreConsoleCommands
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Console;
using Mafi.Localization;
using System;

#nullable disable
namespace Mafi.Core.Utils
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class CoreConsoleCommands
  {
    [ConsoleCommand(false, false, null, null)]
    private string printTranslationWarnings()
    {
      return "\n" + LocalizationManager.TranslationWarnings.Select<string, string>((Func<string, int, string>) ((x, i) => string.Format("#{0:##}: {1}", (object) (i + 1), (object) x))).JoinStrings("\n");
    }

    [ConsoleCommand(false, false, null, null)]
    private string printTranslationErrors()
    {
      return "\n" + LocalizationManager.TranslationErrors.Select<string, string>((Func<string, int, string>) ((x, i) => string.Format("#{0:##}: {1}", (object) (i + 1), (object) x))).JoinStrings("\n");
    }

    public CoreConsoleCommands()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
