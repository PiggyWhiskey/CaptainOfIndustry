// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Transports.TransportHelper
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Products;
using Mafi.Core.Terrain;
using Mafi.Localization;

#nullable disable
namespace Mafi.Core.Factory.Transports
{
  /// <summary>Collection of transport related functions.</summary>
  public static class TransportHelper
  {
    /// <summary>
    /// Returns minimal height on which can transport be placed without colliding with terrain.
    /// </summary>
    public static HeightTilesI GetLowestNonCollidingHeight(TerrainTile tile)
    {
      return tile.IsOcean ? HeightTilesI.One : (tile.MaxHeightOfAllCorners() - TransportProto.MAX_TERRAIN_PENETRATION).TilesHeightCeiled;
    }

    /// <summary>Returns pillar's base height (lowest point).</summary>
    public static HeightTilesI GetPillarBaseHeight(TerrainTile tile)
    {
      return (tile.MinHeightOfAllCorners() + TransportProto.MAX_TERRAIN_PENETRATION).TilesHeightFloored;
    }

    /// <summary>Computes occupied tile range for given pivot.</summary>
    public static OccupiedTileRange GetOccupiedRangeForPivot(Tile3i position)
    {
      return new OccupiedTileRange(position.Xy, position.Height, ThicknessTilesI.One);
    }

    /// <summary>
    /// Whether the given transport position is at its lowest position above terrain and there is no need
    /// for pillar below it.
    /// </summary>
    public static bool IsAtGroundWithNoNeedForPillarBelow(TerrainTile tile, HeightTilesI height)
    {
      HeightTilesI nonCollidingHeight = TransportHelper.GetLowestNonCollidingHeight(tile);
      HeightTilesI pillarBaseHeight = TransportHelper.GetPillarBaseHeight(tile);
      Assert.That<HeightTilesI>(pillarBaseHeight).IsLessOrEqual(nonCollidingHeight);
      return height <= nonCollidingHeight && nonCollidingHeight <= pillarBaseHeight;
    }

    /// <summary>
    /// Computes occupied tiles for a segment BETWEEN given positions. Returns no tiles if the given pivots are
    /// on the same height and directly next to each other tiles.
    /// 
    /// Note that the <paramref name="start" /> and <paramref name="end" /> are NOT part of the output.
    /// 
    /// This method is thread safe.
    /// </summary>
    public static void ComputeOccupiedTilesForSegment(
      Tile3i start,
      Tile3i end,
      Lyst<OccupiedTileRange> outTiles,
      Lyst<TransportTileMetadata?> metadata = null)
    {
      RelTile3i relTile3i = end - start;
      Assert.That<bool>(relTile3i.X != 0 && relTile3i.Y != 0).IsFalse<Tile3i, Tile3i>("Transport segments cannot be diagonal in horizontal plane, got segment from {0} to {1}.", start, end);
      Tile2i xy1 = start.Xy;
      RelTile2i xy2 = relTile3i.Xy;
      RelTile2i signs = xy2.Signs;
      int denominator = (relTile3i.X + relTile3i.Y).Abs();
      TransportTileMetadata transportTileMetadata = new TransportTileMetadata();
      if (denominator == 0)
      {
        int num = relTile3i.Z.Abs();
        Direction903d direction903d = relTile3i.ToDirection903d();
        if (metadata != null)
        {
          TransportStartEndType endType = TransportStartEndType.Vertical;
          transportTileMetadata = new TransportTileMetadata(-direction903d, TransportStartEndType.Vertical, direction903d, endType);
        }
        if (relTile3i.Z < 0)
        {
          for (int index = num - 1; index >= 1; --index)
          {
            outTiles.Add(new OccupiedTileRange(xy1, start.Height.Min(end.Height) + index * ThicknessTilesI.One, ThicknessTilesI.One));
            metadata?.Add(new TransportTileMetadata?(transportTileMetadata));
          }
        }
        else
        {
          for (int index = 1; index < num; ++index)
          {
            outTiles.Add(new OccupiedTileRange(xy1, start.Height.Min(end.Height) + index * ThicknessTilesI.One, ThicknessTilesI.One));
            metadata?.Add(new TransportTileMetadata?(transportTileMetadata));
          }
        }
      }
      else
      {
        if (metadata != null)
        {
          xy2 = relTile3i.Xy;
          Direction903d as3d = xy2.ToDirection90().As3d;
          TransportStartEndType endType = (TransportStartEndType) relTile3i.Z.Sign();
          transportTileMetadata = new TransportTileMetadata(-as3d, (TransportStartEndType) -(int) endType, as3d, endType);
        }
        if (start.Z == end.Z)
        {
          for (int index = 1; index < denominator; ++index)
          {
            xy1 += signs;
            outTiles.Add(new OccupiedTileRange(xy1, start.Height, ThicknessTilesI.One));
            metadata?.Add(new TransportTileMetadata?(transportTileMetadata));
          }
        }
        else
        {
          if (start.Z < end.Z)
          {
            outTiles.Add(new OccupiedTileRange(start.Xy, start.Height + ThicknessTilesI.One, ThicknessTilesI.One));
            metadata?.Add(new TransportTileMetadata?());
          }
          else if (start.Z > end.Z)
          {
            outTiles.Add(new OccupiedTileRange(start.Xy, start.Height - ThicknessTilesI.One, ThicknessTilesI.One));
            metadata?.Add(new TransportTileMetadata?());
          }
          Fix32 fix32_1 = relTile3i.Z.Over(denominator);
          for (int index = 1; index < denominator; ++index)
          {
            xy1 += signs;
            Fix32 fix32_2 = index * fix32_1;
            Fix32 fix32_3 = fix32_2 - Fix32.One;
            ThicknessTilesI thicknessTilesI1 = fix32_3.ToIntCeiled().TilesThick();
            fix32_3 = fix32_2 + Fix32.One;
            ThicknessTilesI thicknessTilesI2 = fix32_3.ToIntFloored().TilesThick();
            outTiles.Add(new OccupiedTileRange(xy1, start.Height + thicknessTilesI1, thicknessTilesI2 - thicknessTilesI1 + ThicknessTilesI.One));
            metadata?.Add(new TransportTileMetadata?(transportTileMetadata));
          }
          if (start.Z < end.Z)
          {
            outTiles.Add(new OccupiedTileRange(end.Xy, end.Height - ThicknessTilesI.One, ThicknessTilesI.One));
            metadata?.Add(new TransportTileMetadata?());
          }
          else
          {
            if (start.Z <= end.Z)
              return;
            outTiles.Add(new OccupiedTileRange(end.Xy, end.Height + ThicknessTilesI.One, ThicknessTilesI.One));
            metadata?.Add(new TransportTileMetadata?());
          }
        }
      }
    }

