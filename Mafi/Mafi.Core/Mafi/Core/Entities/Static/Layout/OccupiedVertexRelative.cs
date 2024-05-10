// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.Layout.OccupiedVertexRelative
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Products;
using Mafi.Utils;
using System;

#nullable disable
namespace Mafi.Core.Entities.Static.Layout
{
  [ExpectedStructSize(24)]
  public readonly struct OccupiedVertexRelative
  {
    public readonly short RelativeX;
    public readonly short RelativeY;
    private readonly ushort m_constraintSlim;
    public readonly short RelativeFromRaw;
    public readonly ushort VerticalSizeRaw;
    public readonly ThicknessTilesISemiSlimOption TerrainHeightSlim;
    public readonly ThicknessTilesISemiSlimOption TerrainHeightAfterDeconstructionSlim;
    public readonly short MinTerrainHeightOrMinValueRaw;
    public readonly short MaxTerrainHeightOrMaxValueRaw;
    /// <summary>
    /// Index of occupied tile. If there are more than one tile touching this vertex, it will be the lowest.
    /// </summary>
    public readonly ushort LowestTileIndex;
    private readonly short m_unusedA;
    public readonly TerrainMaterialSlimIdOption TerrainMaterial;
    private readonly byte m_unusedB;

    public RelTile2i RelCoord => new RelTile2i((int) this.RelativeX, (int) this.RelativeY);

    public ThicknessTilesI FromHeightRel => new ThicknessTilesI((int) this.RelativeFromRaw);

    public ThicknessTilesI ToHeightRelExcl
    {
      get => new ThicknessTilesI((int) this.RelativeFromRaw + (int) this.VerticalSizeRaw);
    }

    public ThicknessTilesI VerticalSize => new ThicknessTilesI((int) this.VerticalSizeRaw);

    public LayoutTileConstraint Constraint => (LayoutTileConstraint) this.m_constraintSlim;

    public OccupiedVertexRelative(
      short relativeX,
      short relativeY,
      ushort constraintSlim,
      short relativeFromRaw,
      ushort verticalSizeRaw,
      TerrainMaterialSlimIdOption terrainMaterial,
      ThicknessTilesISemiSlimOption terrainHeightSlim,
      ThicknessTilesISemiSlimOption terrainHeightAfterDeconstructionSlim,
      short minTerrainHeightOrMinValueRaw,
      short maxTerrainHeightOrMaxValueRaw,
      ushort lowestTileIndex)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.RelativeX = relativeX;
      this.RelativeY = relativeY;
      this.m_constraintSlim = constraintSlim;
      this.RelativeFromRaw = relativeFromRaw;
      this.VerticalSizeRaw = verticalSizeRaw;
      this.TerrainMaterial = terrainMaterial;
      this.TerrainHeightSlim = terrainHeightSlim;
      this.TerrainHeightAfterDeconstructionSlim = terrainHeightAfterDeconstructionSlim;
      this.MinTerrainHeightOrMinValueRaw = minTerrainHeightOrMinValueRaw;
      this.MaxTerrainHeightOrMaxValueRaw = maxTerrainHeightOrMaxValueRaw;
      this.LowestTileIndex = lowestTileIndex;
      this.m_unusedA = (short) 0;
      this.m_unusedB = (byte) 0;
    }

