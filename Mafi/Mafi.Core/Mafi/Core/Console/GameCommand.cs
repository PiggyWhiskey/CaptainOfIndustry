// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Console.GameCommand
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace Mafi.Core.Console
{
  public class GameCommand
  {
    public readonly string CanonicalName;
    public readonly string Documentation;
    public readonly string ParametersDocStr;
    public readonly object Target;
    public readonly MethodInfo Method;
    public readonly ImmutableArray<ParameterInfo> Parameters;
    public readonly int MandatoryParametersCount;
    /// <summary>
    /// Whether this command should be invoked on main thread. Otherwise, it should be invoked on sim thread.
    /// </summary>
    public readonly bool InvokeOnMainThread;
    public readonly bool InvokeDuringSync;

    public GameCommand(
      string canonicalName,
      string documentation,
      object target,
      MethodInfo method,
      bool invokeOnMainThread,
      bool invokeDuringSync)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.CanonicalName = canonicalName;
      this.Documentation = documentation;
      this.Target = target;
      this.Method = method;
      this.InvokeOnMainThread = invokeOnMainThread;
      this.InvokeDuringSync = invokeDuringSync;
      this.Parameters = ((ICollection<ParameterInfo>) method.GetParameters()).ToImmutableArray<ParameterInfo>();
      int num = this.Parameters.IndexOf((Predicate<ParameterInfo>) (x => x.IsOptional));
      this.MandatoryParametersCount = num >= 0 ? num : this.Parameters.Length;
      this.ParametersDocStr = this.Parameters.Select<string>((Func<ParameterInfo, string>) (p => p.ParameterType.Name + " " + p.Name)).JoinStrings(", ");
    }
  }
}
