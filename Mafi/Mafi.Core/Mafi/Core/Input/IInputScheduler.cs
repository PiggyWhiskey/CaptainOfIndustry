﻿// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Input.IInputScheduler
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi.Core.Input
{
  public interface IInputScheduler
  {
    IEvent<IInputCommand> OnCommandProcessed { get; }

    int ProcessedCommandsInThisSessionAffectingSave { get; }

    T ScheduleInputCmd<T>(T cmd) where T : IInputCommand;
  }
}
