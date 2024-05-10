// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.ObjectEditor.ObjEditorLabel
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.UiToolkit.Component;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library.ObjectEditor
{
  internal class ObjEditorLabel : ButtonRow
  {
    private readonly Label m_txt;
    private readonly Icon m_collapseIcon;
    private readonly Action<bool> m_onCollapseRequested;
    private readonly Action<bool> m_onCollapseClick;

    public bool CanBeCollapsed { get; private set; }

    public bool IsCollapsed { get; private set; }

    public ObjEditorLabel(Action<bool> onCollapseRequested, Action<bool> onCollapseClick)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_onCollapseRequested = onCollapseRequested;
      this.m_onCollapseClick = onCollapseClick;
      this.Class<ObjEditorLabel>(Cls.editorLabel).FlexGrow<ObjEditorLabel>(1f);
      this.Add((UiComponent) (this.m_collapseIcon = new Icon("Assets/Unity/UserInterface/General/ArrowDown.svg").Size<Icon>(12.px()).MarginRight<Icon>(1.pt())).Hide<Icon>(), (UiComponent) (this.m_txt = new Label().Class<Label>(Cls.inputLabel)));
      this.OnClick<ObjEditorLabel>(new Action(this.onClick));
      this.setCollapsible(true);
    }

    public ObjEditorLabel SetCollapseDisabled(bool isDisabled)
    {
      this.setCollapsible(!isDisabled);
      return this;
    }

    private ObjEditorLabel setCollapsible(bool isCollapsible)
    {
      this.CanBeCollapsed = isCollapsible;
      if (!isCollapsible && this.IsCollapsed)
        this.setCollapsed(false);
      this.ClassIff<ObjEditorLabel>(Cls.collapsible, isCollapsible);
      this.m_collapseIcon.Visible<Icon>(isCollapsible);
      return this;
    }

    public void SetText(string text, string tooltip, bool isHeader, int nestingLevel)
    {
      this.m_txt.Text<Label>(text.AsLoc()).Tooltip<Label>(new LocStrFormatted?(tooltip.AsLoc())).UpperCase(isHeader);
      this.ClassIff<ObjEditorLabel>(Cls.header, isHeader);
      this.ClassIff<ObjEditorLabel>(Cls.editorLabel0, nestingLevel % 2 == 0);
      this.ClassIff<ObjEditorLabel>(Cls.editorLabel1, nestingLevel % 2 != 0);
    }

    public void SetCollapsed(bool collapse)
    {
      if (collapse && !this.CanBeCollapsed)
        return;
      this.setCollapsed(collapse);
      Action<bool> collapseRequested = this.m_onCollapseRequested;
      if (collapseRequested == null)
        return;
      collapseRequested(this.IsCollapsed);
    }

    private void onClick()
    {
      if (!this.IsCollapsed && !this.CanBeCollapsed)
        return;
      this.setCollapsed(!this.IsCollapsed);
      Action<bool> onCollapseClick = this.m_onCollapseClick;
      if (onCollapseClick == null)
        return;
      onCollapseClick(this.IsCollapsed);
    }

    private void setCollapsed(bool isCollapsed)
    {
      if (this.IsCollapsed == isCollapsed)
        return;
      this.IsCollapsed = isCollapsed;
      this.m_collapseIcon.Rotate<Icon>(this.IsCollapsed ? new int?(-90) : new int?());
      this.MarginLeft<ObjEditorLabel>(isCollapsed ? -1.pt() : 0.pt());
      this.m_collapseIcon.MarginLeft<Icon>(isCollapsed ? 1.pt() : 0.pt());
      this.ClassIff<ObjEditorLabel>(Cls.collapsed, isCollapsed);
    }
  }
}
