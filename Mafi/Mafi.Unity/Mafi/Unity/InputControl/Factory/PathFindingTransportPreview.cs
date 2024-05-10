// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Factory.PathFindingTransportPreview
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Factory.Transports;
using Mafi.Core.GameLoop;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.Terrain;
using Mafi.Localization;
using Mafi.PathFinding;
using Mafi.Unity.Entities;
using Mafi.Unity.InputControl.Toolbar;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Factory
{
  /// <summary>
  /// This class can compute and display a preview transport between to points. It uses pathfinder to find the best
  /// connection. It maintains internal state of currently displayed preview so the user can call
  /// <see cref="M:Mafi.Unity.InputControl.Factory.PathFindingTransportPreview.ShowStartPreview(Mafi.Unity.InputControl.Factory.PathFindingTransportPreview.PreviewRequest,Mafi.ThicknessTilesI,System.Nullable{Mafi.Direction903d}@,Mafi.Option{Mafi.Core.Factory.Transports.TransportTrajectory}@,System.Boolean@)" /> and <see cref="M:Mafi.Unity.InputControl.Factory.PathFindingTransportPreview.ShowContinuationPreview(Mafi.Unity.InputControl.Factory.PathFindingTransportPreview.PreviewRequest,Mafi.ThicknessTilesI,Mafi.Option{Mafi.Core.Factory.Transports.TransportTrajectory}@,System.Boolean@,System.Boolean@)" /> every frame with desired locations
  /// and this class will handle all the caching.
  /// </summary>
  public class PathFindingTransportPreview
  {
    private static readonly Duration MAX_PF_DURATION;
    private readonly IGameLoopEvents m_gameLoopEvents;
    private readonly TransportsManager m_transportsManager;
    private readonly TransportPreviewManager m_transportPreviewManager;
    private readonly TerrainManager m_terrainManager;
    private readonly FloatingPricePopup m_pricePopup;
    private bool m_isActive;
    private readonly ITransportPathFinder m_pathFinder;
    private PathFindingTransportPreview.PreviewRequest? m_requestedPreview;
    private PathFindingTransportPreview.PreviewRequest? m_requestedPreviewOnSimThread;
    private bool m_newRequest;
    private TransportPreviewSpec? m_previewSpec;
    private TransportPreviewSpec? m_processedPreviewSpec;
    private bool m_requestProcessed;
    private Option<CanBuildTransportResult> m_canBuildResult;
    private Option<CanBuildTransportResult> m_canBuildResultOnSim;
    private Duration m_pfDurationLeft;
    private bool m_previewIsCurrent;
    private bool m_forceProcessRequest;
    private ThicknessTilesI m_relativeHeight;
    private readonly TransportPreviewVisualizer m_transportPreviewVisualizer;

    public bool PricePopupIsHidden => this.m_pricePopup.IsTemporarilyHidden;

    public PathFindingTransportPreview(
      ISimLoopEvents simLoopEvents,
      IGameLoopEvents gameLoopEvents,
      NewInstanceOf<ITransportPathFinder> transportPathFinder,
      TransportsManager transportsManager,
      NewInstanceOf<FloatingPricePopup> pricePopup,
      IEntitiesManager entitiesManager,
      TransportPreviewManager transportPreviewManager,
      TerrainManager terrainManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_gameLoopEvents = gameLoopEvents.CheckNotNull<IGameLoopEvents>();
      this.m_pathFinder = transportPathFinder.Instance;
      this.m_transportsManager = transportsManager;
      this.m_transportPreviewManager = transportPreviewManager;
      this.m_terrainManager = terrainManager;
      this.m_pricePopup = pricePopup.Instance;
      this.m_transportPreviewVisualizer = transportPreviewManager.GeTransportPreviewVisualizer();
      entitiesManager.StaticEntityRemoved.AddNonSaveable<PathFindingTransportPreview>(this, new Action<IStaticEntity>(this.entityRemoved));
      simLoopEvents.Sync.AddNonSaveable<PathFindingTransportPreview>(this, new Action(this.syncUpdate));
      simLoopEvents.UpdateEndForUi.AddNonSaveable<PathFindingTransportPreview>(this, new Action(this.simUpdate));
    }

    public void Activate()
    {
      if (this.m_isActive)
      {
        Log.Warning("TransportPreview is already activated!");
      }
      else
      {
        this.m_isActive = true;
        this.m_gameLoopEvents.RenderUpdate.AddNonSaveable<PathFindingTransportPreview>(this, new Action<GameTime>(this.renderUpdate));
      }
    }

    public void Deactivate()
    {
      if (!this.m_isActive)
      {
        Log.Warning("TransportPreview is already deactivated!");
      }
      else
      {
        this.m_isActive = false;
        this.Clear();
        this.m_gameLoopEvents.RenderUpdate.RemoveNonSaveable<PathFindingTransportPreview>(this, new Action<GameTime>(this.renderUpdate));
      }
    }

    private void entityRemoved(IStaticEntity obj)
    {
      this.m_requestedPreview = new PathFindingTransportPreview.PreviewRequest?();
    }

    private void renderUpdate(GameTime time)
    {
      this.m_pricePopup.UpdatePosition();
      if (!this.m_previewSpec.HasValue)
      {
        this.m_transportPreviewVisualizer.Clear();
      }
      else
      {
        if (this.m_previewIsCurrent)
          return;
        this.m_previewIsCurrent = true;
        this.m_transportPreviewVisualizer.ShowPreview(this.m_previewSpec.Value);
        this.m_pricePopup.SetErrorMessage(this.m_previewSpec.Value.IsValid ? LocStrFormatted.Empty : this.m_previewSpec.Value.ErrorMessage);
        if (this.m_canBuildResult.HasValue)
          this.m_pricePopup.SetBuyPrice(this.m_canBuildResult.Value.NewTransportValue);
        int relativeHeight = this.m_relativeHeight.Value;
        if (this.m_previewSpec.HasValue && this.m_previewSpec.Value.IsValid)
        {
          TransportPreviewSpec transportPreviewSpec = this.m_previewSpec.Value;
          if (transportPreviewSpec.MiniZipAtEnd.IsValid)
            relativeHeight = getRelativeHeight(transportPreviewSpec.MiniZipAtEnd.Position);
          else if (transportPreviewSpec.MiniZipAtStart.IsValid && transportPreviewSpec.OkTraj.IsNone && transportPreviewSpec.OkPillars.IsEmpty)
            relativeHeight = getRelativeHeight(transportPreviewSpec.MiniZipAtStart.Position);
          else if (this.m_requestedPreview.HasValue && transportPreviewSpec.OkTraj.HasValue && transportPreviewSpec.OkTraj.Value.Pivots.IsNotEmpty)
          {
            ImmutableArray<Tile3i> pivots = transportPreviewSpec.OkTraj.Value.Pivots;
            if (this.m_requestedPreview.Value.NewPosition == pivots.First)
              relativeHeight = getRelativeHeight(pivots.First);
            else if (this.m_requestedPreview.Value.NewPosition == pivots.Last)
              relativeHeight = getRelativeHeight(pivots.Last);
          }
        }
        this.m_pricePopup.SetExtraText(this.m_relativeHeight.IsNotNegative ? Tr.TransportHeightTooltip.Format(relativeHeight.ToStringCached()).Value : "");
        this.m_pricePopup.Show();
      }

      int getRelativeHeight(Tile3i tile)
      {
        return tile.Z - this.m_terrainManager.GetHeight(tile.Xy).Value.ToIntFloored();
      }
    }

    public void TogglePricePopup()
    {
      this.m_pricePopup.SetTemporarilyHidden(!this.m_pricePopup.IsTemporarilyHidden);
    }

    public bool ShowStartPreview(
      PathFindingTransportPreview.PreviewRequest request,
      ThicknessTilesI relativeHeight,
      out Direction903d? startDirection,
      out Option<TransportTrajectory> successTrajectory,
      out bool stillWorking)
    {
      if (this.m_requestedPreview.HasValue && request.Equals(this.m_requestedPreview.Value))
      {
        stillWorking = !this.m_requestProcessed;
        startDirection = (Direction903d?) this.m_canBuildResult.ValueOrNull?.RequestStartDirection;
        successTrajectory = (Option<TransportTrajectory>) this.m_canBuildResult.ValueOrNull?.NewTrajectory.ValueOrNull;
        return this.m_requestProcessed && this.m_previewSpec.HasValue && this.m_previewSpec.Value.IsValid;
      }
      this.m_requestedPreview = new PathFindingTransportPreview.PreviewRequest?(request);
      this.m_requestProcessed = false;
      this.m_relativeHeight = relativeHeight;
      startDirection = new Direction903d?();
      successTrajectory = Option<TransportTrajectory>.None;
      stillWorking = true;
      return false;
    }

    public Option<CanBuildTransportResult> ShowContinuationPreview(
      PathFindingTransportPreview.PreviewRequest request,
      ThicknessTilesI relativeHeight,
      out Option<TransportTrajectory> successTrajectory,
      out bool stillWorking,
      out bool shouldFinishBuild)
    {
      if (this.m_requestedPreview.HasValue && request.Equals(this.m_requestedPreview.Value))
      {
        stillWorking = !this.m_requestProcessed;
        successTrajectory = (Option<TransportTrajectory>) this.m_canBuildResult.ValueOrNull?.NewTrajectory.ValueOrNull;
        if (this.m_requestProcessed && this.m_previewSpec.HasValue && this.m_previewSpec.Value.IsValid)
        {
          shouldFinishBuild = this.m_previewSpec.Value.IsReadyToBuild;
          return this.m_canBuildResult;
        }
        shouldFinishBuild = false;
        return Option<CanBuildTransportResult>.None;
      }
      this.m_requestedPreview = new PathFindingTransportPreview.PreviewRequest?(request);
      this.m_requestProcessed = false;
      this.m_relativeHeight = relativeHeight;
      successTrajectory = Option<TransportTrajectory>.None;
      stillWorking = false;
      shouldFinishBuild = false;
      return Option<CanBuildTransportResult>.None;
    }

    private void syncUpdate()
    {
      if (!this.m_requestedPreview.HasValue)
      {
        this.m_processedPreviewSpec = new TransportPreviewSpec?();
        this.m_canBuildResult = Option<CanBuildTransportResult>.None;
        this.m_requestedPreviewOnSimThread = new PathFindingTransportPreview.PreviewRequest?();
      }
      else
      {
        if (this.m_requestProcessed)
          return;
        if (this.m_processedPreviewSpec.HasValue)
        {
          Assert.That<bool>(this.m_newRequest).IsFalse();
          Assert.That<bool>(this.m_requestProcessed).IsFalse();
          this.m_previewSpec = new TransportPreviewSpec?(this.m_processedPreviewSpec.Value);
          this.m_processedPreviewSpec = new TransportPreviewSpec?();
          this.m_previewIsCurrent = false;
          this.m_canBuildResult = this.m_canBuildResultOnSim;
          this.m_requestProcessed = true;
        }
        PathFindingTransportPreview.PreviewRequest other = this.m_requestedPreview.Value;
        if (!this.m_forceProcessRequest && this.m_requestedPreviewOnSimThread.HasValue && this.m_requestedPreviewOnSimThread.Value.Equals(other))
          return;
        this.m_requestedPreviewOnSimThread = new PathFindingTransportPreview.PreviewRequest?(other);
        this.m_newRequest = true;
        this.m_processedPreviewSpec = new TransportPreviewSpec?();
        this.m_canBuildResult = Option<CanBuildTransportResult>.None;
        this.m_requestProcessed = false;
        this.m_forceProcessRequest = false;
      }
    }

    private void simUpdate()
    {
      if (this.m_requestProcessed || !this.m_requestedPreviewOnSimThread.HasValue || this.m_processedPreviewSpec.HasValue)
      {
        if (!this.m_isActive || !this.m_requestedPreviewOnSimThread.HasValue)
          return;
        int iterations = 500;
        this.m_pathFinder.SetUndirected();
        int num = (int) this.m_pathFinder.ContinuePathFinding(ref iterations, out ImmutableArray<Tile3i> _);
      }
      else
      {
        PathFindingTransportPreview.PreviewRequest previewRequest = this.m_requestedPreviewOnSimThread.Value;
        CanBuildTransportResult buildTransportResult;
        this.m_processedPreviewSpec = previewRequest.Pivots.IsEmpty ? new TransportPreviewSpec?(this.processStartRequest(previewRequest, out buildTransportResult)) : this.processContRequest(previewRequest, this.m_newRequest, out buildTransportResult);
        this.m_newRequest = false;
        this.m_canBuildResultOnSim = (Option<CanBuildTransportResult>) (this.m_processedPreviewSpec.HasValue ? buildTransportResult : (CanBuildTransportResult) null);
      }
    }

    private TransportPreviewSpec processStartRequest(
      PathFindingTransportPreview.PreviewRequest startRequest,
      out CanBuildTransportResult canBuildResult)
    {
      Tile3i newPosition = startRequest.NewPosition;
      CanBuildTransportResult result;
      LocStrFormatted error;
      Option<IStaticEntity> blockingEntity;
      bool flag1 = this.m_transportsManager.CanBuildOrJoinTransport(startRequest.Proto, ImmutableArray.Create<Tile3i>(newPosition), Set<Tile2i>.Empty, startRequest.StartDirection, new Direction903d?(), startRequest.DisablePortSnapping, out result, out error, out blockingEntity, startRequest.IgnorePillars);
      canBuildResult = flag1 ? result : (CanBuildTransportResult) null;
      Option<TransportTrajectory> trajectory1 = result.NewTrajectory;
      Option<TransportTrajectory> trajectory2 = Option<TransportTrajectory>.None;
      bool flag2;
      bool flag3;
      if (flag1)
      {
        flag2 = !result.MiniZipJoinResultAtStart.HasValue && !result.ChangeDirectionNearStart.HasValue;
        flag3 = !result.MiniZipJoinResultAtEnd.HasValue && !result.ChangeDirectionNearEnd.HasValue;
      }
      else
      {
        if (trajectory1.HasValue)
        {
          trajectory2 = trajectory1;
          trajectory1 = Option<TransportTrajectory>.None;
        }
        else
        {
          TransportProto proto = startRequest.Proto;
          ImmutableArray<Tile3i> requestPivots = result.RequestPivots;
          ref readonly Direction903d? local1 = ref result.RequestStartDirection;
          RelTile3i? startDirMaybe = local1.HasValue ? new RelTile3i?(local1.GetValueOrDefault().ToTileDirection()) : new RelTile3i?();
          ref readonly Direction903d? local2 = ref result.RequestEndDirection;
          RelTile3i? endDirMaybe = local2.HasValue ? new RelTile3i?(local2.GetValueOrDefault().ToTileDirection()) : new RelTile3i?();
          TransportTrajectory transportTrajectory;
          ref TransportTrajectory local3 = ref transportTrajectory;
          string str;
          ref string local4 = ref str;
          TransportTrajectory.TryCreateFromPivots(proto, requestPivots, startDirMaybe, endDirMaybe, out local3, out local4);
          trajectory2 = (Option<TransportTrajectory>) transportTrajectory;
        }
        flag2 = false;
        flag3 = false;
      }
      Option<TransportTrajectory> option1 = trajectory1;
      bool pillarsFound;
      ImmutableArray<PillarVisualsSpec> pillarsForTraj1 = this.m_transportPreviewManager.GetPillarsForTraj(trajectory1, Set<Tile2i>.Empty, out pillarsFound);
      Option<TransportTrajectory> option2 = trajectory2;
      ImmutableArray<PillarVisualsSpec> pillarsForTraj2 = this.m_transportPreviewManager.GetPillarsForTraj(trajectory2, Set<Tile2i>.Empty, out pillarsFound);
      LocStrFormatted? errorMessage = new LocStrFormatted?(error);
      Option<TransportTrajectory> okTraj = option1;
      ImmutableArray<PillarVisualsSpec> okPillars = pillarsForTraj1;
      MiniZipperAtResult? nullable = result.MiniZipperAtStart;
      MiniZipperAtResult miniZipperAtResult1 = nullable ?? PathFindingTransportPreview.CreateZipperResult(result.MiniZipJoinResultAtStart);
      nullable = result.MiniZipperAtEnd;
      MiniZipperAtResult miniZipperAtResult2 = nullable ?? PathFindingTransportPreview.CreateZipperResult(result.MiniZipJoinResultAtEnd);
      EntityHighlightSpec entityHighlightSpec = blockingEntity.HasValue ? new EntityHighlightSpec((IRenderedEntity) blockingEntity.Value, HighlightColors.ERROR_RED) : new EntityHighlightSpec();
      int num1 = flag2 ? 1 : 0;
      int num2 = flag3 ? 1 : 0;
      Option<TransportTrajectory> errTraj = option2;
      ImmutableArray<PillarVisualsSpec> errPillars = pillarsForTraj2;
      Option<TransportTrajectory> highlightTraj = new Option<TransportTrajectory>();
      MiniZipperAtResult miniZipAtStart = miniZipperAtResult1;
      MiniZipperAtResult miniZipAtEnd = miniZipperAtResult2;
      EntityHighlightSpec highlightEntity1 = entityHighlightSpec;
      return new TransportPreviewSpec(errorMessage, okTraj, okPillars, num1 != 0, num2 != 0, errTraj, errPillars, highlightTraj, miniZipAtStart, miniZipAtEnd, highlightEntity1);
    }

    internal static MiniZipperAtResult CreateZipperResult(CanPlaceMiniZipperAtResult? r)
    {
      return r.HasValue ? new MiniZipperAtResult(r.Value.ZipperProto, r.Value.CutOutResult.CutOutPosition) : new MiniZipperAtResult();
    }

    private TransportPreviewSpec? processContRequest(
      PathFindingTransportPreview.PreviewRequest request,
      bool startNew,
      out CanBuildTransportResult outResult)
    {
      TransportProto proto = request.Proto;
      Tile3i last = request.Pivots.Last;
      Tile3i newPosition = request.NewPosition;
      bool flag1 = false;
      ImmutableArray<Tile3i> outPivots;
      bool pillarsFound1;
      if (last == newPosition)
      {
        outPivots = ImmutableArray.Create<Tile3i>(last);
        flag1 = true;
        if (startNew)
        {
          if (this.m_pathFinder.CurrentTransportProto == proto && this.m_pathFinder.CurrentStart == last && this.m_pathFinder.OriginalGoal.Z == newPosition.Z && this.m_pathFinder.Options == request.PathFinderOptions)
            this.m_pathFinder.ChangeGoal(newPosition);
          else
            this.m_pathFinder.InitPathFinding(proto, last, newPosition, request.PathFinderOptions, request.BannedTiles.AsEnumerable());
          this.m_pfDurationLeft = PathFindingTransportPreview.MAX_PF_DURATION;
        }
      }
      else
      {
        int iterations = 1000;
        if (startNew)
        {
          if (this.m_pathFinder.CurrentTransportProto == proto && this.m_pathFinder.CurrentStart == last && this.m_pathFinder.OriginalGoal.Z == newPosition.Z && this.m_pathFinder.Options == request.PathFinderOptions)
            this.m_pathFinder.ChangeGoal(newPosition);
          else
            this.m_pathFinder.InitPathFinding(proto, last, newPosition, request.PathFinderOptions, request.BannedTiles.AsEnumerable());
          this.m_pfDurationLeft = PathFindingTransportPreview.MAX_PF_DURATION;
          iterations -= 50;
        }
        PathFinderResult pathFinderResult = this.m_pathFinder.ContinuePathFinding(ref iterations, out outPivots);
        this.m_pfDurationLeft -= Duration.OneTick;
        switch (pathFinderResult)
        {
          case PathFinderResult.StillSearching:
            if (this.m_pfDurationLeft.IsNotPositive)
            {
              outResult = (CanBuildTransportResult) null;
              LocStrFormatted? errorMessage = new LocStrFormatted?((LocStrFormatted) Tr.TrAdditionError__PathNotFound);
              Option<TransportTrajectory> existingTrajectory = request.ExistingTrajectory;
              ImmutableArray<PillarVisualsSpec> pillarsForTraj = this.m_transportPreviewManager.GetPillarsForTraj(request.ExistingTrajectory, Set<Tile2i>.Empty, out pillarsFound1);
              Option<TransportTrajectory> okTraj = new Option<TransportTrajectory>();
              ImmutableArray<PillarVisualsSpec> okPillars = new ImmutableArray<PillarVisualsSpec>();
              Option<TransportTrajectory> errTraj = existingTrajectory;
              ImmutableArray<PillarVisualsSpec> errPillars = pillarsForTraj;
              Option<TransportTrajectory> highlightTraj = new Option<TransportTrajectory>();
              MiniZipperAtResult miniZipAtStart = new MiniZipperAtResult();
              MiniZipperAtResult miniZipAtEnd = new MiniZipperAtResult();
              EntityHighlightSpec highlightEntity1 = new EntityHighlightSpec();
              return new TransportPreviewSpec?(new TransportPreviewSpec(errorMessage, okTraj, okPillars, errTraj: errTraj, errPillars: errPillars, highlightTraj: highlightTraj, miniZipAtStart: miniZipAtStart, miniZipAtEnd: miniZipAtEnd, highlightEntity1: highlightEntity1));
            }
            outResult = (CanBuildTransportResult) null;
            return new TransportPreviewSpec?();
          case PathFinderResult.PathFound:
            break;
          default:
            outResult = (CanBuildTransportResult) null;
            LocStrFormatted? errorMessage1 = new LocStrFormatted?((LocStrFormatted) Tr.TrAdditionError__PathNotFound);
            Option<TransportTrajectory> existingTrajectory1 = request.ExistingTrajectory;
            ImmutableArray<PillarVisualsSpec> pillarsForTraj1 = this.m_transportPreviewManager.GetPillarsForTraj(request.ExistingTrajectory, Set<Tile2i>.Empty, out pillarsFound1);
            Option<TransportTrajectory> okTraj1 = new Option<TransportTrajectory>();
            ImmutableArray<PillarVisualsSpec> okPillars1 = new ImmutableArray<PillarVisualsSpec>();
            Option<TransportTrajectory> errTraj1 = existingTrajectory1;
            ImmutableArray<PillarVisualsSpec> errPillars1 = pillarsForTraj1;
            Option<TransportTrajectory> highlightTraj1 = new Option<TransportTrajectory>();
            MiniZipperAtResult miniZipAtStart1 = new MiniZipperAtResult();
            MiniZipperAtResult miniZipAtEnd1 = new MiniZipperAtResult();
            EntityHighlightSpec highlightEntity1_1 = new EntityHighlightSpec();
            return new TransportPreviewSpec?(new TransportPreviewSpec(errorMessage1, okTraj1, okPillars1, errTraj: errTraj1, errPillars: errPillars1, highlightTraj: highlightTraj1, miniZipAtStart: miniZipAtStart1, miniZipAtEnd: miniZipAtEnd1, highlightEntity1: highlightEntity1_1));
        }
      }
      Assert.That<ImmutableArray<Tile3i>>(outPivots).IsNotEmpty<Tile3i>();
      Direction903d? startDirection = request.StartDirection;
      Assert.That<Tile3i>(request.Pivots.Last).IsEqualTo<Tile3i>(outPivots.First);
      ImmutableArray<Tile3i> pivots = request.Pivots.Concat(outPivots, 1, outPivots.Length - 1);
      CanBuildTransportResult result;
      LocStrFormatted error;
      Option<IStaticEntity> blockingEntity;
      bool flag2 = this.m_transportsManager.CanBuildOrJoinTransport(proto, pivots, request.PillarHints.ToReadonlySet(), startDirection, new Direction903d?(), request.DisablePortSnapping, out result, out error, out blockingEntity);
      ImmutableArray<PillarVisualsSpec> immutableArray1 = new ImmutableArray<PillarVisualsSpec>();
      ImmutableArray<PillarVisualsSpec> immutableArray2 = new ImmutableArray<PillarVisualsSpec>();
      Option<TransportTrajectory> option1;
      Option<TransportTrajectory> option2;
      bool a;
      bool b;
      bool pillarsFound2;
      if (flag2)
      {
        outResult = result;
        option1 = result.NewTrajectory;
        option2 = Option<TransportTrajectory>.None;
        if (result.NewTrajectory.HasValue)
          immutableArray1 = this.m_transportPreviewManager.CreatePillarsVisualSpec(result.NewTrajectory.Value, result.SupportedTiles);
        if (!flag1)
          flag1 = result.MiniZipJoinResultAtEnd.HasValue || result.MiniZipperAtEnd.HasValue || result.ChangeDirectionNearEnd.HasValue || result.PortAtEnd.HasValue;
        a = !result.MiniZipJoinResultAtStart.HasValue && !result.ChangeDirectionNearStart.HasValue;
        b = !result.MiniZipJoinResultAtEnd.HasValue && !result.ChangeDirectionNearEnd.HasValue;
        if (result.PivotsWereReversed)
          Swap.Them<bool>(ref a, ref b);
      }
      else
      {
        outResult = (CanBuildTransportResult) null;
        if (result.NewTrajectory.HasValue)
        {
          option1 = request.ExistingTrajectory;
          immutableArray1 = this.m_transportPreviewManager.GetPillarsForTraj(request.ExistingTrajectory, request.PillarHints.ToReadonlySet(), out pillarsFound1);
          option2 = result.NewTrajectory;
          immutableArray2 = this.m_transportPreviewManager.CreatePillarsVisualSpec(result.NewTrajectory.Value, result.SupportedTiles);
        }
        else
        {
          option1 = Option<TransportTrajectory>.None;
          option2 = request.ExistingTrajectory;
          immutableArray2 = this.m_transportPreviewManager.GetPillarsForTraj(request.ExistingTrajectory, request.PillarHints.ToReadonlySet(), out pillarsFound2);
        }
        flag1 = false;
        a = false;
        b = false;
      }
      Option<TransportTrajectory> option3 = option1;
      ImmutableArray<PillarVisualsSpec> immutableArray3 = immutableArray1;
      Option<TransportTrajectory> existingTrajectory2 = request.ExistingTrajectory;
      Option<TransportTrajectory> option4 = option2;
      ImmutableArray<PillarVisualsSpec> immutableArray4 = immutableArray2;
      LocStrFormatted? errorMessage2 = new LocStrFormatted?(error);
      Option<TransportTrajectory> okTraj2 = option3;
      ImmutableArray<PillarVisualsSpec> okPillars2 = immutableArray3;
      MiniZipperAtResult? nullable = result.MiniZipperAtStart;
      MiniZipperAtResult miniZipperAtResult1 = nullable ?? PathFindingTransportPreview.CreateZipperResult(result.MiniZipJoinResultAtStart);
      nullable = result.MiniZipperAtEnd;
      MiniZipperAtResult miniZipperAtResult2 = nullable ?? PathFindingTransportPreview.CreateZipperResult(result.MiniZipJoinResultAtEnd);
      EntityHighlightSpec entityHighlightSpec = blockingEntity.HasValue ? new EntityHighlightSpec((IRenderedEntity) blockingEntity.Value, HighlightColors.ERROR_RED) : new EntityHighlightSpec();
      pillarsFound2 = flag1;
      int num1 = a ? 1 : 0;
      int num2 = b ? 1 : 0;
      Option<TransportTrajectory> errTraj2 = option4;
      ImmutableArray<PillarVisualsSpec> errPillars2 = immutableArray4;
      Option<TransportTrajectory> highlightTraj2 = existingTrajectory2;
      MiniZipperAtResult miniZipAtStart2 = miniZipperAtResult1;
      MiniZipperAtResult miniZipAtEnd2 = miniZipperAtResult2;
      EntityHighlightSpec highlightEntity1_2 = entityHighlightSpec;
      int num3 = pillarsFound2 ? 1 : 0;
      return new TransportPreviewSpec?(new TransportPreviewSpec(errorMessage2, okTraj2, okPillars2, num1 != 0, num2 != 0, errTraj2, errPillars2, highlightTraj2, miniZipAtStart2, miniZipAtEnd2, highlightEntity1_2, num3 != 0));
    }

    public void Clear()
    {
      this.m_processedPreviewSpec = new TransportPreviewSpec?();
      this.m_canBuildResult = Option<CanBuildTransportResult>.None;
      this.m_previewSpec = new TransportPreviewSpec?();
      this.m_requestedPreview = new PathFindingTransportPreview.PreviewRequest?();
      this.m_transportPreviewVisualizer.Clear();
      this.m_pricePopup.Hide();
    }

    static PathFindingTransportPreview()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      PathFindingTransportPreview.MAX_PF_DURATION = 10.Ticks();
    }

    public readonly struct PreviewRequest : IEquatable<PathFindingTransportPreview.PreviewRequest>
    {
      public readonly TransportProto Proto;
      public readonly Tile3i NewPosition;
      public readonly ImmutableArray<Tile3i> Pivots;
      public readonly ImmutableArray<Tile2i> PillarHints;
      public readonly Option<TransportTrajectory> ExistingTrajectory;
      public readonly ImmutableArray<Tile3i> BannedTiles;
      public readonly Direction903d? StartDirection;
      public readonly TransportPathFinderOptions PathFinderOptions;
      public readonly bool DisablePortSnapping;
      public readonly bool IgnorePillars;

      private PreviewRequest(
        TransportProto proto,
        Tile3i newPosition,
        ImmutableArray<Tile3i> pivots,
        ImmutableArray<Tile2i> pillarHints,
        Option<TransportTrajectory> existingTrajectory,
        TransportPathFinderOptions pathFinderOptions,
        ImmutableArray<Tile3i> bannedTiles,
        Direction903d? startDirection,
        bool disablePortSnapping,
        bool ignorePillars = false)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.Proto = proto;
        this.NewPosition = newPosition;
        this.Pivots = pivots;
        this.PillarHints = pillarHints;
        this.ExistingTrajectory = existingTrajectory;
        this.PathFinderOptions = pathFinderOptions;
        this.BannedTiles = bannedTiles;
        this.StartDirection = startDirection;
        this.DisablePortSnapping = disablePortSnapping;
        this.IgnorePillars = ignorePillars;
      }

      public static PathFindingTransportPreview.PreviewRequest CreateStartRequest(
        TransportProto proto,
        Tile3i position,
        TransportPathFinderOptions pathFinderOptions,
        Direction903d? startDirection,
        bool disablePortSnapping)
      {
        return new PathFindingTransportPreview.PreviewRequest(proto, position, ImmutableArray<Tile3i>.Empty, ImmutableArray<Tile2i>.Empty, Option<TransportTrajectory>.None, pathFinderOptions, ImmutableArray<Tile3i>.Empty, startDirection, disablePortSnapping, true);
      }

      public static PathFindingTransportPreview.PreviewRequest CreateContRequest(
        TransportProto proto,
        Tile3i newPosition,
        ImmutableArray<Tile3i> pivots,
        ImmutableArray<Tile2i> pillarHints,
        Option<TransportTrajectory> existingTrajectory,
        TransportPathFinderOptions pathFinderOptions,
        ImmutableArray<Tile3i> bannedTiles,
        Direction903d? startDirection,
        bool disablePortSnapping)
      {
        Assert.That<ImmutableArray<Tile3i>>(pivots).IsNotEmpty<Tile3i>();
        return new PathFindingTransportPreview.PreviewRequest(proto, newPosition, pivots, pillarHints, existingTrajectory, pathFinderOptions, bannedTiles, startDirection, disablePortSnapping);
      }

      public bool Equals(PathFindingTransportPreview.PreviewRequest other)
      {
        return (Proto) this.Proto == (Proto) other.Proto && this.NewPosition == other.NewPosition && this.BannedTiles.Equals(other.BannedTiles) && Nullable.Equals<Direction903d>(this.StartDirection, other.StartDirection) && this.PathFinderOptions.Equals(other.PathFinderOptions);
      }

      public override bool Equals(object obj) => throw new InvalidOperationException();

      public override int GetHashCode() => throw new InvalidOperationException();
    }
  }
}
