// Decompiled with JetBrains decompiler
// Type: Mafi.Core.SaveGame.CustomSerializer.CustomEntitiesSerializer
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Buildings.Farms;
using Mafi.Core.Buildings.Forestry;
using Mafi.Core.Buildings.Mine;
using Mafi.Core.Buildings.Storages;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Entities.Validators;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Factory.Transports;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Core.Vehicles;
using Mafi.Core.World.Entities;
using System;
using System.Linq;

#nullable disable
namespace Mafi.Core.SaveGame.CustomSerializer
{
  public class CustomEntitiesSerializer
  {
    private readonly EntitiesManager m_entitiesManager;
    private readonly EntitiesCreator m_entitiesCreator;
    private readonly TransportsManager m_transportsManager;
    private readonly VehiclesManager m_vehiclesManager;
    private readonly LayoutEntityAddRequestFactory m_addRequestFactory;

    public CustomEntitiesSerializer(
      EntitiesManager entitiesManager,
      EntitiesCreator entitiesCreator,
      TransportsManager transportsManager,
      VehiclesManager vehiclesManager,
      LayoutEntityAddRequestFactory addRequestFactory)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entitiesManager = entitiesManager;
      this.m_entitiesCreator = entitiesCreator;
      this.m_transportsManager = transportsManager;
      this.m_vehiclesManager = vehiclesManager;
      this.m_addRequestFactory = addRequestFactory;
    }

    public void WriteTo(CustomTextWriter writer)
    {
      writer.WriteSectionStart("ENTITIES");
      writer.WriteInt(this.m_entitiesManager.EntitiesCount);
      writer.WriteInt(this.m_vehiclesManager.MaxVehiclesLimit);
      writer.WriteNewLine();
      foreach (IEntity entity1 in this.m_entitiesManager.Entities)
      {
        writer.WriteInt(entity1.Id.Value);
        switch (entity1)
        {
          case ILayoutEntity entity2:
            writer.WriteSectionStart("LAYOUT_ENTITY");
            this.writeLayoutEntity(entity2, writer);
            break;
          case Transport transport:
            writer.WriteSectionStart("TRANSPORT");
            this.writeTransport(transport, writer);
            break;
          case TransportPillar tp:
            writer.WriteSectionStart("PILLAR");
            this.writeTransportPillar(tp, writer);
            break;
          case Vehicle vehicle:
            writer.WriteSectionStart("VEHICLE");
            this.writeVehicle(vehicle, writer);
            break;
          default:
            writer.WriteSectionStart("UNKNOWN_ENTITY");
            writer.WriteString(entity1.GetType().Name);
            Log.Warning(string.Format("Unhandled entity {0} ({1}), was not saved.", (object) entity1, (object) entity1.GetType().Name));
            break;
        }
        writer.WriteSectionEnd();
      }
      writer.WriteSectionEnd();
      writer.WriteSectionStart("ASSIGNED_VEHICLES");
      Lyst<IEntityAssignedWithVehicles> lyst = this.m_entitiesManager.Entities.AsEnumerable().OfType<IEntityAssignedWithVehicles>().Where<IEntityAssignedWithVehicles>((Func<IEntityAssignedWithVehicles, bool>) (x => x.AllSpawnedVehicles().IsNotEmpty<Vehicle>())).ToLyst<IEntityAssignedWithVehicles>();
      writer.WriteInt(lyst.Count);
      foreach (IEntityAssignedWithVehicles entity in lyst)
      {
        ImmutableArray<Vehicle> immutableArray = entity.AllSpawnedVehicles().ToImmutableArray<Vehicle>();
        writer.WriteInt(entity.Id.Value);
        writer.WriteArray<Vehicle>(immutableArray, (Action<CustomTextWriter, Vehicle>) ((w, x) => w.WriteInt(x.Id.Value)));
        writer.WriteNewLine();
      }
      writer.WriteSectionEnd();
    }

