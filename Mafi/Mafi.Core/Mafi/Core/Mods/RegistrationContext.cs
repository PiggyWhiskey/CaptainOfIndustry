﻿// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Mods.RegistrationContext
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Mods
{
  public class RegistrationContext
  {
    public readonly ProtosDb PrototypesDb;

    public RegistrationContext(ProtosDb prototypesDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Assert.That<bool>(prototypesDb.IsReadonly).IsTrue();
      this.PrototypesDb = prototypesDb.CheckNotNull<ProtosDb>();
    }
  }
}
