// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.AnimationState
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.Entities
{
  public struct AnimationState
  {
    public readonly bool UseSpeed;
    public readonly Fix32 TimeMs;
    public readonly Percent Speed;

    private AnimationState(bool useSpeed, Fix32 timeMs, Percent speed)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.UseSpeed = useSpeed;
      this.TimeMs = timeMs;
      this.Speed = speed;
    }

    public static AnimationState CreateAsTimeMs(Fix32 timeMs)
    {
      return new AnimationState(false, timeMs, Percent.Zero);
    }

    public static AnimationState CreateAsSpeed(Percent speed)
    {
      return new AnimationState(true, Fix32.Zero, speed);
    }
  }
}
