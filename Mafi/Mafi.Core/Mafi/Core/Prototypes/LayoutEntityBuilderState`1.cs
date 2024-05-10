// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Prototypes.LayoutEntityBuilderState`1
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Economy;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Products;
using System;

#nullable disable
namespace Mafi.Core.Prototypes
{
  public abstract class LayoutEntityBuilderState<TState> : ProtoBuilderState<TState> where TState : ProtoBuilderState<TState>
  {
    private ImmutableArray<ToolbarCategoryProto>? m_categories;
    private Option<EntityLayout> m_layout;
    private Func<ProductProto, bool> m_protoFilter;
    protected Electricity Electricity;
    protected ColorRgba MaterialColor;
    protected string PrefabPath;

    protected EntityCosts Costs { get; private set; }

    protected LayoutEntityBuilderState(
      IProtoBuilder builder,
      StaticEntityProto.ID id,
      string name,
      string translationComment = "building or machine")
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: reference to a compiler-generated field
      this.\u003CCosts\u003Ek__BackingField = EntityCosts.None;
      this.MaterialColor = ColorRgba.Empty;
      // ISSUE: reference to a compiler-generated field
      this.\u003CVisualizedTerrainMaterials\u003Ek__BackingField = ImmutableArray<TerrainMaterialProto>.Empty;
      // ISSUE: reference to a compiler-generated field
      this.\u003CVisualizedVirtualResources\u003Ek__BackingField = ImmutableArray<VirtualResourceProductProto>.Empty;
      // ISSUE: explicit constructor call
      base.\u002Ector(builder, (Proto.ID) id, name, translationComment);
    }

    protected EntityLayout LayoutOrThrow
    {
      get => this.ValueOrThrow<EntityLayout>(this.m_layout, "Layout");
    }

    protected Func<ProductProto, bool> ProductsFilterOrThrow
    {
      get => this.m_protoFilter ?? (Func<ProductProto, bool>) (x => true);
    }

    protected RelTile3f PrefabOrigin { get; private set; }

    protected bool InstancedRenderingEnabled { get; private set; }

    protected bool SemiInstancedRenderingEnabled { get; private set; }

    protected ImmutableArray<string> InstancedRenderingExcludedObjects { get; private set; }

    protected Option<string> CustomIconPath { get; private set; }

    protected ImmutableArray<TerrainMaterialProto> VisualizedTerrainMaterials { get; private set; }

    protected ImmutableArray<VirtualResourceProductProto> VisualizedVirtualResources { get; private set; }

    protected bool VisualizeTerrainDesignators { get; private set; }

    protected bool VisualizeTreeDesignators { get; private set; }

    protected LayoutEntityProto.VisualizedLayers VisualizedResourcesList
    {
      get
      {
        return new LayoutEntityProto.VisualizedLayers(this.VisualizeTerrainDesignators, this.VisualizeTreeDesignators, this.VisualizedTerrainMaterials, this.VisualizedVirtualResources);
      }
    }

    protected LayoutEntityProto.Gfx Graphics
    {
      get
      {
        return !string.IsNullOrEmpty(this.PrefabPath) ? new LayoutEntityProto.Gfx(this.PrefabPath, this.PrefabOrigin, this.CustomIconPath, this.MaterialColor, visualizedLayers: new LayoutEntityProto.VisualizedLayers?(this.VisualizedResourcesList), categories: new ImmutableArray<ToolbarCategoryProto>?(this.GetCategoriesOrThrow()), useInstancedRendering: this.InstancedRenderingEnabled, useSemiInstancedRendering: this.SemiInstancedRenderingEnabled, instancedRenderingExcludedObjects: this.InstancedRenderingExcludedObjects) : LayoutEntityProto.Gfx.Empty;
      }
    }

    [MustUseReturnValue]
    public TState SetLayout(params string[] layout)
    {
      return this.SetLayout(EntityLayoutParams.DEFAULT, layout);
    }

    [MustUseReturnValue]
    public TState SetLayout(EntityLayoutParams layoutParams, params string[] layout)
    {
      try
      {
        this.m_layout = (Option<EntityLayout>) new EntityLayoutParser(this.Builder.ProtosDb).ParseLayoutOrThrow(layoutParams, layout);
      }
      catch (InvalidEntityLayoutException ex)
      {
        throw new ProtoBuilderException(string.Format("Invalid layout of entity '{0}'.", (object) this.Id), (Exception) ex);
      }
      return (this as TState).CheckNotNull<TState>();
    }

    [MustUseReturnValue]
    public TState SetPrefabPath(string prefabPath)
    {
      this.PrefabPath = prefabPath.CheckNotNullOrEmpty();
      return (this as TState).CheckNotNull<TState>();
    }

    [MustUseReturnValue]
    public TState SetPrefabOffset(RelTile3f offset)
    {
      this.PrefabOrigin = offset;
      return (this as TState).CheckNotNull<TState>();
    }

    [MustUseReturnValue]
    public TState EnableInstancedRendering(ImmutableArray<string> excludedObjects = default (ImmutableArray<string>))
    {
      this.InstancedRenderingEnabled = true;
      this.InstancedRenderingExcludedObjects = excludedObjects;
      return (this as TState).CheckNotNull<TState>();
    }

