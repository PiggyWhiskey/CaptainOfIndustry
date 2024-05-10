// Decompiled with JetBrains decompiler
// Type: Mafi.DebugGameMapDrawing
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Buildings.Mine;
using Mafi.Core.Buildings.Storages;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Transports;
using Mafi.Core.Factory.Zippers;
using Mafi.Core.PathFinding;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.Roads;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Designation;
using Mafi.Core.Terrain.Props;
using Mafi.Core.Terrain.Trees;
using Mafi.Core.Utils;
using Mafi.Core.Vehicles.Excavators;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Curves;
using Mafi.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

#nullable disable
namespace Mafi
{
  public class DebugGameMapDrawing
  {
    private static readonly RelTile1i PADDING;
    private static readonly int MIN_PADDING_PX;
    private static readonly int MAX_PADDING_PX;
    private static readonly ColorRgba TICK_COLOR;
    private static readonly ColorRgba TILE_GRID_COLOR;
    private static readonly ColorRgba STATIC_ENTITY_COLOR;
    private static readonly ColorRgba TRANSPORT_COLOR;
    private static readonly ColorRgba TRANSPORTED_PRODUCT_COLOR;
    private static readonly ColorRgba DYNAMIC_ENTITY_COLOR;
    private static readonly ColorRgba EXCAVATOR_COLOR;
    private static readonly ColorRgba TILE_HEIGHTS_TEXT_COLOR;
    private readonly DependencyResolver m_resolver;
    public readonly Tile2i From;
    private readonly Tile2i m_toExcl;
    public readonly RelTile2i Size;
    private readonly Vector2i m_sizePx;
    private readonly int m_pixelsPerTile;
    private readonly Rgb[] m_rowMajorData;
    private readonly int m_startIndex;
    private readonly int m_paddingPx;

    public Option<DependencyResolver> Resolver => (Option<DependencyResolver>) this.m_resolver;

    public bool IsEnabled => this.m_rowMajorData != null;

    public bool IsNotEnabled => this.m_rowMajorData == null;

    internal DebugGameMapDrawing()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    internal DebugGameMapDrawing(
      Tile2i from,
      RelTile2i size,
      int pixelsPerTile,
      DependencyResolver resolver = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Assert.That<int>(size.X).IsPositive();
      Assert.That<int>(size.Y).IsPositive();
      Assert.That<int>(pixelsPerTile).IsPositive();
      this.From = from;
      this.m_toExcl = from + size;
      this.Size = size;
      this.m_pixelsPerTile = pixelsPerTile;
      this.m_resolver = resolver;
      this.m_paddingPx = (DebugGameMapDrawing.PADDING.Value * this.m_pixelsPerTile).Max(DebugGameMapDrawing.MIN_PADDING_PX).Min(DebugGameMapDrawing.MAX_PADDING_PX);
      this.m_sizePx = size.Vector2i * this.m_pixelsPerTile + 2 * new Vector2i(this.m_paddingPx, this.m_paddingPx);
      this.m_rowMajorData = new Rgb[this.m_sizePx.X * this.m_sizePx.Y];
      this.m_startIndex = this.m_sizePx.X * this.m_paddingPx + this.m_paddingPx;
    }

    public bool IsTileOnMap(Tile2i tile) => tile >= this.From && tile < this.m_toExcl;

    public bool IsTileOnMap(Tile2f tile)
    {
      return tile >= this.From.CornerTile2f && tile < this.m_toExcl.CornerTile2f;
    }

    public bool IsPixelOnMap(Vector2i px)
    {
      return px >= Vector2i.Zero && px < this.Size.Vector2i * this.m_pixelsPerTile;
    }

    public bool IsPixelOnImage(Vector2i px)
    {
      if (px.X >= -this.m_paddingPx && px.Y >= -this.m_paddingPx)
      {
        int x = px.X;
        RelTile2i size = this.Size;
        int num1 = size.Vector2i.X * this.m_pixelsPerTile + this.m_paddingPx;
        if (x < num1)
        {
          int y = px.Y;
          size = this.Size;
          int num2 = size.Vector2i.Y * this.m_pixelsPerTile + this.m_paddingPx;
          return y < num2;
        }
      }
      return false;
    }

    private RelTile2i getMapRelativeTile(Tile2i tile) => tile - this.From;

    private RelTile2f getMapRelativeTile(Tile2f tile) => tile - this.From.CornerTile2f;

    private int getDataIndex(Vector2i pixelCoord)
    {
      return this.m_startIndex + (pixelCoord.Y * this.m_sizePx.X + pixelCoord.X);
    }

    private int getDataIndex(RelTile2i rel)
    {
      Assert.That<RelTile2i>(rel).IsGreaterOrEqual<RelTile2i>(RelTile2i.Zero);
      Assert.That<RelTile2i>(rel).IsLess<RelTile2i>(this.Size);
      return this.m_startIndex + (rel.Y * this.m_sizePx.X + rel.X) * this.m_pixelsPerTile;
    }

    private int getDataIndex(Tile2f tile) => this.getDataIndex(this.getPixelCoord(tile));

    private void fillAreaNoChecks(Vector2i fromPx, Vector2i toPx, ColorRgba color)
    {
      if (color.A == byte.MaxValue)
      {
        Rgb rgb = color.ToRgb();
        for (int y = fromPx.Y; y < toPx.Y; ++y)
        {
          int num = this.m_startIndex + y * this.m_sizePx.X;
          for (int x = fromPx.X; x < toPx.X; ++x)
            this.m_rowMajorData[num + x] = rgb;
        }
      }
      else
      {
        for (int y = fromPx.Y; y < toPx.Y; ++y)
        {
          int num = this.m_startIndex + y * this.m_sizePx.X;
          for (int x = fromPx.X; x < toPx.X; ++x)
          {
            int index = num + x;
            this.m_rowMajorData[index] = this.m_rowMajorData[index].BlendWith(color);
          }
        }
      }
    }

    private void fillTileNoChecks(RelTile2i tile, ColorRgba color)
    {
      int dataIndex = this.getDataIndex(tile);
      for (int index1 = 0; index1 < this.m_pixelsPerTile; ++index1)
      {
        int num = dataIndex + index1 * this.m_sizePx.X;
        for (int index2 = 0; index2 < this.m_pixelsPerTile; ++index2)
        {
          int index3 = num + index2;
          this.m_rowMajorData[index3] = this.m_rowMajorData[index3].BlendWith(color);
        }
      }
    }

    private void fillTileAroundVertexNoChecks(RelTile2i tile, ColorRgba color)
    {
      int dataIndex = this.getDataIndex(tile);
      int num1 = this.m_pixelsPerTile / 2;
      for (int index1 = 0; index1 < this.m_pixelsPerTile; ++index1)
      {
        int num2 = dataIndex + (index1 - num1) * this.m_sizePx.X;
        for (int index2 = 0; index2 < this.m_pixelsPerTile; ++index2)
        {
          int index3 = num2 + (index2 - num1);
          this.m_rowMajorData[index3] = this.m_rowMajorData[index3].BlendWith(color);
        }
      }
    }

    public DebugGameMapDrawing FillTile(Tile2i tile, ColorRgba color)
    {
      if (this.m_rowMajorData == null || !this.IsTileOnMap(tile))
        return this;
      this.fillTileNoChecks(this.getMapRelativeTile(tile), color);
      return this;
    }

