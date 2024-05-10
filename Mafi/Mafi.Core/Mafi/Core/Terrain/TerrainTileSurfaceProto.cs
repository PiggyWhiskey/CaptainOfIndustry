// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.TerrainTileSurfaceProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Prototypes;
using System;

#nullable disable
namespace Mafi.Core.Terrain
{
  public class TerrainTileSurfaceProto : 
    Proto,
    IProtoWithSlimID<TileSurfaceSlimId>,
    IProtoWithIcon,
    IProto
  {
    public static readonly Proto.ID PHANTOM_PRODUCT_ID;
    /// <summary>
    /// Phantom prototype does not represent any valid prototype and serves as convenient placeholder to avoid
    /// redundant null checks or unnecessary usage of Option{T}. This is also useful for unit tests. Phantom
    /// prototype should be NEVER returned through public interface of the class that uses it.
    /// </summary>
    public static readonly TerrainTileSurfaceProto Phantom;
    private TileSurfaceSlimId m_slimId;
    public readonly Percent MaintenanceScale;
    public readonly ImmutableArray<Proto.ID> EdgeCompatibleWith;
    public readonly bool CanBePlacedByPlayer;
    public readonly ProductQuantity CostPerTile;
    public readonly TerrainTileSurfaceProto.Gfx Graphics;

    public string IconPath => this.Graphics.IconPath;

    public TileSurfaceSlimId SlimId => this.m_slimId;

    public TerrainTileSurfaceProto(
      Proto.ID id,
      Proto.Str strings,
      Percent maintenanceScale,
      ProductQuantity costPerTile,
      bool canBePlacedByPlayer,
      ImmutableArray<Proto.ID> edgeCompatibleWith,
      TerrainTileSurfaceProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings);
      this.MaintenanceScale = maintenanceScale;
      this.CostPerTile = costPerTile;
      this.CanBePlacedByPlayer = canBePlacedByPlayer;
      this.EdgeCompatibleWith = edgeCompatibleWith;
      this.Graphics = graphics.CheckNotNull<TerrainTileSurfaceProto.Gfx>();
    }

    protected override void OnInitialize(ProtosDb protosDb)
    {
      base.OnInitialize(protosDb);
      foreach (Proto.ID id in this.EdgeCompatibleWith)
      {
        TerrainTileSurfaceProto proto;
        if (!protosDb.TryGetProto<TerrainTileSurfaceProto>(id, out proto))
          Log.Error(string.Format("{0} edge compatibility not mirrored with {1}", (object) this.Id, (object) id));
        else if (!proto.EdgeCompatibleWith.Contains(this.Id))
          Log.Error(string.Format("{0} edge compatibility not mirrored with {1}", (object) this.Id, (object) id));
      }
    }

    public bool CanMergeGraphicsWith(TerrainTileSurfaceProto other)
    {
      return this.SlimId == other.SlimId || this.EdgeCompatibleWith.Contains(other.Id);
    }

    void IProtoWithSlimID<TileSurfaceSlimId>.SetSlimId(TileSurfaceSlimId id)
    {
      if (this.m_slimId.Value != (byte) 0 && this.m_slimId != id)
        throw new InvalidOperationException(string.Format("Slim ID of '{0}' was already set to '{1}'.", (object) this, (object) this.m_slimId));
      this.m_slimId = id;
    }

    static TerrainTileSurfaceProto()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TerrainTileSurfaceProto.PHANTOM_PRODUCT_ID = new Proto.ID("__PHANTOM__TILE_SURFACE__");
      TerrainTileSurfaceProto.Phantom = Proto.RegisterPhantom<TerrainTileSurfaceProto>(new TerrainTileSurfaceProto(TerrainTileSurfaceProto.PHANTOM_PRODUCT_ID, Proto.Str.Empty, Percent.Hundred, ProductQuantity.None, false, ImmutableArray<Proto.ID>.Empty, TerrainTileSurfaceProto.Gfx.Empty));
    }

    public new class Gfx : Proto.Gfx
    {
      public static readonly TerrainTileSurfaceProto.Gfx Empty;
      public readonly float DustinessPerc;
      public readonly ColorRgba DustColor;
      public readonly string IconPath;

      public TileSurfaceTextureSpec TextureSpec { get; private set; }

      public TileSurfacesEdgesSpec EdgesSpec { get; private set; }

      public Gfx(
        TileSurfaceTextureSpec textureSpec,
        TileSurfacesEdgesSpec edgesSpec,
        float dustinessPerc = 0.0f,
        ColorRgba? dustColor = null,
        string customIconPath = null)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.TextureSpec = textureSpec;
        this.EdgesSpec = edgesSpec;
        this.DustinessPerc = dustinessPerc;
        this.DustColor = dustColor ?? (ColorRgba) 6710886;
        this.IconPath = customIconPath;
      }

      public void ReplaceTextureSpec(
        TileSurfaceTextureSpec textureSpec,
        TileSurfacesEdgesSpec edgeSpec)
      {
        this.TextureSpec = textureSpec;
        this.EdgesSpec = edgeSpec;
      }

      static Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        TerrainTileSurfaceProto.Gfx.Empty = new TerrainTileSurfaceProto.Gfx(TileSurfaceTextureSpec.Empty, TileSurfacesEdgesSpec.Empty);
      }
    }
  }
}
