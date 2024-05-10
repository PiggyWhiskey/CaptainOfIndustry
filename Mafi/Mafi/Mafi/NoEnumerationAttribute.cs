// Decompiled with JetBrains decompiler
// Type: Mafi.NoEnumerationAttribute
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
  /// Indicates that IEnumerable passed as a parameter is not enumerated.
  /// Use this annotation to suppress the 'Possible multiple enumeration of IEnumerable' inspection.
  /// </summary>
  /// <example><code>
  /// static void ThrowIfNull&lt;T&gt;([NoEnumeration] T v, string n) where T : class
  /// {
  ///   // custom check for null but no enumeration
  /// }
  /// 
  /// void Foo(IEnumerable&lt;string&gt; values)
  /// {
  ///   ThrowIfNull(values, nameof(values));
  ///   var x = values.ToList(); // No warnings about multiple enumeration
  /// }
  /// </code></example>
  [AttributeUsage(AttributeTargets.Parameter)]
  public sealed class NoEnumerationAttribute : Attribute
  {
    public NoEnumerationAttribute()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
