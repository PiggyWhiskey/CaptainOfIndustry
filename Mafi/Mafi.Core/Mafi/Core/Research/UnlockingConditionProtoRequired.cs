// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Research.UnlockingConditionProtoRequired
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Prototypes;
using System;

#nullable disable
namespace Mafi.Core.Research
{
  public class UnlockingConditionProtoRequired : IResearchNodeUnlockingCondition
  {
    /// <summary>
    /// Proto that needs to be unlocked so this node can be available to research.
    /// </summary>
    public readonly Proto ProtoRequired;

    public UnlockingConditionProtoRequired(Proto protoRequired)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ProtoRequired = protoRequired;
    }

    [GlobalDependency(RegistrationMode.AsEverything, false, false)]
    public class Manager : UnlockingManagerBase<UnlockingConditionProtoRequired>
    {
      private readonly UnlockedProtosDb m_unlockedProtosDb;

      public Manager(UnlockedProtosDb unlockedProtosDb)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_unlockedProtosDb = unlockedProtosDb;
      }

      protected override void OnInitialized()
      {
        this.m_unlockedProtosDb.OnUnlockedSetChanged.AddNonSaveable<UnlockingConditionProtoRequired.Manager>(this, new Action(((UnlockingManagerBase<UnlockingConditionProtoRequired>) this).UpdateAllConditions));
      }

      protected override bool IsConditionSatisfied(UnlockingConditionProtoRequired condition)
      {
        return this.m_unlockedProtosDb.IsUnlocked(condition.ProtoRequired);
      }
    }
  }
}
