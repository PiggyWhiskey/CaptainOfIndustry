// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Trees.TreeProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Localization.Quantity;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;
using System.Diagnostics;
using System.Linq;

#nullable disable
namespace Mafi.Core.Terrain.Trees
{
  [DebuggerDisplay("TreeProto: {Id}")]
  public class TreeProto : 
    Proto,
    IProtoWithIconAndName,
    IProtoWithIcon,
    IProto,
    ILayoutEntityProto,
    IStaticEntityProto,
    IEntityProto
  {
    public static readonly int MAX_TREE_SPACING;
    public static readonly Percent MAX_BASE_SCALE_DEVIATION;
    public static readonly Percent Percent20;
    public static readonly Percent Percent40;
    public static readonly Percent Percent60;
    public static readonly Percent Percent80;
    /// <summary>
    /// Std deviation of trees size. Tree sizes are randomized with normal distribution.
    /// </summary>
    public readonly Percent BaseScaleStdDeviation;
    public readonly RelTile1i MinForestFloorRadius;
    public readonly RelTile1i MaxForestFloorRadius;
    public readonly int SpacingToOtherTree;
    public readonly bool IsDry;
    public readonly Option<TerrainMaterialProto> ForestFloorMaterial;

    public ProductType Type { get; }

    public System.Type EntityType => typeof (LayoutEntityProto);

    public StaticEntityProto.ID Id { get; }

    /// <summary>Costs for the entity - price, maintenance, etc.</summary>
    public EntityCosts Costs => EntityCosts.None;

    /// <summary>Available binding ports of this entity.</summary>
    public ImmutableArray<IoPortTemplate> Ports => ImmutableArray<IoPortTemplate>.Empty;

    /// <summary>Flip is disabled if true.</summary>
    public bool CannotBeReflected => true;

    /// <summary>Only one instance of this proto's entity is allowed.</summary>
    public bool IsUnique => false;

    /// <summary>
    /// If set, will automatically build miniZippers at ports.
    /// </summary>
    public bool AutoBuildMiniZippers => false;

    /// <summary>Product that is obtained when this tree is harvested.</summary>
    public ProductQuantity ProductWhenHarvested => this.TreePlantingGroupProto.ProductWhenHarvested;

    /// <summary>
    /// Allows to formats quantity of the current product with proper units so it can displayed to the player.
    /// </summary>
    public QuantityFormatter QuantityFormatter { get; }

    [OnlyForSaveCompatibility(null)]
    public Option<Mafi.Core.Terrain.Trees.ForestProto> ForestProto { get; private set; }

    public TreePlantingGroupProto TreePlantingGroupProto { get; private set; }

    public LayoutEntityProto.Gfx Graphics { get; }

    public TreeProto.TreeGfx TreeGraphics { get; }

    public string IconPath => this.Graphics.IconPath;

    public string MapEditorIconPath => this.TreeGraphics.MapEditorIconPath;

    /// <summary>Entity layout in relative coordinates.</summary>
    public EntityLayout Layout { get; }

    public TreeProto(
      Proto.ID id,
      Proto.Str strings,
      Percent baseScaleStdDeviation,
      RelTile1i minForestFloorRadius,
      RelTile1i maxForestFloorRadius,
      int spacingToOtherTree,
      bool isDry,
      Option<TerrainMaterialProto> forestFloorMaterial,
      EntityLayout layout,
      LayoutEntityProto.Gfx layoutEntityGraphics,
      TreeProto.TreeGfx treeGraphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings);
      if (minForestFloorRadius > maxForestFloorRadius)
        throw new InvalidProtoException(string.Format("Min forest floor radius '{0}' must be less than max '{1}'.", (object) minForestFloorRadius, (object) maxForestFloorRadius));
      if (spacingToOtherTree > TreeProto.MAX_TREE_SPACING)
        throw new InvalidProtoException(string.Format("Tree spacing '{0}' must be less than or equal to max '{1}'.", (object) spacingToOtherTree, (object) TreeProto.MAX_TREE_SPACING));
      this.Id = new StaticEntityProto.ID(id.Value);
      this.Type = new ProductType(this.GetType());
      this.Layout = layout;
      this.BaseScaleStdDeviation = baseScaleStdDeviation;
      this.IsDry = isDry;
      if (isDry)
        this.SetAvailability(false);
      this.MinForestFloorRadius = minForestFloorRadius;
      this.MaxForestFloorRadius = maxForestFloorRadius;
      this.SpacingToOtherTree = spacingToOtherTree;
      this.ForestFloorMaterial = forestFloorMaterial;
      this.Graphics = layoutEntityGraphics.CheckNotNull<LayoutEntityProto.Gfx>();
      this.TreeGraphics = treeGraphics.CheckNotNull<TreeProto.TreeGfx>();
      this.QuantityFormatter = (QuantityFormatter) ProductCountQuantityFormatter.Instance;
    }

