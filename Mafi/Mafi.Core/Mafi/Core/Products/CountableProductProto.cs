// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Products.CountableProductProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Prototypes;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Products
{
  public class CountableProductProto : ProductProto, IComparable<CountableProductProto>
  {
    public static readonly ProductType ProductType;
    public static readonly CountableProductProto Phantom;
    public readonly CountableProductProto.Gfx Graphics;

    public CountableProductProto(
      ProductProto.ID id,
      Proto.Str strings,
      Quantity maxQuantityPerTransportedProduct,
      bool isStorable,
      CountableProductProto.Gfx graphics,
      int radioactivity = 0,
      bool isWaste = false,
      bool pinToHomeScreenByDefault = false,
      bool doNotTrackSourceProducts = false,
      ProductProto.ID? sourceProduct = null,
      PartialQuantity? sourceProductQuantity = null,
      IEnumerable<Tag> tags = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ProductProto.ID id1 = id;
      Proto.Str strings1 = strings;
      Quantity maxQuantityPerTransportedProduct1 = maxQuantityPerTransportedProduct;
      int num1 = isStorable ? 1 : 0;
      int num2 = isWaste ? 1 : 0;
      int num3 = radioactivity;
      bool flag1 = pinToHomeScreenByDefault;
      bool flag2 = doNotTrackSourceProducts;
      ProductProto.ID? nullable1 = sourceProduct;
      PartialQuantity? nullable2 = sourceProductQuantity;
      CountableProductProto.Gfx graphics1 = graphics;
      int radioactivity1 = num3;
      int num4 = flag1 ? 1 : 0;
      int num5 = flag2 ? 1 : 0;
      ProductProto.ID? sourceProduct1 = nullable1;
      PartialQuantity? sourceProductQuantity1 = nullable2;
      IEnumerable<Tag> tags1 = tags;
      // ISSUE: explicit constructor call
      base.\u002Ector(id1, strings1, maxQuantityPerTransportedProduct1, num1 != 0, false, num2 != 0, (ProductProto.Gfx) graphics1, radioactivity: radioactivity1, pinToHomeScreenByDefault: num4 != 0, doNotTrackSourceProducts: num5 != 0, sourceProduct: sourceProduct1, sourceProductQuantity: sourceProductQuantity1, tags: tags1);
      this.Graphics = graphics;
    }

    public int CompareTo(CountableProductProto other) => this.CompareTo((Proto) other);

    public override string ToString() => string.Format("{0} (unit)", (object) this.Id);

    static CountableProductProto()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      CountableProductProto.ProductType = new ProductType(typeof (CountableProductProto));
      CountableProductProto.Phantom = Proto.RegisterPhantom<CountableProductProto>(new CountableProductProto(new ProductProto.ID(ProductProto.PHANTOM_PRODUCT_ID.Value + "COUNTABLE__"), Proto.Str.Empty, Quantity.One, true, CountableProductProto.Gfx.Empty));
    }

    public new class Gfx : ProductProto.Gfx
    {
      public readonly ImmutableArray<RelTile3f> StackingOffsets;
      public static readonly CountableProductProto.Gfx Empty;
      /// <summary>Are we allowed to add noise to the packing</summary>
      public readonly bool AllowPackingNoise;
      /// <summary>Should the second item be rotated 90 degrees</summary>
      public readonly bool RotateSecondItem90Degs;

      /// <summary>How this product packs</summary>
      public CountableProductStackingMode PackingMode { get; private set; }

      public Gfx(
        string prefabPath,
        Option<string> customIconPath,
        CountableProductStackingMode packingMode,
        bool allowPackingNoise = false,
        bool rotateSecondPackedItem90Degs = false)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector((Option<string>) prefabPath, customIconPath);
        this.PackingMode = packingMode;
        this.AllowPackingNoise = allowPackingNoise;
        this.RotateSecondItem90Degs = rotateSecondPackedItem90Degs;
      }

      protected Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }

      /// <summary>
      /// Returns 5 values determining how stacked products should be offset. These offsets are computed
      /// from the corner of the tile, to avoid having to deal with negative numbers in the packing code.
      /// First two represent the stacking for 2 items.
      /// Final three represent the stacking for 3 items.
      /// It is assumed that a single item goes in the middle of the tile (ie. 0.5, 0.5 offset)
      /// </summary>
      public override ImmutableArray<RelTile3f> GetStackingOffsets(
        float sizeX,
        float sizeY,
        float sizeZ)
      {
        if (this.PackingMode == CountableProductStackingMode.Auto)
        {
          if ((double) sizeX < 0.5 && (double) sizeZ < 0.5)
            this.PackingMode = CountableProductStackingMode.Triangle;
          else if ((double) sizeX < 0.3333333432674408 || (double) sizeZ < 0.3333333432674408)
            this.PackingMode = CountableProductStackingMode.Row;
          else if ((double) sizeY < 0.3333333432674408)
            this.PackingMode = CountableProductStackingMode.Stacked;
        }
        if (this.PackingMode == CountableProductStackingMode.Triangle)
        {
          sizeX = sizeX.Min(0.49f);
          sizeZ = sizeZ.Min(0.49f);
        }
        float num1 = 2f.Sqrt();
        float num2 = 3f.Sqrt();
        RelTile1f relTile1f1 = 0.5.Meters();
        ThicknessTilesF zero = ThicknessTilesF.Zero;
        RelTile1f relTile1f2 = relTile1f1 + RelTile1f.Epsilon;
        ThicknessTilesF z = ThicknessTilesF.Zero + ThicknessTilesF.Epsilon;
        switch (this.PackingMode)
        {
          case CountableProductStackingMode.Stacked:
          case CountableProductStackingMode.StackedAlternating:
            return ImmutableArray.Create<RelTile3f>(new RelTile3f(relTile1f1, relTile1f1, zero), new RelTile3f(relTile1f2, relTile1f2, ThicknessTilesF.FromMeters(sizeY)), new RelTile3f(relTile1f1, relTile1f1, zero), new RelTile3f(relTile1f2, relTile1f2, ThicknessTilesF.FromMeters(sizeY)), new RelTile3f(relTile1f1, relTile1f1, ThicknessTilesF.FromMeters(sizeY * 2f)));
          case CountableProductStackingMode.Triangle:
            float num3 = Math.Max(sizeX, sizeZ);
            return ImmutableArray.Create<RelTile3f>(new RelTile3f((0.5 - (double) num3 / (double) num2).Meters(), relTile1f1, zero), new RelTile3f((0.5 + (double) num3 / (double) num2).Meters(), relTile1f1, zero), new RelTile3f(relTile1f1, (0.5 + 0.5 * (double) num3).Meters(), zero), new RelTile3f((0.5 - (double) num3 / (double) num2).Meters(), (0.5 - 0.5 * (double) num3).Meters(), zero), new RelTile3f((0.5 + (double) num3 / (double) num2).Meters(), (0.5 - 0.5 * (double) num3).Meters(), zero));
          case CountableProductStackingMode.TriangleHorizontal:
            return ImmutableArray.Create<RelTile3f>(new RelTile3f((0.5 + (double) sizeX / 2.0).Meters(), relTile1f1, zero), new RelTile3f((0.5 - (double) sizeX / 2.0).Meters(), relTile1f1, zero), new RelTile3f((0.5 + (double) sizeX / 2.0).Meters(), relTile1f1, zero), new RelTile3f((0.5 - (double) sizeX / 2.0).Meters(), relTile1f1, zero), new RelTile3f(relTile1f1, relTile1f2, ThicknessTilesF.FromMeters((float) (0.5 * (double) sizeY * (1.0 + 1.0 / (double) num1)))));
          case CountableProductStackingMode.Row:
            return (double) sizeX > (double) sizeZ ? ImmutableArray.Create<RelTile3f>(new RelTile3f(relTile1f2, (0.5 - 0.75 * (double) sizeZ).Meters(), zero), new RelTile3f(relTile1f1, (0.5 + 0.75 * (double) sizeZ).Meters(), z), new RelTile3f(relTile1f2, relTile1f1, z), new RelTile3f(relTile1f1, (0.5 - 1.5 * (double) sizeZ).Meters(), zero), new RelTile3f(relTile1f1, (0.5 + 1.5 * (double) sizeZ).Meters(), zero)) : ImmutableArray.Create<RelTile3f>(new RelTile3f((0.5 - 0.75 * (double) sizeX).Meters(), relTile1f2, zero), new RelTile3f((0.5 + 0.75 * (double) sizeX).Meters(), relTile1f1, z), new RelTile3f(relTile1f1, relTile1f2, z), new RelTile3f((0.5 - 1.5 * (double) sizeX).Meters(), relTile1f1, zero), new RelTile3f((0.5 + 1.5 * (double) sizeX).Meters(), relTile1f1, zero));
          default:
            return ImmutableArray.Create<RelTile3f>(new RelTile3f(relTile1f1, relTile1f1, zero), new RelTile3f(relTile1f1, relTile1f1, zero), new RelTile3f(relTile1f1, relTile1f1, zero), new RelTile3f(relTile1f1, relTile1f1, zero), new RelTile3f(relTile1f1, relTile1f1, zero));
        }
      }

      static Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        CountableProductProto.Gfx.Empty = new CountableProductProto.Gfx();
      }
    }
  }
}