    public DebugGameMapDrawing DrawCornerPixel(Tile2i tile, ColorRgba color)
    {
      if (this.m_rowMajorData == null || !this.IsTileOnMap(tile))
        return this;
      int dataIndex = this.getDataIndex(this.getMapRelativeTile(tile));
      this.m_rowMajorData[dataIndex] = this.m_rowMajorData[dataIndex].BlendWith(color);
      return this;
    }

    /// <summary>Returns a pixel coord without padding.</summary>
    private Vector2i getPixelCoord(Tile2f tile)
    {
      return this.getPixelCoord(this.getMapRelativeTile(tile));
    }

    private Vector2i getPixelCoord(RelTile2f rel)
    {
      Fix32 fix32 = rel.X * this.m_pixelsPerTile;
      int intRounded1 = fix32.ToIntRounded();
      fix32 = rel.Y * this.m_pixelsPerTile;
      int intRounded2 = fix32.ToIntRounded();
      return new Vector2i(intRounded1, intRounded2);
    }

    private Vector2i getPixelCoord(Tile2i tile)
    {
      return this.getPixelCoord(this.getMapRelativeTile(tile));
    }

    private Vector2i getPixelCoord(RelTile2i rel) => rel.Vector2i * this.m_pixelsPerTile;

    private void drawPixelNoChecks(Vector2i px, ColorRgba color)
    {
      int dataIndex = this.getDataIndex(px);
      this.m_rowMajorData[dataIndex] = this.m_rowMajorData[dataIndex].BlendWith(color);
    }

    private void drawPixel(Vector2i px, ColorRgba color)
    {
      if (!this.IsPixelOnMap(px))
        return;
      int dataIndex = this.getDataIndex(px);
      this.m_rowMajorData[dataIndex] = this.m_rowMajorData[dataIndex].BlendWith(color);
    }

    public DebugGameMapDrawing DrawPixel(Tile2f tile, ColorRgba color)
    {
      if (this.m_rowMajorData == null || !this.IsTileOnMap(tile.Tile2i))
        return this;
      int dataIndex = this.getDataIndex(tile);
      this.m_rowMajorData[dataIndex] = this.m_rowMajorData[dataIndex].BlendWith(color);
      return this;
    }

    public DebugGameMapDrawing DrawPixels(IEnumerable<KeyValuePair<Tile2f, ColorRgba>> tiles)
    {
      if (this.m_rowMajorData == null)
        return this;
      foreach (KeyValuePair<Tile2f, ColorRgba> tile in tiles)
      {
        if (this.IsTileOnMap(tile.Key.Tile2i))
        {
          int dataIndex = this.getDataIndex(tile.Key);
          this.m_rowMajorData[dataIndex] = this.m_rowMajorData[dataIndex].BlendWith(tile.Value);
        }
      }
      return this;
    }

    public DebugGameMapDrawing DrawPixels(IEnumerable<Tile2f> tiles, ColorRgba color)
    {
      if (this.m_rowMajorData == null)
        return this;
      foreach (Tile2f tile in tiles)
      {
        if (this.IsTileOnMap(tile.Tile2i))
        {
          int dataIndex = this.getDataIndex(tile);
          this.m_rowMajorData[dataIndex] = this.m_rowMajorData[dataIndex].BlendWith(color);
        }
      }
      return this;
    }

    public DebugGameMapDrawing DrawCross(Tile2f tile, ColorRgba color)
    {
      if (this.m_rowMajorData == null || !this.IsTileOnMap(tile.Tile2i))
        return this;
      int dataIndex = this.getDataIndex(tile);
      this.m_rowMajorData[dataIndex - this.m_sizePx.X] = this.m_rowMajorData[dataIndex - this.m_sizePx.X].BlendWith(color);
      this.m_rowMajorData[dataIndex + 1] = this.m_rowMajorData[dataIndex + 1].BlendWith(color);
      this.m_rowMajorData[dataIndex] = this.m_rowMajorData[dataIndex].BlendWith(color);
      this.m_rowMajorData[dataIndex - 1] = this.m_rowMajorData[dataIndex - 1].BlendWith(color);
      this.m_rowMajorData[dataIndex + this.m_sizePx.X] = this.m_rowMajorData[dataIndex + this.m_sizePx.X].BlendWith(color);
      return this;
    }

    public DebugGameMapDrawing DrawCrossAlt(Tile2f tile, ColorRgba color)
    {
      if (this.m_rowMajorData == null || !this.IsTileOnMap(tile.Tile2i))
        return this;
      int dataIndex = this.getDataIndex(tile);
      this.m_rowMajorData[dataIndex - this.m_sizePx.X - 1] = this.m_rowMajorData[dataIndex - this.m_sizePx.X - 1].BlendWith(color);
      this.m_rowMajorData[dataIndex - this.m_sizePx.X + 1] = this.m_rowMajorData[dataIndex - this.m_sizePx.X + 1].BlendWith(color);
      this.m_rowMajorData[dataIndex] = this.m_rowMajorData[dataIndex].BlendWith(color);
      this.m_rowMajorData[dataIndex + this.m_sizePx.X - 1] = this.m_rowMajorData[dataIndex + this.m_sizePx.X - 1].BlendWith(color);
      this.m_rowMajorData[dataIndex + this.m_sizePx.X + 1] = this.m_rowMajorData[dataIndex + this.m_sizePx.X + 1].BlendWith(color);
      return this;
    }

    public DebugGameMapDrawing DrawCross(IEnumerable<Tile2f> tiles, ColorRgba color)
    {
      if (this.m_rowMajorData == null)
        return this;
      foreach (Tile2f tile in tiles)
        this.DrawCross(tile, color);
      return this;
    }

    public DebugGameMapDrawing DrawCross(IEnumerable<KeyValuePair<Tile2f, ColorRgba>> tiles)
    {
      if (this.m_rowMajorData == null)
        return this;
      foreach (KeyValuePair<Tile2f, ColorRgba> tile in tiles)
        this.DrawCross(tile.Key, tile.Value);
      return this;
    }

    public DebugGameMapDrawing DrawCenterPixel(Tile2i tile, ColorRgba color)
    {
      if (this.m_rowMajorData == null || !this.IsTileOnMap(tile))
        return this;
      RelTile2i mapRelativeTile = this.getMapRelativeTile(tile);
      int num = this.m_pixelsPerTile / 2;
      int index = this.getDataIndex(mapRelativeTile) + num * this.m_sizePx.X + num;
      this.m_rowMajorData[index] = this.m_rowMajorData[index].BlendWith(color);
      return this;
    }

    public DebugGameMapDrawing DrawStaticEntity(IStaticEntity entity, ColorRgba color)
    {
      if (this.m_rowMajorData == null)
        return this;
      if (!entity.IsEnabled)
        color = color.SetR((byte) ((int) color.R + 64).Min((int) byte.MaxValue));
      foreach (Tile2i tile in entity.OccupiedTiles.Select<Tile2i>((Func<OccupiedTileRelative, Tile2i>) (x => entity.CenterTile.Xy + x.RelCoord)).Where<Tile2i>(new Func<Tile2i, bool>(this.IsTileOnMap)))
        this.fillTileNoChecks(this.getMapRelativeTile(tile), color);
      string str;
      if (entity is TransportPillar)
        str = "P";
      else if (entity is MiniZipper)
      {
        str = "MZ";
      }
      else
      {
        str = string.Format("{0} #{1}", (object) entity.Prototype.Id.Value, (object) entity.Id.Value);
        if (entity is Storage storage)
          str += string.Format("\n{0}/{1} of {2}", (object) storage.CurrentQuantity, (object) storage.Capacity, (object) (storage.StoredProduct.ValueOrNull?.Id.Value ?? "n/a"));
      }
      this.DrawString(entity.Position2f, str, ColorRgba.White, centered: true);
      return this;
    }

