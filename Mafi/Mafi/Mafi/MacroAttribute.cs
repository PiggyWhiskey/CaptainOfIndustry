// Decompiled with JetBrains decompiler
// Type: Mafi.MacroAttribute
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
  /// Allows specifying a macro for a parameter of a <see cref="T:Mafi.SourceTemplateAttribute">source template</see>.
  /// </summary>
  /// <remarks>
  /// You can apply the attribute on the whole method or on any of its additional parameters. The macro expression
  /// is defined in the <see cref="P:Mafi.MacroAttribute.Expression" /> property. When applied on a method, the target
  /// template parameter is defined in the <see cref="P:Mafi.MacroAttribute.Target" /> property. To apply the macro silently
  /// for the parameter, set the <see cref="P:Mafi.MacroAttribute.Editable" /> property value = -1.
  /// </remarks>
  /// <example>
  /// Applying the attribute on a source template method:
  /// <code>
  /// [SourceTemplate, Macro(Target = "item", Expression = "suggestVariableName()")]
  /// public static void forEach&lt;T&gt;(this IEnumerable&lt;T&gt; collection) {
  ///   foreach (var item in collection) {
  ///     //$ $END$
  ///   }
  /// }
  /// </code>
  /// Applying the attribute on a template method parameter:
  /// <code>
  /// [SourceTemplate]
  /// public static void something(this Entity x, [Macro(Expression = "guid()", Editable = -1)] string newguid) {
  ///   /*$ var $x$Id = "$newguid$" + x.ToString();
  ///   x.DoSomething($x$Id); */
  /// }
  /// </code>
  /// </example>
  [AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter, AllowMultiple = true)]
  public sealed class MacroAttribute : Attribute
  {
    /// <summary>
    /// Allows specifying a macro that will be executed for a <see cref="T:Mafi.SourceTemplateAttribute">source template</see>
    /// parameter when the template is expanded.
    /// </summary>
    [CanBeNull]
    public string Expression { get; set; }

    /// <summary>
    /// Allows specifying which occurrence of the target parameter becomes editable when the template is deployed.
    /// </summary>
    /// <remarks>
    /// If the target parameter is used several times in the template, only one occurrence becomes editable;
    /// other occurrences are changed synchronously. To specify the zero-based index of the editable occurrence,
    /// use values &gt;= 0. To make the parameter non-editable when the template is expanded, use -1.
    /// </remarks>
    public int Editable { get; set; }

    /// <summary>
    /// Identifies the target parameter of a <see cref="T:Mafi.SourceTemplateAttribute">source template</see> if the
    /// <see cref="T:Mafi.MacroAttribute" /> is applied on a template method.
    /// </summary>
    [CanBeNull]
    public string Target { get; set; }

    public MacroAttribute()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
