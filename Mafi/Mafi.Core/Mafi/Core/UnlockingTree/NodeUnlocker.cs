// Decompiled with JetBrains decompiler
// Type: Mafi.Core.UnlockingTree.NodeUnlocker
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using System;
using System.Reflection;

#nullable disable
namespace Mafi.Core.UnlockingTree
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class NodeUnlocker : INodeUnlocker
  {
    private readonly Dict<Type, NodeUnlocker.IUnlockerWrapper> m_unlockersDict;
    private readonly ImmutableArray<NodeUnlocker.IUnlockerWrapper> m_unlockers;

    public NodeUnlocker(
      AllImplementationsOf<IUnitUnlocker> unitUnlockerImpls)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Type[] constrParamTypes = new Type[1];
      object[] constrParams = new object[1];
      this.m_unlockers = unitUnlockerImpls.Implementations.Map<NodeUnlocker.IUnlockerWrapper>((Func<IUnitUnlocker, NodeUnlocker.IUnlockerWrapper>) (unlocker =>
      {
        Type type = typeof (IUnitUnlocker<>);
        Type[] typeArray = new Type[1]
        {
          unlocker.UnlockedType
        };
        constrParamTypes[0] = type.MakeGenericType(typeArray);
        ConstructorInfo constructor = typeof (NodeUnlocker.UnlockerWrapper<>).MakeGenericType(unlocker.UnlockedType).GetConstructor(constrParamTypes);
        constrParams[0] = (object) unlocker;
        return (NodeUnlocker.IUnlockerWrapper) constructor.Invoke(constrParams);
      }));
      this.m_unlockersDict = new Dict<Type, NodeUnlocker.IUnlockerWrapper>();
      foreach (NodeUnlocker.IUnlockerWrapper unlocker in this.m_unlockers)
      {
        NodeUnlocker.IUnlockerWrapper unlockerWrapper;
        if (this.m_unlockersDict.TryGetValue(unlocker.UnlockedType, out unlockerWrapper))
          Log.Error(string.Format("There are two registered type for unlocking units {0}: ", (object) unlocker.UnlockedType) + string.Format("{0} and unlocker", (object) unlockerWrapper));
        else
          this.m_unlockersDict.Add(unlocker.UnlockedType, unlocker);
      }
    }

    public void UnlockUnitsOf(IUnlockingNode node)
    {
      foreach (IUnlockNodeUnit unit in node.Units)
      {
        Option<NodeUnlocker.IUnlockerWrapper> unlockerFor = this.tryGetUnlockerFor(unit);
        if (unlockerFor.IsNone)
          Log.Error(string.Format("Unlocker not found for unlock unit {0}.", (object) unit));
        else
          unlockerFor.Value.AddUnitToUnlock(unit);
      }
      foreach (NodeUnlocker.IUnlockerWrapper unlocker in this.m_unlockers)
        unlocker.UnlockUnitsAndClear();
    }

    private Option<NodeUnlocker.IUnlockerWrapper> tryGetUnlockerFor(IUnlockNodeUnit unit)
    {
      Type type = unit.GetType();
      NodeUnlocker.IUnlockerWrapper unlockerWrapper;
      if (this.m_unlockersDict.TryGetValue(type, out unlockerWrapper))
        return Option.Some<NodeUnlocker.IUnlockerWrapper>(unlockerWrapper);
      for (Type key1 = type; key1 != typeof (object); key1 = key1.BaseType)
      {
        if (this.m_unlockersDict.TryGetValue(key1, out unlockerWrapper))
        {
          this.m_unlockersDict.Add(type, unlockerWrapper);
          return Option.Some<NodeUnlocker.IUnlockerWrapper>(unlockerWrapper);
        }
        foreach (Type key2 in key1.GetInterfaces())
        {
          if (this.m_unlockersDict.TryGetValue(key2, out unlockerWrapper))
          {
            this.m_unlockersDict.Add(type, unlockerWrapper);
            return Option.Some<NodeUnlocker.IUnlockerWrapper>(unlockerWrapper);
          }
        }
      }
      return (Option<NodeUnlocker.IUnlockerWrapper>) Option.None;
    }

    private interface IUnlockerWrapper
    {
      Type UnlockedType { get; }

      bool AnyUnitsToUnlock { get; }

      void AddUnitToUnlock(IUnlockNodeUnit unit);

      void UnlockUnitsAndClear();
    }

    private class UnlockerWrapper<TUnit> : NodeUnlocker.IUnlockerWrapper where TUnit : IUnlockNodeUnit
    {
      private readonly Lyst<TUnit> m_unitsToUnlock;
      private readonly IUnitUnlocker<TUnit> m_unitUnlocker;

      public Type UnlockedType => this.m_unitUnlocker.UnlockedType;

      public bool AnyUnitsToUnlock => this.m_unitsToUnlock.IsNotEmpty;

      public UnlockerWrapper(IUnitUnlocker<TUnit> unitUnlocker)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.m_unitsToUnlock = new Lyst<TUnit>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_unitUnlocker = unitUnlocker;
      }

      public void AddUnitToUnlock(IUnlockNodeUnit unit) => this.m_unitsToUnlock.Add((TUnit) unit);

      public void UnlockUnitsAndClear()
      {
        if (this.m_unitsToUnlock.IsEmpty)
          return;
        this.m_unitUnlocker.Unlock((IIndexable<TUnit>) this.m_unitsToUnlock);
        this.m_unitsToUnlock.Clear();
      }
    }
  }
}
