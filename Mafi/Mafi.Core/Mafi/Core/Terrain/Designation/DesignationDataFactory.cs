// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Designation.DesignationDataFactory
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Localization;
using System;

#nullable disable
namespace Mafi.Core.Terrain.Designation
{
  /// <summary>Helper class that helps with designation creation.</summary>
  public static class DesignationDataFactory
  {
    private static readonly int[] SET_BITS_COUNT;

    public static Tile2i GetOrigin(Vector2i v) => new Tile2i(v) * 4;

    public static DesignationData CreateFlat(Tile2i position, HeightTilesI height)
    {
      return new DesignationData(TerrainDesignation.GetOrigin(position), height);
    }

    public static DesignationData CreateRamp(
      Tile2i position,
      HeightTilesI startHeight,
      bool isRampUp,
      Direction90 rampDirection)
    {
      HeightTilesI heightTilesI = startHeight + (isRampUp ? ThicknessTilesI.One : -ThicknessTilesI.One);
      return new DesignationData(TerrainDesignation.GetOrigin(position), !(rampDirection != Direction90.MinusX) || !(rampDirection != Direction90.MinusY) ? heightTilesI : startHeight, !(rampDirection != Direction90.PlusX) || !(rampDirection != Direction90.MinusY) ? heightTilesI : startHeight, !(rampDirection != Direction90.PlusX) || !(rampDirection != Direction90.PlusY) ? heightTilesI : startHeight, !(rampDirection != Direction90.MinusX) || !(rampDirection != Direction90.PlusY) ? heightTilesI : startHeight);
    }

    public static Lyst<DesignationData> CreateArea(
      DesignationType type,
      Vector2i from,
      Vector2i toIncl,
      HeightTilesI startHeight,
      Direction90 rampDirection)
    {
      return DesignationDataFactory.CreateArea(type, DesignationDataFactory.GetOrigin(from), DesignationDataFactory.GetOrigin(toIncl), startHeight, rampDirection);
    }

    public static Lyst<DesignationData> CreateArea(
      DesignationType type,
      Tile2i from,
      Tile2i toIncl,
      HeightTilesI startHeight,
      Direction90 rampDirection)
    {
      return type != DesignationType.Flat ? DesignationDataFactory.CreateRampArea(from, toIncl, startHeight, type == DesignationType.RampUp, rampDirection) : DesignationDataFactory.CreateFlatArea(from, toIncl, startHeight);
    }

    public static Lyst<DesignationData> CreateFlatArea(
      Vector2i from,
      Vector2i toIncl,
      HeightTilesI height)
    {
      return DesignationDataFactory.CreateFlatArea(DesignationDataFactory.GetOrigin(from), DesignationDataFactory.GetOrigin(toIncl), height);
    }

    public static Lyst<DesignationData> CreateFlatArea(
      Tile2i from,
      Tile2i toIncl,
      HeightTilesI height)
    {
      Tile2i origin1 = TerrainDesignation.GetOrigin(from.Min(toIncl));
      Tile2i origin2 = TerrainDesignation.GetOrigin(from.Max(toIncl));
      RelTile2i relTile2i = (origin2 - origin1) / 4;
      relTile2i = relTile2i.AddXy(1);
      int productInt = relTile2i.ProductInt;
      Lyst<DesignationData> flatArea = new Lyst<DesignationData>(productInt);
      for (int y = origin1.Y; y <= origin2.Y; y += 4)
      {
        for (int x = origin1.X; x <= origin2.X; x += 4)
          flatArea.Add(new DesignationData(new Tile2i(x, y), height));
      }
      Assert.That<Lyst<DesignationData>>(flatArea).HasLength<DesignationData>(productInt);
      return flatArea;
    }

    public static Lyst<DesignationData> CreateRampArea(
      Vector2i from,
      Vector2i toIncl,
      HeightTilesI startHeight,
      bool isRampUp,
      Direction90 rampDirection)
    {
      return DesignationDataFactory.CreateRampArea(DesignationDataFactory.GetOrigin(from), DesignationDataFactory.GetOrigin(toIncl), startHeight, isRampUp, rampDirection);
    }

