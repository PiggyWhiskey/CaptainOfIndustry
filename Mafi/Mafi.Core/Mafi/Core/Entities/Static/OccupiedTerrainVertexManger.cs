// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.OccupiedTerrainVertexManger
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Entities.Static
{
  /// <summary>
  /// Occupied vertices of static entities are tricky to track since there could many entities touching
  /// a single vertex. This manager simplifies the work with them.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class OccupiedTerrainVertexManger
  {
    private readonly IEntitiesManager m_entitiesManager;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Dict<Tile2i, OccupiedTerrainVertexManger.TileVertexData> m_tileVertexData;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Lyst<OccupiedTerrainVertexManger.TileVertexData> m_tileVertexDataOverflow;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Stak<int> m_overflowFreeIndices;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    internal int OccupiedVerticesCount => this.m_tileVertexData.Count;

    public OccupiedTerrainVertexManger(IEntitiesManager entitiesManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_tileVertexData = new Dict<Tile2i, OccupiedTerrainVertexManger.TileVertexData>();
      this.m_tileVertexDataOverflow = new Lyst<OccupiedTerrainVertexManger.TileVertexData>();
      this.m_overflowFreeIndices = new Stak<int>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entitiesManager = entitiesManager;
      this.initAfterLoad();
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void initAfterLoad()
    {
      this.m_entitiesManager.StaticEntityAdded.AddNonSaveable<OccupiedTerrainVertexManger>(this, new Action<IStaticEntity>(this.staticEntityAdded));
      this.m_entitiesManager.StaticEntityRemoved.AddNonSaveable<OccupiedTerrainVertexManger>(this, new Action<IStaticEntity>(this.staticEntityRemoved));
      this.m_entitiesManager.OnUpgradeToBePerformed.AddNonSaveable<OccupiedTerrainVertexManger>(this, new Action<IUpgradableEntity>(this.staticEntityRemoved));
      this.m_entitiesManager.OnUpgradeJustPerformed.AddNonSaveable<OccupiedTerrainVertexManger>(this, new Action<IUpgradableEntity, IEntityProto>(this.entityUpgraded));
      foreach (IEntity entity1 in this.m_entitiesManager.Entities)
      {
        if (entity1 is IStaticEntity entity2)
          this.staticEntityAdded(entity2);
      }
    }

    public void GetEntitiesOnTerrainVertex(
      Tile2i coord,
      Lyst<OccupiedTerrainVertexManger.TileVertexData> entities)
    {
      OccupiedTerrainVertexManger.TileVertexData tileVertexData1;
      if (!this.m_tileVertexData.TryGetValue(coord, out tileVertexData1))
        return;
      entities.Add(tileVertexData1);
      int num = 0;
      int dataOverflowIndex = tileVertexData1.NextDataOverflowIndex;
      while (dataOverflowIndex >= 0)
      {
        OccupiedTerrainVertexManger.TileVertexData tileVertexData2 = this.m_tileVertexDataOverflow[dataOverflowIndex];
        entities.Add(tileVertexData2);
        dataOverflowIndex = tileVertexData2.NextDataOverflowIndex;
        if (++num > 20)
        {
          Log.Error("More than 20 entities on a vertex? Possible infinite recursion.");
          break;
        }
      }
    }

    private void entityUpgraded(IStaticEntity entity, IEntityProto proto)
    {
      this.staticEntityAdded(entity);
    }

    private void staticEntityAdded(IStaticEntity entity)
    {
      Tile2i xy = entity.CenterTile.Xy;
      for (int index = 0; index < entity.OccupiedVertices.Length; ++index)
      {
        OccupiedVertexRelative occupiedVertex = entity.OccupiedVertices[index];
        bool exists;
        ref OccupiedTerrainVertexManger.TileVertexData local = ref this.m_tileVertexData.GetRefValue(xy + occupiedVertex.RelCoord, out exists);
        if (exists)
        {
          Assert.That<IStaticEntity>(local.Entity).IsNotEqualTo<IStaticEntity, IStaticEntity>(entity, "Entity '{0}' has duplicate occupied vertex.", entity);
          int overflow = this.addToOverflow(local);
          local = new OccupiedTerrainVertexManger.TileVertexData(entity, index, overflow);
        }
        else
          local = new OccupiedTerrainVertexManger.TileVertexData(entity, index, -1);
      }
    }

    private void staticEntityRemoved(IStaticEntity entity)
    {
      Tile2i xy = entity.CenterTile.Xy;
      for (int index = 0; index < entity.OccupiedVertices.Length; ++index)
      {
        Tile2i key = xy + entity.OccupiedVertices[index].RelCoord;
        bool exists;
        ref OccupiedTerrainVertexManger.TileVertexData local = ref this.m_tileVertexData.GetRefValue(key, out exists);
        if (!exists)
        {
          Log.Error(string.Format("Failed to remove occupied terrain vertex {0} of '{1}'.", (object) key, (object) entity));
          this.m_tileVertexData.Remove(key);
        }
        else
        {
          int dataOverflowIndex = local.NextDataOverflowIndex;
          if (entity.Equals((object) local.Entity))
          {
            Assert.That<int>(local.VertexIndex).IsEqualTo(index);
            if (dataOverflowIndex < 0)
            {
              this.m_tileVertexData.Remove(key);
            }
            else
            {
              local = this.m_tileVertexDataOverflow[dataOverflowIndex];
              this.m_tileVertexDataOverflow[dataOverflowIndex] = new OccupiedTerrainVertexManger.TileVertexData();
              this.m_overflowFreeIndices.Push(dataOverflowIndex);
            }
          }
          else
            this.removeFromChain(entity, ref local);
        }
      }
    }

    private int addToOverflow(OccupiedTerrainVertexManger.TileVertexData data)
    {
      int index;
      if (this.m_overflowFreeIndices.IsEmpty)
      {
        index = this.m_tileVertexDataOverflow.Count;
        this.m_tileVertexDataOverflow.Add(data);
      }
      else
      {
        index = this.m_overflowFreeIndices.Pop();
        this.m_tileVertexDataOverflow[index] = data;
      }
      return index;
    }

    private void removeFromChain(
      IStaticEntity entity,
      ref OccupiedTerrainVertexManger.TileVertexData valueRef)
    {
      int index = -1;
      int dataOverflowIndex = valueRef.NextDataOverflowIndex;
      int num = 0;
      while (dataOverflowIndex >= 0)
      {
        OccupiedTerrainVertexManger.TileVertexData tileVertexData = this.m_tileVertexDataOverflow[dataOverflowIndex];
        if (entity.Equals((object) tileVertexData.Entity))
        {
          this.m_tileVertexDataOverflow[dataOverflowIndex] = new OccupiedTerrainVertexManger.TileVertexData();
          this.m_overflowFreeIndices.Push(dataOverflowIndex);
          if (index >= 0)
          {
            this.m_tileVertexDataOverflow[index] = this.m_tileVertexDataOverflow[index].WithNewNextIndex(tileVertexData.NextDataOverflowIndex);
            break;
          }
          valueRef = valueRef.WithNewNextIndex(tileVertexData.NextDataOverflowIndex);
          break;
        }
        index = dataOverflowIndex;
        dataOverflowIndex = tileVertexData.NextDataOverflowIndex;
        if (++num > 20)
          Log.Error("More than 20 vertices in overflow? Infinite recursion?");
      }
    }

    public static void Serialize(OccupiedTerrainVertexManger value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<OccupiedTerrainVertexManger>(value))
        return;
      writer.EnqueueDataSerialization((object) value, OccupiedTerrainVertexManger.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteGeneric<IEntitiesManager>(this.m_entitiesManager);
    }

    public static OccupiedTerrainVertexManger Deserialize(BlobReader reader)
    {
      OccupiedTerrainVertexManger terrainVertexManger;
      if (reader.TryStartClassDeserialization<OccupiedTerrainVertexManger>(out terrainVertexManger))
        reader.EnqueueDataDeserialization((object) terrainVertexManger, OccupiedTerrainVertexManger.s_deserializeDataDelayedAction);
      return terrainVertexManger;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<OccupiedTerrainVertexManger>(this, "m_entitiesManager", (object) reader.ReadGenericAs<IEntitiesManager>());
      reader.SetField<OccupiedTerrainVertexManger>(this, "m_overflowFreeIndices", (object) new Stak<int>());
      reader.SetField<OccupiedTerrainVertexManger>(this, "m_tileVertexData", (object) new Dict<Tile2i, OccupiedTerrainVertexManger.TileVertexData>());
      reader.SetField<OccupiedTerrainVertexManger>(this, "m_tileVertexDataOverflow", (object) new Lyst<OccupiedTerrainVertexManger.TileVertexData>());
      reader.RegisterInitAfterLoad<OccupiedTerrainVertexManger>(this, "initAfterLoad", InitPriority.Normal);
    }

    static OccupiedTerrainVertexManger()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      OccupiedTerrainVertexManger.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((OccupiedTerrainVertexManger) obj).SerializeData(writer));
      OccupiedTerrainVertexManger.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((OccupiedTerrainVertexManger) obj).DeserializeData(reader));
    }

    public readonly struct TileVertexData
    {
      public readonly IStaticEntity Entity;
      public readonly int VertexIndex;
      internal readonly int NextDataOverflowIndex;

      public TileVertexData(IStaticEntity entity, int vertexIndex, int nextDataOverflowIndex)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Entity = entity;
        this.VertexIndex = vertexIndex;
        this.NextDataOverflowIndex = nextDataOverflowIndex;
      }

      public OccupiedTerrainVertexManger.TileVertexData WithNewNextIndex(
        int newNextDataOverflowIndex)
      {
        return new OccupiedTerrainVertexManger.TileVertexData(this.Entity, this.VertexIndex, newNextDataOverflowIndex);
      }
    }
  }
}
