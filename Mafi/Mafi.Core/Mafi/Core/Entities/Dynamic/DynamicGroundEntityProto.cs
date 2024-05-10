// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Dynamic.DynamicGroundEntityProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Entities.Dynamic
{
  public abstract class DynamicGroundEntityProto : DynamicEntityProto, IProtoWithIcon, IProto
  {
    /// <summary>
    /// Specifies size of entity bounding box centered at entity origin.
    /// </summary>
    public readonly RelTile3f EntitySize;
    /// <summary>Tolerance for navigation to this entity.</summary>
    public readonly RelTile1i NavTolerance;
    /// <summary>
    /// Specifies how much is terrain surface disrupted at particular radius (where radius in the index). Length of
    /// this array naturally specifies disruption radius.
    /// </summary>
    public readonly ImmutableArray<ThicknessTilesF> DisruptionByDistance;
    /// <summary>Build duration per one quantity of product used.</summary>
    public readonly Duration BuildDurationPerProduct;
    /// <summary>
    /// Build duration at the end of the build (after all products are provided).
    /// </summary>
    public readonly Duration BuildExtraDuration;
    public readonly DynamicGroundEntityProto.Gfx Graphics;

    public bool DisruptsSurface => this.DisruptionByDistance.IsNotEmpty;

    public new string IconPath => this.Graphics.IconPath;

    public DynamicGroundEntityProto(
      DynamicEntityProto.ID id,
      Proto.Str strings,
      RelTile3f entitySize,
      EntityCosts costs,
      int vehicleQuotaCost,
      ImmutableArray<ThicknessTilesF> disruptionByDistance,
      Duration durationToBuild,
      DynamicGroundEntityProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, (DynamicEntityProto.Gfx) graphics, costs, vehicleQuotaCost);
      this.EntitySize = entitySize.CheckNotDefaultStruct<RelTile3f>();
      this.NavTolerance = new RelTile1i(1 + entitySize.X.Max(entitySize.Y).ToIntRounded());
      this.DisruptionByDistance = disruptionByDistance.CheckNotDefaultStruct<ImmutableArray<ThicknessTilesF>>();
      this.Graphics = graphics.CheckNotNull<DynamicGroundEntityProto.Gfx>();
      Quantity quantitySum = this.Costs.Price.GetQuantitySum();
      if (quantitySum.IsPositive)
      {
        this.BuildDurationPerProduct = Duration.FromTicks(Percent.FromPercentVal(90).Apply(durationToBuild.Ticks) / quantitySum.Value);
        this.BuildExtraDuration = durationToBuild - quantitySum.Value * this.BuildDurationPerProduct;
      }
      else
      {
        this.BuildDurationPerProduct = Duration.Zero;
        this.BuildExtraDuration = durationToBuild;
      }
      Assert.That<Duration>(this.BuildExtraDuration + this.BuildDurationPerProduct * quantitySum.Value).IsEqualTo(durationToBuild);
    }

    public new class Gfx : DynamicEntityProto.Gfx
    {
      public static readonly DynamicGroundEntityProto.Gfx Empty;
      public readonly string PrefabPath;
      public readonly RelTile2f FrontContactPtsOffset;
      public readonly RelTile2f RearContactPtsOffset;
      public ImmutableArray<DynamicEntityDustParticlesSpec> DustParticles;
      public readonly Option<VehicleExhaustParticlesSpec> ExhaustParticlesSpec;
      public readonly string EngineSoundPath;
      public readonly string MovementSoundPath;

      public Gfx(
        string prefabPath,
        Option<string> customIconPath,
        RelTile2f frontContactPtsOffset,
        RelTile2f rearContactPtsOffset,
        ImmutableArray<DynamicEntityDustParticlesSpec> dustParticles,
        Option<VehicleExhaustParticlesSpec> exhaustParticlesSpec,
        string engineSoundPath,
        string movementSoundPath)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(customIconPath);
        this.DustParticles = dustParticles;
        this.ExhaustParticlesSpec = exhaustParticlesSpec;
        this.PrefabPath = prefabPath.CheckNotNullOrEmpty();
        this.FrontContactPtsOffset = frontContactPtsOffset.CheckNotDefaultStruct<RelTile2f>();
        this.RearContactPtsOffset = rearContactPtsOffset.CheckNotDefaultStruct<RelTile2f>();
        this.EngineSoundPath = engineSoundPath;
        this.MovementSoundPath = movementSoundPath;
      }

      static Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        DynamicGroundEntityProto.Gfx.Empty = new DynamicGroundEntityProto.Gfx("EMPTY", (Option<string>) "EMPTY", new RelTile2f((Fix32) 1, (Fix32) 1), new RelTile2f((Fix32) 1, (Fix32) 1), ImmutableArray<DynamicEntityDustParticlesSpec>.Empty, Option<VehicleExhaustParticlesSpec>.None, "EMPTY", "EMPTY");
      }
    }
  }
}
