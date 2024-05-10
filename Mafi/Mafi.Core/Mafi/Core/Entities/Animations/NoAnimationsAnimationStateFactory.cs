// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Animations.NoAnimationsAnimationStateFactory
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;

#nullable disable
namespace Mafi.Core.Entities.Animations
{
  public class NoAnimationsAnimationStateFactory : IAnimationStateFactory
  {
    public AnimationStatesProvider CreateProviderFor(IAnimatedEntity entity)
    {
      return new AnimationStatesProvider(entity, (IAnimationStateFactory) this);
    }

    public ImmutableArray<IAnimationStateImpl> Create(ImmutableArray<AnimationParams> animParams)
    {
      return ImmutableArray<IAnimationStateImpl>.Empty;
    }

    public NoAnimationsAnimationStateFactory()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
