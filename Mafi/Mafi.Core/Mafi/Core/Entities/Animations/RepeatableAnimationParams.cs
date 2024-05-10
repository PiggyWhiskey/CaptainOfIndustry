// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Animations.RepeatableAnimationParams
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.Entities.Animations
{
  /// <summary>
  /// Animations that either repeats number of times to fill recipe duration or computes number of repeats
  /// based on animation duration.
  /// </summary>
  public class RepeatableAnimationParams : AnimationParams
  {
    public readonly Duration TotalDuration;
    public readonly int? RepeatCount;
    public readonly bool ChangeSpeedToFit;
    public readonly Percent? CustomSpeed;
    public readonly Duration DelayedStartAt;

    public RepeatableAnimationParams(
      Duration totalDuration,
      int? repeatCount,
      bool changeSpeedToFit,
      Duration? delayedStartAt = null,
      Percent? customSpeed = null,
      string stateName = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(stateName);
      this.CustomSpeed = customSpeed;
      this.TotalDuration = totalDuration.CheckPositive();
      this.RepeatCount = repeatCount;
      this.ChangeSpeedToFit = changeSpeedToFit;
      this.DelayedStartAt = delayedStartAt ?? Duration.Zero;
    }
  }
}