    /// <summary>Computes occupied tiles for given trajectory.</summary>
    public static ImmutableArray<OccupiedTileRange> ComputeOccupiedTiles(
      TransportTrajectory trajectory)
    {
      return TransportHelper.computeOccupiedTiles(trajectory);
    }

    /// <summary>
    /// Computes occupied tiles for given trajectory including per-tile metadata.
    /// </summary>
    public static ImmutableArray<OccupiedTileRange> ComputeOccupiedTiles(
      TransportTrajectory trajectory,
      out ImmutableArray<TransportTileMetadata?> metadata)
    {
      Lyst<TransportTileMetadata?> metadata1 = new Lyst<TransportTileMetadata?>();
      ImmutableArray<OccupiedTileRange> occupiedTiles = TransportHelper.computeOccupiedTiles(trajectory, metadata1);
      Assert.That<ImmutableArray<OccupiedTileRange>>(occupiedTiles).HasLength<OccupiedTileRange>(metadata1.Count, "Occupied tiles and metadata tiles count mismatch");
      metadata = metadata1.ToImmutableArray();
      return occupiedTiles;
    }

    private static ImmutableArray<OccupiedTileRange> computeOccupiedTiles(
      TransportTrajectory trajectory,
      Lyst<TransportTileMetadata?> metadata = null)
    {
      ImmutableArray<Tile3i> pivots = trajectory.Pivots;
      if (pivots.IsEmpty)
      {
        Log.Warning("Calling `ComputeOccupiedTiles` with empty pivots array.");
        return ImmutableArray<OccupiedTileRange>.Empty;
      }
      RelTile3i relTile3i1 = pivots.Length <= 1 ? trajectory.EndDirection : pivots.Second - pivots.First;
      RelTile2i xy = trajectory.StartDirection.Xy;
      Direction90 direction90;
      Direction903d startDirection1;
      TransportStartEndType startType1;
      if (xy.IsNotZero)
      {
        xy = trajectory.StartDirection.Xy;
        direction90 = xy.ToDirection90();
        startDirection1 = direction90.As3d;
        startType1 = dzToType(trajectory.StartDirection.Z);
      }
      else
      {
        startDirection1 = trajectory.StartDirection.ToDirection903d();
        startType1 = TransportStartEndType.Vertical;
      }
      xy = relTile3i1.Xy;
      Direction903d endDirection1;
      TransportStartEndType endType1;
      if (xy.IsNotZero)
      {
        xy = relTile3i1.Xy;
        direction90 = xy.ToDirection90();
        endDirection1 = direction90.As3d;
        endType1 = dzToType(relTile3i1.Z);
      }
      else
      {
        endDirection1 = relTile3i1.ToDirection903d();
        endType1 = TransportStartEndType.Vertical;
      }
      TransportTileMetadata transportTileMetadata = new TransportTileMetadata(startDirection1, startType1, endDirection1, endType1);
      if (pivots.Length == 1)
      {
        metadata?.Add(new TransportTileMetadata?(transportTileMetadata));
        return ImmutableArray.Create<OccupiedTileRange>(new OccupiedTileRange(pivots.First.Xy, pivots.First.Height, ThicknessTilesI.One));
      }
      Lyst<OccupiedTileRange> outTiles = new Lyst<OccupiedTileRange>(pivots.Length * 4);
      outTiles.Add(TransportHelper.GetOccupiedRangeForPivot(pivots.First));
      metadata?.Add(new TransportTileMetadata?(transportTileMetadata));
      Tile3i start = pivots.First;
      for (int index = 1; index < pivots.Length; ++index)
      {
        Tile3i tile3i = pivots[index];
        RelTile3i relTile3i2 = start - tile3i;
        xy = relTile3i2.Xy;
        Direction903d startDirection2;
        TransportStartEndType startType2;
        if (xy.IsNotZero)
        {
          xy = relTile3i2.Xy;
          direction90 = xy.ToDirection90();
          startDirection2 = direction90.As3d;
          startType2 = dzToType(relTile3i2.Z);
        }
        else
        {
          startDirection2 = relTile3i2.ToDirection903d();
          startType2 = TransportStartEndType.Vertical;
        }
        TransportHelper.ComputeOccupiedTilesForSegment(start, tile3i, outTiles, metadata);
        outTiles.Add(TransportHelper.GetOccupiedRangeForPivot(tile3i));
        if (metadata != null)
        {
          RelTile3i relTile3i3 = index + 1 < pivots.Length ? pivots[index + 1] - tile3i : trajectory.EndDirection;
          xy = relTile3i3.Xy;
          Direction903d endDirection2;
          TransportStartEndType endType2;
          if (xy.IsNotZero)
          {
            xy = relTile3i3.Xy;
            direction90 = xy.ToDirection90();
            endDirection2 = direction90.As3d;
            endType2 = dzToType(relTile3i3.Z);
          }
          else
          {
            endDirection2 = relTile3i3.ToDirection903d();
            endType2 = TransportStartEndType.Vertical;
          }
          metadata.Add(new TransportTileMetadata?(new TransportTileMetadata(startDirection2, startType2, endDirection2, endType2)));
        }
        start = tile3i;
      }
      return outTiles.ToImmutableArray();

      static TransportStartEndType dzToType(int dZ) => (TransportStartEndType) dZ.Sign();
    }

