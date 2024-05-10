// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Dynamic.DustParticlesSpec
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.Entities.Dynamic
{
  public class DustParticlesSpec
  {
    public readonly string PrefabPath;
    /// <summary>Scale of dust and also lifetime multiplier.</summary>
    public readonly float DustScale;
    public readonly RelTile3f RelativePosition;

    public DustParticlesSpec(string prefabPath, float dustScale, RelTile3f relativePosition)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.PrefabPath = prefabPath;
      this.DustScale = dustScale;
      this.RelativePosition = relativePosition;
    }
  }
}
