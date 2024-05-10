// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Forestry.ForestryTowersManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Entities;
using Mafi.Core.Terrain;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Forestry
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  [GenerateSerializer(false, null, 0)]
  public class ForestryTowersManager : IForestryTowersManager
  {
    private Event<ForestryTower, EntityAddReason> m_onTowerAdded;
    private Event<ForestryTower, EntityRemoveReason> m_onTowerRemoved;
    private readonly Event<ForestryTower, RectangleTerrainArea2i> m_onAreaChange;
    private readonly EntitiesManager m_entitiesManager;
    private readonly Lyst<ForestryTower> m_forestryTowers;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public IEvent<ForestryTower, EntityAddReason> OnTowerAdded
    {
      get => (IEvent<ForestryTower, EntityAddReason>) this.m_onTowerAdded;
    }

    public IEvent<ForestryTower, EntityRemoveReason> OnTowerRemoved
    {
      get => (IEvent<ForestryTower, EntityRemoveReason>) this.m_onTowerRemoved;
    }

    /// <summary>
    /// Invoked when forestry tower is changed. Parameters are current forestry tower and old area.
    /// </summary>
    public IEvent<ForestryTower, RectangleTerrainArea2i> OnAreaChange
    {
      get => (IEvent<ForestryTower, RectangleTerrainArea2i>) this.m_onAreaChange;
    }

    public IIndexable<ForestryTower> Towers => (IIndexable<ForestryTower>) this.m_forestryTowers;

    public ForestryTowersManager(EntitiesManager entitiesManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_onTowerAdded = new Event<ForestryTower, EntityAddReason>();
      this.m_onTowerRemoved = new Event<ForestryTower, EntityRemoveReason>();
      this.m_onAreaChange = new Event<ForestryTower, RectangleTerrainArea2i>();
      this.m_forestryTowers = new Lyst<ForestryTower>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entitiesManager = entitiesManager;
      entitiesManager.EntityAddedFull.Add<ForestryTowersManager>(this, new Action<IEntity, EntityAddReason>(this.entityAdded));
      entitiesManager.EntityRemovedFull.Add<ForestryTowersManager>(this, new Action<IEntity, EntityRemoveReason>(this.entityRemoved));
    }

    [OnlyForSaveCompatibility(null)]
    [InitAfterLoad(InitPriority.ImmediatelyAfterSelfDeserialized)]
    private void fixEvents(int saveVersion)
    {
      if (saveVersion >= 123)
        return;
      this.m_onTowerAdded = new Event<ForestryTower, EntityAddReason>();
      this.m_onTowerRemoved = new Event<ForestryTower, EntityRemoveReason>();
    }

    private void entityAdded(IEntity entity, EntityAddReason addReason)
    {
      if (!(entity is ForestryTower forestryTower))
        return;
      this.m_forestryTowers.Add(forestryTower);
      this.m_onTowerAdded.Invoke(forestryTower, addReason);
    }

    private void entityRemoved(IEntity entity, EntityRemoveReason removeReason)
    {
      if (!(entity is ForestryTower forestryTower))
        return;
      Assert.That<bool>(this.m_forestryTowers.TryRemoveReplaceLast(forestryTower)).IsTrue();
      this.m_onTowerRemoved.Invoke(forestryTower, removeReason);
    }

    internal void InvokeOnAreaChanged(ForestryTower tower, RectangleTerrainArea2i oldArea)
    {
      this.m_onAreaChange.Invoke(tower, oldArea);
    }

    public static void Serialize(ForestryTowersManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ForestryTowersManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ForestryTowersManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      EntitiesManager.Serialize(this.m_entitiesManager, writer);
      Lyst<ForestryTower>.Serialize(this.m_forestryTowers, writer);
      Event<ForestryTower, RectangleTerrainArea2i>.Serialize(this.m_onAreaChange, writer);
      Event<ForestryTower, EntityAddReason>.Serialize(this.m_onTowerAdded, writer);
      Event<ForestryTower, EntityRemoveReason>.Serialize(this.m_onTowerRemoved, writer);
    }

    public static ForestryTowersManager Deserialize(BlobReader reader)
    {
      ForestryTowersManager forestryTowersManager;
      if (reader.TryStartClassDeserialization<ForestryTowersManager>(out forestryTowersManager))
        reader.EnqueueDataDeserialization((object) forestryTowersManager, ForestryTowersManager.s_deserializeDataDelayedAction);
      return forestryTowersManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<ForestryTowersManager>(this, "m_entitiesManager", (object) EntitiesManager.Deserialize(reader));
      reader.SetField<ForestryTowersManager>(this, "m_forestryTowers", (object) Lyst<ForestryTower>.Deserialize(reader));
      reader.SetField<ForestryTowersManager>(this, "m_onAreaChange", (object) Event<ForestryTower, RectangleTerrainArea2i>.Deserialize(reader));
      this.m_onTowerAdded = Event<ForestryTower, EntityAddReason>.Deserialize(reader);
      this.m_onTowerRemoved = Event<ForestryTower, EntityRemoveReason>.Deserialize(reader);
      this.fixEvents(reader.LoadedSaveVersion);
    }

    static ForestryTowersManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ForestryTowersManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ForestryTowersManager) obj).SerializeData(writer));
      ForestryTowersManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ForestryTowersManager) obj).DeserializeData(reader));
    }
  }
}
