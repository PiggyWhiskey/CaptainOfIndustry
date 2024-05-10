﻿// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Transports.TransportFlowIndicatorPose
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.Factory.Transports
{
  public readonly struct TransportFlowIndicatorPose
  {
    public readonly Tile3f Position;
    public readonly TransportWaypointRotation Rotation;
    public readonly Percent PercentOfSection;
    public readonly int SegmentIndex;

    public TransportFlowIndicatorPose(
      Tile3f position,
      TransportWaypointRotation rotation,
      Percent percentOfSection,
      int segmentIndex)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Position = position;
      this.Rotation = rotation;
      this.PercentOfSection = percentOfSection;
      this.SegmentIndex = segmentIndex;
    }
  }
}
