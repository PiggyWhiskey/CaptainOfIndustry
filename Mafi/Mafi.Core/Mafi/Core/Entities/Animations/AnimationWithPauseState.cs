// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Animations.AnimationWithPauseState
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Entities.Animations
{
  public class AnimationWithPauseState : IAnimationStateImpl, IAnimationState
  {
    private readonly AnimationWithPauseParams m_params;
    private Duration m_currDuration;
    private Duration m_pauseAtDuration;
    private Duration m_resumeAtDuration;
    private Duration m_currentProcessDuration;
    private Percent m_speed;

    public string AnimationStateName => this.m_params.AnimationStateName;

    public AnimationWithPauseState(AnimationWithPauseParams animationParams)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_params = animationParams;
    }

    public void Initialize(IEntity entity)
    {
    }

    public void Start(IEntity entity, Duration currentProcessDuration, int animationIndex)
    {
      this.m_currentProcessDuration = currentProcessDuration.CheckPositive();
      this.m_currDuration = Duration.Zero;
      this.m_speed = this.m_params.BaseSpeed;
      if (this.m_params.FillMode == AnimationWithPauseParams.Mode.ExtendPauseToFit)
      {
        this.m_pauseAtDuration = this.m_params.PauseAt.ScaledBy(Percent.Hundred / this.m_speed);
        Duration duration1 = this.m_params.TotalDuration.ScaledBy(Percent.Hundred / this.m_speed);
        Duration duration2 = this.m_currentProcessDuration - duration1;
        if (duration2.IsNotPositive)
        {
          Log.Warning(string.Format("Animation pause of {0} is {1} ", (object) entity.Prototype.Id, (object) duration2) + "(less than zero) - no pause was needed. " + string.Format("Curr dur: {0}, ", (object) this.m_currentProcessDuration) + string.Format("total dur: {0} ({1}), ", (object) duration1, (object) this.m_params.TotalDuration) + string.Format("pause at: {0}, speed {1}", (object) this.m_pauseAtDuration, (object) this.m_speed));
          duration2 = Duration.Zero;
          if (entity is IAnimatedEntity animatedEntity && animationIndex < animatedEntity.AnimationParams.Length && animatedEntity.AnimationParams[animationIndex] is AnimationWithPauseParams animationParam && this.m_params != animationParam)
          {
            ReflectionUtils.SetField<AnimationWithPauseState>(this, "m_params", (object) animationParam);
            Log.Warning("Animation params of " + entity.GetTitle() + " were reset.");
          }
        }
        this.m_resumeAtDuration = this.m_pauseAtDuration + duration2;
      }
      else if (this.m_params.FillMode == AnimationWithPauseParams.Mode.ScaleAnimationSpeedToFit)
      {
        this.m_speed = Percent.FromRatio(this.m_params.TotalDuration.Ticks, this.m_currentProcessDuration.Ticks - this.m_params.PauseForDuration.Ticks);
        this.m_pauseAtDuration = this.m_params.PauseAt.ScaledBy(Percent.Hundred / this.m_speed);
        this.m_resumeAtDuration = this.m_pauseAtDuration + this.m_params.PauseForDuration;
      }
      else
      {
        Log.Error(string.Format("Unknown scaling mode {0}", (object) this.m_params.FillMode));
        this.m_pauseAtDuration = this.m_params.PauseAt;
        this.m_resumeAtDuration = this.m_params.PauseAt + this.m_params.PauseForDuration;
      }
    }

    public void Pause()
    {
    }

    public void Step(Percent speed, Percent progress)
    {
      Assert.That<Duration>(this.m_currentProcessDuration).IsPositive("Animation not started?");
      this.m_currDuration = this.m_currentProcessDuration.ScaledBy(progress);
    }

    public AnimationState GetState()
    {
      if (this.m_currDuration < this.m_pauseAtDuration)
        return AnimationState.CreateAsTimeMs(this.m_speed.Apply(this.m_currDuration.Millis.ToFix32()));
      return this.m_currDuration < this.m_resumeAtDuration ? AnimationState.CreateAsTimeMs(this.m_speed.Apply(this.m_pauseAtDuration.Millis.ToFix32())) : AnimationState.CreateAsTimeMs((this.m_currDuration - this.m_resumeAtDuration + this.m_pauseAtDuration).ScaledBy(this.m_speed).Millis.ToFix32());
    }
  }
}
