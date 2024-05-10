// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Animations.LoopAnimationState
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Static;

#nullable disable
namespace Mafi.Core.Entities.Animations
{
  public class LoopAnimationState : IAnimationStateImpl, IAnimationState
  {
    private readonly Percent m_baseSpeed;
    private readonly bool m_playBackwardsWhenFlipped;
    private Percent m_speed;
    private int m_speedMult;

    public string AnimationStateName { get; private set; }

    public LoopAnimationState(LoopAnimationParams animationParams)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.AnimationStateName = animationParams.AnimationStateName;
      this.m_baseSpeed = animationParams.Speed;
      this.m_playBackwardsWhenFlipped = animationParams.PlayBackwardsWhenFlipped;
    }

    public void Initialize(IEntity entity)
    {
      this.m_speedMult = !this.m_playBackwardsWhenFlipped || !(entity is ILayoutEntity layoutEntity) || !layoutEntity.Transform.IsReflected ? 1 : -1;
    }

    public void Start(IEntity entity, Duration currentProcessDuration, int animationIndex)
    {
      this.m_speed = Percent.Zero;
    }

    public void Pause() => this.m_speed = Percent.Zero;

    public void Step(Percent speed, Percent progress)
    {
      this.m_speed = this.m_baseSpeed * speed * this.m_speedMult;
    }

    public AnimationState GetState() => AnimationState.CreateAsSpeed(this.m_speed);
  }
}
