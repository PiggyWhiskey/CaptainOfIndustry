// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Animations.LoopAnimationParams
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.Entities.Animations
{
  /// <summary>
  /// Animation that will be simply looped and can be paused anytime.
  /// </summary>
  public class LoopAnimationParams : AnimationParams
  {
    public static readonly LoopAnimationParams FullSpeed;
    public readonly Percent Speed;
    public readonly bool PlayBackwardsWhenFlipped;

    public LoopAnimationParams(Percent speed, bool playBackwardsWhenFlipped = false, string stateName = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(stateName);
      this.Speed = speed;
      this.PlayBackwardsWhenFlipped = playBackwardsWhenFlipped;
    }

    static LoopAnimationParams()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      LoopAnimationParams.FullSpeed = new LoopAnimationParams(Percent.Hundred);
    }
  }
}
