// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Fleet.FleetEntityGfx
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Fleet
{
  public class FleetEntityGfx : Proto.Gfx
  {
    public static readonly FleetEntityGfx Empty;
    public readonly string IconPath;
    /// <summary>Name of sub-models that will be shown.</summary>
    public readonly Option<string> GameObjectToShow;

    public FleetEntityGfx(string iconPath)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(iconPath, Option<string>.None);
      this.IconPath = iconPath;
    }

    public FleetEntityGfx(string iconPath, Option<string> gameObjectToShow)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.IconPath = iconPath;
      this.GameObjectToShow = gameObjectToShow;
    }

    static FleetEntityGfx()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      FleetEntityGfx.Empty = new FleetEntityGfx("EMPTY");
    }
  }
}
