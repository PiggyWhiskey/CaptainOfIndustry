﻿// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.AlwaysNewTutorialProgressTracker
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.Messages
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, true)]
  public sealed class AlwaysNewTutorialProgressTracker : ITutorialProgressTracker
  {
    public void MarkTutorialAsReadFromSim(MessageProto message)
    {
    }

    public bool IsTutorialNew(MessageProto message) => true;

    public AlwaysNewTutorialProgressTracker()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