    public static ImmutableArray<OccupiedTileRelative> ComputeOccupiedTilesRelative(
      Tile3i origin,
      TransportTrajectory trajectory,
      TerrainManager terrainManager)
    {
      ImmutableArrayBuilder<OccupiedTileRelative> immutableArrayBuilder = new ImmutableArrayBuilder<OccupiedTileRelative>(trajectory.OccupiedTiles.Length);
      TerrainTileSurfaceProto valueOrNull = trajectory.TransportProto.TileSurfaceWhenOnGround.ValueOrNull;
      // ISSUE: explicit non-virtual call
      TileSurfaceSlimId tileSurfaceSlimId = valueOrNull != null ? __nonvirtual (valueOrNull.SlimId) : TileSurfaceSlimId.PhantomId;
      for (int index = 0; index < trajectory.OccupiedTiles.Length; ++index)
      {
        OccupiedTileRange occupiedTile = trajectory.OccupiedTiles[index];
        LayoutTileConstraint constraint = LayoutTileConstraint.None;
        TileSurfaceSlimId tileSurface = TileSurfaceSlimId.PhantomId;
        if (TransportHelper.IsAtGroundWithNoNeedForPillarBelow(terrainManager[(Tile2i) occupiedTile.Position], occupiedTile.From))
        {
          constraint = LayoutTileConstraint.Ground;
          tileSurface = tileSurfaceSlimId;
        }
        ThicknessTilesI thicknessTilesI = occupiedTile.From - origin.Height;
        immutableArrayBuilder[index] = new OccupiedTileRelative((Tile2i) occupiedTile.Position - origin.Xy, thicknessTilesI, occupiedTile.VerticalSize, constraint, tileSurface, thicknessTilesI);
      }
      return immutableArrayBuilder.GetImmutableArrayAndClear();
    }

