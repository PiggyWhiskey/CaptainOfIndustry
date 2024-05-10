// Decompiled with JetBrains decompiler
// Type: Mafi.RegistrationMode
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System;

#nullable disable
namespace Mafi
{
  /// <summary>
  /// Dependency registration modes specify how to register marked class. Note that individual flags can be combined.
  /// </summary>
  [Flags]
  public enum RegistrationMode
  {
    /// <summary>Marked class will be registered as self.</summary>
    AsSelf = 1,
    /// <summary>
    /// Marked class will be registered as all implemented interfaces in its entire hierarchy.
    /// </summary>
    AsAllInterfaces = 2,
    /// <summary>
    /// Marked class will be registered as self and as all implemented interfaces in its entire hierarchy.
    /// </summary>
    AsEverything = AsAllInterfaces | AsSelf, // 0x00000003
  }
}
