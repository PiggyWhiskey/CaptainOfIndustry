// Decompiled with JetBrains decompiler
// Type: Mafi.ImplicitUseTargetFlags
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System;

#nullable disable
namespace Mafi
{
  /// <summary>
  /// Specify what is considered to be used implicitly when marked
  /// with <see cref="T:Mafi.MeansImplicitUseAttribute" /> or <see cref="T:Mafi.UsedImplicitlyAttribute" />.
  /// </summary>
  [Flags]
  public enum ImplicitUseTargetFlags
  {
    Default = 1,
    Itself = Default, // 0x00000001
    /// <summary>Members of entity marked with attribute are considered used.</summary>
    Members = 2,
    /// <summary>Entity marked with attribute and all its members considered used.</summary>
    WithMembers = Members | Itself, // 0x00000003
  }
}