    public DebugGameMapDrawing DrawTransport(Transport transport, ColorRgba color)
    {
      if (this.m_rowMajorData == null)
        return this;
      if (!transport.IsEnabled)
        color = color.SetR((byte) ((int) color.R + 64).Min((int) byte.MaxValue));
      if (!transport.IsMoving)
        color = color.SetB((byte) ((int) color.B + 64).Min((int) byte.MaxValue));
      IEnumerable<Tile3i> tile3is = transport.OccupiedTiles.Select<Tile3i>((Func<OccupiedTileRelative, Tile3i>) (x => transport.CenterTile + x.RelCoord.ExtendZ(x.FromHeightRel.Value))).Where<Tile3i>((Func<Tile3i, bool>) (x => this.IsTileOnMap(x.Xy)));
      ColorRgba color1 = color.SetG((byte) 0);
      foreach (Tile3i tile3i in tile3is)
      {
        this.fillTileNoChecks(this.getMapRelativeTile(tile3i.Xy), color);
        this.DrawString(tile3i.Xy.CenterTile2f, tile3i.Z.ToString(), color1, centered: true);
      }
      for (int index = 1; index < transport.Trajectory.Waypoints.Length; ++index)
        this.DrawLine(transport.Trajectory.Waypoints[index - 1].Position.Xy, transport.Trajectory.Waypoints[index].Position.Xy, color, index > 1);
      ImmutableArray<ProductProto> managedProtos = transport.TransportManager.ProductsManager.SlimIdManager.ManagedProtos;
      foreach (TransportedProductMutable transportedProduct in transport.TransportedProducts)
        this.DrawString(transport.GetProductPose(transportedProduct).Position.Xy, managedProtos[(int) transportedProduct.SlimId.Value].Strings.Name.TranslatedString.SubstringSafe(0, new int?(2)), DebugGameMapDrawing.TRANSPORTED_PRODUCT_COLOR, centered: true);
      return this;
    }

    public DebugGameMapDrawing DrawTerrain(TerrainManager terrainManager = null)
    {
      if (this.m_rowMajorData == null)
        return this;
      TerrainManager terrainManager1 = terrainManager ?? this.m_resolver?.Resolve<TerrainManager>();
      if (terrainManager1 == null)
      {
        this.fillAreaNoChecks(this.getPixelCoord(this.From), this.getPixelCoord(this.m_toExcl), ColorRgba.LightGray);
      }
      else
      {
        RectangleTerrainArea2i terrainBounds = new RectangleTerrainArea2i(this.From, this.Size).ClampToTerrainBounds(terrainManager1);
        Tile2i plusXyCoordExcl = terrainBounds.PlusXyCoordExcl;
        for (int y = terrainBounds.Origin.Y; y < plusXyCoordExcl.Y; ++y)
        {
          for (int x = terrainBounds.Origin.X; x < plusXyCoordExcl.X; ++x)
          {
            Tile2i tile2i = new Tile2i(x, y);
            TerrainTile terrainTile = terrainManager1[tile2i];
            Fix32 fix32;
            ColorRgba colorRgba;
            if (terrainTile.IsOcean)
            {
              ref readonly ColorRgba local = ref ColorRgba.LightGray;
              ColorRgba blue = ColorRgba.Blue;
              fix32 = ((Fix32) 1 - terrainTile.Height.Value) / 10;
              fix32 = fix32.Clamp01();
              Percent percent = fix32.ToPercent();
              colorRgba = local.Lerp(blue, percent);
            }
            else
            {
              TileSurfaceData tileSurfaceData;
              if (terrainManager1.TryGetTileSurface(terrainManager1.GetTileIndex(tile2i), out tileSurfaceData) && tileSurfaceData.IsValid && tileSurfaceData.Height >= terrainManager1.GetHeight(tile2i))
              {
                ColorUniversal dustColor = (ColorUniversal) tileSurfaceData.ResolveToProto(terrainManager1).Graphics.DustColor;
                Fix32 h = dustColor.H;
                Fix32 s = dustColor.S / 3;
                fix32 = dustColor.L + (terrainTile.Height.Value + (Fix32) 3) / 30;
                Fix32 l = fix32.Clamp01();
                colorRgba = ColorRgba.FromHsl(h, s, l);
              }
              else
              {
                ColorUniversal color = terrainTile.FirstLayer.Material.Graphics.Color;
                Fix32 h = color.H;
                Fix32 s = color.S / 3;
                fix32 = color.L + (terrainTile.Height.Value + (Fix32) 3) / 30;
                Fix32 l = fix32.Clamp01();
                colorRgba = ColorRgba.FromHsl(h, s, l);
              }
            }
            this.fillTileNoChecks(this.getMapRelativeTile(tile2i), colorRgba.SetA(byte.MaxValue));
          }
        }
      }
      this.DrawGrid(1, DebugGameMapDrawing.TILE_GRID_COLOR);
      return this;
    }

    public DebugGameMapDrawing DrawGrid(int spacing, ColorRgba color)
    {
      for (int x = 0; x < this.Size.X; x += spacing)
        this.drawLineNoChecks(new RelTile2i(x, this.Size.Y), new RelTile2i(x, 0), color);
      for (int y = 0; y < this.Size.Y; y += spacing)
        this.drawLineNoChecks(new RelTile2i(this.Size.X, y), new RelTile2i(0, y), color);
      return this;
    }

    public DebugGameMapDrawing DrawTrees(TreesManager treeManager = null)
    {
      if (this.m_rowMajorData == null)
        return this;
      ColorRgba colorRgba = new ColorRgba(0, 128, 0, (int) byte.MaxValue);
      if (treeManager == null)
        treeManager = this.m_resolver.Resolve<TreesManager>();
      foreach (KeyValuePair<TreeId, TreeData> tree in (IEnumerable<KeyValuePair<TreeId, TreeData>>) treeManager.Trees)
      {
        if (this.IsTileOnMap((Tile2i) tree.Key.Position))
        {
          Vector2i vector2i1 = this.getPixelCoord((Tile2i) tree.Key.Position) + 1;
          Vector2i vector2i2 = vector2i1.AddXy(this.m_pixelsPerTile) - 2;
          Vector2i pixelCoord = this.getPixelCoord(tree.Key.Position.CenterTile2f);
          ColorRgba color = colorRgba;
          if (treeManager.IsTreeSelected(tree.Key))
            color = color.SetG(byte.MaxValue);
          if (treeManager.IsTreeReserved(tree.Key))
            color = color.SetR((byte) 128);
          this.drawLineNoChecks(new Vector2i(vector2i1.X, pixelCoord.Y), new Vector2i(vector2i2.X, pixelCoord.Y), color);
          this.drawLineNoChecks(new Vector2i(pixelCoord.X, vector2i1.Y), new Vector2i(pixelCoord.X, vector2i2.Y), color);
          this.drawLineNoChecks(new Vector2i(vector2i1.X + 1, vector2i1.Y + 1), new Vector2i(vector2i2.X - 1, vector2i2.Y - 1), color);
          this.drawLineNoChecks(new Vector2i(vector2i1.X + 1, vector2i2.Y - 1), new Vector2i(vector2i2.X - 1, vector2i1.Y + 1), color);
        }
      }
      return this;
    }

