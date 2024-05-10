// Decompiled with JetBrains decompiler
// Type: Mafi.EditorValidationSourceAttribute
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
  /// Can be added to a text or number editor to provide it with secondary source of validation result.
  /// IMPORTANT: The validation result is only queried on editor rebuild.
  /// For more lightweight applications, check <see cref="T:Mafi.IEditorValidationAttribute" />
  /// 
  /// Example use:
  /// [EditorValidationSource(nameof(CodeValidationResult))]
  /// public string CustomCode { get; private set; }
  /// public string CodeValidationResult =&gt; !CustomCode.StartsWith("namespace")
  /// 	? $"This must start with namespace." : null;
  /// </summary>
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
  public sealed class EditorValidationSourceAttribute : Attribute
  {
    public readonly string MemberName;

    public EditorValidationSourceAttribute(string memberName)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.MemberName = memberName;
    }
  }
}
