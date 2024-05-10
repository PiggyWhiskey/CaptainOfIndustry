// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Factory.Transports.TransportRenderingSection
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Factory.Transports
{
  public readonly struct TransportRenderingSection
  {
    public readonly short StaticMeshIdx;
    public readonly short MovingMeshIdx;
    public readonly Vector3 Position;
    public readonly float Pitch;
    public readonly float Yaw;
    public readonly float TexOffsetY;
    public readonly float StraightScale;
    public readonly float StartUv;
    public readonly float EndUv;

    public TransportRenderingSection(
      short staticMeshIdx,
      short movingMeshIdx,
      Vector3 position,
      float pitch,
      float yaw,
      float texOffsetY,
      float straightScale,
      float startUv,
      float endUv)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.StaticMeshIdx = staticMeshIdx;
      this.MovingMeshIdx = movingMeshIdx;
      this.Position = position;
      this.Pitch = pitch;
      this.Yaw = yaw;
      this.TexOffsetY = texOffsetY;
      this.StraightScale = straightScale;
      this.StartUv = startUv;
      this.EndUv = endUv;
    }
  }
}
