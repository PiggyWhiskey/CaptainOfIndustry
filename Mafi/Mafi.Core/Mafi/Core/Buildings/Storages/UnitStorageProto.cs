// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Storages.UnitStorageProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;

#nullable disable
namespace Mafi.Core.Buildings.Storages
{
  [DebuggerDisplay("UnitStorageProto: {Id}")]
  public class UnitStorageProto : StorageProto
  {
    public readonly UnitStorageProto.Gfx Graphics;

    public UnitStorageProto(
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
      UnitStorageProto.Gfx graphics,
      IEnumerable<Tag> tags = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, layout, productsFilter, new Mafi.Core.Products.ProductType?(CountableProductProto.ProductType), capacity, costs, transferLimit, transferLimitDuration, powerConsumedForProductsExchange, nextTier, (LayoutEntityProto.Gfx) graphics, tags);
      this.Graphics = graphics;
    }

    public new class Gfx : LayoutEntityProto.Gfx
    {
      public static readonly UnitStorageProto.Gfx Empty;
      /// <summary>Position and sizing info of each rack</summary>
      private readonly UnitStorageRackData[] m_rackData;
      /// <summary>Global placement params for racks and products.</summary>
      private readonly UnitStorageProductRackPlacementParams m_placementParams;
      /// <summary>Prefab for the rack.</summary>
      public readonly string RackPrefabPath;

      /// <summary>The maximum amount of products we can render.</summary>
      public int MaxProductRenderCapacity { get; private set; }

      /// <summary>The number of rack layers.</summary>
      public int RackLayers => this.RackRenderOffsets.Length / 2;

      /// <summary>
      /// The offsets to render the products. We expect to render 3 products at each position.
      /// </summary>
      public ImmutableArray<Vector3> ProductRenderOffsets { get; private set; }

      /// <summary>The offsets to render the racks.</summary>
      public ImmutableArray<Vector3> RackRenderOffsets { get; private set; }

      public Gfx(
        UnitStorageRackData[] rackData,
        UnitStorageProductRackPlacementParams placementParams,
        string prefabPath,
        string rackPrefabPath,
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
        this.m_rackData = rackData;
        this.m_placementParams = placementParams;
        this.RackPrefabPath = rackPrefabPath;
      }

      public override void Initialize(ILayoutEntityProto proto)
      {
        Mafi.Assert.That<LayoutEntityProto.Gfx>(proto.Graphics).IsEqualTo<LayoutEntityProto.Gfx>((LayoutEntityProto.Gfx) this);
        int num1 = proto.Layout.LayoutSize.Y * 2;
        float rackHeightSpacing = this.m_placementParams.RackHeightSpacing;
        int height = this.m_rackData[0].Height;
        ImmutableArrayBuilder<Vector3> immutableArrayBuilder1 = new ImmutableArrayBuilder<Vector3>(height * 2);
        for (int index1 = 0; index1 < this.m_rackData[0].Height; ++index1)
        {
          for (int index2 = 0; index2 < 2; ++index2)
            immutableArrayBuilder1[index1 * 2 + index2] = new Vector3(0.0f, (float) index1 * rackHeightSpacing, (float) index2 * ((float) num1 - this.m_placementParams.RackDepthOffset));
        }
        this.RackRenderOffsets = immutableArrayBuilder1.GetImmutableArrayAndClear();
        float productYoffset = this.m_placementParams.ProductYOffset;
        float productZoffset = this.m_placementParams.ProductZOffset;
        int i1 = 0;
        this.MaxProductRenderCapacity = 0;
        foreach (UnitStorageRackData unitStorageRackData in this.m_rackData)
          this.MaxProductRenderCapacity += 3 * unitStorageRackData.Height * unitStorageRackData.Width;
        ImmutableArrayBuilder<Vector3> immutableArrayBuilder2 = new ImmutableArrayBuilder<Vector3>(this.MaxProductRenderCapacity * 2);
        for (int index3 = 0; index3 < height; ++index3)
        {
          foreach (UnitStorageRackData unitStorageRackData in this.m_rackData)
          {
            float productXspacing = this.m_placementParams.ProductXSpacing;
            int width = unitStorageRackData.Width;
            int num2 = ((float) width * productXspacing).CeilToInt();
            int num3 = width - num2;
            float num4 = (float) num3 + (float) (num3 - 1) * productXspacing;
            float num5 = unitStorageRackData.StartX + (float) (((double) width - (double) num4) / 2.0);
            float num6 = (float) (-num1 / 2);
            for (int index4 = 1; index4 <= width; ++index4)
            {
              immutableArrayBuilder2[i1] = new Vector3((float) ((double) num5 + (double) index4 + (double) (index4 - 1) * (double) productXspacing), (float) index3 * rackHeightSpacing + productYoffset, num6 + productZoffset);
              int i2 = i1 + 1;
              immutableArrayBuilder2[i2] = new Vector3((float) ((double) num5 + (double) index4 + (double) (index4 - 1) * (double) productXspacing), (float) index3 * rackHeightSpacing + productYoffset, num6 + (float) num1 - productZoffset);
              i1 = i2 + 1;
            }
          }
        }
        this.ProductRenderOffsets = immutableArrayBuilder2.GetImmutableArrayAndClear();
        base.Initialize(proto);
      }

      static Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        UnitStorageProto.Gfx.Empty = new UnitStorageProto.Gfx(Array.Empty<UnitStorageRackData>(), UnitStorageProductRackPlacementParams.Default, "EMPTY", "EMPTY");
      }
    }
  }
}
