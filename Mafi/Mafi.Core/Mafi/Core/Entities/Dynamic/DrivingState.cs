// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Dynamic.DrivingState
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi.Core.Entities.Dynamic
{
  public enum DrivingState
  {
    /// <summary>Vehicle is stopped and idle.</summary>
    Stopped,
    /// <summary>Vehicle completed its task and is just braking down.</summary>
    Stopping,
    /// <summary>
    /// Stops the vehicle and then makes it go forwards again.
    /// </summary>
    StopAndContinueForwards,
    /// <summary>Vehicle is driving forwards to reach its goal.</summary>
    DrivingForwards,
    /// <summary>
    /// Vehicle is backing up, most likely to turn around to be able to reach its goal.
    /// </summary>
    DrivingBackwards,
    /// <summary>Vehicle is turning in place towards specified goal.</summary>
    TurningInPlace,
    /// <summary>Driving is paused.</summary>
    Paused,
    StopAndContinueBackwards,
    DrivingForwardsOnRoad,
  }
}
