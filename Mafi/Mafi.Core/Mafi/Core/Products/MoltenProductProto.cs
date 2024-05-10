// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Products.MoltenProductProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Prototypes;
using System;

#nullable disable
namespace Mafi.Core.Products
{
  public class MoltenProductProto : ProductProto, IComparable<MoltenProductProto>
  {
    public static readonly ProductType ProductType;
    public static readonly MoltenProductProto Phantom;
    public readonly MoltenProductProto.Gfx Graphics;

    public MoltenProductProto(
      ProductProto.ID id,
      Proto.Str strings,
      MoltenProductProto.Gfx graphics,
      Quantity? maxQuantityPerTransportedProduct = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, maxQuantityPerTransportedProduct ?? Quantity.One, false, true, false, (ProductProto.Gfx) graphics);
      this.Graphics = graphics;
    }

    public int CompareTo(MoltenProductProto other) => this.CompareTo((Proto) other);

    public override string ToString() => string.Format("{0} (molten)", (object) this.Id);

    static MoltenProductProto()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      MoltenProductProto.ProductType = new ProductType(typeof (MoltenProductProto));
      MoltenProductProto.Phantom = Proto.RegisterPhantom<MoltenProductProto>(new MoltenProductProto(new ProductProto.ID(ProductProto.PHANTOM_PRODUCT_ID.Value + "MOLTEN__"), Proto.Str.Empty, MoltenProductProto.Gfx.Empty));
    }

    public new class Gfx : ProductProto.Gfx
    {
      public static readonly MoltenProductProto.Gfx Empty;
      /// <summary>
      /// Path to material of this molten product. This is used by transport.
      /// </summary>
      public readonly string MaterialPath;

      public Gfx(string prefabPath, Option<string> customIconPath, string materialPath)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector((Option<string>) prefabPath, customIconPath);
        this.MaterialPath = materialPath.CheckNotNullOrEmpty();
      }

      public Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.MaterialPath = "";
      }

      static Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        MoltenProductProto.Gfx.Empty = new MoltenProductProto.Gfx();
      }
    }
  }
}
