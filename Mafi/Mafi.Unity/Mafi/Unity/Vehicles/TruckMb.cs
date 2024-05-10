// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Vehicles.TruckMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Environment;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Unity.Audio;
using Mafi.Unity.Entities;
using Mafi.Unity.Entities.Dynamic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Vehicles
{
  internal class TruckMb : 
    PathFindingEntityMb,
    IEntityMbWithSimUpdateEnd,
    IEntityMb,
    IDestroyableEntityMb
  {
    private Truck m_truck;
    private TruckAttachmentFactory m_attachmentFactory;
    private Option<AttachmentMb> m_attachment;
    private ProductQuantity m_prevCargo;
    private Option<ProductProto> m_prevDefaultProduct;
    private WheelsAnimator m_wheelsAnimator;
    private VehicleLightsController m_lightsController;

    internal void Initialize(
      Truck truck,
      TruckAttachmentFactory attachmentFactory,
      AssetsDb assetsDb,
      IWeatherManager weatherManager,
      RandomProvider randomProvider,
      EntityAudioManager audioManager,
      DynamicGroundEntityDeps dependencies)
    {
      this.m_truck = truck.CheckNotNull<Truck>();
      this.m_attachmentFactory = attachmentFactory.CheckNotNull<TruckAttachmentFactory>();
      TruckProto.Gfx graphics = truck.Prototype.Graphics;
      this.m_wheelsAnimator = new WheelsAnimator(this.gameObject, graphics.SteeringWheelsSubmodelPaths, graphics.StaticWheelsSubmodelPaths, Vector3.up, Vector3.back, graphics.WheelDiameter.ToUnityUnits());
      this.m_lightsController = new VehicleLightsController(weatherManager, randomProvider.GetNonSimRandomFor((object) this, truck.Id.ToString()), (DynamicGroundEntity) truck, this.gameObject, new int?(Shader.PropertyToID("_EmissionStrength")));
      this.Initialize((PathFindingEntity) truck, assetsDb, audioManager, dependencies);
      if (!this.m_truck.Cargo.IsEmpty || !this.m_truck.DefaultProduct.IsNone || !this.m_truck.Prototype.AttachmentWhenEmpty.HasValue)
        return;
      this.setAttachment(this.m_truck.Prototype.AttachmentWhenEmpty.Value);
    }

    public override void SyncUpdate(GameTime time)
    {
      bool flag = this.m_truck.IsEmpty || (Proto) this.m_truck.Cargo.FirstOrPhantom.Product != (Proto) this.m_prevCargo.Product;
      if (flag || this.m_truck.IsEmpty != this.m_prevCargo.IsEmpty || this.m_truck.DefaultProduct != this.m_prevDefaultProduct)
      {
        this.m_prevCargo = this.m_truck.Cargo.FirstOrPhantom;
        this.m_prevDefaultProduct = this.m_truck.DefaultProduct;
        if (this.m_truck.Cargo.IsNotEmpty)
        {
          if (flag || this.m_attachment.IsNone)
            this.setAttachment(this.m_truck.Cargo.FirstOrPhantom.Product);
        }
        else if (this.m_truck.DefaultProduct.HasValue)
          this.setAttachment(this.m_truck.DefaultProduct.Value);
        else if (this.m_attachment.IsNone || !this.m_attachment.Value.Proto.KeepOnEvenIfNotNeeded)
        {
          if (this.m_truck.Prototype.AttachmentWhenEmpty.HasValue)
            this.setAttachment(this.m_truck.Prototype.AttachmentWhenEmpty.Value);
          else
            this.removeAttachment();
        }
      }
      this.m_wheelsAnimator.Sync(this.m_truck.SteeringAngle, this.m_truck.Speed);
      this.m_lightsController.UpdateVehicleLights();
      base.SyncUpdate(time);
      if (!this.m_attachment.HasValue)
        return;
      this.m_attachment.Value.SyncUpdate((DynamicGroundEntityMb) this);
    }

    public override void RenderUpdate(GameTime time)
    {
      if (this.m_attachment.HasValue)
        this.m_attachment.Value.RenderUpdate(time);
      this.m_wheelsAnimator.RenderUpdate(time);
      base.RenderUpdate(time);
    }

    private void removeAttachment()
    {
      if (!this.m_attachment.HasValue)
        return;
      this.m_attachmentFactory.ReturnToPool(this.m_attachment.Value);
      this.m_attachment = (Option<AttachmentMb>) Option.None;
    }

    private void setAttachment(AttachmentProto proto)
    {
      if (this.m_attachment.HasValue && (Proto) this.m_attachment.Value.Proto == (Proto) proto)
        return;
      this.removeAttachment();
      Option<AttachmentMb> pooled = this.m_attachmentFactory.CreatePooled(this.m_truck, proto);
      if (pooled.IsNone)
        return;
      this.setupAttachment(pooled.Value);
    }

    private void setAttachment(ProductProto productProto)
    {
      Option<AttachmentProto> attachmentType = this.m_attachmentFactory.GetAttachmentType(this.m_truck, productProto);
      if (attachmentType.IsNone)
      {
        this.removeAttachment();
      }
      else
      {
        if (this.m_attachment.HasValue && (Proto) attachmentType.Value == (Proto) this.m_attachment.Value.Proto)
          return;
        this.removeAttachment();
        Option<AttachmentMb> pooled = this.m_attachmentFactory.CreatePooled(this.m_truck, productProto);
        if (pooled.IsNone)
          return;
        this.setupAttachment(pooled.Value);
      }
    }

    private void setupAttachment(AttachmentMb attachment)
    {
      this.m_attachment = (Option<AttachmentMb>) attachment;
      GameObject gameObject = attachment.gameObject;
      gameObject.transform.parent = this.gameObject.transform;
      gameObject.transform.localRotation = Quaternion.identity;
      gameObject.transform.localPosition = Vector3.zero;
    }

    public TruckMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
