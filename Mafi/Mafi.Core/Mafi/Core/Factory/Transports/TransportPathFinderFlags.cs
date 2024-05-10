// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Transports.TransportPathFinderFlags
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using System;

#nullable disable
namespace Mafi.Core.Factory.Transports
{
  [Flags]
  public enum TransportPathFinderFlags
  {
    None = 0,
    StartMustBeFlat = 1,
    GoalMustBeFlat = 2,
    InvertTieBreaking = 4,
    BanTilesInFrontOfPorts = 8,
    AllowOnlyStraight = 16, // 0x00000010
    BanStartRampsInX = 32, // 0x00000020
    BanStartRampsInY = 64, // 0x00000040
  }
}
