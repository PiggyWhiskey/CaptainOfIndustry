// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.ObjectEditor.ObjEditorsDock
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Input;
using Mafi.Localization;
using Mafi.Unity.UiToolkit.Component;
using System;
using UnityEngine.UIElements;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library.ObjectEditor
{
  public class ObjEditorsDock : TabContainer, IUiComponent
  {
    /// <summary>
    /// Note: Fixed width is much better than dynamic one. First of all tab container is not always able
    /// to guarantee to measure the right max width of all children. And also collapsing larger parts of
    /// editors ends up in the entire width jumping back and forth.
    /// </summary>
    private static readonly Px WIDTH;
    private readonly bool m_isLeft;
    private bool m_isMinimized;

    public ObjEditor ActiveEditor => this.ActiveTab.ValueOrNull as ObjEditor;

    public ObjEditorsDock(bool isLeft)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(true);
      this.TransformOrigin<ObjEditorsDock>(0.Percent(), 0.Percent()).Width<ObjEditorsDock>(new Px?(ObjEditorsDock.WIDTH)).FlexGrow<ObjEditorsDock>(0.0f).FlexShrink<ObjEditorsDock>(1f).Hide<ObjEditorsDock>().RelativePosition<ObjEditorsDock>().Padding<ObjEditorsDock>(2.pt());
      this.InnerBody.Padding<UiComponent>(4.px(), 0.px(), 8.px(), 0.px());
      this.m_isLeft = isLeft;
      this.m_panel.SetBoltsVisible(false);
      this.Class<ObjEditorsDock>(Cls.windowBorder);
      this.InsertAt(0, (UiComponent) new WindowBackground());
      this.ControlsBar.Add((UiComponent) new IconClickable("Assets/Unity/UserInterface/General/Rotate128.png", new Action(this.refreshClicked)).Tooltip<IconClickable>(new LocStrFormatted?("Reload the editor with last data".AsLoc())).GrowOnHover(), (UiComponent) new IconClickable("Assets/Unity/UserInterface/General/Minimize.svg", new Action(this.toggleMinimized)).Tooltip<IconClickable>(new LocStrFormatted?("Minimize".AsLoc())).GrowOnHover(), (UiComponent) new IconClickable("Assets/Unity/UserInterface/General/Close.svg", new Action(this.close)).Tooltip<IconClickable>(new LocStrFormatted?("Close".AsLoc())).GrowOnHover());
      if (isLeft)
      {
        this.Header.ReversedDirection<Mafi.Unity.UiToolkit.Library.Row>();
        this.ControlsBar.ReverseChildren<Mafi.Unity.UiToolkit.Library.Row>();
        this.TabBar.JustifyItemsEnd<TabBar>();
      }
      this.TabBar.EnableDropdownMode(true);
      this.TransformOrigin<ObjEditorsDock>(0.Percent(), 0.Percent());
      this.Hide<ObjEditorsDock>();
    }

    public void AddDockingReference(ObjEditorsDock dock, string iconPath, LocStrFormatted tooltip)
    {
      this.ControlsBar.InsertAt(this.m_isLeft ? this.ControlsBar.Count : 0, (UiComponent) new IconClickable(iconPath, (Action) (() => this.onDockClick(dock))).Tooltip<IconClickable>(new LocStrFormatted?(tooltip)).GrowOnHover());
    }

    private void toggleMinimized()
    {
      this.m_isMinimized = !this.m_isMinimized;
      if (this.m_isMinimized)
      {
        this.Rotate<ObjEditorsDock>(new int?(90));
        if (this.m_isLeft)
        {
          this.Scale<ObjEditorsDock>(-1f, -1f);
          this.RootElement.style.translate = new StyleTranslate(new Translate((Length) 0.0f, (Length) ObjEditorsDock.WIDTH.Pixels));
        }
        else
          this.RootElement.style.translate = new StyleTranslate(new Translate((Length) ObjEditorsDock.WIDTH.Pixels, (Length) 0.0f));
        this.unselectCurrentTab();
      }
      else
      {
        this.Rotate<ObjEditorsDock>(new int?()).Scale<ObjEditorsDock>();
        this.RootElement.style.translate = new StyleTranslate(new Translate((Length) 0.0f, (Length) 0.0f));
      }
      this.ControlsBar.SetVisible(!this.m_isMinimized);
      this.Body.SetVisible(!this.m_isMinimized);
    }

    private void close() => this.ActiveEditor?.Close();

    private void refreshClicked() => this.ActiveEditor?.Refresh();

    private void onDockClick(ObjEditorsDock targetDock)
    {
      this.ActiveEditor?.DockSelfTo(targetDock);
    }

    protected override void OnTabSwitch()
    {
      if (this.m_isMinimized)
        this.toggleMinimized();
      this.Show<ObjEditorsDock>();
    }

    protected override void OnTabRemoved() => this.Visible<ObjEditorsDock>(this.TabsCount > 0);

    public void RenderUpdate(GameTime time) => this.ActiveEditor?.RenderUpdate(time);

    public bool InputUpdate(IInputScheduler inputScheduler)
    {
      ObjEditor activeEditor = this.ActiveEditor;
      return activeEditor != null && activeEditor.InputUpdate();
    }

    void IUiComponent.SetEnabled(bool enabled) => this.m_panel.Body.Enabled<Mafi.Unity.UiToolkit.Library.Column>(enabled);

    static ObjEditorsDock()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      ObjEditorsDock.WIDTH = 480.px();
    }
  }
}
