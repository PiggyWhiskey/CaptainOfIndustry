// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Storages.UnitStorageProductRackPlacementParams
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.Buildings.Storages
{
  public readonly struct UnitStorageProductRackPlacementParams
  {
    public readonly float RackHeightSpacing;
    public readonly float RackDepthOffset;
    public readonly float ProductXSpacing;
    public readonly float ProductYOffset;
    public readonly float ProductZOffset;
    public static readonly UnitStorageProductRackPlacementParams Default;

    public UnitStorageProductRackPlacementParams(
      float rackHeightSpacing,
      float rackDepthOffset,
      float productXSpacing,
      float productYOffset,
      float productZOffset)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.RackHeightSpacing = 1.05f;
      this.RackDepthOffset = 1.5f;
      this.ProductXSpacing = 0.11f;
      this.ProductYOffset = 0.8f;
      this.ProductZOffset = 0.75f;
      this.RackHeightSpacing = rackHeightSpacing;
      this.RackDepthOffset = rackDepthOffset;
      this.ProductXSpacing = productXSpacing;
      this.ProductYOffset = productYOffset;
      this.ProductZOffset = productZOffset;
    }

    static UnitStorageProductRackPlacementParams()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      UnitStorageProductRackPlacementParams.Default = new UnitStorageProductRackPlacementParams(1.05f, 1.5f, 0.11f, 0.8f, 0.75f);
    }
  }
}