    public static Lyst<DesignationData> CreateRampArea(
      Tile2i from,
      Tile2i toIncl,
      HeightTilesI startHeight,
      bool isRampUp,
      Direction90 rampDirection)
    {
      Tile2i origin1 = TerrainDesignation.GetOrigin(from.Min(toIncl));
      Tile2i origin2 = TerrainDesignation.GetOrigin(from.Max(toIncl));
      Vector2i vector2i = (origin2 - origin1).Vector2i / 4 + Vector2i.One;
      Vector2i directionVector = rampDirection.DirectionVector;
      Lyst<DesignationData> rampArea = new Lyst<DesignationData>(((origin2 - origin1) / 4).ProductInt, true);
      if (rampDirection == Direction90.MinusX)
        startHeight += new ThicknessTilesI(vector2i.X * (isRampUp ? 1 : -1));
      else if (rampDirection == Direction90.MinusY)
        startHeight += new ThicknessTilesI(vector2i.Y * (isRampUp ? 1 : -1));
      ThicknessTilesI thicknessTilesI1 = new ThicknessTilesI(directionVector.X);
      ThicknessTilesI thicknessTilesI2 = new ThicknessTilesI(directionVector.Y);
      if (!isRampUp)
      {
        thicknessTilesI1 = -thicknessTilesI1;
        thicknessTilesI2 = -thicknessTilesI2;
      }
      for (int y = 0; y < vector2i.Y; ++y)
      {
        for (int x = 0; x < vector2i.X; ++x)
        {
          Tile2i originTile = origin1 + new RelTile2i(x, y) * 4;
          HeightTilesI originTargetHeight = startHeight + x * thicknessTilesI1 + y * thicknessTilesI2;
          rampArea.Add(new DesignationData(originTile, originTargetHeight, originTargetHeight + thicknessTilesI1, originTargetHeight + thicknessTilesI1 + thicknessTilesI2, originTargetHeight + thicknessTilesI2));
        }
      }
      return rampArea;
    }

