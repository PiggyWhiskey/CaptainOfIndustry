// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.Layout.LayoutEntityProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#nullable disable
namespace Mafi.Core.Entities.Static.Layout
{
  [DebuggerDisplay("LayoutEntityProto: {Id}")]
  public abstract class LayoutEntityProto : 
    StaticEntityProto,
    ILayoutEntityProto,
    IStaticEntityProto,
    IEntityProto,
    IProto,
    IProtoWithIcon
  {
    public static readonly Duration DEFAULT_CONSTR_DUR_PER_PRODUCT;
    /// <summary>Null if boost not supported.</summary>
    public readonly Upoints? BoostCost;
    /// <summary>Input ports of this entity.</summary>
    public readonly ImmutableArray<IoPortTemplate> InputPorts;
    /// <summary>Output ports of this entity.</summary>
    public readonly ImmutableArray<IoPortTemplate> OutputPorts;
    /// <summary>
    /// Whether this entity is not allowed to be built by the player
    /// (typically is added to the world via other means).
    /// </summary>
    public readonly bool CannotBeBuiltByPlayer;

    /// <summary>Entity layout in relative coordinates.</summary>
    public EntityLayout Layout { get; }

    /// <summary>Available binding ports of this entity.</summary>
    public ImmutableArray<IoPortTemplate> Ports => this.Layout.Ports;

    /// <summary>Whether this entity is allowed to be cloned.</summary>
    public bool CloningDisabled => this.IsUnique || this.CannotBeBuiltByPlayer;

    /// <summary>Only one instance of this proto's entity is allowed.</summary>
    public bool IsUnique { get; }

    /// <summary>Flip is disabled if true.</summary>
    public bool CannotBeReflected { get; }

    /// <summary>
    /// If set, will automatically build miniZippers at ports.
    /// </summary>
    public virtual bool AutoBuildMiniZippers { get; }

    /// <summary>3D model of this entity.</summary>
    public LayoutEntityProto.Gfx Graphics { get; }

    public string IconPath => this.Graphics.IconPath;

    public LayoutEntityProto(
      StaticEntityProto.ID id,
      Proto.Str strings,
      EntityLayout layout,
      EntityCosts costs,
      LayoutEntityProto.Gfx graphics,
      Duration? constructionDurationPerProduct = null,
      Upoints? boostCost = null,
      bool cannotBeBuiltByPlayer = false,
      bool isUnique = false,
      bool cannotBeReflected = false,
      bool autoBuildMiniZippers = false,
      bool doNotStartConstructionAutomatically = false,
      IEnumerable<Tag> tags = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, costs, constructionDurationPerProduct ?? LayoutEntityProto.DEFAULT_CONSTR_DUR_PER_PRODUCT, new ThicknessIRange?(layout.PlacementHeightRange), (StaticEntityProto.Gfx) graphics, doNotStartConstructionAutomatically, tags);
      this.Layout = layout.CheckNotNull<EntityLayout>();
      this.Graphics = graphics.CheckNotNull<LayoutEntityProto.Gfx>();
      this.BoostCost = boostCost;
      this.CannotBeBuiltByPlayer = cannotBeBuiltByPlayer;
      this.IsUnique = isUnique;
      this.CannotBeReflected = cannotBeReflected;
      this.AutoBuildMiniZippers = autoBuildMiniZippers;
      this.InputPorts = this.Layout.Ports.Where((Func<IoPortTemplate, bool>) (pt => pt.Type == IoPortType.Input)).ToImmutableArray<IoPortTemplate>();
      this.OutputPorts = this.Layout.Ports.Where((Func<IoPortTemplate, bool>) (pt => pt.Type == IoPortType.Output)).ToImmutableArray<IoPortTemplate>();
      this.Graphics.Initialize((ILayoutEntityProto) this);
    }