    public void ReadFrom(CustomTextReader reader)
    {
      if (this.m_entitiesManager.Entities.AsEnumerable().Any<IEntity>((Func<IEntity, bool>) (x => !(x is WorldMapEntity))))
        throw new Exception("There are already some entities:\n" + this.m_entitiesManager.Entities.AsEnumerable().Where<IEntity>((Func<IEntity, bool>) (x => !(x is WorldMapEntity))).Select<IEntity, string>((Func<IEntity, string>) (x => x.ToString())).JoinStrings("\n"));
      reader.ReadSectionStart("ENTITIES");
      int capacity = reader.ReadInt();
      this.m_vehiclesManager.IncreaseVehicleLimit((reader.ReadInt() - this.m_vehiclesManager.MaxVehiclesLimit).Max(1));
      reader.ReadNewLine();
      Dict<int, Option<Entity>> dict = new Dict<int, Option<Entity>>(capacity);
      for (int index = 0; index < capacity; ++index)
      {
        int key = reader.ReadInt();
        string str = reader.ReadSectionStart();
        Option<Entity> option;
        switch (str)
        {
          case "LAYOUT_ENTITY":
            option = this.readLayoutEntity(reader).As<Entity>();
            break;
          case "TRANSPORT":
            option = this.readTransport(reader).As<Entity>();
            break;
          case "PILLAR":
            option = this.readTransportPillar(reader).As<Entity>();
            break;
          case "VEHICLE":
            option = this.readVehicle(reader).As<Entity>();
            break;
          case "UNKNOWN_ENTITY":
            option = Option<Entity>.None;
            reader.ReadString();
            break;
          default:
            throw new Exception("Unhandled section " + str);
        }
        dict.AddAndAssertNew(key, option);
        reader.ReadSectionEnd();
      }
      reader.ReadSectionEnd();
      reader.ReadSectionStart("ASSIGNED_VEHICLES");
      int num = reader.ReadInt();
      for (int index = 0; index < num; ++index)
      {
        int key1 = reader.ReadInt();
        Option<Entity> option1;
        bool flag = dict.TryGetValue(key1, out option1) && option1.HasValue && option1.Value is IEntityAssignedWithVehicles;
        ImmutableArray<int> immutableArray = reader.ReadArray<int>((Func<CustomTextReader, int>) (r => r.ReadInt()));
        if (flag)
        {
          IEntityAssignedWithVehicles assignedWithVehicles = (IEntityAssignedWithVehicles) option1.Value;
          foreach (int key2 in immutableArray)
          {
            Option<Entity> option2;
            if (dict.TryGetValue(key2, out option2) && option2.HasValue && option2.Value is Vehicle vehicle)
              assignedWithVehicles.AssignVehicle(vehicle);
            else
              Log.Warning(string.Format("Failed to assign vehicle #{0} to '{1}', vehicle was not loaded.", (object) key2, (object) assignedWithVehicles));
          }
        }
        else
          Log.Warning(string.Format("Failed to load assigned entities to entity #{0}, entity was no loaded.", (object) key1));
        reader.ReadNewLine();
      }
      reader.ReadSectionEnd();
    }

    private void writeLayoutEntity(ILayoutEntity entity, CustomTextWriter writer)
    {
      writer.WriteProto((Proto) entity.Prototype);
      writer.WriteVector3i(entity.Transform.Position.Vector3i);
      writer.WriteInt(entity.Transform.Rotation.AngleIndex);
      writer.WriteBool(entity.Transform.IsReflected);
      writer.WriteBool(entity.IsPaused);
      writer.WriteNewLine();
      if (entity is Machine machine)
      {
        writer.WriteSectionStart("MACHINE");
        if (machine.RecipesAssigned.Count == machine.Prototype.Recipes.Count)
        {
          writer.WriteInt(-1);
        }
        else
        {
          writer.WriteInt(machine.RecipesAssigned.Count);
          foreach (RecipeProto recipeProto in machine.RecipesAssigned)
            writer.WriteString(recipeProto.Id.Value);
        }
        writer.WriteSectionEnd();
      }
      if (entity is Storage storage)
      {
        writer.WriteSectionStart("STORAGE");
        writer.WriteBool(storage.StoredProduct.HasValue);
        if (storage.StoredProduct.HasValue)
        {
          writer.WriteProto((Proto) storage.StoredProduct.Value);
          writer.WriteInt(storage.CurrentQuantity.Value);
        }
        writer.WriteInt(storage.ImportUntilPercent.RawValue);
        writer.WriteInt(storage.ExportFromPercent.RawValue);
        writer.WriteBool(storage.IsGodModeEnabled);
        writer.WriteSectionEnd();
      }
      if (entity is Farm farm)
      {
        writer.WriteSectionStart("FARM");
        writer.WriteInt(farm.Schedule.Length);
        foreach (Option<CropProto> option in farm.Schedule)
        {
          writer.WriteBool(option.HasValue);
          if (option.HasValue)
            writer.WriteProto((Proto) option.Value);
        }
        writer.WriteSectionEnd();
      }
      if (entity is MineTower mineTower)
      {
        writer.WriteSectionStart("MINE_TOWER");
        writer.WriteVector2i(mineTower.Area.Origin.Vector2i);
        writer.WriteVector2i(mineTower.Area.Size.Vector2i);
        writer.WriteSectionEnd();
      }
      if (entity is ForestryTower forestryTower)
      {
        writer.WriteSectionStart("FORESTRY_TOWER");
        writer.WriteVector2i(forestryTower.Area.Origin.Vector2i);
        writer.WriteVector2i(forestryTower.Area.Size.Vector2i);
        writer.WriteSectionEnd();
      }
      writer.WriteSectionStart("NO_MORE_SECTIONS");
      writer.WriteSectionEnd();
    }

