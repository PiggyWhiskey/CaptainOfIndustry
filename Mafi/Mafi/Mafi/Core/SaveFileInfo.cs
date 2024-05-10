// Decompiled with JetBrains decompiler
// Type: Mafi.Core.SaveFileInfo
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi.Core
{
  public readonly struct SaveFileInfo
  {
    /// <summary>
    /// Name of the game this save file is associated with. Can be empty.
    /// </summary>
    public readonly string GameName;
    /// <summary>Filename without extensions. May be empty string.</summary>
    public readonly string NameNoExtension;
    public readonly DateTime WriteTimestamp;
    public readonly long SizeBytes;

    public SaveFileInfo(string name, string gameName, DateTime writeTimestamp, long sizeBytes)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.NameNoExtension = name ?? "MISSING";
      this.GameName = gameName ?? "";
      this.WriteTimestamp = writeTimestamp;
      this.SizeBytes = sizeBytes;
    }

    public SaveFileInfo(string name, string gameName)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.SizeBytes = 0L;
      this.GameName = gameName ?? "";
      this.NameNoExtension = name;
      this.WriteTimestamp = DateTime.Now;
    }
  }
}
