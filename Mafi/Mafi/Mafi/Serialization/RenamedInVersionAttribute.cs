// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.RenamedInVersionAttribute
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi.Serialization
{
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event)]
  public sealed class RenamedInVersionAttribute : Attribute
  {
    public readonly int? RenamedInSaveVersion;
    public readonly string OldName;

    public RenamedInVersionAttribute(int renamedInSaveVersion, string oldName)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.RenamedInSaveVersion = new int?(renamedInSaveVersion);
      this.OldName = oldName;
    }
  }
}
