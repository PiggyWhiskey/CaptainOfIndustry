// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.ObjectEditor.Editors.ObjEditorForString`1
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Localization;
using Mafi.Unity.UiToolkit.Component;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine.UIElements;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library.ObjectEditor.Editors
{
  internal abstract class ObjEditorForString<T> : 
    TxtField,
    IObjEditorWithValueUpdate,
    IObjEditor,
    IEditorWithValidationSource,
    IEditorWithRenderUpdate
  {
    private readonly ObjEditor m_editor;
    private string m_lastValueSeen;
    private readonly Icon m_pendingEditIcon;
    private readonly Mafi.Unity.UiToolkit.Library.Toggle m_optionToggle;
    private LystStruct<IEditorValidationAttribute> m_validators;
    private Option<Func<string>> m_validationSource;

    public UiComponent Component => (UiComponent) this;

    protected abstract string ExpectedFormatMsg { get; }

    protected ObjEditorData Data { get; private set; }

    protected bool IsOptional => this.m_optionToggle.HasParent;

    protected ObjEditorForString(ObjEditor editor)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_editor = editor;
      this.m_pendingEditIcon = new Icon().Class<Icon>(Cls.pendingEditIcon);
      this.OnValueChanged(new Action<string>(this.onFieldValueChanged));
      this.OnEditEnd(new Action<string>(this.OnEditEnd));
      this.ForFieldAttachIcon(this.m_pendingEditIcon);
      this.OnRightClick(new Action(this.onRightClick));
      Mafi.Unity.UiToolkit.Library.Toggle component = new Mafi.Unity.UiToolkit.Library.Toggle(true).Label<Mafi.Unity.UiToolkit.Library.Toggle>("Set value".AsLoc()).ReversedDirection<Mafi.Unity.UiToolkit.Library.Toggle>().OnValueChanged(new Action<bool>(this.onOptionToggle));
      Px? nullable1 = new Px?(0.px());
      Px? nullable2 = new Px?(2.px());
      Px? top = new Px?();
      Px? right = nullable1;
      Px? bottom = nullable2;
      Px? left = new Px?();
      this.m_optionToggle = component.AbsolutePosition<Mafi.Unity.UiToolkit.Library.Toggle>(top, right, bottom, left);
    }

    private void onFieldValueChanged(string newVal)
    {
      this.tryParseAndValidate(out T _, this.m_lastValueSeen == newVal);
    }

    private void OnEditEnd(string newVal)
    {
      if (this.m_lastValueSeen == newVal)
        return;
      this.m_lastValueSeen = newVal;
      if (!this.tryParseAndValidate(out T _))
        return;
      this.m_editor.ReportValueChanged();
    }

    public bool TryGetValue(out object value)
    {
      if (this.m_optionToggle.HasParent && !this.m_optionToggle.GetValue())
      {
        System.Type underlyingType = Nullable.GetUnderlyingType(this.Data.Type);
        value = underlyingType != (System.Type) null ? (object) null : ObjEditor.ObjToOption(this.Data.Value.GetType(), (object) null);
        return true;
      }
      T result;
      if (this.tryParseAndValidate(out result, true))
      {
        value = (object) result;
        this.m_lastValueSeen = this.ToString(result);
        this.Text(this.m_lastValueSeen.AsLoc());
        if (this.Data.Value is IOptionNonGeneric)
          value = ObjEditor.ObjToOption(this.Data.Value.GetType(), value);
        return true;
      }
      value = (object) null;
      return false;
    }

    public virtual void SetData(ObjEditorData data)
    {
      this.Data = data;
      this.m_validationSource = (Option<Func<string>>) Option.None;
      this.Enabled<ObjEditorForString<T>>(true).Label<ObjEditorForString<T>>(data.Label.AsLoc()).Tooltip<ObjEditorForString<T>>(new LocStrFormatted?(data.Tooltip.AsLoc())).ForFieldSetWidth(new Px?(ObjEditor.GetEditorWidth(data)));
      this.m_validators.Clear();
      if (data.Member.HasValue)
        getValidatorsFrom(data.Member.Value);
      if (data.PassedThroughParentMember.HasValue)
        getValidatorsFrom(data.PassedThroughParentMember.Value);
      this.UpdateValue(data.Value);
      TextElement textElement = this.RootElement.Q<TextElement>();
      if ((double) textElement.resolvedStyle.translate.x >= 0.0)
        return;
      Log.Error(string.Format("Found negative translate {0}", (object) textElement.resolvedStyle.translate.x));

      void getValidatorsFrom(MemberInfo member)
      {
        this.m_validators.AddRange(member.GetCustomAttributes().Where<Attribute>((Func<Attribute, bool>) (x => x is IEditorValidationAttribute)).Cast<IEditorValidationAttribute>());
      }
    }

    public void UpdateValue(object value)
    {
      System.Type underlyingType = Nullable.GetUnderlyingType(this.Data.Type);
      T result;
      if (value is IOptionNonGeneric optionNonGeneric)
      {
        this.updateOptionalUi(true, optionNonGeneric.HasValue);
        object obj;
        if (!optionNonGeneric.HasValue)
        {
          result = default (T);
          obj = (object) result;
        }
        else
          obj = optionNonGeneric.Value;
        value = obj;
      }
      else if (underlyingType != (System.Type) null)
        this.updateOptionalUi(true, value != null);
      else
        this.updateOptionalUi(false);
      this.m_lastValueSeen = value == null ? string.Empty : this.ToString((T) value);
      this.Text(this.m_lastValueSeen.AsLoc());
      this.tryParseAndValidate(out result, true);
      this.OnValueSet(value);
    }

    protected virtual void OnValueSet(object value)
    {
    }

    private bool tryParseAndValidate(out T result, bool valueSaved = false)
    {
      string text = this.GetText();
      if (text.IsEmpty() && !typeof (T).IsAssignableTo<string>())
        result = default (T);
      else if (!this.TryParse(text, out result))
      {
        this.updateStatus(("Incorrect value: " + this.ExpectedFormatMsg).AsLoc(), valueSaved);
        return false;
      }
      string str1 = "";
      foreach (IEditorValidationAttribute validator in this.m_validators)
      {
        string str2 = validator.ValidateObj((object) result);
        if (str2.IsNotEmpty())
          str1 = str1 + "\n - " + str2;
      }
      if (str1.IsNotEmpty())
      {
        this.updateStatus(("Validation failed:" + str1).AsLoc(), valueSaved);
        return false;
      }
      this.updateStatus(LocStrFormatted.Empty, valueSaved);
      return true;
    }

    private void updateStatus(LocStrFormatted errorMessage, bool valueSaved)
    {
      bool isNotEmpty = errorMessage.IsNotEmpty;
      if (errorMessage.IsNotEmpty && !valueSaved)
        errorMessage = string.Format("{0}\n\n<b>Right click to reset</b>", (object) errorMessage).AsLoc();
      this.MarkAsError(isNotEmpty, errorMessage);
      this.m_pendingEditIcon.SetVisible(!isNotEmpty && !valueSaved);
      if (!this.m_pendingEditIcon.IsVisible())
        return;
      this.m_pendingEditIcon.Tooltip<Icon>(new LocStrFormatted?((this.IsMultiline ? "Not saved yet (Ctrl + Enter to save)." : "Not saved yet").AsLoc()));
    }

    private void onOptionToggle(bool hasValue)
    {
      this.updateOptionalUi(true, hasValue);
      if (hasValue)
        return;
      this.m_editor.ReportValueChanged();
      this.Text(LocStrFormatted.Empty);
    }

    private void updateOptionalUi(bool isOptional, bool hasValue = false)
    {
      if (!isOptional)
      {
        this.m_optionToggle.RemoveFromHierarchy();
        this.PaddingBottom<ObjEditorForString<T>>(0.px());
        this.ForFieldSetEnabled(true);
      }
      else
      {
        if (!this.m_optionToggle.HasParent)
        {
          this.Add((UiComponent) this.m_optionToggle);
          this.PaddingBottom<ObjEditorForString<T>>(30.px());
        }
        this.m_optionToggle.Value(hasValue);
        this.ForFieldSetEnabled(hasValue);
      }
    }

    private void onRightClick()
    {
      if (!this.Data.Member.HasValue || !this.Data.Owner.HasValue)
        return;
      this.UpdateValue(ObjEditorUtils.GetValue(this.Data.Member.Value, this.Data.Owner.Value));
    }

    protected abstract string ToString(T value);

    protected abstract bool TryParse(string input, out T result);

    public void SetValidationSource(Func<string> source)
    {
      this.m_validationSource = (Option<Func<string>>) source;
      this.updateFromValidationSource();
    }

    private void updateFromValidationSource()
    {
      if (this.m_validationSource.IsNone || this.m_pendingEditIcon.IsVisible())
        return;
      string str = this.m_validationSource.Value();
      this.updateStatus(str.IsNullOrEmpty() ? LocStrFormatted.Empty : str.AsLoc(), true);
    }

    void IEditorWithRenderUpdate.RenderUpdate() => this.updateFromValidationSource();
  }
}
