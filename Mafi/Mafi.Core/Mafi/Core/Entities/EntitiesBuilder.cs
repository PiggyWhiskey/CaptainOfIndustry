// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.EntitiesBuilder
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Buildings.Cargo;
using Mafi.Core.Buildings.Cargo.Modules;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Entities.Validators;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Numerics;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Entities
{
  /// <summary>
  /// Helper class for convenient building entities such as buildings or vehicles.
  /// </summary>
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class EntitiesBuilder
  {
    private readonly EntitiesManager m_entitiesManager;
    private readonly TerrainManager m_terrainManager;
    private readonly ProtosDb m_protosDb;
    private readonly DependencyResolver m_resolver;
    private readonly LayoutEntityAddRequestFactory m_addRequestFactory;

    public EntitiesBuilder(
      EntitiesManager entitiesManager,
      TerrainManager terrainManager,
      ProtosDb protosDb,
      DependencyResolver resolver,
      LayoutEntityAddRequestFactory addRequestFactory)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entitiesManager = entitiesManager;
      this.m_terrainManager = terrainManager;
      this.m_protosDb = protosDb;
      this.m_resolver = resolver;
      this.m_addRequestFactory = addRequestFactory;
    }

    public bool CanBuildLayoutEntity(
      StaticEntityProto.ID id,
      Tile3i location,
      Rotation90 rotation,
      bool isReflected,
      out string error)
    {
      LayoutEntityProto proto;
      if (!this.m_protosDb.TryGetProto<LayoutEntityProto>((Proto.ID) id, out proto))
      {
        error = string.Format("Failed to get LayoutEntityProto with id: '{0}'.", (object) id);
        Log.Error(error);
        return false;
      }
      EntityValidationResult validationResult = this.m_entitiesManager.CanAdd((IEntityAddRequest) this.m_addRequestFactory.CreateRequestFor<LayoutEntityProto>(proto, new EntityAddRequestData(new TileTransform(location, rotation, isReflected))));
      error = validationResult.ErrorMessage;
      return validationResult.IsSuccess;
    }

    public bool CanBuildLayoutEntityApprox(
      StaticEntityProto.ID id,
      Tile2i fromLocation,
      Tile2i toLocation,
      HeightTilesI height,
      Lyst<KeyValuePair<Tile3i, Option<string>>> results,
      Rotation90 rotation = default (Rotation90),
      bool isReflected = false)
    {
      LineRasterizer lineRasterizer = new LineRasterizer(fromLocation.Vector2i, toLocation.Vector2i, false);
      bool flag1 = false;
      foreach (Vector2i coord in lineRasterizer)
      {
        Tile3i tile3i = new Tile3i(new Tile2i(coord), height.Value);
        string error;
        bool flag2 = this.CanBuildLayoutEntity(id, tile3i, rotation, isReflected, out error);
        flag1 |= flag2;
        results.Add(Make.Kvp<Tile3i, Option<string>>(tile3i, flag2 ? Option<string>.None : (Option<string>) (error ?? "")));
      }
      return flag1;
    }

    public bool CanBuildLayoutEntityApprox(
      StaticEntityProto.ID id,
      Tile2i fromLocation,
      Tile2i toLocation,
      HeightTilesI height,
      out Tile3i firstBuildPosition,
      Rotation90 rotation = default (Rotation90),
      bool isReflected = false,
      Option<Lyst<Pair<Tile3i, Option<string>>>> results = default (Option<Lyst<Pair<Tile3i, Option<string>>>>))
    {
      LineRasterizer lineRasterizer = new LineRasterizer(fromLocation.Vector2i, toLocation.Vector2i, false);
      firstBuildPosition = new Tile3i();
      foreach (Vector2i coord in lineRasterizer)
      {
        Tile3i tile3i = new Tile3i(new Tile2i(coord), height.Value);
        string error;
        bool flag = this.CanBuildLayoutEntity(id, tile3i, rotation, isReflected, out error);
        if (results.HasValue)
          results.Value.Add(Pair.Create<Tile3i, Option<string>>(tile3i, string.IsNullOrEmpty(error) ? Option<string>.None : (Option<string>) error));
        if (flag)
        {
          firstBuildPosition = tile3i;
          return true;
        }
      }
      return false;
    }

    /// <summary>
    /// Attempts to build a layout entity all locations between <paramref name="fromLocation" /> and <paramref name="toLocation" />
    /// </summary>
    public Option<T> TryBuildLayoutEntityApprox<T>(
      StaticEntityProto.ID id,
      Tile2i fromLocation,
      Tile2i toLocation,
      HeightTilesI height,
      Rotation90 rotation = default (Rotation90),
      bool isReflected = false,
      bool makeFullyConstructed = false)
      where T : class, ILayoutEntity
    {
      LineRasterizer lineRasterizer = new LineRasterizer(fromLocation.Vector2i, toLocation.Vector2i, false);
      Lyst<string> strings = new Lyst<string>();
      foreach (Vector2i coord in lineRasterizer)
      {
        Tile3i location = new Tile3i(new Tile2i(coord), height.Value);
        string error;
        if (this.CanBuildLayoutEntity(id, location, rotation, isReflected, out error))
          return (Option<T>) this.TryBuildLayoutEntity<T>(id, location, rotation, isReflected, makeFullyConstructed).Value;
        strings.Add(error);
      }
      Log.Error(string.Format("Failed to build layout entity '{0}' at all location between {1} and {2} ", (object) id, (object) fromLocation, (object) toLocation) + string.Format("at height {0}. Errors:\n{1}", (object) height, (object) strings.JoinStrings("\n")));
      return Option<T>.None;
    }

    public Option<T> TryBuildLayoutEntity<T>(
      StaticEntityProto.ID id,
      Tile3i location,
      Rotation90 rotation = default (Rotation90),
      bool isReflected = false,
      bool makeFullyConstructed = false)
      where T : class, ILayoutEntity
    {
      return this.TryBuildLayoutEntity<T>(id, new TileTransform(location, rotation, isReflected), makeFullyConstructed);
    }

    public Option<T> TryBuildLayoutEntity<T>(
      StaticEntityProto.ID id,
      TileTransform transform,
      bool makeFullyConstructed)
      where T : class, ILayoutEntity
    {
      Option<ILayoutEntity> option = this.TryBuildLayoutEntityUntyped(id, transform, makeFullyConstructed);
      return option.IsNone ? (Option<T>) Option.None : (Option<T>) (T) option.Value;
    }

    public Option<ILayoutEntity> TryBuildLayoutEntityUntyped(
      StaticEntityProto.ID id,
      TileTransform transform,
      bool makeFullyConstructed)
    {
      Option<LayoutEntityProto> option1 = this.m_protosDb.Get<LayoutEntityProto>((Proto.ID) id);
      if (option1.IsNone)
      {
        Log.Error(string.Format("Failed to get LayoutEntityProto with id: '{0}'.", (object) id));
        return (Option<ILayoutEntity>) Option.None;
      }
      Option<StaticEntity> option2 = this.m_resolver.TryInvokeFactoryHierarchy<StaticEntity>((object) option1.Value, (object) transform);
      if (!option2.HasValue)
      {
        Log.Error(string.Format("Failed to instantiate layout entity '{0}'.", (object) option1));
        return (Option<ILayoutEntity>) Option.None;
      }
      ILayoutEntity layoutEntity = (ILayoutEntity) option2.Value;
      EntityValidationResult validationResult = this.m_entitiesManager.TryAddEntity((IEntity) layoutEntity);
      if (!validationResult.IsSuccess)
      {
        Log.Error(string.Format("Failed to add entity '{0}' to the world. ", (object) layoutEntity) + "Error: '" + validationResult.ErrorMessage + "'");
        ((IEntityFriend) layoutEntity).Destroy();
        return (Option<ILayoutEntity>) Option.None;
      }
      if (makeFullyConstructed)
        ((StaticEntity) layoutEntity).MakeFullyConstructed();
      return layoutEntity.CreateOption<ILayoutEntity>();
    }

    public Option<CargoDepotModule> TryBuildCargoDepotModule(
      CargoDepot td,
      CargoDepotModuleProto.ID id,
      int slotIndex,
      bool makeFullyConstructed)
    {
      Option<CargoDepotModuleProto> option = this.m_protosDb.Get<CargoDepotModuleProto>((Proto.ID) id);
      if (option.IsNone)
        return (Option<CargoDepotModule>) Option.None;
      TileTransform moduleTransform = td.GetModuleTransform(slotIndex, option.Value);
      return this.TryBuildLayoutEntity<CargoDepotModule>((StaticEntityProto.ID) id, moduleTransform, makeFullyConstructed);
    }

    public bool TryCreateVehicles<T>(
      DynamicEntityProto.ID id,
      int count,
      Tile2i position,
      AngleDegrees1f direction,
      RelTile2i increment)
      where T : Vehicle
    {
      Tile2i position1 = position;
      for (int index = 0; index < count; ++index)
      {
        Option<T> vehicle = this.TryCreateVehicle<T>(id, position1, direction);
        if (vehicle.IsNone)
          return false;
        vehicle.Value.Maintenance.SetCurrentMaintenanceTo(310.Percent());
        position1 += increment;
      }
      return true;
    }

    public Option<T> TryCreateVehicle<T>(
      DynamicEntityProto.ID id,
      Tile2i position,
      AngleDegrees1f direction)
      where T : Vehicle
    {
      Option<DynamicEntityProto> option1 = this.m_protosDb.Get<DynamicEntityProto>((Proto.ID) id);
      if (option1.IsNone)
        return Option<T>.None;
      Option<Vehicle> option2 = (Option<Vehicle>) this.m_resolver.InvokeFactoryHierarchy<Vehicle>((object) option1.Value);
      if (option2.IsNone)
        return Option<T>.None;
      Option<T> vehicle = option2.As<T>();
      if (vehicle.IsNone)
        return Option<T>.None;
      EntityValidationResult validationResult = this.m_entitiesManager.TryAddEntity((IEntity) vehicle.Value);
      if (validationResult.IsError)
      {
        Log.Warning(string.Format("Cannot vehicle id '{0}'. Error: '{1}'", (object) id, (object) validationResult.ErrorMessage));
        return Option<T>.None;
      }
      option2.Value.Spawn(position.CenterTile2f, direction);
      return vehicle;
    }
  }
}