    public static DesignationData CreateFlatNoSnapping(
      Tile2i position,
      bool preferInitialBelowTerrain,
      TerrainManager terrainManager)
    {
      Tile2i origin = TerrainDesignation.GetOrigin(position);
      HeightTilesI height1;
      if (preferInitialBelowTerrain)
      {
        HeightTilesF height2 = terrainManager.GetHeight(origin);
        HeightTilesI tilesHeightFloored1 = height2.TilesHeightFloored;
        ref HeightTilesI local1 = ref tilesHeightFloored1;
        height2 = terrainManager.GetHeight(origin.AddX(2));
        HeightTilesI tilesHeightFloored2 = height2.TilesHeightFloored;
        HeightTilesI heightTilesI1 = local1.Min(tilesHeightFloored2);
        ref HeightTilesI local2 = ref heightTilesI1;
        height2 = terrainManager.GetHeight(origin.AddX(4));
        HeightTilesI tilesHeightFloored3 = height2.TilesHeightFloored;
        HeightTilesI heightTilesI2 = local2.Min(tilesHeightFloored3);
        ref HeightTilesI local3 = ref heightTilesI2;
        height2 = terrainManager.GetHeight(origin.AddX(4).AddY(2));
        HeightTilesI tilesHeightFloored4 = height2.TilesHeightFloored;
        HeightTilesI heightTilesI3 = local3.Min(tilesHeightFloored4);
        ref HeightTilesI local4 = ref heightTilesI3;
        height2 = terrainManager.GetHeight(origin.AddXy(4));
        HeightTilesI tilesHeightFloored5 = height2.TilesHeightFloored;
        HeightTilesI heightTilesI4 = local4.Min(tilesHeightFloored5);
        ref HeightTilesI local5 = ref heightTilesI4;
        height2 = terrainManager.GetHeight(origin.AddY(2));
        HeightTilesI tilesHeightFloored6 = height2.TilesHeightFloored;
        HeightTilesI heightTilesI5 = local5.Min(tilesHeightFloored6);
        ref HeightTilesI local6 = ref heightTilesI5;
        height2 = terrainManager.GetHeight(origin.AddY(4));
        HeightTilesI tilesHeightFloored7 = height2.TilesHeightFloored;
        HeightTilesI heightTilesI6 = local6.Min(tilesHeightFloored7);
        ref HeightTilesI local7 = ref heightTilesI6;
        height2 = terrainManager.GetHeight(origin.AddY(4).AddX(2));
        HeightTilesI tilesHeightFloored8 = height2.TilesHeightFloored;
        height1 = local7.Min(tilesHeightFloored8);
      }
      else
      {
        HeightTilesF height3 = terrainManager.GetHeight(origin);
        HeightTilesI tilesHeightCeiled1 = height3.TilesHeightCeiled;
        ref HeightTilesI local8 = ref tilesHeightCeiled1;
        height3 = terrainManager.GetHeight(origin.AddX(2));
        HeightTilesI tilesHeightCeiled2 = height3.TilesHeightCeiled;
        HeightTilesI heightTilesI7 = local8.Max(tilesHeightCeiled2);
        ref HeightTilesI local9 = ref heightTilesI7;
        height3 = terrainManager.GetHeight(origin.AddX(4));
        HeightTilesI tilesHeightCeiled3 = height3.TilesHeightCeiled;
        HeightTilesI heightTilesI8 = local9.Max(tilesHeightCeiled3);
        ref HeightTilesI local10 = ref heightTilesI8;
        height3 = terrainManager.GetHeight(origin.AddX(4).AddY(2));
        HeightTilesI tilesHeightCeiled4 = height3.TilesHeightCeiled;
        HeightTilesI heightTilesI9 = local10.Max(tilesHeightCeiled4);
        ref HeightTilesI local11 = ref heightTilesI9;
        height3 = terrainManager.GetHeight(origin.AddXy(4));
        HeightTilesI tilesHeightCeiled5 = height3.TilesHeightCeiled;
        HeightTilesI heightTilesI10 = local11.Max(tilesHeightCeiled5);
        ref HeightTilesI local12 = ref heightTilesI10;
        height3 = terrainManager.GetHeight(origin.AddY(2));
        HeightTilesI tilesHeightCeiled6 = height3.TilesHeightCeiled;
        HeightTilesI heightTilesI11 = local12.Max(tilesHeightCeiled6);
        ref HeightTilesI local13 = ref heightTilesI11;
        height3 = terrainManager.GetHeight(origin.AddY(4));
        HeightTilesI tilesHeightCeiled7 = height3.TilesHeightCeiled;
        HeightTilesI heightTilesI12 = local13.Max(tilesHeightCeiled7);
        ref HeightTilesI local14 = ref heightTilesI12;
        height3 = terrainManager.GetHeight(origin.AddY(4).AddX(2));
        HeightTilesI tilesHeightCeiled8 = height3.TilesHeightCeiled;
        height1 = local14.Max(tilesHeightCeiled8);
      }
      return DesignationDataFactory.CreateFlat(origin, height1);
    }

