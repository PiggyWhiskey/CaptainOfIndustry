﻿// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Transports.CanChangeDirectionResult
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.Factory.Transports
{
  public readonly struct CanChangeDirectionResult
  {
    public readonly Transport Transport;
    public readonly Direction903d NewDirection;
    public readonly bool ChangeAtStart;

    public CanChangeDirectionResult(
      Transport transport,
      Direction903d newDirection,
      bool changeAtStart)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Transport = transport;
      this.NewDirection = newDirection;
      this.ChangeAtStart = changeAtStart;
    }
  }
}
