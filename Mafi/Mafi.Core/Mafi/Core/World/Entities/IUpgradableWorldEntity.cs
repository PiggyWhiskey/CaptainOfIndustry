// Decompiled with JetBrains decompiler
// Type: Mafi.Core.World.Entities.IUpgradableWorldEntity
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Localization;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.World.Entities
{
  public interface IUpgradableWorldEntity : IWorldMapEntity, IEntity, IIsSafeAsHashKey
  {
    void StartUpgrade();

    bool IsUpgradeAvailable(out LocStrFormatted error);

    bool IsUpgradeVisible();

    AssetValue PriceToUpgrade { get; }

    /// <summary>Title of the upgrade to be shown in the UI.</summary>
    LocStrFormatted UpgradeTitle { get; }

    /// <summary>Whether upgrade exists.</summary>
    bool UpgradeExists { get; }

    /// <summary>Icon of the upgraded item.</summary>
    string UpgradeIcon { get; }
  }
}
