// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Factory.MainframeWindowView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Base.Prototypes.Machines.ComputingEntities;
using Mafi.Core;
using Mafi.Core.Syncers;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Factory
{
  internal class MainframeWindowView : StaticEntityInspectorBase<Mainframe>
  {
    private readonly MainframeInspector m_controller;

    protected override Mainframe Entity => this.m_controller.SelectedEntity;

    public MainframeWindowView(MainframeInspector controller)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector((IEntityInspector) controller);
      this.m_controller = controller.CheckNotNull<MainframeInspector>();
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      base.AddCustomItems(itemContainer);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      this.AddClearButton(new Action(((EntityInspector<Mainframe, MainframeWindowView>) this.m_controller).Clear));
      this.AddComputingGenerationPanel(updaterBuilder, (Func<Computing>) (() => this.Entity.MaxComputingGenerationCapacity));
      StatusPanel statusInfo = this.AddStatusInfoPanel();
      updaterBuilder.Observe<Mainframe.State>((Func<Mainframe.State>) (() => this.Entity.CurrentState)).Do((Action<Mainframe.State>) (state =>
      {
        switch (state)
        {
          case Mainframe.State.None:
            statusInfo.SetStatus(Tr.EntityStatus__Idle, StatusPanel.State.Warning);
            break;
          case Mainframe.State.Working:
            statusInfo.SetStatusWorking();
            break;
          case Mainframe.State.Paused:
            statusInfo.SetStatusPaused();
            break;
          case Mainframe.State.Broken:
            statusInfo.SetStatus(TrCore.EntityStatus__Broken, StatusPanel.State.Critical);
            break;
          case Mainframe.State.NotEnoughWorkers:
            statusInfo.SetStatusNoWorkers();
            break;
          case Mainframe.State.NotEnoughElectricity:
            statusInfo.SetStatus(TrCore.EntityStatus__LowPower, StatusPanel.State.Critical);
            break;
        }
      }));
      this.AddUpdater(updaterBuilder.Build());
    }
  }
}
