// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Storages.LooseStorageProto
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
  [DebuggerDisplay("LooseStorageProto: {Id}")]
  public class LooseStorageProto : StorageProto
  {
    public readonly LooseStorageProto.Gfx Graphics;

    public LooseStorageProto(
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
      LooseStorageProto.Gfx graphics,
      IEnumerable<Tag> tags = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, layout, productsFilter, new Mafi.Core.Products.ProductType?(LooseProductProto.ProductType), capacity, costs, transferLimit, transferLimitDuration, powerConsumedForProductsExchange, nextTier, (LayoutEntityProto.Gfx) graphics, tags);
      this.Graphics = graphics;
    }

    public new class Gfx : LayoutEntityProto.Gfx
    {
      public static readonly LooseStorageProto.Gfx Empty;
      public readonly string SmoothPileObjectPath;
      public readonly string RoughPileObjectPath;
      public readonly LoosePileTextureParams PileTextureParams;

      public Gfx(
        string prefabPath,
        string smoothPileObjectPath,
        string roughPileObjectPath,
        LoosePileTextureParams pileTextureParams,
        RelTile3f prefabOrigin = default (RelTile3f),
        Option<string> customIconPath = default (Option<string>),
        ColorRgba color = default (ColorRgba),
        bool hideBlockedPortsIcon = false,
        LayoutEntityProto.VisualizedLayers? visualizedLayers = null,
        ImmutableArray<ToolbarCategoryProto>? categories = null,
        bool useSemiInstancedRendering = false,
        ImmutableArray<string> instancedRenderingExcludedObjects = default (ImmutableArray<string>))
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(prefabPath, prefabOrigin, customIconPath, color, hideBlockedPortsIcon, visualizedLayers, categories, useSemiInstancedRendering: useSemiInstancedRendering, instancedRenderingExcludedObjects: instancedRenderingExcludedObjects);
        this.SmoothPileObjectPath = smoothPileObjectPath;
        this.RoughPileObjectPath = roughPileObjectPath;
        this.PileTextureParams = pileTextureParams;
      }

      static Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        LooseStorageProto.Gfx.Empty = new LooseStorageProto.Gfx("EMPTY", "EMPTY", "EMPTY", LoosePileTextureParams.Default);
      }
    }
  }
}
