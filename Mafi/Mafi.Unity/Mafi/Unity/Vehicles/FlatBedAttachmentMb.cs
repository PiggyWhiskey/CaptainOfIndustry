// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Vehicles.FlatBedAttachmentMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Products;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Unity.Entities.Dynamic;
using Mafi.Unity.InstancedRendering;
using Mafi.Utils;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Vehicles
{
  internal class FlatBedAttachmentMb : AttachmentMb
  {
    private Truck m_truck;
    private ProductsRenderer m_productsRenderer;
    private ProductsRenderer.ProductInstanceQDataPair[] m_productsToRenderDynamic;
    private ProductSlimId m_productId;

    public void Initialize(AttachmentProto proto, ProductsRenderer productsRenderer)
    {
      this.m_productsRenderer = productsRenderer;
      this.Initialize(proto);
      Assert.That<Truck>(this.m_truck).IsNull<Truck>();
      Assert.That<ProductsRenderer>(this.m_productsRenderer).IsNotNull<ProductsRenderer>();
    }

    public override void SyncUpdate(DynamicGroundEntityMb parent)
    {
      Quantity totalQuantity = this.m_truck.Cargo.TotalQuantity;
      if (totalQuantity.IsZero)
        return;
      int num1 = totalQuantity.Value;
      this.m_productId = this.m_truck.Cargo.FirstOrPhantom.Product.SlimId;
      if (this.m_productsToRenderDynamic == null)
        this.m_productsToRenderDynamic = new ProductsRenderer.ProductInstanceQDataPair[60];
      ImmutableArray<ImmutableArray<ushort>> stackOffsetsPacked = this.m_productsRenderer.StackOffsetsPacked;
      UnityEngine.Vector3 currentPosition = parent.CurrentPosition;
      UnityEngine.Vector3 futurePosition = parent.FuturePosition;
      FlatBedAttachmentProto.Gfx graphics = this.Proto.Graphics as FlatBedAttachmentProto.Gfx;
      Assert.That<FlatBedAttachmentProto.Gfx>(graphics).IsNotNull<FlatBedAttachmentProto.Gfx>();
      float num2 = (float) num1 / (float) this.m_truck.Capacity.Value;
      int num3 = Mathf.CeilToInt((float) graphics.maxProductRenderCapacity * num2).Min(graphics.maxProductRenderCapacity);
      ArrayUtils.EnsureLengthPow2NoCopy<ProductsRenderer.ProductInstanceQDataPair>(ref this.m_productsToRenderDynamic, num3);
      Assert.That<int>(num3).IsNotZero();
      if ((num3 - 1) / 3 >= graphics.productRenderOffsets.Length)
      {
        Log.Warning(string.Format("NumToRender would go out of bounds of array for `{0}`", (object) this.m_truck.Id));
        num3 = graphics.productRenderOffsets.Length * 3;
      }
      uint seed = 13371337;
      int num4 = 0;
      int index1 = 0;
label_20:
      while (index1 < num3)
      {
        ++num4;
        if (num4 >= 1048576)
        {
          Log.Error("While loop overflow!");
          break;
        }
        System.Numerics.Vector3 productRenderOffset = graphics.productRenderOffsets[index1 / 3];
        UnityEngine.Vector3 position1 = currentPosition;
        UnityEngine.Vector3 position2 = futurePosition;
        UnityEngine.Vector3 vector3_1 = new UnityEngine.Vector3(productRenderOffset.X, productRenderOffset.Y, productRenderOffset.Z);
        UnityEngine.Vector3 vector3_2 = parent.CurrentRotation * vector3_1;
        UnityEngine.Vector3 vector3_3 = parent.FutureRotation * vector3_1;
        position1.x += vector3_2.x;
        position1.y += vector3_2.y;
        position1.z += vector3_2.z;
        position2.x += vector3_3.x;
        position2.y += vector3_3.y;
        position2.z += vector3_3.z;
        ProductsRenderer.ProductInstanceQData productInstanceQdata1 = new ProductsRenderer.ProductInstanceQData(position1, new UnityEngine.Vector4(parent.CurrentRotation.x, parent.CurrentRotation.y, parent.CurrentRotation.z, parent.CurrentRotation.w));
        ProductsRenderer.ProductInstanceQData other1 = new ProductsRenderer.ProductInstanceQData(position2, new UnityEngine.Vector4(parent.FutureRotation.x, parent.FutureRotation.y, parent.FutureRotation.z, parent.FutureRotation.w));
        ImmutableArray<ushort> immutableArray = stackOffsetsPacked[(int) this.m_productId.Value];
        if (immutableArray.IsNotEmpty)
        {
          int index2 = 2;
          ushort num5 = immutableArray[index2];
          int num6 = 0;
          while (true)
          {
            if (num6 < 3 && num6 + index2 < immutableArray.Length)
            {
              ushort offsets = immutableArray[num6 + index2];
              ProductsRenderer.ProductInstanceQData productInstanceQdata2 = productInstanceQdata1;
              ProductsRenderer.ProductInstanceQData other2 = other1;
              CountableProductProto product = this.m_truck.Cargo.FirstOrPhantom.Product as CountableProductProto;
              if ((Mafi.Core.Prototypes.Proto) product != (Mafi.Core.Prototypes.Proto) null)
              {
                if (product.Graphics.AllowPackingNoise)
                {
                  offsets = ProductsRenderer.ProductInstanceDataUtils.ApplyNoiseToOffsets(ref seed, offsets);
                  productInstanceQdata2 = productInstanceQdata2.ApplyNoiseToYaw(seed, 10f);
                  other2 = other2.ApplyNoiseToYaw(seed, 10f);
                }
                if (num6 % 2 == 0 && product.Graphics.PackingMode == CountableProductStackingMode.StackedAlternating)
                {
                  productInstanceQdata2 = productInstanceQdata2.ApplyOffsetToYaw(90f);
                  other2 = other2.ApplyOffsetToYaw(90f);
                }
              }
              this.m_productsToRenderDynamic[index1] = productInstanceQdata2.SetOffsetPacked(offsets).PairWith(other2);
              ++index1;
              if (index1 < num3)
                ++num6;
              else
                goto label_20;
            }
            else
              goto label_20;
          }
        }
        else
        {
          this.m_productsToRenderDynamic[index1] = productInstanceQdata1.PairWith(other1);
          index1 += 3;
        }
      }
      this.m_productsRenderer.PrepareRenderDynamicQuaternions(this.m_productId, this.m_productsToRenderDynamic, num3);
    }

    public override void SetTruck(Truck truck) => this.m_truck = truck.CheckNotNull<Truck>();

    public override void Reset()
    {
      base.Reset();
      this.m_truck = (Truck) null;
    }

    public FlatBedAttachmentMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
