// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Vehicles.TruckAttachmentFactory
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Unity.InstancedRendering;
using Mafi.Unity.Terrain;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Vehicles
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  internal class TruckAttachmentFactory
  {
    private readonly Dict<KeyValuePair<TruckProto, ProductProto>, AttachmentProto> m_truckProductToAttachment;
    private readonly Dict<AttachmentProto, MbPool<AttachmentMb>> m_attachmentToPool;

    public TruckAttachmentFactory(
      AssetsDb assetsDb,
      ProtosDb protosDb,
      ProductsRenderer transportedProductsRenderer,
      LooseProductMaterialManager looseProductMaterialManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_truckProductToAttachment = new Dict<KeyValuePair<TruckProto, ProductProto>, AttachmentProto>();
      this.m_attachmentToPool = new Dict<AttachmentProto, MbPool<AttachmentMb>>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Assert.That<AssetsDb>(assetsDb).IsNotNull<AssetsDb>();
      Assert.That<ProtosDb>(protosDb).IsNotNull<ProtosDb>();
      ProductProto[] array = protosDb.All<ProductProto>().ToArray<ProductProto>();
      Set<AttachmentProto> set = new Set<AttachmentProto>();
      foreach (TruckProto key in protosDb.All<TruckProto>())
      {
        foreach (AttachmentProto attachment in key.Attachments)
        {
          AttachmentProto attachmentProto = attachment;
          set.Add(attachmentProto);
          foreach (ProductProto productProto in ((IEnumerable<ProductProto>) array).Where<ProductProto>((Func<ProductProto, bool>) (p => attachmentProto.EligibleProductsFilter(p))))
            this.m_truckProductToAttachment.Add(new KeyValuePair<TruckProto, ProductProto>(key, productProto), attachmentProto);
        }
      }
      foreach (AttachmentProto attachmentProto1 in set)
      {
        AttachmentProto attachmentProto = attachmentProto1;
        GameObject assetGo;
        if (!assetsDb.TryGetSharedAsset<GameObject>(attachmentProto.Graphics.PrefabPath, out assetGo))
        {
          Assert.Fail(string.Format("Could not load model for attachment '{0}'", (object) attachmentProto.Id));
        }
        else
        {
          DumpAttachmentProto dumpAttachmentProto = attachmentProto as DumpAttachmentProto;
          FlatBedAttachmentProto flatBedAttachmentProto = attachmentProto as FlatBedAttachmentProto;
          TankAttachmentProto tankAttachmentProto = attachmentProto as TankAttachmentProto;
          Func<MbPool<AttachmentMb>, AttachmentMb> factory = !((Proto) dumpAttachmentProto != (Proto) null) ? (!((Proto) flatBedAttachmentProto != (Proto) null) ? (!((Proto) tankAttachmentProto != (Proto) null) ? (Func<MbPool<AttachmentMb>, AttachmentMb>) (pool =>
          {
            AttachmentMb attachmentMb = UnityEngine.Object.Instantiate<GameObject>(assetGo).AddComponent<AttachmentMb>();
            attachmentMb.Initialize(attachmentProto);
            ((IAttachmentMbFriend) attachmentMb).SetPool(pool);
            return attachmentMb;
          }) : (Func<MbPool<AttachmentMb>, AttachmentMb>) (pool =>
          {
            TankAttachmentMb tankAttachmentMb = UnityEngine.Object.Instantiate<GameObject>(assetGo).AddComponent<TankAttachmentMb>();
            tankAttachmentMb.Initialize(tankAttachmentProto, assetsDb);
            ((IAttachmentMbFriend) tankAttachmentMb).SetPool(pool);
            return (AttachmentMb) tankAttachmentMb;
          })) : (Func<MbPool<AttachmentMb>, AttachmentMb>) (pool =>
          {
            FlatBedAttachmentMb flatBedAttachmentMb = UnityEngine.Object.Instantiate<GameObject>(assetGo).AddComponent<FlatBedAttachmentMb>();
            flatBedAttachmentMb.Initialize((AttachmentProto) flatBedAttachmentProto, transportedProductsRenderer);
            ((IAttachmentMbFriend) flatBedAttachmentMb).SetPool(pool);
            return (AttachmentMb) flatBedAttachmentMb;
          })) : (Func<MbPool<AttachmentMb>, AttachmentMb>) (pool =>
          {
            DumpAttachmentMb dumpAttachmentMb = UnityEngine.Object.Instantiate<GameObject>(assetGo).AddComponent<DumpAttachmentMb>();
            dumpAttachmentMb.Initialize(dumpAttachmentProto, looseProductMaterialManager);
            ((IAttachmentMbFriend) dumpAttachmentMb).SetPool(pool);
            return (AttachmentMb) dumpAttachmentMb;
          });
          MbPool<AttachmentMb> mbPool = new MbPool<AttachmentMb>(string.Format("{0}_{1}", (object) nameof (TruckAttachmentFactory), (object) attachmentProto.Strings.Name), 10, factory, (Action<AttachmentMb>) (x => x.Reset()));
          this.m_attachmentToPool.Add(attachmentProto, mbPool);
        }
      }
    }

    public Option<AttachmentMb> CreatePooled(Truck truck, ProductProto productProto)
    {
      Assert.That<Truck>(truck).IsNotNull<Truck>();
      Assert.That<ProductProto>(productProto).IsNotNull<ProductProto>();
      AttachmentProto attachmentProto;
      if (!this.m_truckProductToAttachment.TryGetValue(Make.Kvp<TruckProto, ProductProto>(truck.Prototype, productProto), out attachmentProto))
        return (Option<AttachmentMb>) Option.None;
      Option<AttachmentMb> pooled = this.CreatePooled(truck, attachmentProto);
      if (pooled.IsNone)
        return (Option<AttachmentMb>) Option.None;
      ColorRgba c = attachmentProto.Graphics.ColorsMap(productProto);
      if (c.IsNotEmpty)
        pooled.Value.gameObject.GetComponent<Renderer>().material.color = c.ToColor();
      return pooled;
    }

    public Option<AttachmentProto> GetAttachmentType(Truck truck, ProductProto productProto)
    {
      return this.m_truckProductToAttachment.Get<KeyValuePair<TruckProto, ProductProto>, AttachmentProto>(Make.Kvp<TruckProto, ProductProto>(truck.Prototype, productProto));
    }

    public Option<AttachmentMb> CreatePooled(Truck truck, AttachmentProto attachmentProto)
    {
      Assert.That<Truck>(truck).IsNotNull<Truck>();
      Assert.That<AttachmentProto>(attachmentProto).IsNotNull<AttachmentProto>();
      MbPool<AttachmentMb> mbPool;
      if (!this.m_attachmentToPool.TryGetValue(attachmentProto, out mbPool))
        return (Option<AttachmentMb>) Option.None;
      AttachmentMb instance = mbPool.GetInstance();
      instance.SetTruck(truck);
      return (Option<AttachmentMb>) instance;
    }

    public bool TryCreatePooled(AttachmentProto attachmentProto, out AttachmentMb attachmentMb)
    {
      MbPool<AttachmentMb> mbPool;
      if (this.m_attachmentToPool.TryGetValue(attachmentProto, out mbPool))
      {
        attachmentMb = mbPool.GetInstance();
        return true;
      }
      attachmentMb = (AttachmentMb) null;
      return false;
    }

    public void ReturnToPool(AttachmentMb attachment)
    {
      ((IAttachmentMbFriend) attachment).ReturnToPool();
    }
  }
}
