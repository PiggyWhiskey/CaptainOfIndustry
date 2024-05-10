// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.Layout.EntityLayout
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.Terrain;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Entities.Static.Layout
{
  /// <summary>
  /// Entity layout represented as [relative x, relative y, thickness]. Coordinate (0, 0) is the origin of the entity
  /// and is always at the low-xy corner of the entity.
  /// </summary>
  public class EntityLayout
  {
    /// <summary>Port selector matching all compatible ports.</summary>
    public const string ANY_COMPATIBLE_PORT = "*";
    public static readonly HeightTilesF VEHICLE_INACCESSIBLE_HEIGHT;
    public static readonly ThicknessTilesF VEHICLE_INACCESSIBLE_HEIGHT_REL;
    /// <summary>
    /// Original layout string. Can be empty if not available. Never null.
    /// </summary>
    public readonly string SourceLayoutStr;
    /// <summary>
    /// Data about each individual tile of this layout. Mainly their height and constraints. Tiles are ordered in the
    /// order given to ctor, which should be row-major order.
    /// </summary>
    public readonly ImmutableArray<LayoutTile> LayoutTiles;
    /// <summary>
    /// Terrain vertices of this layout. Note that every terrain tile has four vertices but neighboring tiles will
    /// share vertices. Each vertex that touches at least one tile is in this array.
    /// </summary>
    public readonly ImmutableArray<TerrainVertexRel> TerrainVertices;
    /// <summary>
    /// Height of vehicle-accessible tiles and their neighbors. Unlike <see cref="F:Mafi.Core.Entities.Static.Layout.EntityLayout.LayoutTiles" /> these values are
    /// "vertices of tiles", so for every navigable tile (x, y), height will also exist at (x + 1, y), (x, y + 1),
    /// and (x + 1, y + 1).
    /// </summary>
    public readonly ImmutableArray<KeyValuePair<RelTile2i, ThicknessTilesF>> VehicleSurfaceHeights;
    /// <summary>I/O ports on this entity.</summary>
    public readonly ImmutableArray<IoPortTemplate> Ports;
    /// <summary>
    /// Extra parameters that are needed for layout construction or validation.
    /// </summary>
    public readonly EntityLayoutParams LayoutParams;
    public readonly ThicknessTilesI LayoutMinHeight;
    /// <summary>
    /// Occupied size of this layout [size x, size y, height].
    /// </summary>
    public readonly RelTile3i LayoutSize;
    /// <summary>Custom origin tile (in layout coord system).</summary>
    public readonly RelTile2f? OriginTile;
    /// <summary>
    /// Origin of the core part of this layout. Core part is the part considered for center computation. This is in
    /// relative coordinates of the layout.
    /// </summary>
    public readonly RelTile2i CoreMin;
    /// <summary>Max coordinate of the core layout part.</summary>
    public readonly RelTile2i CoreMax;
    /// <summary>Total number of occupied 3D tiles.</summary>
    public readonly int TilesCount;
    /// <summary>
    /// Combined constraint of all tiles. Each flag bit is set if there is at least one tile with that constraint.
    /// </summary>
    public readonly LayoutTileConstraint CombinedConstraint;
    /// <summary>
    /// Entity will collapse if the number of vertices violating height constraints IS GREATER than this threshold.
    /// </summary>
    public readonly int CollapseVerticesThreshold;
    /// <summary>
    /// Valid placement height range determined by <see cref="F:Mafi.Core.Entities.Static.Layout.LayoutTile.MinTerrainHeight" />
    /// and <see cref="F:Mafi.Core.Entities.Static.Layout.LayoutTile.MaxTerrainHeight" />.
    /// </summary>
    public readonly ThicknessIRange PlacementHeightRange;

    /// <summary>Size of the layout core.</summary>
    public RelTile2i CoreSize => this.CoreMax - this.CoreMin + RelTile2i.One;

    public EntityLayout(
      string sourceLayoutStr,
      ImmutableArray<LayoutTile> tiles,
      ImmutableArray<TerrainVertexRel> vertices,
      ImmutableArray<IoPortTemplate> ports,
      EntityLayoutParams layoutParams,
      int collapseVerticesThreshold,
      RelTile2f? originTile = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Assert.That<ImmutableArray<LayoutTile>>(tiles).IsNotEmpty<LayoutTile>("Constructing empty layout.");
      Assert.That<ImmutableArray<TerrainVertexRel>>(vertices).IsNotEmpty<TerrainVertexRel>("Constructing empty layout.");
      this.SourceLayoutStr = sourceLayoutStr.CheckNotNull<string>();
      this.LayoutTiles = tiles.CheckNotDefaultStruct<ImmutableArray<LayoutTile>>();
      this.TerrainVertices = vertices.CheckNotDefaultStruct<ImmutableArray<TerrainVertexRel>>();
      this.VehicleSurfaceHeights = vertices.Filter((Predicate<TerrainVertexRel>) (x => x.VehicleSurfaceRelHeight.HasValue)).Map<KeyValuePair<RelTile2i, ThicknessTilesF>>((Func<TerrainVertexRel, KeyValuePair<RelTile2i, ThicknessTilesF>>) (x => Make.Kvp<RelTile2i, ThicknessTilesF>(x.Coord, x.VehicleSurfaceRelHeight.Value)));
      this.Ports = ports.CheckNotDefaultStruct<ImmutableArray<IoPortTemplate>>();
      this.OriginTile = originTile;
      this.LayoutParams = layoutParams;
      this.CollapseVerticesThreshold = collapseVerticesThreshold;
      this.CombinedConstraint = LayoutTileConstraint.None;
      this.TilesCount = 0;
      this.LayoutMinHeight = tiles.First.OccupiedThickness.From;
      this.LayoutSize = RelTile3i.Zero;
      foreach (LayoutTile tile in tiles)
      {
        Assert.That<ThicknessTilesI>(tile.OccupiedThickness.From).IsLess(tile.OccupiedThickness.To);
        this.LayoutSize = this.LayoutSize.Max(tile.Coord.ExtendZ(tile.OccupiedThickness.To.Value));
        this.LayoutMinHeight = this.LayoutMinHeight.Min(tile.OccupiedThickness.From);
        this.TilesCount += tile.OccupiedThickness.To.Value - tile.OccupiedThickness.From.Value;
        this.CombinedConstraint |= tile.Constraint;
      }
      this.LayoutSize += RelTile2i.One;
      if (layoutParams.CustomPlacementRange.HasValue)
      {
        this.PlacementHeightRange = layoutParams.CustomPlacementRange.Value;
      }
      else
      {
        int self1 = int.MinValue;
        int self2 = int.MaxValue;
        foreach (TerrainVertexRel terrainVertex in this.TerrainVertices)
        {
          if (terrainVertex.MinTerrainHeight.HasValue)
            self1 = self1.Max(terrainVertex.MinTerrainHeight.Value.Value);
          if (terrainVertex.MaxTerrainHeight.HasValue)
            self2 = self2.Min(terrainVertex.MaxTerrainHeight.Value.Value);
        }
        if (self1 == int.MinValue || self2 == int.MaxValue || self2 <= self1)
        {
          this.PlacementHeightRange = new ThicknessIRange(0, 0);
        }
        else
        {
          if (self1 > 0)
          {
            Log.Error(string.Format("Max value of all terrain min heights {0} is above 0", (object) self1));
            self1 = 0;
          }
          if (self2 < 0)
          {
            Log.Error(string.Format("Min value of all terrain max heights {0} is below 0", (object) self2));
            self2 = 0;
          }
          this.PlacementHeightRange = new ThicknessIRange(-self2, -self1);
          Assert.That<ThicknessTilesI>(this.PlacementHeightRange.Height).IsNotNegative();
        }
      }
      if (layoutParams.IgnoreTilesForCore.IsNone)
      {
        this.CoreMin = RelTile2i.Zero;
        this.CoreMax = this.LayoutSize.Xy - RelTile2i.One;
      }
      else
      {
        Predicate<LayoutTile> predicate = layoutParams.IgnoreTilesForCore.Value;
        RelTile2i relTile2i1 = this.LayoutSize.Xy - RelTile2i.One;
        RelTile2i relTile2i2 = RelTile2i.Zero;
        foreach (LayoutTile tile in tiles)
        {
          if (!predicate(tile))
          {
            relTile2i1 = relTile2i1.Min(tile.Coord);
            relTile2i2 = relTile2i2.Max(tile.Coord);
          }
        }
        if (relTile2i2.X < relTile2i1.X || relTile2i2.Y < relTile2i1.Y)
        {
          Log.Error("Invalid (empty) core of layout.");
          this.CoreMin = RelTile2i.Zero;
          this.CoreMax = this.LayoutSize.Xy - RelTile2i.One;
        }
        else
        {
          this.CoreMin = relTile2i1;
          this.CoreMax = relTile2i2;
        }
      }
    }

    public bool HasTilesWithConstraint(LayoutTileConstraint constraint)
    {
      return (this.CombinedConstraint & constraint) != 0;
    }

    public Tile3f GetModelOrigin(TileTransform transform)
    {
      return !this.OriginTile.HasValue ? this.GetCenter(transform) : this.TransformF_Point(this.OriginTile.Value, transform).ExtendHeight(transform.Position.Height.HeightTilesF);
    }

    /// <summary>
    /// Exact center and center of rotation. All 3D models are placed on this coordinate relative to layout origin.
    /// </summary>
    public Tile3f GetCenter(TileTransform transform)
    {
      Tile3i tile3i = this.Transform(this.CoreMin.ExtendZ(0), transform);
      Tile2i tile2i = this.Transform(this.CoreMax, transform);
      return new Tile3f((tile3i.X + tile2i.X + 1).Over(2), (tile3i.Y + tile2i.Y + 1).Over(2), (Fix32) tile3i.Z);
    }

    /// <summary>
    /// Returns an array of ports matching given criteria. Throws an exception if no port is matched.
    /// </summary>
    public Lyst<IoPortTemplate> ResolvePortSelectorOrThrow(
      string portSelector,
      IoPortType type,
      ProductType productType)
    {
      if (string.IsNullOrEmpty(portSelector))
        throw new ProtoBuilderException(string.Format("Port selector for product '{0}' ({1}) is null or empty. Layout:\n{2}", (object) productType, (object) type, (object) this.SourceLayoutStr));
      if (portSelector != "*" && portSelector.Contains("*"))
        throw new ProtoBuilderException(string.Format("Port selector '{0}' for product '{1}' ({2}) has illegal format. ", (object) portSelector, (object) productType, (object) type) + "Layout:\n" + this.SourceLayoutStr);
      Lyst<IoPortTemplate> lyst = new Lyst<IoPortTemplate>();
      if (portSelector == "*")
      {
        lyst.AddRange(this.Ports.Where((Func<IoPortTemplate, bool>) (x => x.Type == type && x.Shape.AllowedProductType == productType)));
      }
      else
      {
        foreach (char ch in portSelector)
        {
          foreach (IoPortTemplate port in this.Ports)
          {
            if ((int) port.Name == (int) ch)
            {
              if (port.Shape.AllowedProductType != productType)
                throw new ProtoBuilderException(string.Format("Selected port '{0}' product type '{1}' is ", (object) portSelector, (object) port.Shape.AllowedProductType) + string.Format("not compatible with requested type '{0}'. Layout:\n{1}", (object) productType, (object) this.SourceLayoutStr));
              lyst.Add(port);
            }
          }
        }
      }
      if (!lyst.IsEmpty)
        return lyst;
      if (portSelector == "*")
        throw new ProtoBuilderException(string.Format("Failed to find '{0}' port that matches product type '{1}'. ", (object) type, (object) productType) + "Layout:\n" + this.SourceLayoutStr);
      throw new ProtoBuilderException(string.Format("Port selector '{0}' doesn't match any '{1}' port for product type ", (object) portSelector, (object) type) + string.Format("'{0}'. Layout:\n{1}", (object) productType, (object) this.SourceLayoutStr));
    }

    /// <summary>
    /// Transforms given relative tile coordinate from this layout space to absolute coordinate based on given
    /// <paramref name="transform" />.
    /// </summary>
    public Tile2i Transform(RelTile2i relTile, TileTransform transform)
    {
      return this.TransformRelative(relTile, transform) + transform.Position.Xy;
    }

    /// <summary>
    /// Transforms a layout point (not a tile) relative to the true center and returns absolute position.
    /// </summary>
    public Tile2f TransformF_Point(RelTile2f relTile, TileTransform transform)
    {
      RelTile2f coreOffsetF = this.GetCoreOffsetF();
      RelTile2f relTile2f1 = relTile - coreOffsetF;
      RelTile2f relTile2f2 = new RelTile2f(transform.TransformMatrix.Transform(relTile2f1.Vector2f));
      Vector2f absValue = transform.TransformMatrix.Transform((coreOffsetF - coreOffsetF.RelTile2fFloored).Vector2f).AbsValue;
      return relTile2f2 + transform.Position.Xy.CornerTile2f + new RelTile2f(absValue);
    }

    /// <summary>
    /// Transforms a 2D point (not a tile) relative to the true center and returns absolute position.
    /// </summary>
    public Tile2f TransformF_PointRelToCore(RelTile2f relTile, TileTransform transform)
    {
      RelTile2f relTile2f = new RelTile2f(transform.TransformMatrix.Transform(relTile.Vector2f));
      RelTile2f coreOffsetF = this.GetCoreOffsetF();
      Vector2f absValue = transform.TransformMatrix.Transform((coreOffsetF - coreOffsetF.RelTile2fFloored).Vector2f).AbsValue;
      return relTile2f + transform.Position.Xy.CornerTile2f + new RelTile2f(absValue);
    }

    public Tile3i TransformF_PointRelToCenter(RelTile3i relTile, TileTransform transform)
    {
      RelTile2f relTile2f = new RelTile2f(transform.TransformMatrix.Transform(relTile.Xy.Vector2f));
      this.GetCoreOffsetF();
      return relTile2f.RelTile2iFloored.ExtendZ(relTile.Z) + transform.Position;
    }

    public AngleDegrees1f Transform(AngleDegrees1f angle, TileTransform transform)
    {
      return (angle + transform.Rotation.Angle).Normalized;
    }

    /// <summary>
    /// Transforms given relative tile coordinate from this layout space to absolute coordinate based on given
    /// <paramref name="transform" />.
    /// </summary>
    public Tile3i Transform(RelTile3i relTile, TileTransform transform)
    {
      return this.TransformRelative(relTile.Xy, transform).ExtendZ(relTile.Z) + transform.Position;
    }

    public Tile3f TransformF(RelTile3f relTile, TileTransform transform)
    {
      return this.TransformRelativeF(relTile.Xy, transform).ExtendZ(relTile.Z) + transform.Position.CornerTile3f;
    }

    public Tile3f TransformF_Fixed(RelTile3f relTile, TileTransform transform)
    {
      return this.TransformRelativeF_Point(relTile.Xy, transform).ExtendZ(relTile.Z) + transform.Position.CornerTile3f;
    }

    /// <summary>
    /// Center of rotation when rotating tiles but keep in mind that the entity is not placed/aligned on this origin.
    /// </summary>
    public RelTile2f GetCoreOffset()
    {
      RelTile2i relTile2i = this.CoreMin;
      RelTile2f relTile2f = relTile2i.RelTile2f;
      relTile2i = this.CoreMax - this.CoreMin;
      RelTile2f halfFast = relTile2i.RelTile2f.HalfFast;
      return relTile2f + halfFast;
    }

    /// <summary>
    /// True center of rotation but keep in mind that the entity is not placed/aligned on this origin.
    /// </summary>
    public RelTile2f GetCoreOffsetF()
    {
      RelTile2i relTile2i = this.CoreMin;
      RelTile2f relTile2f = relTile2i.RelTile2f;
      relTile2i = this.CoreMax - this.CoreMin + 1;
      RelTile2f halfFast = relTile2i.RelTile2f.HalfFast;
      return relTile2f + halfFast;
    }

    /// <summary>
    /// Transforms given relative tile coordinate from this layout space to entity relative space (relative to its
    /// origin). To transform a coordinate to absolute coord use <see cref="M:Mafi.Core.Entities.Static.Layout.EntityLayout.Transform(Mafi.RelTile3i,Mafi.Core.TileTransform)" />.
    /// </summary>
    /// <remarks>
    /// This transform does not simply rotate around (LayoutSize / 2) because it is off-center for layouts with even
    /// sizes. In order to prevent wobbling we need to transform around real center if possible. The only time this
    /// will introduce a little wobble is when one size is even and other odd. In this case there is no "stable"
    /// solution due to integer grid.
    /// </remarks>
    public RelTile2i TransformRelative(RelTile2i relTile, TileTransform transform)
    {
      RelTile2f relTile2f = relTile - this.GetCoreOffset();
      return new RelTile2f(transform.TransformMatrix.Transform(relTile2f.Vector2f)).RelTile2iFloored;
    }

    public RelTile2f TransformRelativeF(RelTile2f relTile, TileTransform transform)
    {
      RelTile2f relTile2f = relTile - this.GetCoreOffset();
      return new RelTile2f(transform.TransformMatrix.Transform(relTile2f.Vector2f));
    }

    /// <summary>Transforms a point around "true" core center.</summary>
    public RelTile2f TransformRelativeF_Point(RelTile2f relTile, TileTransform transform)
    {
      RelTile2f relTile2f = relTile - this.GetCoreOffsetF();
      return new RelTile2f(transform.TransformMatrix.Transform(relTile2f.Vector2f));
    }

    /// <summary>
    /// Transforms a point relative to the origin tile (e.g. <see cref="P:Mafi.Core.Entities.Static.StaticEntity.CenterTile" />), not the true center.
    /// </summary>
    public RelTile2f TransformRelativeF_Point_RelToCenterTile(
      RelTile2f relTile,
      TileTransform transform)
    {
      RelTile2f coreOffsetF = this.GetCoreOffsetF();
      Vector2f vector2f = (coreOffsetF - coreOffsetF.RelTile2fFloored).Vector2f;
      Vector2f absValue = transform.TransformMatrix.Transform(vector2f).AbsValue;
      return new RelTile2f(transform.TransformMatrix.Transform(relTile.Vector2f - vector2f) + absValue);
    }

    /// <summary>
    /// Transforms a point relative to the origin tile (e.g. <see cref="P:Mafi.Core.Entities.Static.StaticEntity.CenterTile" />), not the true center.
    /// </summary>
    public RelTile3f TransformRelativeF_Point_RelToCenterTile(
      RelTile3f relTile,
      TileTransform transform)
    {
      return this.TransformRelativeF_Point_RelToCenterTile(relTile.Xy, transform).ExtendZ(relTile.Z);
    }

    /// <summary>
    /// Transforms a point relative to the origin tile (e.g. <see cref="P:Mafi.Core.Entities.Static.StaticEntity.CenterTile" />), not the true center.
    /// </summary>
    public Tile3i TransformPoint_RelToCenterTile(RelTile3i relTile, TileTransform transform)
    {
      return this.TransformRelativeF_Point_RelToCenterTile(relTile.CornerRelTile3f, transform).RelTile3iRounded + transform.Position;
    }

    public Tile2i TransformPoint_RelToCenterTile(RelTile2i relTile, TileTransform transform)
    {
      return this.TransformRelativeF_Point_RelToCenterTile(relTile.RelTile2f, transform).RoundedRelTile2i + transform.Position.Xy;
    }

    public Tile3f TransformPoint_RelToCenterTile(RelTile3f relTile, TileTransform transform)
    {
      return this.TransformRelativeF_Point_RelToCenterTile(relTile, transform) + transform.Position.CornerTile3f;
    }

    public RelTile2f TransformDirection(RelTile2f direction, TileTransform transform)
    {
      return new RelTile2f(transform.TransformMatrix.Transform(direction.Vector2f));
    }

    public RelTile3f TransformDirection(RelTile3f direction, TileTransform transform)
    {
      return new RelTile3f(transform.TransformMatrix.Transform(direction.Xy.Vector2f).ExtendZ(direction.Z));
    }

    public RelTile2i TransformDirection(RelTile2i direction, TileTransform transform)
    {
      return new RelTile2i(transform.TransformMatrix.Transform(direction.Vector2i));
    }

    public RelTile3i TransformDirection(RelTile3i direction, TileTransform transform)
    {
      return new RelTile3i(transform.TransformMatrix.Transform(direction.Xy.Vector2i).ExtendZ(direction.Z));
    }

    public ImmutableArray<OccupiedTileRelative> GetOccupiedTilesRelative(TileTransform transform)
    {
      ImmutableArrayBuilder<OccupiedTileRelative> immutableArrayBuilder = new ImmutableArrayBuilder<OccupiedTileRelative>(this.LayoutTiles.Length);
      for (int index = 0; index < this.LayoutTiles.Length; ++index)
      {
        LayoutTile layoutTile = this.LayoutTiles[index];
        immutableArrayBuilder[index] = new OccupiedTileRelative(this.TransformRelative(layoutTile.Coord, transform), layoutTile.OccupiedThickness.From, layoutTile.OccupiedThickness.To - layoutTile.OccupiedThickness.From, layoutTile.Constraint, layoutTile.TileSurfaceProto.HasValue ? layoutTile.TileSurfaceProto.Value.SlimId : TileSurfaceSlimId.PhantomId, ThicknessTilesI.Zero);
      }
      return immutableArrayBuilder.GetImmutableArrayAndClear();
    }

    public ImmutableArray<OccupiedVertexRelative> GetOccupiedVerticesRelative(
      TileTransform transform)
    {
      ImmutableArrayBuilder<OccupiedVertexRelative> immutableArrayBuilder = new ImmutableArrayBuilder<OccupiedVertexRelative>(this.TerrainVertices.Length);
      RelTile2i relTile2i1 = (RelTile2i.One - transform.TransformMatrix.Transform(RelTile2i.One)) / 2;
      for (int index = 0; index < this.TerrainVertices.Length; ++index)
      {
        TerrainVertexRel terrainVertex = this.TerrainVertices[index];
        RelTile2i relTile2i2 = this.TransformRelative(terrainVertex.Coord, transform);
        immutableArrayBuilder[index] = new OccupiedVertexRelative(relTile2i2 + relTile2i1, terrainVertex.OccupiedThickness.From, terrainVertex.OccupiedThickness.Height, terrainVertex.Constraint, (TerrainMaterialSlimIdOption) (terrainVertex.TerrainMaterial.HasValue ? terrainVertex.TerrainMaterial.Value.SlimId : TerrainMaterialSlimId.PhantomId), terrainVertex.TerrainHeight, !terrainVertex.TerrainHeight.HasValue || terrainVertex.TerrainHeight.Value.IsZero ? new ThicknessTilesI?() : new ThicknessTilesI?(ThicknessTilesI.Zero), terrainVertex.MinTerrainHeight, terrainVertex.MaxTerrainHeight, terrainVertex.LowestTileIndex);
      }
      return immutableArrayBuilder.GetImmutableArrayAndClear();
    }

    public ImmutableArray<KeyValuePair<Tile2i, HeightTilesF>> GetVehicleSurfaceHeights(
      TileTransform transform)
    {
      ImmutableArrayBuilder<KeyValuePair<Tile2i, HeightTilesF>> immutableArrayBuilder = new ImmutableArrayBuilder<KeyValuePair<Tile2i, HeightTilesF>>(this.VehicleSurfaceHeights.Length);
      HeightTilesF heightTilesF1 = transform.Position.Height.HeightTilesF;
      RelTile2i relTile2i = (RelTile2i.One - transform.TransformMatrix.Transform(RelTile2i.One)) / 2;
      for (int index = 0; index < this.VehicleSurfaceHeights.Length; ++index)
      {
        KeyValuePair<RelTile2i, ThicknessTilesF> vehicleSurfaceHeight = this.VehicleSurfaceHeights[index];
        HeightTilesF heightTilesF2 = vehicleSurfaceHeight.Value == EntityLayout.VEHICLE_INACCESSIBLE_HEIGHT_REL ? EntityLayout.VEHICLE_INACCESSIBLE_HEIGHT : heightTilesF1 + vehicleSurfaceHeight.Value;
        immutableArrayBuilder[index] = Make.Kvp<Tile2i, HeightTilesF>(this.Transform(vehicleSurfaceHeight.Key, transform) + relTile2i, heightTilesF2);
      }
      return immutableArrayBuilder.GetImmutableArrayAndClear();
    }

    public RectangleTerrainArea2i GetBoundingBox2i(TileTransform transform)
    {
      return RectangleTerrainArea2i.FromTwoPositions(this.Transform(RelTile2i.Zero, transform), this.Transform(this.LayoutSize.Xy, transform));
    }

    static EntityLayout()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      EntityLayout.VEHICLE_INACCESSIBLE_HEIGHT = HeightTilesF.MinValue;
      EntityLayout.VEHICLE_INACCESSIBLE_HEIGHT_REL = ThicknessTilesF.MinValue;
    }
  }
}
