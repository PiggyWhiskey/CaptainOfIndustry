// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Trees.TreeHarvestingToolbox
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Localization;
using Mafi.Unity.InputControl;
using Mafi.Unity.InputControl.Messages;
using Mafi.Unity.InputControl.Toolbar;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Trees
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  internal class TreeHarvestingToolbox : Mafi.Unity.InputControl.Toolbar.Toolbox
  {
    private readonly MessagesCenterController m_messagesCenter;

    public ToggleBtn PrimaryActionBtn { get; private set; }

    public ToggleBtn SecondaryActionBtn { get; private set; }

    public TreeHarvestingToolbox(
      ToolbarController toolbar,
      UiBuilder builder,
      MessagesCenterController messagesCenter)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(toolbar, builder);
      this.m_messagesCenter = messagesCenter;
    }

    protected override void BuildCustomItems(UiBuilder builder)
    {
      this.PrimaryActionBtn = this.AddToggleButton("Add tree", "Assets/Unity/UserInterface/Toolbar/TreeHarvesting.svg", (Action<bool>) (_ => { }), (Func<ShortcutsManager, KeyBindings>) (m => m.PrimaryAction));
      this.SecondaryActionBtn = this.AddToggleButton("Remove tree", "Assets/Unity/UserInterface/General/Clear128.png", (Action<bool>) (_ => { }), (Func<ShortcutsManager, KeyBindings>) (m => m.SecondaryAction), (LocStrFormatted) Tr.DesignationRemovalTooltip);
      this.AddButton("Help", "Assets/Unity/UserInterface/General/Info128.png", (Action) (() => this.m_messagesCenter.ShowMessage(IdsCore.Messages.TutorialTreeHarvesting)), (Func<ShortcutsManager, KeyBindings>) null).AddToolTip(Tr.OpenTutorial);
      this.AddToToolbar();
    }
  }
}
