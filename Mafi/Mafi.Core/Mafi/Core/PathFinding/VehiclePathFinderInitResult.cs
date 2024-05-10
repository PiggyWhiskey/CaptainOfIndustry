// Decompiled with JetBrains decompiler
// Type: Mafi.Core.PathFinding.VehiclePathFinderInitResult
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi.Core.PathFinding
{
  public enum VehiclePathFinderInitResult
  {
    /// <summary>No result, pathfinding not started.</summary>
    Unknown,
    /// <summary>Goal is already reached, no PF was needed.</summary>
    GoalAlreadyReached,
    /// <summary>Trivial path exists. No further PF needed.</summary>
    PathFound,
    /// <summary>PF initialized and ready for processing.</summary>
    ReadyForPf,
    /// <summary>No start tiles returned by the task.</summary>
    NoStarts,
    /// <summary>All returned starts are invalid.</summary>
    AllStartsInvalid,
    /// <summary>No goals returned by the task.</summary>
    NoGoals,
    /// <summary>All returned goals are invalid.</summary>
    AllGoalsInvalid,
  }
}
