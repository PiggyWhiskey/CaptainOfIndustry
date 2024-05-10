// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.ObjectEditor.Editors.ObjEditorDisplay
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UserInterface;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library.ObjectEditor.Editors
{
  internal class ObjEditorDisplay : Row
  {
    private readonly Label m_label;
    private readonly Label m_text;
    private Option<Icon> m_errorIcon;

    public ObjEditorDisplay()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.AlignItemsStart<ObjEditorDisplay>().JustifyItemsSpaceBetween<ObjEditorDisplay>().Gap<ObjEditorDisplay>(new Px?(Theme.InputLabelGap));
      this.Add((UiComponent) (this.m_label = new Label().Class<Label>(Cls.inputLabel)), (UiComponent) (this.m_text = new Label().Class<Label>(Cls.inputLabel).PaddingRight<Label>(0.px()).FlexNoShrink<Label>()));
    }

    public void SetData(
      string label,
      string text,
      string tooltip,
      bool isError,
      int nestingLevel)
    {
      if (label.IsNullOrEmpty())
      {
        this.m_label.Text<Label>(text.AsLoc());
        this.m_label.PaddingLeft<Label>(isError ? 4.pt() : 1.pt());
      }
      else
      {
        this.m_label.Text<Label>(label.AsLoc());
        this.m_label.PaddingLeft<Label>(isError ? 4.pt() : 0.pt());
        this.m_text.Text<Label>(text.AsLoc());
        this.m_text.Width<Label>(new Px?(ObjEditor.GetEditorWidth(nestingLevel)));
      }
      if (isError && this.m_errorIcon.IsNone)
      {
        this.m_errorIcon = (Option<Icon>) new Icon().Class<Icon>(Cls.errorIcon, "attachedIcon").Size<Icon>(16.px());
        this.InsertAt(0, (UiComponent) this.m_errorIcon.Value);
        Icon component = this.m_errorIcon.Value;
        Px? nullable = new Px?(0.px());
        Px? top = new Px?(6.px());
        Px? right = new Px?();
        Px? bottom = new Px?();
        Px? left = nullable;
        component.AbsolutePosition<Icon>(top, right, bottom, left);
        this.RunWithBuilder((Action<UiBuilder>) (builder => this.m_errorIcon.Value.Build(builder)));
      }
      this.m_label.Color<Label>(isError ? new ColorRgba?(Theme.WarningText) : new ColorRgba?());
      this.m_text.Color<Label>(isError ? new ColorRgba?(Theme.WarningText) : new ColorRgba?());
      this.m_errorIcon.ValueOrNull?.SetVisible(isError);
      this.m_text.Visible<Label>(!label.IsNullOrEmpty());
      this.m_label.Tooltip<Label>(new LocStrFormatted?(tooltip.AsLoc()));
    }
  }
}