    public Percent GetScale(int ticksAlive)
    {
      if (ticksAlive < this.TreePlantingGroupProto.TimeTo40PercentGrowth.Ticks)
        return Percent.FromRatio(ticksAlive, this.TreePlantingGroupProto.TimeTo40PercentGrowth.Ticks) * TreeProto.Percent40;
      if (ticksAlive < this.TreePlantingGroupProto.TimeTo60PercentGrowth.Ticks)
      {
        int ticks = this.TreePlantingGroupProto.TimeTo40PercentGrowth.Ticks;
        return Percent.FromRatio(ticksAlive - ticks, this.TreePlantingGroupProto.TimeTo60PercentGrowth.Ticks - ticks) * TreeProto.Percent20 + TreeProto.Percent40;
      }
      if (ticksAlive < this.TreePlantingGroupProto.TimeTo80PercentGrowth.Ticks)
      {
        int ticks = this.TreePlantingGroupProto.TimeTo60PercentGrowth.Ticks;
        return Percent.FromRatio(ticksAlive - ticks, this.TreePlantingGroupProto.TimeTo80PercentGrowth.Ticks - ticks) * TreeProto.Percent20 + TreeProto.Percent60;
      }
      if (ticksAlive >= this.TreePlantingGroupProto.TimeTo100PercentGrowth.Ticks)
        return Percent.Hundred;
      int ticks1 = this.TreePlantingGroupProto.TimeTo80PercentGrowth.Ticks;
      return Percent.FromRatio(ticksAlive - ticks1, this.TreePlantingGroupProto.TimeTo100PercentGrowth.Ticks - ticks1) * TreeProto.Percent20 + TreeProto.Percent80;
    }

    public Percent GetRandomBaseScale(IRandom rng)
    {
      return Percent.Hundred + rng.NextGaussianTrunc(TreeProto.MAX_BASE_SCALE_DEVIATION) * this.BaseScaleStdDeviation;
    }

    internal void SetForestProto(Mafi.Core.Terrain.Trees.ForestProto forestProto)
    {
      if (this.IsInitialized)
      {
        Mafi.Log.Warning("Proto already initialized.");
      }
      else
      {
        if (this.ForestProto != (Mafi.Core.Terrain.Trees.ForestProto) null)
          Mafi.Log.Warning(string.Format("Failed set forest proto of {0} to {1}, ", (object) this, (object) forestProto) + string.Format("it's already set to {0}.", (object) this.ForestProto));
        this.ForestProto = (Option<Mafi.Core.Terrain.Trees.ForestProto>) forestProto;
      }
    }

    internal void SetPlantingGroupProto(TreePlantingGroupProto groupProto)
    {
      if (this.IsInitialized)
      {
        Mafi.Log.Warning("Proto already initialized.");
      }
      else
      {
        if ((Proto) this.TreePlantingGroupProto != (Proto) null)
          Mafi.Log.Warning(string.Format("Failed set planting group proto of {0} to {1}, ", (object) this, (object) groupProto) + string.Format("it's already set to {0}.", (object) this.TreePlantingGroupProto));
        this.TreePlantingGroupProto = groupProto;
      }
    }

    protected override void OnInitialize(ProtosDb protosDb)
    {
      base.OnInitialize(protosDb);
      if (!protosDb.All<TreePlantingGroupProto>().Any<TreePlantingGroupProto>((Func<TreePlantingGroupProto, bool>) (x => x.Trees.Contains(this))))
        throw new ProtoBuilderException(string.Format("Tree proto '{0}' is not part of any tree planting group.", (object) this.Id));
      this.Graphics.Initialize((ILayoutEntityProto) this);
    }

    static TreeProto()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TreeProto.MAX_TREE_SPACING = 4;
      TreeProto.MAX_BASE_SCALE_DEVIATION = 300.Percent();
      TreeProto.Percent20 = Percent.FromRatio(2, 10);
      TreeProto.Percent40 = Percent.FromRatio(4, 10);
      TreeProto.Percent60 = Percent.FromRatio(6, 10);
      TreeProto.Percent80 = Percent.FromRatio(8, 10);
    }

    public class TreeGfx : Proto.Gfx
    {
      public static readonly TreeProto.TreeGfx Empty;
      public readonly ImmutableArray<Pair<string, string>> PrefabPaths;
      public readonly Option<string> TrimmedTreePrefabPath;
      public readonly RelTile1f TrimmedTreeLength;
      public readonly string MapEditorIconPath;

      public TreeGfx(
        ImmutableArray<Pair<string, string>> prefabPaths,
        Option<string> trimmedTreePrefabPath,
        RelTile1f trimmedTreeLength,
        string mapEditorIconPath)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.PrefabPaths = prefabPaths.CheckNotEmpty<Pair<string, string>>();
        this.TrimmedTreePrefabPath = trimmedTreePrefabPath;
        this.TrimmedTreeLength = trimmedTreeLength.CheckNotNegative();
        this.MapEditorIconPath = mapEditorIconPath;
      }

      static TreeGfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        TreeProto.TreeGfx.Empty = new TreeProto.TreeGfx(ImmutableArray.Create<Pair<string, string>>(new Pair<string, string>("EMPTY", "EMPTY")), Option<string>.None, RelTile1f.Zero, string.Empty);
      }
    }
  }
}
