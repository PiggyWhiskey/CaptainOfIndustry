// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Priorities.GeneralPriorities
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi.Core.Entities.Priorities
{
  public static class GeneralPriorities
  {
    public const int HIGHEST_PRIORITY = 0;
    public const int LOWEST_ACTIONABLE_PRIORITY = 14;
    public const int LOWEST_PRIORITY = 15;
    public const int IGNORE = 16;
    public const int DEFAULT = 9;
    public const int HIGH = 8;
    public const int VERY_HIGH = 7;
    public const int SUPER_HIGH = 4;
    public const int LOAN_PAYMENTS = 5;
    public const int CONSTRUCTION_DECONSTRUCTION = 7;
    public const int CONSTRUCTION_DECONSTRUCTION_PRIORITIZED = 0;
    public const int CLEARING = 7;
    public const int CARGO_DEPOT_MODULE_CARGO = 14;
    public const int RUINS_EXPORT = 14;
    public const int SHIPYARD_REPAIR_IMPORT_LOW = 14;
    public const int SHIPYARD_DEFAULT_CARGO_EXPORT = 14;
    public const int TRADE_DOCK_DEFAULT_CARGO_EXPORT = 14;
    public const int VEHICLE_DEPOT_EXPORT = 14;
    public const int STORAGE_CARGO_INCREASED = 8;
    public const int VEHICLES = 0;
    public const int SHIP = 0;
    public const int CARGO_SHIPS = 0;
    public const int TRANSPORTS_ZIPPERS = 4;
    public const int STORAGE = 4;
    public const int FARM = 8;
    public const int POWER = 8;
    public const int WORLD_MINES = 8;
    public const int SETTLEMENT_MODULE = 7;
    public const int VEHICLE_DEPOT = 7;

    public static bool AssertAssignableRange(int priority)
    {
      if (priority >= 0 && priority <= 14)
        return true;
      Log.Error(string.Format("Priority {0} out of range {1}-{2}", (object) priority, (object) 0, (object) 14));
      return false;
    }
  }
}