    public static ImmutableArray<OccupiedVertexRelative> ComputeOccupiedVertices(
      ImmutableArray<OccupiedTileRelative> occupiedTiles,
      bool canBeBuried)
    {
      Dict<RelTile2i, OccupiedVertexRelative> vertices = new Dict<RelTile2i, OccupiedVertexRelative>(occupiedTiles.Length * 2);
      for (int index = 0; index < occupiedTiles.Length; ++index)
      {
        OccupiedTileRelative occupiedTile = occupiedTiles[index];
        ThicknessTilesI? nullable = occupiedTile.Constraint.HasAnyConstraints(LayoutTileConstraint.Ground) ? new ThicknessTilesI?(occupiedTile.FromHeightRel) : new ThicknessTilesI?();
        ThicknessTilesI? maxTerrainHeight = canBeBuried ? new ThicknessTilesI?() : new ThicknessTilesI?(occupiedTile.FromHeightRel);
        mergeVertex(new OccupiedVertexRelative(occupiedTile.RelCoord, occupiedTile.FromHeightRel, occupiedTile.VerticalSize, occupiedTile.Constraint, TerrainMaterialSlimIdOption.Phantom, nullable, new ThicknessTilesI?(), nullable, maxTerrainHeight, index));
        RelTile2i relCoord = occupiedTile.RelCoord;
        mergeVertex(new OccupiedVertexRelative(relCoord.IncrementX, occupiedTile.FromHeightRel, occupiedTile.VerticalSize, occupiedTile.Constraint, TerrainMaterialSlimIdOption.Phantom, nullable, new ThicknessTilesI?(), nullable, maxTerrainHeight, index));
        relCoord = occupiedTile.RelCoord;
        mergeVertex(new OccupiedVertexRelative(relCoord.IncrementY, occupiedTile.FromHeightRel, occupiedTile.VerticalSize, occupiedTile.Constraint, TerrainMaterialSlimIdOption.Phantom, nullable, new ThicknessTilesI?(), nullable, maxTerrainHeight, index));
        relCoord = occupiedTile.RelCoord;
        mergeVertex(new OccupiedVertexRelative(relCoord.AddXy(1), occupiedTile.FromHeightRel, occupiedTile.VerticalSize, occupiedTile.Constraint, TerrainMaterialSlimIdOption.Phantom, nullable, new ThicknessTilesI?(), nullable, maxTerrainHeight, index));
      }
      return vertices.Values.ToImmutableArray<OccupiedVertexRelative>();

      void mergeVertex(OccupiedVertexRelative newV)
      {
        bool exists;
        ref OccupiedVertexRelative local = ref vertices.GetRefValue(newV.RelCoord, out exists);
        local = exists ? local.MergeWithRelaxedHeightConstraints(newV) : newV;
      }
    }

