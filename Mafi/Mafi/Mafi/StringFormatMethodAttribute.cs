// Decompiled with JetBrains decompiler
// Type: Mafi.StringFormatMethodAttribute
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
  /// Indicates that the marked method builds string by the format pattern and (optional) arguments.
  /// The parameter, which contains the format string, should be given in constructor. The format string
  /// should be in <see cref="M:System.String.Format(System.IFormatProvider,System.String,System.Object[])" />-like form.
  /// </summary>
  /// <example><code>
  /// [StringFormatMethod("message")]
  /// void ShowError(string message, params object[] args) { /* do something */ }
  /// 
  /// void Foo() {
  ///   ShowError("Failed: {0}"); // Warning: Non-existing argument in format string
  /// }
  /// </code></example>
  [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Delegate)]
  public sealed class StringFormatMethodAttribute : Attribute
  {
    /// <param name="formatParameterName">
    /// Specifies which parameter of an annotated method should be treated as the format string
    /// </param>
    public StringFormatMethodAttribute([NotNull] string formatParameterName)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.FormatParameterName = formatParameterName;
    }

    [NotNull]
    public string FormatParameterName { get; }
  }
}
