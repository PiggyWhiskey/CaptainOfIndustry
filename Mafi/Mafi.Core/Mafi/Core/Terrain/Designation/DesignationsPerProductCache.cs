// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Designation.DesignationsPerProductCache
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Products;

#nullable disable
namespace Mafi.Core.Terrain.Designation
{
  public class DesignationsPerProductCache
  {
    public readonly ProductProto Product;
    public Quantity TotalToPlace;
    public Quantity ReservedToPlace;
    public Quantity TotalToClear;
    public Quantity ReservedToClear;
    public readonly Lyst<SurfaceDesignation> Placement;
    public readonly Lyst<SurfaceDesignation> Clearing;

    public Quantity LeftToPlace => this.TotalToPlace - this.ReservedToPlace;

    public Quantity LeftToClear => this.TotalToClear - this.ReservedToClear;

    public DesignationsPerProductCache(ProductProto product)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Placement = new Lyst<SurfaceDesignation>();
      this.Clearing = new Lyst<SurfaceDesignation>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Product = product;
    }

    public void Reset()
    {
      this.Placement.Clear();
      this.Clearing.Clear();
      this.TotalToPlace = Quantity.Zero;
      this.ReservedToPlace = Quantity.Zero;
      this.TotalToClear = Quantity.Zero;
      this.ReservedToClear = Quantity.Zero;
    }
  }
}
