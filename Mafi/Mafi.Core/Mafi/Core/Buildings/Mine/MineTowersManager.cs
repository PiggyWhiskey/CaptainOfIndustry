// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Mine.MineTowersManager
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
namespace Mafi.Core.Buildings.Mine
{
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class MineTowersManager
  {
    private readonly Event<MineTower, EntityAddReason> m_onTowerAdded;
    private readonly Event<MineTower, EntityRemoveReason> m_onTowerRemoved;
    private readonly Event<MineTower, RectangleTerrainArea2i> m_onAreaChange;
    private readonly EntitiesManager m_entitiesManager;
    private readonly Lyst<MineTower> m_mineTowers;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public IEvent<MineTower, EntityAddReason> OnTowerAdded
    {
      get => (IEvent<MineTower, EntityAddReason>) this.m_onTowerAdded;
    }

    public IEvent<MineTower, EntityRemoveReason> OnTowerRemoved
    {
      get => (IEvent<MineTower, EntityRemoveReason>) this.m_onTowerRemoved;
    }

    /// <summary>
    /// Invoked when mine tower is changed. Parameters are current mine tower and old area.
    /// </summary>
    public IEvent<MineTower, RectangleTerrainArea2i> OnAreaChange
    {
      get => (IEvent<MineTower, RectangleTerrainArea2i>) this.m_onAreaChange;
    }

    public IIndexable<MineTower> Towers => (IIndexable<MineTower>) this.m_mineTowers;

    public MineTowersManager(EntitiesManager entitiesManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_onTowerAdded = new Event<MineTower, EntityAddReason>();
      this.m_onTowerRemoved = new Event<MineTower, EntityRemoveReason>();
      this.m_onAreaChange = new Event<MineTower, RectangleTerrainArea2i>();
      this.m_mineTowers = new Lyst<MineTower>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entitiesManager = entitiesManager;
      entitiesManager.EntityAddedFull.Add<MineTowersManager>(this, new Action<IEntity, EntityAddReason>(this.entityAdded));
      entitiesManager.EntityRemovedFull.Add<MineTowersManager>(this, new Action<IEntity, EntityRemoveReason>(this.entityRemoved));
    }

    private void entityAdded(IEntity entity, EntityAddReason addReason)
    {
      if (!(entity is MineTower mineTower))
        return;
      this.m_mineTowers.Add(mineTower);
      this.m_onTowerAdded.Invoke(mineTower, addReason);
    }

    private void entityRemoved(IEntity entity, EntityRemoveReason removeReason)
    {
      if (!(entity is MineTower mineTower))
        return;
      Assert.That<bool>(this.m_mineTowers.TryRemoveReplaceLast(mineTower)).IsTrue();
      this.m_onTowerRemoved.Invoke(mineTower, removeReason);
    }

    internal void InvokeOnAreaChanged(MineTower tower, RectangleTerrainArea2i oldArea)
    {
      this.m_onAreaChange.Invoke(tower, oldArea);
    }

    public static void Serialize(MineTowersManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<MineTowersManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, MineTowersManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      EntitiesManager.Serialize(this.m_entitiesManager, writer);
      Lyst<MineTower>.Serialize(this.m_mineTowers, writer);
      Event<MineTower, RectangleTerrainArea2i>.Serialize(this.m_onAreaChange, writer);
      Event<MineTower, EntityAddReason>.Serialize(this.m_onTowerAdded, writer);
      Event<MineTower, EntityRemoveReason>.Serialize(this.m_onTowerRemoved, writer);
    }

    public static MineTowersManager Deserialize(BlobReader reader)
    {
      MineTowersManager mineTowersManager;
      if (reader.TryStartClassDeserialization<MineTowersManager>(out mineTowersManager))
        reader.EnqueueDataDeserialization((object) mineTowersManager, MineTowersManager.s_deserializeDataDelayedAction);
      return mineTowersManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<MineTowersManager>(this, "m_entitiesManager", (object) EntitiesManager.Deserialize(reader));
      reader.SetField<MineTowersManager>(this, "m_mineTowers", (object) Lyst<MineTower>.Deserialize(reader));
      reader.SetField<MineTowersManager>(this, "m_onAreaChange", (object) Event<MineTower, RectangleTerrainArea2i>.Deserialize(reader));
      reader.SetField<MineTowersManager>(this, "m_onTowerAdded", (object) Event<MineTower, EntityAddReason>.Deserialize(reader));
      reader.SetField<MineTowersManager>(this, "m_onTowerRemoved", (object) Event<MineTower, EntityRemoveReason>.Deserialize(reader));
    }

    static MineTowersManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      MineTowersManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((MineTowersManager) obj).SerializeData(writer));
      MineTowersManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((MineTowersManager) obj).DeserializeData(reader));
    }
  }
}
