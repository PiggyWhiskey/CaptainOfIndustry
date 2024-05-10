// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Map.MapCellInspector
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.GameLoop;
using Mafi.Core.Input;
using Mafi.Core.Map;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UserInterface;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Map
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public class MapCellInspector : IUnityUi
  {
    private readonly IGameLoopEvents m_gameLoop;
    private readonly IInputScheduler m_inputScheduler;
    private readonly MapCellWindowView m_windowView;

    public MapCell MapCell { get; private set; }

    public bool IsActive { get; private set; }

    public MapCellInspector(IGameLoopEvents gameLoop, IInputScheduler inputScheduler)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_gameLoop = gameLoop;
      this.m_inputScheduler = inputScheduler;
      this.m_windowView = new MapCellWindowView(this);
    }

    public void RegisterUi(UiBuilder builder)
    {
      this.m_windowView.SetOnCloseButtonClickAction(new Action(this.Deactivate));
      this.m_windowView.BuildUi(builder);
    }

    public void Activate(MapCell cell)
    {
      this.IsActive = true;
      this.MapCell = cell.CheckNotNull<MapCell>();
      this.m_gameLoop.SyncUpdate.AddNonSaveable<MapCellInspector>(this, new Action<GameTime>(this.syncUpdate));
      this.m_gameLoop.RenderUpdate.AddNonSaveable<MapCellInspector>(this, new Action<GameTime>(this.renderUpdate));
      this.m_windowView.Show();
    }

    public void Deactivate()
    {
      this.m_gameLoop.RenderUpdate.RemoveNonSaveable<MapCellInspector>(this, new Action<GameTime>(this.renderUpdate));
      this.m_gameLoop.SyncUpdate.RemoveNonSaveable<MapCellInspector>(this, new Action<GameTime>(this.syncUpdate));
      this.m_windowView.Hide();
      this.IsActive = false;
    }

    public void UnlockCurrentCell()
    {
      this.m_inputScheduler.ScheduleInputCmd<UnlockMapCellCmd>(new UnlockMapCellCmd(this.MapCell.Id));
    }

    private void renderUpdate(GameTime time) => this.m_windowView.RenderUpdate(time);

    private void syncUpdate(GameTime time) => this.m_windowView.SyncUpdate(time);
  }
}