    [MustUseReturnValue]
    public TState EnableSemiInstancedRendering(ImmutableArray<string> excludedObjects = default (ImmutableArray<string>))
    {
      this.SemiInstancedRenderingEnabled = true;
      this.InstancedRenderingExcludedObjects = excludedObjects;
      return (this as TState).CheckNotNull<TState>();
    }

    [MustUseReturnValue]
    public TState SetCustomIconPath(string customIconPath)
    {
      this.CustomIconPath = (Option<string>) customIconPath.CheckNotNullOrEmpty();
      return (this as TState).CheckNotNull<TState>();
    }

    [MustUseReturnValue]
    public TState SetCost(EntityCostsTpl value, bool costsDisabled = false)
    {
      if (costsDisabled)
        return (this as TState).CheckNotNull<TState>();
      if (this.Costs.Price.IsNotEmpty)
        throw new ProtoBuilderException(string.Format("Cost of {0} was already set.", (object) this.Id));
      this.Costs = value.MapToEntityCosts(this.Builder.Registrator);
      return (this as TState).CheckNotNull<TState>();
    }

    [MustUseReturnValue]
    public TState SetCost(AssetValue value)
    {
      if (this.Costs.Price.IsNotEmpty)
        throw new ProtoBuilderException(string.Format("Cost of {0} was already set.", (object) this.Id));
      this.Costs = new EntityCosts(value);
      return (this as TState).CheckNotNull<TState>();
    }

    /// <summary>
    /// Sets toolbar categories into which this entity belongs. If the entity does not belong to any category some
    /// entities might require to explicitly call <see cref="M:Mafi.Core.Prototypes.LayoutEntityBuilderState`1.SetNoCategory" /> to make clear it was intentional.
    /// </summary>
    [MustUseReturnValue]
    public TState SetCategories(params Proto.ID[] categoryIds)
    {
      ImmutableArrayBuilder<ToolbarCategoryProto> immutableArrayBuilder = new ImmutableArrayBuilder<ToolbarCategoryProto>(categoryIds.Length);
      int i = 0;
      foreach (Proto.ID categoryId in categoryIds)
      {
        immutableArrayBuilder[i] = this.Builder.ProtosDb.GetOrThrow<ToolbarCategoryProto>(categoryId);
        ++i;
      }
      this.m_categories = new ImmutableArray<ToolbarCategoryProto>?(immutableArrayBuilder.GetImmutableArrayAndClear());
      return (this as TState).CheckNotNull<TState>();
    }

    /// <summary>
    /// Sets toolbar categories into which this entity belongs. If the entity does not belong to any category some
    /// entities might require to explicitly call <see cref="M:Mafi.Core.Prototypes.LayoutEntityBuilderState`1.SetNoCategory" /> to make clear it was intentional.
    /// </summary>
    [MustUseReturnValue]
    public TState SetCategories(ImmutableArray<ToolbarCategoryProto> categories)
    {
      this.m_categories = new ImmutableArray<ToolbarCategoryProto>?(categories);
      return (this as TState).CheckNotNull<TState>();
    }

    /// <summary>
    /// Explicitly sets that this entity does not belong to any toolbar category.
    /// </summary>
    [MustUseReturnValue]
    public TState SetNoCategory()
    {
      this.m_categories = new ImmutableArray<ToolbarCategoryProto>?(ImmutableArray<ToolbarCategoryProto>.Empty);
      return (this as TState).CheckNotNull<TState>();
    }

    protected ImmutableArray<ToolbarCategoryProto> GetCategoriesOrThrow()
    {
      return this.m_categories.HasValue ? this.m_categories.Value : throw new ProtoBuilderException(string.Format("Value 'Categories' of proto '{0}' is invalid.", (object) this.Id));
    }

    [MustUseReturnValue]
    public TState SetProductsFilter(Func<ProductProto, bool> predicate)
    {
      this.m_protoFilter = predicate.CheckNotNull<Func<ProductProto, bool>>();
      return (this as TState).CheckNotNull<TState>();
    }

    [MustUseReturnValue]
    public TState ShowTerrainDesignatorsOnCreation()
    {
      this.VisualizeTerrainDesignators = true;
      return (this as TState).CheckNotNull<TState>();
    }

    [MustUseReturnValue]
    public TState ShowTreeDesignatorsOnCreation()
    {
      this.VisualizeTreeDesignators = true;
      return (this as TState).CheckNotNull<TState>();
    }

    [MustUseReturnValue]
    public TState ShowTerrainMaterialsOnCreation(params TerrainMaterialProto[] terrainMaterials)
    {
      this.VisualizedTerrainMaterials = ImmutableArray.Create<TerrainMaterialProto>(terrainMaterials);
      return (this as TState).CheckNotNull<TState>();
    }

    [MustUseReturnValue]
    public TState ShowVirtualResourcesOnCreation(
      params VirtualResourceProductProto[] virtualResources)
    {
      this.VisualizedVirtualResources = ImmutableArray.Create<VirtualResourceProductProto>(virtualResources);
      return (this as TState).CheckNotNull<TState>();
    }

    [MustUseReturnValue]
    public TState SetMaterialColor(ColorRgba color)
    {
      this.MaterialColor = color;
      return (this as TState).CheckNotNull<TState>();
    }
  }
}
