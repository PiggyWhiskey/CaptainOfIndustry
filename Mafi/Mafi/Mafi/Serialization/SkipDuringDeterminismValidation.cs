// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.SkipDuringDeterminismValidation
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi.Serialization
{
  /// <summary>
  /// Serialization of marked class will be skipped during determinism validation. This works only for filtering
  /// classes in resolver, not as members of other classes.
  /// </summary>
  /// <remarks>
  /// This should be generally used for UI state classes that are not present in any other serializable classes.
  /// </remarks>
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
  public sealed class SkipDuringDeterminismValidation : Attribute
  {
    public SkipDuringDeterminismValidation()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
