// Decompiled with JetBrains decompiler
// Type: Mafi.ProvidesContextAttribute
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi
{
  /// <summary>
  /// Indicates the type member or parameter of some type, that should be used instead of all other ways
  /// to get the value of that type. This annotation is useful when you have some "context" value evaluated
  /// and stored somewhere, meaning that all other ways to get this value must be consolidated with existing one.
  /// </summary>
  /// <example><code>
  /// class Foo {
  ///   [ProvidesContext] IBarService _barService = ...;
  /// 
  ///   void ProcessNode(INode node) {
  ///     DoSomething(node, node.GetGlobalServices().Bar);
  ///     //              ^ Warning: use value of '_barService' field
  ///   }
  /// }
  /// </code></example>
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Interface | AttributeTargets.Parameter | AttributeTargets.GenericParameter)]
  public sealed class ProvidesContextAttribute : Attribute
  {
    public ProvidesContextAttribute()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
