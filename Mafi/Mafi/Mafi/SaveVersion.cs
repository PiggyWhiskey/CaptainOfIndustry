// Decompiled with JetBrains decompiler
// Type: Mafi.SaveVersion
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using System.Collections.Generic;

#nullable disable
namespace Mafi
{
  public static class SaveVersion
  {
    public const int CURRENT_SAVE_VERSION = 168;
    public const int MIN_COMPATIBLE_SAVE_VERSION = 96;
    public const int V168_PATHFINDER_3D_GOAL_MIN_DIST = 168;
    public const int V167_SAVE_CHECKSUM = 167;
    public const int V166_VEHICLE_RECOVERY = 166;
    public const int V165_FUEL_NOTIF = 165;
    public const int V164_DESIGNATION_STUMP_ADD = 164;
    public const int V163_REFUEL_FIX2 = 163;
    public const int V162_REFUEL_FIX = 162;
    public const int V161_MAP_NAME_LOGGING = 161;
    public const int V160_GOAL_TRACKING = 160;
    public const int V159_STARTING_LOCATION_ORDER = 159;
    public const int V158_FIX_FLOWER_CONFIG = 158;
    public const int V157_MAP_SAVE_VERSION = 157;
    public const int V156_SORTER_NOTIF = 156;
    public const int V155_MAX_REPLACED_DEPTH = 155;
    public const int V154_SCREENSHOT_FOG_DENSITY = 154;
    public const int V153_SORTER_CHANGES = 153;
    public const int V152_TOO_MANY_SHIPS = 152;
    public const int V151_EROSION_IGNORE = 151;
    public const int V150_PROTECTED_MAP = 150;
    public const int V149_REMOVED_UNUSED_PLAYER_CONFIG = 149;
    public const int V148_INTERACTION_MODE_FOR_FLATTEN_AND_RAMP = 148;
    public const int V147_EROSION_SUPPRESSION_REGIONS = 147;
    public const int V146_UPDATE_2_MAP_FIX_PROP_TER_MAT = 146;
    public const int V141_UPDATE_2_SURFACE_ON_LOWERED = 145;
    public const int V141_UPDATE_2_MAP_TR_ID = 144;
    public const int V141_UPDATE_2_INITIAL_MAP_CREATION = 143;
    public const int V141_UPDATE_2_BETTER_BEDROCK_IN_MAPS = 142;
    public const int V141_UPDATE_2_EXTRA_MAP_STATS = 141;
    public const int V140_UPDATE_2 = 140;
    public const int V132_REMOVED_FULFILLED_DESIGNATION = 132;
    public const int V131_CENTURY_STATS = 131;
    public const int V130_TERRAIN_PHYSICS_UPDATE_RAMP = 130;
    public const int V129_REACTOR_SIMPLIFICATION = 129;
    public const int V128_MACHINE_BUFFER_LEAK = 128;
    public const int V127_OFF_LIMITS_DESIGNATIONS = 127;
    public const int V126_TERRAIN_MANAGER_FIELDS = 126;
    public const int V125_TREE_HARVESTER_UNREACHABLES = 125;
    public const int V124_PROPAGATE_NO_PORT_SNAP = 124;
    public const int V123_FORESTRY_EVENTS = 123;
    public const int V122_EXCAVATOR_FIXES = 122;
    public const int V121_TRANSPORT_QUICK_DELIVER = 121;
    public const int V120_FARMABLE_MANAGER = 120;
    public const int V119_UNREACHABLE_TREE_CHUNKS = 119;
    public const int V118_TERRAIN_LOAD_CACHE = 118;
    public const int V117_TREE_PLACEMENT_FIX = 117;
    public const int V116_NUCLEAR_WASTE_FIX = 116;
    public const int V115_CAMERA_SAVE = 115;
    public const int V114_CACHE_DUMPING_DESIGNATIONS = 114;
    public const int V113_QUICK_DELIVER_FROM_TRUCKS = 113;
    public const int V112_FORESTRY_ASSIGN_OUTPUT = 112;
    public const int V111_CLONE_CONFIG_ON_REPLACE = 111;
    public const int V110_STUCK_EXCAVATOR = 110;
    public const int V109_DEPRIORITIZE_FAILED_NAVS = 109;
    public const int V108_PRIORITIES_ORDER_FIX = 108;
    public const int V107_FORESTRY_STORAGE_ASSIGNMENT = 107;
    public const int V106_NAVIGATION_THROTTLING = 106;
    public const int V105_NAVIGATION_STRUGGLING = 105;
    public const int V104_PRODUCTS_LEAK = 104;
    public const int V103_NO_MINING_PREFERRED = 103;
    public const int V102_PLANTER_UNREACHABLE = 102;
    public const int V101_NOTIFICATOR_MESS = 101;
    public const int V100_LANDFILL_STATS = 100;
    public const int V99_OCEAN_AREA_NOTIFS = 99;
    public const int V98_TREES_AND_STUMPS = 98;
    public const int V97_CONTRACTS_CONFIG = 97;
    public const int V96_UPDATE1 = 96;
    public static readonly ImmutableArray<SaveVersion.MinBranchVersion> BRANCH_MAP;

    public static bool IsCompatibleSaveVersion(int version) => version >= 96 && version <= 168;

    static SaveVersion()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      SaveVersion.BRANCH_MAP = ((ICollection<SaveVersion.MinBranchVersion>) new SaveVersion.MinBranchVersion[1]
      {
        new SaveVersion.MinBranchVersion("ea-legacy", 42, "0.4.14")
      }).ToImmutableArray<SaveVersion.MinBranchVersion>();
    }

    public struct MinBranchVersion
    {
      /// <summary>Name of this Steam branch.</summary>
      public readonly string SteamBranchName;
      /// <summary>
      /// Min save version this branch can open - our internal number no one understands.
      /// </summary>
      public readonly int MinSaveVersion;
      /// <summary>
      /// The latest game version in this branch (the one we show to players).
      /// </summary>
      public readonly string LatestGameVersion;

      public MinBranchVersion(string steamBranchName, int minSaveVersion, string latestGameVersion)
      {
        MBiHIp97M4MqqbtZOh.RFLpSOptx();
        this.SteamBranchName = steamBranchName;
        this.MinSaveVersion = minSaveVersion;
        this.LatestGameVersion = latestGameVersion;
      }
    }
  }
}
