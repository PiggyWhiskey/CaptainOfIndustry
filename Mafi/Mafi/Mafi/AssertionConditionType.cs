// Decompiled with JetBrains decompiler
// Type: Mafi.AssertionConditionType
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

#nullable disable
namespace Mafi
{
  /// <summary>
  /// Specifies assertion type. If the assertion method argument satisfies the condition,
  /// then the execution continues. Otherwise, execution is assumed to be halted.
  /// </summary>
  public enum AssertionConditionType
  {
    /// <summary>Marked parameter should be evaluated to true.</summary>
    IS_TRUE,
    /// <summary>Marked parameter should be evaluated to false.</summary>
    IS_FALSE,
    /// <summary>Marked parameter should be evaluated to null value.</summary>
    IS_NULL,
    /// <summary>Marked parameter should be evaluated to not null value.</summary>
    IS_NOT_NULL,
  }
}
