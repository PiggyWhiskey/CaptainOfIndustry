// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Designation.TerrainMiningManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Buildings.Mine;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Products;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Terrain.Designation
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public class TerrainMiningManager : ITerrainMiningManager
  {
    private readonly ITerrainDesignationsManager m_terrainDesignationManager;
    private readonly Dict<Tile2i, TerrainDesignation> m_miningDesignations;
    private readonly UnreachableTerrainDesignationsManager m_unreachableDesignationsManager;

    public IEnumerable<TerrainDesignation> MiningDesignations
    {
      get => (IEnumerable<TerrainDesignation>) this.m_miningDesignations.Values;
    }

    public int MiningDesignationsCount => this.m_miningDesignations.Count;

    public TerrainMiningManager(
      ITerrainDesignationsManager terrainDesignationManager,
      UnreachableTerrainDesignationsManager unreachableDesignationsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_miningDesignations = new Dict<Tile2i, TerrainDesignation>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_terrainDesignationManager = terrainDesignationManager;
      this.m_unreachableDesignationsManager = unreachableDesignationsManager;
      foreach (TerrainDesignation designation in (IEnumerable<TerrainDesignation>) terrainDesignationManager.Designations)
        this.designationAdded(designation);
      terrainDesignationManager.DesignationAdded.AddNonSaveable<TerrainMiningManager>(this, new Action<TerrainDesignation>(this.designationAdded));
      terrainDesignationManager.DesignationRemoved.AddNonSaveable<TerrainMiningManager>(this, new Action<TerrainDesignation>(this.designationRemoved));
    }

    private void designationAdded(TerrainDesignation d)
    {
      if (!(d.ProtoId == IdsCore.TerrainDesignators.MiningDesignator) && !(d.ProtoId == IdsCore.TerrainDesignators.LevelDesignator))
        return;
      this.m_miningDesignations.Add(d.OriginTileCoord, d);
    }

    private void designationRemoved(TerrainDesignation d)
    {
      if (!(d.ProtoId == IdsCore.TerrainDesignators.MiningDesignator) && !(d.ProtoId == IdsCore.TerrainDesignators.LevelDesignator))
        return;
      this.m_miningDesignations.Remove(d.OriginTileCoord);
    }

    public Option<TerrainDesignation> GetMiningDesignationAt(Tile2i coord)
    {
      TerrainDesignation terrainDesignation;
      return !this.m_miningDesignations.TryGetValue(TerrainDesignation.GetOrigin(coord), out terrainDesignation) ? Option<TerrainDesignation>.None : Option<TerrainDesignation>.Create(terrainDesignation);
    }

    public bool TryFindClosestReadyToMine(
      MineTower tower,
      Tile2i position,
      Vehicle servicingVehicle,
      out TerrainDesignation bestDesignation,
      Option<LooseProductProto> productToPrefer = default (Option<LooseProductProto>),
      bool tryIgnoreReservations = false,
      Predicate<TerrainDesignation> predicate = null,
      Lyst<TerrainDesignation> additionalNearbyDesignations = null)
    {
      IReadOnlySet<IDesignation> unreachableSet = this.m_unreachableDesignationsManager.GetUnreachableDesignationsFor((IPathFindingVehicle) servicingVehicle);
      Predicate<TerrainDesignation> predicate1 = (Predicate<TerrainDesignation>) (d =>
      {
        if (!(d.ProtoId == IdsCore.TerrainDesignators.MiningDesignator) && !(d.ProtoId == IdsCore.TerrainDesignators.LevelDesignator) || unreachableSet.Contains((IDesignation) d) || !d.IsMiningNotFulfilled)
          return false;
        return predicate == null || predicate(d);
      });
      TerrainDesignation bestDesignation1;
      if (this.m_terrainDesignationManager.TryFindBestReadyToFulfill((IEnumerable<TerrainDesignation>) tower.ManagedDesignations, position, servicingVehicle, out bestDesignation1, productToPrefer, tryIgnoreReservations, predicate1, additionalNearbyDesignations, true, true))
      {
        bestDesignation = bestDesignation1;
        return true;
      }
      bestDesignation = (TerrainDesignation) null;
      return false;
    }
  }
}
