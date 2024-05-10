// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.ProtoModelFactory
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Ports.Io;
using Mafi.Core.Prototypes;
using Mafi.Unity.Entities.Static;
using Mafi.Unity.Ports.Io;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities
{
  /// <summary>
  /// Convenience class for creation of <see cref="T:UnityEngine.GameObject" /> for entities created just by <see cref="T:Mafi.Core.Entities.EntityProto" />. This class also handles model pooling.
  /// </summary>
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class ProtoModelFactory
  {
    private readonly DependencyResolver m_resolver;
    private readonly IoPortModelFactory m_portsFactory;
    private readonly Dict<IProto, GameObjectPool> m_goPools;

    public ProtoModelFactory(DependencyResolver resolver, IoPortModelFactory portsFactory)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_goPools = new Dict<IProto, GameObjectPool>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_resolver = resolver.CheckNotNull<DependencyResolver>();
      this.m_portsFactory = portsFactory.CheckNotNull<IoPortModelFactory>();
    }

    public GameObject CreateModelFor<TProto>(TProto proto) where TProto : IProto
    {
      GameObjectPool pool = this.getPool((IProto) proto, false);
      return pool == null || pool.IsEmpty ? this.m_resolver.InvokeFactoryHierarchy<GameObject>((object) proto) : pool.GetInstance();
    }

    /// <summary>
    /// Returns given model to the cache. It is responsibility of the caller to make sure that the model is in
    /// exactly the same condition as the one received from <see cref="M:Mafi.Unity.Entities.ProtoModelFactory.CreateModelFor``1(``0)" />.
    /// </summary>
    public void ReturnModelOf<TProto>(TProto proto, ref GameObject go) where TProto : IProto
    {
      if ((object) proto == null || (UnityEngine.Object) go == (UnityEngine.Object) null)
        Assert.Fail("Returning null to GO pool?");
      else
        this.getPool((IProto) proto, true).ReturnInstance(ref go);
    }

    private GameObjectPool getPool(IProto proto, bool createPool)
    {
      GameObjectPool pool1;
      if (this.m_goPools.TryGetValue(proto, out pool1))
        return pool1;
      if (!createPool)
        return (GameObjectPool) null;
      GameObjectPool pool2 = new GameObjectPool(proto.Id.Value, 4, (Func<GameObject>) (() =>
      {
        throw new InvalidOperationException();
      }), (Action<GameObject>) (x => { }));
      this.m_goPools[proto] = pool2;
      return pool2;
    }

    /// <summary>
    /// Creates model of <see cref="T:Mafi.Core.Entities.Static.Layout.LayoutEntity" /> that includes port models.
    /// </summary>
    public GameObject CreateModelWithPortsFor<TProto>(TProto proto) where TProto : LayoutEntityProto
    {
      GameObject modelWithPortsFor = this.m_resolver.InvokeFactoryHierarchy<GameObject>((object) proto);
      StaticEntityTransform transformData = StaticEntityMb.GetTransformData(TileTransform.Identity, proto.Graphics.PrefabOrigin, proto.Layout);
      modelWithPortsFor.transform.localPosition = transformData.Position;
      modelWithPortsFor.transform.localRotation = transformData.Rotation;
      modelWithPortsFor.transform.localScale = transformData.LocalScale;
      foreach (IoPortTemplate port in proto.Ports)
      {
        GameObject modelFor = this.m_portsFactory.CreateModelFor(port.Shape, false);
        IoPortMb.SetTransform(modelFor, port, proto.Layout, TileTransform.Identity);
        modelFor.transform.SetParent(modelWithPortsFor.transform, true);
      }
      modelWithPortsFor.transform.localPosition = Vector3.zero;
      modelWithPortsFor.transform.localRotation = Quaternion.identity;
      return modelWithPortsFor;
    }
  }
}
