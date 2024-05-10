// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Factory.TransportPreviewSpec
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Factory.Transports;
using Mafi.Localization;
using Mafi.Unity.Entities;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Factory
{
  public readonly struct TransportPreviewSpec
  {
    public readonly LocStrFormatted ErrorMessage;
    public readonly Option<TransportTrajectory> OkTraj;
    public readonly ImmutableArray<PillarVisualsSpec> OkPillars;
    public readonly bool ShowStartPort;
    public readonly bool ShowEndPort;
    public readonly Option<TransportTrajectory> ErrTraj;
    public readonly ImmutableArray<PillarVisualsSpec> ErrPillars;
    public readonly Option<TransportTrajectory> HighlightTraj;
    public readonly MiniZipperAtResult MiniZipAtStart;
    public readonly MiniZipperAtResult MiniZipAtEnd;
    public readonly EntityHighlightSpec HighlightEntity1;
    public readonly bool IsReadyToBuild;

    public bool IsValid => string.IsNullOrEmpty(this.ErrorMessage.Value);

    public TransportPreviewSpec(
      LocStrFormatted? errorMessage = null,
      Option<TransportTrajectory> okTraj = default (Option<TransportTrajectory>),
      ImmutableArray<PillarVisualsSpec> okPillars = default (ImmutableArray<PillarVisualsSpec>),
      bool showStartPort = false,
      bool showEndPort = false,
      Option<TransportTrajectory> errTraj = default (Option<TransportTrajectory>),
      ImmutableArray<PillarVisualsSpec> errPillars = default (ImmutableArray<PillarVisualsSpec>),
      Option<TransportTrajectory> highlightTraj = default (Option<TransportTrajectory>),
      MiniZipperAtResult miniZipAtStart = default (MiniZipperAtResult),
      MiniZipperAtResult miniZipAtEnd = default (MiniZipperAtResult),
      EntityHighlightSpec highlightEntity1 = default (EntityHighlightSpec),
      bool isReadyToBuild = false)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.ErrorMessage = errorMessage ?? LocStrFormatted.Empty;
      this.OkTraj = okTraj;
      this.OkPillars = okPillars;
      this.ShowStartPort = showStartPort;
      this.ShowEndPort = showEndPort;
      this.ErrTraj = errTraj;
      this.ErrPillars = errPillars;
      this.HighlightTraj = highlightTraj;
      this.MiniZipAtStart = miniZipAtStart;
      this.MiniZipAtEnd = miniZipAtEnd;
      this.HighlightEntity1 = highlightEntity1;
      this.IsReadyToBuild = isReadyToBuild;
    }
  }
}
