// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.TopStatusBar.GameSpeedController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.GameLoop;
using Mafi.Core.Input;
using Mafi.Core.Simulation;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.TopStatusBar
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class GameSpeedController
  {
    private static readonly int[] SPEEDS;
    private readonly IInputScheduler m_inputScheduler;
    private readonly ISimLoopEvents m_simLoopEvents;
    private readonly ShortcutsManager m_shortcutsManager;

    public bool IsPaused => this.m_simLoopEvents.IsSimPaused;

    public GameSpeedController(
      IUnityInputMgr inputManager,
      IInputScheduler inputScheduler,
      IGameLoopEvents gameLoopEvents,
      ISimLoopEvents simLoopEvents,
      ShortcutsManager shortcutsManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_inputScheduler = inputScheduler;
      this.m_simLoopEvents = simLoopEvents;
      this.m_shortcutsManager = shortcutsManager;
      inputManager.RegisterGameSpeedController(this);
    }

    public bool InputUpdate()
    {
      if (UnityInputManager.IsInputFieldFocused())
        return false;
      if (this.m_shortcutsManager.IsDown(this.m_shortcutsManager.PauseGame) || this.m_shortcutsManager.IsDown(this.m_shortcutsManager.SetGameSpeedTo0))
      {
        this.togglePause();
        return true;
      }
      if (this.m_shortcutsManager.IsDown(this.m_shortcutsManager.SetGameSpeedTo1))
      {
        this.SetSpeedIndex(1);
        return true;
      }
      if (this.m_shortcutsManager.IsDown(this.m_shortcutsManager.SetGameSpeedTo2))
      {
        this.SetSpeedIndex(2);
        return true;
      }
      if (this.m_shortcutsManager.IsDown(this.m_shortcutsManager.SetGameSpeedTo3))
      {
        this.SetSpeedIndex(3);
        return true;
      }
      if (this.m_shortcutsManager.IsDown(this.m_shortcutsManager.IncreaseGameSpeed))
      {
        int currentSpeedIndex = this.GetCurrentSpeedIndex();
        int index = (currentSpeedIndex + 1).Min(GameSpeedController.SPEEDS.Length - 1);
        if (currentSpeedIndex != index)
          this.SetSpeedIndex(index);
        return true;
      }
      if (!this.m_shortcutsManager.IsDown(this.m_shortcutsManager.DecreaseGameSpeed))
        return false;
      int currentSpeedIndex1 = this.GetCurrentSpeedIndex();
      int index1 = (currentSpeedIndex1 - 1).Max(0);
      if (currentSpeedIndex1 != index1)
        this.SetSpeedIndex(index1);
      return true;
    }

    public int GetCurrentSpeedIndex()
    {
      if (this.m_simLoopEvents.IsSimPaused)
        return 0;
      if (this.m_simLoopEvents.SimSpeedMult == 2)
        return 2;
      return this.m_simLoopEvents.SimSpeedMult == 3 ? 3 : 1;
    }

    public void RequestPause()
    {
      this.m_inputScheduler.ScheduleInputCmd<SetSimPauseStateCmd>(new SetSimPauseStateCmd(true));
    }

    public void RequestResume()
    {
      this.m_inputScheduler.ScheduleInputCmd<SetSimPauseStateCmd>(new SetSimPauseStateCmd(false));
    }

    private void togglePause()
    {
      this.m_inputScheduler.ScheduleInputCmd<SetSimPauseStateCmd>(new SetSimPauseStateCmd(!this.m_simLoopEvents.IsSimPaused));
    }

    public void SetSpeedIndex(int index)
    {
      if (index > 0)
      {
        if (this.m_simLoopEvents.IsSimPaused)
          this.RequestResume();
        this.m_inputScheduler.ScheduleInputCmd<GameSpeedChangeCmd>(new GameSpeedChangeCmd(GameSpeedController.SPEEDS[index]));
      }
      else
        this.RequestPause();
    }

    static GameSpeedController()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      GameSpeedController.SPEEDS = new int[4]{ 0, 1, 2, 3 };
    }
  }
}
