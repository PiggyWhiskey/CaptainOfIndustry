// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Mine.AreaToolbox
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Prototypes;
using Mafi.Unity.InputControl;
using Mafi.Unity.InputControl.Messages;
using Mafi.Unity.InputControl.Toolbar;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Mine
{
  public class AreaToolbox : Mafi.Unity.InputControl.Toolbar.Toolbox, IAreaToolbox
  {
    private static readonly int s_areasCnt;
    private readonly MessagesCenterController m_messagesCenter;
    private readonly AreaToolbox.ToolType m_toolType;
    private Option<Action> m_onRotate;
    private Option<Action> m_onIncreaseElevation;
    private Option<Action> m_onDecreaseElevation;
    private readonly ToggleBtn[] m_buttons;
    private Btn m_rotateButton;

    public event Action<AreaMode> OnModeChanged;

    public AreaToolbox(
      ToolbarController toolbar,
      UiBuilder builder,
      MessagesCenterController messagesCenter,
      AreaToolbox.ToolType toolType)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_buttons = new ToggleBtn[AreaToolbox.s_areasCnt];
      // ISSUE: explicit constructor call
      base.\u002Ector(toolbar, builder);
      this.m_messagesCenter = messagesCenter;
      this.m_toolType = toolType;
    }

    public void SetMode(AreaMode mode)
    {
      this.BuildIfNeeded();
      foreach (ToggleBtn button in this.m_buttons)
        button.SetIsOn(false);
      this.m_buttons[(int) mode].SetIsOn(true);
      this.m_rotateButton.SetEnabled(mode == AreaMode.Ramp);
    }

    public void SetOnRotate(Action action) => this.m_onRotate = (Option<Action>) action;

    public void SetOnIncreaseElevation(Action action)
    {
      this.m_onIncreaseElevation = (Option<Action>) action;
    }

    public void SetOnDecreaseElevation(Action action)
    {
      this.m_onDecreaseElevation = (Option<Action>) action;
    }

    protected override void BuildCustomItems(UiBuilder builder)
    {
      string[] strArray = new string[AreaToolbox.s_areasCnt];
      if (this.m_toolType == AreaToolbox.ToolType.Dumping)
      {
        strArray[1] = "Assets/Unity/UserInterface/General/DumpFlat128.png";
        strArray[0] = "Assets/Unity/UserInterface/General/PlatformUp128.png";
      }
      else if (this.m_toolType == AreaToolbox.ToolType.Mining)
      {
        strArray[1] = "Assets/Unity/UserInterface/General/MineFlat128.png";
        strArray[0] = "Assets/Unity/UserInterface/General/PlatformDown128.png";
      }
      else if (this.m_toolType == AreaToolbox.ToolType.Leveling)
      {
        strArray[1] = "Assets/Unity/UserInterface/General/PlatformFlat128.png";
        strArray[0] = "Assets/Unity/UserInterface/General/PlatformUpDown128.png";
      }
      foreach (AreaMode areaMode in Enum.GetValues(typeof (AreaMode)))
      {
        AreaMode mode = areaMode;
        this.m_buttons[(int) mode] = this.AddToggleButton(mode.ToString(), strArray[(int) mode], (Action<bool>) (s =>
        {
          if (!s)
            return;
          Action<AreaMode> onModeChanged = this.OnModeChanged;
          if (onModeChanged == null)
            return;
          onModeChanged(mode);
        }), (Func<ShortcutsManager, KeyBindings>) (m => m.Flip));
      }
      this.AddButton("+1", (Action) (() =>
      {
        if (!this.m_onIncreaseElevation.HasValue)
          return;
        this.m_onIncreaseElevation.Value();
      }), (Func<ShortcutsManager, KeyBindings>) (m => m.RaiseUp));
      this.AddButton("-1", (Action) (() =>
      {
        if (!this.m_onDecreaseElevation.HasValue)
          return;
        this.m_onDecreaseElevation.Value();
      }), (Func<ShortcutsManager, KeyBindings>) (m => m.LowerDown));
      this.m_rotateButton = this.AddButton("Rotate", "Assets/Unity/UserInterface/General/Rotate128.png", (Action) (() =>
      {
        if (!this.m_onRotate.HasValue)
          return;
        this.m_onRotate.Value();
      }), (Func<ShortcutsManager, KeyBindings>) (m => m.Rotate));
      Btn btn = this.AddButton("Clear", "Assets/Unity/UserInterface/General/Clear128.png", (Action) (() => { }), (Func<ShortcutsManager, KeyBindings>) (m => m.SecondaryAction));
      btn.AddToolTip(Tr.DesignationRemovalTooltip);
      btn.SetEnabled(false);
      Proto.ID tutorialId = this.m_toolType == AreaToolbox.ToolType.Mining ? IdsCore.Messages.TutorialOnMineTower : IdsCore.Messages.TutorialOnDumping;
      this.AddButton("Help", "Assets/Unity/UserInterface/General/Info128.png", (Action) (() => this.m_messagesCenter.ShowMessage(tutorialId)), (Func<ShortcutsManager, KeyBindings>) null).AddToolTip(Tr.OpenTutorial);
      this.AddToToolbar();
    }

    static AreaToolbox()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      AreaToolbox.s_areasCnt = Enum.GetValues(typeof (AreaMode)).Length;
    }

    public enum ToolType
    {
      Mining,
      Dumping,
      Leveling,
    }
  }
}
