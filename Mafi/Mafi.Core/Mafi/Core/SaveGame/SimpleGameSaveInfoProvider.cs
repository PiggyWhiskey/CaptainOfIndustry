// Decompiled with JetBrains decompiler
// Type: Mafi.Core.SaveGame.SimpleGameSaveInfoProvider
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Buildings.Settlements;
using Mafi.Core.Research;
using Mafi.Core.Simulation;
using Mafi.Core.SpaceProgram;
using System;

#nullable disable
namespace Mafi.Core.SaveGame
{
  public class SimpleGameSaveInfoProvider : IGameSaveInfoProvider
  {
    private readonly ICalendar m_calendar;
    private readonly SettlementsManager m_settlementsManager;
    private readonly ResearchManager m_researchManager;
    private readonly IRocketLaunchManager m_rocketLaunchManager;

    public SimpleGameSaveInfoProvider(
      ICalendar calendar,
      SettlementsManager settlementsManager,
      ResearchManager researchManager,
      IRocketLaunchManager rocketLaunchManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_calendar = calendar;
      this.m_settlementsManager = settlementsManager;
      this.m_researchManager = researchManager;
      this.m_rocketLaunchManager = rocketLaunchManager;
    }

    public void ScheduleScreenshotRendering(Action doneCallback)
    {
    }

    public GameSaveInfo CreateGameSaveInfo()
    {
      GameDate currentDate = this.m_calendar.CurrentDate;
      int totalPopulation = this.m_settlementsManager.GetTotalPopulation();
      ImmutableArray<ResearchNode> allNodes = this.m_researchManager.AllNodes;
      int researchNodesUnlocked = allNodes.Count((Func<ResearchNode, bool>) (x => x.State == ResearchNodeState.Researched));
      allNodes = this.m_researchManager.AllNodes;
      int length = allNodes.Length;
      int launchesCount = this.m_rocketLaunchManager.LaunchesCount;
      Vector2i zero = Vector2i.Zero;
      byte[] thumbnailImageBytes = Array.Empty<byte>();
      return new GameSaveInfo(currentDate, "", totalPopulation, researchNodesUnlocked, length, launchesCount, "", zero, thumbnailImageBytes);
    }

    public void NotifySaveDone()
    {
    }
  }
}
