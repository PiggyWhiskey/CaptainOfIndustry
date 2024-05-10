// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Products.FluidProductProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Prototypes;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Products
{
  public class FluidProductProto : ProductProto, IComparable<FluidProductProto>
  {
    public static readonly ProductType ProductType;
    /// <summary>
    /// Phantom prototype does not represent any valid prototype and serves as convenient placeholder to avoid
    /// redundant null checks or unnecessary usage of Option{T}. This is also useful for unit tests. Phantom
    /// prototype should be NEVER returned through public interface of the class that uses it.
    /// </summary>
    public static readonly FluidProductProto Phantom;

    public FluidProductProto(
      ProductProto.ID id,
      Proto.Str strings,
      bool isStorable,
      bool canBeDiscarded,
      bool isWaste,
      ProductProto.Gfx graphics,
      bool pinToHomeScreenByDefault = false,
      Quantity? maxQuantityPerTransportedProduct = null,
      IEnumerable<Tag> tags = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ProductProto.ID id1 = id;
      Proto.Str strings1 = strings;
      Quantity maxQuantityPerTransportedProduct1 = maxQuantityPerTransportedProduct ?? 3.Quantity();
      int num1 = isStorable ? 1 : 0;
      int num2 = canBeDiscarded ? 1 : 0;
      int num3 = isWaste ? 1 : 0;
      bool flag = pinToHomeScreenByDefault;
      ProductProto.Gfx graphics1 = graphics;
      int num4 = flag ? 1 : 0;
      IEnumerable<Tag> tags1 = tags;
      ProductProto.ID? sourceProduct = new ProductProto.ID?();
      PartialQuantity? sourceProductQuantity = new PartialQuantity?();
      IEnumerable<Tag> tags2 = tags1;
      // ISSUE: explicit constructor call
      base.\u002Ector(id1, strings1, maxQuantityPerTransportedProduct1, num1 != 0, num2 != 0, num3 != 0, graphics1, pinToHomeScreenByDefault: num4 != 0, doNotTrackSourceProducts: true, sourceProduct: sourceProduct, sourceProductQuantity: sourceProductQuantity, tags: tags2);
      if (graphics.PrefabsPath.HasValue)
        throw new InvalidProtoException(string.Format("Fluid proto '{0}' should have no prefab but has '{1}'.", (object) id, (object) graphics.PrefabsPath.Value));
      if (graphics.Color.IsEmpty)
        throw new InvalidProtoException(string.Format("Fluid proto '{0}' is missing color.", (object) id));
    }

    public int CompareTo(FluidProductProto other) => this.CompareTo((Proto) other);

    public override string ToString() => string.Format("{0} (fluid)", (object) this.Id);

    static FluidProductProto()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      FluidProductProto.ProductType = new ProductType(typeof (FluidProductProto));
      FluidProductProto.Phantom = Proto.RegisterPhantom<FluidProductProto>(new FluidProductProto(new ProductProto.ID(ProductProto.PHANTOM_PRODUCT_ID.Value + "FLUID__"), Proto.Str.Empty, false, false, false, new ProductProto.Gfx(Option<string>.None, (Option<string>) "TODO: Phantom icon", ColorRgba.Gray)));
    }
  }
}
