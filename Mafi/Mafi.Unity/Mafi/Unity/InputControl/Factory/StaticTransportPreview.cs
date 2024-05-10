// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Factory.StaticTransportPreview
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Entities.Validators;
using Mafi.Core.Factory.Transports;
using Mafi.Core.Ports.Io;
using Mafi.Localization;
using Mafi.Unity.Entities;
using Mafi.Unity.InputControl.LayoutEntityPlacing;
using System;
using System.Collections.Generic;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Factory
{
  public class StaticTransportPreview : IStaticEntityPreview
  {
    private readonly TransportPreviewManager m_manager;
    private readonly TransportPreviewVisualizer m_transportPreviewVisualizer;
    private Tile3i[] m_currentPivots;
    private Vector3i m_currentStartDirection;
    private Vector3i m_currentEndDirection;
    private TransportTrajectory m_originalTrajectory;
    private TransportTrajectory m_currentTrajectory;
    private Option<TransportTrajectory> m_requestedPreview;
    private Option<TransportTrajectory> m_requestedPreviewOnSimThread;
    private ImmutableArray<Tile2i> m_currentPillarHints;
    private ImmutableArray<Tile2i> m_requestedPillarHints;
    private ImmutableArray<Tile2i> m_requestedPillarHintsOnSim;
    private TransportPreviewSpec? m_previewSpec;
    private TransportPreviewSpec? m_processedPreviewSpec;
    private bool m_requestProcessed;
    private bool m_previewIsCurrent;
    private bool m_forceProcessRequest;
    private bool m_inputIsConnected;
    private bool m_outputIsConnected;
    private Tile3i m_basePosition;
    private ThicknessIRange m_allowedPlacementRange;

    public bool IsDestroyed => this.m_originalTrajectory == null;

    public IStaticEntityProto EntityProto
    {
      get => (IStaticEntityProto) this.m_originalTrajectory.TransportProto;
    }

    public AssetValue Price { get; private set; }

    public EntityValidationResult? ValidationResult { get; private set; }

    public bool ShowOceanAreas => false;

    public StaticTransportPreview(TransportPreviewManager manager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_currentPivots = Array.Empty<Tile3i>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_manager = manager;
      this.m_transportPreviewVisualizer = manager.GeTransportPreviewVisualizer();
    }

    public void Initialize(TransportTrajectory trajectory, ImmutableArray<Tile2i> pillarHints)
    {
      if (!this.IsDestroyed)
      {
        Log.Warning("Initializing non-cleared transport preview.");
        this.clear();
      }
      this.m_originalTrajectory = this.m_currentTrajectory = trajectory;
      this.m_currentPillarHints = pillarHints;
      this.m_currentPivots = this.m_originalTrajectory.Pivots.ToArray();
      this.m_currentStartDirection = trajectory.StartDirection.Vector3i;
      this.m_currentEndDirection = trajectory.EndDirection.Vector3i;
      this.Price = trajectory.TransportProto.GetPriceFor(trajectory.Pivots);
      this.m_inputIsConnected = false;
      this.m_outputIsConnected = false;
      HeightTilesI height1 = HeightTilesI.MaxValue;
      HeightTilesI height2 = this.m_originalTrajectory.Pivots.First.Height;
      HeightTilesI heightTilesI = height2;
      Tile2i tile2i = new Tile2i();
      foreach (Tile3i pivot in this.m_originalTrajectory.Pivots)
      {
        HeightTilesI nonCollidingHeight = TransportHelper.GetLowestNonCollidingHeight(this.m_manager.TerrainManager[pivot.Xy]);
        if (nonCollidingHeight < height1)
        {
          height1 = nonCollidingHeight;
          tile2i = pivot.Xy;
        }
        if (pivot.Height < height2)
          height2 = pivot.Height;
        else if (pivot.Height > heightTilesI)
          heightTilesI = pivot.Height;
      }
      this.m_basePosition = tile2i.ExtendHeight(height1);
      this.m_allowedPlacementRange = new ThicknessIRange((height1 - height2).Min(ThicknessTilesI.Zero), (TransportPillarProto.MAX_PILLAR_HEIGHT - (heightTilesI - height1) - ThicknessTilesI.One).Max(ThicknessTilesI.Zero));
      this.m_transportPreviewVisualizer.ShowPreview(new TransportPreviewSpec(okTraj: (Option<TransportTrajectory>) this.m_currentTrajectory, showStartPort: true, showEndPort: true, isReadyToBuild: true));
    }

    public void ApplyTransformDelta(
      RelTile3i deltaPosition,
      Rotation90 deltaRotation,
      bool deltaReflection,
      Tile2i pivot)
    {
      Matrix2i m = Matrix2i.FromRotationFlip(deltaRotation, deltaReflection);
      for (int index = 0; index < this.m_currentPivots.Length; ++index)
      {
        Tile3i currentPivot = this.m_currentPivots[index];
        this.m_currentPivots[index] = deltaPosition + (pivot + m.Transform(currentPivot.Xy - pivot)).ExtendHeight(currentPivot.Height);
      }
      this.m_currentPillarHints = this.m_currentPillarHints.Map<Tile2i, Tile2i, Matrix2i, RelTile2i>(pivot, m, deltaPosition.Xy, (Func<Tile2i, Tile2i, Matrix2i, RelTile2i, Tile2i>) ((x, p, mat, d) => d + p + mat.Transform(x - p)));
      Vector2i vector2i;
      if (this.m_currentStartDirection.Z == 0)
      {
        vector2i = m.Transform(this.m_currentStartDirection.Xy);
        this.m_currentStartDirection = vector2i.ExtendZ(0);
      }
      if (this.m_currentEndDirection.Z == 0)
      {
        vector2i = m.Transform(this.m_currentEndDirection.Xy);
        this.m_currentEndDirection = vector2i.ExtendZ(0);
      }
      string error;
      if (TransportTrajectory.TryCreateFromPivots(this.m_originalTrajectory.TransportProto, ((ICollection<Tile3i>) this.m_currentPivots).ToImmutableArray<Tile3i>(), new RelTile3i?(new RelTile3i(this.m_currentStartDirection)), new RelTile3i?(new RelTile3i(this.m_currentEndDirection)), out this.m_currentTrajectory, out error))
      {
        if (this.m_transportPreviewVisualizer.PreviewSpec.OkTraj.HasValue)
          this.m_transportPreviewVisualizer.ShowPreview(new TransportPreviewSpec(okTraj: (Option<TransportTrajectory>) this.m_currentTrajectory, showStartPort: true, showEndPort: true, isReadyToBuild: true));
        else
          this.m_transportPreviewVisualizer.ShowPreview(new TransportPreviewSpec(showStartPort: true, showEndPort: true, errTraj: (Option<TransportTrajectory>) this.m_currentTrajectory));
        this.m_requestedPreview = (Option<TransportTrajectory>) this.m_currentTrajectory;
        this.m_requestedPillarHints = this.m_currentPillarHints;
        this.m_requestProcessed = false;
      }
      else
      {
        Log.Error("Trajectory cannot be created after transform. " + error);
        this.m_transportPreviewVisualizer.Clear();
      }
    }

    public ThicknessTilesI GetEstPlacementHeight() => ThicknessTilesI.Zero;

    public void GetPlacementParams(
      out Tile3i basePosition,
      out bool canBeReflected,
      out ThicknessIRange allowedPlacementRange)
    {
      basePosition = this.m_basePosition;
      canBeReflected = true;
      allowedPlacementRange = this.m_allowedPlacementRange;
    }

    public void GetPortTypes(Set<ShapeTypePair> portsSet)
    {
      portsSet.Add(new ShapeTypePair(this.m_originalTrajectory.TransportProto.PortsShape, IoPortType.Input));
      portsSet.Add(new ShapeTypePair(this.m_originalTrajectory.TransportProto.PortsShape, IoPortType.Output));
    }

    public void GetPortLocations(Dict<IoPortKey, IoPortType> ports)
    {
      TransportTrajectory originalTrajectory = this.m_originalTrajectory;
      ports.AddAndAssertNew(new IoPortKey(originalTrajectory.Pivots.First, originalTrajectory.StartDirection.ToDirection903d()), IoPortType.Input);
      ports.AddAndAssertNew(new IoPortKey(originalTrajectory.Pivots.Last, originalTrajectory.EndDirection.ToDirection903d()), IoPortType.Output);
    }

    public void HideConnectedPorts(Dict<IoPortKey, IoPortType> ports)
    {
      TransportTrajectory originalTrajectory = this.m_originalTrajectory;
      IoPortKey key1 = new IoPortKey(originalTrajectory.Pivots.First + originalTrajectory.StartDirection, -originalTrajectory.StartDirection.ToDirection903d());
      IoPortType ioPortType;
      this.m_inputIsConnected = ports.TryGetValue(key1, out ioPortType);
      IoPortKey key2 = new IoPortKey(originalTrajectory.Pivots.Last + originalTrajectory.EndDirection, -originalTrajectory.EndDirection.ToDirection903d());
      this.m_outputIsConnected = ports.TryGetValue(key2, out ioPortType);
    }

    public bool CanMoveUpDownIfValid() => true;

    public void UpdateConfigWithTransform(EntityConfigData data)
    {
      data.Trajectory = (Option<TransportTrajectory>) this.m_currentTrajectory;
      data.Pillars = new ImmutableArray<Tile2i>?(this.m_currentPillarHints);
    }

    public void SetPlacementPhase(EntityPlacementPhase placementPhase)
    {
    }

    public void SyncUpdate()
    {
      if (this.m_requestedPreview == (TransportTrajectory) null)
      {
        this.m_processedPreviewSpec = new TransportPreviewSpec?();
        this.m_requestedPreviewOnSimThread = Option<TransportTrajectory>.None;
      }
      else
      {
        if (this.m_requestProcessed)
          return;
        if (this.m_processedPreviewSpec.HasValue)
        {
          Assert.That<bool>(this.m_requestProcessed).IsFalse();
          this.m_previewSpec = new TransportPreviewSpec?(this.m_processedPreviewSpec.Value);
          this.ValidationResult = new EntityValidationResult?(this.m_processedPreviewSpec.Value.IsValid ? EntityValidationResult.Success : EntityValidationResult.CreateError(this.m_processedPreviewSpec.Value.ErrorMessage));
          this.m_previewIsCurrent = false;
          this.m_requestProcessed = true;
          this.m_processedPreviewSpec = new TransportPreviewSpec?();
        }
        TransportTrajectory transportTrajectory = this.m_requestedPreview.Value;
        if (!this.m_forceProcessRequest && !(this.m_requestedPreviewOnSimThread == (TransportTrajectory) null) && this.m_requestedPreviewOnSimThread.Value.Equals((object) transportTrajectory))
          return;
        this.m_requestedPreviewOnSimThread = (Option<TransportTrajectory>) transportTrajectory;
        this.m_requestedPillarHintsOnSim = this.m_requestedPillarHints;
        this.m_previewSpec = new TransportPreviewSpec?();
        this.m_processedPreviewSpec = new TransportPreviewSpec?();
        this.m_requestProcessed = false;
        this.m_forceProcessRequest = false;
      }
    }

    public void RenderUpdate()
    {
      if (!this.m_previewSpec.HasValue || this.m_previewIsCurrent)
        return;
      this.m_previewIsCurrent = true;
      this.m_transportPreviewVisualizer.ShowPreview(this.m_previewSpec.Value);
    }

    public void SimUpdate()
    {
      if (this.m_requestProcessed)
        return;
      TransportTrajectory valueOrNull = this.m_requestedPreviewOnSimThread.ValueOrNull;
      if (valueOrNull == null || this.m_processedPreviewSpec.HasValue)
        return;
      CanBuildTransportResult result;
      LocStrFormatted error;
      bool flag1 = this.m_manager.TransportsManager.CanBuildOrJoinTransport(valueOrNull.TransportProto, valueOrNull.Pivots, this.m_requestedPillarHintsOnSim.ToReadonlySet(), new Direction903d?(valueOrNull.StartDirection.ToDirection903d()), new Direction903d?(valueOrNull.EndDirection.ToDirection903d()), true, out result, out error, out Option<IStaticEntity> _, disallowMiniZipAtStart: this.m_inputIsConnected, disallowMiniZipAtEnd: this.m_outputIsConnected, skipExtraPillarsForBetterVisuals: true);
      ImmutableArray<PillarVisualsSpec> immutableArray1;
      if (result.NewTrajectory.HasValue)
        immutableArray1 = this.m_manager.CreatePillarsVisualSpec(result.NewTrajectory.Value, result.SupportedTiles);
      Option<TransportTrajectory> option1;
      Option<TransportTrajectory> option2;
      ImmutableArray<PillarVisualsSpec> immutableArray2;
      bool flag2;
      bool flag3;
      if (flag1)
      {
        option1 = result.NewTrajectory;
        option2 = Option<TransportTrajectory>.None;
        immutableArray2 = ImmutableArray<PillarVisualsSpec>.Empty;
        flag2 = !result.MiniZipJoinResultAtStart.HasValue && !result.ChangeDirectionNearStart.HasValue;
        flag3 = !result.MiniZipJoinResultAtEnd.HasValue && !result.ChangeDirectionNearEnd.HasValue;
      }
      else
      {
        option1 = Option<TransportTrajectory>.None;
        option2 = !option1.HasValue ? (Option<TransportTrajectory>) valueOrNull : option1;
        immutableArray2 = immutableArray1;
        immutableArray1 = ImmutableArray<PillarVisualsSpec>.Empty;
        flag2 = false;
        flag3 = false;
      }
      Option<TransportTrajectory> option3 = option1;
      ImmutableArray<PillarVisualsSpec> immutableArray3 = immutableArray1;
      Option<TransportTrajectory> option4 = option2;
      ImmutableArray<PillarVisualsSpec> immutableArray4 = immutableArray2;
      LocStrFormatted? errorMessage = new LocStrFormatted?(error);
      Option<TransportTrajectory> okTraj = option3;
      ImmutableArray<PillarVisualsSpec> okPillars = immutableArray3;
      MiniZipperAtResult? nullable = result.MiniZipperAtStart;
      MiniZipperAtResult miniZipperAtResult1 = nullable ?? PathFindingTransportPreview.CreateZipperResult(result.MiniZipJoinResultAtStart);
      nullable = result.MiniZipperAtEnd;
      MiniZipperAtResult miniZipperAtResult2 = nullable ?? PathFindingTransportPreview.CreateZipperResult(result.MiniZipJoinResultAtEnd);
      bool flag4 = flag1;
      int num1 = !flag2 ? 0 : (!this.m_inputIsConnected ? 1 : 0);
      int num2 = !flag3 ? 0 : (!this.m_outputIsConnected ? 1 : 0);
      Option<TransportTrajectory> errTraj = option4;
      ImmutableArray<PillarVisualsSpec> errPillars = immutableArray4;
      Option<TransportTrajectory> highlightTraj = new Option<TransportTrajectory>();
      MiniZipperAtResult miniZipAtStart = miniZipperAtResult1;
      MiniZipperAtResult miniZipAtEnd = miniZipperAtResult2;
      EntityHighlightSpec highlightEntity1 = new EntityHighlightSpec();
      int num3 = flag4 ? 1 : 0;
      this.m_processedPreviewSpec = new TransportPreviewSpec?(new TransportPreviewSpec(errorMessage, okTraj, okPillars, num1 != 0, num2 != 0, errTraj, errPillars, highlightTraj, miniZipAtStart, miniZipAtEnd, highlightEntity1, num3 != 0));
    }

    public void Invalidate() => this.m_requestProcessed = false;

    public void DestroyAndReturnToPool() => this.clear();

    private void clear()
    {
      this.ValidationResult = new EntityValidationResult?();
      this.m_currentTrajectory = (TransportTrajectory) null;
      this.m_processedPreviewSpec = new TransportPreviewSpec?();
      this.m_previewSpec = new TransportPreviewSpec?();
      this.m_requestedPreview = Option<TransportTrajectory>.None;
      this.m_currentPivots = Array.Empty<Tile3i>();
      this.m_transportPreviewVisualizer.Clear();
    }
  }
}
