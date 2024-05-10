// Decompiled with JetBrains decompiler
// Type: Mafi.PureAttribute
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
  /// Indicates that a method does not make any observable state changes.
  /// The same as <c>System.Diagnostics.Contracts.PureAttribute</c>.
  /// </summary>
  /// <example><code>
  /// [Pure] int Multiply(int x, int y) =&gt; x * y;
  /// 
  /// void M() {
  ///   Multiply(123, 42); // Waring: Return value of pure method is not used
  /// }
  /// </code></example>
  [AttributeUsage(AttributeTargets.Method)]
  public sealed class PureAttribute : Attribute
  {
    public PureAttribute()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
