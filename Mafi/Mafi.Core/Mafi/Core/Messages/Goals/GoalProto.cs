// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.Goals.GoalProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Localization;
using System;

#nullable disable
namespace Mafi.Core.Messages.Goals
{
  public abstract class GoalProto : Mafi.Core.Prototypes.Proto
  {
    public readonly Mafi.Core.Prototypes.Proto.ID? Tutorial;
    public readonly GoalProto.TutorialUnlockMode TutorialUnlock;
    /// <summary>
    /// Adds a small suggestion icon that shows this text on hover
    /// </summary>
    public readonly LocStrFormatted? Tip;
    public readonly int LockedByIndex;

    public abstract Type Implementation { get; }

    protected GoalProto(
      string id,
      Mafi.Core.Prototypes.Proto.ID? tutorial,
      int lockedByIndex,
      LocStrFormatted? tip = null,
      GoalProto.TutorialUnlockMode tutorialUnlock = GoalProto.TutorialUnlockMode.DoNotUnlock)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(new Mafi.Core.Prototypes.Proto.ID("Goal_" + id), Mafi.Core.Prototypes.Proto.Str.Empty);
      this.Tutorial = tutorial;
      this.Tip = tip;
      this.LockedByIndex = lockedByIndex;
      this.TutorialUnlock = tutorialUnlock;
      Assert.That<bool>(tutorial.HasValue && tip.HasValue).IsFalse("Tip and tutorial at the same time are not supported!");
    }

    public enum TutorialUnlockMode
    {
      DoNotUnlock,
      UnlockSilently,
      UnlockAndNotify,
    }
  }
}
