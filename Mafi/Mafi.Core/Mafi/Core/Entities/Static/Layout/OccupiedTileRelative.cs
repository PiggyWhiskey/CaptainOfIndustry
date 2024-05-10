// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.Layout.OccupiedTileRelative
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Terrain;
using Mafi.Utils;

#nullable disable
namespace Mafi.Core.Entities.Static.Layout
{
  [ExpectedStructSize(14)]
  public readonly struct OccupiedTileRelative
  {
    public readonly short RelativeX;
    public readonly short RelativeY;
    public readonly ushort ConstraintSlim;
    public readonly short RelativeFrom;
    public readonly ushort VerticalSizeRaw;
    public readonly short TileSurfaceRelHeightRaw;
    public readonly TileSurfaceSlimId TileSurface;
    private readonly byte m_unused;

    public RelTile2i RelCoord => new RelTile2i((int) this.RelativeX, (int) this.RelativeY);

    public ThicknessTilesI FromHeightRel => new ThicknessTilesI((int) this.RelativeFrom);

    public ThicknessTilesI ToHeightRelExcl
    {
      get => new ThicknessTilesI((int) this.RelativeFrom + (int) this.VerticalSizeRaw);
    }

    public ThicknessTilesI VerticalSize => new ThicknessTilesI((int) this.VerticalSizeRaw);

    public LayoutTileConstraint Constraint => (LayoutTileConstraint) this.ConstraintSlim;

    public ThicknessTilesI TileSurfaceRelHeight
    {
      get => new ThicknessTilesI((int) this.TileSurfaceRelHeightRaw);
    }

    public OccupiedTileRelative(
      short relativeX,
      short relativeY,
      short relativeFrom,
      ushort verticalSizeRaw,
      ushort constraintSlim,
      TileSurfaceSlimId tileSurface,
      short tileSurfaceRelHeightRaw)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.RelativeX = relativeX;
      this.RelativeY = relativeY;
      this.RelativeFrom = relativeFrom;
      this.VerticalSizeRaw = verticalSizeRaw;
      this.ConstraintSlim = constraintSlim;
      this.TileSurface = tileSurface;
      this.TileSurfaceRelHeightRaw = tileSurfaceRelHeightRaw;
      this.m_unused = (byte) 0;
    }

    public OccupiedTileRelative(
      RelTile2i relTile,
      ThicknessTilesI relativeFrom,
      ThicknessTilesI verticalSize,
      LayoutTileConstraint constraint,
      TileSurfaceSlimId tileSurface,
      ThicknessTilesI tileSurfaceRelHeight)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this = new OccupiedTileRelative((short) relTile.X, (short) relTile.Y, (short) relativeFrom.Value, (ushort) verticalSize.Value, (ushort) (byte) constraint, tileSurface, (short) tileSurfaceRelHeight.Value);
    }

    public OccupiedTileRelative(
      RelTile2i relTile,
      ThicknessTilesI relativeFrom,
      ThicknessTilesI verticalSize,
      LayoutTileConstraint constraint)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this = new OccupiedTileRelative((short) relTile.X, (short) relTile.Y, (short) relativeFrom.Value, (ushort) verticalSize.Value, (ushort) (byte) constraint, TileSurfaceSlimId.PhantomId, (short) 0);
    }

    public override string ToString()
    {
      return string.Format("{0} {1}+{2} ({3})", (object) this.RelCoord, (object) this.FromHeightRel, (object) this.VerticalSizeRaw, (object) this.Constraint);
    }
  }
}
