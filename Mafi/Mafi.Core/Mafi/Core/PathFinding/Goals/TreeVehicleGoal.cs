// Decompiled with JetBrains decompiler
// Type: Mafi.Core.PathFinding.Goals.TreeVehicleGoal
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Designation;
using Mafi.Core.Terrain.Trees;
using Mafi.Core.Vehicles;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.PathFinding.Goals
{
  [MemberRemovedInSaveVersion("m_treeManager", 140, typeof (ITreesManager), 0, false)]
  [GenerateSerializer(false, null, 0)]
  public sealed class TreeVehicleGoal : AnnularVehicleGoal
  {
    [NewInSaveVersion(140, null, null, typeof (TreeVehicleGoal.Factory), null)]
    private readonly TreeVehicleGoal.Factory m_factory;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public TreeId? GoalTreeId { get; private set; }

    public RelTile1i TreeDistance => this.Distance;

    internal TreeVehicleGoal(TreeVehicleGoal.Factory factory)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(factory.m_vehicleSurfaceProvider, factory.m_terrainManager);
      this.m_factory = factory;
    }

    public void Initialize(TreeId treeId, RelTile1i treeDistance)
    {
      Assert.That<bool>(this.m_factory.m_treeManager.HasTree(treeId)).IsTrue("Creating goal to non-existing tree.");
      this.Initialize((Tile2i) treeId.Position, treeDistance);
      this.GoalTreeId = new TreeId?(treeId);
    }

    protected override bool ShouldCheckGoalHeights(
      int retryNumber,
      out HeightTilesF goalHeightLow,
      out HeightTilesF goalHeightHigh)
    {
      HeightTilesF height = this.m_terrainManager[(Tile2i) this.GoalTreeId.Value.Position].Height;
      goalHeightLow = height.TilesHeightFloored.HeightTilesF - ThicknessTilesF.One * retryNumber;
      goalHeightHigh = height.TilesHeightCeiled.HeightTilesF + ThicknessTilesF.One * retryNumber;
      return true;
    }

    public override bool IsGoalValid(PathFindingEntity vehicle, out bool retryPf)
    {
      return base.IsGoalValid(vehicle, out retryPf) && this.m_factory.m_treeManager.HasTree(this.GoalTreeId.Value);
    }

    public override void OnNavigationResult(bool isSuccess, IPathFindingVehicle vehicle)
    {
      if (!this.GoalTreeId.HasValue)
        return;
      if (isSuccess)
        this.m_factory.UnreachablesManager.MarkReachableFor(this.GoalTreeId.Value, vehicle);
      else
        this.m_factory.UnreachablesManager.MarkUnreachableFor(this.GoalTreeId.Value, vehicle);
    }

    public override string ToString()
    {
      TreeId? goalTreeId = this.GoalTreeId;
      ref TreeId? local = ref goalTreeId;
      return string.Format("Tree at {0} +- {1}", (object) (local.HasValue ? new Tile2iSlim?(local.GetValueOrDefault().Position) : new Tile2iSlim?()), (object) this.TreeDistance);
    }

    public static void Serialize(TreeVehicleGoal value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TreeVehicleGoal>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TreeVehicleGoal.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteNullableStruct<TreeId>(this.GoalTreeId);
      TreeVehicleGoal.Factory.Serialize(this.m_factory, writer);
    }

    public static TreeVehicleGoal Deserialize(BlobReader reader)
    {
      TreeVehicleGoal treeVehicleGoal;
      if (reader.TryStartClassDeserialization<TreeVehicleGoal>(out treeVehicleGoal))
        reader.EnqueueDataDeserialization((object) treeVehicleGoal, TreeVehicleGoal.s_deserializeDataDelayedAction);
      return treeVehicleGoal;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.GoalTreeId = reader.ReadNullableStruct<TreeId>();
      reader.SetField<TreeVehicleGoal>(this, "m_factory", reader.LoadedSaveVersion >= 140 ? (object) TreeVehicleGoal.Factory.Deserialize(reader) : (object) (TreeVehicleGoal.Factory) null);
      if (reader.LoadedSaveVersion < 140)
        reader.RegisterResolvedMember<TreeVehicleGoal>(this, "m_factory", typeof (TreeVehicleGoal.Factory), true);
      if (reader.LoadedSaveVersion >= 140)
        return;
      reader.ReadGenericAs<ITreesManager>();
    }

    static TreeVehicleGoal()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TreeVehicleGoal.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VehicleGoalBase) obj).SerializeData(writer));
      TreeVehicleGoal.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VehicleGoalBase) obj).DeserializeData(reader));
    }

    [GlobalDependency(RegistrationMode.AsSelf, false, false)]
    [OnlyForSaveCompatibility("This does not need a serializer!")]
    [GenerateSerializer(false, null, 0)]
    public new sealed class Factory
    {
      internal readonly IVehicleSurfaceProvider m_vehicleSurfaceProvider;
      internal readonly ITreesManager m_treeManager;
      internal readonly TerrainManager m_terrainManager;
      [NewInSaveVersion(140, null, null, typeof (UnreachableTerrainDesignationsManager), null)]
      internal readonly UnreachableTerrainDesignationsManager UnreachablesManager;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public Factory(
        IVehicleSurfaceProvider vehicleSurfaceProvider,
        ITreesManager treeManager,
        TerrainManager terrainManager,
        UnreachableTerrainDesignationsManager unreachablesManager)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_vehicleSurfaceProvider = vehicleSurfaceProvider;
        this.m_treeManager = treeManager;
        this.m_terrainManager = terrainManager;
        this.UnreachablesManager = unreachablesManager;
      }

      public TreeVehicleGoal Create(TreeId treeId, RelTile1i tolerance)
      {
        TreeVehicleGoal treeVehicleGoal = new TreeVehicleGoal(this);
        treeVehicleGoal.Initialize(treeId, tolerance);
        return treeVehicleGoal;
      }

      public static void Serialize(TreeVehicleGoal.Factory value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<TreeVehicleGoal.Factory>(value))
          return;
        writer.EnqueueDataSerialization((object) value, TreeVehicleGoal.Factory.s_serializeDataDelayedAction);
      }

      private void SerializeData(BlobWriter writer)
      {
        TerrainManager.Serialize(this.m_terrainManager, writer);
        writer.WriteGeneric<ITreesManager>(this.m_treeManager);
        writer.WriteGeneric<IVehicleSurfaceProvider>(this.m_vehicleSurfaceProvider);
        UnreachableTerrainDesignationsManager.Serialize(this.UnreachablesManager, writer);
      }

      public static TreeVehicleGoal.Factory Deserialize(BlobReader reader)
      {
        TreeVehicleGoal.Factory factory;
        if (reader.TryStartClassDeserialization<TreeVehicleGoal.Factory>(out factory))
          reader.EnqueueDataDeserialization((object) factory, TreeVehicleGoal.Factory.s_deserializeDataDelayedAction);
        return factory;
      }

      private void DeserializeData(BlobReader reader)
      {
        reader.SetField<TreeVehicleGoal.Factory>(this, "m_terrainManager", (object) TerrainManager.Deserialize(reader));
        reader.SetField<TreeVehicleGoal.Factory>(this, "m_treeManager", (object) reader.ReadGenericAs<ITreesManager>());
        reader.SetField<TreeVehicleGoal.Factory>(this, "m_vehicleSurfaceProvider", (object) reader.ReadGenericAs<IVehicleSurfaceProvider>());
        reader.SetField<TreeVehicleGoal.Factory>(this, "UnreachablesManager", reader.LoadedSaveVersion >= 140 ? (object) UnreachableTerrainDesignationsManager.Deserialize(reader) : (object) (UnreachableTerrainDesignationsManager) null);
        if (reader.LoadedSaveVersion >= 140)
          return;
        reader.RegisterResolvedMember<TreeVehicleGoal.Factory>(this, "UnreachablesManager", typeof (UnreachableTerrainDesignationsManager), true);
      }

      static Factory()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        TreeVehicleGoal.Factory.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((TreeVehicleGoal.Factory) obj).SerializeData(writer));
        TreeVehicleGoal.Factory.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((TreeVehicleGoal.Factory) obj).DeserializeData(reader));
      }
    }
  }
}
