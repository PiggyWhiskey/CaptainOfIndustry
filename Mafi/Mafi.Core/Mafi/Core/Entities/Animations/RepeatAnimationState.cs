// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Animations.RepeatAnimationState
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.Entities.Animations
{
  public class RepeatAnimationState : IAnimationStateImpl, IAnimationState
  {
    private readonly RepeatableAnimationParams m_params;
    private Duration m_currentProcessDuration;
    private Duration m_currDuration;
    private Duration m_repeatDuration;
    private Percent m_speed;
    private int m_repeatCount;

    public string AnimationStateName => this.m_params.AnimationStateName;

    public RepeatAnimationState(RepeatableAnimationParams animationParams)
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
      this.m_speed = Percent.Hundred;
      if (this.m_params.TotalDuration.IsNotPositive)
      {
        Log.Error(string.Format("TotalDuration is not positive, entity: {0}", (object) entity));
        this.m_repeatCount = 1;
        this.m_repeatDuration = 1.Ticks();
      }
      else
      {
        if (this.m_params.RepeatCount.HasValue)
        {
          this.m_repeatCount = this.m_params.RepeatCount.Value;
          if (this.m_repeatCount <= 0)
          {
            Log.Error(string.Format("RepeatCount is not positive, entity: {0}", (object) entity));
            this.m_repeatCount = 1;
          }
          if (this.m_params.ChangeSpeedToFit)
          {
            this.m_speed = Percent.FromRatio(this.m_repeatCount * this.m_params.TotalDuration.Ticks, this.m_currentProcessDuration.Ticks);
            this.m_repeatDuration = this.m_currentProcessDuration / this.m_repeatCount;
          }
          else if (this.m_params.CustomSpeed.HasValue)
          {
            this.m_speed = this.m_params.CustomSpeed.Value;
            this.m_repeatDuration = this.m_params.TotalDuration.ScaleByAnimationSpeed(this.m_speed);
          }
          else
            this.m_repeatDuration = this.m_params.TotalDuration;
        }
        else
        {
          Duration duration = currentProcessDuration - this.m_params.DelayedStartAt;
          if (this.m_params.CustomSpeed.HasValue)
          {
            this.m_speed = this.m_params.CustomSpeed.Value;
            if (this.m_speed.IsNotPositive)
            {
              Log.Error(string.Format("Animation CustomSpeed is not positive, entity: {0} ", (object) entity));
              this.m_speed = Percent.Hundred;
            }
            this.m_repeatDuration = this.m_params.TotalDuration.ScaleByAnimationSpeed(this.m_speed);
            if (this.m_repeatDuration.IsNotPositive)
            {
              Log.Error(string.Format("Computed RepeatDuration is not positive, entity: {0}", (object) entity));
              this.m_repeatDuration = Duration.OneTick;
            }
            this.m_repeatCount = (duration.Seconds / this.m_repeatDuration.Seconds).ToIntRounded();
          }
          else
          {
            this.m_repeatCount = (duration.Seconds / this.m_params.TotalDuration.Seconds).ToIntRounded();
            if (this.m_repeatCount <= 0)
            {
              Log.Error(string.Format("RepeatCount is not positive, entity:  {0}", (object) entity));
              this.m_repeatCount = 1;
            }
            this.m_repeatDuration = duration / this.m_repeatCount;
          }
        }
        if (this.m_repeatDuration.IsNotPositive)
        {
          Log.Error(string.Format("Final RepeatDuration is not positive, entity:  {0}", (object) entity));
          this.m_repeatDuration = Duration.OneTick;
        }
        if (this.m_repeatCount > 0)
          return;
        Log.Error(string.Format("Final RepeatCount is not positive, entity:  {0}", (object) entity));
        this.m_repeatCount = 1;
      }
    }

    public void Pause()
    {
    }

    public void Step(Percent speed, Percent progress)
    {
      this.m_currDuration = this.m_currentProcessDuration.ScaledBy(progress);
    }

    public AnimationState GetState()
    {
      if (this.m_repeatDuration == Duration.Zero)
        return AnimationState.CreateAsTimeMs((Fix32) 0);
      if (this.m_params.DelayedStartAt > this.m_currDuration)
        return AnimationState.CreateAsTimeMs((Fix32) 0);
      Duration duration = this.m_currDuration - this.m_params.DelayedStartAt;
      Percent p = Percent.FromRatio(duration.Ticks % this.m_repeatDuration.Ticks, this.m_repeatDuration.Ticks);
      if (duration.Ticks / this.m_repeatDuration.Ticks >= this.m_repeatCount)
        return AnimationState.CreateAsTimeMs(Fix32.Zero);
      Fix64 fix64 = this.m_params.TotalDuration.Millis;
      fix64 = fix64.ScaledBy(p);
      return AnimationState.CreateAsTimeMs(fix64.ToFix32());
    }
  }
}
