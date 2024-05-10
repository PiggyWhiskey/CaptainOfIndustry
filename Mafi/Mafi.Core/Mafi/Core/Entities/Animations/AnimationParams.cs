// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Animations.AnimationParams
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.Entities.Animations
{
  /// <summary>
  /// Parameters for animation. If the entity has AnimationController, its animation will be played based on these
  /// parameters.
  /// </summary>
  public abstract class AnimationParams
  {
    public readonly string AnimationStateName;

    public AnimationParams(string stateName = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.AnimationStateName = "Main";
      // ISSUE: explicit constructor call
      base.\u002Ector();
      if (stateName == null)
        return;
      this.AnimationStateName = stateName;
    }

    public static RepeatableAnimationParams PlayOnce(
      Duration totalDuration,
      bool changeSpeedToFit = false,
      Percent? customSpeed = null,
      string stateName = null)
    {
      Duration totalDuration1 = totalDuration;
      int? repeatCount = new int?(1);
      int num = changeSpeedToFit ? 1 : 0;
      Percent? nullable = customSpeed;
      string str = stateName;
      Duration? delayedStartAt = new Duration?();
      Percent? customSpeed1 = nullable;
      string stateName1 = str;
      return new RepeatableAnimationParams(totalDuration1, repeatCount, num != 0, delayedStartAt, customSpeed1, stateName1);
    }

    public static RepeatableAnimationParams RepeatTimes(
      Duration totalDuration,
      int times,
      bool changeSpeedToFit = false,
      string stateName = null)
    {
      Duration totalDuration1 = totalDuration;
      int? repeatCount = new int?(times);
      int num = changeSpeedToFit ? 1 : 0;
      string str = stateName;
      Duration? delayedStartAt = new Duration?();
      Percent? customSpeed = new Percent?();
      string stateName1 = str;
      return new RepeatableAnimationParams(totalDuration1, repeatCount, num != 0, delayedStartAt, customSpeed, stateName1);
    }

    public static RepeatableAnimationParams RepeatAutoTimes(
      Duration totalDuration,
      Percent? customSpeed = null,
      Duration? delayedStartAt = null,
      string stateName = null)
    {
      Duration totalDuration1 = totalDuration;
      int? repeatCount = new int?();
      Percent? nullable = customSpeed;
      Duration? delayedStartAt1 = delayedStartAt;
      Percent? customSpeed1 = nullable;
      string stateName1 = stateName;
      return new RepeatableAnimationParams(totalDuration1, repeatCount, false, delayedStartAt1, customSpeed1, stateName1);
    }

    public static AnimationWithPauseParams PlayOnceAndPauseAt(
      Duration totalDuration,
      AnimationWithPauseParams.Mode fillMode,
      Duration pauseAt,
      Duration pauseDuration = default (Duration),
      Percent? baseSpeed = null,
      string stateName = null)
    {
      return new AnimationWithPauseParams(totalDuration, fillMode, pauseAt, pauseDuration, baseSpeed, stateName);
    }

    public static LoopAnimationParams Loop(
      Percent? speed = null,
      bool playBackwardsWhenFlipped = false,
      string stateName = null)
    {
      return new LoopAnimationParams(speed ?? Percent.Hundred, playBackwardsWhenFlipped, stateName);
    }
  }
}
