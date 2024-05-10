// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Mine.ForestryAreaToolbox
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

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
  public class ForestryAreaToolbox : Mafi.Unity.InputControl.Toolbar.Toolbox, IAreaToolbox
  {
    private readonly MessagesCenterController m_messagesCenter;

    public event Action<AreaMode> OnModeChanged;

    public ForestryAreaToolbox(
      ToolbarController toolbar,
      UiBuilder builder,
      MessagesCenterController messagesCenter)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(toolbar, builder);
      this.m_messagesCenter = messagesCenter;
    }

    public void SetMode(AreaMode mode)
    {
    }

    public void SetOnRotate(Action action)
    {
    }

    public void SetOnIncreaseElevation(Action action)
    {
    }

    public void SetOnDecreaseElevation(Action action)
    {
    }

    protected override void BuildCustomItems(UiBuilder builder)
    {
      Btn btn = this.AddButton("Clear", "Assets/Unity/UserInterface/General/Clear128.png", (Action) (() => { }), (Func<ShortcutsManager, KeyBindings>) (m => m.SecondaryAction));
      btn.AddToolTip(Tr.DesignationRemovalTooltip);
      btn.SetEnabled(false);
      this.AddToToolbar();
    }
  }
}
