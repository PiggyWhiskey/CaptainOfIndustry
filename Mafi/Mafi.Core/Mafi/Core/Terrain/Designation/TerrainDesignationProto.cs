// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Designation.TerrainDesignationProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using System;

#nullable disable
namespace Mafi.Core.Terrain.Designation
{
  public class TerrainDesignationProto : Proto
  {
    public readonly bool PreferInitialBelowTerrain;
    public readonly Func<ITerrainDesignationsManager, Tile2iAndIndex, HeightTilesF, bool, bool> IsFulfilledFn;
    /// <summary>Fulfillment function for mining specifically.</summary>
    public readonly Option<Func<ITerrainDesignationsManager, Tile2iAndIndex, HeightTilesF, bool, bool>> IsFulfilledMiningFn;
    /// <summary>Fulfillment function for dumping specifically.</summary>
    public readonly Option<Func<ITerrainDesignationsManager, Tile2iAndIndex, HeightTilesF, bool, bool>> IsFulfilledDumpingFn;
    public readonly bool CanOverflowReservations;
    public readonly int MaxAssignedEntities;
    public readonly bool DisplayWarningWhenNotOwned;
    public readonly LocStr WarningWhenNotOwned;
    public readonly bool ShouldUpdateTowerNotificationOnFulfilledChanged;
    /// <summary>Whether this designation is used for mining/dumping.</summary>
    public readonly bool IsTerraforming;
    public readonly TerrainDesignationProto.Gfx Graphics;

    public TerrainDesignationProto(
      Proto.ID id,
      Proto.Str strings,
      Func<ITerrainDesignationsManager, Tile2iAndIndex, HeightTilesF, bool, bool> isFulfilledFn,
      Option<Func<ITerrainDesignationsManager, Tile2iAndIndex, HeightTilesF, bool, bool>> isFulfilledMiningFn,
      Option<Func<ITerrainDesignationsManager, Tile2iAndIndex, HeightTilesF, bool, bool>> isFulfilledDumpingFn,
      bool preferInitialBelowTerrain,
      int maxAssignedEntities,
      bool displayWarningWhenNotOwned,
      LocStr warningWhenNotOwned,
      bool isTerraforming,
      bool shouldUpdateTowerNotificationOnFulfilledChanged,
      TerrainDesignationProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings);
      this.IsFulfilledFn = isFulfilledFn;
      this.IsFulfilledMiningFn = isFulfilledMiningFn;
      this.IsFulfilledDumpingFn = isFulfilledDumpingFn;
      this.PreferInitialBelowTerrain = preferInitialBelowTerrain;
      this.MaxAssignedEntities = maxAssignedEntities;
      this.DisplayWarningWhenNotOwned = displayWarningWhenNotOwned;
      this.WarningWhenNotOwned = warningWhenNotOwned;
      this.CanOverflowReservations = this.MaxAssignedEntities > 1;
      this.IsTerraforming = isTerraforming;
      this.ShouldUpdateTowerNotificationOnFulfilledChanged = shouldUpdateTowerNotificationOnFulfilledChanged;
      this.Graphics = graphics.CheckNotNull<TerrainDesignationProto.Gfx>();
    }

    public new class Gfx : Proto.Gfx
    {
      public static readonly TerrainDesignationProto.Gfx Empty;
      public readonly ColorRgba ColorCanBeFulfilled;
      public readonly ColorRgba ColorCanNotBeFulfilled;
      public readonly ColorRgba ColorIsFulfilled;
      public readonly ColorRgba ColorRemove;

      public Gfx(
        ColorRgba colorCanBeFulfilled,
        ColorRgba colorCanNotBeFulfilled,
        ColorRgba colorIsFulfilled,
        ColorRgba colorRemove)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.ColorCanBeFulfilled = colorCanBeFulfilled;
        this.ColorCanNotBeFulfilled = colorCanNotBeFulfilled;
        this.ColorIsFulfilled = colorIsFulfilled;
        this.ColorRemove = colorRemove;
      }

      static Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        TerrainDesignationProto.Gfx.Empty = new TerrainDesignationProto.Gfx(ColorRgba.Empty, ColorRgba.Empty, ColorRgba.Empty, ColorRgba.Empty);
      }
    }
  }
}