    public DebugGameMapDrawing DrawProps(TerrainPropsManager propsManager = null)
    {
      if (this.m_rowMajorData == null)
        return this;
      ColorRgba colorRgba = new ColorRgba(0, 0, 128, (int) byte.MaxValue);
      if (propsManager == null)
        propsManager = this.m_resolver.Resolve<TerrainPropsManager>();
      foreach (KeyValuePair<TerrainPropId, TerrainPropData> terrainProp in (IEnumerable<KeyValuePair<TerrainPropId, TerrainPropData>>) propsManager.TerrainProps)
      {
        if (this.IsTileOnMap((Tile2i) terrainProp.Key.Position))
        {
          Vector2i vector2i1 = this.getPixelCoord((Tile2i) terrainProp.Key.Position) + 1;
          Vector2i vector2i2 = vector2i1.AddXy(this.m_pixelsPerTile) - 2;
          Vector2i pixelCoord = this.getPixelCoord(terrainProp.Key.Position.CenterTile2f);
          ColorRgba color = colorRgba;
          this.drawLineNoChecks(new Vector2i(vector2i1.X, pixelCoord.Y), new Vector2i(vector2i2.X, pixelCoord.Y), color);
          this.drawLineNoChecks(new Vector2i(pixelCoord.X, vector2i1.Y), new Vector2i(pixelCoord.X, vector2i2.Y), color);
          this.drawLineNoChecks(new Vector2i(vector2i1.X + 1, vector2i1.Y + 1), new Vector2i(vector2i2.X - 1, vector2i2.Y - 1), color);
          this.drawLineNoChecks(new Vector2i(vector2i1.X + 1, vector2i2.Y - 1), new Vector2i(vector2i2.X - 1, vector2i1.Y + 1), color);
        }
      }
      return this;
    }

    public DebugGameMapDrawing DrawAllStaticEntities(IEntitiesManager entitiesManager = null)
    {
      if (this.m_rowMajorData == null)
        return this;
      foreach (IEntity entity1 in (entitiesManager ?? this.m_resolver.Resolve<IEntitiesManager>()).Entities)
      {
        if (entity1 is IStaticEntity entity2)
        {
          switch (entity2)
          {
            case Transport transport:
              this.DrawTransport(transport, DebugGameMapDrawing.TRANSPORT_COLOR);
              continue;
            case IRoadGraphEntity roadEntity:
              ColorRgba? lanesColor = new ColorRgba?();
              this.DrawRoadEntity(roadEntity, lanesColor);
              continue;
            default:
              this.DrawStaticEntity(entity2, DebugGameMapDrawing.STATIC_ENTITY_COLOR);
              continue;
          }
        }
      }
      return this;
    }

    public DebugGameMapDrawing DrawAllPorts(IoPortsManager portsManager = null)
    {
      if (this.m_rowMajorData == null)
        return this;
      foreach (IoPort port in (portsManager ?? this.m_resolver.Resolve<IoPortsManager>()).Ports)
        this.DrawPort(port);
      return this;
    }

    public DebugGameMapDrawing DrawPort(IoPort port)
    {
      if (this.m_rowMajorData == null)
        return this;
      Tile3i position = port.Position;
      Vector2i pixelCoord = this.getPixelCoord(position.Tile2i.CenterTile2f);
      Vector3i directionVector = port.Direction.DirectionVector;
      ColorRgba color = (port.Type == IoPortType.Input ? ColorRgba.Green : (port.Type == IoPortType.Output ? ColorRgba.Red : ColorRgba.Yellow)).SetA((byte) 128);
      this.drawLine(pixelCoord, pixelCoord + directionVector.Xy * this.m_pixelsPerTile / 2, color);
      position = port.Position;
      this.DrawString(position.Tile2i.CenterTile2f, port.Name.ToString(), color, centered: true);
      return this;
    }

    public DebugGameMapDrawing DrawAllDynamicEntities(IEntitiesManager entitiesManager = null)
    {
      if (this.m_rowMajorData == null)
        return this;
      foreach (IEntity entity in (entitiesManager ?? this.m_resolver.Resolve<IEntitiesManager>()).Entities)
      {
        if (entity is DynamicGroundEntity dynamicEntity)
          this.DrawDynamicEntity(dynamicEntity, dynamicEntity is Excavator ? DebugGameMapDrawing.EXCAVATOR_COLOR : DebugGameMapDrawing.DYNAMIC_ENTITY_COLOR);
      }
      return this;
    }

    public DebugGameMapDrawing DrawDynamicEntity(DynamicGroundEntity dynamicEntity, ColorRgba color)
    {
      if (this.m_rowMajorData == null)
        return this;
      Tile2f position2f1 = dynamicEntity.Position2f;
      RelTile3f entitySize = dynamicEntity.Prototype.EntitySize;
      RelTile2f relTile2f1 = entitySize.Xy / (Fix32) 2;
      if (this.IsTileOnMap(position2f1 - relTile2f1))
      {
        Tile2f position2f2 = dynamicEntity.Position2f;
        entitySize = dynamicEntity.Prototype.EntitySize;
        RelTile2f relTile2f2 = entitySize.Xy / (Fix32) 2;
        if (this.IsTileOnMap(position2f2 + relTile2f2))
        {
          DynamicGroundEntityProto.Gfx graphics = dynamicEntity.Prototype.Graphics;
          AngleDegrees1f direction = dynamicEntity.Direction;
          Tile2f position2f3 = dynamicEntity.Position2f;
          Tile2f tile2f1 = position2f3 + graphics.FrontContactPtsOffset.Rotate(direction);
          Tile2f tile2f2 = position2f3 + graphics.FrontContactPtsOffset.MultiplyY((Fix32) -1).Rotate(direction);
          Tile2f tile2f3 = position2f3 - graphics.RearContactPtsOffset.MultiplyY((Fix32) -1).Rotate(direction);
          Tile2f tile2f4 = position2f3 - graphics.RearContactPtsOffset.Rotate(direction);
          this.DrawLine(tile2f1, position2f3, color, true);
          this.DrawLine(tile2f2, position2f3, color, true);
          this.DrawLine(tile2f1, tile2f2, color, true);
          this.DrawLine(tile2f2, tile2f4, color, true);
          this.DrawLine(tile2f4, tile2f3, color, true);
          this.DrawLine(tile2f3, tile2f1, color, true);
          if (dynamicEntity is DrivingEntity drivingEntity && drivingEntity.Target.HasValue)
            this.DrawLine(position2f3, drivingEntity.Target.Value, ColorRgba.Green);
          if (dynamicEntity is PathFindingEntity pathFindingEntity && pathFindingEntity.UnreachableGoal.HasValue)
            this.DrawLine(position2f3, pathFindingEntity.UnreachableGoal.Value.GetGoalPosition().Xy, ColorRgba.Red.SetA((byte) 127));
          string str = "";
          if (dynamicEntity is Vehicle vehicle)
          {
            str = "\n" + (vehicle.CurrentJob.ValueOrNull?.GetType().Name ?? "no job");
            foreach (IDesignation designation1 in (IEnumerable<IDesignation>) this.m_resolver.Resolve<UnreachableTerrainDesignationsManager>().GetUnreachableDesignationsFor((IPathFindingVehicle) vehicle))
            {
              if (designation1 is TerrainDesignation designation2)
              {
                this.DrawDesignation(designation2, designation2.Prototype.Graphics.ColorCanBeFulfilled.Lerp(ColorRgba.Red, 50.Percent()));
                this.DrawLine(vehicle.Position2f, designation2.CenterTileCoord.CenterTile2f, ColorRgba.Red.SetA((byte) 128));
              }
            }
            if (vehicle is Truck truck)
              str += string.Format("\n{0}", (object) truck.Cargo);
          }
          this.DrawString(dynamicEntity.Position2f, string.Format("{0} #{1}{2}", (object) dynamicEntity.Prototype.Id.Value, (object) dynamicEntity.Id.Value, (object) str), color.SetA((byte) (((int) color.A + (int) sbyte.MaxValue) / 2)), centered: true);
          return this;
        }
      }
      return this;
    }

