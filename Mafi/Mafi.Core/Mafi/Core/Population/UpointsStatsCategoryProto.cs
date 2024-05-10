// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Population.UpointsStatsCategoryProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Prototypes;
using Mafi.Localization;

#nullable disable
namespace Mafi.Core.Population
{
  /// <summary>
  /// Read UpointsCategoryProto documentation to understand how this works.
  /// </summary>
  public class UpointsStatsCategoryProto : Proto
  {
    public readonly UpointsStatsCategoryProto.Gfx Graphics;

    /// <summary>Title is used for grouping in overview table.</summary>
    public LocStr Title { get; protected set; }

    public bool IsOneTimeAction => this.Id == IdsCore.UpointsStatsCategories.OneTimeAction;

    public bool IgnoreInStats => this.Id == IdsCore.UpointsStatsCategories.Ignore;

    public bool HideInUI => this.IsOneTimeAction || this.IgnoreInStats;

    public UpointsStatsCategoryProto(
      Proto.ID id,
      Proto.Str strings,
      UpointsStatsCategoryProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings);
      this.Graphics = graphics;
      this.Title = strings.Name;
    }

    public new class Gfx : Proto.Gfx
    {
      public static readonly UpointsStatsCategoryProto.Gfx Empty;

      /// <summary>Icon asset path to be used in UI.</summary>
      public string IconPath { get; private set; }

      public Gfx(string iconPath)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.IconPath = iconPath;
      }

      static Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        UpointsStatsCategoryProto.Gfx.Empty = new UpointsStatsCategoryProto.Gfx("EMPTY");
      }
    }
  }
}
