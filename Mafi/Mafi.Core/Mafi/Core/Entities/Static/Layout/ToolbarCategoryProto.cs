// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.Layout.ToolbarCategoryProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Prototypes;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Mafi.Core.Entities.Static.Layout
{
  [DebuggerDisplay("ToolbarCategoryProto: {Id}")]
  public class ToolbarCategoryProto : Proto
  {
    public static readonly Proto.ID PHANTOM_CATEGORY_ID;
    public static readonly ToolbarCategoryProto Phantom;
    public readonly float Order;
    public readonly string ShortcutId;
    /// <summary>Path for icon sprite.</summary>
    public readonly string IconPath;
    /// <summary>
    /// Whether transport building is allowed while this menu is open (and entity laying is not active).
    /// </summary>
    public readonly bool IsTransportBuildAllowed;
    public readonly bool ContainsTransports;

    public ToolbarCategoryProto(
      Proto.ID id,
      Proto.Str strings,
      float order,
      string iconPath,
      bool isTransportBuildAllowed = false,
      bool containsTransports = false,
      string shortcutId = "",
      IEnumerable<Tag> tags = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, tags);
      Mafi.Assert.That<bool>(isTransportBuildAllowed || !containsTransports).IsTrue();
      this.Order = order;
      this.IconPath = iconPath;
      this.IsTransportBuildAllowed = isTransportBuildAllowed;
      this.ContainsTransports = containsTransports;
      this.ShortcutId = shortcutId ?? "";
    }

    static ToolbarCategoryProto()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ToolbarCategoryProto.PHANTOM_CATEGORY_ID = new Proto.ID("__PHANTOM__TOOLBAR_CAT__");
      ToolbarCategoryProto.Phantom = Proto.RegisterPhantom<ToolbarCategoryProto>(new ToolbarCategoryProto(ToolbarCategoryProto.PHANTOM_CATEGORY_ID, Proto.Str.Empty, 0.0f, ""));
    }
  }
}
