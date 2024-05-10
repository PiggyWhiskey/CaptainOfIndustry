// Decompiled with JetBrains decompiler
// Type: Mafi.SourceTemplateAttribute
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
  /// An extension method marked with this attribute is processed by code completion
  /// as a 'Source Template'. When the extension method is completed over some expression, its source code
  /// is automatically expanded like a template at call site.
  /// </summary>
  /// <remarks>
  /// Template method body can contain valid source code and/or special comments starting with '$'.
  /// Text inside these comments is added as source code when the template is applied. Template parameters
  /// can be used either as additional method parameters or as identifiers wrapped in two '$' signs.
  /// Use the <see cref="T:Mafi.MacroAttribute" /> attribute to specify macros for parameters.
  /// </remarks>
  /// <example>
  /// In this example, the 'forEach' method is a source template available over all values
  /// of enumerable types, producing ordinary C# 'foreach' statement and placing caret inside block:
  /// <code>
  /// [SourceTemplate]
  /// public static void forEach&lt;T&gt;(this IEnumerable&lt;T&gt; xs) {
  ///   foreach (var x in xs) {
  ///      //$ $END$
  ///   }
  /// }
  /// </code>
  /// </example>
  [AttributeUsage(AttributeTargets.Method)]
  public sealed class SourceTemplateAttribute : Attribute
  {
    public SourceTemplateAttribute()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
