// Decompiled with JetBrains decompiler
// Type: Mafi.Core.SaveFileGroup
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;

#nullable disable
namespace Mafi.Core
{
  public readonly struct SaveFileGroup
  {
    public readonly string GameName;
    public readonly ImmutableArray<SaveFileInfo> Saves;

    public SaveFileGroup(string gameName, ImmutableArray<SaveFileInfo> saves)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.GameName = gameName ?? "";
      this.Saves = saves;
    }
  }
}