    /// <summary>Computes supportable positions along trajectory.</summary>
    public static void ComputeSupportableAlongTrajectory(
      TransportTrajectory trajectory,
      Lyst<TransportSupportableTile> supportablePositions)
    {
      ImmutableArray<OccupiedTileRange> occupiedTiles = trajectory.OccupiedTiles;
      if (occupiedTiles.IsEmpty)
        return;
      ImmutableArray<TransportTileMetadata?> occupiedTilesMetadata = trajectory.OccupiedTilesMetadata;
      Assert.That<TransportTileMetadata?>(occupiedTilesMetadata.First).HasValue<TransportTileMetadata>();
      Assert.That<Lyst<TransportSupportableTile>>(supportablePositions).IsEmpty<TransportSupportableTile>();
      supportablePositions.EnsureCapacity(occupiedTiles.Length);
      if (trajectory.TransportProto.ZStepLength.IsZero)
      {
        OccupiedTileRange? nullable1 = new OccupiedTileRange?();
        for (int index = 0; index < occupiedTiles.Length; ++index)
        {
          TransportTileMetadata? nullable2 = occupiedTilesMetadata[index];
          if (!nullable2.HasValue)
          {
            nullable1 = new OccupiedTileRange?();
          }
          else
          {
            OccupiedTileRange occupiedTileRange1 = occupiedTiles[index];
            if (nullable1.HasValue && nullable1.Value.Position == occupiedTileRange1.Position && nullable1.Value.From < occupiedTileRange1.From)
            {
              nullable1 = new OccupiedTileRange?(occupiedTileRange1);
            }
            else
            {
              if (index + 1 < occupiedTiles.Length && occupiedTilesMetadata[index + 1].HasValue)
              {
                OccupiedTileRange occupiedTileRange2 = occupiedTiles[index + 1];
                if (occupiedTileRange2.Position == occupiedTileRange1.Position && occupiedTileRange2.From < occupiedTileRange1.From)
                {
                  nullable1 = new OccupiedTileRange?(occupiedTileRange1);
                  continue;
                }
              }
              Rotation90 rotation;
              bool flipY;
              TransportPillarAttachmentType attachmentType = nullable2.Value.GetAttachmentType(out rotation, out flipY);
              supportablePositions.Add(new TransportSupportableTile(occupiedTileRange1.Position.ExtendHeight(occupiedTileRange1.From), index, attachmentType, rotation, flipY));
              nullable1 = new OccupiedTileRange?(occupiedTileRange1);
            }
          }
        }
      }
      else
      {
        for (int index = 0; index < occupiedTiles.Length; ++index)
        {
          TransportTileMetadata? nullable = occupiedTilesMetadata[index];
          if (nullable.HasValue)
          {
            OccupiedTileRange occupiedTileRange = occupiedTiles[index];
            Rotation90 rotation;
            bool flipY;
            TransportPillarAttachmentType attachmentType = nullable.Value.GetAttachmentType(out rotation, out flipY);
            supportablePositions.Add(new TransportSupportableTile(occupiedTileRange.Position.ExtendHeight(occupiedTileRange.From), index, attachmentType, rotation, flipY));
          }
        }
      }
    }

    public static bool TryCutFirstTileFromPivots(
      ReadOnlyArraySlice<Tile3i> pivots,
      out ReadOnlyArraySlice<Tile3i> newPivots,
      out LocStrFormatted error)
    {
      if (pivots.Length <= 1)
      {
        error = (LocStrFormatted) TrCore.TrAdditionError__InvalidTransport;
        newPivots = ReadOnlyArraySlice<Tile3i>.Empty;
        return false;
      }
      if (pivots.First.Z != pivots.Second.Z)
      {
        Tile3i tile3i = pivots.First;
        Tile2i xy1 = tile3i.Xy;
        tile3i = pivots.Second;
        Tile2i xy2 = tile3i.Xy;
        if (xy1 != xy2)
        {
          error = (LocStrFormatted) TrCore.TrAdditionError__NotFlat;
          newPivots = ReadOnlyArraySlice<Tile3i>.Empty;
          return false;
        }
      }
      RelTile3i relTile3i = pivots.Second - pivots.First;
      if (relTile3i.Sum.Abs() > 1)
      {
        Tile3i[] array = pivots.ToArray();
        array[0] = pivots.First + relTile3i.Signs;
        newPivots = array.AsSlice<Tile3i>();
      }
      else
        newPivots = pivots.Slice(1);
      error = LocStrFormatted.Empty;
      return true;
    }

    public static bool TryCutLastTileFromPivots(
      ReadOnlyArraySlice<Tile3i> pivots,
      out ReadOnlyArraySlice<Tile3i> newPivots,
      out LocStrFormatted error)
    {
      if (pivots.Length <= 1)
      {
        error = (LocStrFormatted) TrCore.TrAdditionError__InvalidTransport;
        newPivots = ReadOnlyArraySlice<Tile3i>.Empty;
        return false;
      }
      if (pivots.Last.Z != pivots.PreLast.Z)
      {
        Tile3i tile3i = pivots.Last;
        Tile2i xy1 = tile3i.Xy;
        tile3i = pivots.PreLast;
        Tile2i xy2 = tile3i.Xy;
        if (xy1 != xy2)
        {
          error = (LocStrFormatted) TrCore.TrAdditionError__NotFlat;
          newPivots = ReadOnlyArraySlice<Tile3i>.Empty;
          return false;
        }
      }
      RelTile3i relTile3i = pivots.PreLast - pivots.Last;
      if (relTile3i.Sum.Abs() > 1)
      {
        Tile3i[] array = pivots.ToArray();
        array[array.Length - 1] = pivots.Last + relTile3i.Signs;
        newPivots = array.AsSlice<Tile3i>();
      }
      else
        newPivots = pivots.Slice(0, pivots.Length - 1);
      error = LocStrFormatted.Empty;
      return true;
    }
  }
}
