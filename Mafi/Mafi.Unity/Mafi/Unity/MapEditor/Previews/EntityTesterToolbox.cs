// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MapEditor.Previews.EntityTesterToolbox
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.InputControl;
using Mafi.Unity.InputControl.Toolbar;
using Mafi.Unity.UserInterface;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MapEditor.Previews
{
  public class EntityTesterToolbox : Mafi.Unity.InputControl.Toolbar.Toolbox
  {
    private Option<Action> m_onRotate;
    private Option<Action> m_onFlip;

    public EntityTesterToolbox(ToolbarController toolbar, UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(toolbar, builder);
    }

    public void SetOnRotate(Action action)
    {
      Assert.That<Option<Action>>(this.m_onRotate).IsNone<Action>();
      this.m_onRotate = (Option<Action>) action;
    }

    public void SetOnFlip(Action action)
    {
      Assert.That<Option<Action>>(this.m_onFlip).IsNone<Action>();
      this.m_onFlip = (Option<Action>) action;
    }

    protected override void BuildCustomItems(UiBuilder builder)
    {
      this.AddButton("Rotate", "Assets/Unity/UserInterface/General/Rotate128.png", (Action) (() =>
      {
        if (!this.m_onRotate.HasValue)
          return;
        this.m_onRotate.Value();
      }), (Func<ShortcutsManager, KeyBindings>) (m => m.Rotate), (LocStrFormatted) Tr.RotateShortcut__Tooltip);
      this.AddButton("Flip", "Assets/Unity/UserInterface/General/Flip128.png", (Action) (() =>
      {
        if (!this.m_onFlip.HasValue)
          return;
        this.m_onFlip.Value();
      }), (Func<ShortcutsManager, KeyBindings>) (m => m.Flip), (LocStrFormatted) Tr.FlipShortcut__Tooltip);
      this.AddToToolbar();
    }
  }
}