    public DebugGameMapDrawing DrawLine(
      Tile2i from,
      Tile2i to,
      ColorRgba color,
      bool skipFirstPixel = false)
    {
      if (this.m_rowMajorData == null)
        return this;
      this.drawLine(this.getPixelCoord(from), this.getPixelCoord(to), color, skipFirstPixel);
      return this;
    }

    public DebugGameMapDrawing DrawLine(
      Tile2f fromTile,
      Tile2f toTile,
      ColorRgba color,
      bool skipFirstPixel = false)
    {
      if (this.m_rowMajorData == null)
        return this;
      if (this.IsTileOnMap(fromTile) && this.IsTileOnMap(toTile))
        this.drawLineNoChecks(this.getPixelCoord(fromTile), this.getPixelCoord(toTile), color, skipFirstPixel);
      else
        this.drawLine(this.getPixelCoord(fromTile), this.getPixelCoord(toTile), color, skipFirstPixel);
      return this;
    }

    public DebugGameMapDrawing DrawLine(IEnumerable<Tile2f> points, ColorRgba color)
    {
      if (this.m_rowMajorData == null)
        return this;
      Vector2i? nullable = new Vector2i?();
      bool skipFirstPoint = false;
      foreach (Tile2f point in points)
      {
        if (!nullable.HasValue)
        {
          nullable = new Vector2i?(this.getPixelCoord(point));
        }
        else
        {
          Vector2i pixelCoord = this.getPixelCoord(point);
          if (this.IsPixelOnMap(nullable.Value) && this.IsPixelOnMap(pixelCoord))
            this.drawLineNoChecks(nullable.Value, pixelCoord, color, skipFirstPoint);
          else
            this.drawLine(nullable.Value, pixelCoord, color, skipFirstPoint);
          nullable = new Vector2i?(pixelCoord);
          skipFirstPoint = true;
        }
      }
      return this;
    }

    public DebugGameMapDrawing DrawPolygon(IEnumerable<Tile2f> points, ColorRgba color)
    {
      if (this.m_rowMajorData == null)
        return this;
      Vector2i to = new Vector2i();
      Vector2i? nullable = new Vector2i?();
      foreach (Tile2f point in points)
      {
        if (!nullable.HasValue)
        {
          nullable = new Vector2i?(to = this.getPixelCoord(point));
        }
        else
        {
          Vector2i pixelCoord = this.getPixelCoord(point);
          if (this.IsPixelOnMap(nullable.Value) && this.IsPixelOnMap(pixelCoord))
            this.drawLineNoChecks(nullable.Value, pixelCoord, color, true);
          else
            this.drawLine(nullable.Value, pixelCoord, color, true);
          nullable = new Vector2i?(pixelCoord);
        }
      }
      if (nullable.HasValue)
        this.drawLine(nullable.Value, to, color, true);
      return this;
    }

    public DebugGameMapDrawing DrawArrow(
      Tile2f from,
      Tile2f to,
      ColorRgba color,
      RelTile1f headSize)
    {
      if (this.m_rowMajorData == null)
        return this;
      Vector2i pixelCoord1 = this.getPixelCoord(to);
      RelTile2f relTile2f1 = to - from;
      if (relTile2f1 == RelTile2f.Zero)
        relTile2f1 = RelTile2f.UnitX;
      RelTile2f relTile2f2 = relTile2f1.OfLength(headSize.Value);
      Tile2f tile = to - relTile2f2;
      Vector2i pixelCoord2 = this.getPixelCoord(tile + relTile2f2.LeftOrthogonalVector / (Fix32) 2);
      Vector2i pixelCoord3 = this.getPixelCoord(tile + relTile2f2.RightOrthogonalVector / (Fix32) 2);
      this.drawLine(this.getPixelCoord(tile), this.getPixelCoord(from), color);
      this.drawLine(pixelCoord1, pixelCoord2, color, true);
      this.drawLine(pixelCoord2, pixelCoord3, color, true);
      this.drawLine(pixelCoord3, pixelCoord1, color, true);
      return this;
    }

    private void drawLineNoChecks(Tile2f from, Tile2f to, ColorRgba color, bool skipFirstPoint = false)
    {
      this.drawLineNoChecks(this.getPixelCoord(from), this.getPixelCoord(to), color, skipFirstPoint);
    }

    private void drawLineNoChecks(
      RelTile2i from,
      RelTile2i to,
      ColorRgba color,
      bool skipFirstPoint = false)
    {
      this.drawLineNoChecks(this.getPixelCoord(from), this.getPixelCoord(to), color, skipFirstPoint);
    }

    private void drawLineNoChecks(
      Vector2i from,
      Vector2i to,
      ColorRgba color,
      bool skipFirstPoint = false)
    {
      foreach (Vector2i px in new LineRasterizer(from, to, skipFirstPoint))
        this.drawPixelNoChecks(px, color);
    }

    private void drawLine(Vector2i from, Vector2i to, ColorRgba color, bool skipFirstPoint = false)
    {
      foreach (Vector2i px in new LineRasterizer(from, to, skipFirstPoint))
      {
        if (this.IsPixelOnMap(px))
          this.drawPixelNoChecks(px, color);
      }
    }

    public DebugGameMapDrawing DrawCircle(Tile2f center, RelTile1i radius, ColorRgba color)
    {
      Vector2i c = this.getPixelCoord(center);
      MafiMath.IterateCirclePoints(this.m_pixelsPerTile * radius.Value, (Action<int, int>) ((dx, dy) => this.drawPixel(c + new Vector2i(dx, dy), color)));
      return this;
    }

    public DebugGameMapDrawing DrawCircle(Tile2f center, RelTile1f radius, ColorRgba color)
    {
      Vector2i c = this.getPixelCoord(center);
      MafiMath.IterateCirclePoints((this.m_pixelsPerTile * radius.Value).ToIntRounded(), (Action<int, int>) ((dx, dy) => this.drawPixel(c + new Vector2i(dx, dy), color)));
      return this;
    }

    public DebugGameMapDrawing DrawCurve(
      CubicBezierCurve3f curve,
      ColorRgba color,
      RelTile1f ctrlPtRadius = default (RelTile1f))
    {
      for (int index = 0; index < curve.SegmentsCount; ++index)
      {
        this.DrawLine(new Tile2f(curve.ControlPoints[index].Xy), new Tile2f(curve.ControlPoints[index + 1].Xy), color);
        this.DrawLine(new Tile2f(curve.ControlPoints[index + 3].Xy), new Tile2f(curve.ControlPoints[index + 2].Xy), color);
      }
      foreach (Vector3f controlPoint in curve.ControlPoints)
        this.DrawCircle(new Tile2f(controlPoint.Xy), ctrlPtRadius == new RelTile1f() ? RelTile1f.Half : ctrlPtRadius, color);
      return this;
    }

