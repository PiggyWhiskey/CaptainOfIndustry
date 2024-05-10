// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Settlements.SettlementHousingModuleProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Population;
using Mafi.Core.Prototypes;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Buildings.Settlements
{
  public class SettlementHousingModuleProto : 
    LayoutEntityProto,
    ISettlementSquareModuleProto,
    ILayoutEntityProto,
    IStaticEntityProto,
    IEntityProto,
    IProto,
    IProtoWithUpgrade<SettlementHousingModuleProto>,
    IProtoWithUpgrade
  {
    /// <summary>
    /// Needs that are required in order to apply special benefits (unity boost) of this housing.
    /// More beneficial ones are first. So if you satisfy the first one, no need to check the other ones.
    /// </summary>
    public readonly ImmutableArray<KeyValuePair<ImmutableArray<PopNeedProto>, Percent>> UnityIncreases;
    /// <summary>
    /// Increases consumption of the needs by the given percentage.
    /// </summary>
    public readonly IReadOnlyDictionary<PopNeedProto, Percent> NeedsIncreases;
    /// <summary>Max capacity of pops.</summary>
    public readonly int Capacity;
    /// <summary>Added global unity capacity.</summary>
    public readonly Upoints UpointsCapacity;
    public readonly SettlementHousingModuleProto.Gfx Graphics;

    public override Type EntityType => typeof (SettlementHousingModule);

    public UpgradeData<SettlementHousingModuleProto> Upgrade { get; }

    public IUpgradeData UpgradeNonGeneric => (IUpgradeData) this.Upgrade;

    public SettlementHousingModuleProto(
      StaticEntityProto.ID id,
      Proto.Str strings,
      EntityLayout layout,
      EntityCosts costs,
      int capacity,
      Upoints upointsCapacity,
      ImmutableArray<KeyValuePair<ImmutableArray<PopNeedProto>, Percent>> unityIncreases,
      IReadOnlyDictionary<PopNeedProto, Percent> needsIncreases,
      Option<SettlementHousingModuleProto> nextTier,
      SettlementHousingModuleProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, layout, costs, (LayoutEntityProto.Gfx) graphics);
      this.UnityIncreases = unityIncreases.AsEnumerable().OrderByDescending<KeyValuePair<ImmutableArray<PopNeedProto>, Percent>, Percent>((Func<KeyValuePair<ImmutableArray<PopNeedProto>, Percent>, Percent>) (x => x.Value)).ToImmutableArray<KeyValuePair<ImmutableArray<PopNeedProto>, Percent>>();
      this.NeedsIncreases = needsIncreases;
      this.Upgrade = new UpgradeData<SettlementHousingModuleProto>(this, nextTier);
      this.Capacity = capacity;
      this.UpointsCapacity = upointsCapacity.CheckNotNegative();
      this.Graphics = graphics;
    }

    public new class Gfx : LayoutEntityProto.Gfx
    {
      public static readonly SettlementHousingModuleProto.Gfx Empty;
      public readonly ImmutableArray<string> MaterialPaths;

      public Gfx(
        string prefabPath,
        ImmutableArray<string> materialPaths,
        RelTile3f prefabOrigin = default (RelTile3f),
        Option<string> customIconPath = default (Option<string>),
        ColorRgba color = default (ColorRgba),
        bool hideBlockedPortsIcon = false,
        LayoutEntityProto.VisualizedLayers? visualizedLayers = null,
        ImmutableArray<ToolbarCategoryProto>? categories = null)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(prefabPath, prefabOrigin, customIconPath, color, hideBlockedPortsIcon, visualizedLayers, categories);
        this.MaterialPaths = materialPaths;
      }

      static Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        SettlementHousingModuleProto.Gfx.Empty = new SettlementHousingModuleProto.Gfx("EMPTY", ImmutableArray.Create<string>("EMPTY"), RelTile3f.Zero, (Option<string>) "EMPTY", ColorRgba.Empty, visualizedLayers: new LayoutEntityProto.VisualizedLayers?(LayoutEntityProto.VisualizedLayers.Empty), categories: new ImmutableArray<ToolbarCategoryProto>?(ImmutableArray<ToolbarCategoryProto>.Empty));
      }
    }
  }
}
