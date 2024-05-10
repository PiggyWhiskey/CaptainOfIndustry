// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Utils.ChangelogEntry
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;

#nullable disable
namespace Mafi.Core.Utils
{
  public readonly struct ChangelogEntry
  {
    public readonly string Version;
    public readonly Lyst<ChangelogSubEntry> SubEntries;

    public ChangelogEntry(string version)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Version = version;
      this.SubEntries = new Lyst<ChangelogSubEntry>();
    }
  }
}
