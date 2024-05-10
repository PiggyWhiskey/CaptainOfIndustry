// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Prototypes.EntityCosts
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Economy;
using Mafi.Core.Maintenance;
using Mafi.Core.Products;

#nullable disable
namespace Mafi.Core.Prototypes
{
  public struct EntityCosts
  {
    public static readonly EntityCosts None;
    /// <summary>Price of an entity to be added to the world.</summary>
    public readonly AssetValue Price;
    public readonly int Workers;
    public readonly int DefaultPriority;
    public readonly bool IsQuickBuildDisabled;
    public readonly MaintenanceCosts Maintenance;

    public EntityCosts(
      AssetValue price,
      int defaultPriority = 9,
      int workers = 0,
      MaintenanceCosts? maintenance = null,
      bool isQuickBuildDisabled = false)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Price = price;
      this.DefaultPriority = defaultPriority;
      this.Workers = workers;
      this.IsQuickBuildDisabled = isQuickBuildDisabled;
      this.Maintenance = maintenance ?? new MaintenanceCosts(VirtualProductProto.Phantom, Quantity.Zero);
    }

    static EntityCosts()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      EntityCosts.None = new EntityCosts(AssetValue.Empty);
    }
  }
}
