// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Props.TerrainPropProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Terrain.Props
{
  public class TerrainPropProto : Proto
  {
    public readonly bool AllowYawRandomization;
    public readonly bool AllowPitchRandomization;
    public readonly bool AllowRollRandomization;
    public readonly bool DoesNotBlocksVehicles;

    public new Proto.ID Id { get; }

    public TerrainPropBoundingShape BoundingShape { get; }

    /// <summary>
    /// Radius (if circular) or half width/height (if rectangular). If circular X and Y must be the same.
    /// </summary>
    public RelTile2f Extents { get; }

    public Percent BaseScale { get; }

    /// <summary>
    /// This product is provided only when the prop is harvested by a vehicle, not if it is destroyed
    /// in any other way. It is scaled by the prop's scale.
    /// </summary>
    public ProductQuantity ProductWhenHarvested { get; }

    /// <summary>
    /// The maximum depth this prop can be generated at. This will be scaled by the prop's scale.
    /// </summary>
    public ThicknessTilesF MaxSpawnDepth { get; }

    /// <summary>
    /// Amount of the prop that needs to be buried before we remove it.
    /// This will be scaled by the prop's scale.
    /// </summary>
    public ThicknessTilesF DespawnBuriedThreshold { get; }

    public ImmutableArray<TerrainPropData.PropVariant> Variants { get; }

    public TerrainPropProto.PropGfx Graphics { get; }

    public TerrainPropProto(
      Proto.ID id,
      Proto.Str strings,
      TerrainPropBoundingShape boundingShape,
      RelTile2f extents,
      Percent baseScale,
      ThicknessTilesF maxSpawnDepth,
      ThicknessTilesF despawnBuriedThreshold,
      ProductQuantity productWhenHarvested,
      bool allowYawRandomization,
      ImmutableArray<TerrainPropData.PropVariant> variants,
      TerrainPropProto.PropGfx graphics,
      bool allowPitchRandomization = false,
      bool allowRollRandomization = false,
      bool doesNotBlocksVehicles = false)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings);
      this.Id = new Proto.ID(id.Value);
      this.BoundingShape = boundingShape;
      this.Extents = extents;
      this.BaseScale = baseScale.CheckPositive();
      this.MaxSpawnDepth = maxSpawnDepth.CheckNotNegative();
      this.DespawnBuriedThreshold = despawnBuriedThreshold.CheckPositive();
      this.ProductWhenHarvested = productWhenHarvested;
      this.AllowYawRandomization = allowYawRandomization;
      this.AllowPitchRandomization = allowPitchRandomization;
      this.AllowRollRandomization = allowRollRandomization;
      this.DoesNotBlocksVehicles = doesNotBlocksVehicles;
      if (this.Extents.X < 0 || this.Extents.Y < 0)
        throw new InvalidProtoException(string.Format("Prop extents '{0}' must be positive.", (object) this.Extents));
      if (boundingShape == TerrainPropBoundingShape.Circle && this.Extents.X != this.Extents.Y)
        throw new InvalidProtoException(string.Format("Inconsistent radius for circular bounding area: '{0}' != '{1}'.", (object) this.Extents.X, (object) this.Extents.Y));
      this.Variants = variants.CheckNotEmpty<TerrainPropData.PropVariant>();
      this.Graphics = graphics.CheckNotNull<TerrainPropProto.PropGfx>();
    }

    public class PropGfx : Proto.Gfx
    {
      public static readonly TerrainPropProto.PropGfx Empty;
      public readonly string PrefabPath;
      public readonly bool UseTerrainTextures;
      public readonly Aabb BoundingBox;
      public readonly string IconPath;
      public readonly string PreviewMatPath;

      public PropGfx(
        string prefabPath,
        Aabb boundingBox,
        string iconPath,
        string previewMatPath,
        bool useTerrainTextures = false)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.PrefabPath = prefabPath;
        this.BoundingBox = boundingBox;
        this.UseTerrainTextures = useTerrainTextures;
        this.IconPath = iconPath;
        this.PreviewMatPath = previewMatPath;
      }

      static PropGfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        TerrainPropProto.PropGfx.Empty = new TerrainPropProto.PropGfx("EMPTY", new Aabb(), (string) null, "EMPTY");
      }
    }
  }
}
