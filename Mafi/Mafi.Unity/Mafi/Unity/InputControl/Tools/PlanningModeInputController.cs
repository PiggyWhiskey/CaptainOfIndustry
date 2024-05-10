// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Tools.PlanningModeInputController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.GameLoop;
using Mafi.Core.Input;
using Mafi.Core.Messages;
using Mafi.Core.Prototypes;
using Mafi.Unity.InputControl.Messages;
using Mafi.Unity.InputControl.Toolbar;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Styles;
using Mafi.Unity.UserInterface;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Tools
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  internal class PlanningModeInputController : 
    IToolbarItemController,
    IUnityInputController,
    IToolbarItemRegistrar
  {
    private readonly UnlockedProtosDbForUi m_unlockedProtosDb;
    private readonly ToolbarController m_toolbarController;
    private readonly MessagesCenterController m_messagesCenterController;
    private readonly IUnityInputMgr m_inputManager;
    private readonly IInputScheduler m_inputScheduler;
    private readonly UiBuilder m_builder;
    private bool m_doNotSendCommand;
    private Option<MessageProto> m_planningModeTutorial;
    private readonly Proto m_lockedByProto;

    public bool IsVisible { get; private set; }

    public bool DeactivateShortcutsIfNotVisible => true;

    public ControllerConfig Config => ControllerConfig.Mode;

    public event Action<IToolbarItemController> VisibilityChanged;

    public PlanningModeInputController(
      ProtosDb protosDb,
      UnlockedProtosDbForUi unlockedProtosDb,
      ToolbarController toolbarController,
      PlanningModeManager planningModeManager,
      MessagesCenterController messagesCenterController,
      IUnityInputMgr inputManager,
      IInputScheduler inputScheduler,
      IGameLoopEvents gameLoopEvents,
      UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      PlanningModeInputController controller = this;
      this.m_unlockedProtosDb = unlockedProtosDb;
      this.m_toolbarController = toolbarController;
      this.m_messagesCenterController = messagesCenterController;
      this.m_inputManager = inputManager;
      this.m_inputScheduler = inputScheduler;
      this.m_builder = builder;
      this.m_planningModeTutorial = protosDb.Get<MessageProto>(IdsCore.Messages.PlanningModeTutorial);
      this.m_lockedByProto = protosDb.GetOrThrow<Proto>(IdsCore.Technology.PlanningTool);
      gameLoopEvents.RegisterRendererInitState((object) this, (Action) (() =>
      {
        controller.m_doNotSendCommand = true;
        if (planningModeManager.IsPlanningModeEnabled)
          inputManager.ActivateNewController((IUnityInputController) controller);
        controller.m_doNotSendCommand = false;
      }));
    }

    public void RegisterIntoToolbar(ToolbarController controller)
    {
      this.IsVisible = this.m_unlockedProtosDb.IsUnlocked((IProto) this.m_lockedByProto);
      if (!this.IsVisible)
        this.m_unlockedProtosDb.OnUnlockedSetChangedForUi += new Action(this.updateIsVisible);
      ToolbarController toolbarController = controller;
      string translatedString = TrCore.PlanningMode.TranslatedString;
      BtnStyle? nullable1 = new BtnStyle?(this.m_builder.Style.Toolbar.ButtonOn.ExtendText(new ColorRgba?((ColorRgba) 10725375)));
      BtnStyle? nullable2;
      ref BtnStyle? local1 = ref nullable2;
      BtnStyle buttonOff = this.m_builder.Style.Toolbar.ButtonOff;
      ref BtnStyle local2 = ref buttonOff;
      ColorRgba? nullable3 = new ColorRgba?((ColorRgba) 10725375);
      TextStyle? text = new TextStyle?();
      BorderStyle? border = new BorderStyle?();
      ColorRgba? backgroundClr = new ColorRgba?();
      ColorRgba? normalMaskClr = new ColorRgba?();
      ColorRgba? hoveredClr = nullable3;
      ColorRgba? pressedClr = new ColorRgba?();
      ColorRgba? disabledMaskClr = new ColorRgba?();
      ColorRgba? foregroundClrWhenDisabled = new ColorRgba?();
      ColorRgba? backgroundClrWhenDisabled = new ColorRgba?();
      bool? shadow = new bool?();
      int? width = new int?();
      int? height = new int?();
      int? sidePaddings = new int?();
      Offset? iconPadding = new Offset?();
      BtnStyle btnStyle = local2.Extend(text, border, backgroundClr, normalMaskClr, hoveredClr, pressedClr, disabledMaskClr, foregroundClrWhenDisabled, backgroundClrWhenDisabled, shadow, width, height, sidePaddings, iconPadding);
      local1 = new BtnStyle?(btnStyle);
      Proto.ID planningModeTutorial = IdsCore.Messages.PlanningModeTutorial;
      BtnStyle? styleWhenOn = nullable1;
      BtnStyle? styleWhenOff = nullable2;
      toolbarController.AddLeftMenuButton(translatedString, (IToolbarItemController) this, "Assets/Unity/UserInterface/Toolbar/Planning.svg", 90f, (Func<ShortcutsManager, KeyBindings>) (m => m.TogglePlanningMode), planningModeTutorial, styleWhenOn, styleWhenOff);
    }

    private void updateIsVisible()
    {
      bool flag = this.m_unlockedProtosDb.IsUnlocked((IProto) this.m_lockedByProto);
      if (this.IsVisible == flag)
        return;
      this.IsVisible = flag;
      Action<IToolbarItemController> visibilityChanged = this.VisibilityChanged;
      if (visibilityChanged == null)
        return;
      visibilityChanged((IToolbarItemController) this);
    }

    public void Activate()
    {
      if (this.m_doNotSendCommand)
        return;
      if (this.m_planningModeTutorial.HasValue)
      {
        this.m_messagesCenterController.ForceOpenMessageIfNotRead(this.m_planningModeTutorial.Value);
        this.m_planningModeTutorial = Option<MessageProto>.None;
      }
      this.m_inputScheduler.ScheduleInputCmd<SetPlanningModeEnabledCmd>(new SetPlanningModeEnabledCmd(true));
    }

    public void Deactivate()
    {
      if (this.m_doNotSendCommand)
        return;
      this.m_inputScheduler.ScheduleInputCmd<SetPlanningModeEnabledCmd>(new SetPlanningModeEnabledCmd(false));
    }

    public void DeactivateSelf()
    {
      this.m_inputManager.DeactivateController((IUnityInputController) this);
      this.m_inputScheduler.ScheduleInputCmd<SetPlanningModeEnabledCmd>(new SetPlanningModeEnabledCmd(false));
    }

    public bool InputUpdate(IInputScheduler inputScheduler) => false;
  }
}