    internal DebugGameMapDrawing DrawPathabilityOverlay(
      ClearancePathabilityProvider clearancePathabilityProvider = null)
    {
      if (this.m_rowMajorData == null)
        return this;
      ClearancePathabilityProvider pathabilityProvider = clearancePathabilityProvider ?? this.m_resolver.Resolve<ClearancePathabilityProvider>();
      Tile2i tile2i1 = this.From - 2 * new RelTile2i(8, 8);
      Tile2i tile2i2 = this.m_toExcl + new RelTile2i(8, 8);
      foreach (ClearancePathabilityProvider.DataChunk dataChunk in pathabilityProvider.Chunks.Values)
      {
        if (dataChunk.OriginTile.TileCoordSlim >= tile2i1 && dataChunk.OriginTile.TileCoordSlim < tile2i2)
        {
          for (int y = 0; y < 8; ++y)
          {
            for (int x = 0; x < 8; ++x)
            {
              Tile2i tile = dataChunk.OriginTile.TileCoordSlim.AsFull + new RelTile2i(x, y);
              if (this.IsTileOnMap(tile))
              {
                string debugStr = dataChunk.RawDataToDebugStr(x, y);
                this.drawStringNoChecks(this.getPixelCoord(tile), debugStr, new ColorRgba((int) byte.MaxValue, (int) byte.MaxValue, 0, 96));
              }
            }
          }
        }
      }
      return this;
    }

    internal DebugGameMapDrawing DrawPathabilityOverlayFor(
      PathFindingEntity vehicle,
      ClearancePathabilityProvider clearancePathabilityProvider = null)
    {
      return this.DrawPathabilityOverlayFor(vehicle.PathFindingParams, clearancePathabilityProvider);
    }

    internal DebugGameMapDrawing DrawPathabilityOverlayFor(
      VehiclePathFindingParams pfParams,
      ClearancePathabilityProvider clearancePathabilityProvider = null)
    {
      if (this.m_rowMajorData == null)
        return this;
      ClearancePathabilityProvider clearancePathabilityProvider1 = clearancePathabilityProvider ?? this.m_resolver.Resolve<ClearancePathabilityProvider>();
      RelTile1i requiredClearance = pfParams.RequiredClearance;
      ulong pathabilityMaskRaw = ClearancePathabilityProvider.GetPathabilityMaskRaw(requiredClearance, pfParams.SteepnessPathability, pfParams.HeightClearancePathability);
      Tile2i tile2i1 = this.From - 2 * new RelTile2i(8, 8);
      Tile2i tile2i2 = this.m_toExcl + new RelTile2i(8, 8);
      bool flag = requiredClearance.Value % 2 == 0;
      RelTile2i relTile2i = new RelTile2i(requiredClearance / 2, requiredClearance / 2);
      RelTile2f relTile2f = new RelTile2f(pfParams.RequiredClearance.Value.Over(2), pfParams.RequiredClearance.Value.Over(2)).AddXy(-Fix32.Half);
      int pathabilityClassIndex = clearancePathabilityProvider1.GetPathabilityClassIndex(pfParams.PathabilityQueryMask);
      TerrainManager terrainManager = clearancePathabilityProvider1.TerrainManager;
      foreach (ClearancePathabilityProvider.DataChunk dataChunk in clearancePathabilityProvider1.Chunks.Values)
      {
        if (dataChunk.AllNeighborsEnsured && dataChunk.OriginTile.TileCoordSlim >= tile2i1 && dataChunk.OriginTile.TileCoordSlim < tile2i2)
        {
          int b = dataChunk.IsDirty ? 128 : 0;
          for (int y = 0; y < 8; ++y)
          {
            if (dataChunk.OriginTile.TileCoordSlim.Y >= (ushort) 8 && (int) dataChunk.OriginTile.TileCoordSlim.Y + 8 < terrainManager.TerrainHeight)
            {
              for (int x = 0; x < 8; ++x)
              {
                if (dataChunk.OriginTile.TileCoordSlim.X >= (ushort) 8 && (int) dataChunk.OriginTile.TileCoordSlim.X + 8 < terrainManager.TerrainWidth)
                {
                  Tile2i tile = dataChunk.OriginTile.TileCoordSlim.AsFull + new RelTile2i(x, y) + relTile2i;
                  if (this.IsTileOnMap(tile))
                  {
                    ColorRgba color = dataChunk.IsPathable(new RelTile2i(x, y), requiredClearance, pathabilityMaskRaw) ? new ColorRgba(0, (int) byte.MaxValue, b, 32) : new ColorRgba((int) byte.MaxValue, 0, b, 32);
                    if (flag)
                      this.fillTileAroundVertexNoChecks(this.getMapRelativeTile(tile), color);
                    else
                      this.fillTileNoChecks(this.getMapRelativeTile(tile), color);
                  }
                }
              }
            }
          }
          Option<ClearancePathabilityProvider.CapabilityChunkData> pfData = dataChunk.GetPfData(pathabilityClassIndex);
          if (pfData.HasValue)
          {
            ColorRgba color1 = pfData.Value.IsDirty ? new ColorRgba(32, 64, 64, 64) : new ColorRgba(0, 64, 192, 64);
            ColorRgba colorRgba1 = new ColorRgba(0, (int) byte.MaxValue, (int) byte.MaxValue, 64);
            ColorRgba colorRgba2 = new ColorRgba(64, 128, 128, 64);
            ColorRgba colorRgba3 = new ColorRgba((int) byte.MaxValue, 64, 64, 64);
            foreach (PfNode node in pfData.Value.Nodes)
            {
              this.drawArea(node.Area.Origin.CornerTile2f + relTile2f, node.Area.Size.RelTile2f, color1, -1);
              foreach (PfNode.Edge currentNeighbor in node.CurrentNeighbors)
              {
                ColorRgba color2 = node.IsDirty || currentNeighbor.Node.IsDirty ? colorRgba2 : colorRgba1;
                this.DrawLine(node.Area.CenterCoordF + relTile2f, currentNeighbor.Node.Area.CenterCoordF + relTile2f, color2);
              }
            }
          }
        }
      }
      this.DrawPathabilityOverlay(clearancePathabilityProvider1);
      return this;
    }

    private void drawArea(Tile2f origin, RelTile2f size, ColorRgba color, int offsetPx = 0)
    {
      Vector2i vector2i1 = this.getPixelCoord(origin).AddXy(-offsetPx);
      Vector2i vector2i2 = this.getPixelCoord(origin + size).AddXy(offsetPx);
      if (!this.IsPixelOnImage(vector2i1) || !this.IsPixelOnImage(vector2i2))
        return;
      this.drawLineNoChecks(vector2i1, new Vector2i(vector2i2.X, vector2i1.Y), color, true);
      this.drawLineNoChecks(new Vector2i(vector2i2.X, vector2i1.Y), vector2i2, color, true);
      this.drawLineNoChecks(vector2i2, new Vector2i(vector2i1.X, vector2i2.Y), color, true);
      this.drawLineNoChecks(new Vector2i(vector2i1.X, vector2i2.Y), vector2i1, color, true);
    }

    public DebugGameMapDrawing DrawAllDesignations(
      TerrainDesignationsManager terrainDesignationsManager = null)
    {
      if (this.m_rowMajorData == null)
        return this;
      foreach (TerrainDesignation designation in (IEnumerable<TerrainDesignation>) (terrainDesignationsManager ?? this.m_resolver.Resolve<TerrainDesignationsManager>()).Designations)
        this.DrawDesignation(designation);
      foreach (MineTower mineTower in this.m_resolver.Resolve<EntitiesManager>().GetAllEntitiesOfType<MineTower>())
      {
        this.DrawArea(mineTower.Area, ColorRgba.DarkYellow);
        this.DrawString(mineTower.Area.PlusYCoordExcl.CornerTile2f, string.Format("{0} #{1}", (object) mineTower.Prototype.Id, (object) mineTower.Id), ColorRgba.DarkYellow);
      }
      return this;
    }

