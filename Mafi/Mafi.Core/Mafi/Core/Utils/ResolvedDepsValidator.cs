// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Utils.ResolvedDepsValidator
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.GameLoop;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Utils
{
  /// <summary>
  /// This class ensures and verifies that dependencies are being resolved in deterministic manner.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  internal class ResolvedDepsValidator : 
    ICommandProcessor<ResolvedDependenciesDuringSimVerifCmd>,
    IAction<ResolvedDependenciesDuringSimVerifCmd>
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly DependencyResolver m_dependencyResolver;
    private readonly InputScheduler m_inputScheduler;

    public static void Serialize(ResolvedDepsValidator value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ResolvedDepsValidator>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ResolvedDepsValidator.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      DependencyResolver.Serialize(this.m_dependencyResolver, writer);
      InputScheduler.Serialize(this.m_inputScheduler, writer);
    }

    public static ResolvedDepsValidator Deserialize(BlobReader reader)
    {
      ResolvedDepsValidator resolvedDepsValidator;
      if (reader.TryStartClassDeserialization<ResolvedDepsValidator>(out resolvedDepsValidator))
        reader.EnqueueDataDeserialization((object) resolvedDepsValidator, ResolvedDepsValidator.s_deserializeDataDelayedAction);
      return resolvedDepsValidator;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<ResolvedDepsValidator>(this, "m_dependencyResolver", (object) DependencyResolver.Deserialize(reader));
      reader.SetField<ResolvedDepsValidator>(this, "m_inputScheduler", (object) InputScheduler.Deserialize(reader));
    }

    public ResolvedDepsValidator(
      DependencyResolver dependencyResolver,
      InputScheduler inputScheduler,
      IGameLoopEvents gameLoopEvents)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_dependencyResolver = dependencyResolver;
      this.m_inputScheduler = inputScheduler;
      gameLoopEvents.RegisterNewGameCreated((object) this, new Action(this.saveDepsVerifCmd));
    }

    private void saveDepsVerifCmd()
    {
      this.m_inputScheduler.ScheduleInputCmd<ResolvedDependenciesDuringSimVerifCmd>(new ResolvedDependenciesDuringSimVerifCmd(this.getDeps()));
    }

    private ImmutableArray<string> getDeps()
    {
      return this.m_dependencyResolver.ResolvedObjects.ToImmutableArray<object, string>((Func<object, string>) (x => x.GetType().FullName));
    }

    public void Invoke(ResolvedDependenciesDuringSimVerifCmd cmd)
    {
      ImmutableArray<string> lyst1 = cmd.ResolvedDepTypes;
      ImmutableArray<string> lyst2 = this.getDeps();
      Set<string> set = new Set<string>(lyst1.AsEnumerable());
      set.IntersectWith((IEnumerable<string>) new Set<string>(lyst2.AsEnumerable()));
      lyst1 = lyst1.Filter(new Predicate<string>(set.Contains));
      lyst2 = lyst2.Filter(new Predicate<string>(set.Contains));
      if (lyst1.Length == lyst2.Length)
      {
        bool flag = true;
        for (int index = 0; index < lyst1.Length; ++index)
        {
          if (lyst1[index] != lyst2[index])
          {
            flag = false;
            break;
          }
        }
        if (flag)
        {
          cmd.SetResultSuccess();
          return;
        }
      }
      cmd.SetResultError("Mismatch of count or order of resolved deps.\nExpected: " + lyst1.JoinStrings(", ") + "\nActual: " + lyst2.JoinStrings(", "));
    }

    static ResolvedDepsValidator()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ResolvedDepsValidator.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ResolvedDepsValidator) obj).SerializeData(writer));
      ResolvedDepsValidator.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ResolvedDepsValidator) obj).DeserializeData(reader));
    }
  }
}
