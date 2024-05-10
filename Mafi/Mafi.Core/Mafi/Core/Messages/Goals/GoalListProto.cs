// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.Goals.GoalListProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Localization;

#nullable disable
namespace Mafi.Core.Messages.Goals
{
  public class GoalListProto : Mafi.Core.Prototypes.Proto
  {
    public readonly ImmutableArray<GoalProto> Goals;
    public readonly IGoalListTriggerData TriggerData;
    public readonly ImmutableArray<ProductQuantity> Rewards;
    /// <summary>
    /// If true, the player is recommended to avoid rushing this task
    /// </summary>
    public readonly bool IsLongTermTask;

    public GoalListProto(
      Mafi.Core.Prototypes.Proto.ID id,
      ImmutableArray<GoalProto> goals,
      IGoalListTriggerData trigger,
      ImmutableArray<ProductQuantity> rewards,
      string title,
      bool isLongTermTask = false)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, new Mafi.Core.Prototypes.Proto.Str(Loc.Str(id.ToString() + "__name", title, "title for set of goals")));
      this.Goals = goals;
      this.TriggerData = trigger;
      this.Rewards = rewards;
      this.IsLongTermTask = isLongTermTask;
    }
  }
}