    static LayoutEntityProto()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      LayoutEntityProto.DEFAULT_CONSTR_DUR_PER_PRODUCT = 0.2.Seconds();
    }

    public new class Gfx : StaticEntityProto.Gfx
    {
      public static readonly LayoutEntityProto.Gfx Empty;
      /// <summary>
      /// Whether custom icon path was set. Otherwise, icon path is automatically generated.
      /// </summary>
      public readonly bool IconIsCustom;
      public readonly bool UseInstancedRendering;
      /// <summary>
      /// Used for entities which require particles, animations, sounds, emission or other dynamic things.
      /// </summary>
      public readonly bool UseSemiInstancedRendering;
      /// <summary>
      /// Object names that are excluded when using SemiInstancedRendering and are instead rendered via an MB.
      /// </summary>
      public readonly ImmutableArray<string> SemiInstancedRenderingExcludedObjects;
      public readonly int MaxRenderedLod;
      /// <summary>
      /// Whether we can strip out empty child gameObjects. If they are used in animation this is often safe
      /// but there are cases (eg. if we use a child's transform to dynamically spawn something) where it's not.
      /// </summary>
      public readonly bool DisableEmptyChildrenStripping;
      /// <summary>
      /// This is used solely by the implementation of IEntitiesRenderer that renders this entity.
      /// </summary>
      public uint InstancedRendererIndex;
      /// <summary>
      /// This is used solely by the implementation of IEntitiesRenderer that renders this entity.
      /// </summary>
      public ImmutableArray<ImmutableArray<KeyValuePair<string, IReadOnlySet<string>>>> AnimatedGameObjects;
      /// <summary>
      /// This is used solely by the implementation of IEntitiesRenderer that renders this entity.
      /// </summary>
      public float AnimationLength;
      private readonly string m_instancedRenderingAnimationProtoSwap;
      private readonly IReadOnlyDictionary<string, string> m_instancedRenderingAnimationMaterialSwap;
      private ILayoutEntityProto m_proto;

      public string PrefabPath { get; private set; }

      /// <summary>
      /// Custom prefab origin. This can be used when a prefab has incorrect origin.
      /// </summary>
      public RelTile3f PrefabOrigin { get; }

      /// <summary>Path for icon sprite.</summary>
      /// <remarks>This path is valid only after <see cref="M:Mafi.Core.Entities.Static.Layout.LayoutEntityProto.Gfx.Initialize(Mafi.Core.Entities.Static.Layout.ILayoutEntityProto)" /> was called.</remarks>
      public string IconPath { get; private set; }

      public LayoutEntityProto.VisualizedLayers VisualizedLayers { get; }

      /// <summary>All toolbar categories the entity belongs to.</summary>
      public ImmutableArray<ToolbarCategoryProto> Categories { get; }

      public Gfx(
        string prefabPath,
        RelTile3f prefabOrigin = default (RelTile3f),
        Option<string> customIconPath = default (Option<string>),
        ColorRgba color = default (ColorRgba),
        bool hideBlockedPortsIcon = false,
        LayoutEntityProto.VisualizedLayers? visualizedLayers = null,
        ImmutableArray<ToolbarCategoryProto>? categories = null,
        bool useInstancedRendering = false,
        bool useSemiInstancedRendering = false,
        string instancedRenderingAnimationProtoSwap = null,
        IReadOnlyDictionary<string, string> instancedRenderingAnimationMaterialSwap = null,
        ImmutableArray<string> instancedRenderingExcludedObjects = default (ImmutableArray<string>),
        int maxRenderedLod = 2147483647,
        bool disableEmptyChildrenStripping = false)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(color, hideBlockedPortsIcon);
        this.PrefabPath = prefabPath.CheckNotNullOrEmpty();
        this.PrefabOrigin = prefabOrigin;
        this.IconPath = customIconPath.ValueOrNull;
        this.IconIsCustom = customIconPath.HasValue;
        this.VisualizedLayers = visualizedLayers ?? LayoutEntityProto.VisualizedLayers.Empty;
        this.Categories = categories ?? ImmutableArray<ToolbarCategoryProto>.Empty;
        this.UseInstancedRendering = useInstancedRendering;
        this.UseSemiInstancedRendering = useSemiInstancedRendering;
        this.SemiInstancedRenderingExcludedObjects = instancedRenderingExcludedObjects == new ImmutableArray<string>() ? ImmutableArray<string>.Empty : instancedRenderingExcludedObjects;
        this.m_instancedRenderingAnimationProtoSwap = instancedRenderingAnimationProtoSwap;
        this.m_instancedRenderingAnimationMaterialSwap = instancedRenderingAnimationMaterialSwap ?? (IReadOnlyDictionary<string, string>) new Dict<string, string>();
        if (useInstancedRendering & useSemiInstancedRendering)
          throw new InvalidProtoException("Proto trying to use both semi and total instancing");
        this.MaxRenderedLod = maxRenderedLod;
        this.DisableEmptyChildrenStripping = disableEmptyChildrenStripping;
      }

      public virtual void Initialize(ILayoutEntityProto proto)
      {
        Mafi.Assert.That<LayoutEntityProto.Gfx>(proto.Graphics).IsEqualTo<LayoutEntityProto.Gfx>(this);
        if (!this.IconIsCustom)
          this.IconPath = string.Format("{0}/LayoutEntity/{1}.png", (object) "Assets/Unity/Generated/Icons", (object) proto.Id);
        this.m_proto = proto;
      }

      public void ReplacePrefabWith(string prefabPath) => this.PrefabPath = prefabPath;

      private string animationProtoName()
      {
        return this.m_instancedRenderingAnimationProtoSwap ?? this.m_proto.Id.ToString();
      }

      private string animationMaterialName(string origName)
      {
        string str;
        return !this.m_instancedRenderingAnimationMaterialSwap.TryGetValue(origName, out str) ? origName : str;
      }

      /// <summary>Path for list of animated game objects.</summary>
      public string AnimationGameObjectsPathForLod(string lodName)
      {
        return "Assets/Unity/Generated/Animations/" + this.animationProtoName() + "_" + lodName + ".txt";
      }

      /// <summary>Path for list of animated game objects.</summary>
      public string AnimationGameObjectsPathForLod(int lod)
      {
        return string.Format("{0}/{1}_LOD{2}.txt", (object) "Assets/Unity/Generated/Animations", (object) this.animationProtoName(), (object) lod);
      }

      public string AnimationTextureVerticesPathForMaterialAndLod(
        string name,
        string lodName,
        string part)
      {
        return "Assets/Unity/Generated/Animations/" + this.animationProtoName() + "-" + this.animationMaterialName(name) + "_" + lodName + "_" + part + "_vert.exr";
      }

      public string AnimationTextureVerticesPathForMaterialAndLod(
        string name,
        int lod,
        string part)
      {
        return string.Format("{0}/{1}-{2}_LOD{3}_{4}_vert.exr", (object) "Assets/Unity/Generated/Animations", (object) this.animationProtoName(), (object) this.animationMaterialName(name), (object) lod, (object) part);
      }

      public string AnimationTextureNormalsPathForMaterialAndLod(string name, string lodName)
      {
        return "Assets/Unity/Generated/Animations/" + this.animationProtoName() + "-" + this.animationMaterialName(name) + "_" + lodName + "_norm.png";
      }

      public string AnimationTextureNormalsPathForMaterialAndLod(string name, int lod)
      {
        return string.Format("{0}/{1}-{2}_LOD{3}_norm.png", (object) "Assets/Unity/Generated/Animations", (object) this.animationProtoName(), (object) this.animationMaterialName(name), (object) lod);
      }

      public string AnimationTextureTangentsPathForMaterialAndLod(string name, string lodName)
      {
        return "Assets/Unity/Generated/Animations/" + this.animationProtoName() + "-" + this.animationMaterialName(name) + "_" + lodName + "_tang.png";
      }

      public string AnimationTextureTangentsPathForMaterialAndLod(string name, int lod)
      {
        return string.Format("{0}/{1}-{2}_LOD{3}_tang.png", (object) "Assets/Unity/Generated/Animations", (object) this.animationProtoName(), (object) this.animationMaterialName(name), (object) lod);
      }

      static Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        LayoutEntityProto.Gfx.Empty = new LayoutEntityProto.Gfx("EMPTY", RelTile3f.Zero, (Option<string>) "EMPTY", ColorRgba.Empty, visualizedLayers: new LayoutEntityProto.VisualizedLayers?(LayoutEntityProto.VisualizedLayers.Empty), categories: new ImmutableArray<ToolbarCategoryProto>?(ImmutableArray<ToolbarCategoryProto>.Empty));
      }
    }

    /// <summary>
    /// A list of resources to be displayed when an entity is being laid down or moved.
    /// </summary>
    public struct VisualizedLayers
    {
      public static readonly LayoutEntityProto.VisualizedLayers Empty;
      public readonly ImmutableArray<TerrainMaterialProto> TerrainMaterials;
      public readonly ImmutableArray<VirtualResourceProductProto> VirtualResources;
      public readonly ImmutableArray<ProductProto> AllVisualizedProducts;
      /// <summary>
      /// Whether to enable terrain designators (mining,dumping,concreting);
      /// </summary>
      public readonly bool TerrainDesignators;
      /// <summary>Whether to enable tree harvesting designators.</summary>
      public readonly bool TreeDesignators;

      public VisualizedLayers(
        bool terrainDesignators,
        bool treeDesignators,
        ImmutableArray<TerrainMaterialProto> terrainMaterials,
        ImmutableArray<VirtualResourceProductProto> virtualResources)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.TerrainDesignators = terrainDesignators;
        this.TreeDesignators = treeDesignators;
        this.TerrainMaterials = terrainMaterials;
        this.VirtualResources = virtualResources;
        if (terrainMaterials.Length > 0 || virtualResources.Length > 0)
          this.AllVisualizedProducts = ((IEnumerable<ProductProto>) terrainMaterials.Select<LooseProductProto>((Func<TerrainMaterialProto, LooseProductProto>) (x => x.MinedProduct))).Concat<ProductProto>(virtualResources.Select<ProductProto>((Func<VirtualResourceProductProto, ProductProto>) (x => x.Product))).ToImmutableArray<ProductProto>();
        else
          this.AllVisualizedProducts = ImmutableArray<ProductProto>.Empty;
      }

      static VisualizedLayers()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        LayoutEntityProto.VisualizedLayers.Empty = new LayoutEntityProto.VisualizedLayers(false, false, ImmutableArray<TerrainMaterialProto>.Empty, ImmutableArray<VirtualResourceProductProto>.Empty);
      }
    }
  }
}
