// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Storages.FluidStorageProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Mafi.Core.Buildings.Storages
{
  [DebuggerDisplay("FluidStorageProto: {Id}")]
  public class FluidStorageProto : StorageProto
  {
    public readonly FluidStorageProto.Gfx Graphics;

    public FluidStorageProto(
      StaticEntityProto.ID id,
      Proto.Str strings,
      EntityLayout layout,
      Func<ProductProto, bool> productsFilter,
      Quantity capacity,
      EntityCosts costs,
      Quantity transferLimit,
      Duration transferLimitDuration,
      Electricity powerConsumedForProductsExchange,
      Option<StorageProto> nextTier,
      FluidStorageProto.Gfx graphics,
      IEnumerable<Tag> tags = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, layout, productsFilter, new Mafi.Core.Products.ProductType?(FluidProductProto.ProductType), capacity, costs, transferLimit, transferLimitDuration, powerConsumedForProductsExchange, nextTier, (LayoutEntityProto.Gfx) graphics, tags);
      this.Graphics = graphics;
    }

    public new class Gfx : LayoutEntityProto.Gfx
    {
      public static readonly FluidStorageProto.Gfx Empty;
      public readonly string SignObjectPath;
      public readonly string FluidIndicatorObjectPath;
      public readonly FluidIndicatorGfxParams FluidIndicatorParams;

      public Gfx(
        string prefabPath,
        string signObjectPath,
        string fluidIndicatorObjectPath,
        FluidIndicatorGfxParams fluidIndicatorParams,
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
        this.SignObjectPath = signObjectPath;
        this.FluidIndicatorObjectPath = fluidIndicatorObjectPath;
        this.FluidIndicatorParams = fluidIndicatorParams;
      }

      static Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        FluidStorageProto.Gfx.Empty = new FluidStorageProto.Gfx("EMPTY", "EMPTY", "EMPTY", new FluidIndicatorGfxParams(1f, 1f, 1f));
      }
    }
  }
}
