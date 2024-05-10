// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Animations.AnimationWithPauseParams
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.Entities.Animations
{
  /// <summary>
  /// Animation that will be paused at the specified time for the specified duration. If the pause duration is not
  /// specified the duration will be calculated to match duration of currently running recipe.
  /// </summary>
  public class AnimationWithPauseParams : AnimationParams
  {
    public readonly Duration TotalDuration;
    public readonly AnimationWithPauseParams.Mode FillMode;
    /// <summary>
    /// Offset at which the animation should be the paused. Applied only iff <see cref="F:Mafi.Core.Entities.Animations.AnimationWithPauseParams.PauseForDuration" /> is positive.
    /// </summary>
    public readonly Duration PauseAt;
    /// <summary>
    /// Duration for which should be the animation be paused at time defined in <see cref="F:Mafi.Core.Entities.Animations.AnimationWithPauseParams.PauseAt" />.
    /// </summary>
    public readonly Duration PauseForDuration;
    public readonly Percent BaseSpeed;

    public AnimationWithPauseParams(
      Duration totalDuration,
      AnimationWithPauseParams.Mode mode,
      Duration pauseAt,
      Duration pauseDuration = default (Duration),
      Percent? baseSpeed = null,
      string stateName = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(stateName);
      Assert.That<bool>(mode == AnimationWithPauseParams.Mode.ExtendPauseToFit).IsEqualTo<bool>(pauseDuration.IsZero, "Invalid pause duration");
      this.TotalDuration = totalDuration;
      this.FillMode = mode;
      this.PauseAt = pauseAt.CheckNotNegative();
      this.PauseForDuration = pauseDuration.CheckNotNegative();
      this.BaseSpeed = baseSpeed ?? Percent.Hundred;
    }

    public enum Mode
    {
      ExtendPauseToFit,
      ScaleAnimationSpeedToFit,
    }
  }
}