    public DebugGameMapDrawing DrawDesignations(
      IEnumerable<IDesignation> designations,
      ColorRgba colorOverride = default (ColorRgba))
    {
      foreach (IDesignation designation1 in designations)
      {
        if (designation1 is TerrainDesignation designation2)
          this.DrawDesignation(designation2, colorOverride);
      }
      return this;
    }

    public DebugGameMapDrawing DrawDesignation(
      TerrainDesignation designation,
      ColorRgba colorOverride = default (ColorRgba))
    {
      if (this.m_rowMajorData == null)
        return this;
      ColorRgba colorNormal = (colorOverride.IsEmpty ? designation.Prototype.Graphics.ColorCanBeFulfilled : colorOverride).SetA((byte) 192);
      ColorRgba colorFulfilled = (colorOverride.IsEmpty ? designation.Prototype.Graphics.ColorIsFulfilled : colorOverride).Lerp(ColorRgba.Gray, Percent.Fifty).SetA((byte) 128);
      ColorRgba colorWarning = new ColorRgba((int) byte.MaxValue, 0, 0, 128);
      Vector2i pixelCoord = this.getPixelCoord(designation.OriginTileCoord);
      Vector2i vector2i1 = this.getPixelCoord(designation.PlusXTileCoord).AddX(-1);
      Vector2i vector2i2 = this.getPixelCoord(designation.PlusXyTileCoord).AddXy(-1);
      Vector2i vector2i3 = this.getPixelCoord(designation.PlusYTileCoord).AddY(-1);
      drawDesignationLine(pixelCoord, vector2i1, NeighborCoord.MinusY);
      drawDesignationLine(vector2i1, vector2i2, NeighborCoord.PlusX);
      drawDesignationLine(vector2i2, vector2i3, NeighborCoord.PlusY);
      drawDesignationLine(vector2i3, pixelCoord, NeighborCoord.MinusX);
      ColorRgba color = designation.IsFulfilled ? colorFulfilled : colorNormal;
      string str = designation.Prototype.Id.Value.EndsWith("Designator") ? designation.Prototype.Id.Value.WithoutSuffix("Designator".Length) : designation.Prototype.Id.Value;
      this.DrawString(designation.CenterTileCoord.CornerTile2f, str, color, centered: true);
      this.DrawString(designation.OriginTileCoord.CenterTile2f, designation.GetTargetHeightAt(new RelTile2i(0, 0)).ToString(), color, centered: true);
      this.DrawString(designation.PlusXTileCoord.DecrementX.CenterTile2f, designation.GetTargetHeightAt(new RelTile2i(4, 0)).ToString(), color, centered: true);
      Tile2i tile2i = designation.PlusXyTileCoord;
      tile2i = tile2i.AddXy(-1);
      this.DrawString(tile2i.CenterTile2f, designation.GetTargetHeightAt(new RelTile2i(4, 4)).ToString(), color, centered: true);
      tile2i = designation.PlusYTileCoord;
      tile2i = tile2i.DecrementY;
      this.DrawString(tile2i.CenterTile2f, designation.GetTargetHeightAt(new RelTile2i(0, 4)).ToString(), color, centered: true);
      return this;

      void drawDesignationLine(Vector2i fromPx, Vector2i toPx, NeighborCoord towards)
      {
        ColorRgba color = designation.DisplayWarningTowards(towards) ? colorWarning : (designation.IsFulfilledTowards(towards) ? colorFulfilled : colorNormal);
        if (this.IsPixelOnImage(fromPx) && this.IsPixelOnImage(toPx))
          this.drawLineNoChecks(fromPx, toPx, color);
        else
          this.drawLine(fromPx, toPx, color);
      }
    }

    public DebugGameMapDrawing DrawTilesTicks()
    {
      if (this.m_rowMajorData == null)
        return this;
      Tile2i tile2i = this.From + this.Size;
      if (this.m_pixelsPerTile <= 6)
      {
        for (int multipleOf = this.From.X.CeilToMultipleOf(10); multipleOf < tile2i.X; multipleOf += 10)
        {
          string stringCached = multipleOf.ToStringCached();
          this.drawString(this.getPixelCoord(new Tile2i(multipleOf, this.From.Y)).AddY(-7), stringCached, DebugGameMapDrawing.TICK_COLOR, true);
        }
        for (int multipleOf = this.From.Y.CeilToMultipleOf(10); multipleOf < tile2i.Y; multipleOf += 10)
        {
          string stringCached = multipleOf.ToStringCached();
          this.drawString(this.getPixelCoord(new Tile2i(this.From.X, multipleOf)).AddX(-4 * stringCached.Length - 2), stringCached, DebugGameMapDrawing.TICK_COLOR);
        }
      }
      else
      {
        for (int x = this.From.X; x < tile2i.X; ++x)
        {
          int num = x % 10;
          string str = num == 0 ? x.ToStringCached() : num.ToStringCached();
          this.drawString(this.getPixelCoord(new Tile2i(x, this.From.Y)).AddY(-7), str, DebugGameMapDrawing.TICK_COLOR, true);
        }
        for (int y = this.From.Y; y < tile2i.Y; ++y)
        {
          int num = y % 10;
          string str = num == 0 ? y.ToStringCached() : num.ToStringCached();
          this.drawString(this.getPixelCoord(new Tile2i(this.From.X, y)).AddX(-4 * str.Length - 2), str, DebugGameMapDrawing.TICK_COLOR);
        }
      }
      return this;
    }

    public DebugGameMapDrawing DrawString(
      Tile2f position,
      string str,
      ColorRgba color,
      bool vertical = false,
      bool centered = false)
    {
      this.drawString(this.getPixelCoord(position), str, color, vertical, centered);
      return this;
    }

    public DebugGameMapDrawing DrawString(
      RelTile2f position,
      string str,
      ColorRgba color,
      bool vertical = false,
      bool centered = false,
      int? maxLines = null)
    {
      this.drawString(this.getPixelCoord(position), str, color, vertical, centered, maxLines);
      return this;
    }

    public DebugGameMapDrawing DrawStringTopLeft(string str, ColorRgba color, int? maxLines = null)
    {
      this.drawString(this.getPixelCoord(new RelTile2i(0, this.Size.Y - 2)), str, color, maxLines: maxLines);
      return this;
    }

    private void drawString(
      Vector2i pxPosition,
      string str,
      ColorRgba color,
      bool vertical = false,
      bool centered = false,
      int? maxLines = null)
    {
      if (string.IsNullOrEmpty(str))
        return;
      BitmapFont5Px.DrawString(str, new Action<Vector2i>(setPixel), vertical, centered, maxLines);

      void setPixel(Vector2i px)
      {
        px += pxPosition;
        if (!this.IsPixelOnImage(px))
          return;
        int dataIndex = this.getDataIndex(px);
        this.m_rowMajorData[dataIndex] = this.m_rowMajorData[dataIndex].BlendWith(color);
      }
    }

    public DebugGameMapDrawing DrawStringNoChecks(
      RelTile2f position,
      string str,
      ColorRgba color,
      bool vertical = false,
      int? maxLines = null)
    {
      this.drawStringNoChecks(this.getPixelCoord(position), str, color, vertical);
      return this;
    }

