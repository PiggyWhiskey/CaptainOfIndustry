// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Factory.Transports.TransportMbFactory
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Products;
using Mafi.Unity.Camera;
using Mafi.Unity.Entities;
using Mafi.Unity.InstancedRendering;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Factory.Transports
{
  /// <summary>Handles creation of MBs for transports.</summary>
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class TransportMbFactory : IEntityMbFactory<Mafi.Core.Factory.Transports.Transport>, IFactory<Mafi.Core.Factory.Transports.Transport, EntityMb>
  {
    private readonly TransportModelFactory m_modelFactory;
    private readonly InstancedChunkBasedTransportsRenderer m_transportsRenderer;
    private readonly ProductsRenderer m_productsRenderer;
    private readonly ProductsSlimIdManager m_productsSlimIdManager;
    private readonly CameraController m_cameraController;
    private readonly AssetsDb m_assetsDb;

    public TransportMbFactory(
      TransportModelFactory modelFactory,
      ProductsRenderer productsRenderer,
      ProductsSlimIdManager productsSlimIdManager,
      AssetsDb assetsDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_modelFactory = modelFactory;
      this.m_productsRenderer = productsRenderer;
      this.m_productsSlimIdManager = productsSlimIdManager;
      this.m_assetsDb = assetsDb;
    }

    public EntityMb Create(Mafi.Core.Factory.Transports.Transport entity)
    {
      GameObject transportGo;
      if (entity.Prototype.Graphics.UseInstancedRendering)
      {
        transportGo = new GameObject(entity.Prototype.Id.Value);
        transportGo.transform.localPosition = entity.CenterTile.ToGroundCenterVector3();
        TransportModelFactory.AddColliders(transportGo, entity.Trajectory.Pivots, -entity.CenterTile.ToGroundCenterVector3());
      }
      else
        transportGo = this.m_modelFactory.CreateModel(entity.Trajectory, customOrigin: new Vector3?(entity.CenterTile.ToGroundCenterVector3()));
      TransportMb transportMb = transportGo.AddComponent<TransportMb>();
      transportMb.Initialize(entity, this.m_productsRenderer, this.m_productsSlimIdManager, this.m_assetsDb);
      return (EntityMb) transportMb;
    }
  }
}
