// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.IUpgrader
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Economy;
using Mafi.Core.Prototypes;
using Mafi.Localization;

#nullable disable
namespace Mafi.Core.Entities
{
  /// <summary>Entity component that handles its upgrade process.</summary>
  public interface IUpgrader
  {
    /// <summary>
    /// How much does the upgrade costs in total (including virtual products).
    /// </summary>
    AssetValue PriceToUpgrade { get; }

    /// <summary>
    /// Construction costs for the upgrades (virtual products removed).
    /// </summary>
    AssetValue ConstructionCostToUpgrade { get; }

    /// <summary>Whether upgrade exists.</summary>
    bool UpgradeExists { get; }

    /// <summary>Title of the upgrade to be shown in the UI.</summary>
    LocStr UpgradeTitle { get; }

    Option<Proto> NextTier { get; }

    /// <summary>Icon of the upgraded item.</summary>
    string Icon { get; }

    /// <summary>
    /// Whether the upgrade should be offered to the player. This does not necessary mean that the upgrade is
    /// available. There might be something that is blocking the upgrade (e.g. not enough space or money). But still
    /// the player should know that there is a chance for upgrade.
    /// </summary>
    bool IsUpgradeVisible();

    /// <summary>
    /// Whether the upgrade is available immediately. This is more restrictive than <see cref="M:Mafi.Core.Entities.IUpgrader.IsUpgradeVisible" />.
    /// When this is false it means that upgrade might be available but something is blocking it (e.g. not enough
    /// money).
    /// </summary>
    bool IsUpgradeAvailable(out LocStrFormatted errorMessage);

    /// <summary>Performs upgrade of the current entity.</summary>
    void Upgrade();

    void UpgradeStarted();

    void UpgradeCanceled();
  }
}
