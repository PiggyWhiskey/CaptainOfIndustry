// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Logistics.LogisticsOverviewController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.GameLoop;
using Mafi.Unity.InputControl.Toolbar;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UserInterface;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Logistics
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  internal class LogisticsOverviewController : BaseWindowController<LogisticsOverviewWindow>
  {
    public event Action<IToolbarItemController> VisibilityChanged;

    public bool IsVisible => true;

    public bool DeactivateShortcutsIfNotVisible => false;

    public LogisticsOverviewController(
      IUnityInputMgr inputManager,
      LogisticsOverviewWindow view,
      IGameLoopEvents gameLoop,
      UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(inputManager, gameLoop, builder, view);
    }
  }
}