    public static DesignationData CreateRampNoSnapping(
      Tile2i position,
      bool preferInitialBelowTerrain,
      bool isRampUp,
      Direction90 rampDirection,
      TerrainManager terrainManager)
    {
      Tile2i origin = TerrainDesignation.GetOrigin(position);
      Tile2i tile;
      RelTile2i relTile2i;
      switch (rampDirection.DirectionIndex)
      {
        case 0:
          tile = origin;
          relTile2i = new RelTile2i(0, 1);
          break;
        case 1:
          tile = origin;
          relTile2i = new RelTile2i(1, 0);
          break;
        case 2:
          tile = origin.AddXy(4);
          relTile2i = new RelTile2i(0, -1);
          break;
        default:
          Assert.That<int>(rampDirection.DirectionIndex).IsEqualTo(3);
          tile = origin.AddXy(4);
          relTile2i = new RelTile2i(-1, 0);
          break;
      }
      HeightTilesI startHeight;
      if (preferInitialBelowTerrain)
      {
        startHeight = terrainManager.GetHeight(tile).TilesHeightFloored;
        for (int index = 1; index < 4; ++index)
        {
          tile += relTile2i;
          startHeight = startHeight.Min(terrainManager.GetHeight(tile).TilesHeightFloored);
        }
      }
      else
      {
        startHeight = terrainManager.GetHeight(tile).TilesHeightCeiled;
        for (int index = 1; index < 4; ++index)
        {
          tile += relTile2i;
          startHeight = startHeight.Max(terrainManager.GetHeight(tile).TilesHeightCeiled);
        }
      }
      return DesignationDataFactory.CreateRamp(origin, startHeight, isRampUp, rampDirection);
    }