    private Option<StaticEntity> readLayoutEntity(CustomTextReader reader)
    {
      LayoutEntityProto proto1;
      bool flag1 = reader.TryReadProto<LayoutEntityProto>(out proto1);
      TileTransform transform = new TileTransform(new Tile3i(reader.ReadVector3i()), new Rotation90(reader.ReadInt()), reader.ReadBool());
      bool isPaused = reader.ReadBool();
      reader.ReadNewLine();
      StaticEntity entity = (StaticEntity) null;
      if (flag1)
      {
        if (!this.m_entitiesCreator.TryCreateStaticEntity((StaticEntityProto) proto1, transform, out entity))
          throw new Exception(string.Format("Failed to create entity '{0}'.", (object) proto1));
        if (this.m_entitiesManager.CanAdd((IEntityAddRequest) this.m_addRequestFactory.CreateRequestFor<LayoutEntityProto>(proto1, new EntityAddRequestData(transform))).IsError)
          Log.Warning(string.Format("Entity '{0}' could not be built, forcing it.", (object) entity));
        this.m_entitiesManager.AddEntityNoChecks((IEntity) entity);
        entity.MakeFullyConstructed();
      }
      string str;
      do
      {
        str = reader.ReadSectionStart();
        switch (str)
        {
          case "MACHINE":
            if (entity is Machine machine)
              machine.ClearAssignedRecipes();
            int num1 = reader.ReadInt();
            if (num1 == -1)
            {
              if (machine != null)
              {
                foreach (RecipeProto recipe in machine.Prototype.Recipes)
                  machine.AssignRecipe(recipe);
                break;
              }
              break;
            }
            for (int index = 0; index < num1; ++index)
            {
              string recipeId = reader.ReadString();
              if (machine != null)
              {
                RecipeProto recipe = machine.Prototype.Recipes.FirstOrDefault<RecipeProto>((Predicate<RecipeProto>) (x => x.Id.Value == recipeId));
                if ((Proto) recipe == (Proto) null)
                  Log.Warning(string.Format("Failed to activate recipe '{0}' in '{1}'.", (object) recipeId, (object) machine));
                else
                  machine.AssignRecipe(recipe);
              }
            }
            break;
          case "STORAGE":
            Storage storage = entity as Storage;
            if (reader.ReadBool())
            {
              ProductProto proto2;
              bool flag2 = reader.TryReadProto<ProductProto>(out proto2);
              Quantity quantity = new Quantity(reader.ReadInt());
              if (flag2 && storage != null)
                storage.Cheat_NewProduct(proto2, new Quantity?(quantity));
            }
            Percent percent1 = Percent.FromRaw(reader.ReadInt());
            Percent percent2 = Percent.FromRaw(reader.ReadInt());
            bool isEnabled = reader.ReadBool();
            if (storage != null)
            {
              if (storage.ImportUntilPercent != percent1)
                storage.SetImportPercent(percent1);
              if (storage.ExportFromPercent != percent2)
                storage.SetImportPercent(percent2);
              storage.Cheat_SetGodMode(isEnabled);
              break;
            }
            break;
          case "FARM":
            Farm farm = entity as Farm;
            int num2 = reader.ReadInt();
            for (int slot = 0; slot < num2; ++slot)
            {
              Option<CropProto> crop = Option<CropProto>.None;
              CropProto proto3;
              if (reader.ReadBool() && reader.TryReadProto<CropProto>(out proto3))
                crop = (Option<CropProto>) proto3;
              farm?.AssignCropToSlot(crop, slot);
            }
            break;
          case "MINE_TOWER":
            MineTower mineTower = entity as MineTower;
            Vector2i coord1 = reader.ReadVector2i();
            Vector2i vector2i1 = reader.ReadVector2i();
            if (mineTower != null)
            {
              mineTower.SetNewArea(new RectangleTerrainArea2i(new Tile2i(coord1), new RelTile2i(vector2i1)));
              break;
            }
            break;
          case "FORESTRY_TOWER":
            ForestryTower forestryTower = entity as ForestryTower;
            Vector2i coord2 = reader.ReadVector2i();
            Vector2i vector2i2 = reader.ReadVector2i();
            if (forestryTower != null)
            {
              forestryTower.SetNewArea(new RectangleTerrainArea2i(new Tile2i(coord2), new RelTile2i(vector2i2)));
              break;
            }
            break;
        }
        reader.ReadSectionEnd();
      }
      while (str != "NO_MORE_SECTIONS");
      entity?.SetPaused(isPaused);
      return (Option<StaticEntity>) entity;
    }

