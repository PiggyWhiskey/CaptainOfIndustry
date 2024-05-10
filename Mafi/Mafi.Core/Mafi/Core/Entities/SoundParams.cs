// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.SoundParams
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.Entities
{
  public readonly struct SoundParams
  {
    public string SoundPrefabPath { get; }

    public SoundSignificance Significance { get; }

    public bool Loop { get; }

    public bool FadeOnChange { get; }

    public bool DoNotLimit { get; }

    public SoundParams(
      string prefabPath,
      SoundSignificance significance,
      bool loop = true,
      bool fadeOnChange = true,
      bool doNotLimit = false)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.SoundPrefabPath = prefabPath;
      this.Significance = significance;
      this.Loop = loop;
      this.FadeOnChange = fadeOnChange;
      this.DoNotLimit = doNotLimit;
    }
  }
}