    public static bool TryCreateSnapToNeighbors(
      Tile2i position,
      bool preferFlat,
      bool preferInitialBelowTerrain,
      Direction90 preferredRampDirection,
      ThicknessTilesI heightBias,
      ITerrainDesignationsManager designationManager,
      out DesignationData data)
    {
      Tile2i origin = TerrainDesignation.GetOrigin(position);
      HeightTilesI? designatedHeightAtOrigin1 = designationManager.GetDesignatedHeightAtOrigin(origin);
      HeightTilesI? designatedHeightAtOrigin2 = designationManager.GetDesignatedHeightAtOrigin(origin.AddX(4));
      HeightTilesI? designatedHeightAtOrigin3 = designationManager.GetDesignatedHeightAtOrigin(origin.AddXy(4));
      HeightTilesI? designatedHeightAtOrigin4 = designationManager.GetDesignatedHeightAtOrigin(origin.AddY(4));
      uint index = (uint) ((designatedHeightAtOrigin1.HasValue ? 1 : 0) | (!designatedHeightAtOrigin2.HasValue ? 0 : 2) | (!designatedHeightAtOrigin3.HasValue ? 0 : 4) | (!designatedHeightAtOrigin4.HasValue ? 0 : 8));
      if (index == 0U)
      {
        DesignationData designationData = preferFlat ? DesignationDataFactory.CreateFlatNoSnapping(position, preferInitialBelowTerrain, designationManager.TerrainManager) : DesignationDataFactory.CreateRampNoSnapping(position, preferInitialBelowTerrain, preferInitialBelowTerrain, preferredRampDirection, designationManager.TerrainManager);
        data = new DesignationData(designationData.OriginTile, designationData.OriginTargetHeight + heightBias, designationData.PlusXTargetHeight + heightBias, designationData.PlusXyTargetHeight + heightBias, designationData.PlusYTargetHeight + heightBias);
        return true;
      }
      int num = DesignationDataFactory.SET_BITS_COUNT[(int) index];
      switch (num)
      {
        case 1:
          switch (index)
          {
            case 1:
              data = oneHeightConstraint(designatedHeightAtOrigin1.Value, Rotation90.Deg0);
              return true;
            case 2:
              data = oneHeightConstraint(designatedHeightAtOrigin2.Value, Rotation90.Deg90);
              return true;
            case 4:
              data = oneHeightConstraint(designatedHeightAtOrigin3.Value, Rotation90.Deg180);
              return true;
            default:
              Assert.That<uint>(index).IsEqualTo(8U);
              data = oneHeightConstraint(designatedHeightAtOrigin4.Value, Rotation90.Deg270);
              return true;
          }
        case 2:
          switch (index)
          {
            case 3:
              return twoHeightConstraint(designatedHeightAtOrigin1.Value, designatedHeightAtOrigin2.Value, Rotation90.Deg0, out data);
            case 5:
              if (designatedHeightAtOrigin1.Value == designatedHeightAtOrigin3.Value)
              {
                data = DesignationDataFactory.CreateFlat(origin, designatedHeightAtOrigin1.Value + heightBias);
                return true;
              }
              data = new DesignationData();
              return false;
            case 6:
              return twoHeightConstraint(designatedHeightAtOrigin2.Value, designatedHeightAtOrigin3.Value, Rotation90.Deg90, out data);
            case 9:
              return twoHeightConstraint(designatedHeightAtOrigin4.Value, designatedHeightAtOrigin1.Value, Rotation90.Deg270, out data);
            case 12:
              return twoHeightConstraint(designatedHeightAtOrigin3.Value, designatedHeightAtOrigin4.Value, Rotation90.Deg180, out data);
            default:
              Assert.That<uint>(index).IsEqualTo(10U);
              if (designatedHeightAtOrigin2.Value == designatedHeightAtOrigin4.Value)
              {
                data = DesignationDataFactory.CreateFlat(origin, designatedHeightAtOrigin2.Value);
                return true;
              }
              data = new DesignationData();
              return false;
          }
        case 3:
          switch (index)
          {
            case 7:
              return threeHeightConstraint(designatedHeightAtOrigin1.Value, designatedHeightAtOrigin2.Value, designatedHeightAtOrigin3.Value, Rotation90.Deg0, out data);
            case 13:
              return threeHeightConstraint(designatedHeightAtOrigin3.Value, designatedHeightAtOrigin4.Value, designatedHeightAtOrigin1.Value, Rotation90.Deg180, out data);
            case 14:
              return threeHeightConstraint(designatedHeightAtOrigin2.Value, designatedHeightAtOrigin3.Value, designatedHeightAtOrigin4.Value, Rotation90.Deg90, out data);
            default:
              Assert.That<uint>(index).IsEqualTo(11U);
              return threeHeightConstraint(designatedHeightAtOrigin4.Value, designatedHeightAtOrigin1.Value, designatedHeightAtOrigin2.Value, Rotation90.Deg270, out data);
          }
        default:
          Assert.That<int>(num).IsEqualTo(4);
          Assert.That<uint>(index).IsEqualTo(15U);
          if (designatedHeightAtOrigin1.Value == designatedHeightAtOrigin2.Value && designatedHeightAtOrigin3.Value == designatedHeightAtOrigin4.Value)
          {
            if (designatedHeightAtOrigin1.Value == designatedHeightAtOrigin4.Value)
            {
              data = DesignationDataFactory.CreateFlat(origin, designatedHeightAtOrigin1.Value + heightBias);
              return true;
            }
            if ((designatedHeightAtOrigin1.Value - designatedHeightAtOrigin4.Value).Abs == ThicknessTilesI.One)
            {
              data = new DesignationData(origin, designatedHeightAtOrigin1.Value, designatedHeightAtOrigin2.Value, designatedHeightAtOrigin3.Value, designatedHeightAtOrigin4.Value);
              return true;
            }
            data = new DesignationData();
            return false;
          }
          if (designatedHeightAtOrigin1.Value == designatedHeightAtOrigin4.Value && designatedHeightAtOrigin2.Value == designatedHeightAtOrigin3.Value)
          {
            if ((designatedHeightAtOrigin1.Value - designatedHeightAtOrigin2.Value).Abs == ThicknessTilesI.One)
            {
              data = new DesignationData(origin, designatedHeightAtOrigin1.Value, designatedHeightAtOrigin2.Value, designatedHeightAtOrigin3.Value, designatedHeightAtOrigin4.Value);
              return true;
            }
            data = new DesignationData();
            return false;
          }
          data = new DesignationData();
          return false;
      }

      DesignationData oneHeightConstraint(HeightTilesI h, Rotation90 relativeRot)
      {
        h += heightBias;
        if (preferFlat)
          return DesignationDataFactory.CreateFlat(origin, h);
        HeightTilesI heightTilesI = preferredRampDirection.DirectionIndex >= 2 ? h + ThicknessTilesI.One : h - ThicknessTilesI.One;
        Direction90 direction90 = preferredRampDirection - relativeRot;
        return DesignationDataFactory.rotate(direction90 == Direction90.PlusX || direction90 == Direction90.MinusX ? new DesignationData(origin, h, heightTilesI, heightTilesI, h) : new DesignationData(origin, h, h, heightTilesI, heightTilesI), relativeRot);
      }

      bool twoHeightConstraint(
        HeightTilesI h0,
        HeightTilesI h1,
        Rotation90 relativeRot,
        out DesignationData data)
      {
        if (h0 == h1)
        {
          if (preferFlat)
          {
            data = DesignationDataFactory.CreateFlat(origin, h0 + heightBias);
            return true;
          }
          HeightTilesI heightTilesI = preferredRampDirection.DirectionIndex % 2 != 0 ^ preferInitialBelowTerrain ? h0 + ThicknessTilesI.One : h0 - ThicknessTilesI.One;
          data = DesignationDataFactory.rotate(new DesignationData(origin, h0, h0, heightTilesI, heightTilesI), relativeRot);
          return true;
        }
        if ((h0 - h1).Abs == ThicknessTilesI.One)
        {
          data = DesignationDataFactory.rotate(new DesignationData(origin, h0, h1, h1, h0), relativeRot);
          return true;
        }
        data = new DesignationData();
        return false;
      }

      bool threeHeightConstraint(
        HeightTilesI h0,
        HeightTilesI h1,
        HeightTilesI h2,
        Rotation90 relativeRot,
        out DesignationData data)
      {
        if (h0 == h1)
        {
          if (h1 == h2)
          {
            data = DesignationDataFactory.CreateFlat(origin, h0 + heightBias);
            return true;
          }
          if ((h1 - h2).Abs == ThicknessTilesI.One)
          {
            data = DesignationDataFactory.rotate(new DesignationData(origin, h0, h1, h2, h2), relativeRot);
            return true;
          }
          data = new DesignationData();
          return false;
        }
        if (h1 == h2)
        {
          if ((h0 - h1).Abs == ThicknessTilesI.One)
          {
            data = DesignationDataFactory.rotate(new DesignationData(origin, h0, h1, h2, h0), relativeRot);
            return true;
          }
          data = new DesignationData();
          return false;
        }
        data = new DesignationData();
        return false;
      }
    }

