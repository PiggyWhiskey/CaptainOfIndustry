// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.Layout.Upgrade.LayoutEntityUpgraderFactory
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;

#nullable disable
namespace Mafi.Core.Entities.Static.Layout.Upgrade
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public class LayoutEntityUpgraderFactory : ILayoutEntityUpgraderFactory
  {
    private readonly UnlockedProtosDb m_unlockedProtosDb;
    private readonly TerrainOccupancyManager m_occupancyManager;
    private readonly DependencyResolver m_dependencyResolver;
    private readonly UpgradeCostResolver m_upgradeCostResolver;
    private readonly LayoutEntityAddRequestFactory m_addRequestFactory;

    public EntityId.Factory EntityIdFactory { get; private set; }

    public LayoutEntityUpgraderFactory(
      UnlockedProtosDb unlockedProtosDb,
      TerrainOccupancyManager occupancyManager,
      DependencyResolver dependencyResolver,
      UpgradeCostResolver upgradeCostResolver,
      EntityId.Factory idFactory,
      LayoutEntityAddRequestFactory addRequestFactory)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_unlockedProtosDb = unlockedProtosDb;
      this.m_occupancyManager = occupancyManager;
      this.m_dependencyResolver = dependencyResolver;
      this.m_upgradeCostResolver = upgradeCostResolver;
      this.m_addRequestFactory = addRequestFactory;
      this.EntityIdFactory = idFactory;
    }

    IUpgrader ILayoutEntityUpgraderFactory.CreateInstance<TProto, TEntity>(
      TEntity entity,
      TProto entityProto)
    {
      return (IUpgrader) new LayoutEntityUpgrader<TProto, TEntity>(this.m_unlockedProtosDb, this.m_occupancyManager, this.m_dependencyResolver, this.m_upgradeCostResolver, this.m_addRequestFactory, entity, entityProto);
    }
  }
}
