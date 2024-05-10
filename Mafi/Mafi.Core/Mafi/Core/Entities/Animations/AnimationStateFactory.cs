// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Animations.AnimationStateFactory
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;

#nullable disable
namespace Mafi.Core.Entities.Animations
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class AnimationStateFactory : IAnimationStateFactory
  {
    public AnimationStatesProvider CreateProviderFor(IAnimatedEntity entity)
    {
      return new AnimationStatesProvider(entity, (IAnimationStateFactory) this);
    }

    public ImmutableArray<IAnimationStateImpl> Create(ImmutableArray<AnimationParams> animParams)
    {
      Lyst<IAnimationStateImpl> lyst = new Lyst<IAnimationStateImpl>();
      foreach (AnimationParams animParam in animParams)
      {
        Option<IAnimationStateImpl> option = this.Create(animParam);
        if (option.HasValue)
          lyst.Add(option.Value);
      }
      return lyst.ToImmutableArray();
    }

    public Option<IAnimationStateImpl> Create(AnimationParams animationParams)
    {
      switch (animationParams)
      {
        case LoopAnimationParams animationParams1:
          return (Option<IAnimationStateImpl>) (IAnimationStateImpl) new LoopAnimationState(animationParams1);
        case AnimationWithPauseParams animationParams2:
          return (Option<IAnimationStateImpl>) (IAnimationStateImpl) new AnimationWithPauseState(animationParams2);
        case RepeatableAnimationParams animationParams3:
          return (Option<IAnimationStateImpl>) (IAnimationStateImpl) new RepeatAnimationState(animationParams3);
        default:
          Log.Error("Unknown animation params " + animationParams.GetType().Name);
          return Option<IAnimationStateImpl>.None;
      }
    }

    public AnimationStateFactory()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
