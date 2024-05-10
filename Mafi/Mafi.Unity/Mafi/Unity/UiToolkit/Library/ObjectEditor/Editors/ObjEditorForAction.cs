// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.ObjectEditor.Editors.ObjEditorForAction
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.UiToolkit.Component;
using System;
using System.Reflection;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library.ObjectEditor.Editors
{
  internal class ObjEditorForAction : Row, IObjEditor
  {
    private readonly ObjEditor m_editor;
    private readonly Label m_label;
    private readonly ButtonText m_btn;
    private readonly IconButton m_iconBtn;
    private readonly Row m_btnsRow;
    private bool m_usesIcon;
    private Action m_actionToCall;
    private bool m_notifyActionCall;

    public UiComponent Component => (UiComponent) this;

    public ObjEditorForAction(ObjEditor editor)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_editor = editor;
      this.JustifyItemsEnd<ObjEditorForAction>();
      UiComponent[] uiComponentArray = new UiComponent[2]
      {
        (UiComponent) (this.m_label = new Label().Class<Label>(Cls.inputLabel).FlexGrow<Label>(1f)),
        null
      };
      Row component = new Row();
      component.Add<Row>((Action<Row>) (c => c.Wrap<Row>()));
      component.Add((UiComponent) (this.m_btn = new ButtonText(LocStrFormatted.Empty, new Action(this.onClick)).MarginTopBottom<ButtonText>(1.pt()).MarginRight<ButtonText>(2.pt()).PaddingLeftRight<ButtonText>(3.pt()).PaddingTopBottom<ButtonText>(6.px())));
      component.Add((UiComponent) (this.m_iconBtn = new IconButton(onClick: new Action(this.onClick)).MarginTopBottom<IconButton>(1.pt()).MarginRight<IconButton>(2.pt())).IconSize(18.px(), 18.px()));
      Row row = component;
      this.m_btnsRow = component;
      uiComponentArray[1] = (UiComponent) row;
      this.Add(uiComponentArray);
    }

    public void SetData(MemberInfo member, Action action, int nestingLevel)
    {
      EditorLabelAttribute customAttribute1 = member.GetCustomAttribute<EditorLabelAttribute>();
      EditorButtonAttribute customAttribute2 = member.GetCustomAttribute<EditorButtonAttribute>();
      this.setAction(action, true);
      this.setLabel(customAttribute1?.Label ?? "", customAttribute1?.Tooltip, nestingLevel);
      string buttonTitle = customAttribute2?.ButtonText.ValueOrNull ?? (member.Name.StartsWith("m_") ? member.Name.SubstringSafe(2) : member.Name).CamelCaseToSpacedSentenceCase();
      string tooltip;
      string valueOrNull = this.iconToAsset(customAttribute2 != null ? customAttribute2.Icon : ObjEditorIcon.None, out tooltip).ValueOrNull;
      this.setBtn(buttonTitle, customAttribute2?.ButtonTooltip ?? tooltip, valueOrNull, customAttribute2 != null && customAttribute2.IsPrimary);
    }

    public void SetData(
      ObjEditorsRegistry.RegisteredAction registeredAction,
      object objToEdit,
      Action<object> setter)
    {
      this.setAction((Action) (() =>
      {
        setter(registeredAction.ProcessingAction(objToEdit));
        this.m_editor.ReportValueChanged(rebuildUi: true);
      }), true);
      string tooltip;
      string valueOrNull = this.iconToAsset(registeredAction.Icon, out tooltip).ValueOrNull;
      this.setBtn(registeredAction.Name, registeredAction.Tooltip.ValueOrNull ?? tooltip, valueOrNull, false);
      this.setLabel((string) null, (string) null, 0);
    }

    public void SetData(
      ObjEditorsRegistry.RegisteredCustomEditor customEditor,
      ObjEditorData data)
    {
      this.setAction(new Action(this.m_editor.CreateCustomEditor(customEditor.EditorFactory, data.Value).ButtonPressed), false);
      this.setBtn(customEditor.Name, customEditor.Tooltip, "", false);
      this.setLabel(data.Label, data.Tooltip, data.NestingLevel);
    }

    public void SetData(ObjEditorData data) => Log.Error("Not implemented");

    private void setBtn(string buttonTitle, string tooltip, string iconPath, bool isPrimary)
    {
      this.m_iconBtn.RemoveFromHierarchy();
      this.m_btn.RemoveFromHierarchy();
      this.m_usesIcon = !iconPath.IsNullOrEmpty();
      if (this.m_usesIcon)
      {
        this.m_iconBtn.SetIcon(iconPath);
        this.m_iconBtn.Tooltip<IconButton>(new LocStrFormatted?(tooltip.AsLoc()));
        this.m_btnsRow.Add((UiComponent) this.m_iconBtn);
      }
      else
      {
        this.m_btn.Text<ButtonText>(buttonTitle.AsLoc());
        this.m_btn.Tooltip<ButtonText>(new LocStrFormatted?(tooltip.AsLoc()));
        this.m_btn.Variant<ButtonText>(isPrimary ? ButtonVariant.Primary : ButtonVariant.Default);
        this.m_btnsRow.Add((UiComponent) this.m_btn);
      }
    }

    private void setLabel(string label, string tooltip, int nestingLevel)
    {
      this.m_label.Text<Label>(label.AsLoc()).Tooltip<Label>(new LocStrFormatted?(tooltip.AsLoc()));
      if (!label.IsNullOrEmpty())
        this.m_btnsRow.JustifyItemsStart<Row>();
      else
        this.m_btnsRow.JustifyItemsEnd<Row>();
      this.m_btnsRow.FlexGrow<Row>((float) (label == null ? 1 : 0));
      this.m_btnsRow.Width<Row>(!label.IsNullOrEmpty() ? new Px?(ObjEditor.GetEditorWidth(nestingLevel)) : new Px?());
    }

    public bool AppendActionIfCan(ObjEditorForAction action)
    {
      if (action.m_label.GetText().IsNotEmpty)
        return false;
      if (action.m_usesIcon)
      {
        action.m_iconBtn.RemoveFromHierarchy();
        this.m_btnsRow.Add((UiComponent) action.m_iconBtn);
      }
      else
      {
        action.m_btn.RemoveFromHierarchy();
        this.m_btnsRow.Add((UiComponent) action.m_btn);
      }
      return true;
    }

    private void setAction(Action action, bool notifyActionCall)
    {
      this.m_actionToCall = action;
      this.m_notifyActionCall = notifyActionCall;
    }

    private void onClick()
    {
      try
      {
        Action actionToCall = this.m_actionToCall;
        if (actionToCall != null)
          actionToCall();
        if (!this.m_notifyActionCall)
          return;
        this.m_editor.ReportActionCalled();
      }
      catch (Exception ex)
      {
        Log.Exception(ex, "Failed to call action");
      }
    }

    public bool TryGetValue(out object value)
    {
      value = (object) null;
      return false;
    }

    private Option<string> iconToAsset(ObjEditorIcon icon, out string tooltip)
    {
      tooltip = "";
      switch (icon)
      {
        case ObjEditorIcon.None:
          return (Option<string>) Option.None;
        case ObjEditorIcon.View:
          tooltip = "View";
          return (Option<string>) "Assets/Unity/UserInterface/General/Search.svg";
        case ObjEditorIcon.Delete:
          tooltip = "Delete";
          return (Option<string>) "Assets/Unity/UserInterface/General/Trash128.png";
        case ObjEditorIcon.Edit:
          tooltip = "Edit";
          return (Option<string>) "Assets/Unity/UserInterface/General/Rename.svg";
        case ObjEditorIcon.Clone:
          tooltip = "Clone";
          return (Option<string>) "Assets/Unity/UserInterface/General/PlaceMultiple.svg";
        case ObjEditorIcon.Randomize:
          tooltip = "Randomize";
          return (Option<string>) "Assets/Unity/UserInterface/General/Randomize.svg";
        default:
          Log.Error(string.Format("Icon {0} not supported yet?", (object) icon));
          return (Option<string>) Option.None;
      }
    }
  }
}
