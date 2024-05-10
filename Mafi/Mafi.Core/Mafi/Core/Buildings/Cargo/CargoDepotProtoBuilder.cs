// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Cargo.CargoDepotProtoBuilder
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;

#nullable disable
namespace Mafi.Core.Buildings.Cargo
{
  public sealed class CargoDepotProtoBuilder : IProtoBuilder
  {
    public ProtosDb ProtosDb => this.Registrator.PrototypesDb;

    public ProtoRegistrator Registrator { get; }

    public CargoDepotProtoBuilder(ProtoRegistrator registrator)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Registrator = registrator;
    }

    public CargoDepotProtoBuilder.State Start(string name, CargoDepotProto.ID labId)
    {
      return new CargoDepotProtoBuilder.State(this, labId, name);
    }

    public class State : LayoutEntityBuilderState<CargoDepotProtoBuilder.State>
    {
      private readonly CargoDepotProto.ID m_id;
      private ImmutableArray<CargoDepotProto.ModuleSlotPosition>? m_slots;
      private EntityProto.ID m_cargoShipProtoId;
      private Option<CargoDepotProto> m_nextTier;
      private ImmutableArray<ImmutableArray<RectangleTerrainArea2iRelative>>? m_reservedOceanAreasSets;
      private ImmutableArray<string>? m_animationPaths;

      public State(CargoDepotProtoBuilder builder, CargoDepotProto.ID id, string name)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector((IProtoBuilder) builder, (StaticEntityProto.ID) id, name);
        this.m_id = id;
      }

      public CargoDepotProtoBuilder.State SetSlots(
        RelTile2i origin,
        int width,
        int length,
        int count)
      {
        this.m_slots = new ImmutableArray<CargoDepotProto.ModuleSlotPosition>?(CargoDepotProtoBuilder.State.generateSlots(origin, width, length, count));
        return this;
      }

      public CargoDepotProtoBuilder.State SetNextTier(CargoDepotProto nextTier)
      {
        this.m_nextTier = (Option<CargoDepotProto>) nextTier.CheckNotNull<CargoDepotProto>();
        return this;
      }

      public CargoDepotProtoBuilder.State SetCargoShipProtoId(EntityProto.ID cargoShipProtoId)
      {
        this.m_cargoShipProtoId = cargoShipProtoId;
        return this;
      }

      public CargoDepotProtoBuilder.State SetReservedOceanAreasSets(
        ImmutableArray<ImmutableArray<RectangleTerrainArea2iRelative>> reservedOceanAreasSets,
        ImmutableArray<string> animationPaths)
      {
        this.m_reservedOceanAreasSets = new ImmutableArray<ImmutableArray<RectangleTerrainArea2iRelative>>?(reservedOceanAreasSets);
        this.m_animationPaths = new ImmutableArray<string>?(animationPaths);
        return this;
      }

      public CargoDepotProto BuildAndAdd()
      {
        CargoDepotProto.ID id = this.m_id;
        Proto.Str strings = this.Strings;
        EntityLayout layoutOrThrow = this.LayoutOrThrow;
        EntityCosts costs = this.Costs;
        ImmutableArray<CargoDepotProto.ModuleSlotPosition> moduleSlots = this.ValueOrThrow<ImmutableArray<CargoDepotProto.ModuleSlotPosition>>(this.m_slots, "Slots");
        EntityProto.ID cargoShipProtoId = this.m_cargoShipProtoId;
        Option<CargoDepotProto> nextTier = this.m_nextTier;
        ImmutableArray<ImmutableArray<RectangleTerrainArea2iRelative>> reservedOceanAreasSets = this.ValueOrThrow<ImmutableArray<ImmutableArray<RectangleTerrainArea2iRelative>>>(this.m_reservedOceanAreasSets, "ReservedOceanAreas");
        LayoutEntityProto.Gfx graphics;
        if (!string.IsNullOrEmpty(this.PrefabPath))
        {
          string prefabPath = this.PrefabPath;
          RelTile3f prefabOrigin = this.PrefabOrigin;
          Option<string> customIconPath = this.CustomIconPath;
          ColorRgba empty = ColorRgba.Empty;
          ImmutableArray<ToolbarCategoryProto>? nullable = new ImmutableArray<ToolbarCategoryProto>?(this.GetCategoriesOrThrow());
          LayoutEntityProto.VisualizedLayers? visualizedLayers = new LayoutEntityProto.VisualizedLayers?();
          ImmutableArray<ToolbarCategoryProto>? categories = nullable;
          ImmutableArray<string> instancedRenderingExcludedObjects = new ImmutableArray<string>();
          graphics = new LayoutEntityProto.Gfx(prefabPath, prefabOrigin, customIconPath, empty, visualizedLayers: visualizedLayers, categories: categories, instancedRenderingExcludedObjects: instancedRenderingExcludedObjects);
        }
        else
          graphics = LayoutEntityProto.Gfx.Empty;
        ImmutableArray<string> dockingAnimationsPrefabPaths = this.ValueOrThrow<ImmutableArray<string>>(this.m_animationPaths, "AnimationPaths");
        return this.AddToDb<CargoDepotProto>(new CargoDepotProto(id, strings, layoutOrThrow, costs, moduleSlots, cargoShipProtoId, nextTier, reservedOceanAreasSets, graphics, dockingAnimationsPrefabPaths));
      }

      private static ImmutableArray<CargoDepotProto.ModuleSlotPosition> generateSlots(
        RelTile2i origin,
        int width,
        int length,
        int count)
      {
        ImmutableArrayBuilder<CargoDepotProto.ModuleSlotPosition> immutableArrayBuilder = new ImmutableArrayBuilder<CargoDepotProto.ModuleSlotPosition>(count);
        for (int i = 0; i < count; ++i)
          immutableArrayBuilder[i] = new CargoDepotProto.ModuleSlotPosition(origin.AddY((i + 1) * width), new RelTile2i(length, width));
        return immutableArrayBuilder.GetImmutableArrayAndClear();
      }
    }
  }
}
