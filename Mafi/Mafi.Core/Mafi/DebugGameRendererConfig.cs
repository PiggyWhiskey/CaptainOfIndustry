// Decompiled with JetBrains decompiler
// Type: Mafi.DebugGameRendererConfig
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi
{
  /// <summary>
  /// Common place for various debug game rendering settings to make them discoverable and easy to use during
  /// debugging. Note that these settings are only effective when <see cref="P:Mafi.DebugGameRenderer.IsEnabled" /> is true.
  /// </summary>
  public static class DebugGameRendererConfig
  {
    public static bool SaveVehiclePathFindingSuccesses { get; set; }

    public static bool SaveVehiclePathFindingFailures { get; set; }

    public static bool SaveVehiclePathFindingFailuresClosebyApproach { get; set; }

    public static bool SaveVehicleIdleTooLong { get; set; }

    public static bool SaveVehicleGettingUnstuck { get; set; }

    public static bool SaveVehicleReachabilityResults { get; set; }

    public static bool SaveVehicleHomeUnreachable { get; set; }

    public static bool SaveVehicleFailToReachHome { get; set; }

    public static bool SaveVehicleGoalUnreachable { get; set; }

    public static bool SaveVehicleGoalReplanDueToBlock { get; set; }

    public static bool SaveVehicleDriveTooFarWithoutPf { get; set; }

    public static bool SaveTransportPillarsSupportFailures { get; set; }

    public static bool SaveClearancePathabilityProviderValidationErrors { get; set; }

    public static bool SaveSuspiciouslyLongVehicleDriveTargets { get; set; }

    public static bool SaveLayoutEntityFailedToBuild { get; set; }

    public static bool? DrawTileHeights { get; set; }

    public static void EnableDebugGameRendererAndAllFailureReporting()
    {
      DebugGameRenderer.SetEnabled(true);
      DebugGameRendererConfig.SaveVehiclePathFindingFailures = true;
      DebugGameRendererConfig.SaveVehiclePathFindingFailuresClosebyApproach = true;
      DebugGameRendererConfig.SaveVehicleIdleTooLong = true;
      DebugGameRendererConfig.SaveVehicleReachabilityResults = true;
      DebugGameRendererConfig.SaveVehicleHomeUnreachable = true;
      DebugGameRendererConfig.SaveVehicleFailToReachHome = true;
      DebugGameRendererConfig.SaveClearancePathabilityProviderValidationErrors = true;
      DebugGameRendererConfig.SaveLayoutEntityFailedToBuild = true;
      DebugGameRendererConfig.SaveVehicleGoalUnreachable = true;
      DebugGameRendererConfig.SaveTransportPillarsSupportFailures = true;
      DebugGameRendererConfig.SaveVehicleDriveTooFarWithoutPf = true;
    }

    public static void EnableDebugGameRendererAndReporting()
    {
      DebugGameRendererConfig.EnableDebugGameRendererAndAllFailureReporting();
      DebugGameRendererConfig.SaveVehiclePathFindingSuccesses = true;
      DebugGameRendererConfig.SaveVehicleGettingUnstuck = true;
      DebugGameRendererConfig.SaveVehicleGoalReplanDueToBlock = true;
      DebugGameRendererConfig.SaveSuspiciouslyLongVehicleDriveTargets = true;
    }
  }
}
