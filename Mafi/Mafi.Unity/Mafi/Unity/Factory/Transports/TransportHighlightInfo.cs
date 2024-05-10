﻿// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Factory.Transports.TransportHighlightInfo
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Factory.Transports
{
  public readonly struct TransportHighlightInfo
  {
    public readonly short StaticMeshIdx;
    public readonly short MovingMeshIdx;
    public readonly bool IsFlowIndicator;
    public readonly uint InstanceId;

    public TransportHighlightInfo(
      short staticMeshIdx,
      short movingMeshIdx,
      bool isFlowIndicator,
      uint instanceId)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.StaticMeshIdx = staticMeshIdx;
      this.MovingMeshIdx = movingMeshIdx;
      this.IsFlowIndicator = isFlowIndicator;
      this.InstanceId = instanceId;
    }
  }
}
