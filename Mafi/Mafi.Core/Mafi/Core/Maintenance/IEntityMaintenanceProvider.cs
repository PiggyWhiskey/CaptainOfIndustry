// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Maintenance.IEntityMaintenanceProvider
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.PropertiesDb;

#nullable disable
namespace Mafi.Core.Maintenance
{
  public interface IEntityMaintenanceProvider
  {
    MaintenanceCosts Costs { get; }

    MaintenanceStatus Status { get; }

    Upoints RepairCost { get; }

    /// <summary>
    /// Returns false if entity shouldn't work due to maintenance issues.
    /// This is when it is broken but it can be overriden via difficulty config.
    /// </summary>
    bool CanWork();

    /// <summary>
    /// Return true if entity should slow down due to maintenance issues.
    /// This happens if it is broken but on easier difficulty.
    /// </summary>
    bool ShouldSlowDown();

    void SetCurrentMaintenanceTo(Percent percent);

    /// <summary>
    /// Used to set extra property that can affect the maintenance.
    /// </summary>
    void SetExtraMultiplierProperty(IProperty<Percent> property);

    /// <summary>
    /// Used to set a scaling factor that can affect the maintenance.
    /// Note: this is not cumulative: only the latest scaling factor set is applied.
    /// </summary>
    void SetDynamicExtraMultiplier(Percent percent);

    void DecreaseBy(Percent percent);

    void RefreshMaintenanceCost();
  }
}
