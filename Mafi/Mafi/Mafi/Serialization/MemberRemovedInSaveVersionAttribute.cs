// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.MemberRemovedInSaveVersionAttribute
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi.Serialization
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true, Inherited = false)]
  public sealed class MemberRemovedInSaveVersionAttribute : Attribute
  {
    public readonly int RemovedInVersion;
    public readonly string Name;
    public readonly Type Type;
    public int? WasNewInVersion;
    public readonly bool WasSerializedUsingNonVariableEncoding;

    public MemberRemovedInSaveVersionAttribute(
      string name,
      int removedInVersion,
      Type type,
      int wasNewInVersion = 0,
      bool wasSerializedUsingNonVariableEncoding = false)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Name = name;
      this.RemovedInVersion = removedInVersion;
      this.Type = type;
      this.WasNewInVersion = wasNewInVersion == 0 ? new int?() : new int?(wasNewInVersion);
      this.WasSerializedUsingNonVariableEncoding = wasSerializedUsingNonVariableEncoding;
    }
  }
}