    private void writeTransport(Transport transport, CustomTextWriter writer)
    {
      writer.WriteProto((Proto) transport.Prototype);
      writer.WriteInt(transport.StartDirection.DirectionIndex);
      writer.WriteInt(transport.EndDirection.DirectionIndex);
      writer.WriteArray<Tile3i>(transport.Trajectory.Pivots, (Action<CustomTextWriter, Tile3i>) ((w, x) => w.WriteVector3i(x.Vector3i)));
    }

    private Option<Transport> readTransport(CustomTextReader reader)
    {
      TransportProto proto;
      bool flag = reader.TryReadProto<TransportProto>(out proto);
      RelTile3i relTile3i1 = new Direction90(reader.ReadInt()).ToTileDirection().ExtendZ(0);
      RelTile3i relTile3i2 = new Direction90(reader.ReadInt()).ToTileDirection().ExtendZ(0);
      ImmutableArray<Tile3i> pivots = reader.ReadArray<Tile3i>((Func<CustomTextReader, Tile3i>) (r => new Tile3i(r.ReadVector3i())));
      if (reader.SaveVersion <= 1)
        reader.ReadArray<ThicknessTilesI>((Func<CustomTextReader, ThicknessTilesI>) (r => new ThicknessTilesI(r.ReadInt())));
      if (!flag)
        return Option<Transport>.None;
      TransportTrajectory trajectory;
      string error;
      if (!TransportTrajectory.TryCreateFromPivots(proto, pivots, new RelTile3i?(relTile3i1), new RelTile3i?(relTile3i2), out trajectory, out error))
      {
        Log.Warning("Failed to create transport trajectory: " + error);
        return Option<Transport>.None;
      }
      bool addPillars = reader.SaveVersion <= 2;
      Transport transportNoChecks = this.m_transportsManager.CreateAndAddTransportNoChecks(trajectory, addPillars);
      transportNoChecks.MakeFullyConstructed();
      return (Option<Transport>) transportNoChecks;
    }

    private void writeTransportPillar(TransportPillar tp, CustomTextWriter writer)
    {
      writer.WriteProto((Proto) tp.Prototype);
      writer.WriteVector3i(tp.CenterTile.Vector3i);
      writer.WriteInt(tp.Height.Value);
    }

    private Option<TransportPillar> readTransportPillar(CustomTextReader reader)
    {
      bool flag = reader.TryReadProto<TransportPillarProto>(out TransportPillarProto _);
      Tile3i origin = new Tile3i(reader.ReadVector3i());
      ThicknessTilesI height = new ThicknessTilesI(reader.ReadInt());
      return !flag ? Option<TransportPillar>.None : (Option<TransportPillar>) this.m_transportsManager.BuildOrReplacePillarNoChecks(origin, height);
    }

    private void writeVehicle(Vehicle vehicle, CustomTextWriter writer)
    {
      writer.WriteProto((Proto) vehicle.Prototype);
      writer.WriteVector2f(vehicle.Position2f.Vector2f);
      writer.WriteFix32(vehicle.Direction.Degrees);
    }

    private Option<Vehicle> readVehicle(CustomTextReader reader)
    {
      DynamicGroundEntityProto proto;
      bool flag = reader.TryReadProto<DynamicGroundEntityProto>(out proto);
      Tile2f position = new Tile2f(reader.ReadVector2f());
      AngleDegrees1f direction = AngleDegrees1f.FromDegrees(reader.ReadFix32());
      if (!flag)
        return Option<Vehicle>.None;
      Vehicle vehicle;
      if (!this.m_entitiesCreator.TryCreateVehicle(proto, out vehicle))
      {
        Log.Warning(string.Format("Failed to create vehicle '{0}'.", (object) proto));
        return Option<Vehicle>.None;
      }
      this.m_entitiesManager.AddEntityNoChecks((IEntity) vehicle);
      vehicle.Spawn(position, direction);
      return (Option<Vehicle>) vehicle;
    }
  }
}
