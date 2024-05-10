// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Surfaces.TerrainTileSurfaceDecalProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Prototypes;
using System;
using System.Diagnostics;

#nullable disable
namespace Mafi.Core.Terrain.Surfaces
{
  [DebuggerDisplay("TileDecalProto: {Id}")]
  public class TerrainTileSurfaceDecalProto : Proto, IProtoWithSlimID<TileSurfaceDecalSlimId>
  {
    public static readonly Proto.ID PHANTOM_ID;
    /// <summary>
    /// Phantom prototype does not represent any valid prototype and serves as convenient placeholder to avoid
    /// redundant null checks or unnecessary usage of Option{T}. This is also useful for unit tests. Phantom
    /// prototype should be NEVER returned through public interface of the class that uses it.
    /// </summary>
    public static readonly TerrainTileSurfaceDecalProto Phantom;
    private TileSurfaceDecalSlimId m_slimId;

    public TileSurfaceDecalSlimId SlimId => this.m_slimId;

    public StaticEntityProto.ID Id { get; }

    public TerrainTileSurfaceDecalProto.Gfx Graphics { get; }

    public string IconPath => this.Graphics.IconPath;

    public TerrainTileSurfaceDecalProto(
      Proto.ID id,
      Proto.Str strings,
      TerrainTileSurfaceDecalProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings);
      this.Id = new StaticEntityProto.ID(id.Value);
      this.Graphics = graphics.CheckNotNull<TerrainTileSurfaceDecalProto.Gfx>();
    }

    void IProtoWithSlimID<TileSurfaceDecalSlimId>.SetSlimId(TileSurfaceDecalSlimId id)
    {
      if (this.m_slimId.Value != (byte) 0 && this.m_slimId != id)
        throw new InvalidOperationException(string.Format("Slim ID of '{0}' was already set to '{1}'.", (object) this, (object) this.m_slimId));
      this.m_slimId = id;
    }

    static TerrainTileSurfaceDecalProto()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TerrainTileSurfaceDecalProto.PHANTOM_ID = new Proto.ID("__PHANTOM__TILE_SURFACE_DECAL__");
      TerrainTileSurfaceDecalProto.Phantom = Proto.RegisterPhantom<TerrainTileSurfaceDecalProto>(new TerrainTileSurfaceDecalProto(TerrainTileSurfaceDecalProto.PHANTOM_ID, Proto.Str.Empty, TerrainTileSurfaceDecalProto.Gfx.Empty));
    }

    public new class Gfx : EntityProto.Gfx
    {
      public static readonly TerrainTileSurfaceDecalProto.Gfx Empty;
      public readonly Option<SurfaceDecalCategoryProto> Category;
      public readonly string AlbedoTexturePath;

      public string IconPath => this.AlbedoTexturePath;

      public Gfx(string albedoTexturePath, Option<SurfaceDecalCategoryProto> category)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(ColorRgba.Empty);
        this.AlbedoTexturePath = albedoTexturePath;
        this.Category = category;
      }

      static Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        TerrainTileSurfaceDecalProto.Gfx.Empty = new TerrainTileSurfaceDecalProto.Gfx("EMPTY", (Option<SurfaceDecalCategoryProto>) Option.None);
      }
    }
  }
}
