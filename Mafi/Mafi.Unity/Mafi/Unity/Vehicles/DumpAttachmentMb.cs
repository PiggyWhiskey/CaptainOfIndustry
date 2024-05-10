// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Vehicles.DumpAttachmentMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Products;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Unity.Entities.Dynamic;
using Mafi.Unity.Terrain;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Vehicles
{
  internal class DumpAttachmentMb : AttachmentMb
  {
    private static readonly int DUMP_UP_ANIM_STATE_ID;
    private static readonly int DUMP_DOWN_ANIM_STATE_ID;
    private Truck m_truck;
    private Option<Animator> m_dumpBedAnimator;
    private LooseMixedProductsTextureSetter m_smoothPileMaterialSetter;
    private LooseMixedProductsTextureSetter m_roughPileMaterialSetter;
    private Option<Animator> m_pileAnimator;
    private Option<Animator> m_smoothPileAnimator;
    private Option<Animator> m_roughPileAnimator;
    private int m_pileAnimatorStateId;
    private Option<Transform> m_pileTransform;
    private Option<Transform> m_smoothPileTransform;
    private Option<Transform> m_roughPileTransform;
    private readonly Lyst<ProductQuantity> m_products;
    private bool m_isDumping;
    private bool m_prevIsDumping;
    private float m_syncCargoFullness;
    private float m_prevCargoFullness;
    private float m_renderedCargoFullness;
    private Vector3 m_offsetEmpty;
    private Vector3 m_offsetFull;
    private bool m_isNewProduct;

    public void Initialize(
      DumpAttachmentProto proto,
      LooseProductMaterialManager looseProductMaterialManager)
    {
      this.Initialize((AttachmentProto) proto);
      Assert.That<Truck>(this.m_truck).IsNull<Truck>();
      this.m_offsetEmpty = proto.Graphics.OffsetEmpty.ToVector3();
      this.m_offsetFull = proto.Graphics.OffsetFull.ToVector3();
      Animator component1;
      if (this.gameObject.TryGetComponent<Animator>(out component1))
        this.m_dumpBedAnimator = (Option<Animator>) component1;
      else
        Log.Warning(string.Format("No animator found on dump bed '{0}' ({1}).", (object) this.gameObject, (object) proto.Id));
      GameObject resultGo1;
      if (this.gameObject.TryFindChild(proto.Graphics.SmoothPileObjectPath, out resultGo1))
      {
        this.m_smoothPileTransform = (Option<Transform>) resultGo1.transform;
        resultGo1.SetActive(false);
        Renderer component2;
        if (resultGo1.TryGetComponent<Renderer>(out component2))
        {
          this.m_smoothPileMaterialSetter = looseProductMaterialManager.SetupMixedSharedMaterialsFor(component2, proto.Graphics.PileTextureParams);
          MeshFilter component3;
          if (resultGo1.TryGetComponent<MeshFilter>(out component3))
          {
            this.setUvs(component3.mesh);
          }
          else
          {
            SkinnedMeshRenderer component4;
            if (resultGo1.TryGetComponent<SkinnedMeshRenderer>(out component4))
              this.setUvs(component4.sharedMesh);
            else
              Log.Warning(string.Format("Failed to find mesh of pile '{0}'.", (object) resultGo1));
          }
        }
        else
          Log.Warning(string.Format("Failed to find renderer of pile '{0}'.", (object) resultGo1));
      }
      else
        Log.Warning("Failed to find material pile '" + proto.Graphics.SmoothPileObjectPath + "' " + string.Format("on object '{0}'.", (object) this.gameObject));
      if (proto.Graphics.SmoothPileObjectPath == proto.Graphics.RoughPileObjectPath)
      {
        this.m_roughPileTransform = this.m_smoothPileTransform;
        this.m_roughPileAnimator = this.m_smoothPileAnimator;
        this.m_roughPileMaterialSetter = this.m_smoothPileMaterialSetter;
      }
      else
      {
        GameObject resultGo2;
        if (this.gameObject.TryFindChild(proto.Graphics.RoughPileObjectPath, out resultGo2))
        {
          this.m_roughPileTransform = (Option<Transform>) resultGo2.transform;
          resultGo2.SetActive(false);
          Renderer component5;
          if (resultGo2.TryGetComponent<Renderer>(out component5))
            this.m_roughPileMaterialSetter = looseProductMaterialManager.SetupMixedSharedMaterialsFor(component5, proto.Graphics.PileTextureParams);
          else
            Log.Warning(string.Format("Failed to find renderer of pile '{0}'.", (object) resultGo2));
        }
      }
      if (!proto.Graphics.AnimationStateName.HasValue)
        return;
      this.m_pileAnimatorStateId = Animator.StringToHash(proto.Graphics.AnimationStateName.Value);
      Animator component6;
      if (this.m_smoothPileTransform.HasValue && this.m_smoothPileTransform.Value.gameObject.TryGetComponent<Animator>(out component6))
      {
        this.m_smoothPileAnimator = (Option<Animator>) component6;
        component6.speed = 0.0f;
      }
      else
        Log.Warning("Failed to find animator on animation-controlled " + string.Format("pile '{0}'.", (object) this.m_smoothPileTransform.Value.gameObject));
      if (proto.Graphics.SmoothPileObjectPath == proto.Graphics.RoughPileObjectPath)
      {
        this.m_roughPileAnimator = this.m_smoothPileAnimator;
      }
      else
      {
        Animator component7;
        if (this.m_roughPileTransform.HasValue && this.m_roughPileTransform.Value.gameObject.TryGetComponent<Animator>(out component7))
        {
          this.m_roughPileAnimator = (Option<Animator>) component7;
          component7.speed = 0.0f;
        }
        else
          Log.Warning("Failed to find animator on animation-controlled " + string.Format("pile '{0}'.", (object) this.m_roughPileTransform.Value.gameObject));
      }
    }

    private void setUvs(Mesh mesh)
    {
      if (mesh.isReadable)
      {
        float num = float.MaxValue;
        float self = float.MinValue;
        foreach (Vector2 vector2 in mesh.uv)
        {
          num = num.Min(vector2.x);
          self = self.Max(vector2.x);
        }
        this.m_smoothPileMaterialSetter.SetUVs(num, (float) (1.0 / ((double) self - (double) num)));
      }
      else
        Log.Warning(string.Format("Mesh of {0} is not readable.", (object) mesh));
    }

    public override void SetTruck(Truck truck)
    {
      this.m_truck = truck.CheckNotNull<Truck>();
      if (!this.m_dumpBedAnimator.HasValue)
        return;
      this.m_dumpBedAnimator.Value.Play(DumpAttachmentMb.DUMP_DOWN_ANIM_STATE_ID, 0, 1f);
    }

    public override void SyncUpdate(DynamicGroundEntityMb parent)
    {
      Assert.That<Truck>(this.m_truck).IsNotNull<Truck>();
      if (this.m_truck.Cargo.Count != this.m_products.Count)
      {
        this.m_isNewProduct = true;
      }
      else
      {
        int index = 0;
        foreach (KeyValuePair<ProductProto, Quantity> keyValuePair in this.m_truck.Cargo)
        {
          if ((Mafi.Core.Prototypes.Proto) this.m_products[index].Product != (Mafi.Core.Prototypes.Proto) keyValuePair.Key)
          {
            this.m_isNewProduct = true;
            break;
          }
          ++index;
        }
      }
      this.m_products.Clear();
      foreach (KeyValuePair<ProductProto, Quantity> keyValuePair in this.m_truck.Cargo)
        this.m_products.Add(new ProductQuantity(keyValuePair.Key, keyValuePair.Value));
      this.m_prevCargoFullness = this.m_syncCargoFullness;
      this.m_syncCargoFullness = ((float) this.m_truck.Cargo.TotalQuantity.Value / (float) this.m_truck.Capacity.Value).Clamp01();
      this.m_isDumping = this.m_truck.IsDumping;
    }

    public override void RenderUpdate(GameTime time)
    {
      Assert.That<Truck>(this.m_truck).IsNotNull<Truck>();
      if (this.m_isNewProduct)
      {
        this.m_isNewProduct = false;
        if (this.m_products.IsEmpty)
        {
          Assert.That<float>(this.m_syncCargoFullness).IsZero();
        }
        else
        {
          if (this.m_pileTransform.HasValue)
            this.m_pileTransform.Value.gameObject.SetActive(false);
          Assert.That<int>(this.m_products.Count).IsLessOrEqual(LooseMixedProductsTextureSetter.MAX_TEXTURE_COUNT);
          for (int index = 0; index < this.m_products.Count; ++index)
          {
            ProductQuantity product1 = this.m_products[index];
            if (product1.Product is LooseProductProto product2)
              this.m_smoothPileMaterialSetter.SetTexture(product2.LooseSlimId, index);
            else
              Log.Warning(string.Format("Loose attachment of {0}#{1} is transporting non-loose product: {2}", (object) this.m_truck.Prototype.Id, (object) this.m_truck.Id, (object) product1.Product));
          }
          this.m_pileTransform = this.m_smoothPileTransform;
          this.m_pileAnimator = this.m_smoothPileAnimator;
          if (this.m_pileAnimator.HasValue)
            this.m_pileTransform.Value.gameObject.SetActive(false);
          else if (this.m_pileTransform.HasValue)
            this.m_pileTransform.Value.gameObject.SetActive(false);
        }
      }
      if ((double) this.m_renderedCargoFullness != (double) this.m_syncCargoFullness)
      {
        this.m_renderedCargoFullness = (double) this.m_prevCargoFullness == (double) this.m_syncCargoFullness ? this.m_syncCargoFullness : this.m_prevCargoFullness.Lerp(this.m_syncCargoFullness, time.AbsoluteT);
        if ((double) this.m_syncCargoFullness != 0.0)
        {
          Quantity zero = Quantity.Zero;
          for (int index = 0; index < this.m_products.Count.Min(3); ++index)
            zero += this.m_products[index].Quantity;
          if (zero.IsPositive)
          {
            float num = 1f / (float) zero.Value;
            this.m_smoothPileMaterialSetter.SetRatios(this.m_products.Count >= 1 ? (float) this.m_products[0].Quantity.Value * num : 0.0f, this.m_products.Count >= 2 ? (float) this.m_products[1].Quantity.Value * num : 0.0f, this.m_products.Count >= 3 ? (float) this.m_products[2].Quantity.Value * num : 0.0f);
          }
        }
        if (this.m_pileAnimator.HasValue)
        {
          if ((double) this.m_renderedCargoFullness > 0.0)
          {
            this.m_pileTransform.Value.gameObject.SetActive(true);
            this.m_pileAnimator.Value.Play(this.m_pileAnimatorStateId, 0, this.m_renderedCargoFullness);
          }
          else if (this.m_pileTransform.Value.gameObject.activeSelf)
          {
            this.m_pileAnimator.Value.Play(this.m_pileAnimatorStateId, 0, 0.0f);
            this.m_pileTransform.Value.gameObject.SetActive(false);
          }
        }
        else if (this.m_pileTransform.HasValue)
        {
          this.m_pileTransform.Value.localPosition = Vector3.Lerp(this.m_offsetEmpty, this.m_offsetFull, this.m_renderedCargoFullness);
          this.m_pileTransform.Value.gameObject.SetActive((double) this.m_renderedCargoFullness > 0.0);
          double prevCargoFullness = (double) this.m_prevCargoFullness;
          double syncCargoFullness = (double) this.m_syncCargoFullness;
        }
      }
      if (!this.m_dumpBedAnimator.HasValue)
        return;
      this.m_dumpBedAnimator.Value.enabled = !time.IsGamePaused;
      this.m_dumpBedAnimator.Value.speed = this.m_truck.SpeedFactor.ToFloat() * (float) time.GameSpeedMult;
      if (this.m_isDumping != this.m_prevIsDumping)
      {
        this.m_dumpBedAnimator.Value.Play(this.m_isDumping ? DumpAttachmentMb.DUMP_UP_ANIM_STATE_ID : DumpAttachmentMb.DUMP_DOWN_ANIM_STATE_ID);
        this.m_prevIsDumping = this.m_isDumping;
      }
      else
      {
        if (!this.m_pileAnimator.HasValue || (double) this.m_renderedCargoFullness <= 0.0 || !this.gameObject.activeInHierarchy)
          return;
        this.m_pileAnimator.Value.Play(this.m_pileAnimatorStateId, 0, this.m_renderedCargoFullness);
      }
    }

    public override void Reset()
    {
      base.Reset();
      this.m_truck = (Truck) null;
    }

    public DumpAttachmentMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_products = new Lyst<ProductQuantity>(3);
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static DumpAttachmentMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      DumpAttachmentMb.DUMP_UP_ANIM_STATE_ID = Animator.StringToHash("Up");
      DumpAttachmentMb.DUMP_DOWN_ANIM_STATE_ID = Animator.StringToHash("Down");
    }
  }
}
