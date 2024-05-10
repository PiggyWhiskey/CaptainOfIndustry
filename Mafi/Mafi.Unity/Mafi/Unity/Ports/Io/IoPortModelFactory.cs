// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Ports.Io.IoPortModelFactory
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core.Ports.Io;
using Mafi.Core.Prototypes;
using System;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Ports.Io
{
  /// <summary>IO port model factory.</summary>
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class IoPortModelFactory
  {
    private static readonly Fix32 PORT_THICKNESS_TILES;
    private readonly AssetsDb m_assetsDb;
    private readonly Material m_vertexColorMaterial;
    private readonly Dict<IoPortModelFactory.PoolKey, GameObjectPool> m_pools;

    public IoPortModelFactory(AssetsDb assetsDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_pools = new Dict<IoPortModelFactory.PoolKey, GameObjectPool>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_assetsDb = assetsDb;
      this.m_vertexColorMaterial = assetsDb.GetSharedMaterial("Assets/Core/Materials/VertexColor.mat");
    }

    /// <summary>
    /// Returns a model for requested port. This method uses GO pool. Please use method <see cref="M:Mafi.Unity.Ports.Io.IoPortModelFactory.ReturnModelOf(Mafi.Core.Ports.Io.IoPort,UnityEngine.GameObject@)" /> to return GOs back to pool.
    /// </summary>
    public GameObject CreateModelFor(IoPort port)
    {
      return this.CreateModelFor(port.ShapePrototype, port.IsEndPort);
    }

    /// <summary>
    /// Returns a model for requested shape. This method uses GO pool. Please use method <see cref="M:Mafi.Unity.Ports.Io.IoPortModelFactory.ReturnModelOf(Mafi.Core.Ports.Io.IoPortShapeProto,UnityEngine.GameObject@)" /> to return GOs back to pool.
    /// </summary>
    /// <param name="shapeProto">Proto</param>
    /// <param name="isEndModel">Whether to return special end-model.</param>
    /// <param name="customSharedMaterial">Optional custom material to use.</param>
    public GameObject CreateModelFor(
      IoPortShapeProto shapeProto,
      bool isEndModel,
      Material customSharedMaterial = null)
    {
      bool hasCustomMaterial = (UnityEngine.Object) customSharedMaterial != (UnityEngine.Object) null;
      GameObjectPool gameObjectPool;
      if (this.m_pools.TryGetValue(new IoPortModelFactory.PoolKey(shapeProto, isEndModel, hasCustomMaterial), out gameObjectPool) && !gameObjectPool.IsEmpty)
        return gameObjectPool.GetInstance();
      GameObject prefab;
      if (!this.m_assetsDb.TryGetClonedPrefab(isEndModel ? shapeProto.Graphics.DisconnectedPortPrefabPath : shapeProto.Graphics.ConnectedPortPrefabPath, out prefab))
        return this.GeneratePlaceholderMesh();
      prefab.name = string.Format("{0}{1}-{2}", (object) "port-", (object) (char) (isEndModel ? 69 : 78), (object) (char) (hasCustomMaterial ? 67 : 68));
      if (hasCustomMaterial)
        prefab.GetComponent<MeshRenderer>().sharedMaterial = customSharedMaterial;
      prefab.SetActive(true);
      Assert.That<bool>(!(bool) (UnityEngine.Object) prefab.GetComponent<Collider>()).IsTrue(string.Format("Port '{0}' has collider but ports should not have colliders.", (object) shapeProto.Id));
      return prefab;
    }

    /// <summary>Creates placeholder mesh for a port.</summary>
    public GameObject GeneratePlaceholderMesh()
    {
      GameObject entityGo = new GameObject();
      MeshBuilder instance = MeshBuilder.Instance;
      instance.AddAaBox(new Tile3f(Fix32.Half, (Fix32) 0, Fix32.Half).ToVector3(), (new Tile3f(IoPortModelFactory.PORT_THICKNESS_TILES, 0.8.ToFix32(), 0.8.ToFix32()) / (Fix32) 2).ToVector3(), ColorRgba.Yellow.ToColor32());
      instance.UpdateGoAndClear(entityGo);
      entityGo.GetComponent<MeshRenderer>().sharedMaterial = this.m_vertexColorMaterial;
      return entityGo;
    }

    /// <summary>
    /// Returns model of this port. The model should not have arrow attached and should not be highlighted.
    /// </summary>
    public void ReturnModelOf(IoPort port, ref GameObject go)
    {
      this.ReturnModelOf(port.ShapePrototype, ref go);
    }

    /// <summary>
    /// Returns model of the port. The model should not have arrow attached and should not be highlighted.
    /// </summary>
    public void ReturnModelOf(IoPortShapeProto shapeProto, ref GameObject go)
    {
      if (go.name.Length != "port-".Length + 3 || !go.name.StartsWith("port-"))
      {
        Log.Error("Invalid port name " + go.name);
        go.Destroy();
      }
      else
      {
        char ch1 = go.name["port-".Length];
        Assert.That<bool>(ch1 == 'N' || ch1 == 'E').IsTrue("Invalid port name " + go.name);
        char ch2 = go.name["port-".Length + 2];
        Assert.That<bool>(ch2 == 'D' || ch2 == 'C').IsTrue("Invalid port name " + go.name);
        bool isEndModel = ch1 == 'E';
        bool hasCustomMaterial = ch2 == 'C';
        IoPortModelFactory.PoolKey key = new IoPortModelFactory.PoolKey(shapeProto, isEndModel, hasCustomMaterial);
        GameObjectPool gameObjectPool;
        if (!this.m_pools.TryGetValue(key, out gameObjectPool))
        {
          gameObjectPool = new GameObjectPool(string.Format("{0}", (object) shapeProto.Id), 8, (Func<GameObject>) (() =>
          {
            throw new InvalidOperationException();
          }), (Action<GameObject>) (x =>
          {
            if (x.transform.childCount <= 0)
              return;
            Assert.Fail("Port GO has some children. Left some arrows in port GO? " + Enumerable.Range(0, x.transform.childCount).Select<int, string>((Func<int, string>) (i => x.transform.GetChild(i).name)).JoinStrings(", "));
          }));
          this.m_pools[key] = gameObjectPool;
        }
        gameObjectPool.ReturnInstance(ref go);
      }
    }

    static IoPortModelFactory()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      IoPortModelFactory.PORT_THICKNESS_TILES = 0.2.ToFix32();
    }

    private struct PoolKey : IEquatable<IoPortModelFactory.PoolKey>
    {
      public readonly IoPortShapeProto Proto;
      public readonly bool IsEndModel;
      public readonly bool HasCustomMaterial;

      public PoolKey(IoPortShapeProto proto, bool isEndModel, bool hasCustomMaterial)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.Proto = proto;
        this.IsEndModel = isEndModel;
        this.HasCustomMaterial = hasCustomMaterial;
      }

      public bool Equals(IoPortModelFactory.PoolKey other)
      {
        return (Proto) this.Proto == (Proto) other.Proto && this.IsEndModel == other.IsEndModel && this.HasCustomMaterial == other.HasCustomMaterial;
      }

      public override bool Equals(object obj)
      {
        return obj is IoPortModelFactory.PoolKey other && this.Equals(other);
      }

      public override int GetHashCode()
      {
        return Hash.Combine<IoPortShapeProto, bool, bool>(this.Proto, this.IsEndModel, this.HasCustomMaterial);
      }
    }
  }
}
