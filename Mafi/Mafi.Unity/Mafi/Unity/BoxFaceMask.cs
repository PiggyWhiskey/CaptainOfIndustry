// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.BoxFaceMask
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;

#nullable disable
namespace Mafi.Unity
{
  [Flags]
  public enum BoxFaceMask
  {
    None = 0,
    All = 63, // 0x0000003F
    PlusX = 1,
    MinusX = 2,
    PlusY = 4,
    MinusY = 8,
    PlusZ = 16, // 0x00000010
    MinusZ = 32, // 0x00000020
  }
}
