// Decompiled with JetBrains decompiler
// Type: Mafi.Core.SaveGame.LoadFailInfo
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Localization;

#nullable disable
namespace Mafi.Core.SaveGame
{
  public class LoadFailInfo
  {
    public readonly LoadFailInfo.Reason FailReason;
    public readonly int? SaveVersion;
    public readonly LocStrFormatted? MessageForPlayer;

    public LoadFailInfo(
      LoadFailInfo.Reason reason,
      int? saveVersion = null,
      LocStrFormatted? messageForPlayer = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.FailReason = reason;
      this.SaveVersion = saveVersion;
      this.MessageForPlayer = messageForPlayer;
    }

    public enum Reason
    {
      Version,
      FileAccessIssue,
      ModsMissing,
      FileCorrupted,
      Unknown,
    }
  }
}
