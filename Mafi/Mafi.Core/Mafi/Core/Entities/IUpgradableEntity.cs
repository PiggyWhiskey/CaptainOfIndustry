// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.IUpgradableEntity
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Entities.Static;
using Mafi.Localization;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Entities
{
  /// <summary>Represents an entity that can be upgraded.</summary>
  public interface IUpgradableEntity : 
    IStaticEntity,
    IEntityWithPosition,
    IRenderedEntity,
    IEntity,
    IIsSafeAsHashKey,
    IAreaSelectableEntity
  {
    IUpgrader Upgrader { get; }

    /// <summary>
    /// Implements custom upgrade verification for entity. To test whether this entity is upgradable use
    /// <see cref="M:Mafi.Core.Entities.IUpgrader.IsUpgradeAvailable(Mafi.Localization.LocStrFormatted@)" />
    /// </summary>
    bool IsUpgradeAvailable(out LocStrFormatted errorMessage);

    /// <summary>Upgrades itself.</summary>
    void UpgradeSelf();
  }
}