    private void drawStringNoChecks(
      Vector2i pxPosition,
      string str,
      ColorRgba color,
      bool vertical = false)
    {
      if (string.IsNullOrEmpty(str))
        return;
      BitmapFont5Px.DrawString(str, new Action<Vector2i>(setPixel), vertical);

      void setPixel(Vector2i px)
      {
        int dataIndex = this.getDataIndex(px + pxPosition);
        this.m_rowMajorData[dataIndex] = this.m_rowMajorData[dataIndex].BlendWith(color);
      }
    }

    public DebugGameMapDrawing DrawAllTileHeights(
      TerrainManager terrainManager = null,
      StaticEntityOceanReservationManager oceanManager = null)
    {
      if (this.m_rowMajorData == null)
        return this;
      if (terrainManager == null)
        terrainManager = this.m_resolver.Resolve<TerrainManager>();
      if (oceanManager == null)
        this.m_resolver.TryResolve<StaticEntityOceanReservationManager>(out oceanManager);
      foreach (Tile2iAndIndex enumerateTilesAndIndex in new RectangleTerrainArea2i(this.From, this.Size).ClampToTerrainBounds(terrainManager).EnumerateTilesAndIndices(terrainManager))
      {
        ColorRgba color = DebugGameMapDrawing.TILE_HEIGHTS_TEXT_COLOR;
        int num = (terrainManager.GetHeight(enumerateTilesAndIndex.Index).Value * 10).IntegerPart;
        if (num < 0)
        {
          num = -num;
          color = color.SetR((byte) 128);
        }
        string str = (num % 100).ToString("00");
        if (terrainManager.IsOcean(enumerateTilesAndIndex.Index))
        {
          color = color.SetB(byte.MaxValue).SetR((byte) 0);
          if (oceanManager != null && !oceanManager.IsTileValid(enumerateTilesAndIndex.Index))
            this.HighlightTile(enumerateTilesAndIndex.TileCoord, new ColorRgba((int) byte.MaxValue, 0, 0, 32));
        }
        this.drawStringNoChecks(this.getPixelCoord(enumerateTilesAndIndex.TileCoord), str, color);
      }
      return this;
    }

    /// <summary>Draws a square around all given tiles.</summary>
    public DebugGameMapDrawing HighlightTiles(IEnumerable<Tile2i> tiles, ColorRgba color)
    {
      if (this.m_rowMajorData == null)
        return this;
      foreach (Tile2i tile in tiles)
      {
        if (this.IsTileOnMap(tile))
        {
          int num = this.m_pixelsPerTile - 1;
          Vector2i pixelCoord = this.getPixelCoord(tile);
          this.drawLineNoChecks(pixelCoord, pixelCoord.AddX(num), color, true);
          this.drawLineNoChecks(pixelCoord.AddX(num), pixelCoord.AddXy(num), color, true);
          this.drawLineNoChecks(pixelCoord.AddXy(num), pixelCoord.AddY(num), color, true);
          this.drawLineNoChecks(pixelCoord.AddY(num), pixelCoord, color, true);
        }
      }
      return this;
    }

    public DebugGameMapDrawing HighlightTiles(IEnumerable<Tile2iSlim> tiles, ColorRgba color)
    {
      if (this.m_rowMajorData == null)
        return this;
      foreach (Tile2iSlim tile1 in tiles)
      {
        Tile2i tile2 = (Tile2i) tile1;
        if (this.IsTileOnMap(tile2))
        {
          int num = this.m_pixelsPerTile - 1;
          Vector2i pixelCoord = this.getPixelCoord(tile2);
          this.drawLineNoChecks(pixelCoord, pixelCoord.AddX(num), color, true);
          this.drawLineNoChecks(pixelCoord.AddX(num), pixelCoord.AddXy(num), color, true);
          this.drawLineNoChecks(pixelCoord.AddXy(num), pixelCoord.AddY(num), color, true);
          this.drawLineNoChecks(pixelCoord.AddY(num), pixelCoord, color, true);
        }
      }
      return this;
    }

    public DebugGameMapDrawing HighlightTile(Tile2i tile, ColorRgba color, int offsetPx = 1)
    {
      if (this.m_rowMajorData == null || !this.IsTileOnMap(tile))
        return this;
      int num = this.m_pixelsPerTile - 2 * offsetPx;
      Vector2i vector2i = this.getPixelCoord(tile).AddXy(offsetPx);
      this.drawLineNoChecks(vector2i, vector2i.AddX(num), color, true);
      this.drawLineNoChecks(vector2i.AddX(num), vector2i.AddXy(num), color, true);
      this.drawLineNoChecks(vector2i.AddXy(num), vector2i.AddY(num), color, true);
      this.drawLineNoChecks(vector2i.AddY(num), vector2i, color, true);
      return this;
    }

    public DebugGameMapDrawing DrawArea(RectangleTerrainArea2i area, ColorRgba color)
    {
      Tile2f cornerTile2f = area.Origin.CornerTile2f;
      Tile2f tile2f = cornerTile2f + area.Size.RelTile2f;
      this.DrawLine(cornerTile2f, cornerTile2f.SetX(tile2f.X), color);
      this.DrawLine(cornerTile2f.SetX(tile2f.X), tile2f, color);
      this.DrawLine(tile2f, cornerTile2f.SetY(tile2f.Y), color);
      this.DrawLine(cornerTile2f.SetY(tile2f.Y), cornerTile2f, color);
      return this;
    }

    public string SaveMapAsTga(string name)
    {
      int num = Interlocked.Increment(ref DebugGameRenderer.s_imagesSet);
      if (this.m_rowMajorData == null)
        return "";
      string timestampedFilePath = (this.m_resolver?.Resolve<IFileSystemHelper>() ?? (IFileSystemHelper) new FileSystemHelper()).GetTimestampedFilePath(string.Format("_{0}_{1}.tga", (object) num, (object) name), FileType.Debug);
      TgaImageUtils.SaveTgaImage(this.m_rowMajorData, this.m_sizePx.X, this.m_sizePx.Y, timestampedFilePath);
      Log.Info("Image saved to: " + timestampedFilePath);
      return timestampedFilePath;
    }

    static DebugGameMapDrawing()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      DebugGameMapDrawing.PADDING = new RelTile1i(4);
      DebugGameMapDrawing.MIN_PADDING_PX = 20;
      DebugGameMapDrawing.MAX_PADDING_PX = 50;
      DebugGameMapDrawing.TICK_COLOR = new ColorRgba(128, 128, 128, (int) byte.MaxValue);
      DebugGameMapDrawing.TILE_GRID_COLOR = new ColorRgba(0, 0, 0, 8);
      DebugGameMapDrawing.STATIC_ENTITY_COLOR = new ColorRgba(32, 32, 32, 192);
      DebugGameMapDrawing.TRANSPORT_COLOR = new ColorRgba(32, 128, 32, 128);
      DebugGameMapDrawing.TRANSPORTED_PRODUCT_COLOR = new ColorRgba(128, 192, 0, 192);
      DebugGameMapDrawing.DYNAMIC_ENTITY_COLOR = new ColorRgba(0, 128, (int) byte.MaxValue, 192);
      DebugGameMapDrawing.EXCAVATOR_COLOR = new ColorRgba((int) byte.MaxValue, (int) byte.MaxValue, 0, 192);
      DebugGameMapDrawing.TILE_HEIGHTS_TEXT_COLOR = new ColorRgba(0, 0, 0, 32);
    }
  }
}