    private static DesignationData rotate(DesignationData d, Rotation90 r)
    {
      switch (r.AngleIndex)
      {
        case 0:
          return d;
        case 1:
          return new DesignationData(d.OriginTile, d.PlusYTargetHeight, d.OriginTargetHeight, d.PlusXTargetHeight, d.PlusXyTargetHeight);
        case 2:
          return new DesignationData(d.OriginTile, d.PlusXyTargetHeight, d.PlusYTargetHeight, d.OriginTargetHeight, d.PlusXTargetHeight);
        default:
          Assert.That<int>(r.AngleIndex).IsEqualTo(3);
          return new DesignationData(d.OriginTile, d.PlusXTargetHeight, d.PlusXyTargetHeight, d.PlusYTargetHeight, d.OriginTargetHeight);
      }
    }

    /// <summary>
    /// Extends given designation to area specified by <paramref name="extensionEnd" />.
    /// </summary>
    public static void UpdateDesignationExtension(
      ITerrainDesignationsManager manager,
      TerrainDesignationProto proto,
      DesignationData sourceData,
      Tile2i extensionEnd,
      Action<TerrainDesignationProto, DesignationData> added,
      Action<TerrainDesignationProto, DesignationData> removed,
      Dict<Tile2i, DesignationData> designations)
    {
      Assert.That<Dict<Tile2i, DesignationData>>(designations).ContainsKey<Tile2i, DesignationData>(sourceData.OriginTile);
      Tile2i originTile = sourceData.OriginTile;
      Tile2i origin = TerrainDesignation.GetOrigin(extensionEnd);
      Tile2i tile2i1 = originTile.Min(origin);
      Tile2i tile2i2 = originTile.Max(origin);
      Lyst<Tile2i> lyst = (Lyst<Tile2i>) null;
      foreach (DesignationData designationData in designations.Values)
      {
        if (!(designationData.OriginTile >= tile2i1) || !(designationData.OriginTile <= tile2i2))
        {
          if (lyst == null)
            lyst = new Lyst<Tile2i>();
          lyst.Add(designationData.OriginTile);
        }
      }
      if (lyst != null)
      {
        foreach (Tile2i key in lyst)
        {
          DesignationData designationData;
          designations.TryRemove(key, out designationData).AssertTrue();
          removed(proto, designationData);
        }
      }
      RelTile2i relTile2i = (origin - originTile).Signs * 4;
      Vector2i vector2i = (origin - originTile).Vector2i.AbsValue / 4;
      ThicknessTilesI thicknessTilesI1 = ThicknessTilesI.Zero;
      ThicknessTilesI thicknessTilesI2 = ThicknessTilesI.Zero;
      bool flag = false;
      if (sourceData.OriginTargetHeight == sourceData.PlusXTargetHeight && sourceData.PlusXyTargetHeight == sourceData.PlusYTargetHeight)
      {
        if (sourceData.OriginTargetHeight == sourceData.PlusYTargetHeight)
        {
          flag = true;
        }
        else
        {
          if (!((sourceData.OriginTargetHeight - sourceData.PlusYTargetHeight).Abs == ThicknessTilesI.One))
            return;
          thicknessTilesI2 = origin.Y > originTile.Y ? sourceData.PlusYTargetHeight - sourceData.OriginTargetHeight : sourceData.OriginTargetHeight - sourceData.PlusYTargetHeight;
        }
      }
      else if (sourceData.OriginTargetHeight == sourceData.PlusYTargetHeight && sourceData.PlusXTargetHeight == sourceData.PlusXyTargetHeight)
      {
        if (!((sourceData.OriginTargetHeight - sourceData.PlusXTargetHeight).Abs == ThicknessTilesI.One))
          return;
        thicknessTilesI1 = origin.X > originTile.X ? sourceData.PlusXTargetHeight - sourceData.OriginTargetHeight : sourceData.OriginTargetHeight - sourceData.PlusXTargetHeight;
      }
      if (flag)
      {
        for (int y = tile2i1.Y; y <= tile2i2.Y; y += 4)
        {
          for (int x = tile2i1.X; x <= tile2i2.X; x += 4)
          {
            if (!designations.ContainsKey(new Tile2i(x, y)))
            {
              DesignationData flat = DesignationDataFactory.CreateFlat(new Tile2i(x, y), sourceData.OriginTargetHeight);
              designations.Add(flat.OriginTile, flat);
              added(proto, flat);
            }
          }
        }
      }
      else
      {
        for (int index1 = 0; index1 <= vector2i.Y; ++index1)
        {
          for (int index2 = 0; index2 <= vector2i.X; ++index2)
          {
            Tile2i tile2i3 = originTile + new RelTile2i(index2 * relTile2i.X, index1 * relTile2i.Y);
            if (!designations.ContainsKey(tile2i3))
            {
              ThicknessTilesI thicknessTilesI3 = index2 * thicknessTilesI1 + index1 * thicknessTilesI2;
              DesignationData designationData = new DesignationData(tile2i3, sourceData.OriginTargetHeight + thicknessTilesI3, sourceData.PlusXTargetHeight + thicknessTilesI3, sourceData.PlusXyTargetHeight + thicknessTilesI3, sourceData.PlusYTargetHeight + thicknessTilesI3);
              if (manager.IsDesignationAllowed(designationData, out LocStrFormatted _))
              {
                designations.Add(designationData.OriginTile, designationData);
                added(proto, designationData);
              }
            }
          }
        }
      }
    }

    static DesignationDataFactory()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      DesignationDataFactory.SET_BITS_COUNT = new int[16]
      {
        0,
        1,
        1,
        2,
        1,
        2,
        2,
        3,
        1,
        2,
        2,
        3,
        2,
        3,
        3,
        4
      };
    }
  }
}
