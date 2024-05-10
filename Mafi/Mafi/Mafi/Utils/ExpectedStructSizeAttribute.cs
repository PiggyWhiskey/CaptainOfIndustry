// Decompiled with JetBrains decompiler
// Type: Mafi.Utils.ExpectedStructSizeAttribute
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi.Utils
{
  /// <summary>
  /// Size of marked struct will be automatically checked in unit tests.
  /// Struct size is critical for structs that are used for data exchange with native code.
  /// </summary>
  [AttributeUsage(AttributeTargets.Struct)]
  public sealed class ExpectedStructSizeAttribute : Attribute
  {
    public readonly int SizeBytes;

    public ExpectedStructSizeAttribute(int sizeBytes)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.SizeBytes = sizeBytes;
    }
  }
}
