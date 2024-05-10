// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.Static.LayoutEntityAnimatedCollapseInstanceData
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Utils;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities.Static
{
  [ExpectedStructSize(44)]
  internal readonly struct LayoutEntityAnimatedCollapseInstanceData
  {
    public readonly Vector3 Position;
    public readonly uint Data;
    public readonly int TransitionStartTime;
    public readonly int TransitionEndTime;
    public readonly int TransitionHeightTiles;
    public readonly float AnimationStartTime;
    public readonly float AnimationPausedAtTime;
    public readonly float InvAnimationLength;
    public readonly float EmissionIntensity;

    public LayoutEntityAnimatedCollapseInstanceData(
      Vector3 position,
      Rotation90 rotation,
      bool isReflected,
      int transitionStartTime,
      int transitionEndTime,
      int transitionHeightTiles,
      float animationStartTime,
      float animationPausedAtTime,
      float invAnimationLength,
      int simStepsSinceLoad)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.Position = position;
      this.AnimationStartTime = animationStartTime;
      this.AnimationPausedAtTime = (double) animationPausedAtTime >= 0.0 ? animationPausedAtTime : (float) simStepsSinceLoad / 10f;
      this.InvAnimationLength = invAnimationLength;
      this.Data = (uint) (-16 | rotation.AngleIndex | (isReflected ? 4 : 0));
      this.TransitionStartTime = transitionStartTime;
      this.TransitionEndTime = transitionEndTime;
      this.TransitionHeightTiles = transitionHeightTiles;
      this.EmissionIntensity = 0.0f;
    }
  }
}
