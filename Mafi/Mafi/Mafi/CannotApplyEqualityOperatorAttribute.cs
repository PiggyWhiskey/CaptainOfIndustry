// Decompiled with JetBrains decompiler
// Type: Mafi.CannotApplyEqualityOperatorAttribute
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
  /// Indicates that the value of the marked type (or its derivatives)
  /// cannot be compared using '==' or '!=' operators and <c>Equals()</c>
  /// should be used instead. However, using '==' or '!=' for comparison
  /// with <c>null</c> is always permitted.
  /// </summary>
  /// <example><code>
  /// [CannotApplyEqualityOperator]
  /// class NoEquality { }
  /// 
  /// class UsesNoEquality {
  ///   void Test() {
  ///     var ca1 = new NoEquality();
  ///     var ca2 = new NoEquality();
  ///     if (ca1 != null) { // OK
  ///       bool condition = ca1 == ca2; // Warning
  ///     }
  ///   }
  /// }
  /// </code></example>
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
  public sealed class CannotApplyEqualityOperatorAttribute : Attribute
  {
    public CannotApplyEqualityOperatorAttribute()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
