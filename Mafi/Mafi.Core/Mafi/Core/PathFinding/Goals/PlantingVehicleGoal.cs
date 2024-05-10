// Decompiled with JetBrains decompiler
// Type: Mafi.Core.PathFinding.Goals.PlantingVehicleGoal
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Designation;
using Mafi.Core.Vehicles;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.PathFinding.Goals
{
  [GenerateSerializer(false, null, 0)]
  public sealed class PlantingVehicleGoal : AnnularVehicleGoal
  {
    [NewInSaveVersion(140, null, null, typeof (PlantingVehicleGoal.Factory), null)]
    private readonly PlantingVehicleGoal.Factory m_factory;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    internal PlantingVehicleGoal(PlantingVehicleGoal.Factory factory)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(factory.m_vehicleSurfaceProvider, factory.m_terrainManager);
      this.m_factory = factory;
    }

    protected override bool ShouldCheckGoalHeights(
      int retryNumber,
      out HeightTilesF goalHeightLow,
      out HeightTilesF goalHeightHigh)
    {
      HeightTilesF height = this.m_terrainManager[this.GoalPosition.Value].Height;
      goalHeightLow = height.TilesHeightFloored.HeightTilesF - ThicknessTilesF.One * retryNumber;
      goalHeightHigh = height.TilesHeightCeiled.HeightTilesF + ThicknessTilesF.One * retryNumber;
      return true;
    }

    public override void OnNavigationResult(bool isSuccess, IPathFindingVehicle vehicle)
    {
      if (!this.GoalPosition.HasValue)
        return;
      if (isSuccess)
        this.m_factory.UnreachablesManager.MarkReachableFor(this.GoalPosition.Value, vehicle);
      else
        this.m_factory.UnreachablesManager.MarkUnreachableFor(this.GoalPosition.Value, vehicle);
    }

    public static void Serialize(PlantingVehicleGoal value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<PlantingVehicleGoal>(value))
        return;
      writer.EnqueueDataSerialization((object) value, PlantingVehicleGoal.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      PlantingVehicleGoal.Factory.Serialize(this.m_factory, writer);
    }

    public static PlantingVehicleGoal Deserialize(BlobReader reader)
    {
      PlantingVehicleGoal plantingVehicleGoal;
      if (reader.TryStartClassDeserialization<PlantingVehicleGoal>(out plantingVehicleGoal))
        reader.EnqueueDataDeserialization((object) plantingVehicleGoal, PlantingVehicleGoal.s_deserializeDataDelayedAction);
      return plantingVehicleGoal;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<PlantingVehicleGoal>(this, "m_factory", reader.LoadedSaveVersion >= 140 ? (object) PlantingVehicleGoal.Factory.Deserialize(reader) : (object) (PlantingVehicleGoal.Factory) null);
      if (reader.LoadedSaveVersion >= 140)
        return;
      reader.RegisterResolvedMember<PlantingVehicleGoal>(this, "m_factory", typeof (PlantingVehicleGoal.Factory), true);
    }

    static PlantingVehicleGoal()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      PlantingVehicleGoal.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VehicleGoalBase) obj).SerializeData(writer));
      PlantingVehicleGoal.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VehicleGoalBase) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    [OnlyForSaveCompatibility(null)]
    [GlobalDependency(RegistrationMode.AsSelf, false, false)]
    public new sealed class Factory
    {
      internal readonly IVehicleSurfaceProvider m_vehicleSurfaceProvider;
      internal readonly TerrainManager m_terrainManager;
      [NewInSaveVersion(140, null, null, typeof (IUnreachablesManager), null)]
      internal readonly IUnreachablesManager UnreachablesManager;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public Factory(
        IVehicleSurfaceProvider vehicleSurfaceProvider,
        TerrainManager terrainManager,
        IUnreachablesManager unreachablesManager)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_vehicleSurfaceProvider = vehicleSurfaceProvider;
        this.m_terrainManager = terrainManager;
        this.UnreachablesManager = unreachablesManager;
      }

      public PlantingVehicleGoal Create(Tile2i position, RelTile1i tolerance)
      {
        PlantingVehicleGoal plantingVehicleGoal = new PlantingVehicleGoal(this);
        plantingVehicleGoal.Initialize(position, tolerance);
        return plantingVehicleGoal;
      }

      public static void Serialize(PlantingVehicleGoal.Factory value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<PlantingVehicleGoal.Factory>(value))
          return;
        writer.EnqueueDataSerialization((object) value, PlantingVehicleGoal.Factory.s_serializeDataDelayedAction);
      }

      private void SerializeData(BlobWriter writer)
      {
        TerrainManager.Serialize(this.m_terrainManager, writer);
        writer.WriteGeneric<IVehicleSurfaceProvider>(this.m_vehicleSurfaceProvider);
        writer.WriteGeneric<IUnreachablesManager>(this.UnreachablesManager);
      }

      public static PlantingVehicleGoal.Factory Deserialize(BlobReader reader)
      {
        PlantingVehicleGoal.Factory factory;
        if (reader.TryStartClassDeserialization<PlantingVehicleGoal.Factory>(out factory))
          reader.EnqueueDataDeserialization((object) factory, PlantingVehicleGoal.Factory.s_deserializeDataDelayedAction);
        return factory;
      }

      private void DeserializeData(BlobReader reader)
      {
        reader.SetField<PlantingVehicleGoal.Factory>(this, "m_terrainManager", (object) TerrainManager.Deserialize(reader));
        reader.SetField<PlantingVehicleGoal.Factory>(this, "m_vehicleSurfaceProvider", (object) reader.ReadGenericAs<IVehicleSurfaceProvider>());
        reader.SetField<PlantingVehicleGoal.Factory>(this, "UnreachablesManager", reader.LoadedSaveVersion >= 140 ? (object) reader.ReadGenericAs<IUnreachablesManager>() : (object) (IUnreachablesManager) null);
        if (reader.LoadedSaveVersion >= 140)
          return;
        reader.RegisterResolvedMember<PlantingVehicleGoal.Factory>(this, "UnreachablesManager", typeof (IUnreachablesManager), true);
      }

      static Factory()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        PlantingVehicleGoal.Factory.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((PlantingVehicleGoal.Factory) obj).SerializeData(writer));
        PlantingVehicleGoal.Factory.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((PlantingVehicleGoal.Factory) obj).DeserializeData(reader));
      }
    }
  }
}
