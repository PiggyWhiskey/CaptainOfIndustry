// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Products.VirtualProductProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Localization.Quantity;
using Mafi.Core.Prototypes;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Products
{
  public class VirtualProductProto : ProductProto, IComparable<VirtualProductProto>
  {
    public static readonly ProductType ProductType;
    public static readonly VirtualProductProto Phantom;

    public override bool TrackSourceProducts => false;

    public VirtualProductProto(
      ProductProto.ID id,
      Proto.Str strings,
      ProductProto.Gfx graphics,
      bool doNotNormalize = false,
      bool isExcludedFromStats = true,
      QuantityFormatter quantityFormatter = null,
      IEnumerable<Tag> tags = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ProductProto.ID id1 = id;
      Proto.Str strings1 = strings;
      Mafi.Quantity maxQuantityPerTransportedProduct = 1.Quantity();
      bool flag1 = isExcludedFromStats;
      bool flag2 = doNotNormalize;
      QuantityFormatter quantityFormatter1 = quantityFormatter;
      ProductProto.Gfx graphics1 = graphics;
      int num1 = flag2 ? 1 : 0;
      int num2 = flag1 ? 1 : 0;
      IEnumerable<Tag> tags1 = tags;
      ProductProto.ID? sourceProduct = new ProductProto.ID?();
      PartialQuantity? sourceProductQuantity = new PartialQuantity?();
      QuantityFormatter quantityFormatter2 = quantityFormatter1;
      IEnumerable<Tag> tags2 = tags1;
      // ISSUE: explicit constructor call
      base.\u002Ector(id1, strings1, maxQuantityPerTransportedProduct, false, true, false, graphics1, num1 != 0, num2 != 0, sourceProduct: sourceProduct, sourceProductQuantity: sourceProductQuantity, quantityFormatter: quantityFormatter2, tags: tags2);
    }

    public int CompareTo(VirtualProductProto other) => this.CompareTo((Proto) other);

    public override string ToString() => string.Format("{0} (virtual)", (object) this.Id);

    static VirtualProductProto()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      VirtualProductProto.ProductType = new ProductType(typeof (VirtualProductProto));
      VirtualProductProto.Phantom = Proto.RegisterPhantom<VirtualProductProto>(new VirtualProductProto(new ProductProto.ID("__PHANTOM__VIRTUAL__"), Proto.Str.Empty, ProductProto.Gfx.Empty));
    }
  }
}
