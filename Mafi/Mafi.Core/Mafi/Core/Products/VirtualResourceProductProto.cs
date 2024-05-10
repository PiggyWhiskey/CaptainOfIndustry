// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Products.VirtualResourceProductProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Prototypes;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Products
{
  /// <summary>
  /// Represents a product that can be mined from virtual resources of a terrain. Contains mined ProductProto and
  /// additional properties necessary for virtual mining/visualization.
  /// </summary>
  public class VirtualResourceProductProto : Proto
  {
    /// <summary>The mined product.</summary>
    public readonly ProductProto Product;
    public readonly VirtualResourceProductProto.Gfx Graphics;
    /// <summary>
    /// If resource is not final we will show its designations even when it temporarily
    /// ran out because otherwise it might be confusing and scary :)
    /// </summary>
    [OnlyForSaveCompatibility(null)]
    public readonly bool IsResourceFinal;

    public VirtualResourceProductProto(
      Proto.ID id,
      Proto.Str strings,
      ProductProto product,
      bool isResourceFinal,
      VirtualResourceProductProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings);
      this.Product = product.CheckNotNull<ProductProto>();
      this.Graphics = graphics.CheckNotNull<VirtualResourceProductProto.Gfx>();
      this.IsResourceFinal = isResourceFinal;
    }

    public new class Gfx : Proto.Gfx
    {
      public static readonly VirtualResourceProductProto.Gfx Empty;
      public ColorRgba ResourcesVizColor;
      public ThicknessTilesF ResourceBarsMaxHeight;

      public Gfx(ColorRgba resourcesVizColor, ThicknessTilesF resourceBarsMaxHeight)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.ResourcesVizColor = resourcesVizColor;
        this.ResourceBarsMaxHeight = resourceBarsMaxHeight;
      }

      static Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        VirtualResourceProductProto.Gfx.Empty = new VirtualResourceProductProto.Gfx(ColorRgba.Empty, 10.0.TilesThick());
      }
    }
  }
}
