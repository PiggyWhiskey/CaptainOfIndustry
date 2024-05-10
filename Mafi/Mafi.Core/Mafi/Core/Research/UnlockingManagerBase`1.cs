// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Research.UnlockingManagerBase`1
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;

#nullable disable
namespace Mafi.Core.Research
{
  /// <summary>
  /// NOTE: These classes are not meant to be saved. Conditions should be entirely stateless and
  /// depend on other global managers. We recreate all conditions on load. Thanks to this
  /// conditions can be freely changed between save games.
  /// </summary>
  public abstract class UnlockingManagerBase<T> : IResearchUnlockingConditionManager where T : IResearchNodeUnlockingCondition
  {
    private ResearchManager m_researchManager;
    private Lyst<Pair<ResearchNode, T>> m_lockedNodesData;
    private Lyst<Pair<ResearchNode, T>> m_cache;

    protected UnlockingManagerBase()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_lockedNodesData = new Lyst<Pair<ResearchNode, T>>();
      this.m_cache = new Lyst<Pair<ResearchNode, T>>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public void Initialize(ResearchManager researchManager)
    {
      this.m_researchManager = researchManager;
      foreach (ResearchNode allNode in this.m_researchManager.AllNodes)
      {
        if (allNode.State != ResearchNodeState.Researched)
        {
          foreach (IResearchNodeUnlockingCondition unlockingCondition in allNode.Proto.UnlockingConditions)
          {
            if (unlockingCondition is T obj)
            {
              this.InitializeCondition(obj);
              if (!this.IsConditionSatisfied(obj))
              {
                allNode.LockedByConditions.AddAssertNew((IResearchNodeUnlockingCondition) obj);
                this.m_lockedNodesData.Add(new Pair<ResearchNode, T>(allNode, obj));
              }
            }
          }
        }
      }
      this.OnInitialized();
    }

    protected void UpdateAllConditions()
    {
      bool flag = false;
      this.m_cache.Clear();
      foreach (Pair<ResearchNode, T> pair in this.m_lockedNodesData)
      {
        ResearchNode first = pair.First;
        T second = pair.Second;
        if (this.IsConditionSatisfied(second))
        {
          first.LockedByConditions.Remove((IResearchNodeUnlockingCondition) second);
          flag = true;
        }
        else
          this.m_cache.Add(pair);
      }
      Swap.Them<Lyst<Pair<ResearchNode, T>>>(ref this.m_cache, ref this.m_lockedNodesData);
      if (!flag)
        return;
      this.m_researchManager.OnSomeNodeUnlockedMaybe();
    }

    public void RemoveConditionsIfCan(ResearchNode node)
    {
      this.m_cache.Clear();
      foreach (Pair<ResearchNode, T> pair in this.m_lockedNodesData)
      {
        if (pair.First == node)
          node.LockedByConditions.Remove((IResearchNodeUnlockingCondition) pair.Second);
        else
          this.m_cache.Add(pair);
      }
      Swap.Them<Lyst<Pair<ResearchNode, T>>>(ref this.m_cache, ref this.m_lockedNodesData);
    }

    protected virtual void InitializeCondition(T condition)
    {
    }

    protected virtual void OnInitialized()
    {
    }

    protected abstract bool IsConditionSatisfied(T condition);
  }
}
