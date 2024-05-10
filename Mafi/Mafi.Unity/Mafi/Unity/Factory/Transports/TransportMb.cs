// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Factory.Transports.TransportMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Core.Entities.Static;
using Mafi.Core.Factory.Transports;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Unity.Entities;
using Mafi.Unity.Entities.Static;
using Mafi.Unity.InstancedRendering;
using Mafi.Utils;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Factory.Transports
{
  internal class TransportMb : 
    StaticEntityMb,
    IEntityMbWithSyncUpdate,
    IEntityMb,
    IDestroyableEntityMb,
    IEntityMbWithSimUpdateEnd
  {
    public const string MOVING_GO_NAME = "moving";
    private static readonly RelTile1f FLOW_INDICATOR_RANGE_RADIUS;
    private Mafi.Core.Factory.Transports.Transport m_transportEntity;
    private ProductsRenderer m_productsRenderer;
    private ProductsSlimIdManager m_productsSlimIdManager;
    private ImmutableArray<TransportFlowIndicatorPose> m_flowIndicatorPoses;
    private InstancedFluidIndicatorState[] m_flowIndicatorStates;
    private int[] m_indicatorWaypointIndices;
    private TransportFlow[] m_indicatorFlows;
    private float m_speedMult;
    private float m_prevTexOffset;
    public float MovedOffset;
    private int m_prevStepsTotal;
    private int m_prevProductsVersion;
    private bool m_flowIndicatorsGraphicsDirty;
    /// <summary>Pre-computed positions and rotations of waypoints.</summary>
    private ProductsRenderer.ProductInstanceData[] m_waypointsConverted;
    private bool m_renderProducts;
    private ProductsRenderer.ProductInstanceData[] m_productsToRenderStatic;
    private ProductsRenderer.ProductInstanceDataPair[] m_productsToRenderDynamic;
    private ProductSlimId[] m_productIds;
    private int m_movedSteps;
    private bool m_updatedProductsLastTime;
    private bool m_allProductsSameStatic;
    private bool m_lastStepMoving;
    private int m_productsCountStatic;
    private int m_flowIndicatorUpdateCounter;
    private int m_flowIndicatorRangeWaypoints;

    public ReadOnlyArray<InstancedFluidIndicatorState> FlowIndicatorStates
    {
      get => this.m_flowIndicatorStates.AsReadOnlyArray<InstancedFluidIndicatorState>();
    }

    public float MovementTextureOffsetFrom => this.m_prevTexOffset;

    public float MovementTextureOffsetTo => this.m_prevTexOffset + this.MovedOffset;

    public bool IsMoving => this.m_movedSteps != 0;

    public ColorRgba PipeColor { get; private set; }

    public ColorRgba PipeAccentColor { get; private set; }

    public bool AreFlowIndicatorsDirty => this.m_flowIndicatorsGraphicsDirty;

    public bool ArePipeColorsDirty { get; private set; }

    public bool ColorIsFromProducts => !this.m_renderProducts;

    public void Initialize(
      Mafi.Core.Factory.Transports.Transport transport,
      ProductsRenderer productsRenderer,
      ProductsSlimIdManager productsSlimIdManager,
      AssetsDb assetsDb)
    {
      Assert.That<Mafi.Core.Factory.Transports.Transport>(this.m_transportEntity).IsNull<Mafi.Core.Factory.Transports.Transport>("Already initialized.");
      StaticEntityTransform entityTransform = new StaticEntityTransform(this.transform.localPosition, Quaternion.identity, Vector3.one);
      this.transform.localPosition = Vector3.zero;
      this.Initialize((IStaticEntity) transport, entityTransform);
      this.m_transportEntity = transport.CheckNotNull<Mafi.Core.Factory.Transports.Transport>();
      this.m_productsRenderer = productsRenderer;
      this.m_productsSlimIdManager = productsSlimIdManager;
      this.m_renderProducts = transport.Prototype.Graphics.RenderProducts;
      this.m_prevStepsTotal = transport.MovedStepsTotal;
      Material sharedMaterial = assetsDb.GetSharedMaterial(transport.Prototype.Graphics.MaterialPath);
      this.m_speedMult = (float) -((double) this.m_transportEntity.Prototype.SpeedPerTick.ToUnityUnits() * (double) sharedMaterial.mainTextureScale.x) / transport.Prototype.Graphics.TransportUvLength.ToUnityUnits();
      this.m_flowIndicatorPoses = transport.Prototype.Graphics.FlowIndicator.IsNone ? ImmutableArray<TransportFlowIndicatorPose>.Empty : transport.Trajectory.FlowIndicatorsPoses;
      if (this.m_flowIndicatorPoses.IsNotEmpty)
      {
        this.m_flowIndicatorRangeWaypoints = (TransportMb.FLOW_INDICATOR_RANGE_RADIUS.Value / this.m_transportEntity.Prototype.SpeedPerTick.Value).ToIntCeiled();
        this.m_indicatorFlows = new TransportFlow[this.m_flowIndicatorPoses.Length];
        this.m_indicatorWaypointIndices = new int[this.m_flowIndicatorPoses.Length];
        this.m_flowIndicatorStates = new InstancedFluidIndicatorState[this.m_flowIndicatorPoses.Length];
        ImmutableArray<int> segmentWaypointIndices = this.m_transportEntity.Trajectory.CurveSegmentWaypointIndices;
        for (int index = 0; index < this.m_flowIndicatorPoses.Length; ++index)
        {
          int segmentIndex = this.m_flowIndicatorPoses[index].SegmentIndex;
          int num = segmentWaypointIndices[segmentIndex] + ((segmentWaypointIndices[segmentIndex + 1] - segmentWaypointIndices[segmentIndex]) * this.m_flowIndicatorPoses[index].PercentOfSection.ToFix32()).ToIntRounded();
          this.m_indicatorWaypointIndices[index] = num;
          this.m_flowIndicatorStates[index] = new InstancedFluidIndicatorState(transport.Prototype.Graphics.FlowIndicator.Value.Parameters);
        }
        this.updateFlowIndicators();
      }
      if (this.m_renderProducts)
        return;
      this.updatePipeColor();
    }

    void IEntityMbWithSyncUpdate.SyncUpdate(GameTime time)
    {
      Log.Error("Nope");
      this.SyncTextureOffsets();
    }

    public void SyncTextureOffsets()
    {
      this.m_prevTexOffset = (float) (((double) this.m_prevTexOffset + (double) this.MovedOffset) % 1.0);
      this.MovedOffset = (float) this.m_movedSteps * this.m_speedMult;
    }

    void IEntityMbWithSimUpdateEnd.SimUpdateEnd()
    {
      int num1 = this.m_movedSteps = this.m_transportEntity.MovedStepsTotal - this.m_prevStepsTotal;
      this.m_prevStepsTotal = this.m_transportEntity.MovedStepsTotal;
      if (this.m_flowIndicatorPoses.IsNotEmpty)
      {
        ++this.m_flowIndicatorUpdateCounter;
        if (this.m_flowIndicatorUpdateCounter >= 8)
        {
          this.m_flowIndicatorUpdateCounter = 0;
          this.updateFlowIndicators();
        }
      }
      if (!this.m_renderProducts && this.m_transportEntity.ProductsStateVersion != this.m_prevProductsVersion)
        this.updatePipeColor();
      if (!this.m_renderProducts || this.m_transportEntity.TransportedProducts.IsEmpty<TransportedProductMutable>())
      {
        this.m_updatedProductsLastTime = false;
        this.m_prevProductsVersion = this.m_transportEntity.ProductsStateVersion;
      }
      else
      {
        ImmutableArray<TransportWaypoint> waypoints;
        if (this.m_waypointsConverted != null)
        {
          int length1 = this.m_waypointsConverted.Length;
          waypoints = this.m_transportEntity.Trajectory.Waypoints;
          int length2 = waypoints.Length;
          if (length1 == length2)
            goto label_10;
        }
        waypoints = this.m_transportEntity.Trajectory.Waypoints;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_waypointsConverted = waypoints.ToArray<ProductsRenderer.ProductInstanceData>(TransportMb.\u003C\u003EO.\u003C0\u003E__FromWaypoint ?? (TransportMb.\u003C\u003EO.\u003C0\u003E__FromWaypoint = new Func<TransportWaypoint, ProductsRenderer.ProductInstanceData>(ProductsRenderer.ProductInstanceData.FromWaypoint)));
        int maxProducts = this.m_transportEntity.Trajectory.MaxProducts;
        this.m_productsToRenderStatic = new ProductsRenderer.ProductInstanceData[maxProducts];
        this.m_productsToRenderDynamic = new ProductsRenderer.ProductInstanceDataPair[maxProducts];
        this.m_productIds = new ProductSlimId[maxProducts];
label_10:
        Assertion<ProductsRenderer.ProductInstanceData[]> actual = Assert.That<ProductsRenderer.ProductInstanceData[]>(this.m_waypointsConverted);
        waypoints = this.m_transportEntity.Trajectory.Waypoints;
        int length = waypoints.Length;
        actual.HasLength<ProductsRenderer.ProductInstanceData>(length);
        ImmutableArray<LooseProductSlimId> slimIdToLoose = this.m_productsRenderer.SlimIdToLoose;
        Queueue<TransportedProductMutable> transportedProductsMutable = ((ITransportFriend) this.m_transportEntity).TransportedProductsMutable;
        ImmutableArray<ImmutableArray<ushort>> stackOffsetsPacked = this.m_productsRenderer.StackOffsetsPacked;
        if (num1 == 0 || !this.m_updatedProductsLastTime)
        {
          ProductSlimId slimId = transportedProductsMutable.First.SlimId;
          int productsCount = 0;
          if (this.m_transportEntity.ProductsStateVersion != this.m_prevProductsVersion || this.m_lastStepMoving)
          {
            ArrayUtils.EnsureLengthPow2NoCopy<ProductsRenderer.ProductInstanceData>(ref this.m_productsToRenderStatic, transportedProductsMutable.Count);
            ArrayUtils.EnsureLengthPow2NoCopy<ProductSlimId>(ref this.m_productIds, transportedProductsMutable.Count);
            bool flag = true;
            int productsIndexBase = this.m_transportEntity.ProductsIndexBase;
            for (int index1 = 0; index1 < transportedProductsMutable.Count; ++index1)
            {
              ref TransportedProductMutable local = ref transportedProductsMutable.GetRefAt(index1);
              int index2 = productsIndexBase + (int) local.TrajectoryIndexRelative;
              if (slimId != local.SlimId)
                flag = false;
              if (index2 < 0 || index2 >= this.m_waypointsConverted.Length)
              {
                Log.Error(string.Format("Invalid transported product waypoint index: {0},", (object) index2) + string.Format(" waypoints {0}", (object) this.m_waypointsConverted.Length));
                break;
              }
              local.LastSeenIndexAbsoluteForUi = (ushort) index2;
              int nextProdIndex = getNextProdIndex();
              this.m_productIds[nextProdIndex] = local.SlimId;
              ProductsRenderer.ProductInstanceData productInstanceData1 = this.m_waypointsConverted[index2];
              if (slimIdToLoose[(int) local.SlimId.Value].IsNotPhantom)
              {
                this.m_productsToRenderStatic[nextProdIndex] = productInstanceData1.SetTexIdQuantityAndSeq((byte) ((uint) slimIdToLoose[(int) local.SlimId.Value].Value - 1U), local.QuantitySlim, local.SeqNumber);
              }
              else
              {
                ImmutableArray<ushort> immutableArray = stackOffsetsPacked[(int) local.SlimId.Value];
                if (immutableArray.IsNotEmpty)
                {
                  int num2 = local.QuantitySlim == (byte) 2 ? 0 : 2;
                  uint seqNumber = (uint) local.SeqNumber;
                  for (int index3 = 0; index3 < (int) local.QuantitySlim && index3 + num2 < immutableArray.Length; ++index3)
                  {
                    if (index3 != 0)
                      nextProdIndex = getNextProdIndex();
                    this.m_productIds[nextProdIndex] = local.SlimId;
                    ProductsRenderer.ProductInstanceData productInstanceData2 = productInstanceData1;
                    ushort offsets = immutableArray[index3 + num2];
                    CountableProductProto fullOrPhantom = local.SlimId.ToFullOrPhantom(this.m_productsSlimIdManager) as CountableProductProto;
                    if ((Proto) fullOrPhantom != (Proto) null)
                    {
                      if (fullOrPhantom.Graphics.AllowPackingNoise)
                      {
                        offsets = ProductsRenderer.ProductInstanceDataUtils.ApplyNoiseToOffsets(ref seqNumber, offsets);
                        productInstanceData2 = productInstanceData2.ApplyNoiseToYaw(seqNumber, 3);
                      }
                      if (index3 % 2 == 0 && fullOrPhantom.Graphics.PackingMode == CountableProductStackingMode.StackedAlternating)
                        productInstanceData2 = productInstanceData2.SetAlternativeTexture();
                    }
                    this.m_productsToRenderStatic[nextProdIndex] = productInstanceData2.SetOffsetPacked(offsets);
                  }
                }
                else
                  this.m_productsToRenderStatic[nextProdIndex] = productInstanceData1;
              }
            }
            this.m_allProductsSameStatic = flag;
            this.m_productsCountStatic = productsCount;
          }
          else
            Assert.That<int>((int) transportedProductsMutable.First.LastSeenIndexAbsoluteForUi).IsEqualTo(this.m_transportEntity.ProductsIndexBase + (int) transportedProductsMutable.First.TrajectoryIndexRelative, "Products moved without UI knowing about it?");
          if (this.m_allProductsSameStatic)
            this.m_productsRenderer.PrepareRenderStatic(slimId, this.m_productsToRenderStatic, this.m_productsCountStatic);
          else
            this.m_productsRenderer.PrepareRenderStaticMixed(this.m_productIds, this.m_productsToRenderStatic, this.m_productsCountStatic);
          this.m_prevProductsVersion = this.m_transportEntity.ProductsStateVersion;
          this.m_lastStepMoving = false;
          this.m_updatedProductsLastTime = true;

          int getNextProdIndex()
          {
            int productsCount = productsCount;
            if (productsCount >= this.m_productsToRenderStatic.Length)
            {
              int length = this.m_productsToRenderStatic.Length * 2;
              ArrayUtils.EnsureLengthPow2KeepContents<ProductsRenderer.ProductInstanceData>(ref this.m_productsToRenderStatic, length);
              ArrayUtils.EnsureLengthPow2KeepContents<ProductSlimId>(ref this.m_productIds, length);
            }
            ++productsCount;
            return productsCount;
          }
        }
        else
        {
          ArrayUtils.EnsureLengthPow2NoCopy<ProductsRenderer.ProductInstanceDataPair>(ref this.m_productsToRenderDynamic, transportedProductsMutable.Count);
          ArrayUtils.EnsureLengthPow2NoCopy<ProductSlimId>(ref this.m_productIds, transportedProductsMutable.Count);
          int productsIndexBase = this.m_transportEntity.ProductsIndexBase;
          bool flag = true;
          ProductSlimId slimId = transportedProductsMutable.First.SlimId;
          int productsCount = 0;
          for (int index4 = 0; index4 < transportedProductsMutable.Count; ++index4)
          {
            ref TransportedProductMutable local = ref transportedProductsMutable.GetRefAt(index4);
            int indexAbsoluteForUi = (int) local.LastSeenIndexAbsoluteForUi;
            int index5 = productsIndexBase + (int) local.TrajectoryIndexRelative;
            if (slimId != local.SlimId)
              flag = false;
            if (index5 < 0 || index5 >= this.m_waypointsConverted.Length)
            {
              Log.Error(string.Format("Invalid transported product waypoint index: {0},", (object) index5) + string.Format(" waypoints {0}", (object) this.m_waypointsConverted.Length));
              break;
            }
            local.LastSeenIndexAbsoluteForUi = (ushort) index5;
            int nextProdIndex = getNextProdIndex();
            this.m_productIds[nextProdIndex] = local.SlimId;
            ProductsRenderer.ProductInstanceData productInstanceData3 = this.m_waypointsConverted[indexAbsoluteForUi];
            ProductsRenderer.ProductInstanceData other1 = this.m_waypointsConverted[index5];
            if (slimIdToLoose[(int) local.SlimId.Value].IsNotPhantom)
            {
              this.m_productsToRenderDynamic[nextProdIndex] = productInstanceData3.SetTexIdQuantityAndSeq((byte) ((uint) slimIdToLoose[(int) local.SlimId.Value].Value - 1U), local.QuantitySlim, local.SeqNumber).PairWith(other1);
            }
            else
            {
              ImmutableArray<ushort> immutableArray = stackOffsetsPacked[(int) local.SlimId.Value];
              if (immutableArray.IsNotEmpty)
              {
                int num3 = local.QuantitySlim == (byte) 2 ? 0 : 2;
                uint seqNumber = (uint) local.SeqNumber;
                for (int index6 = 0; index6 < (int) local.QuantitySlim && index6 + num3 < immutableArray.Length; ++index6)
                {
                  if (index6 != 0)
                    nextProdIndex = getNextProdIndex();
                  this.m_productIds[nextProdIndex] = local.SlimId;
                  ProductsRenderer.ProductInstanceData productInstanceData4 = productInstanceData3;
                  ProductsRenderer.ProductInstanceData other2 = other1;
                  ushort offsets = immutableArray[index6 + num3];
                  CountableProductProto fullOrPhantom = local.SlimId.ToFullOrPhantom(this.m_productsSlimIdManager) as CountableProductProto;
                  if ((Proto) fullOrPhantom != (Proto) null)
                  {
                    if (fullOrPhantom.Graphics.AllowPackingNoise)
                    {
                      offsets = ProductsRenderer.ProductInstanceDataUtils.ApplyNoiseToOffsets(ref seqNumber, offsets);
                      productInstanceData4 = productInstanceData4.ApplyNoiseToYaw(seqNumber, 3);
                      other2 = other2.ApplyNoiseToYaw(seqNumber, 3);
                    }
                    if (index6 % 2 == 0 && fullOrPhantom.Graphics.PackingMode == CountableProductStackingMode.StackedAlternating)
                    {
                      productInstanceData4 = productInstanceData4.SetAlternativeTexture();
                      other2 = other2.SetAlternativeTexture();
                    }
                  }
                  this.m_productsToRenderDynamic[nextProdIndex] = productInstanceData4.SetOffsetPacked(offsets).PairWith(other2);
                }
              }
              else
                this.m_productsToRenderDynamic[nextProdIndex] = productInstanceData3.PairWith(other1);
            }
          }
          if (flag)
            this.m_productsRenderer.PrepareRenderDynamic(slimId, this.m_productsToRenderDynamic, productsCount);
          else
            this.m_productsRenderer.PrepareRenderDynamicMixed(this.m_productIds, this.m_productsToRenderDynamic, productsCount);
          this.m_prevProductsVersion = this.m_transportEntity.ProductsStateVersion;
          this.m_lastStepMoving = true;

          int getNextProdIndex()
          {
            int productsCount = productsCount;
            if (productsCount >= this.m_productsToRenderDynamic.Length)
            {
              int length = this.m_productsToRenderDynamic.Length * 2;
              ArrayUtils.EnsureLengthPow2KeepContents<ProductsRenderer.ProductInstanceDataPair>(ref this.m_productsToRenderDynamic, length);
              ArrayUtils.EnsureLengthPow2KeepContents<ProductSlimId>(ref this.m_productIds, length);
            }
            ++productsCount;
            return productsCount;
          }
        }
      }
    }

    private void updateFlowIndicators()
    {
      this.m_flowIndicatorsGraphicsDirty = false;
      this.getFlowAtPositions(this.m_indicatorWaypointIndices, this.m_indicatorFlows);
      RelTile1f speedPerTick = this.m_transportEntity.Prototype.SpeedPerTick;
      for (int index = 0; index < this.m_flowIndicatorPoses.Length; ++index)
      {
        TransportFlow indicatorFlow = this.m_indicatorFlows[index];
        float metersPerTick = indicatorFlow.IsFlowing ? speedPerTick.ToUnityUnits() : 0.0f;
        this.m_flowIndicatorStates[index].SetFlow(metersPerTick);
        this.m_flowIndicatorStates[index].SetColor(indicatorFlow.Color);
        if ((double) metersPerTick != 0.0 || this.m_flowIndicatorStates[index].IsTransitioning)
          this.m_flowIndicatorsGraphicsDirty = true;
      }
    }

    private void updatePipeColor()
    {
      if (this.PipeColor != this.m_transportEntity.TransportColor || this.PipeAccentColor != this.m_transportEntity.TransportAccentColor)
      {
        this.ArePipeColorsDirty = true;
        this.PipeColor = this.m_transportEntity.TransportColor;
        this.PipeAccentColor = this.m_transportEntity.TransportAccentColor;
      }
      else
        this.ArePipeColorsDirty = false;
    }

    private void getFlowAtPositions(int[] indicatorWpIndices, TransportFlow[] outFlow)
    {
      if (indicatorWpIndices == null || outFlow == null)
      {
        Log.Error(string.Format("Arg unexpectedly null {0} {1}", (object) indicatorWpIndices, (object) outFlow));
      }
      else
      {
        Assert.That<int[]>(indicatorWpIndices).HasLength<int>(outFlow.Length, "Invalid input arrays lengths.");
        if (indicatorWpIndices.Length == 0)
          return;
        Queueue<TransportedProductMutable> transportedProductsMutable = ((ITransportFriend) this.m_transportEntity).TransportedProductsMutable;
        int productsIndexBase = this.m_transportEntity.ProductsIndexBase;
        ImmutableArray<ProductProto> managedProtos = this.m_transportEntity.TransportManager.ProductsManager.SlimIdManager.ManagedProtos;
        int index1 = 0;
        int index2 = transportedProductsMutable.Count - 1;
        for (; index1 < indicatorWpIndices.Length; ++index1)
        {
          if (index2 < 0)
          {
            outFlow[index1] = TransportFlow.Empty;
          }
          else
          {
            int indicatorWpIndex = indicatorWpIndices[index1];
            int num1 = indicatorWpIndex - this.m_flowIndicatorRangeWaypoints;
            while (index2 >= 0 && productsIndexBase + (int) transportedProductsMutable[index2].TrajectoryIndexRelative < num1)
              --index2;
            if (index2 < 0)
            {
              outFlow[index1] = TransportFlow.Empty;
            }
            else
            {
              int num2 = indicatorWpIndex + this.m_flowIndicatorRangeWaypoints;
              int index3 = -1;
              int num3 = int.MaxValue;
              for (; index2 >= 0; --index2)
              {
                TransportedProductMutable transportedProductMutable = transportedProductsMutable[index2];
                int num4 = productsIndexBase + (int) transportedProductMutable.TrajectoryIndexRelative;
                if (num4 <= num2)
                {
                  int num5 = num4 - indicatorWpIndex;
                  if (num5 < 0)
                  {
                    index3 = index2;
                    num3 = num5;
                  }
                  else
                  {
                    if (num3 >= num5)
                    {
                      index3 = index2;
                      break;
                    }
                    break;
                  }
                }
                else
                  break;
              }
              if (index3 < 0)
              {
                outFlow[index1] = TransportFlow.Empty;
              }
              else
              {
                TransportedProductMutable transportedProductMutable = transportedProductsMutable[index3];
                int num6 = productsIndexBase + (int) transportedProductMutable.TrajectoryIndexRelative;
                outFlow[index1] = new TransportFlow(num6 != (int) transportedProductMutable.LastSeenIndexAbsoluteForUi, managedProtos[(int) transportedProductMutable.SlimId.Value].Graphics.Color);
              }
            }
          }
        }
        for (int index4 = 0; index4 < transportedProductsMutable.Count; ++index4)
        {
          ref TransportedProductMutable local = ref transportedProductsMutable.GetRefAt(index4);
          local.LastSeenIndexAbsoluteForUi = (ushort) ((uint) productsIndexBase + (uint) local.TrajectoryIndexRelative);
        }
      }
    }

    public TransportMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_flowIndicatorStates = Array.Empty<InstancedFluidIndicatorState>();
      this.m_prevProductsVersion = -1;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static TransportMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      TransportMb.FLOW_INDICATOR_RANGE_RADIUS = 2.0.Tiles();
    }
  }
}
