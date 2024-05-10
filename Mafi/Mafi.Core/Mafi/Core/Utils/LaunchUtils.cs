// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Utils.LaunchUtils
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi.Core.Utils
{
  public static class LaunchUtils
  {
    public static readonly DateTime EARLY_ACCESS_LAUNCH_DATE_TIME;

    public static TimeSpan GetTimeUntilEaLaunch()
    {
      return LaunchUtils.EARLY_ACCESS_LAUNCH_DATE_TIME - DateTime.UtcNow;
    }

    static LaunchUtils()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      LaunchUtils.EARLY_ACCESS_LAUNCH_DATE_TIME = new DateTime(2022, 5, 31, 13, 0, 0, DateTimeKind.Utc);
    }
  }
}