    public OccupiedVertexRelative(
      RelTile2i relTile,
      ThicknessTilesI relativeFrom,
      ThicknessTilesI verticalSize,
      LayoutTileConstraint constraint,
      TerrainMaterialSlimIdOption terrainMaterial,
      ThicknessTilesI? terrainHeight,
      ThicknessTilesI? terrainHeightAfterDeconstruction,
      ThicknessTilesI? minTerrainHeight,
      ThicknessTilesI? maxTerrainHeight,
      int lowestTileIndex)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      int x = (int) (short) relTile.X;
      int y = (int) (short) relTile.Y;
      int constraintSlim = (int) (ushort) constraint;
      int relativeFromRaw = (int) (short) relativeFrom.Value;
      int verticalSizeRaw = (int) (ushort) verticalSize.Value;
      TerrainMaterialSlimIdOption terrainMaterial1 = terrainMaterial;
      ThicknessTilesI valueOrDefault;
      ThicknessTilesISemiSlim? nullable1;
      if (!terrainHeight.HasValue)
      {
        nullable1 = new ThicknessTilesISemiSlim?();
      }
      else
      {
        valueOrDefault = terrainHeight.GetValueOrDefault();
        nullable1 = new ThicknessTilesISemiSlim?(valueOrDefault.AsSemiSlim);
      }
      ThicknessTilesISemiSlim? nullable2 = nullable1;
      ThicknessTilesISemiSlimOption terrainHeightSlim = nullable2.HasValue ? (ThicknessTilesISemiSlimOption) nullable2.GetValueOrDefault() : ThicknessTilesISemiSlimOption.None;
      ThicknessTilesISemiSlim? nullable3;
      if (!terrainHeightAfterDeconstruction.HasValue)
      {
        nullable3 = new ThicknessTilesISemiSlim?();
      }
      else
      {
        valueOrDefault = terrainHeightAfterDeconstruction.GetValueOrDefault();
        nullable3 = new ThicknessTilesISemiSlim?(valueOrDefault.AsSemiSlim);
      }
      nullable2 = nullable3;
      ThicknessTilesISemiSlimOption terrainHeightAfterDeconstructionSlim = nullable2.HasValue ? (ThicknessTilesISemiSlimOption) nullable2.GetValueOrDefault() : ThicknessTilesISemiSlimOption.None;
      int minTerrainHeightOrMinValueRaw = minTerrainHeight.HasValue ? (int) (short) minTerrainHeight.Value.Value : (int) short.MinValue;
      int maxTerrainHeightOrMaxValueRaw = maxTerrainHeight.HasValue ? (int) (short) maxTerrainHeight.Value.Value : (int) short.MaxValue;
      int lowestTileIndex1 = (int) (ushort) lowestTileIndex;
      this = new OccupiedVertexRelative((short) x, (short) y, (ushort) constraintSlim, (short) relativeFromRaw, (ushort) verticalSizeRaw, terrainMaterial1, terrainHeightSlim, terrainHeightAfterDeconstructionSlim, (short) minTerrainHeightOrMinValueRaw, (short) maxTerrainHeightOrMaxValueRaw, (ushort) lowestTileIndex1);
    }

    public OccupiedVertexRelative MergeWith(OccupiedVertexRelative other)
    {
      short relativeFromRaw = Math.Min(this.RelativeFromRaw, other.RelativeFromRaw);
      int num = Math.Max((int) this.RelativeFromRaw + (int) this.VerticalSizeRaw, (int) other.RelativeFromRaw + (int) other.VerticalSizeRaw);
      return new OccupiedVertexRelative(this.RelativeX, this.RelativeY, (ushort) ((uint) this.m_constraintSlim | (uint) other.m_constraintSlim), relativeFromRaw, (ushort) (byte) ((uint) num - (uint) relativeFromRaw), this.TerrainMaterial.HasValue ? this.TerrainMaterial : other.TerrainMaterial, new ThicknessTilesISemiSlimOption(Math.Max(this.TerrainHeightSlim.RawValue, other.TerrainHeightSlim.RawValue)), new ThicknessTilesISemiSlimOption(Math.Max(this.TerrainHeightAfterDeconstructionSlim.RawValue, other.TerrainHeightAfterDeconstructionSlim.RawValue)), Math.Max(this.MinTerrainHeightOrMinValueRaw, other.MinTerrainHeightOrMinValueRaw), Math.Min(this.MaxTerrainHeightOrMaxValueRaw, other.MaxTerrainHeightOrMaxValueRaw), Math.Min(this.LowestTileIndex, other.LowestTileIndex));
    }

    /// <summary>
    /// Chooses the less strict min/max terrain height constraints while merging.
    /// </summary>
    public OccupiedVertexRelative MergeWithRelaxedHeightConstraints(OccupiedVertexRelative other)
    {
      short relativeFromRaw = Math.Min(this.RelativeFromRaw, other.RelativeFromRaw);
      int num = Math.Max((int) this.RelativeFromRaw + (int) this.VerticalSizeRaw, (int) other.RelativeFromRaw + (int) other.VerticalSizeRaw);
      return new OccupiedVertexRelative(this.RelativeX, this.RelativeY, (ushort) ((uint) this.m_constraintSlim | (uint) other.m_constraintSlim), relativeFromRaw, (ushort) (byte) ((uint) num - (uint) relativeFromRaw), this.TerrainMaterial.HasValue ? this.TerrainMaterial : other.TerrainMaterial, new ThicknessTilesISemiSlimOption(Math.Max(this.TerrainHeightSlim.RawValue, other.TerrainHeightSlim.RawValue)), new ThicknessTilesISemiSlimOption(Math.Max(this.TerrainHeightAfterDeconstructionSlim.RawValue, other.TerrainHeightAfterDeconstructionSlim.RawValue)), Math.Min(this.MinTerrainHeightOrMinValueRaw, other.MinTerrainHeightOrMinValueRaw), Math.Max(this.MaxTerrainHeightOrMaxValueRaw, other.MaxTerrainHeightOrMaxValueRaw), Math.Min(this.LowestTileIndex, other.LowestTileIndex));
    }

    public override string ToString()
    {
      return string.Format("{0} {1}+{2} ({3})", (object) this.RelCoord, (object) this.RelativeFromRaw, (object) this.VerticalSizeRaw, (object) this.Constraint);
    }
  }
}
